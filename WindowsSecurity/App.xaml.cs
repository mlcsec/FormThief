using System;
using System.Windows;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public static string[] Args;
        public static string Arg { get; private set; }

        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App();
            app.InitializeComponent();

            if (args.Length > 0)
            {
                Arg = args[0];
            }

            MainWindow mainWindow = new MainWindow();
            app.Run(mainWindow);
        }
    }
}