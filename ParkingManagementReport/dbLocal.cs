using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ParkingManagementReport
{
    public static class dbLocal
    {
        private static MySqlConnection mscon;
        //private static MySqlConnection msconb;

        public static DataTable LoadData(string sql)
        {
            DataTable dt = null;
            DataSet ds = new DataSet();
            try
            {
                Open();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, mscon);
                da.Fill(ds, "Table");
                dt = ds.Tables["Table"];
                mscon.Close();
            }
            catch (Exception)
            {
            }
            return dt;
        }

        public static int LoadNO()
        {
            int no = 0;
            DataTable dt = null;
            DataSet ds = new DataSet();
            try
            {
                Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT no FROM recordno", mscon);
                da.Fill(ds, "Table");
                dt = ds.Tables["Table"];
                no = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                mscon.Close();
            }
            catch (Exception)
            {
            }
            return no;
        }

        public static string SaveNO(int no)
        {
            string result = "";
            MySqlCommand cmd;
            string sql = "UPDATE recordno SET no=" + no.ToString();
            try
            {
                Open();
                cmd = new MySqlCommand(sql, mscon);
                cmd.ExecuteNonQuery();
                mscon.Close();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        //public static void Connect()
        //{
        //    string strConnMySQL = "Database=carpark2;Data Source=127.0.0.1;User Id=cit;Password=db13apr;Charset=utf8;";
        //    mscon = new MySqlConnection(strConnMySQL);
        //    mscon.Open();
        //    mscon.Close();
        //}

        public static bool Connect()
        {
            string strConnMySQL = "Database=carpark2;Data Source=127.0.0.1;User Id=cit;Password=Psk9Cmt854;Charset=utf8;";
            //string strConnMySQL = "Database=carparkoffline;Data Source=127.0.0.1;User Id=cit;Password=db13apr;Charset=utf8;";
            bool booConnect = false;
            try
            {
                mscon = new MySqlConnection(strConnMySQL);
                mscon.Open();
                mscon.Close();
                //msconb = new MySqlConnection(strConnMySQLB);
                //msconb.Open();
                //msconb.Close();
                booConnect = true;
                //IPServerMain = strIP;
            }
            catch (Exception)
            {
            }
            return booConnect;
        }

        private static void Open()
        {
            if (mscon.State == ConnectionState.Open)
                mscon.Close();
            mscon.Open();
        }

        //private static void OpenB()
        //{
        //    if (msconb.State == ConnectionState.Open)
        //        msconb.Close();
        //    msconb.Open();
        //}

        public static string SaveData(string sql)
        {
            string result = "";
            MySqlCommand cmd;
            try
            {
                Open();
                cmd = new MySqlCommand(sql, mscon);
                cmd.ExecuteNonQuery();
                mscon.Close();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }


        /*
        public static void SaveRecordCarIn(string sql)
        {
            string strFile = @"C:\Windows\carpark\recordin.txt";
            SaveFlle(sql, strFile);
        }

        public static void SaveImage(string sql)
        {
            string strFile = @"C:\Windows\carpark\image.txt";
            SaveFlle(sql, strFile);
        }

        public static void SaveRecordCarOut(string sql)
        {
            string strFile = @"C:\Windows\carpark\recordout.txt";
            SaveFlle(sql, strFile);
        }

        public static void SaveRecordUser(string sql)
        {
            string strFile = @"C:\Windows\carpark\user_record.txt";
            SaveFlle(sql, strFile);
        }

        public static void SaveRecordLift(string sql)
        {
            string strFile = @"C:\Windows\carpark\lift_record.txt";
            SaveFlle(sql, strFile);
        }

        public static void SaveRecordProname(string sql)
        {
            string strFile = @"C:\Windows\carpark\recordproname.txt";
            SaveFlle(sql, strFile);
        }

        private static void SaveFlle(String sql, string strFile)
        {
            FileStream MyFileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(MyFileStream);
            DateTime dti = DateTime.Now;
            dti = dti.AddYears(-543);
            string strD = dti.ToString("yyyy-MM-dd HH:mm:ss");
            strD = "'" + strD + "'";
            sql = sql.Replace("NOW()", strD);
            sw.WriteLine(sql);
            sw.Close();
            MyFileStream.Close();
        }
        */
    }
}
