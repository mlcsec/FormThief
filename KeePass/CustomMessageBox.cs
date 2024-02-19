using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeePass
{
    public partial class CustomMessageBox : Form
    {
        private PictureBox iconPictureBox;
        private Label messageLabel;

        public CustomMessageBox(string message, string caption, MessageBoxIcon iconType)
        {
            InitializeCustomMessageBox(message, caption, iconType);
        }

        private void InitializeCustomMessageBox(string message, string caption, MessageBoxIcon iconType)
        {
            this.Text = caption;
            this.ShowInTaskbar = false; // Hide the taskbar icon
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 8, FontStyle.Regular);
            this.Size = new Size(395, 214);

            // Add icon
            this.iconPictureBox = new PictureBox();
            this.iconPictureBox.Location = new Point(20, 20);
            this.iconPictureBox.Size = new Size(32, 32);
            this.iconPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(this.iconPictureBox);

            // Set icon based on MessageBoxIcon
            // - these are the default Windows ones, KeePass appears to use a slightly mofified version which i've included as a resource below
            switch (iconType)
            {
                case MessageBoxIcon.Information:
                    this.iconPictureBox.Image = SystemIcons.Information.ToBitmap();
                    break;
                case MessageBoxIcon.Warning:
                    this.iconPictureBox.Image = SystemIcons.Warning.ToBitmap();
                    break;
                case MessageBoxIcon.Error:
                    this.iconPictureBox.Image = SystemIcons.Error.ToBitmap();
                    //this.iconPictureBox.Image = Properties.Resources.
                    break;
                case MessageBoxIcon.Question:
                    this.iconPictureBox.Image = SystemIcons.Question.ToBitmap();
                    break;
                default:
                    break;
            }

            // Add custom panel with background color near bottom of window
            Panel lowerPanel = new Panel();
            lowerPanel.BackColor = Color.WhiteSmoke; 
            lowerPanel.Location = new Point(0, 130); 
            lowerPanel.Size = new Size(this.Width, this.Height - 100); // set dize to fill lower portion of window
            this.Controls.Add(lowerPanel);

            // Add message label
            this.messageLabel = new Label();
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new Point(60, 20);
            this.messageLabel.Text = message;
            this.Controls.Add(this.messageLabel);

            // Add OK button
            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.BackColor = Color.WhiteSmoke;
            okButton.Location = new Point(290, 140);
            okButton.Click += buttonOK_Click; 
            this.Controls.Add(okButton);
            okButton.BringToFront();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}