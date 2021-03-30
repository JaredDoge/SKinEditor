
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
    public partial class BuildSkin : Form
    {
        private string selectSkinProjectPath;

        private bool openFolder=true;

        private bool success = false;


        public BuildSkin(string selectSkinProjectPath)
        {
            InitializeComponent();
            this.selectSkinProjectPath = selectSkinProjectPath;
        }

        private  void BuildSkin_Load(object sender, EventArgs e)
        {
           richTextBox1.ReadOnly = true;


           
           
           // string line;

            //    p.StandardInput.WriteLine("gradlew clean");
            //while ((line = p.StandardOutput.ReadLine()) != null)
            //{

            //    Console.WriteLine("資料");
            //    Console.WriteLine(line);
            //}

            //   Console.WriteLine("帶G狀態前等太");
            //    p.WaitForExit();
            //     Console.WriteLine("帶G狀態");

           // p.StandardInput.WriteLine("gradlew clean build");
            

       //     Console.WriteLine(str);
        //    while ((line = p.StandardOutput.ReadLine()) != null || (line = p.StandardError.ReadLine()) != null)
        //    {
         //       Console.WriteLine(line);
          //  }



          //  p.WaitForExit();
       //     p.Close();


         //   Console.ReadKey();
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {

            btn_start.Enabled = false;
            richTextBox1.Text = "";
            cb_open_folder.Enabled = false;
            cb_clear.Enabled = false;
            
        
            var t = Task.Run(() => {

                if (cb_clear.Checked && Directory.Exists($@"{selectSkinProjectPath}\skin"))
                {

                    foreach (var file in Directory.GetFiles($@"{selectSkinProjectPath}\skin")) {
                        File.Delete(file);
                    }
                }

                Process p = new Process();

                p.StartInfo.FileName = "cmd.exe";

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true; //不跳出cmd視窗
                p.StartInfo.WorkingDirectory = $@"{selectSkinProjectPath}";
                if (cb_clear.Checked) {

                    p.StartInfo.Arguments = "clean assembleRelease";
                }
                else
                {
                    p.StartInfo.Arguments = "assembleRelease";

                }
                p.StartInfo.FileName = $@"{selectSkinProjectPath}/gradlew.bat";

                

                p.Start();

                //   p.StandardInput.WriteLine($@"cd /d {selectSkinProjectPath}");
                //  p.StandardInput.WriteLine("gradlew clean assembleDebug");

                p.StandardInput.WriteLine("exit");
                p.OutputDataReceived += ((sd, ev) =>
                {
                    string data = ev.Data;
                    if (data == null) {

                        return;
                    }

                    if (data.Contains("BUILD SUCCESSFUL")) {
                        success = true;
                    }

                    data += "\r\n";

                    this.Invoke((Action)(() =>
                    {
                        richTextBox1.AppendText(data);
                        richTextBox1.ScrollToCaret();
                    }));

                });

                p.ErrorDataReceived += ((sd, ev) =>
                {
                    string data = ev.Data;
                    if (data == null)
                    {
                        return;
                    }

                    data += "\r\n";
                    this.Invoke((Action)(() =>
                    {
                        richTextBox1.AppendText(data);
                        richTextBox1.ScrollToCaret();
                    }));
                });

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();

                p.Close();
            });

            await t;

            if (success)
            {
                if (MessageBox.Show("編譯成功", "提示", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    if (openFolder)
                    {

                        FileUtil.OpenFolder($@"{selectSkinProjectPath}\skin");
                    }
                }

            }
            else {
                MessageBox.Show("編譯失敗");

                btn_start.Enabled = true;
                cb_open_folder.Enabled = true;
                cb_clear.Enabled = true;
            }


        }

        private void cb1_CheckedChanged(object sender, EventArgs e)
        {
            openFolder = cb_open_folder.Checked;
        }
    }
}
