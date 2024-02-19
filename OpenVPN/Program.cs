using System;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace OpenVPN
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string titleArg = (args.Length > 0) ? args[0] : "";
            //Application.Run(new SplashForm(titleArg));

            ApplicationContext context = new ApplicationContext(new SplashForm(titleArg));
            Application.Run(context);

            //Application.Run(new LoginForm(titleArg));
        }
    }
}