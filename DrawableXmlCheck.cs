using SKinEditer.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKinEditer
{
    public partial class DrawableXmlCheck : Form
    {
        private string selectProject;

        private string rootProject;

        private Form parent;
        public DrawableXmlCheck(Form parent,string selectProject,string rootProject)
        {
            InitializeComponent();
            this.parent = parent;
            this.selectProject = selectProject;
            this.rootProject = rootProject;
            FormClosing += new FormClosingEventHandler(_FormClosing);
        }

        private void _FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        public void setText(string s)
        {
            lb_text.Text = s;
        }
        public void Plus(int count = 1)
        {
            setProgress(pb.Value + count);
        }


        public void setProgress(int i)
        {
            if (i > pb.Maximum)
            {
                pb.Value = pb.Maximum;
            }

            if (i < 0)
            {
                pb.Value = 0;
            }
            pb.Value = i;
        }

        public void Max(int i)
        {
            pb.Maximum = i;
        }


        private  void DrawableXmlCheck_Load(object sender, EventArgs e)
        {
            //
            StartPosition = FormStartPosition.CenterParent;
            ControlBox = false;
            pb.Maximum = 100;

            var x = parent.Location.X + (parent.Width - Width) / 2;
            var y = parent.Location.Y + (parent.Height - Height) / 2;
            Location = new Point(Math.Max(x, 0), Math.Max(y, 0));

            panel1.Controls.Add(label1);

            label1.Text = "*各個skin專案的drawable資料夾會完全清空，並把主專案裡在drawable資料夾下的XML檔，複製到各個skin專案的drawable下";

            

       

        }

        private async void btn_ok_Click(object sender, EventArgs e)
        {
            btn_ok.Enabled = false;
            btn_close.Enabled = false;

            var t = Task.Run(() =>
            {

                //所有skin
                List<string> skinProject =
                  Directory.GetDirectories($@"{selectProject}")
                  .Where(path =>
                  {
                      Boolean isApp = FileUtil.CheckIsAndroidApplication(path);
                      string folderName = Path.GetFileNameWithoutExtension(path);
                      return isApp && !folderName.Equals("app");
                  }).Select(path => Path.GetFileNameWithoutExtension(path)).ToList();


                //
                List<string> xmls = new List<string>();
                //主專案下資料夾
                foreach (string path in Directory.GetDirectories($@"{rootProject}"))
                {
                    
                    string[] src = Directory.GetDirectories(path, "src", System.IO.SearchOption.TopDirectoryOnly);
                    //該資料夾下如果有src這個資料夾
                    foreach (string s in src)
                    {
                        string folder = s + $@"\main\res\drawable";

                        if (Directory.Exists(folder))
                        {
                            var swOrigin = Stopwatch.StartNew();

                            var o = Directory.GetFiles($"{folder}", "*.xml", SearchOption.AllDirectories);

                            
                     
                            xmls.AddRange(o);

                            swOrigin.Stop();
                            Console.WriteLine($"找原專案{s}:" + swOrigin.ElapsedMilliseconds);


                        }
                    }
                }

                this.Invoke((Action)(() =>
                {
                    setText("正在移除drawable");

                    Max(skinProject.Count * xmls.Count);

                }));
                //刪除所有資料
                foreach (string skin in skinProject)
                {
                    string path = $@"{selectProject}\{skin}\src\main\res\drawable";
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }

                }


                //開始檢查檔案
                foreach (string skin in skinProject)
                {

                    var targetFolder = $@"{selectProject}\{skin}\src\main\res\drawable";

                    if (!Directory.Exists(targetFolder))
                    {
                        //drawable不在就創一個壓壓驚
                        Directory.CreateDirectory(targetFolder);
                    }

                    foreach (string xml in xmls)
                    {

                        //原專案檔名
                        var rootFileName = Path.GetFileName(xml);
                        //目標檔名
                        var destFile = Path.Combine(targetFolder, rootFileName);

                        this.Invoke((Action)(() =>
                        {
                            setText($@"正在複製{rootFileName}到{skin}");
                            lb_progress.Text = $"{pb.Value}/{pb.Maximum}";
                        }));

                        File.Copy(xml, destFile, true);

                        this.Invoke((Action)(() =>
                        {
                            Plus();

                        }));

                    }
                }

                this.Invoke((Action)(() =>
                {
                    setText("完成");
                    lb_progress.Text = $"{pb.Value}/{pb.Maximum}";
                }));

            });

            await t;

            btn_close.Enabled = true;

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
