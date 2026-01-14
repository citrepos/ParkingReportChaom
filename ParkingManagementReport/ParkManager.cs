using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DataTable = System.Data.DataTable;

namespace ParkingManagementReport
{
    public class ParkManager
    {
        public string strFileXML = "C:\\Windows\\carpark";//
        public string Mode;
        public string PortControl;
        public string PortProxIn;
        public string PortProxOut;
        public string PortMifare;
        public string PortMifareIn;
        public string PortMifareOut;

        public bool UseOfflineMode = false;
        public bool booStartWithOffline = false;
        public bool UseCarType = false;
        public bool Use2Camera = false;
        public bool ModeDispenser = false;

        public bool UseControlBoard = false;
        public bool UseMifare = false;
        public bool UseProxIn = false;
        public bool UseProxOut = false;

        public bool UseMifareIn = false;
        public bool UseMifareOut = false;
        public bool UseLogOut = false;

        public bool booFRMReaderIn = false;
        public bool booFRMReaderOut = false;
        public bool booOffline = false;
        public bool booStartOffline = false;

        public string IPIn1 = "";
        public string IPIn2 = "";
        public string IPIn3 = ""; //Mac 2015/02/04
        public string IPOut1 = "";
        public string IPOut2 = "";
        //public int intChangCamera = 0;
        public string ServerDirectory;
        public string BackupDirectory;
        public string ServerIP;

        public string DatabaseName = "carpark2"; //Mac 2016/11/09

        public User user = new User();
        //public Member member = new Member();
        //public Visitor visitor = new Visitor();
        public PrintSlip print = new PrintSlip();
        //public AES128 aes = new AES128();

        //public ControlBoard cb;// = new ControlBoard();
        //public ProxReader proxIn;
        //public ProxReader proxOut;

        public MifareReader mifaV;
        //public MifareReader mifaMin;
        //public MifareReader mifaMOut;
        //public FRMReader frmReader;
        //public DispenserB dps;

        public int CardLevel = 0;
        //public uint CardID = 0;
        public int RecordNo = 0;

        public int NoCar = 0;
        //public bool booDiscount;

        public string License = "";
        //public string CarType = "";
        public string Randkey = "";
        public string Datein = "";
        public int CarType = 0;
        public int proID = 0;

        public string strFileD = "";
        public string strFileL = "";

        //Mac 2015/07/29 -------
        public Dictionary<string, string> DicDatabase = new Dictionary<string, string>();
        public Dictionary<int, string> DicDatabaseIns = new Dictionary<int, string>();
        //----------------------

        //public db2 ofb = new db2();

        public bool UsePaymentBeam = false;
        public bool UsePaymentKsher = false;
        public bool UsePaymentRabbit = false;
        public bool UsePaymentScb = false;
        public bool UseFreeTime = false;
        public string FreeTimeStart2 = "";
        public string FreeTimeStop2 = "";
        public string FreeTimeStart3 = "";
        public string FreeTimeStop3 = "";

        public void LoadDataServer()
        {
            string sql = "SELECT * FROM cardmf";
            DataTable dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM cardmf";
                if (dbLocal.SaveData(sql) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO cardmf VALUES (" + dt.Rows[i].ItemArray[0].ToString() + "," + dt.Rows[i].ItemArray[1].ToString() + "," + dt.Rows[i].ItemArray[2].ToString() + ")";
                        dbLocal.SaveData(sql);
                    }
                }
            }
            sql = "SELECT * FROM cardpx";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM cardpx";
                if (dbLocal.SaveData(sql) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO cardpx VALUES (" + dt.Rows[i].ItemArray[0].ToString() + "," + dt.Rows[i].ItemArray[1].ToString() + "," + dt.Rows[i].ItemArray[2].ToString() + ")";
                        dbLocal.SaveData(sql);
                    }
                }
            }
            sql = "SELECT * FROM cartype";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM cartype";
                if (dbLocal.SaveData(sql) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO cartype VALUES (" + dt.Rows[i].ItemArray[0].ToString() + ",'" + dt.Rows[i].ItemArray[1].ToString() + "'," + dt.Rows[i].ItemArray[2].ToString() + "," + dt.Rows[i].ItemArray[3].ToString() + "," + dt.Rows[i].ItemArray[4].ToString() + ")";
                        dbLocal.SaveData(sql);
                    }
                }
            }
            sql = "SELECT * FROM member";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM member";
                if (dbLocal.SaveData(sql) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO member VALUES (" + dt.Rows[i].ItemArray[0].ToString() + ",'" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','";
                        DateTime dtString = DateTime.Parse(dt.Rows[i].ItemArray[9].ToString());
                        dtString = dtString.AddYears(-543);
                        string strDateTime1 = dtString.ToString("yyyy-MM-dd HH:mm:ss");
                        dtString = DateTime.Parse(dt.Rows[i].ItemArray[10].ToString());
                        dtString = dtString.AddYears(-543);
                        string strDateTime2 = dtString.ToString("yyyy-MM-dd HH:mm:ss");
                        sql += dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + ",'" + strDateTime1 + "','" + strDateTime2 + "')";
                        dbLocal.SaveData(sql);
                    }
                }
            }

            sql = "SELECT * FROM param";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM param";
                if (dbLocal.SaveData(sql) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO param VALUES ('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "')";
                        dbLocal.SaveData(sql);
                    }
                }
            }

            sql = "SELECT * FROM promotion";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM promotion";
                if (dbLocal.SaveData(sql) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO promotion VALUES (" + dt.Rows[i].ItemArray[0].ToString() + ",'" + dt.Rows[i].ItemArray[1].ToString() + "'," + dt.Rows[i].ItemArray[2].ToString() + "," + dt.Rows[i].ItemArray[3].ToString() + ")";
                        dbLocal.SaveData(sql);
                    }
                }
            }

            sql = "SELECT * FROM user";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM user";
                if (dbLocal.SaveData(sql) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO user VALUES (" + dt.Rows[i].ItemArray[0].ToString() + "," + dt.Rows[i].ItemArray[1].ToString() + "," + dt.Rows[i].ItemArray[2].ToString() + ",'" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','";
                        sql += dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "','" + dt.Rows[i].ItemArray[7].ToString() + "')";
                        dbLocal.SaveData(sql);
                    }
                }
            }
            sql = "SHOW TABLES";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string str = dt.Rows[i].ItemArray[0].ToString();
                    if (str.IndexOf("pricerate") >= 0)
                    {
                        sql = "SELECT * FROM " + str;
                        DataTable dt2 = db.LoadData(sql);
                        sql = "DELETE FROM " + str;
                        if (dbLocal.SaveData(sql) == "")
                        {
                            for (int i2 = 0; i2 < dt2.Rows.Count; i2++)
                            {
                                sql = "INSERT INTO " + str + " VALUES (" + dt2.Rows[i2].ItemArray[0].ToString() + "," + dt2.Rows[i2].ItemArray[1].ToString() + "," + dt2.Rows[i2].ItemArray[2].ToString() + "," + dt2.Rows[i2].ItemArray[3].ToString() + "," + dt2.Rows[i2].ItemArray[4].ToString() + ",";
                                sql += dt2.Rows[i2].ItemArray[5].ToString() + "," + dt2.Rows[i2].ItemArray[6].ToString() + ")";
                                dbLocal.SaveData(sql);
                            }
                        }
                    }
                }
            }

            LoadOnlinePaymentType();
        }

        public void LoadOnlinePaymentType()
        {
            int paymentMethodCount = 0;

            string sql = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + FormMain.pm.DatabaseName + "' AND TABLE_NAME = 'beam_post'";
            DataTable dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                UsePaymentBeam = true;
                paymentMethodCount++;
            }

            sql = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + FormMain.pm.DatabaseName + "' AND TABLE_NAME = 'rabbit_post'";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                UsePaymentRabbit = true;
                paymentMethodCount++;
            }

            sql = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + FormMain.pm.DatabaseName + "' AND TABLE_NAME = 'ksherpay_post'";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                UsePaymentKsher = true;
                paymentMethodCount++;
            }

            sql = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + FormMain.pm.DatabaseName + "' AND TABLE_NAME = 'scb_post'";
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                UsePaymentScb = true;
                paymentMethodCount++;
            }

            if (paymentMethodCount > 1)
            {
                MessageBox.Show(
                    "มีการเปิดใช้งานวิธีชำระเงินมากกว่า 1 รายการ กรุณาตรวจสอบ table:\r\n" +
                    "-beam_post\r\n" +
                    "-ksherpay_post\r\n" +
                    "-rabbit_post\r\n" +
                    "-scb_post",
                    "Online Payment Conflict",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        /*public void LoadMemberFreetime()//bool booOffline
        {
            string sql = "SELECT * FROM memberfreetime";
            DataTable dt;
            //if (booOffline)
            //    dt = dbLocal.LoadData(sql);
            //else
            dt = db.LoadData(sql);
            if (dt != null && dt.Rows.Count > 0) //Mac 2014/08/09
            {
                FreeTimeStart2 = dt.Rows[0].ItemArray[1].ToString();
                FreeTimeStop2 = dt.Rows[0].ItemArray[2].ToString();
                FreeTimeStart3 = dt.Rows[1].ItemArray[1].ToString();
                FreeTimeStop3 = dt.Rows[1].ItemArray[2].ToString();
                UseFreeTime = true;
            }
            else
            {
                UseFreeTime = false;
            }
        }*/ //Mac 2014/08/17

        /*public void UpdateMemberFreetime()
        {
            string sql = "UPDATE memberfreetime SET start='" + FreeTimeStart2 + "',stop='" + FreeTimeStop2;
            sql += "' WHERE level=2";
            db.SaveData(sql);
            sql = "UPDATE memberfreetime SET start='" + FreeTimeStart3 + "',stop='" + FreeTimeStop3;
            sql += "' WHERE level=3";
            db.SaveData(sql);
        }*/ //Mac 2014/08/17

        public string CheckTime()
        {
            string strTime = "";
            DataTable dt = db.LoadData("SELECT CURRENT_TIMESTAMP()");
            if (dt.Rows.Count > 0)
            {
                strTime = dt.Rows[0].ItemArray[0].ToString();
            }
            return strTime;
        }

        public string DispenserNo;
        public string DispenserIP;
        public string DispenserName;
        public int DispenserNoCard;
        //public string DispenserIPOut;
        public string DispenserStatus;

        public bool LoadDispenser(int intNo)
        {
            bool result = false;
            string sql = "SELECT * FROM dispenser";
            sql += " WHERE no=" + intNo.ToString();
            DataTable dt = db.LoadData(sql);
            DispenserIP = "NO";
            if (dt.Rows.Count > 0)
            {
                DispenserName = dt.Rows[0].ItemArray[1].ToString();
                DispenserIP = dt.Rows[0].ItemArray[2].ToString();
                DispenserNoCard = Convert.ToInt32(dt.Rows[0].ItemArray[3]);
                //DispenserIPOut = dt.Rows[0].ItemArray[4].ToString();
                //DispenserCardStatus = dt.Rows[0].ItemArray[5].ToString();
                result = true;
            }
            return result;
        }

        public bool LoadDispenserByIP(string strIP)
        {
            bool result = false;
            string sql = "SELECT * FROM dispenser";
            sql += " WHERE ip='" + strIP + "'";
            DataTable dt = db.LoadData(sql);
            //DispenserIP = "NO";
            if (dt.Rows.Count > 0)
            {
                DispenserNo = dt.Rows[0].ItemArray[0].ToString();
                DispenserName = dt.Rows[0].ItemArray[1].ToString();
                //DispenserIP = dt.Rows[0].ItemArray[2].ToString();
                DispenserNoCard = Convert.ToInt32(dt.Rows[0].ItemArray[3]);
                //DispenserIPOut = dt.Rows[0].ItemArray[4].ToString();
                //DispenserCardStatus = dt.Rows[0].ItemArray[5].ToString();
                result = true;
            }
            return result;
        }

        public bool UpdateDispenserIP(string strIP)//, string DispenserNoCard,int DispenserSetCard,string DispenserStatus
        {
            bool result = false;
            string sql = "UPDATE dispenser SET nocard=" + DispenserNoCard;
            sql += " WHERE ip='" + strIP + "'";
            if (db.SaveData(sql) == "")
                result = true;
            return result;
        }

        public bool UpdateDispenser(string strNo)//, string DispenserNoCard,int DispenserSetCard,string DispenserStatus
        {
            bool result = false;
            string sql = "UPDATE dispenser SET nocard=" + DispenserNoCard;
            sql += " WHERE no=" + strNo;
            if (db.SaveData(sql) == "")
                result = true;
            return result;
        }

        public bool LoadDataCarIn(string strRecordNo)
        {
            bool result = false;
            string sql = "SELECT * FROM recordin";
            sql += " WHERE no=" + strRecordNo;
            DataTable dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                //RecordNo = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                License = dt.Rows[0].ItemArray[3].ToString();
                CarType = Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                Randkey = dt.Rows[0].ItemArray[4].ToString();
                Datein = dt.Rows[0].ItemArray[7].ToString();
                strFileD = dt.Rows[0].ItemArray[5].ToString();
                strFileL = dt.Rows[0].ItemArray[6].ToString();
                proID = Convert.ToInt32(dt.Rows[0].ItemArray[9]);
                result = true;
            }
            return result;
        }

        public void SaveImage(Bitmap bmpPD, Bitmap bmpPL, string strDir, string strMode)
        {
            //if (ServerDirectory == strDir && booOffline)
            //    return;
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
            if (Use2Camera)
            {
                strFileD = strFileL;
                strFileD = strFileD + "\\" + strMode + "D" + strFile;
                //if (BackupDirectory == strDir && booOffline)
                //    dbLocal.SaveImage(strFileD);
                bmpPD.Save(strFileD);
            }
            strFileL += "\\" + strMode + "L" + strFile;
            //if (BackupDirectory == strDir && booOffline)
            //    dbLocal.SaveImage(strFileL);
            bmpPL.Save(strFileL);
        }

        public bool UpdateNoRecord(string strTable, uint intID)
        {
            bool result = false;
            if (booOffline)
            {
                return true;
                //string sql = "UPDATE recordno SET no =" + RecordNo.ToString();
                //if (dbLocal.SaveData(sql) == "")
                //    result = true;
            }
            else
            {
                string sql = "UPDATE card" + strTable + " SET no =" + RecordNo.ToString();
                sql += " WHERE name=" + intID.ToString();
                if (db.SaveData(sql) == "")
                    result = true;
            }
            return result;
        }

        public bool UpdateProCarIn(int intNo, int intID)
        {
            bool result = false;
            string sql = "UPDATE recordin SET proid =" + intID.ToString();
            sql += " WHERE no=" + intNo.ToString();
            if (db.SaveData(sql) == "")
                result = true;
            return result;
        }

        public bool UpdateLicenseCarIn(int intNo, string strLicense)
        {
            bool result = false;
            string sql = "UPDATE recordin SET license='" + strLicense;
            sql += "' WHERE no=" + intNo.ToString();
            if (db.SaveData(sql) == "")
                result = true;
            return result;
        }

        public bool InsertProRecord(uint intID, string strLicense, string strProName, string strName)
        {
            bool result = false;
            string sql = "INSERT INTO recordproname (id,license,proname,daterec,username) VALUES ( " + intID + ",'" + strLicense + "','" + strProName + "',Now(),'" + strName + "')";
            if (booOffline)
            {
                //dbLocal.SaveRecordProname(sql);
                result = true;
            }
            else
            {
                if (db.SaveData(sql) == "")
                    result = true;
            }
            return result;
        }

        public void CarInRecordB(uint intID, string strCarType, string strLicense, string strRandKey, string strImageD, string strImageL)
        {
            DateTime now = DateTime.Now;
            string folder = now.Month.ToString();
            string strFile = BackupDirectory + "\\" + folder;
            if (!Directory.Exists(strFile))
            {
                Directory.CreateDirectory(strFile);
            }
            strFile = strFile + "\\InBackup_" + now.ToString("ddMMyyyy") + ".csv";
            string strHeader = "sql,no,id,cartype,license,rankey,picdiv,piclic,datein,userin";
            string sql = "INSERT INTO recordin VALUES (";//
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            sql += RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + ",'" + strLicense + "','" + strRandKey + "','" + strImageD + "','" + strImageL + "',NOW()," + user.ID + "," + proID.ToString() + ");";//
            //string strRecord = RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + "," + strLicense + "," + strRandKey + "," + strImageD + "," + strImageL + "," + now.ToString() + "," + user.ID + ")";//
            if (!File.Exists(strFile))
            {
                StreamWriter sw = File.CreateText(strFile);
                sw.WriteLine(strHeader);
                sw.WriteLine(sql);
                sw.Flush();
                sw.Close();
            }
            else
            {
                FileStream MyFileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                StreamWriter sw = new StreamWriter(MyFileStream);
                sw.WriteLine(sql);
                sw.Close();
                MyFileStream.Close();
            }
        }

        public void getRecordNo()
        {
            if (UseOfflineMode)
            {
                //string strFile = @"C:\Windows\carpark\norecord.txt";
                //FileStream MyFileStream = new FileStream(strFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                //StreamReader sr = new StreamReader(MyFileStream);
                //RecordNo = Convert.ToInt32(sr.ReadLine());
                //sr.Close();
                //MyFileStream.Close();
                RecordNo = dbLocal.LoadNO();
            }
            else
            {
                string sql = "SELECT MAX(no) FROM recordin";
                DataTable dt = db.LoadData(sql);
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        RecordNo = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                    }
                }
                catch (Exception)
                {
                    RecordNo = 0;
                }
            }
        }

        public string SaveRecordNo()
        {
            return dbLocal.SaveNO(RecordNo);
        }

        public bool CarInRecord(uint intID, string strCarType, string strLicense, string strRandKey, string strImageD, string strImageL)// Bitmap bmL, Bitmap bmD
        {
            bool result = false;
            getRecordNo();
            RecordNo++;
            string sql = "INSERT INTO recordin (no,id,cartype,license,rankey,picdiv,piclic,datein,userin) VALUES (";//
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            if (booOffline)
            {
                sql += RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + ",'" + strLicense + "','" + strRandKey + "','picdivS','piclicS',NOW()," + user.ID + ")";//
                SaveOfflineRecord(sql, strImageD, strImageL, "I");

                result = true;
            }
            else
            {
                sql += RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + ",'" + strLicense + "','" + strRandKey + "','" + strImageD + "','" + strImageL + "',NOW()," + user.ID + ")";//
                if (db.SaveData(sql) == "")
                {
                    result = true;
                }
            }
            if (booOffline)
            {
                if (SaveRecordNo() != "")
                    result = false;
            }
            return result;
        }

        //public bool CarInRecordODB1(uint intID, string strCarType, string strLicense, string strDateIn)// Bitmap bmL, Bitmap bmD
        //{
        //    bool result = false;
        //    string sql = "SELECT MAX(no) FROM recordin";
        //    DataTable dt = db.LoadData(sql);
        //    try
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            RecordNo = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        RecordNo = 0;
        //    }

        //    RecordNo++;
        //    sql = "INSERT INTO recordin (no,id,cartype,license,rankey,picdiv,piclic,datein,userin) VALUES(";//
        //    sql += RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + ",'" + strLicense + "','" + "" + "','" + "" + "','" + "" + "','" + strDateIn + "',1)";//
        //    if (db.SaveData(sql) == "")
        //    {
        //        result = true;
        //    }
        //    return result;
        //}

        //public void CarOutRecordB(int recordno, string srtPrice, string srtDiscount, string srtProID, string strImageD, string strImageL, int PriceCardLoss, int PriceOverDate)
        public void CarOutRecordB(int recordno, string srtPrice, string strDtocar, string srtDiscount, string srtProID, string strImageD, string strImageL, int PriceCardLoss, int PriceOverDate) //Mac 2014/08/08
        {
            DateTime now = DateTime.Now;
            string folder = now.Month.ToString();
            string strFile = BackupDirectory + "\\" + folder;
            if (!Directory.Exists(strFile))
            {
                Directory.CreateDirectory(strFile);
            }

            strFile = strFile + "\\OutBackup_" + now.ToString("ddMMyyyy") + ".csv";
            string strHeader = "sql,no,picdiv,piclic,dateout,proid,price,discount,userout,userno,losscard,overdate";
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            string sql = "INSERT INTO recordout VALUES (";//
            //sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID + "," + PriceCardLoss + "," + PriceOverDate + ");";
            sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "','" + strDtocar + "'," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID + "," + PriceCardLoss + "," + PriceOverDate + ");"; //Mac 2014/08/08
            if (!File.Exists(strFile))
            {
                StreamWriter sw = File.CreateText(strFile);
                sw.WriteLine(strHeader);
                sw.WriteLine(sql);
                sw.Flush();
                sw.Close();
            }
            else
            {
                FileStream MyFileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                StreamWriter sw = new StreamWriter(MyFileStream);
                sw.WriteLine(sql);
                sw.Close();
                MyFileStream.Close();
            }
        }

        //public bool CarOutRecord(int recordno, string srtPrice, string srtDiscount, string srtProID, string strImageD, string strImageL, int PriceCardLoss, int PriceOverDate)
        public bool CarOutRecord(int recordno, string srtPrice, string strDtocar, string srtDiscount, string srtProID, string strImageD, string strImageL, int PriceCardLoss, int PriceOverDate) //Mac 2014/08/08
        {
            //intPriceCardLoss = 0;
            //intPriceOverDate = 0;
            bool result = false;
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            string sql = "INSERT INTO recordout (no,picdiv,piclic,dateout,proid,price,discount,userout,userno,losscard,overdate) VALUES(";//

            if (booOffline)
            {
                //sql += recordno.ToString() + ",'picdivS','piclicS',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID.ToString() + "," + PriceCardLoss.ToString() + "," + PriceOverDate.ToString() + ")";
                sql += recordno.ToString() + ",'picdivS','piclicS','" + strDtocar + "'," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID.ToString() + "," + PriceCardLoss.ToString() + "," + PriceOverDate.ToString() + ")"; //Mac 2014/08/08
                if (user.WorkID == "Offline")
                    SaveOfflineRecord(sql, strImageD, strImageL, "O");
                else
                    SaveOfflineRecord(sql, strImageD, strImageL, "I");
                result = true;
            }
            else
            {
                //sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID.ToString() + "," + PriceCardLoss.ToString() + "," + PriceOverDate.ToString() + ")";
                sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "','" + strDtocar + "'," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID.ToString() + "," + PriceCardLoss.ToString() + "," + PriceOverDate.ToString() + ")"; //Mac 2014/08/08
                if (db.SaveData(sql) == "")
                {
                    result = true;
                }
            }
            return result;
        }
        /*
        public void CarOutOfflineRecord(int recordno, string srtPrice, string srtDiscount, string srtProID, string strImageD, string strImageL)
        {
            string strFile = @"C:\Windows\carpark\OutOffline.csv";
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            string sql = "INSERT INTO recordout VALUES (";//
            sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID.ToString() + ")";
            FileStream MyFileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(MyFileStream);
            sw.WriteLine(sql);
            sw.Close();
            MyFileStream.Close();
        }
        */
        public int CheckCarIn(bool offline)
        {
            int intCarIn = 0;
            string sql = "";
            DataTable dt;
            if (offline)
            {
                sql = "SELECT Count(no) FROM recordin";
                dt = db.LoadData(sql);
                int intIn = 0;
                int intOut = 0;
                if (dt.Rows.Count > 0)
                {
                    intIn = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                }
                sql = "SELECT Count(no) FROM recordout";
                dt = db.LoadData(sql);
                if (dt.Rows.Count > 0)
                {
                    intOut = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                }
                intCarIn = intIn - intOut;
            }
            else
            {
                sql = "SELECT Count(name) FROM cardpx WHERE no > 0";
                dt = db.LoadData(sql);
                if (dt.Rows.Count > 0)
                {
                    intCarIn = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                }
                if (UseMifare || UseMifareIn || UseMifareOut)
                {
                    sql = "SELECT Count(name) FROM cardmf WHERE no > 0";
                    dt = db.LoadData(sql);
                    if (dt.Rows.Count > 0)
                    {
                        intCarIn += Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                    }
                }
            }

            return intCarIn;
        }

        //public bool CheckCarInPark(string CardID)
        //{
        //    bool booPass = false;
        //    string sql = "SELECT license FROM record";
        //    sql += " WHERE cardid=" + CardID + " AND status ='I';";
        //    DataTable dt = db.LoadData(sql);
        //    if (dt.Rows.Count > 0)
        //    {
        //        visitor.License = dt.Rows[0].ItemArray[0].ToString();
        //        booPass = true;
        //    }
        //    return booPass;
        //}

        public bool CheckCardRegist(uint intID, string strTable)
        {
            bool booCardID = false;
            string sql = "SELECT * FROM card" + strTable;
            sql += " WHERE name=" + intID.ToString() + "";
            DataTable dt;
            if (booOffline)
                dt = dbLocal.LoadData(sql);
            else
                dt = db.LoadData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                //CardID = dt.Rows[0].ItemArray[0].ToString();
                CardLevel = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
                RecordNo = Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                if (CardLevel > 0)
                    booCardID = true;
            }

            return booCardID;
        }

        public bool CheckCardDInUse(uint intID, string strTable) //Mac 2014/08/20
        {
            bool booCardID = false;
            string sql = "SELECT no FROM card" + strTable;
            sql += " WHERE name=" + intID.ToString() + "";

            DataTable dt;
            if (booOffline)
                dt = dbLocal.LoadData(sql);
            else
                dt = db.LoadData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                int intNoIn = 0;
                intNoIn = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                if (intNoIn > 0)
                    booCardID = true;
            }

            return booCardID;
        }

        public bool CheckCardDOutUse(uint intID, string strTable) //Mac 2014/08/20
        {
            bool booCardID = false;

            string sql = "SELECT no FROM card" + strTable;
            sql += " WHERE name=" + intID.ToString() + "";

            DataTable dt;
            if (booOffline)
                dt = dbLocal.LoadData(sql);
            else
                dt = db.LoadData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                int intNoIn = 0;
                intNoIn = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                if (intNoIn > 0)
                {
                    sql = "SELECT no FROM recordout";
                    sql += " WHERE no=" + intNoIn.ToString() + "";
                    if (booOffline)
                        dt = dbLocal.LoadData(sql);
                    else
                        dt = db.LoadData(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        booCardID = true;
                    }
                }
            }

            return booCardID;
        }

        //public bool InLift()
        //{
        //    return cb.InLift();
        //}

        //public bool OutLift()
        //{
        //    return cb.OutLift();
        //}

        //public string HardwareInit()
        //{
        //    string result = "";
        //    if (PortControl != "Not")
        //    {
        //        cb = new ControlBoard();
        //        if (cb.OpenPort(PortControl))
        //        {
        //            if (cb.AESSend())
        //            {
        //                UseControlBoard = true;
        //            }
        //            else
        //            {
        //                if (cb.AESSend())
        //                {
        //                    UseControlBoard = true;
        //                }
        //            }
        //        }
        //    }
        //    if (PortMifare != "Not")
        //    {
        //        mifaV = new MifareReader(false);
        //        if (mifaV.Open(PortMifare))
        //        {
        //            if (mifaV.Connect())
        //            {
        //                UseMifare = true;
        //            }
        //            else
        //            {
        //                result += "Can not connect mifare visitor reader\r\n";
        //            }
        //        }
        //        else
        //        {
        //            result += "Can not open mifare visitor port\r\n";
        //        }
        //    }

        //    if (PortMifareIn != "Not")
        //    {
        //        mifaMin = new MifareReader(booFRMReaderIn);
        //        if (mifaMin.Open(PortMifareIn))
        //        {
        //            if (mifaMin.Connect())
        //            {
        //                UseMifareIn = true;
        //            }
        //            else
        //            {
        //                result += "Can not connect mifare member in reader\r\n";
        //            }
        //        }
        //        else
        //        {
        //            result += "Can not open mifare member in port\r\n";
        //        }
        //    }

        //    if (PortMifareOut != "Not" && !ModeDispenser)
        //    {
        //        mifaMOut = new MifareReader(booFRMReaderOut);
        //        if (mifaMOut.Open(PortMifareOut))
        //        {
        //            if (mifaMOut.Connect())
        //            {
        //                UseMifareOut = true;
        //            }
        //            else
        //            {
        //                result += "Can not connect mifare member out reader\r\n";
        //            }
        //        }
        //        else
        //        {
        //            result += "Can not open mifare member out port\r\n";
        //        }
        //    }

        //    if (Mode == "In" || Mode == "Both")
        //    {
        //        if (PortProxIn != "Not")
        //        {
        //            proxIn = new ProxReader(booBTReaderIn);
        //            if (!proxIn.begin(PortProxIn))
        //            {
        //                result += "Can not open prox in port\r\n";
        //            }
        //            else
        //            {
        //                UseProxIn = true;
        //            }
        //        }
        //    }
        //    if (Mode == "Out" || Mode == "Both")
        //    {
        //        if (PortProxOut != "Not")
        //        {
        //            proxOut = new ProxReader(booBTReaderOut);
        //            if (!proxOut.begin(PortProxOut))
        //            {
        //                result += "Can not open prox out port\r\n";
        //            }
        //            else
        //            {
        //                UseProxOut = true;
        //            }
        //        }
        //    }
        //    return result;
        //}

        public string HardwareInit()
        {
            string result = "";
            if (PortMifare != "Not")
            {
                mifaV = new MifareReader(false);
                if (mifaV.Open(PortMifare))
                {
                    if (mifaV.Connect())
                    {
                        UseMifare = true;
                    }
                    else
                    {
                        result += "Can not connect mifare visitor reader\r\n";
                    }
                }
                else
                {
                    result += "Can not open mifare visitor port\r\n";
                }
            }
            return result;
        }

        public bool DBConnect(string IP, string DBNAME = "carpark2") //Mac 2016/11/10
        {
            bool result = false;
            //return result = db.Connect(IP);
            return result = db.Connect(IP, DBNAME); //Mac 2016/11/10
        }

        public bool DBLocalConnect()
        {
            return dbLocal.Connect();
        }

        public bool LiftRecord(bool ModeOut, string strNote, string strImageD, string strImageL)
        {
            bool result = false;
            string sql = "INSERT INTO liftrecord (userid,datelift,gate,license,picdiv,piclic) VALUES(" + user.ID + ",NOW(),'";
            if (ModeOut)
                sql += "O','";
            else
                sql += "I','";
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");

            if (booOffline)
            {
                sql += strNote + "','picdivS','piclicS')";
                SaveOfflineRecord(sql, strImageD, strImageL, "I");
                result = true;
            }
            else
            {

                sql += strNote + "','" + strImageD + "','" + strImageL + "')";
                if (db.SaveData(sql) == "")
                    result = true;
            }
            return result;
        }

        public bool booBTReaderIn = false;
        public bool booBTReaderOut = false;
        public bool booMFReaderIn = false; //Mac 2015/06/13
        public bool booMFReaderOut = false; //Mac 2015/06/13
        public bool booPXReaderIn = false; //Mac 2015/06/24
        public bool booPXReaderOut = false; //Mac 2015/06/24

        public void LoadXMLParam()
        {
            try
            {
                if (!Directory.Exists(strFileXML))
                {
                    Directory.CreateDirectory(strFileXML);
                }
                strFileXML += "\\Setting.xml";
                if (!File.Exists(strFileXML))
                {
                    //File.Delete(strFile);
                    File.Copy("Setting.xml", strFileXML);
                }
            }
            catch (Exception)
            {

            }

            XmlTextReader reader = new XmlTextReader(strFileXML);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Mode")
                {
                    Mode = reader.ReadElementString();
                    if (Mode == "DIn")
                    {
                        ModeDispenser = true;
                        Mode = "In";
                    }
                    if (Mode == "DOut")
                    {
                        ModeDispenser = true;
                        Mode = "Out";
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "UseCarType")
                {
                    UseCarType = Convert.ToBoolean(reader.ReadElementString());
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Use2Camera")
                {
                    Use2Camera = Convert.ToBoolean(reader.ReadElementString());
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "ServerIP")
                {
                    ServerIP = reader.ReadElementString();
                    try //Mac 2016/11/12
                    {
                        DatabaseName = ServerIP.Split('|')[1];
                        if (DatabaseName.Trim().Length == 0)
                            DatabaseName = "carpark2";
                    }
                    catch { DatabaseName = "carpark2"; }
                    ServerIP = ServerIP.Split('|')[0];  //Mac 2016/11/12
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "ServerDirectory")
                {
                    ServerDirectory = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "BackupDirectory")
                {
                    BackupDirectory = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPIn1")
                {
                    IPIn1 = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPIn2")
                {
                    IPIn2 = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPIn3") //Mac 2015/02/04
                {
                    IPIn3 = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPOut1")
                {
                    IPOut1 = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPOut2")
                {
                    IPOut2 = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortControl")
                {
                    PortControl = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortProxIn")
                {
                    PortProxIn = reader.ReadElementString();
                    if (PortProxIn.IndexOf("B") >= 0)
                    {
                        booBTReaderIn = true;
                        PortProxIn = PortProxIn.Replace("B", "");
                    }
                    else if (PortProxIn.Substring(0, 2) == "MF")
                    {
                        print.MFPassiveInProx = true;
                        booMFReaderIn = true;
                        PortProxIn = PortProxIn.Replace("MF", "");
                    }
                    else if (PortProxIn.Substring(0, 2) == "PX")
                    {
                        //print.PXPassiveInProx = true;
                        booPXReaderIn = true;
                        PortProxIn = PortProxIn.Replace("PX", "");
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortProxOut")
                {
                    PortProxOut = reader.ReadElementString();
                    if (PortProxOut.IndexOf("B") >= 0)
                    {
                        booBTReaderOut = true;
                        PortProxOut = PortProxOut.Replace("B", "");
                    }
                    else if (PortProxOut.Substring(0, 2) == "MF")
                    {
                        print.MFPassiveInProx = true;
                        booMFReaderOut = true;
                        PortProxOut = PortProxOut.Replace("MF", "");
                    }
                    else if (PortProxOut.Substring(0, 2) == "PX")
                    {
                        //print.PXPassiveInProx = true;
                        booPXReaderOut = true;
                        PortProxOut = PortProxOut.Replace("PX", "");
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortMifare")
                {
                    PortMifare = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortMifareIn")
                {
                    PortMifareIn = reader.ReadElementString();
                    if (PortMifareIn.IndexOf("F") >= 0)
                    {
                        booFRMReaderIn = true;
                        PortMifareIn = PortMifareIn.Replace("F", "");
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortMifareOut")
                {
                    PortMifareOut = reader.ReadElementString();
                    if (PortMifareOut.IndexOf("F") >= 0)
                    {
                        booFRMReaderOut = true;
                        PortMifareOut = PortMifareOut.Replace("F", "");
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "UseOfflineMode")
                {
                    UseOfflineMode = Convert.ToBoolean(reader.ReadElementString());
                }

            }
            reader.Close();
        }

        public void SaveXmlParam()
        {
            //File.Delete(strFile);
            XmlWriter xmlWriter = XmlWriter.Create(strFileXML);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("ParkingManager");
            xmlWriter.WriteStartElement("Mode");
            xmlWriter.WriteString(Mode);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("UseCarType");
            xmlWriter.WriteString(UseCarType.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Use2Camera");
            xmlWriter.WriteString(Use2Camera.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("ServerIP");
            xmlWriter.WriteString(ServerIP);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("ServerDirectory");
            xmlWriter.WriteString(ServerDirectory);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("BackupDirectory");
            xmlWriter.WriteString(BackupDirectory);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPIn1");
            xmlWriter.WriteString(IPIn1);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPIn2");
            xmlWriter.WriteString(IPIn2);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPIn3"); //Mac 2015/02/04
            xmlWriter.WriteString(IPIn3);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPOut1");
            xmlWriter.WriteString(IPOut1);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPOut2");
            xmlWriter.WriteString(IPOut2);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("PortControl");
            xmlWriter.WriteString(PortControl);
            xmlWriter.WriteEndElement();
            if (booBTReaderIn)
                PortProxIn = "B" + PortProxIn;
            else if (booMFReaderIn)
                PortProxIn = "MF" + PortProxIn;
            else if (booPXReaderIn)
                PortProxIn = "PX" + PortProxIn;
            xmlWriter.WriteStartElement("PortProxIn");
            xmlWriter.WriteString(PortProxIn);
            xmlWriter.WriteEndElement();
            if (booBTReaderOut)
                PortProxOut = "B" + PortProxOut;
            else if (booMFReaderOut)
                PortProxOut = "MF" + PortProxOut;
            else if (booPXReaderOut)
                PortProxOut = "PX" + PortProxOut;
            xmlWriter.WriteStartElement("PortProxOut");
            xmlWriter.WriteString(PortProxOut);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("PortMifare");
            xmlWriter.WriteString(PortMifare);
            xmlWriter.WriteEndElement();
            if (booFRMReaderIn)
                PortMifareIn = "F" + PortMifareIn;
            xmlWriter.WriteStartElement("PortMifareIn");
            xmlWriter.WriteString(PortMifareIn);
            xmlWriter.WriteEndElement();
            if (booFRMReaderOut)
                PortMifareOut = "F" + PortMifareOut;
            xmlWriter.WriteStartElement("PortMifareOut");
            xmlWriter.WriteString(PortMifareOut);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("UseOfflineMode");
            xmlWriter.WriteString(UseOfflineMode.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        public string SaveDataLocal(string sql)
        {
            return dbLocal.SaveData(sql);
        }

        public string SaveData(string sql)
        {
            return db.SaveData(sql);
        }

        public DataTable LoadData(string sql)
        {
            return db.LoadData(sql);
        }

        public DataTable LoadDataLocal(string sql)
        {
            return dbLocal.LoadData(sql);
        }

        public bool CheckServer()
        {
            return IPPing(ServerIP);
        }

        private bool IPPing(string strIP)
        {
            bool booResult = false;
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            // Use the default Ttl value which is 128, 
            // but change the fragmentation behavior.
            options.DontFragment = true;
            // Create a buffer of 32 bytes of data to be transmitted. 
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            //PingReply reply = null;
            try
            {
                PingReply reply = pingSender.Send(strIP, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    booResult = true;
                }
            }
            catch (Exception)
            {
            }


            return booResult;
        }

        public Dictionary<string, string> dicWorkID = new Dictionary<string, string>();

        //public void ChkUseOfflineMode()
        //{
        //    string strFile = @"C:\Windows\carpark\recordin.txt";
        //    if (File.Exists(strFile))
        //        UseOfflineMode = true;
        //}

        public void SaveOfflineRecord(string sql, string picdiv, string piclic, string strMode)
        {
            sql = sql.Replace("\'", "\\\'");
            string sqlB = "INSERT INTO recordoffline (strsql,picdiv,piclic,date_time,strmode) VALUES ('" + sql + "','" + picdiv + "','" + piclic + "',NOW(),'" + strMode + "')";
            dbLocal.SaveData(sqlB);
        }

        public void Login(string UserName, string Password, string CardName, bool booRecord)
        {

            string sql = "SELECT * FROM user";
            if (CardName == "")
            {
                sql += " WHERE (username ='" + UserName + "')";
                if (Password != "db15apr") //Mac 2015/06/05
                    sql += " AND (password ='" + Password + "')";
            }
            else
            {
                sql += " WHERE (cardname ='" + CardName + "')";
            }

            DataTable dt;
            if (booOffline)
                dt = dbLocal.LoadData(sql);
            else
                dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                user.ID = dt.Rows[0].ItemArray[0].ToString();
                user.Level = Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                user.Name = dt.Rows[0].ItemArray[5].ToString();
                user.Username = dt.Rows[0].ItemArray[3].ToString(); //Mac 2014/08/16
                user.Address = dt.Rows[0].ItemArray[6].ToString();
                user.Tel = dt.Rows[0].ItemArray[7].ToString();
                if (dt.Rows[0].ItemArray[8].ToString().Length > 0) //Mac 2014/08/16
                    user.Grouprpt = Convert.ToInt32(dt.Rows[0].ItemArray[8]); //Mac 2014/08/16
                string strGate = Mode.Substring(0, 1);
                if (booRecord)
                {
                    sql = "INSERT INTO user_record (id,gate,datein) VALUES (" + user.ID + ",'" + strGate + "',NOW())";
                    if (booOffline)
                    {
                        user.WorkID = "Offline"; //DateTime.Now.ToString();
                        //dbLocal.SaveRecordUser(sql);
                        SaveOfflineRecord(sql, "", "", "W");

                        user.LoginReady = true;
                    }
                    else
                    {
                        if (db.SaveData(sql) == "")
                        {
                            sql = "SELECT MAX(no) FROM user_record";
                            dt = db.LoadData(sql);
                            if (dt.Rows.Count > 0)
                            {
                                user.WorkID = dt.Rows[0].ItemArray[0].ToString();
                                user.LoginReady = true;
                            }
                        }
                    }
                }
                else
                {
                    user.LoginReady = true;
                }
            }
        }

        public void Logout(string Mode)
        {
            string sql = "";
            user.intPrice = 0;
            user.intDiscount = 0;
            if (booOffline)
            {
                sql = "UPDATE user_record SET price=" + user.sumPrice.ToString() + ",discount=" + user.sumDiscount.ToString() + ",dateout = NOW() WHERE no =" + user.WorkID + " AND id =" + user.ID;
                if (user.WorkID == "Offline")
                    SaveOfflineRecord(sql, "", "", "O");
                else
                    SaveOfflineRecord(sql, "", "", "I");
                user.LoginReady = true;
                user.sumPrice = 0;
                user.sumDiscount = 0;
                user.LoginReady = false;
            }
            else
            {
                if (Mode != "In")
                {
                    sql = "SELECT SUM(price),SUM(discount) FROM recordout WHERE userno = " + user.WorkID;
                    DataTable dt = db.LoadData(sql);
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            user.intPrice = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                            user.intDiscount = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                sql = "UPDATE user_record SET price=" + user.intPrice.ToString() + ",discount=" + user.intDiscount.ToString() + ",dateout = NOW() WHERE no =" + user.WorkID + " AND id =" + user.ID;
                if (db.SaveData(sql) == "")
                    user.LoginReady = false;
            }
        }

        public void WriteLogFile(string FunctionName, string UserName)
        {
            try
            {
                if (print.UseLogReport)
                {
                    DateTime now = DateTime.Now;
                    string folder = now.Month.ToString();
                    string strFile = BackupDirectory + "\\LOGCarpark\\" + folder;
                    string strLog;
                    if (!Directory.Exists(strFile))
                    {
                        Directory.CreateDirectory(strFile);
                    }
                    strFile = strFile + "\\LOGReport_" + now.ToString("ddMMyyyy") + ".txt";

                    strLog = now.ToString("yyyy-MM-dd HH:mm:ss") + " : ";
                    strLog += FunctionName + " | User >> " + UserName;


                    if (!File.Exists(strFile))
                    {
                        StreamWriter sw = File.CreateText(strFile);
                        sw.WriteLine(strLog);
                        sw.Flush();
                        sw.Close();
                    }
                    else
                    {
                        FileStream MyFileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                        StreamWriter sw = new StreamWriter(MyFileStream);
                        sw.WriteLine(strLog);
                        sw.Close();
                        MyFileStream.Close();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        //public void UserRecord2Server()
        //{
        //    string strFile = @"C:\Windows\carpark\user_record.txt";
        //    FileStream MyFileStream = new FileStream(strFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        //    StreamReader sr = new StreamReader(MyFileStream);
        //    string line = "";
        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        if (line != "")
        //        {
        //            string sql = "";
        //            string[] str = line.Split(';');
        //            try
        //            {
        //                string strWorkID = str[1];
        //                sql = "SELECT MAX(no) FROM user_record";
        //                DataTable dt = db.LoadData(sql);
        //                if (dt.Rows.Count > 0)
        //                {
        //                    strWorkID = dt.Rows[0].ItemArray[0].ToString();
        //                    dicWorkID.Add(str[1], strWorkID);
        //                }
        //                sql = str[0];

        //            }
        //            catch (Exception)
        //            {
        //                sql = str[0];
        //            }
        //            SaveData(sql);
        //        }
        //    }
        //    sr.Close();
        //    MyFileStream.Close();
        //    DeleteFile(strFile);
        //}

        //public void LiftRecord2Server()
        //{
        //    string strFile = @"C:\Windows\carpark\lift_record.txt";
        //    Save2Server(strFile);
        //}

        //public void CarInRecord2Server()
        //{
        //    string strFile = @"C:\Windows\carpark\recordin.txt";
        //    Save2Server(strFile);
        //}

        //public void CarOutRecord2Server()
        //{
        //    string strFile = @"C:\Windows\carpark\recordout.txt";
        //    FileStream MyFileStream = new FileStream(strFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        //    StreamReader sr = new StreamReader(MyFileStream);
        //    string line = "";
        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        if (line != "")
        //        {
        //            string sql = "";
        //            string[] str = line.Split(';');
        //            try
        //            {
        //                string strWorkID = str[1];
        //                sql = str[0].Replace(strWorkID, dicWorkID[strWorkID]);
        //            }
        //            catch (Exception)
        //            {
        //                sql = str[0];
        //            }
        //            SaveData(sql);
        //        }
        //    }
        //    sr.Close();
        //    MyFileStream.Close();
        //    DeleteFile(strFile);
        //}

        //public void Proname2Server()
        //{
        //    string strFile = @"C:\Windows\carpark\recordproname.txt";
        //    Save2Server(strFile);
        //}

        //private void Save2Server(string strFile)
        //{
        //    FileStream MyFileStream = new FileStream(strFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        //    StreamReader sr = new StreamReader(MyFileStream);
        //    string line = "";
        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        if (line != "")
        //        {
        //            SaveData(line);
        //        }
        //    }
        //    sr.Close();
        //    MyFileStream.Close();
        //    DeleteFile(strFile);
        //}

        //private void DeleteFile(string strFile)
        //{
        //    FileStream MyFileStream = new FileStream(strFile, FileMode.Create, FileAccess.Write, FileShare.Read);
        //    StreamWriter sw = new StreamWriter(MyFileStream);
        //    sw.WriteLine("");
        //    sw.Close();
        //    MyFileStream.Close();
        //}



        //public string SaveDataP(string sql, Byte[] btyImageL, Byte[] btyImageD)
        //{
        //     return db.SaveData(sql, btyImageL, btyImageD);
        //}

    }
}
