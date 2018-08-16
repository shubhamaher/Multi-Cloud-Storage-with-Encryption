using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Panda
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
            Application.Run(new frmLogin());
            /*if (frmLogin.log_flg == 1)
            {
                Application.Run(new Formreg());
            }

            if (frmLogin.log_flg == 2)
            {
                Application.Run(new Form1());
            }
            if (frmLogin.log_flg == 3)
            {
                Application.Run(new operations());
            }*/

        }
    }
}
