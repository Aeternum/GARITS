using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GARITS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Splash()); //is it ok to the the garbage collector tidy this one up?
            Application.Run(new GARITS());
        }
    }
}
