using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementReport.Common;

namespace ParkingManagementReport.Utilities
{
    internal class ImagesManager
    {
        private static string strFileL;
        private static string strFileD;

        public static void SaveImage(Bitmap bmpPD, Bitmap bmpPL, string strDir, string strMode)
        {
            DateTime now = DateTime.Now;
            string strFile = now.ToString("ddMMyyyy_HHmmss") + ".jpg";
            string folder = now.Month.ToString();
            strFileL = strDir + "\\" + folder;
            if (!Directory.Exists(strFileL))
            {
                Directory.CreateDirectory(strFileL);
            }
            folder = now.Day.ToString();
            strFileL = strFileL + "\\" + folder;
            if (!Directory.Exists(strFileL))
            {
                Directory.CreateDirectory(strFileL);
            }
            strFileD = "";
            if (Configs.Use2Camera)
            {
                strFileD = strFileL;
                strFileD = strFileD + "\\" + strMode + "D" + strFile;
                bmpPD.Save(strFileD);
            }
            strFileL += "\\" + strMode + "L" + strFile;

            bmpPL.Save(strFileL);
        }

        public static Image GetCopyImage(string path)
        {
            try
            {
                using (Image im = Image.FromFile(path))
                {
                    Bitmap bm = new Bitmap(im);
                    return bm;
                }
            }
            catch (Exception) { }
            return null;
        }
    }
}
