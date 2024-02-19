using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


// TODO 
// - username X button?? - not too important 

namespace LastPass
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string titleArg = (args.Length > 0) ? args[0] : "";
            //Application.Run(new Form1(titleArg));  

            // * WORKS * in CS 4.9  
            // - only runs once though
            ApplicationContext context = new ApplicationContext(new Form1(titleArg));
            Application.Run(context);
        }
    }
}
