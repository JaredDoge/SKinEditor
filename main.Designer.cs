
namespace SKinEditer
{
    partial class main
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.contextMenuStripRight = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.前往資料夾ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加入圖片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刪除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.同步像素ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lable_skin_src = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_open_project = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_check_xml = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_check_color = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_build_skin = new System.Windows.Forms.ToolStripMenuItem();
            this.cb_src = new System.Windows.Forms.ComboBox();
            this.contextMenuStripHeader = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.加入白名單ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.調整所有圖片像素ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.將所有SKIN圖片移至ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_search = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pb_refresh = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.contextMenuStripRight.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStripHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_refresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(0, 88);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(1030, 631);
            this.dgv.TabIndex = 0;
            this.dgv.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellMouseClick);
            // 
            // contextMenuStripRight
            // 
            this.contextMenuStripRight.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.前往資料夾ToolStripMenuItem,
            this.加入圖片ToolStripMenuItem,
            this.刪除ToolStripMenuItem,
            this.同步像素ToolStripMenuItem});
            this.contextMenuStripRight.Name = "contextMenuStripRight";
            this.contextMenuStripRight.Size = new System.Drawing.Size(135, 92);
            // 
            // 前往資料夾ToolStripMenuItem
            // 
            this.前往資料夾ToolStripMenuItem.Name = "前往資料夾ToolStripMenuItem";
            this.前往資料夾ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.前往資料夾ToolStripMenuItem.Text = "前往資料夾";
            // 
            // 加入圖片ToolStripMenuItem
            // 
            this.加入圖片ToolStripMenuItem.Name = "加入圖片ToolStripMenuItem";
            this.加入圖片ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.加入圖片ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.加入圖片ToolStripMenuItem.Text = "新增";
            // 
            // 刪除ToolStripMenuItem
            // 
            this.刪除ToolStripMenuItem.Name = "刪除ToolStripMenuItem";
            this.刪除ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.刪除ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.刪除ToolStripMenuItem.Text = "刪除";
            // 
            // 同步像素ToolStripMenuItem
            // 
            this.同步像素ToolStripMenuItem.Name = "同步像素ToolStripMenuItem";
            this.同步像素ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.同步像素ToolStripMenuItem.Text = "調整像素";
            // 
            // lable_skin_src
            // 
            this.lable_skin_src.AutoSize = true;
            this.lable_skin_src.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lable_skin_src.Location = new System.Drawing.Point(12, 39);
            this.lable_skin_src.Name = "lable_skin_src";
            this.lable_skin_src.Size = new System.Drawing.Size(92, 16);
            this.lable_skin_src.TabIndex = 1;
            this.lable_skin_src.Text = "目標資料夾:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1030, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_file_open_project,
            this.menu_file_check_xml,
            this.menu_file_check_color,
            this.menu_file_build_skin});
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(46, 21);
            this.ToolStripMenuItem.Text = "檔案";
            // 
            // menu_file_open_project
            // 
            this.menu_file_open_project.Name = "menu_file_open_project";
            this.menu_file_open_project.Size = new System.Drawing.Size(180, 22);
            this.menu_file_open_project.Text = "開啟專案";
            this.menu_file_open_project.Click += new System.EventHandler(this.menu_file_open_project_Click);
            // 
            // menu_file_check_xml
            // 
            this.menu_file_check_xml.Name = "menu_file_check_xml";
            this.menu_file_check_xml.Size = new System.Drawing.Size(180, 22);
            this.menu_file_check_xml.Text = "檢查XML(SVG)";
            this.menu_file_check_xml.Click += new System.EventHandler(this.menu_file_add_img_Click);
            // 
            // menu_file_check_color
            // 
            this.menu_file_check_color.Name = "menu_file_check_color";
            this.menu_file_check_color.Size = new System.Drawing.Size(180, 22);
            this.menu_file_check_color.Text = "檢查Color";
            this.menu_file_check_color.Click += new System.EventHandler(this.menu_file_open_color_Click);
            // 
            // menu_file_build_skin
            // 
            this.menu_file_build_skin.Name = "menu_file_build_skin";
            this.menu_file_build_skin.Size = new System.Drawing.Size(180, 22);
            this.menu_file_build_skin.Text = "輸出SKIN";
            this.menu_file_build_skin.Click += new System.EventHandler(this.menu_build_skin_Click);
            // 
            // cb_src
            // 
            this.cb_src.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_src.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cb_src.FormattingEnabled = true;
            this.cb_src.Location = new System.Drawing.Point(101, 34);
            this.cb_src.Name = "cb_src";
            this.cb_src.Size = new System.Drawing.Size(121, 21);
            this.cb_src.TabIndex = 5;
            this.cb_src.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // contextMenuStripHeader
            // 
            this.contextMenuStripHeader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.加入白名單ToolStripMenuItem,
            this.調整所有圖片像素ToolStripMenuItem,
            this.將所有SKIN圖片移至ToolStripMenuItem});
            this.contextMenuStripHeader.Name = "contextMenuStripHeader";
            this.contextMenuStripHeader.Size = new System.Drawing.Size(186, 70);
            // 
            // 加入白名單ToolStripMenuItem
            // 
            this.加入白名單ToolStripMenuItem.Name = "加入白名單ToolStripMenuItem";
            this.加入白名單ToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.加入白名單ToolStripMenuItem.Text = "加入白名單";
            // 
            // 調整所有圖片像素ToolStripMenuItem
            // 
            this.調整所有圖片像素ToolStripMenuItem.Name = "調整所有圖片像素ToolStripMenuItem";
            this.調整所有圖片像素ToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.調整所有圖片像素ToolStripMenuItem.Text = "調整所有圖片像素";
            // 
            // 將所有SKIN圖片移至ToolStripMenuItem
            // 
            this.將所有SKIN圖片移至ToolStripMenuItem.Name = "將所有SKIN圖片移至ToolStripMenuItem";
            this.將所有SKIN圖片移至ToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.將所有SKIN圖片移至ToolStripMenuItem.Text = "將所有SKIN圖片移至";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(286, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "圖片像素與主專案不同";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(437, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "在白名單內，但SKIN有對應的圖";
            // 
            // tb_search
            // 
            this.tb_search.Location = new System.Drawing.Point(62, 61);
            this.tb_search.Name = "tb_search";
            this.tb_search.Size = new System.Drawing.Size(306, 22);
            this.tb_search.TabIndex = 10;
            this.tb_search.TextChanged += new System.EventHandler(this.tb_search_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "搜尋:";
            // 
            // pb_refresh
            // 
            this.pb_refresh.Cursor = System.Windows.Forms.Cursors.Default;
            this.pb_refresh.Image = global::SKinEditer.Properties.Resources.round_refresh_black_24dp;
            this.pb_refresh.Location = new System.Drawing.Point(228, 34);
            this.pb_refresh.Name = "pb_refresh";
            this.pb_refresh.Size = new System.Drawing.Size(21, 21);
            this.pb_refresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_refresh.TabIndex = 12;
            this.pb_refresh.TabStop = false;
            this.pb_refresh.WaitOnLoad = true;
            this.pb_refresh.Click += new System.EventHandler(this.pb_refresh_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Green;
            this.pictureBox2.Location = new System.Drawing.Point(417, 41);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(14, 14);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Red;
            this.pictureBox1.Location = new System.Drawing.Point(266, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(14, 14);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1030, 708);
            this.Controls.Add(this.pb_refresh);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_search);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cb_src);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lable_skin_src);
            this.Controls.Add(this.dgv);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "main";
            this.Text = "SKIN編輯器";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.contextMenuStripRight.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStripHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_refresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRight;
        private System.Windows.Forms.ToolStripMenuItem 前往資料夾ToolStripMenuItem;
        private System.Windows.Forms.Label lable_skin_src;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menu_file_open_project;
        private System.Windows.Forms.ComboBox cb_src;
        private System.Windows.Forms.ToolStripMenuItem menu_file_check_xml;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripHeader;
        private System.Windows.Forms.ToolStripMenuItem 加入白名單ToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem 加入圖片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刪除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 同步像素ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 調整所有圖片像素ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 將所有SKIN圖片移至ToolStripMenuItem;
        private System.Windows.Forms.TextBox tb_search;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pb_refresh;
        private System.Windows.Forms.ToolStripMenuItem menu_file_check_color;
        private System.Windows.Forms.ToolStripMenuItem menu_file_build_skin;
    }
}

