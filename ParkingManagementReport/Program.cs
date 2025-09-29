using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ParkingManagementReport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool mutexCreated = false;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, @"Local\slimCODE.slimKEYS.exe", out mutexCreated);

            if (!mutexCreated)
            {
                mutex.Close();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
