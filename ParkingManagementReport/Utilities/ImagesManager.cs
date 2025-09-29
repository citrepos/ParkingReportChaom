using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        internal static void SetImageSourceToPictureBox(string imagePath, PictureBox pictureBox)
        {
            if (!string.IsNullOrEmpty(imagePath?.Trim()))
            {
                Image image = GetCopyImage(imagePath);
                pictureBox.Image = image;
            }
        }

        internal static void AssignFileBytesToDataRow(DataRow dataRow, string columnName, string filePath)
        {
            try
            {
                using (FileStream fiStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader binReader = new BinaryReader(fiStream))
                {
                    byte[] pic = binReader.ReadBytes((int)fiStream.Length);
                    dataRow[columnName] = pic;
                }
            }
            catch
            {
                dataRow[columnName] = DBNull.Value; 
            }
        }


        #region HELPERS
        private static Image GetCopyImage(string path)
        {
            try
            {
                using (Image im = Image.FromFile(path))
                {
                    Bitmap bm = new Bitmap(im);
                    return bm;
                }
            }
            catch { }
            return null;
        }
        #endregion HELPERS_END
    }
}