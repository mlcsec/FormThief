using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


// TODO:
// - grey panel bottom of error message window 
// - accept list of vpn connect hosts?
namespace CiscoAnyConnect
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.EnableVisualStyles();

            string titleArg = (args.Length > 0) ? args[0] : "";
            //Application.Run(new SplashForm(titleArg));

            ApplicationContext context = new ApplicationContext(new Form1(titleArg));
            Application.Run(context);

            //Application.Run(new Form1(titleArg));
        }
    }
}
