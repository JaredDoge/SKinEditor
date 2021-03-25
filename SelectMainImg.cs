using SKinEditer.util;
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
    public partial class SelectMainImg : Form
    {
        private List<string> filePath;

        private string _SelectPath;

        private bool _IsCover;

        public string SelectPath { get { return _SelectPath; } } 



        ImageList listImg = new ImageList();
        public SelectMainImg(List<string> filePath)

        {
            InitializeComponent();
            this.filePath = filePath;

            DoubleBuffer.DoubleBufferedListView(lv);
        }


        private void lv_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SelectMainImg_Load(object sender, EventArgs e)
        {
            Init();
            LoadListView(filePath);

            tb_search.TextChanged += new EventHandler(Tb_search_TextChange);
        }

        private void Init()
        {
            lv.MultiSelect = false;
            lv.View = View.LargeIcon;

            listImg.ImageSize = new Size(100, 100);

            lv.LargeImageList = listImg;
            foreach (string path in filePath)
            {
                var img = Image.FromFile(path);
                listImg.Images.Add(img);
            }
        }

        private void Tb_search_TextChange(object sender, EventArgs e)
        {
            LoadListView(filePath.Where(path => {
                return path.Contains(tb_search.Text);
            }).ToList()); 

        }

        private void LoadListView(List<string> files)
        {

            lv.BeginUpdate();
            lv.Items.Clear();
            foreach (string path in files)
            {
                var lvi = new ListViewItem();
                lvi.ImageIndex = filePath.IndexOf(path);
                lvi.Text = Path.GetFileName(path);
                lvi.Name = path;
                lv.Items.Add(lvi);
            }

            lv.EndUpdate();
        }

        private void btn_cencel_Click(object sender, EventArgs e)
        {


            DialogResult = DialogResult.Cancel;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            //沒有選擇東西
            if (lv.SelectedItems.Count <= 0) {
                MessageBox.Show("請選擇要對應的主專案圖片");
                return;
            }

            _SelectPath= lv.SelectedItems[0].Name;

            DialogResult = DialogResult.OK;


        }
    }
}
