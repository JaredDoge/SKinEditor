using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKinEditer
{
    public partial class Progress : Form
    {

        private Form parent;
        public Progress(Form parent)
        {
            
            InitializeComponent();
            this.parent = parent;
            FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        public Progress setProgress(int i) {

            if (i > progressBar1.Maximum)
            {

                progressBar1.Value = progressBar1.Maximum;
            }

            if (i < 0) {

                progressBar1.Value = 0;
            }

            progressBar1.Value = i;
            return this;
        }

        public Progress setText(string s) {
            label1.Text = s;
            return this;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        public Progress Plus(int count=1) {
            setProgress(progressBar1.Value + count);
            return this;
        }


        public Progress Max(int i) {
            progressBar1.Maximum = i;
            return this;
        }

        private void Progress_Load(object sender, EventArgs e)
        {

            StartPosition = FormStartPosition.CenterParent;
            ControlBox = false;
            progressBar1.Maximum = 100;
        
            var x = parent.Location.X + (parent.Width - Width) / 2;
            var y = parent.Location.Y + (parent.Height - Height) / 2;
            Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
            
        }
    }
}
