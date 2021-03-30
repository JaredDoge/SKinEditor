
namespace SKinEditer
{
    partial class ColorCheck
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
            this.lb_time = new System.Windows.Forms.Label();
            this.btn_next = new System.Windows.Forms.Button();
            this.cb_all = new System.Windows.Forms.CheckBox();
            this.clb = new System.Windows.Forms.CheckedListBox();
            this.gb_1 = new System.Windows.Forms.GroupBox();
            this.p_label = new System.Windows.Forms.Panel();
            this.lb_prompt = new System.Windows.Forms.Label();
            this.gb_2 = new System.Windows.Forms.GroupBox();
            this.lb_update = new System.Windows.Forms.Label();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.gb_1.SuspendLayout();
            this.p_label.SuspendLayout();
            this.gb_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_time
            // 
            this.lb_time.AutoSize = true;
            this.lb_time.Location = new System.Drawing.Point(12, 629);
            this.lb_time.Name = "lb_time";
            this.lb_time.Size = new System.Drawing.Size(33, 12);
            this.lb_time.TabIndex = 1;
            this.lb_time.Text = "label1";
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(581, 624);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(75, 23);
            this.btn_next.TabIndex = 2;
            this.btn_next.Text = "下一步";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // cb_all
            // 
            this.cb_all.AutoSize = true;
            this.cb_all.Location = new System.Drawing.Point(588, 90);
            this.cb_all.Name = "cb_all";
            this.cb_all.Size = new System.Drawing.Size(48, 16);
            this.cb_all.TabIndex = 2;
            this.cb_all.Text = "全選";
            this.cb_all.UseVisualStyleBackColor = true;
            // 
            // clb
            // 
            this.clb.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.clb.FormattingEnabled = true;
            this.clb.Location = new System.Drawing.Point(6, 112);
            this.clb.Name = "clb";
            this.clb.Size = new System.Drawing.Size(630, 488);
            this.clb.TabIndex = 1;
            // 
            // gb_1
            // 
            this.gb_1.Controls.Add(this.p_label);
            this.gb_1.Controls.Add(this.cb_all);
            this.gb_1.Controls.Add(this.clb);
            this.gb_1.Location = new System.Drawing.Point(14, 12);
            this.gb_1.Name = "gb_1";
            this.gb_1.Size = new System.Drawing.Size(642, 606);
            this.gb_1.TabIndex = 3;
            this.gb_1.TabStop = false;
            this.gb_1.Text = "step 1";
            // 
            // p_label
            // 
            this.p_label.Controls.Add(this.lb_prompt);
            this.p_label.Location = new System.Drawing.Point(6, 21);
            this.p_label.Name = "p_label";
            this.p_label.Size = new System.Drawing.Size(630, 59);
            this.p_label.TabIndex = 0;
            // 
            // lb_prompt
            // 
            this.lb_prompt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_prompt.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lb_prompt.Location = new System.Drawing.Point(0, 0);
            this.lb_prompt.Name = "lb_prompt";
            this.lb_prompt.Size = new System.Drawing.Size(630, 59);
            this.lb_prompt.TabIndex = 0;
            this.lb_prompt.Text = "提示";
            // 
            // gb_2
            // 
            this.gb_2.Controls.Add(this.lb_update);
            this.gb_2.Controls.Add(this.rtb);
            this.gb_2.Location = new System.Drawing.Point(14, 12);
            this.gb_2.Name = "gb_2";
            this.gb_2.Size = new System.Drawing.Size(642, 606);
            this.gb_2.TabIndex = 3;
            this.gb_2.TabStop = false;
            this.gb_2.Text = "step 2";
            // 
            // lb_update
            // 
            this.lb_update.AutoSize = true;
            this.lb_update.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lb_update.Location = new System.Drawing.Point(7, 18);
            this.lb_update.Name = "lb_update";
            this.lb_update.Size = new System.Drawing.Size(165, 19);
            this.lb_update.TabIndex = 1;
            this.lb_update.Text = "下列SKIN將會更新";
            // 
            // rtb
            // 
            this.rtb.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rtb.Location = new System.Drawing.Point(6, 40);
            this.rtb.Name = "rtb";
            this.rtb.Size = new System.Drawing.Size(627, 560);
            this.rtb.TabIndex = 0;
            this.rtb.Text = "";
            // 
            // ColorCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 659);
            this.Controls.Add(this.gb_2);
            this.Controls.Add(this.gb_1);
            this.Controls.Add(this.btn_next);
            this.Controls.Add(this.lb_time);
            this.Name = "ColorCheck";
            this.Text = "ColorCheck";
            this.Load += new System.EventHandler(this.ColorCheck_LoadAsync);
            this.gb_1.ResumeLayout(false);
            this.gb_1.PerformLayout();
            this.p_label.ResumeLayout(false);
            this.gb_2.ResumeLayout(false);
            this.gb_2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lb_time;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.CheckBox cb_all;
        private System.Windows.Forms.CheckedListBox clb;
        private System.Windows.Forms.GroupBox gb_1;
        private System.Windows.Forms.Panel p_label;
        private System.Windows.Forms.Label lb_prompt;
        private System.Windows.Forms.GroupBox gb_2;
        private System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.Label lb_update;
    }
}