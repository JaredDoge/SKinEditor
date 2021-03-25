using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKinEditer
{
    public partial class BuildSkin : Form
    {
        private string selectSkinProjectPath;

        public BuildSkin(string selectSkinProjectPath)
        {
            InitializeComponent();
            this.selectSkinProjectPath = selectSkinProjectPath;
        }

        private void BuildSkin_Load(object sender, EventArgs e)
        {
            Process p = new Process();
            String str = null;

            p.StartInfo.FileName = "cmd.exe";

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
           // p.StartInfo.CreateNoWindow = true; //不跳出cmd視窗

            p.Start();
            p.StandardInput.WriteLine($@"cd /d {selectSkinProjectPath}");
            p.StandardInput.WriteLine("gradlew assembleDebug");


       //     Console.WriteLine(str);
            string line;

            while ((line = p.StandardOutput.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
            p.WaitForExit();
            p.Close();


         //   Console.ReadKey();
        }
    }
}
