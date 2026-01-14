using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Printing;
//using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;
using BarcodeLib;

namespace ParkingManagementReport
{
    public class PrintSlip
    {
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


        public string strPcom1;
        public string strPadd1;
        public string strPadd2;

        public string strPtax;
        public string strPtel;
        public string strTxtPrint;
        public bool PrintCarIn;
        public bool UsePrintOfficer = false;
        public bool UsePrintBarcod = false;
        public bool UseCardlossAddPrice = false;
        public bool UseLastPromotion = false;
        public bool UseNotDay = false;
        public bool UseSlipRecord = false;
        public bool UseAsiaTriqPrice = false;
        public bool UseGroupPrice = false;
        public bool UseSumPrice = false; //Mac 2014/08/16
        public bool UseVoidSlip = false;
        public bool MFPassiveInProx = false; //Mac 2015/06/13
        public bool UseProIDAll = false; //Mac 2015/10/06
        public bool UseLogReport = false; //Mac 2015/11/09
        public bool UseActivePromotion = false; //Mac 2016/01/05
        public bool UseMemberGroupPriceMonth = false; //Mac 2016/04/01
        public bool UseGroupPromotion = false; //Mac 2016/04/02
        public bool UseAsciiMem = false; //Mac 2016/04/18
        public bool VisitorFillDetail = false; //Mac 2016/04/19
        public bool OutReceiptNameMonth = false; //Mac 2016/04/26
        public bool Member2Cartype = false; //Mac 2016/05/03
        public bool Report3Decimal = false; //Mac 2016/10/08
        public string NoPanelUp2U = ""; //Mac 2017/03/08
        public bool UseReport5_1 = false; //Mac 2017/06/19
        public bool UseFormVoidSlip = false; //Mac 2017/07/02
        public string[] NoshowSelectTime; //Mac 2017/11/03
        public bool UseReport1_3 = false; //Mac 2017/11/03
        public bool UseReport13_3 = false; //Mac 2017/11/03
        public bool UseReport13_10 = false; //Mac 2017/11/03
        public bool UseReport13_11 = false; //Mac 2017/11/29
        public bool UsePDFOnly = false; //Mac 2017/11/03
        public bool UseReport3_1 = false; //Mac 2017/11/07
        public bool UseReportDateString = false; //Mac 2017/11/25
        public bool ReportNoRunning = false; //Mac 2018/01/05
        public bool UseMemType = false; //Mac 2018/01/16
        public bool ReportCartypeFree15Min = false; //Mac 2018/01/16
        public bool UseReport1_4 = false; //Mac 2018/02/21
        public bool UseReport5_2 = false; //Mac 2018/02/23
        public bool UseReport14like13 = false; //Mac 2018/02/23
        public bool UseReport1_5 = false; //Mac 2018/02/28
        public bool UseReport5_3 = false; //Mac 2018/02/28

        public bool UseReport1logo = false; //Mac 2018/05/07
        public bool UseReport2logo = false; //Mac 2018/05/07
        public bool UseReport3logo = false; //Mac 2018/05/07
        public bool UseReport4logo = false; //Mac 2018/05/08
        public bool UseReport5logo = false; //Mac 2018/05/08
        public bool UseReport8logo = false; //Mac 2018/05/08
        public bool UseReport13logo = false; //Mac 2018/05/08
        public bool UseReport13_1logo = false; //Mac 2018/05/08
        public bool UseReport16logo = false; //Mac 2018/05/08
        public bool UseReport32logo = false; //Mac 2018/05/08
        public bool ReportPriceSplitLosscard = false; //Mac 2018/05/13
        public bool UseReport50logo = false; //Mac 2018/05/23      
        public bool UseReport21_1 = false; //Mac 2018/07/03
        public bool Report21_22_NoCreditNoShow = false; //Mac 2018/08/28
        public bool UseMemLicensePlate = false; //Mac 2018/09/03
        public bool UseReport72_1 = false; //Mac 2018/10/29
        public bool UseReport71_1 = false; //Mac 2018/10/29
        public bool UseReport5_4 = false; //Mac 2018/11/12
        public bool UseReport1_6 = false; //Mac 2018/11/12
        public bool UseReceiptFor1Out = false; //Mac 2018/11/14
        public bool UseNameOnCard = false; //Mac 2018/12/13
        public bool UseReport13_7 = false; //Mac 2019/02/15
        public bool UseReport6 = false; //Mac 2019/05/03
        public bool UseReport23_1 = false; //Mac 2019/05/07
        public bool ReportProsetPriceDayWeek = false; //Mac 2019/05/27
        public bool UseReport1_7 = false; //Mac 2019/07/24
        public bool UseMemHourBalance = false; //Mac 2019/10/22
        public string strBuilding = ""; //Mac 2020/01/20
        public string strOffice = ""; //Mac 2020/01/20
        public bool UseQRCodeNew = false; //Mac 2020/06/04
        public bool UseReceiptFor1Mem = false; //Mac 2020/08/14
        public bool UseReport24_1 = false; //Mac 2020/08/15
        public bool UseReport108_110_1 = false; //Mac 2020/08/19
        public bool Report21_1_Switch = false; //Mac 2020/10/13
        public bool UseSettingNewMem = false; //Mac 2020/12/18
        public bool Report13Pro_SwitchPriceNot0 = false; //Mac 2021/01/26
        public bool ReportSearchMemGroup = false; //Mac 2021/03/04
        public bool Report49_LossCard_NoVat = false; //Mac 2021/05/28
        public bool UseReport24_2 = false; //Mac 2021/07/22
        public bool UseReport49_1 = false; //Mac 2021/10/14
        public bool UseReport35_1 = false; //Mac 2021/10/15
        public bool UseReport36_1 = false; //Mac 2021/10/16
        public bool UseReport24_3 = false; //Mac 2021/10/20
        public bool UseReport1_8 = false; //Mac 2021/12/17
        public string NotShowNoString = ""; //Mac 2022/04/22
        public bool UseReport21_2 = false; //Mac 2022/05/30
        public string UseDayWeek = ""; //Mac 2022/07/21
        public bool UseHoliday = false; //Mac 2022/07/26
        public bool UseFlatRateProSetPrice = false; //Mac 2022/07/29
        public bool UseReport21_3 = false; //Mac 2022/09/29
        public bool UseCalVatFromTotal = false; //Mac 2022/09/30
        public bool UseReport_houruse = false; //Mac 2023/02/22
        public bool UseReport13_12 = false; //Mac 2023/08/09
        public bool UseReport13_13 = false; //Mac 2024/12/06
        public bool UseReport2_4 = false; //Mac 2025/03/06
        public bool UsePrintQRcode = false; //Mac 2025/03/07
        public bool UseReportJasmine = false; 

        public string PrinterName = "";
        public int NoCar = 0;
        public int CarInRepeat = 0;
        public int intPriceCardLoss = 0;
        public string strDiscount;
        private PrintDocument pd;
        private string strPrint;
        private string strBarcode;
        public bool booPrintImage = false;
        private Image img;
        private const int xImage = 280;
        private const int yImage = 50;

        public PrintSlip()
        {
            pd = new PrintDocument();
            pd.PrintPage += pd_PrintPage;
            //pd.PrinterSettings = new PrinterSettings();
        }
        //Rectangle m = e.MarginBounds;
        //if ((double)img.Width / (double)img.Height > (double)m.Width / (double)m.Height) // image is wider
        //{
        //    m.Height = (int)((double)img.Height / (double)img.Width * (double)m.Width);
        //}
        //else
        //{
        //    m.Width = (int)((double)img.Width / (double)img.Height * (double)m.Height);
        //}
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            float yPos = 0f;
            float leftMargin = e.MarginBounds.Left;
            leftMargin = 12;
            yPos = 0;
            if (booPrintImage)
            {
                e.Graphics.DrawString(strPrint, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                e.Graphics.DrawImage(img, leftMargin, 310, xImage, yImage);
                e.HasMorePages = false;
            }
            else
            {

                if (printFont == null)
                    printFont = new System.Drawing.Font("Cordia New", 12);
                e.Graphics.DrawString(strPrint, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            }
            booPrintImage = false;
        }

        private bool booOffline = false;

        public string ReadParam(string strParam)
        {
            string str = "";
            string sql = "SELECT * FROM param WHERE name='" + strParam + "'";
            DataTable dt;
            if (booOffline)
                dt = dbLocal.LoadData(sql);
            else
                dt = db.LoadData(sql);
            str = dt.Rows[0].ItemArray[1].ToString();
            return str;
        }

        public void LoadParam(bool Offline)
        {
            booOffline = Offline;

            //strPcom1 = dt.Rows[0].ItemArray[1].ToString();
            //strPadd1 = dt.Rows[2].ItemArray[1].ToString();
            //strPadd2 = dt.Rows[3].ItemArray[1].ToString();
            //strPtax = dt.Rows[7].ItemArray[1].ToString();
            //strPtel = dt.Rows[6].ItemArray[1].ToString();
            //NoCar = Convert.ToInt32(dt.Rows[8].ItemArray[1]);
            //strDiscount = dt.Rows[10].ItemArray[1].ToString();
            //PrintCarIn = Convert.ToBoolean(dt.Rows[9].ItemArray[1]);
            //intPriceCardLoss = Convert.ToInt32(dt.Rows[12].ItemArray[1]);
            //CarInRepeat = Convert.ToInt32(dt.Rows[13].ItemArray[1]);
            //UsePrintBarcod = Convert.ToBoolean(dt.Rows[14].ItemArray[1]);
            //UsePrintOfficer = Convert.ToBoolean(dt.Rows[15].ItemArray[1]);
            strPcom1 = ReadParam("com1");
            strPadd1 = ReadParam("add1");
            strPadd2 = ReadParam("add2");
            strPtax = ReadParam("tax");
            strPtel = ReadParam("tel");
            NoCar = Convert.ToInt32(ReadParam("no_car"));
            strDiscount = ReadParam("discount");
            PrintCarIn = Convert.ToBoolean(ReadParam("printin"));
            intPriceCardLoss = Convert.ToInt32(ReadParam("cardloss"));
            CarInRepeat = Convert.ToInt32(ReadParam("carin_repeat"));
            UsePrintBarcod = Convert.ToBoolean(ReadParam("print_barcode"));
            UsePrintOfficer = Convert.ToBoolean(ReadParam("print_officer"));
            try
            {
                UseCardlossAddPrice = Convert.ToBoolean(ReadParam("cardloss_price"));
                UseLastPromotion = Convert.ToBoolean(ReadParam("use_lastpro"));
                UseNotDay = Convert.ToBoolean(ReadParam("not_day"));
                UseSlipRecord = Convert.ToBoolean(ReadParam("not_slipprice"));
                UseAsiaTriqPrice = Convert.ToBoolean(ReadParam("asiatriq_price"));
                //UseOfflineMode = Convert.ToBoolean(ReadParam("offline_mode"));
               
                //UseCardlossAddPrice = Convert.ToBoolean(dt.Rows[16].ItemArray[1]);
                //UseLastPromotion = Convert.ToBoolean(dt.Rows[17].ItemArray[1]);
                //UseNotDay = Convert.ToBoolean(dt.Rows[18].ItemArray[1]);
                //UseSlipRecord = Convert.ToBoolean(dt.Rows[19].ItemArray[1]);

                //UseFreeTime = Convert.ToBoolean(dt.Rows[20].ItemArray[1]);
                //FreeTimeStart = dt.Rows[21].ItemArray[1].ToString();
                //FreeTimeStop = dt.Rows[22].ItemArray[1].ToString();

                //UseAsiaTriqPrice = Convert.ToBoolean(dt.Rows[23].ItemArray[1]);
                //UseOfflineMode = Convert.ToBoolean(dt.Rows[24].ItemArray[1]);
            }
            catch (Exception)
            {
            }
            try
            {
                UseGroupPrice = Convert.ToBoolean(ReadParam("group_price"));
                UseSumPrice = Convert.ToBoolean(ReadParam("sum_price")); //Mac 2014/08/16
            }
            catch (Exception)
            {
            }

            try
            {
                string sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'carpark2' AND TABLE_NAME = 'recordout' AND COLUMN_NAME = 'status'";
                DataTable dt = db.LoadData(sql);
                if (dt.Rows.Count > 0)
                    UseVoidSlip = true;
            }
            catch (Exception)
            {
            }
            strTxtPrint = TextCenter(strPcom1);
            strTxtPrint += TextCenter(strPadd1);
            strTxtPrint += TextCenter(strPadd2);
            //strTxtPrint += TextCenter(strPadd3);
            strTxtPrint += TextCenter(strPtel);
            strTxtPrint += TextCenter(strPtax) + "\r\n";
            if (UseSlipRecord)
            {
                strTxtPrint += TextCenter("                               ใบบันทึกเวลา");
                //strTxtPrint += TextCenter("                             (ไม่ใช่บัตรจอดรถ)");
            }
            else
            {
                strTxtPrint += TextCenter("               OFFICIAL RECEIPT / TAX INVOICE");
                strTxtPrint += TextCenter("               ใบเสร็จรับเงิน / ใบกำกับภาษีอย่างย่อ");
            }

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
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        private void OpenDraw()
        {
            string COMMAND = "";
            COMMAND = Convert.ToString((char)27) + "p";
            COMMAND += Convert.ToString((char)0);
            COMMAND += Convert.ToString((char)250);
            COMMAND += Convert.ToString((char)250);
            SendStringToPrinter(COMMAND);
        }

        private void Cut()
        {
            string COMMAND = "";
            COMMAND = Convert.ToString((char)27) + "@";
            COMMAND += Convert.ToString((char)29 + "V");
            COMMAND += Convert.ToString((char)1);
            SendStringToPrinter(COMMAND);
        }

        //public void PrintOfficer(string strName, int intPrice, int intDiscount)
        public void PrintOfficer(string strName,string strDti, int intPrice, int intDiscount) //Mac 2014/08/09
        {
            DateTime now = DateTime.Now;
            DateTime dti = Convert.ToDateTime(strDti); //Mac 2014/08/09
            string strPr = "\r\n      บันทึกการปฏิบัติงาน\r\n";
            //strPr = "";
            strPr += "\r\nชื่อ:         " + strName;
            strPr += "\r\nเวลาเข้า: " + dti.ToString("dd/MM/yyyy HH:mm:ss"); //Mac 2014/08/09
            //strPr += "\r\nเวลาออก: " + now.ToString();// Mac 2014/08/21
            strPr += "\r\nเวลาออก: " + now.ToString("dd/MM/yyyy HH:mm:ss");
            strPr += "\r\nรายได้:    " + intPrice + " บาท";
            strPr += "\r\nส่วนลด:  " + intDiscount + " บาท";
            strPr += "\r\n\r\n";
            Print(strPr, false);
        }

        public void PrintSlipOut(int RecordNo, string License, int intPriceTotel, string TimeIn, string Timeout, string TimeParking,bool booPrintRecord)
        {
            DateTime now = DateTime.Now;
            string strPr = "";
            if (UseGroupPrice)
            {
                DateTime dt = DateTime.Now;
                dt = dt.AddYears(-543);
                strPr = strTxtPrint + "\r\nเลขที่/Receipt No.:    " + dt.ToString("yyyyMM") + RecordNo.ToString("00#"); 
            }
            else
            {
                //if (!booPrintRecord)
                    strPr = strTxtPrint + "\r\nเลขที่/Receipt No.:    IV" + DateTime.Now.ToString("yy") + RecordNo.ToString("00000#");
                //else
                //    strPr = strTxtPrint + "\r\nเลขที่/Receipt No.:    1-" + RecordNo.ToString("00000#");
            }
          
            //strPr = "";
            strPr += "\r\nวันที่/Date:                     " + now.ToShortDateString();
            //strPr += "\r\nทะเบียนรถ/License:      " + PrintInteger(License);
            strPr += "\r\nทะเบียนรถ/License:      " + License;
            strPr += "\r\nเวลาเข้า/In:                    " + TimeIn;
            strPr += "\r\nเวลาออก/Out:                " + Timeout;
            if (!UseSlipRecord)
            {
                strPr += "\r\nราคา/Price:                   " + intPriceTotel.ToString() + " บาท";
            }
            else
            {
                //strPr += "\r\nเวลาจอด/Time:                   " + TimeParking;
                strPr += "\r\nเวลาจอด/Tim:                " + TimeParking;
            }
            strPr += "\r\n\r\n";
            strPr += TextCenter("                    ขอบคุณที่ใช้บริการ/Thank you");
            Print(strPr, false);
        }

        public void PrintSlipCarIn(int RecordNo, string License)
        {
            DateTime now = DateTime.Now;
            string strPr = strTxtPrint + "\r\nลำดับที่/Receipt No.:    " + RecordNo;
            strPr += "\r\nวันที่/Date:                     " + now.ToShortDateString();
            strPr += "\r\nทะเบียนรถ/License:      " + PrintInteger(License);
            strPr += "\r\nเวลาเข้า/In:                    " + now.ToLongTimeString(); ;
            strPr += "\r\nเวลาออก/Out:                " + "-";
            if (!UseSlipRecord)
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
            string strPr = strTxtPrint + "\r\nลำดับที่/Receipt No.:    " + RecordNo;
            strPr += "\r\nวันที่/Date:                     " + now.ToShortDateString();
            strPr += "\r\nทะเบียนรถ/License:      " + PrintInteger(License);
            strPr += "\r\nเวลาเข้า/In:                    " + now.ToLongTimeString(); ;
            //strPr += "\r\nเวลาออก/Out:                " + "-";
            //strPr += "\r\nราคา/Price:                   " + "-";
            strPr += "\r\n\r\n";
            strPr += "\r\n\r\n";
            strPr += TextCenter("                    ขอบคุณที่ใช้บริการ/Thank you");
            //PrintBarcode2(strPr, true);

            strPr += "\r\n";
            strPr += "                               ______________\r\n";
            strPr += "                              |                          |\r\n";
            strPr += "                              |        Stamp       |\r\n";
            strPr += "                              |______________|";

            strPrint = strPr;
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
            //Print2();
            booPrintImage = true;
            strBarcode = Barcode;
            Print2();
            Cut();
        }

        private string PrintInteger(String strSend)
        {
            string strPrint = "";
            for (int i = 0; i < strSend.Length; i++)
            {
                string s = strSend.Substring(i, 1);
                if (s == "0" || s == "1" || s == "2" || s == "3" || s == "4")
                {
                    strPrint += s;
                }
                if (s == "5" || s == "6" || s == "7" || s == "8" || s == "9")
                {
                    strPrint += s;
                }
            }
            return strPrint;
        }

        private string TextCenter(String s)
        {
            string txt = s + "\r\n";
            //string str = "";
            //if (s != null && s != "")
            //{
            //    int n = (48 - s.Length) / 2;
            //    if (n > 0)
            //    {
            //        for (int i = 0; i < n; i++)
            //        {
            //            str += " ";
            //        }
            //        txt = str + s + str + "\r\n";
            //    }
            //}
            return txt;
        }


        private void Print(string COMMAND, bool booPrintCarIn)//pd.PrinterSettings.PrinterName
        {
            string str = "";
            if (booPrintCarIn)
            {
                str = "\r\n";
                str += "                               ______________\r\n";
                str += "                              |                          |\r\n";
                str += "                              |        Stamp       |\r\n";
                str += "                              |______________|";
            }
            else
            {
                OpenDraw();
            }
            strPrint = COMMAND + str;
            pd.Print();
            Cut();
        }

        System.Drawing.Font printFont;

        private void Print2()
        {
            printFont = new System.Drawing.Font("Cordia New", 12);
            pd.Print();
        }

        private bool SendStringToPrinter(string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            //pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(PrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

        public void Update()
        {
            UpdateParam("printin", PrintCarIn.ToString());
            UpdateParam("com1", strPcom1.ToString());
            UpdateParam("add1", strPadd1.ToString());
            UpdateParam("add2", strPadd2.ToString());
            //UpdateParam("add3", strPadd3.ToString());
            UpdateParam("tax", strPtax.ToString());
            UpdateParam("tel", strPtel.ToString());
            UpdateParam("print_barcode", UsePrintBarcod.ToString());
            UpdateParam("print_officer", UsePrintOfficer.ToString());
        }

        private void UpdateParam(string strpname, string strpvalue)
        {
            string sql = "UPDATE param SET value ='" + strpvalue;
            sql += "' WHERE name='" + strpname + "'";
            db.SaveData(sql);
        }
    }
}
