using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using SKinEditer.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SKinEditer
{
    public partial class ColorCheck : Form
    {

        private string selectProject;

        private string rootProject;

        private List<NewColor> newColor = new List<NewColor>();

        private List<DiffColor> diffColor = new List<DiffColor>();

        //內容從白名單來的，要在每個skin的colors.xml後面加上的，因為有些SVG 或 drawable會用到
        private Dictionary<string, string> defaultColor=new Dictionary<string, string>();

        private HashSet<string> white ;

        private Step curStep = Step.CheckWhite;

        private List<string> skinProject;

        private Dictionary<string, Dictionary<string, string>> main;

        private Dictionary<string, Dictionary<string, string>> local;

        private Dictionary<string, Dictionary<string, string>> remote;

        private Form parent;

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public static System.Drawing.Color Green { get; internal set; }

        public ColorCheck(Form parent, string selectProject, string rootProject)
        {
            InitializeComponent();

            //Properties.Settings.Default.whiteColor = "";
            //Properties.Settings.Default.Save();

            initWhite();
            initTime();
            this.parent = parent;
            this.selectProject = selectProject;
            this.rootProject = rootProject;
        }

        private void initTime()
        {
            var value = Properties.Settings.Default.updateTime;
            if (value == null || value.Length == 0)
            {
                lb_time.Text = "";
            }
            else {
                lb_time.Text = $"上次更新時間:{value}";
            }
        }

        private void initWhite()
        {
            var value = Properties.Settings.Default.whiteColor;
            if (value == null || value.Length==0)
            {
                white = new HashSet<string>();
            }
            else {
                white = JsonSerializer.Deserialize<HashSet<string>>(value);
            }
        }

        class DiffColor { 

        public string file { get; set; } 
        
        public string module { get; set; }

        public string key { get; set; }
        public string oldValue { get; set; }

        public string newValue { get; set; }
        public bool isDefault { get; set; }

            public DiffColor(string file, string key, string oldValue, string newValue, string module, bool isDefault)
            {
                this.file = file;
                this.key = key;
                this.oldValue = oldValue;
                this.newValue = newValue;
                this.isDefault = isDefault;
                this.module = module;
            }
        }

        class NewColor
        {
            public string file { get; set; }
            public string key { get; set; }


            public string value { get; set; }

            public string module { get; set; }
            public NewColor(string file, string key, string value)
            {
                this.file = file;
                this.key = key;
                this.value = value;
                module = $"[{Path.GetFileNameWithoutExtension(FileUtil.GetParentDirectoryPath(file, 5))}]  {key}";
               

            }
        }

        enum Step{ 
           CheckWhite,
           EditColor
        }
        private async void ColorCheck_LoadAsync(object sender, EventArgs e)
        {

            p_label.Controls.Add(lb_prompt);
            gb_1.Controls.Add(clb);
            gb_1.Controls.Add(p_label);
            gb_1.Controls.Add(cb_all);
            gb_2.Controls.Add(rtb);
            gb_2.Visible = false;
            btn_next.Enabled = false;
            cb_all.Enabled = false;
            lb_prompt.Text = "下列項目是雲端上找不到對應android_key的color，" +
                "請勾選要加入白名單的項目(代表沒有要換SKIN)，沒勾選的請至雲端上加入對應的android_key後，再刷新一次";
           
            initListView();

            //所有skin
            skinProject =
              Directory.GetDirectories($@"{selectProject}")
              .Where(path =>
              {
                  Boolean isApp = FileUtil.CheckIsAndroidApplication(path);
                  string folderName = Path.GetFileNameWithoutExtension(path);
                  return isApp && !folderName.Equals("app");
              }).Select(path => Path.GetFileNameWithoutExtension(path)).ToList();


            var task = new List<Task<Dictionary<string, Dictionary<string, string>>>>();

            //找remote資料
            task.Add(GetRemote());
            //找主專案
            task.Add(GetMain());
            //找本地SKIN資料
            task.Add(GetLocal());


            var taskResult= await Task.WhenAll(task);



            if (taskResult[0] == null) {
                //雲端資料有問題
                return;
            }
            //緩存主專案跟本地skin
            main = taskResult[1];
            
            local = taskResult[2];

            remote = taskResult[0];

            ComparDataAndShow(taskResult[0], taskResult[1], taskResult[2]);


        }

        private async void ComparDataAndShow(Dictionary<string, Dictionary<string, string>> remoteResult,
            Dictionary<string, Dictionary<string, string>> mainResult,
            Dictionary<string, Dictionary<string, string>> localResult
            ) {

            //把資料清掉
            var t= Task.Run(() =>
            {
             diffColor.Clear();
             newColor.Clear();
             defaultColor.Clear();
             
             //檢查default 
             foreach (var main in mainResult)
             {
                 //kv就是 顏色key 跟 value
                 foreach (var kv in main.Value)
                 {
                     var r = remoteResult["default"];
                     if (r.ContainsKey(kv.Key))
                     {
                         if (white.Contains(kv.Key))
                         {
                             //在白名單內，也在雲端上
                             //白名單中除名
                             white.Remove(kv.Key);
                             Properties.Settings.Default.whiteColor = JsonSerializer.Serialize(white);
                             Properties.Settings.Default.Save();
                         }

                         //雲端找的到對應的android_key
                         if (!r[kv.Key].Equals(kv.Value))
                         {
                             //顏色不一樣

                             diffColor.Add(new DiffColor(main.Key, kv.Key, kv.Value, r[kv.Key], "default",true));
                         }

                     }
                     //不在白名單內
                     else if (!white.Contains(kv.Key))
                     {
                         newColor.Add(new NewColor(main.Key, kv.Key, kv.Value));
                     }
                     //在白名單內，雲端也不換膚，加到預設
                     else {
                         defaultColor.Add(kv.Key, kv.Value);
                     }

                 }
             }
             //檢查SKIN
             foreach (var local in localResult)
             {
                 //local.key 是哪個SKIN的名子 ex:green_c3
                 //kv就是 顏色key 跟 value
                 foreach (var kv in local.Value)
                 {
                     var r = remoteResult[local.Key];
                     if (r.ContainsKey(kv.Key))
                     {
                         //雲端找的到對應的android_key
                         if (!r[kv.Key].Equals(kv.Value))
                         {
                             //顏色不一樣
                             string file = $@"{selectProject}\{local.Key}\src\main\res\values\colors.xml";
                             diffColor.Add(new DiffColor(file, kv.Key, kv.Value, r[kv.Key],local.Key ,false));
                         }
                     }
                     else
                     {
                         //沒有key的話會被刪除，因為整張xml會重寫
                     }
                 }
             }



             foreach (var n in newColor)
             {


                 Console.WriteLine($"-------------------------new-------------------------");
                 Console.WriteLine($"{n.file},{n.key},{n.value}");

             }

             foreach (var d in diffColor)
             {

                 Console.WriteLine($"-------------------------diff-------------------------");
                 Console.WriteLine($"{d.file},{d.key},{d.oldValue} =====> {d.newValue} 是否主專案:{d.isDefault}");
             }


         });

         await t;


            if (newColor.Count > 0)
            {
                curStep = Step.CheckWhite;

                gb_1.Visible = true;
                gb_2.Visible = false;
                clb.BeginUpdate();
                clb.Items.Clear();
                foreach (var c in newColor) {
                    clb.Items.Add(c.module);
                }
                clb.EndUpdate();
                cb_all.Enabled = true;
                cb_all.Checked = false;
                btn_next.Enabled = true;


            }
            else
            {
                curStep = Step.EditColor;

                btn_next.Enabled = true;
                btn_next.Text = "更新檔案";
                gb_1.Visible = false;
                gb_2.Visible = true;

                rtb.ReadOnly = true;
                foreach (var color in diffColor) {

                    rtb.SelectionColor = System.Drawing.Color.Blue;
                    rtb.AppendText($"[{color.module}]");
                    rtb.SelectionColor= System.Drawing.Color.Red;
                    rtb.AppendText($" | {color.key} | ");
                    rtb.SelectionColor = System.Drawing.Color.Black;
                    rtb.AppendText($"{color.oldValue} ==> {color.newValue}\n");

                }



            }

        }

        private Task<Dictionary<string, Dictionary<string, string>>> GetLocal() {
            return Task<Dictionary<string, Dictionary<string, string>>>.Run(() =>
            {

                var local = new Dictionary<string, Dictionary<string, string>>();

                //找skin

                foreach (string s in skinProject)
                {
                    string path = $@"{selectProject}\{s}\src\main\res\values\colors.xml";
                    if (File.Exists(path))
                    {
                        local.Add(s, readColorXml(path));
                    }
                    else
                    {
                        local.Add(s, new Dictionary<string, string>());
                    }

                }
                return local;
            });
        } 

        private Task<Dictionary<string, Dictionary<string, string>>> GetMain() {
            return Task<Dictionary<string, Dictionary<string, string>>>.Run(() =>
             {

                 var main = new Dictionary<string, Dictionary<string, string>>();

                //主專案下資料夾
                foreach (string path in Directory.GetDirectories($@"{rootProject}"))
                 {

                     string[] src = Directory.GetDirectories(path, "src", System.IO.SearchOption.TopDirectoryOnly);
                    //該資料夾下如果有src這個資料夾
                    foreach (string s in src)
                     {

                         Dictionary<string, string> root = new Dictionary<string, string>();
                         string file = s + $@"\main\res\values\colors.xml";

                         if (File.Exists(file))
                         {

                             var color = readColorXml(file);
                             foreach (var key in color)
                             {
                                 root.Add(key.Key, key.Value);
                             }
                         }
                         main.Add(file, root);
                     }
                 }

                 return main;
             });
        }

        private Task<Dictionary<string, Dictionary<string, string>>> GetRemote() {

          return  Task<Dictionary<string, Dictionary<string, string>>>.Run(() =>
            {

                var remote = new Dictionary<string, Dictionary<string, string>>();

                UserCredential credential;

                using (var stream =
                    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define request parameters.
                String spreadsheetId = "19jXk5PXAfwbFy7tNwE3eRdp9Ee7p_kLJjuK_DgMt--U";
                String range = "工作表1";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                        service.Spreadsheets.Values.Get(spreadsheetId, range);

                // Prints the names and majors of students in a sample spreadsheet:
                // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;

                if (values == null && values.Count == 0)
                {

                    showBox("資料表無資料");
                    return null;
                }
                //先找andorid_key
                int androidKeyColumnIndex = values[0].IndexOf("android_key");

                if (androidKeyColumnIndex == -1)
                {
                    showBox("資料表找不到android_key欄位");
                    return null;
                }

                int androidSkinRowIndex = values.IndexOf(values.First(v => { return v[0].Equals("android_skin"); }));

                if (androidSkinRowIndex == -1)
                {
                    showBox("資料表找不到android_skin欄位");
                    return null;
                }

                var defaultIndext = values[androidSkinRowIndex].IndexOf("default");

                if (defaultIndext == -1)
                {
                    showBox("資料表找不到default欄位");
                    return null;
                }
                //最上面欄位有幾欄，用來檢查row是否有填寫完整
                var columnSize = values[0].Count;

                //跳過空白欄位
                var skins = values[androidSkinRowIndex]
                .Where(skin =>
                {

                    return (skin != null &&
                    !skin.Equals("")
                    && skinProject.Contains(skin))
                    || skin.Equals("default");
                }
                ).ToList();
                foreach (string skin in skins)
                {

                    Dictionary<string, string> data = new Dictionary<string, string>();
                    var skinColumnIndex = values[androidSkinRowIndex].IndexOf(skin);

                    for (int i = 0; i < values.Count; i++)
                    {
                        var row = values[i];
                        //檢查資料表是否完整
                        //skinColumnIndex是該skin在表中的column index 
                        //如果表的某row最後一欄空白，則該row的size就會-1
                        //回傳的list長度就會比其他row少1，所以用 skinColumnIndex> row.Count
                        //可以判斷最後一欄是不是空白

                        if (skinColumnIndex >= row.Count)
                        {
                            showBox($"資料表[{values[0][skinColumnIndex]},{i + 1}]不完整");
                            return null;
                        }
                        if (!row[androidKeyColumnIndex].Equals("")
                        && !row[androidKeyColumnIndex].Equals("android_key"))
                        {

                            if (row[androidKeyColumnIndex].Equals(""))
                            {
                                showBox($"資料表[{values[0][androidKeyColumnIndex]},{i + 1}]不完整");
                                return null;
                            }
                            string color;
                            if (!checkColor(row[skinColumnIndex].ToString(), out color))
                            {
                                showBox($"資料表[{values[0][skinColumnIndex]},{i + 1}]不完整");
                                return null;
                            }

                            data.Add(row[androidKeyColumnIndex].ToString()
                                , color);

                        }

                    }

                    remote.Add(skin, data);

                }

                //foreach (var k in remote)
                //{
                //    Console.WriteLine($"-----------------------{k.Key}-----------------------");
                //    foreach (var v in k.Value)
                //    {
                //        Console.WriteLine($"[{v.Key},{v.Value}]");
                //    }
                //}


                return remote;

            });
        }

        private void initListView()
        {

            clb.CheckOnClick = true;
            cb_all.CheckedChanged += new EventHandler(cb_all_CheckedChanged);

        }

        private void cb_all_CheckedChanged(object sender, EventArgs e)
        {
            var isCheck = cb_all.Checked;
              for(int i = 0; i < newColor.Count; i++)
            {
                clb.SetItemChecked(i, isCheck);
            }
        }



        private void showBox(string text)
        {
            this.Invoke((Action)(() =>
            {
                MessageBox.Show(text);
            }));
        }

        private bool checkColor(string o,out string color) {
            Regex rgx = new Regex(@"^[A-Fa-f0-9]{6}$|^[A-Fa-f0-9]{8}$");
            var match = rgx.IsMatch(o);
            if (match)
            {
                color = $"#{o}";
            }
            else {
                color = o;
            }
            return match;
        }

        private Dictionary<string,string> readColorXml(string filePath) {
            Dictionary<string, string> color = new Dictionary<string, string>();

            XmlDocument XmlDoc = new XmlDocument();

            XmlDoc.Load(filePath);

            XmlNodeList NodeLists = XmlDoc.SelectNodes("resources/color");

            foreach (XmlNode OneNode in NodeLists)
            {
                String colorKey = OneNode.Attributes["name"].Value;
                String colorValue = OneNode.InnerText;
                color.Add(colorKey, colorValue);
            }

            return color;

        }


        private async void updateColorXml()
        {
            btn_next.Enabled = false;
            btn_next.Text = "更新中";
            //改主專案的color
           var t= Task.Run(() =>
            {

                IEnumerable<IGrouping<string,DiffColor>> query= from diff in diffColor.Where(c => c.isDefault)
                                                                group diff by diff.file;

                foreach (var group in query) {

                    XmlDocument XmlDoc = new XmlDocument();
                    XmlDoc.PreserveWhitespace = true;
                    XmlDoc.Load(group.Key);

                    foreach (var d in group) {

                        var node = XmlDoc.SelectSingleNode($"resources/color[@name = '{d.key}']");
                        if (node != null)
                        {
                            node.InnerText = d.newValue;
                        }

                    }

                    XmlDoc.Save(group.Key);

                }
            
            });
            var time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            foreach (var r in remote)
            {
                if (r.Key.Equals("default")) continue;

                string path = $@"{selectProject}\{r.Key}\src\main\res\values\colors.xml";
                if (File.Exists(path)) {
                    File.Delete(path);
                }
                XmlDocument xmlDoc = new XmlDocument();

                 //增加一個文件說明
                 XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                 xmlDoc.AppendChild(xmlDeclaration);

                //Create a comment.
                 XmlComment newComment = xmlDoc.CreateComment(time);
                xmlDoc.AppendChild(newComment);

                //為文件新增一個根元素
                 XmlElement rootElement = xmlDoc.CreateElement("resources");


                foreach (var k in r.Value) {
               

                    //為根元素裡面增加子元素，接下來增加元素都要講子元素新增到rootElement元素下
                    XmlElement colorElement = xmlDoc.CreateElement("color");

                    colorElement.SetAttribute("name", k.Key);

                    colorElement.InnerText = k.Value;

                    rootElement.AppendChild(colorElement);

                 }

                rootElement.AppendChild(xmlDoc.CreateComment("***********以下是不換SKIN的Color***********"));

                foreach(var d in defaultColor) {

                    //為根元素裡面增加子元素，接下來增加元素都要講子元素新增到rootElement元素下
                    XmlElement colorElement = xmlDoc.CreateElement("color");

                    colorElement.SetAttribute("name", d.Key);

                    colorElement.InnerText = d.Value;

                    rootElement.AppendChild(colorElement);


                }


                xmlDoc.AppendChild(rootElement);

                //將該Dom物件寫入xml檔案
                xmlDoc.Save(path);
            }

            await t;

            Properties.Settings.Default.updateTime = time;
            Properties.Settings.Default.Save();


            Dispose();

            MessageBox.Show("更新成功");



        }


        private async void btn_next_Click(object sender, EventArgs e)
        {
            if (curStep == Step.CheckWhite)
            {
                if (clb.CheckedItems.Count <= 0)
                {
                    MessageBox.Show("至少勾選一項");
                    return;
                }
                btn_next.Enabled = false;
                foreach (var item in clb.CheckedItems)
                {
                    var index = newColor.FindIndex(color => color.module.Equals(item.ToString()));
                    white.Add(newColor[index].key);
                }


                Properties.Settings.Default.whiteColor = JsonSerializer.Serialize(white);
                Properties.Settings.Default.Save();

                var result = await GetRemote();
                if (result == null)
                {
                    // 雲端資料有問題
                    return;
                }
                remote = result;

                ComparDataAndShow(result, main, local);

            }
            else {

                //更新

                updateColorXml();


            }

        }

    }
}
