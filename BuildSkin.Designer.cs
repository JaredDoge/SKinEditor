

namespace SKinEditer
{
    partial class BuildSkin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cb_open_folder = new System.Windows.Forms.CheckBox();
            this.btn_start = new System.Windows.Forms.Button();
            this.cb_clear = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(603, 534);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // cb_open_folder
            // 
            this.cb_open_folder.AutoSize = true;
            this.cb_open_folder.Checked = true;
            this.cb_open_folder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_open_folder.Location = new System.Drawing.Point(12, 552);
            this.cb_open_folder.Name = "cb_open_folder";
            this.cb_open_folder.Size = new System.Drawing.Size(120, 16);
            this.cb_open_folder.TabIndex = 1;
            this.cb_open_folder.Text = "完成後開啟資料夾";
            this.cb_open_folder.UseVisualStyleBackColor = true;
            this.cb_open_folder.CheckedChanged += new System.EventHandler(this.cb1_CheckedChanged);
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(539, 581);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(75, 23);
            this.btn_start.TabIndex = 2;
            this.btn_start.Text = "開始";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // cb_clear
            // 
            this.cb_clear.AutoSize = true;
            this.cb_clear.Location = new System.Drawing.Point(12, 574);
            this.cb_clear.Name = "cb_clear";
            this.cb_clear.Size = new System.Drawing.Size(224, 16);
            this.cb_clear.TabIndex = 3;
            this.cb_clear.Text = "清除輸出資料夾後編譯(編譯時間較長)";
            this.cb_clear.UseVisualStyleBackColor = true;
            // 
            // BuildSkin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 616);
            this.Controls.Add(this.cb_clear);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.cb_open_folder);
            this.Controls.Add(this.richTextBox1);
            this.Name = "BuildSkin";
            this.Text = "BuildSkin";
            this.Load += new System.EventHandler(this.BuildSkin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox cb_open_folder;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.CheckBox cb_clear;
    }
}