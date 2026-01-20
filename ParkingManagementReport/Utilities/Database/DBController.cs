using System;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using MySql.Data.MySqlClient;

namespace ParkingManagementReport.Utilities.Database
{
    public static class DbController
    {
        #region FIELDS
        private static MySqlConnection remoteConnection;

        private static MySqlConnection localConnection;

        //private static readonly string dbPassword = "Psk9Cmt854";
        private static readonly string dbPassword = "db13apr";

        private static readonly string localIp = "127.0.0.1";
        #endregion FIELDS_END

        public static DataTable LoadData(string sql, bool isLocal = false)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            var connection = isLocal ? localConnection : remoteConnection;

            try
            {
                Open(connection);
                using (MySqlDataAdapter da = new MySqlDataAdapter(sql, connection))
                {
                    da.SelectCommand.CommandTimeout = 1000;
                    da.Fill(ds, "Table");
                    dt = ds.Tables["Table"];
                }
                connection.Close();
            }
            catch { }

            return dt;
        }

        public static string SaveData(string sql, bool isLocal = false)
        {
            string result = "";
            var connection = isLocal ? localConnection : remoteConnection;

            try
            {
                Open(connection);
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        public static void SaveOfflineRecord(string sql, string driverImage, string licenseImage, string mode)
        {
            sql = sql.Replace("\'", "\\\'");

            try
            {
                string query = $"INSERT INTO recordoffline (strsql,picdiv,piclic,date_time,strmode) VALUES ('{sql}','{driverImage}','{licenseImage}',NOW(),'{mode}')";

                SaveData(query, true);
            }
            catch
            {
                MessageBox.Show("[SaveOfflineRecordSuccess] failed, sql : " + sql);
            }

        }

        public static int LoadNO()
        {
            int no = 0;
            try
            {
                var dt = LoadData("SELECT no FROM recordno", true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    no = Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            catch (Exception)
            {
                // Consider logging the exception
            }
            return no;
        }

        public static string SaveNO(int no)
        {
            return SaveData($"UPDATE recordno SET no={no}", true);
        }

        public static bool Connect(string strIP, string strDatabase = "carpark2_donki_thonglor", bool isLocal = false)
        {
            localConnection = new MySqlConnection();
            remoteConnection = new MySqlConnection();

            string strConnMySQL = isLocal
                ? $"Database=carpark2;Data Source={localIp};User Id=cit;Password={dbPassword};Charset=utf8;"
                : $"Database={strDatabase};Data Source={strIP};User Id=cit;Password={dbPassword};Charset=utf8;";

            bool booConnect = false;
            try
            {
                MySqlConnection connection = new MySqlConnection(strConnMySQL);
                connection.Close();
                connection.Open();
                connection.Close();

                if (isLocal)
                    localConnection = connection;
                else
                    remoteConnection = connection;

                booConnect = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MySQL Error: {ex.GetType()} - {ex.Message}");
                booConnect = false;
            }
            return booConnect;
        }


        #region HELPERS
        private static void Open(MySqlConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
        }

        private static bool IPPing(string strIP)
        {
            bool booResult = false;
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

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
        #endregion
    }
}

/* public static class dbLocal
    {
        private static MySqlConnection msConnectionLocal;
        public static DataTable LoadData(string sql)
        {
            DataTable dt = null;
            DataSet ds = new DataSet();
            try
            {
                Open();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, msConnectionLocal);
                da.Fill(ds, "Table");
                dt = ds.Tables["Table"];
                msConnectionLocal.Close();
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
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT no FROM recordno", msConnectionLocal);
                da.Fill(ds, "Table");
                dt = ds.Tables["Table"];
                no = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                msConnectionLocal.Close();
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
                cmd = new MySqlCommand(sql, msConnectionLocal);
                cmd.ExecuteNonQuery();
                msConnectionLocal.Close();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        public static bool Connect()
        {
            string strConnMySQL = "Database=carpark2;Data Source=127.0.0.1;User Id=cit;Password=Psk9Cmt854;Charset=utf8;";
            bool booConnect = false;
            try
            {
                msConnectionLocal = new MySqlConnection(strConnMySQL);
                msConnectionLocal.Open();
                msConnectionLocal.Close();
                booConnect = true;
            }
            catch (Exception)
            {
            }
            return booConnect;
        }

        private static void Open()
        {
            if (msConnectionLocal.State == ConnectionState.Open)
                msConnectionLocal.Close();
            msConnectionLocal.Open();
        }

        public static string SaveData(string sql)
        {
            string result = "";
            MySqlCommand cmd;
            try
            {
                Open();
                cmd = new MySqlCommand(sql, msConnectionLocal);
                cmd.ExecuteNonQuery();
                msConnectionLocal.Close();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
    }
 
 */