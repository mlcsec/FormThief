using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace OpenVPN
{
    public partial class SplashForm : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private TextBox textBox1;
        private string commandLineArg;

        public SplashForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            Task.Run(() => ShowLoginForm());
            //Thread loginThread = new Thread(() => ShowLoginForm());
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout(); // user can't interact with log window


            // button1
            this.button1.Location = new System.Drawing.Point(18, 303);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 26);
            this.button1.Text = "Disconnect";
            this.button1.UseVisualStyleBackColor = true;

            // button2
            this.button2.Location = new System.Drawing.Point(145, 303);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 26);
            this.button2.Text = "Reconnect";
            this.button2.UseVisualStyleBackColor = true;

            // button3
            this.button3.Location = new System.Drawing.Point(436, 303);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 26);
            this.button3.Text = "Hide";
            this.button3.UseVisualStyleBackColor = true;

            // richTextBox1
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(385, 278);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(151, 19);
            this.richTextBox1.Text = "OpenVPN GUI 11.46.0.0/2.6.8";

            // richTextBox2
            this.richTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.richTextBox2.Location = new System.Drawing.Point(18, 27);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(530, 221);
            string formattedDateTime = DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy");
            string originalText = @"{DateTimePlaceholder} WARNING: Compression for receiving enabled. Compression has been used in the past to break encryption. Sent packets are not compressed unless ""allow-compression yes"" is also set.
{DateTimePlaceholder} DEPRECATED OPTION: --cipher set to 'AES-256-CBC' but missing in --data-ciphers (AES-256-GCM:AES-128-GCM). OpenVPN ignores --cipher for cipher negotiations. 
{DateTimePlaceholder} Note: '--allow-compression' is not set to 'no', disabling data channel offload.
{DateTimePlaceholder} Flag 'def1' added to --redirect-gateway (iservice is in use)
{DateTimePlaceholder} OpenVPN 2.6.8 [git:v2.6.8/3b0d9489cc423da3] Windows [SSL (OpenSSL)] [LZO] [LZ4] [PKCS11] [AEAD] [DCO] built on Nov 17 2023
{DateTimePlaceholder} Windows version 10.0 (Windows 10 or greater), amd64 executable
{DateTimePlaceholder} library versions: OpenSSL 3.1.4 24 Oct 2023, LZO 2.10
{DateTimePlaceholder} DCO version: 1.0.0
{DateTimePlaceholder} MANAGEMENT: TCP Socket listening on [AF_INET]127.0.0.1:25342
{DateTimePlaceholder} Need hold release from management interface, waiting...
{DateTimePlaceholder} MANAGEMENT: Client connected from [AF_INET]127.0.0.1:53453
{DateTimePlaceholder} MANAGEMENT: CMD 'state on'
{DateTimePlaceholder} MANAGEMENT: CMD 'log on all'
{DateTimePlaceholder} MANAGEMENT: CMD 'echo on all'
{DateTimePlaceholder} MANAGEMENT: CMD 'bytecount 5'
{DateTimePlaceholder} MANAGEMENT: CMD 'state'
{DateTimePlaceholder} MANAGEMENT: CMD 'hold off'
{DateTimePlaceholder} MANAGEMENT: CMD 'hold release'
";
            string newText = originalText.Replace("{DateTimePlaceholder}", formattedDateTime);
            this.richTextBox2.Text = newText;
            this.richTextBox2.Font = new Font("Microsoft Sans Serif", 8f);
            this.richTextBox2.WordWrap = false;
            this.richTextBox2.SelectionStart = this.richTextBox2.Text.Length;
            this.richTextBox2.ScrollToCaret();

            // textBox1
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(18, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(134, 13);
            this.textBox1.Text = "Current State: Connecting";

            // SplashForm
            this.ClientSize = new System.Drawing.Size(569, 341);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon embeddedIcon = Properties.Resources.connecting;
            this.Icon = embeddedIcon;
            this.Name = "SplashForm";
            this.ShowInTaskbar = false;
            this.Text = "OpenVPN Connection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ShowLoginForm()
        {
            using (LoginForm loginForm = new LoginForm(commandLineArg))
            {
                loginForm.FormClosed += (sender, e) => this.Dispose(); //Application.Exit(); - this causes problems running via beacon
                loginForm.Text = commandLineArg;
                loginForm.ShowDialog(this); // errors in VS debug, fine with release
            }
        }

        public SplashForm(string commandLineArg) : this()
        {
            this.commandLineArg = commandLineArg;
            this.Text = "OpenVPN Connection (" + commandLineArg + ")";
        }
    }
}
