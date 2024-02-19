using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// TODO
// custom messagebox warning image update - not too important

namespace KeePass
{
    internal static class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string titleArg = (args.Length > 0) ? args[0] : "";

            // * WORKS * in CS 4.1/.2?
            //Application.Run(new Form1(titleArg));


            // * WORKS * in CS 4.9 
            //using (var form = new Form1(titleArg))
            //{
                //Application.Run(new Form1(titleArg));
            //}

            // * WORKS * in CS 4.9 
            ApplicationContext context = new ApplicationContext(new Form1(titleArg));
            Application.Run(context);
        }
    }
}
