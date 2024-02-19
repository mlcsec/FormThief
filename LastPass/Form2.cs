using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LastPass
{
    public partial class Form2 : Form
    {
        public Form2(string message)
        {
            InitializeComponent();

            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.ResizeRedraw = false;
            this.Cursor = Cursors.Default;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Size = new Size(345, 195);
            this.AutoSize = false;
            this.Resize += Form1_Resize;
            this.AutoScaleMode = AutoScaleMode.None;
            button1.Location = new Point(button1.Location.X, button1.Location.Y + 20);
            button1.BackColor = Color.LightGray;

            Label labelMessage = new Label();
            labelMessage.Text = message;
            labelMessage.AutoSize = true;
            labelMessage.Font = new Font("Microsoft Tai Le", 11, FontStyle.Regular);
            labelMessage.Location = new Point(20, 15);
            this.Controls.Add(labelMessage);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Cancel the resizing attempt
            this.Size = new Size(350, 195);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
