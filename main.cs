using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using SKinEditer.row;
using SKinEditer.util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKinEditer
{


    public partial class main : Form
    {
        List<Dictionary<string, SKinRow>> rowList = new List<Dictionary<string, SKinRow>>();

        List<Dictionary<string, SKinRow>> allRowList;

        DataTable Alldt;

        DataTable dt = new DataTable("dt");

        private string selectFolder;

        private string selectRootProjectPath;

        private string selectSkinProjectPath;

        private string curInsertSelectPath;

        private List<WhileItem> whileList;

        private OriginIndex c=new OriginIndex(-1,-1);

        public main()
        {
            InitializeComponent();



            Load += new EventHandler(Form1_Load);
        }


        private void Form1_Load(object sender, EventArgs e)

        {
            DoubleBuffer.DoubleBufferedDataGirdView(dgv);
            initialDataGridView();
            hideWidget();
            initWhite();

            menu_file_check_xml.Enabled = false;

            menu_file_check_color.Enabled = false;

            menu_file_build_skin.Enabled = false;

        }

        private void initWhite()
        {
            string white = Properties.Settings.Default.whiteFile;

            if (white == null || white.Length == 0)
            {
                whileList = new List<WhileItem>();
            }
            else
            {

                whileList = JsonSerializer.Deserialize<List<WhileItem>>(white);
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };

                Console.Write(JsonSerializer.Serialize(whileList, options));
                whileList.ForEach(set => set.file?.RemoveWhere(i =>
                {
                    var isE = File.Exists(i);
                    if (!isE) Console.WriteLine("檔案不在:" + i);
                    return !isE;
                }));

            }

        }

        private void saveWhite()
        {

            Properties.Settings.Default.whiteFile = JsonSerializer.Serialize(whileList);
            Properties.Settings.Default.Save();

        }

        private HashSet<string> getWhiteBySrc()
        {
            HashSet<string> f = whileList.Find(item => item.src.Equals(selectFolder))?.file;

         
            if (f == null)
            {
                f = new HashSet<string>();
                whileList.Add(new WhileItem
                {
                    file = f,
                    src = selectFolder
                });
            }

            return f;
        }


        private void hideWidget()
        {
            //cb_src.Hide();
            //label3.Hide();
            //tb_search.Hide();
            //lable_skin_src.Hide();
            //label1.Hide();
            //label2.Hide();
            //pictureBox1.Hide();
            //pictureBox2.Hide();
            //dgv.Hide();

            foreach (Control c in Controls)
            {
                if(!(c is MenuStrip))
                c.Visible = false;
            }
        }

        private void showWidget()
        {
            //cb_src.Show();
            //lable_skin_src.Show();
            //label1.Show();
            //label2.Show();
            //pictureBox1.Show();
            //pictureBox2.Show();
            //dgv.Show();
            //label3.Show();
            //tb_search.Show();

            foreach (Control c in Controls)
            {
                    c.Visible = true;
            }
        }

        private void loadProject()
        {
            showWidget();
            initColumn();
            initComboBox();


        }

        private void initComboBox()
        {
            cb_src.Items.Add("drawable");
            cb_src.Items.Add("drawable-ldpi");
            cb_src.Items.Add("drawable-mdpi");
            cb_src.Items.Add("drawable-hdpi");
            cb_src.Items.Add("drawable-xhdpi");
            cb_src.Items.Add("drawable-xxhdpi");
            cb_src.Items.Add("drawable-xxxhdpi");
            cb_src.Items.Add("mipmap-hdpi");
            cb_src.Items.Add("mipmap-mdpi");
            cb_src.Items.Add("mipmap-hdpi");
            cb_src.Items.Add("mipmap-xhdpi");
            cb_src.Items.Add("mipmap-xxhdpi");
            cb_src.Items.Add("mipmap-xxxhdpi");
            //預設用drawable-xhdpi
            cb_src.SelectedIndex = 4;

            selectFolder = (string)cb_src.SelectedItem;
        }

 
        private void initialDataGridView()
        {

            // dgv.VirtualMode = true;
            dgv.AutoResizeColumns();
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // DataGridView 基礎設定
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            //padding
            Padding newPadding = new Padding(10, 10, 10, 10);
            dgv.RowTemplate.DefaultCellStyle.Padding = newPadding;
            dgv.DataSource = dt;
            //首行(圖片名稱 + 圖片像素那個) 只讀
            //dgv.Columns[0].ReadOnly = true;
            ////首行(圖片名稱 + 圖片像素那個) 置中
            //dgv.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.CellFormatting +=
           new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
           // dgv.CellPainting += new DataGridViewCellPaintingEventHandler(dgv_CellPainting);
            dgv.KeyDown += new KeyEventHandler(dgv_KeyDown);

            //  dgv.CellValueNeeded += new
            //DataGridViewCellValueEventHandler (dgv_CellValueNeeded);

            dgv.CellContextMenuStripNeeded += new DataGridViewCellContextMenuStripNeededEventHandler(dgv_Users_CellContextMenuStripNeeded);

            dgv.CellContentDoubleClick += new DataGridViewCellEventHandler(dgv_CellContentDoubleClick);

            contextMenuStripRight.ItemClicked += new ToolStripItemClickedEventHandler(StripItemClickedEvent);

            contextMenuStripHeader.ItemClicked += new ToolStripItemClickedEventHandler(StripItemClickedEvent);
        }

    
    

        private void initColumn()
        {
            string[] dirs = Directory.GetDirectories($@"{selectSkinProjectPath}");

            dt.Columns.Add("Img", typeof(string));
            dt.Columns.Add("default", typeof(Image));
            //創建column
            foreach (string path in dirs)
            {

                Boolean isApp = FileUtil.CheckIsAndroidApplication(path);
                string folderName = Path.GetFileNameWithoutExtension(path);
                //檢查是不是skin 過濾掉app
                if (isApp && !folderName.Equals("app"))
                {
                    //如果是skin 資料夾，加入column
                    dt.Columns.Add(folderName, typeof(Image));

                }

            }

            dgv.Columns[0].ReadOnly = true;
            ////首行(圖片名稱 + 圖片像素那個) 置中
            dgv.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                var d = dgv.Columns[i].MinimumWidth = 120;
            }

        }



        private void initRow(
            List<string> origin,
            List<string> allSkin,
            Dictionary<string, List<string>> SkinAllImg)
        {
            dgv.Enabled = false;
            //dgv.Dispose();
            dt.BeginLoadData();

            dt.Rows.Clear();

            rowList.Clear();

            //裡面資料都換成檔案名稱，要取差集，skin專案有的圖 而 原專案沒有的圖
            var expectedList = allSkin.
                Except(origin.Select((path, index) => Path.GetFileName(path)));


            //加入skin資料夾有的圖，但主專案沒有的圖

            foreach (string fileName in expectedList)
            {

                var row = dt.NewRow();
                Dictionary<string, SKinRow> rowDictionary = new Dictionary<string, SKinRow>();
                //先加入column 0 跟 1 都會是skinEmpty
                //folderPath則用主專案資料夾(因為原專案沒有相對應的圖)
                //而原專案有模組化，無法知道要開啟哪一個模組下的src資料夾
                rowDictionary.Add("Img", new SkinRowEmpty(fileName, selectRootProjectPath));
                rowDictionary.Add("default", new SkinRowEmpty(fileName, selectRootProjectPath));

                //沒有資訊，所以空字串
                row["Img"] = "";
                //沒有圖，用1px
                row["default"] = new Bitmap(1, 1);

                inflateSkinRow(fileName, row, rowDictionary, SkinAllImg);
            }

            getWhiteBySrc().RemoveWhere(white => !File.Exists(white));

            //檢查白名單裡是否有其中skin是有對應圖的
            //有可能原本已經加入白名單不打算換圖，但是後續卻突然說要換圖
            foreach (string white in getWhiteBySrc())
            {
                if (File.Exists(white))
                {
                 var fileName = Path.GetFileName(white);
                //如果所有skin都沒有對應的圖，又是白名單的話，不顯示
                //如果有其中的skin有對應圖，但是在白名單內
                if (allSkin.Contains(fileName))
                {
                    MakeShowHeaderRow(white, SkinAllImg, true);
                }
                }

             

            }
            
            //過濾掉白名單，剩下的才是要換skin
            foreach (string mainAppPath in origin.Where(s => !getWhiteBySrc().Contains(s)))
            {
                //所有skin裡對應的圖片名稱
                MakeShowHeaderRow(mainAppPath, SkinAllImg);

            }
            allRowList = new List<Dictionary<string, SKinRow>>(rowList);

            Alldt = dt.Copy();

            dt.EndLoadData();


            dgv.ClearSelection();

            dgv.Enabled = true;

        }

        private void MakeShowHeaderRow(string mainAppPath, Dictionary<string, List<string>> SkinAllImg, bool headerIsWhite = false)
        {

            var fileName = Path.GetFileName(mainAppPath);
            var row = dt.NewRow();
            Dictionary<string, SKinRow> rowDictionary = new Dictionary<string, SKinRow>();

            //先加入原專案的圖(column 0 跟 1)
            var mainAppImg = Image.FromFile(mainAppPath);
            var showText = $"{fileName}\n\n{mainAppImg.Width}x{mainAppImg.Height}";
            rowDictionary.Add("default", new SKinRowImg(fileName, mainAppPath, mainAppImg.Width, mainAppImg.Height));
            rowDictionary.Add("Img", new SKinRowHeader(fileName, mainAppPath, mainAppImg.Width, mainAppImg.Height, showText, headerIsWhite));

            row["Img"] = showText;
            row["default"] = BitmapUtil.ZoomPicture(fileName, mainAppImg, 100, 100);

            inflateSkinRow(fileName, row, rowDictionary, SkinAllImg);
        }


        private async void LoadFolderData()
        {

            Progress p = new Progress(this);

            p.Show();

            p.setText("正在載入");

            //progress index
            int index = 0;

            int mainSize = 0;


            var ts = new List<Task<List<string>>>();

            ConcurrentDictionary<string, List<string>> SkinAllImg = new ConcurrentDictionary<string, List<string>>();

            //找出原專案所有圖片

            string[] dirs = Directory.GetDirectories($@"{selectRootProjectPath}");
            //主專案下資料夾
            foreach (string path in dirs)
            {

                string[] src = Directory.GetDirectories(path, "src", System.IO.SearchOption.TopDirectoryOnly);
                //該資料夾下如果有src這個資料夾
                foreach (string s in src)
                {
                    string folder = s + $@"\main\res\{selectFolder}";
                    Console.WriteLine(folder);
                    if (Directory.Exists(folder))
                    {
                        mainSize++;
                        ts.Add(Task<List<string>>.Run(() =>
                        {

                            var swOrigin = Stopwatch.StartNew();

                            var o = Directory.GetFiles($"{folder}", "*", System.IO.SearchOption.AllDirectories)
                            .Where(file => file.EndsWith(".png") || file.EndsWith(".jpg")).ToList();


                            swOrigin.Stop();
                            Console.WriteLine($"找原專案{s}:" + swOrigin.ElapsedMilliseconds);

                            this.Invoke((Action)(() =>
                            {
                                index++;
                                p.setProgress(index);
                            }));

                            return o;

                        }));
                    }


                }
            }


            //找出所有skin的圖片
            for (int i = 2; i < dt.Columns.Count; i++)
            {
                string sk = dt.Columns[i].ColumnName;
                ts.Add(
                    Task<List<string>>.Run(() =>
                    {
                        var swSk = Stopwatch.StartNew();
                        string path = $@"{selectSkinProjectPath}\{sk}\src\main\res\{selectFolder}";
                        List<string> r;

                        if (Directory.Exists(path))
                        {
                            r = Directory.GetFiles(path).Where(s => ((s.EndsWith(".png") || s.EndsWith(".jpg"))))
                            .Select(s => Path.GetFileName(s))
                            .ToList();
                        }
                        else {
                            r = new List<string>();
                        }
                      
                        this.Invoke((Action)(() =>
                        {
                            index++;
                            p.setProgress(index);
                            SkinAllImg.TryAdd(sk, r);
                        }));

                        swSk.Stop();
                        Console.WriteLine($"找{sk}:" + swSk.ElapsedMilliseconds);
                        return r;
                    }
                    ));

            }

            p.Max(ts.Count());

            var result = await Task.WhenAll(ts);

            //將skin所有圖片過濾重複
            var l = new List<string>();
            foreach (KeyValuePair<string, List<string>> item in SkinAllImg)
            {
                l.AddRange(item.Value);
            }


            List<string> main = new List<string>();
            //合併主專案資料夾裡所有檔案名稱

            for (int i = 0; i < mainSize; i++)
            {

                main.AddRange(result[i]);
            }

            var myDictionary = SkinAllImg.ToDictionary(entry => entry.Key,
                                                       entry => entry.Value);
            p.Dispose();
            //顯示資料
            initRow(main, l.Distinct().ToList(), myDictionary);

        }



        private void inflateSkinRow(string fileName, DataRow row, Dictionary<string, SKinRow> rowDictionary,
        Dictionary<string, List<string>> SkinAllImg)
        {

            //0 跟 1 已經加入 從2開始
            for (int i = 2; i < dt.Columns.Count; i++)
            {

                string columnsName = dt.Columns[i].ColumnName;

                if (SkinAllImg[columnsName].Contains(fileName))
                {

                    inflateImgRow(fileName, columnsName, rowDictionary, row);
                }
                else
                {
                    inflateEmptyImgRow(fileName, columnsName, rowDictionary, row);
                }
            }

            rowList.Add(rowDictionary);

            dt.Rows.Add(row);

        }

        private void inflateImgRow(string fileName, string columnsName,
            Dictionary<string, SKinRow> rowDictionary,
             DataRow row)
        {

           


            string path = $@"{selectSkinProjectPath}\{columnsName}\src\main\res\{selectFolder}\{fileName}";
            var img = Image.FromFile(path);

            var Skin = new SKinRowImg(fileName, path, img.Width, img.Height);
            //加入紀錄資料
            if (rowDictionary.ContainsKey(columnsName))
            {
                rowDictionary[columnsName] = Skin;
            }
            else
            {
                rowDictionary.Add(columnsName, Skin);
            }

            //傳入的img會被釋放掉，後續再使用會是null報錯
            var thumbnail = BitmapUtil.ZoomPicture(fileName, img, 100, 100);
            //顯示縮圖
            row[columnsName] = thumbnail;
        }

        private void inflateEmptyImgRow(string fileName, string columnsName,
            Dictionary<string, SKinRow> rowDictionary,
             DataRow row)
        {

            var Skin = new SkinRowEmpty(fileName,
                $@"{selectSkinProjectPath}\{columnsName}\src\main\res\{selectFolder}");
            if (rowDictionary.ContainsKey(columnsName))
            {
                rowDictionary[columnsName] = Skin;
            }
            else
            {
                rowDictionary.Add(columnsName, Skin);
            }


            row[columnsName] = new Bitmap(1, 1);
        }

    

        private SKinRow getSkinRow(int column, int row)
        {
            if (!hasData(column, row))
                return null;
            return rowList[row][dgv.Columns[column].Name];
        }

        private bool hasData(int column, int row)
        {
            return row < rowList.Count && rowList[row].ContainsKey(dgv.Columns[column].Name);
        }



        private void dgv_CellFormatting(object sender,
                System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
        {


            var RowIndex = getOriginTableIndex(e.RowIndex);

          //  Console.WriteLine($"[{oi.ColumnIndex},{oi.RowIndex}]---{dgv.Rows.Count}");

            var columnName = dgv.Columns[e.ColumnIndex].Name;
            //判斷資料用原資料cell 修改畫面則用e
            Dictionary<string, SKinRow> row = rowList[RowIndex];

            if (row["Img"] is SKinRowHeader)
            {

                SKinRowHeader h = row["Img"] as SKinRowHeader;

                if (h.isWhite && e.ColumnIndex==0)
                {
                    //如果是白明單，header為綠色
                   e.CellStyle.BackColor = Color.Green;
                }
                //else {
                //    e.CellStyle.BackColor = Color.Empty;
                //}


                if (row.ContainsKey(columnName) && row[columnName] is SKinRowImg)
                {
                    //不是白名單，正常格

                    SKinRowImg i = row[columnName] as SKinRowImg;

                    if (i.width != h.width || i.height != h.height)
                    {
                        //如果像素跟原App的圖片不同，則顯示紅底
                        e.CellStyle.BackColor = Color.Red;
                    }
                }


            }


        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //這裡不用 getOriginTableCell(e.ColumnIndex, e.RowIndex);
            //因為e是在rowFilter之後的dataGrid
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
             //   Console.WriteLine($"[{e.ColumnIndex},{e.RowIndex}]");
                setSelect(e.ColumnIndex, e.RowIndex);
            }

        }


        private void dgv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


            var RowIndex = getOriginTableIndex(e.RowIndex);

            var s = getSkinRow(e.ColumnIndex, RowIndex);

            if (s is SKinRowImg)
            {
                SKinRowImg sri = s as SKinRowImg;
                FileUtil.OpenFile(sri.filePath);

            }
        }

        //設置右鍵時要跳出什麼菜單
        private void dgv_Users_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
           // var oi = getOriginTableIndex(, e.RowIndex);

            var RowIndex = getOriginTableIndex(e.RowIndex);


            //右鍵菜單
            if (RowIndex >= 0 && hasData(e.ColumnIndex, RowIndex))
            {

                var s = getSkinRow(e.ColumnIndex, RowIndex);
                if (e.ColumnIndex >= 1)
                {
                    var header = getSkinRow(0, RowIndex);
                    e.ContextMenuStrip = contextMenuStripRight;
                    //新增
                    //主專案有圖才能加入圖片
                    contextMenuStripRight.Items[1].Enabled = header is SKinRowHeader ;
                    //刪除
                    //有資料才能刪除(源專案的圖不可單獨刪除，要刪除只能連同skin一起刪除)
                    contextMenuStripRight.Items[2].Enabled = s is SKinRowImg && e.ColumnIndex != 1 ;
                    //調整像素
                    //有資料才能調整像素
                    contextMenuStripRight.Items[3].Enabled = s is SKinRowImg && header is SKinRowHeader && e.ColumnIndex != 1;
                }
                else
                {
                    //header 的右鍵菜單不同

                    e.ContextMenuStrip = contextMenuStripHeader;
                    contextMenuStripHeader.Items[0].Enabled = s is SKinRowHeader;
                    if ( s is SKinRowHeader)
                    {
                        SKinRowHeader h = s as SKinRowHeader;

                        if (h.isWhite) { 
                            contextMenuStripHeader.Items[0].Text = "取消白名單"; }
                        else {
                            contextMenuStripHeader.Items[0].Text = "加入白名單";
                        }

                    }

                    contextMenuStripHeader.Items[1].Enabled = s is SKinRowHeader;

                }
            }

        }
        //按鈕事件(快捷鍵)
        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                DeleteImg();
            }
            else if (e.KeyData == Keys.Insert)
            {

                var RowIndex = getOriginTableCurrentIndex();
                var ColumnIndex = dgv.CurrentCell.ColumnIndex;

                var s = getSkinRow(ColumnIndex, RowIndex);
                var s0 = getSkinRow(0, RowIndex);

                 AddImg();
            }
        }

        //菜單點擊事件
        private void StripItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = (ContextMenuStrip)sender;
            menu.Hide();

            switch (e.ClickedItem.Text)
            {

                case "前往資料夾":
                    ToFolder();
                    break;
                case "加入白名單":
                    AddWhite();
                    break;
                case "取消白名單":
                    CancelWhite();
                    break;
                case "新增":
                    AddImg();
                    break;

                case "刪除":
                    DeleteImg();
                    break;
                case "調整像素":
                    AdjPixel();
                    break;
                case "調整所有圖片像素":
                    AdjAllPixelAsync();
                    break;
                case "將所有SKIN圖片移至":
                    MoveAllSkin();
                    break;



            }
        }





        //資料夾選擇
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectFolder = (string)cb_src.SelectedItem;
            LoadFolderData();
        }


        //選擇跟目錄點擊事件
        private void menu_file_open_project_Click(object sender, EventArgs e)
        {

            SelectProjectDialog d = new SelectProjectDialog();
            d.Owner = this;
            d.StartPosition = FormStartPosition.CenterParent;

            if (d.ShowDialog(this) == DialogResult.OK)
            {
                selectRootProjectPath = d.rootPath;
                selectSkinProjectPath = d.skinPath;
                d.Dispose();
                loadProject();
                //啟用檢查XML
                menu_file_check_xml.Enabled = true;
                menu_file_check_color.Enabled = true;
                menu_file_build_skin.Enabled = true;
                menu_file_open_project.Enabled = false;
            }

        }

        private void menu_file_add_img_Click(object sender, EventArgs e)
        {
               DrawableXmlCheck a = new DrawableXmlCheck(this,selectSkinProjectPath,selectRootProjectPath);
              a.Show();
        }   




        private async void MoveAllSkin()
        {

            var RowIndex = getOriginTableCurrentIndex();

            var ColumnIndex = dgv.CurrentCell.ColumnIndex;

            var mine = rowList[RowIndex];

            Console.WriteLine("自己:" + RowIndex + "  " + rowList[RowIndex]["Img"].imgName);
            SelectMainImg smi = new SelectMainImg(
                rowList
                .Where(r => {
                    //過濾不是圖片 跟 自己
                    return r["default"] is SKinRowImg && r != mine;
                })
                .Select(r =>{
                    return ((SKinRowImg)r["default"]).filePath;
                }).ToList());

            smi.Owner = this;
            smi.StartPosition = FormStartPosition.CenterParent;

            if (smi.ShowDialog(this) == DialogResult.OK)
            {
               var SelectPath = smi.SelectPath;

                //  var IsCover = smi.IsCover;
                smi.Dispose();


                Progress p = new Progress(this);
                p.setText("處理中");
                p.Show();
                //找出row 裡的所有圖

                Console.WriteLine($"index:{RowIndex} {rowList[RowIndex]["Img"].imgName}");
                List<SKinRowImg> sris = rowList[RowIndex].Where((key) => {
                    return key.Value is SKinRowImg && !key.Key.Equals("default");
                }).Select((key) => {

                    return key.Value as SKinRowImg;
                
                }).ToList();

                foreach (SKinRowImg sria in sris) {

                    Console.WriteLine($"檔案路徑:{sria.filePath}  檔案名稱:{sria.imgName}");
                } 

           

                p.Max(sris.Count());

                var t = Task.Factory.StartNew(() =>
                {
                 
                    foreach (SKinRowImg sri in sris)
                    {
                        string newPath = Path.GetDirectoryName(sri.filePath) + $@"/{Path.GetFileName(SelectPath)}";
                
                        //如果目標檔案存在，則將目標檔案移到資源回收桶
                        if (File.Exists(newPath))
                        {
                            //移到資源回收桶
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(newPath,
                                UIOption.OnlyErrorDialogs,
                                RecycleOption.SendToRecycleBin);

                        }

                        Console.WriteLine($"從:{sri.filePath}到{newPath}");


                        File.Move(sri.filePath, newPath);

                        this.Invoke((Action)(() =>
                        {
                            p.Plus();
                        }));
                    }
                });
                await t;
                p.Dispose();
                //更新

                Console.WriteLine($"移動完成");

                int targetIndex = rowList.FindIndex(d => {
                    return d["default"] is SKinRowImg && ((SKinRowImg)d["default"]).filePath.Equals(SelectPath);
                });
                Console.WriteLine("目標:" + targetIndex +"  "+ rowList[targetIndex]["Img"].imgName);
                //檢查原本那個row有沒有原圖，如果沒有原圖那就把整行刪除，如果有則保留
                bool hasImg = getSkinRow(1, RowIndex) is SKinRowImg; 

                
                dt.Rows[targetIndex].BeginEdit();
                //有原圖才需要編輯，否則後面會直接刪除
                if (hasImg) {
                    dt.Rows[RowIndex].BeginEdit();
                }

                for (int i = 2; i < dt.Columns.Count; i++) {

                    //把資料丟過去目標index
                    var sk = getSkinRow(i, RowIndex);
                    //目標row的dafault imgName名稱
                    var targetSkName = getSkinRow(1,targetIndex).imgName;
                    
                    var columnName = dgv.Columns[i].Name;
                    //檢查看原本的row的這格欄位有沒有圖
                    //如果有圖就在目標index Row 對應的欄位加圖
                    //如果沒有圖則不動，也不用填EmptyImgRow，因為原本就是
                    //如果沒圖，目標的row對應的欄位也會保持原本狀態
                    //也就是如果目標row的對應欄位原本有圖，則不會被覆蓋
                    //如果目標row的對應欄位原本沒圖，就保持EmptyImgRow
                    if (sk is SKinRowImg) {
                        SKinRowImg sri = (SKinRowImg)sk;
                        //fileName要用目標的 name
                        inflateImgRow(targetSkName,
                        columnName,rowList[targetIndex],dt.Rows[targetIndex]);
                        //如果有原圖，則整個row要保留，把原本skin對應的cell變成EmptySkinRow
                        if (hasImg){
                            inflateEmptyImgRow(sk.imgName, columnName, rowList[RowIndex], dt.Rows[RowIndex]);
                        }
                        
                    }
                   

                }

                //如果沒原圖，目標row填完後直接刪除，
                if (!hasImg)
                {
                    rowList.RemoveAt(RowIndex);
                    dt.Rows.RemoveAt(RowIndex);
                }
                else { 
                
                dt.Rows[RowIndex].EndEdit();
                }

                dt.Rows[targetIndex].EndEdit();

               // setSelect(0,targetIndex);
            
            }

        }

        private void setSelect(int col ,int row)
        {
            dgv.CurrentCell = dgv.Rows[row].Cells[col];
            dgv.ClearSelection();
            dgv.Rows[row].Cells[col].Selected = true;
        }

        private void ToFolder()
        {
            var RowIndex = getOriginTableCurrentIndex();
            var ColumnIndex = dgv.CurrentCell.ColumnIndex;

            var s = getSkinRow(ColumnIndex, RowIndex);
            if (s is SkinRowEmpty)
            {
                SkinRowEmpty se = (SkinRowEmpty)s;
                FileUtil.OpenFolder(se.folderPath);
            }
            else if (s is SKinRowImg)
            {
                SKinRowImg sri = (SKinRowImg)s;
                FileUtil.OpenFolderAndSelectFile(sri.filePath);
            }

        }


        private void AddWhite()
        {

            var RowIndex = getOriginTableCurrentIndex();
            var ColumnIndex = dgv.CurrentCell.ColumnIndex;

            var s = getSkinRow(ColumnIndex,RowIndex);
            if (s != null && s is SKinRowHeader)
            {
                SKinRowHeader header = (SKinRowHeader)s;
                DialogResult dialogResult = MessageBox.Show($"確定要將{s.imgName}加入白名單嗎?", "加入白名單", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //案同意
                    getWhiteBySrc().Add(header.filePath);
                    saveWhite();
                    bool haveSkin = rowList[RowIndex].Any(key =>
                    //不是 column 0 跟 1 並且 有圖片，代表其中有skin 有對應的圖 
                    !key.Key.Equals("Img") &&
                    !key.Key.Equals("default") &&
                    key.Value is SKinRowImg );
                    //if (haveSkin)
                    //{

                    //    foreach (DataGridViewCell cell in dgv.Rows[dgv.CurrentCell.RowIndex].Cells) {
                    //        cell.Style.BackColor = Color.Green;
                    //    }

                    //    header.isWhite = true;
                    //}
                    //else { 
                       rowList.RemoveAt(RowIndex);
                       dt.Rows.RemoveAt(RowIndex);
                  //  }
                 
                }
            }
        }

        private void CancelWhite()
        {

            var RowIndex = getOriginTableCurrentIndex();
            var ColumnIndex = dgv.CurrentCell.ColumnIndex;

            var s = getSkinRow(ColumnIndex,RowIndex);
            if (s != null && s is SKinRowHeader)
            {
                SKinRowHeader header = (SKinRowHeader)s;
                DialogResult dialogResult = MessageBox.Show($"確定要從白名單內將{s.imgName}取消嗎?", "取消白名單", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //案同意
                    getWhiteBySrc().Remove(header.filePath);
                    saveWhite();
                    header.isWhite = false;

                    //把頭部的顏色取消
                    dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0].Style.BackColor = Color.Empty;

                   
                }
            }
        }

        private void AddImg()
        {

            var RowIndex = getOriginTableCurrentIndex();
            var ColumnIndex = dgv.CurrentCell.ColumnIndex;

            var s = getSkinRow(ColumnIndex, RowIndex);
            var s0 = getSkinRow(0, RowIndex);

            if (//s is SkinRowEmpty &&
                
                s0 is SKinRowHeader)
            {
                String savePath="";
                String name = "";
                if (s is SkinRowEmpty)
                {
                 
                    if (!Directory.Exists(((SkinRowEmpty)s).folderPath))
                    {
                        Directory.CreateDirectory(((SkinRowEmpty)s).folderPath);
                    }
                    savePath = ((SkinRowEmpty)s).folderPath + $@"\{((SkinRowEmpty)s).imgName}";
                    name = ((SkinRowEmpty)s).imgName;
                }
                else if (s is SKinRowImg){
                    savePath = ((SKinRowImg)s).filePath;
                    name = ((SKinRowImg)s).imgName;
                }
                SKinRowHeader srh = s0 as SKinRowHeader;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (curInsertSelectPath != null)
                {

                    openFileDialog.InitialDirectory = curInsertSelectPath;
                }
                else {

                    openFileDialog.InitialDirectory = "c:\\";
                }
                openFileDialog.Filter = "Image files|*.jpg;*.png";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    curInsertSelectPath = Path.GetDirectoryName(filePath);

                    BitmapUtil.ResizeImageAndSave(filePath, savePath, srh.width, srh.height);

                    //更新

                    var columnName = dgv.Columns[ColumnIndex].Name;
                  //  Console.WriteLine($"{ dt.Rows[cell.RowIndex].}")
                    dt.Rows[RowIndex].BeginEdit();
                    
                    inflateImgRow(name, columnName, rowList[RowIndex], dt.Rows[RowIndex]);
                    dt.Rows[RowIndex].EndEdit();

                }
            }

        }

        private void DeleteImg()
        {

            var RowIndex = getOriginTableCurrentIndex();
            var ColumnIndex = dgv.CurrentCell.ColumnIndex;

            var s = getSkinRow(ColumnIndex, RowIndex);
            if (s is SKinRowImg)
            {
                SKinRowImg sri = (SKinRowImg)s;
                DialogResult dialogResult = MessageBox.Show($"確定要將{sri.imgName}移至資源回收桶嗎?", "刪除", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //移到資源回收桶
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(sri.filePath,
                        UIOption.OnlyErrorDialogs,
                        RecycleOption.SendToRecycleBin);

                    var columnName = dgv.Columns[ColumnIndex].Name;

                    //過濾源專案圖 跟 自己 看是否還有其他skin有對應圖，如果沒有就刪除整行
                    bool another = rowList[RowIndex].Where(r =>
                    {
                        return
                        !r.Key.Equals("default") &&
                        !r.Key.Equals(columnName);
                    }).Any(r => { return r.Value is SKinRowImg; });
                    //有其他專案  或 不是白名單就更新  

                    if (another || (getSkinRow(0, RowIndex) is SKinRowHeader
                        && !((SKinRowHeader)getSkinRow(0, RowIndex)).isWhite))
                    {
                        //更新
                        dt.Rows[RowIndex].BeginEdit();
                        inflateEmptyImgRow(sri.imgName, columnName, rowList[RowIndex], dt.Rows[RowIndex]);
                        dt.Rows[RowIndex].EndEdit();
                  

               
                    }
                    else  {

                        rowList.RemoveAt(RowIndex);
                        dt.Rows.RemoveAt(RowIndex);
                    }
               
                }

            }
        }


        private async void AdjAllPixelAsync()
        {

            var RowIndex = getOriginTableCurrentIndex();
            var ColumnIndex = dgv.CurrentCell.ColumnIndex;

            var s = getSkinRow(0, RowIndex);
            if (!(s is SKinRowHeader)) return;

            Dictionary<string, SKinRowImg> sriDictionary = new Dictionary<string, SKinRowImg>();

            SKinRowHeader srh = s as SKinRowHeader;

            for (int i = 2; i < dt.Columns.Count; i++)
            {
                var sr = getSkinRow(i, RowIndex);
                if (sr is SKinRowImg)
                {
                    SKinRowImg sri = sr as SKinRowImg;
                    if (srh.width != sri.width || srh.height != sri.height)
                    {
                        sriDictionary.Add(dgv.Columns[i].Name, sri);
                    }
                }
            }
            if (sriDictionary.Count == 0) return;

            DialogResult dialogResult =
                MessageBox.Show($"確定要將對應的圖片像素\n" +
                $"調整成({srh.width}x{srh.height})?", "調整像素", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Progress p = new Progress(this);
                p.Max(sriDictionary.Count).setText("處理中");
                p.Show();
                int index = 0;
                var t = Task.Factory.StartNew(() =>
                {
                    foreach (KeyValuePair<string, SKinRowImg> key in sriDictionary)
                    {
                        BitmapUtil.ResizeImageAndSave(key.Value.filePath,
                            key.Value.filePath,
                            srh.width,
                            srh.height);
                        this.Invoke((Action)(() =>
                        {
                            index++;
                            p.setProgress(index);

                        }));
                    }
                });

                await t;
                p.Dispose();
                //更新
                dt.Rows[RowIndex].BeginEdit();
                foreach (KeyValuePair<string, SKinRowImg> key in sriDictionary)
                {
                    inflateImgRow(key.Value.imgName,
                       key.Key,
                       rowList[RowIndex],
                       dt.Rows[RowIndex]);
                }
                dt.Rows[RowIndex].EndEdit();

            }
        }



        //調整像素
        private void AdjPixel()
        {
            var RowIndex = getOriginTableCurrentIndex();
            var ColumnIndex = dgv.CurrentCell.ColumnIndex;

            var s = getSkinRow(ColumnIndex, RowIndex);
            var s0 = getSkinRow(0, RowIndex);
            if (s is SKinRowImg && s0 is SKinRowHeader)
            {
                SKinRowImg sri = (SKinRowImg)s;
                SKinRowHeader srh = s0 as SKinRowHeader;
                DialogResult dialogResult =
                    MessageBox.Show($"確定要將{sri.imgName}({sri.width}x{sri.height})的像素\n" +
                    $"調整成({srh.width}x{srh.height})?", "調整像素", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    BitmapUtil.ResizeImageAndSave(sri.filePath, sri.filePath, srh.width, srh.height);

                    //更新
                    var columnName = dgv.Columns[ColumnIndex].Name;
                    dt.Rows[RowIndex].BeginEdit();
                    inflateImgRow(srh.imgName, columnName, rowList[RowIndex], dt.Rows[RowIndex]);
                    dt.Rows[RowIndex].EndEdit();


                }


            }

        }
        //取得完整資料的dataTable所對應的CurrentCell
        //因為 dt.DefaultView.RowFilter 的關係
        private int getOriginTableCurrentIndex() {
            //DataGridViewCell c=null;
            //if (tb_search.TextLength == 0)
            //{
            //    c = dgv.CurrentCell;
            //}
            //else {
            //    var r =dt.DefaultView[dgv.CurrentCell.RowIndex].Row;
            //    var index =dt.Rows.IndexOf(r);
            //    c = dgv[dgv.CurrentCell.ColumnIndex, index];
            //}
            var cell = dgv.CurrentCell;

            return getOriginTableIndex(cell.RowIndex);
        }

        private int getOriginTableIndex(int row)
        {
            if (tb_search.TextLength == 0)
            {
                //Console.WriteLine($"getOriginTableCell[{col},{row}]");
                //c.ColumnIndex = col;
                //c.RowIndex = row;
                return row;
            }
            else
            {
               // var r = ;

              return dt.Rows.IndexOf(dt.DefaultView[row].Row);
               // c.ColumnIndex = col;
              //  c.RowIndex = index;
            }
         //   return c;
        }


        private void tb_search_TextChanged(object sender, EventArgs e)
        {

            if (tb_search.TextLength == 0)
            {

                //清除搜尋
                //  dt = Alldt;
                //  dgv.DataSource = dt;

                //DataView dv = new DataView(dt);
                //dv.RowFilter = null;
                //dt = dv.ToTable();
                // dgv.DataSource = Alldt;

                dt.DefaultView.RowFilter = null;
                // rowList = new List<Dictionary<string, SKinRow>>(allRowList);
            }
            else {

                //搜尋img

                  dt.DefaultView.RowFilter = "Img LIKE '%" + tb_search.Text + "%'";



                //   var newDt = Alldt.Select();


                //DataView dv = new DataView(Alldt);
                //dv.RowFilter = "Img LIKE '%" + tb_search.Text + "%'";
                //dt = dv.ToTable();
                //dgv.DataSource = dt;

                //rowList = allRowList.Where(row =>
                //{
                //    return row["Img"] is SKinRowHeader && ((SKinRowHeader)row["Img"]).showText.Contains(tb_search.Text);
                //}).ToList();


                //if (newDt.Length == 0) {
                //    dt = Alldt.Clone();
                //}
                //else {

                //    dt = newDt.CopyToDataTable();
                //}

                //dgv.DataSource = dt;


            }
            dgv.ClearSelection();
        }

        private void pb_refresh_Click(object sender, EventArgs e)
        {
            LoadFolderData();
        }


        private void menu_file_open_color_Click(object sender, EventArgs e)
        {
            ColorCheck cc = new ColorCheck(this,selectSkinProjectPath,selectRootProjectPath);
            cc.Show();
        }

        private void menu_build_skin_Click(object sender, EventArgs e)
        {
            BuildSkin buildSkin = new BuildSkin(selectSkinProjectPath);
            buildSkin.Show();

        }
    }


}
