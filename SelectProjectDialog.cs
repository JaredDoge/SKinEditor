using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKinEditer
{
    public partial class SelectProjectDialog : Form
    {

        private String _RootPath;

        private String _SkinPath;

        public string rootPath {
            get { return _RootPath; }
        }
        public string skinPath
        {
            get { return _SkinPath; }
        }

        public SelectProjectDialog()
        {
            InitializeComponent();
            Load += new EventHandler(load);
            
        }

      

        private void load(object sender, EventArgs e)
        {

            tb_root.Text = Properties.Settings.Default.rootPath;
            tb_skin.Text = Properties.Settings.Default.skinPath;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();

            tb_root.Text = path.SelectedPath;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            tb_skin.Text = path.SelectedPath;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (tb_root.Text.Length==0 || tb_skin.Text.Length == 0) {
                MessageBox.Show("請選擇資料夾目錄");
                return;         
            }
            if (!Directory.Exists(tb_root.Text)) {
                MessageBox.Show("原專案資料夾不存在!");
                return;
            }
            if (!Directory.Exists(tb_skin.Text))
            {
                MessageBox.Show("skin專案資料夾不存在!");
                return;
            }


            _RootPath = tb_root.Text;
            _SkinPath = tb_skin.Text;
            Properties.Settings.Default.rootPath = _RootPath;
            Properties.Settings.Default.skinPath = _SkinPath;
            Properties.Settings.Default.Save();

            DialogResult = DialogResult.OK;

        }
    }
}
