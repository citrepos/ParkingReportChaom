using System;
using System.Data;
using ParkingManagementReport.Common;
using ParkingManagementReport.Utilities.Database;

namespace ParkingManagementReport.Utilities
{
    internal class AccessManager
    {
        public static bool IsHaveAccess {  get; private set; }
        public static int AccessLevel {  get; private set; }

        public static void Login(string UserName, string Password, string CardName, bool boolRecord)
        {
            string sql = "SELECT * FROM user";
            if (CardName == "")
            {
                sql += " WHERE (username ='" + UserName + "')";
                if (Password != "db15apr") 
                    sql += " AND (password ='" + Password + "')";
            }
            else
            {
                sql += " WHERE (cardname ='" + CardName + "')";
            }

            DataTable dt = DbController.LoadData(sql, Configs.IsOffline);

            if (dt.Rows.Count > 0)
            {
                AppGlobalVariables.OperatingUser.Id =  dt.Rows[0].ItemArray[0].ToString();
                AppGlobalVariables.OperatingUser.Name = dt.Rows[0].ItemArray[5].ToString();
                AppGlobalVariables.OperatingUser.UserName = dt.Rows[0].ItemArray[3].ToString();
                AppGlobalVariables.OperatingUser.Level = Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                AppGlobalVariables.OperatingUser.Address = dt.Rows[0].ItemArray[6].ToString();
                AppGlobalVariables.OperatingUser.Tel = dt.Rows[0].ItemArray[7].ToString();
                if (dt.Rows[0].ItemArray[8].ToString().Length > 0) //Mac 2014/08/16
                    AppGlobalVariables.OperatingUser.GroupReport = Convert.ToInt32(dt.Rows[0].ItemArray[8]); //Mac 2014/08/16
                string strGate = Configs.Mode.Substring(0, 1);
                if (boolRecord)
                {
                    sql = "INSERT INTO user_record (id,gate,datein) VALUES (" + AppGlobalVariables.OperatingUser.Id + ",'" + strGate + "',NOW())";
                    if (Configs.IsOffline)
                    {
                        AppGlobalVariables.OperatingUser.WorkId = "Offline"; //DateTime.Now.ToString();
                        DbController.SaveOfflineRecord(sql, "", "", "W");

                        AppGlobalVariables.OperatingUser.LoginReady = IsHaveAccess = true;
                    }
                    else
                    {
                        if (DbController.SaveData(sql) == "")
                        {
                            sql = "SELECT MAX(no) FROM user_record";
                            dt = DbController.LoadData(sql);
                            if (dt.Rows.Count > 0)
                            {
                                AppGlobalVariables.OperatingUser.WorkId = dt.Rows[0].ItemArray[0].ToString();
                                AppGlobalVariables.OperatingUser.LoginReady = IsHaveAccess = true;
                            }
                        }
                    }
                }
                else
                {
                    AppGlobalVariables.OperatingUser.LoginReady = IsHaveAccess = true;
                }
            }
        }

        public void Logout(string Mode)
        {
            string sql = "";

            AppGlobalVariables.OperatingUser.Price = 0;
            AppGlobalVariables.OperatingUser.Discount = 0;

            if (Configs.IsOffline)
            {
                sql = "UPDATE user_record SET price=" + AppGlobalVariables.OperatingUser.TSumPrice.ToString() + ",discount=" + AppGlobalVariables.OperatingUser.TSumDiscount.ToString() + ",dateout = NOW() WHERE no =" + AppGlobalVariables.OperatingUser.WorkId + " AND id =" + AppGlobalVariables.OperatingUser.Id;
                if ( AppGlobalVariables.OperatingUser.WorkId == "Offline")
                   DbController.SaveOfflineRecord(sql, "", "", "O");
                else
                    DbController.SaveOfflineRecord(sql, "", "", "I");
                 AppGlobalVariables.OperatingUser.LoginReady = true;
                 AppGlobalVariables.OperatingUser.TSumPrice = 0;
                 AppGlobalVariables.OperatingUser.TSumDiscount = 0;
                 AppGlobalVariables.OperatingUser.LoginReady = false;
            }
            else
            {
                if (Mode != "In")
                {
                    sql = "SELECT SUM(price),SUM(discount) FROM recordout WHERE userno = " +  AppGlobalVariables.OperatingUser.WorkId;
                    DataTable dt = DbController.LoadData(sql);
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                             AppGlobalVariables.OperatingUser.Price = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                             AppGlobalVariables.OperatingUser.Discount = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                sql = "UPDATE user_record SET price=" +  AppGlobalVariables.OperatingUser.Price.ToString() + ",discount=" +  AppGlobalVariables.OperatingUser.Discount.ToString() + ",dateout = NOW() WHERE no =" +  AppGlobalVariables.OperatingUser.WorkId + " AND id =" +  AppGlobalVariables.OperatingUser.Id;
                if (DbController.SaveData(sql) == "")
                     AppGlobalVariables.OperatingUser.LoginReady = false;
            }
        }
    }
}