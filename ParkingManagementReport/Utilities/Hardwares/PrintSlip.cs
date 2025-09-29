using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using BarcodeLib;
using ParkingManagementReport.Common;

namespace ParkingManagementReport.Utilities.Hardwares
{
    public class PrintSlip
    {

        #region FIELDS
        string PrinterName = "";
        string strDiscount;
        string printingBody;
        string printingText;
        string strBarcode;
        bool isPrintImage = false;
        bool isOffline = false;
        int noCar = 0;
        int carInRepeat = 0;
        int intPriceCardLoss = 0;
        const int xImage = 280;
        const int yImage = 50;

        Image img;
        PrintDocument pd;
        Font printFont;
        #endregion

        public PrintSlip()
        {
            pd = new PrintDocument();
            pd.PrintPage += DoPrintPage;
        }
      
        private void DoPrintPage(object sender, PrintPageEventArgs e)
        {
            float yPos = 0f;
            float leftMargin = e.MarginBounds.Left;
            leftMargin = 12;
            yPos = 0;
            if (isPrintImage)
            {
                e.Graphics.DrawString(printingBody, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                e.Graphics.DrawImage(img, leftMargin, 310, xImage, yImage);
                e.HasMorePages = false;
            }
            else
            {

                if (printFont == null)
                    printFont = new Font("Cordia New", 12);
                e.Graphics.DrawString(printingBody, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            }
            isPrintImage = false;
        }

        #region PRINTING
        private void Print(string COMMAND, bool boolPrintCarIn)
        {
            string str = "";
            if (boolPrintCarIn)
            {
                str = "\r\n";
                str += "                               ______________\r\n";
                str += "                              |                          |\r\n";
                str += "                              |        Stamp       |\r\n";
                str += "                              |______________|";
            }
            else
            {
                OpenCashDrawer();
            }
            printingBody = COMMAND + str;
            pd.Print();
            CutPrintingPaper();
        }

        private void Print2()
        {
            printFont = new System.Drawing.Font("Cordia New", 12);
            pd.Print();
        }

        public void PrintOfficer(string strName,string strDti, int intPrice, int intDiscount) //Mac 2014/08/09
        {
            DateTime now = DateTime.Now;
            DateTime dti = Convert.ToDateTime(strDti); //Mac 2014/08/09
            string strPr = "\r\n      บันทึกการปฏิบัติงาน\r\n";
            //strPr = "";
            strPr += "\r\nชื่อ:         " + strName;
            strPr += "\r\nเวลาเข้า: " + dti.ToString("dd/MM/yyyy HH:mm:ss"); //Mac 2014/08/09
            strPr += "\r\nเวลาออก: " + now.ToString("dd/MM/yyyy HH:mm:ss");
            strPr += "\r\nรายได้:    " + intPrice + " บาท";
            strPr += "\r\nส่วนลด:  " + intDiscount + " บาท";
            strPr += "\r\n\r\n";
            Print(strPr, false);
        }

        public void PrintSlipOut(int RecordNo, string License, int intPriceTotel, string TimeIn, string Timeout, string TimeParking,bool boolPrintRecord)
        {
            DateTime now = DateTime.Now;
            string strPr = "";
            if (Configs.UseGroupPrice)
            {
                DateTime dt = DateTime.Now;
                dt = dt.AddYears(-543);
                strPr = printingText + "\r\nเลขที่/Receipt No.:    " + dt.ToString("yyyyMM") + RecordNo.ToString("00#"); 
            }
            else
            {
                    strPr = printingText + "\r\nเลขที่/Receipt No.:    IV" + DateTime.Now.ToString("yy") + RecordNo.ToString("00000#");
            }
          
            strPr += "\r\nวันที่/Date:                     " + now.ToShortDateString();
            strPr += "\r\nทะเบียนรถ/License:      " + License;
            strPr += "\r\nเวลาเข้า/In:                    " + TimeIn;
            strPr += "\r\nเวลาออก/Out:                " + Timeout;
            if (!Configs.UseSlipRecord)
            {
                strPr += "\r\nราคา/Price:                   " + intPriceTotel.ToString() + " บาท";
            }
            else
            {
                strPr += "\r\nเวลาจอด/Tim:                " + TimeParking;
            }
            strPr += "\r\n\r\n";
            strPr += TextCenter("                    ขอบคุณที่ใช้บริการ/Thank you");
            Print(strPr, false);
        }

        public void PrintSlipCarIn(int RecordNo, string License)
        {
            DateTime now = DateTime.Now;
            string strPr = printingText + "\r\nลำดับที่/Receipt No.:    " + RecordNo;
            strPr += "\r\nวันที่/Date:                     " + now.ToShortDateString();
            strPr += "\r\nทะเบียนรถ/License:      " + PrintInteger(License);
            strPr += "\r\nเวลาเข้า/In:                    " + now.ToLongTimeString(); ;
            strPr += "\r\nเวลาออก/Out:                " + "-";
            if (!Configs.UseSlipRecord)
            {
                strPr += "\r\nราคา/Price:                   " + "-";
            }
            else
            {

            }
            strPr += "\r\n\r\n";
            strPr += TextCenter("                    ขอบคุณที่ใช้บริการ/Thank you");
            Print(strPr, true);
        }

        public void PrintBarcode(int RecordNo, string License)
        {
            string Barcode = String.Format("{0:0000000000}", RecordNo);
            DateTime now = DateTime.Now;
            string strPr = printingText + "\r\nลำดับที่/Receipt No.:    " + RecordNo;
            strPr += "\r\nวันที่/Date:                     " + now.ToShortDateString();
            strPr += "\r\nทะเบียนรถ/License:      " + PrintInteger(License);
            strPr += "\r\nเวลาเข้า/In:                    " + now.ToLongTimeString(); ;
            strPr += "\r\n\r\n";
            strPr += "\r\n\r\n";
            strPr += TextCenter("                    ขอบคุณที่ใช้บริการ/Thank you");

            strPr += "\r\n";
            strPr += "                               ______________\r\n";
            strPr += "                              |                          |\r\n";
            strPr += "                              |        Stamp       |\r\n";
            strPr += "                              |______________|";

            printingBody = strPr;
            Barcode barcode = new BarcodeLib.Barcode()
            {
                IncludeLabel = true,
                Alignment = AlignmentPositions.CENTER,
                Width = xImage,
                Height = yImage,
                RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                BackColor = Color.White,
                ForeColor = Color.Black,
            };
            img = barcode.Encode(TYPE.CODE128B, Barcode);
            
            isPrintImage = true;
            strBarcode = Barcode;
            Print2();
            CutPrintingPaper();
        }

        private string PrintInteger(String strSend)
        {
            string printingBody = "";
            for (int i = 0; i < strSend.Length; i++)
            {
                string s = strSend.Substring(i, 1);
                if (s == "0" || s == "1" || s == "2" || s == "3" || s == "4")
                {
                    printingBody += s;
                }
                if (s == "5" || s == "6" || s == "7" || s == "8" || s == "9")
                {
                    printingBody += s;
                }
            }
            return printingBody;
        }
        #endregion PRINTING_END

        #region HELPERS
        private string TextCenter(String s)
        {
            string txt = s + "\r\n";

            return txt;
        }

        private bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = DateTime.Now.ToString();//"CIT Carpark";
            di.pDataType = "RAW";

            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }

            if (bSuccess == false) dwError = Marshal.GetLastWin32Error();

            return bSuccess;
        }

        private void OpenCashDrawer()
        {
            string COMMAND = "";
            COMMAND = Convert.ToString((char)27) + "p";
            COMMAND += Convert.ToString((char)0);
            COMMAND += Convert.ToString((char)250);
            COMMAND += Convert.ToString((char)250);
            SendStringToPrinter(COMMAND);
        }

        private void CutPrintingPaper()
        {
            string COMMAND = "";
            COMMAND = Convert.ToString((char)27) + "@";
            COMMAND += Convert.ToString((char)29 + "V");
            COMMAND += Convert.ToString((char)1);
            SendStringToPrinter(COMMAND);
        }

        private bool SendStringToPrinter(string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            dwCount = szString.Length;
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            SendBytesToPrinter(PrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
        #endregion HELPERS_END

        #region DLL/IMPORT

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        #endregion
    }
}