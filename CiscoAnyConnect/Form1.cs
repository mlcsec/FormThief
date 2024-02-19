using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace CiscoAnyConnect
{
    public partial class Form1 : Form
    {

        private string logFilePath = Path.Combine(Path.GetTempPath(), "abckdlfdpeoovnp.bin");
        private int loginAttempts = 0;
        private int clickCount = 0;
        private string commandLineArg;

        public Form1(string commandLineArg)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.commandLineArg = commandLineArg;
            comboBox1.Text = commandLineArg; // VPN endpoint
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 20, Screen.PrimaryScreen.WorkingArea.Height - this.Height - 20);

            // run cred window
            //Task.Run(() => ShowLoginForm());
            //this.Enabled = false;

            // or this..
            //
            Form2 form2 = new Form2(commandLineArg);
            form2.FormClosed += Form2_FormClosed;
            form2.Show(this);
            this.Enabled = false;

        }
        /*
        private void ShowLoginForm()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowLoginForm));
                return;
            }

            using (Form2 form2 = new Form2(commandLineArg))
            {
                form2.FormClosed += (sender, e) => this.Dispose();
                form2.ShowDialog(this);
            }
        }*/


        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Enabled = true;
            this.Dispose();
            // check this may error...
        }
    }
}
