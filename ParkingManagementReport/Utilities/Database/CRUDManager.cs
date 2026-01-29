using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ParkingManagementReport.Common;
using ParkingManagementReport.Utilities.Formatters;

using Constants = ParkingManagementReport.Common.Constants;
using DataTable = System.Data.DataTable;

namespace ParkingManagementReport.Utilities.Database
{
    internal class CRUDManager
    {
        #region CREATE (insert)
        public static bool SaveLiftRecord(bool ModeOut, string strNote, string strImageD, string strImageL)
        {
            bool result = false;
            string sql = "INSERT INTO liftrecord (userid,datelift,gate,license,picdiv,piclic) VALUES(" + AppGlobalVariables.OperatingUser.Id + ",NOW(),'";
            if (ModeOut)
                sql += "O','";
            else
                sql += "I','";
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");

            if (Configs.IsOffline)
            {
                sql += strNote + "','picdivS','piclicS')";
                SaveOfflineRecord(sql, strImageD, strImageL, "I");
                result = true;
            }
            else
            {

                sql += strNote + "','" + strImageD + "','" + strImageL + "')";
                if (DbController.SaveData(sql) == "")
                    result = true;
            }
            return result;
        }

        private static void SaveOfflineRecord(string sql, string picdiv, string piclic, string strMode)
        {
            sql = sql.Replace("\'", "\\\'");
            string sqlB = "INSERT INTO recordoffline (strsql,picdiv,piclic,date_time,strmode) VALUES ('" + sql + "','" + picdiv + "','" + piclic + "',NOW(),'" + strMode + "')";
            DbController.SaveData(sqlB, true);
        }
        #endregion


        #region READ (get)
        public string GetTimeFromDatabase()
        {
            string strTime = "";
            DataTable dt = DbController.LoadData("SELECT CURRENT_TIMESTAMP()");
            if (dt.Rows.Count > 0)
            {
                strTime = dt.Rows[0].ItemArray[0].ToString();
            }
            return strTime;
        }

        public static DataTable GetItemizedPromotionUsage(string sqlQuery, string paymentText)
        {
            DataTable Map = new DataTable("myMember");

            try
            {
                DataTable dt = DbController.LoadData(sqlQuery);

                DataRow dr = null;                                               // ชื่อเก่า -> for reference
                Map.Columns.Add(new DataColumn("หมายเลขบัตร", typeof(string)));    // no
                Map.Columns.Add(new DataColumn("เลขทะเบียน", typeof(string)));     // id
                Map.Columns.Add(new DataColumn("เวลาเข้า", typeof(string)));       // datein
                Map.Columns.Add(new DataColumn("เวลาออก", typeof(string)));       // dateout
                Map.Columns.Add(new DataColumn("ชม.ที่จอด", typeof(string)));      // ParkTime
                Map.Columns.Add(new DataColumn("ชม.ปัด", typeof(string)));
                Map.Columns.Add(new DataColumn("ค่าจอดทั้งหมด", typeof(string)));   
                Map.Columns.Add(new DataColumn("ส่วนลด", typeof(string)));        // ProDiscount
                Map.Columns.Add(new DataColumn("ชำระเงินเอง", typeof(string)));     // price
                Map.Columns.Add(new DataColumn("ค่าบริการเรียกเก็บ", typeof(string))); // Credit
                Map.Columns.Add(new DataColumn("ชื่อส่วนลด", typeof(string)));      // Proname
                Map.Columns.Add(new DataColumn("รหัสส่วนลด", typeof(string)));     // Proid
                Map.Columns.Add(new DataColumn("รหัสใบเสร็จ", typeof(string)));     // printno
                
                if(Configs.Reports.UseReportThanapoom) 
                    Map.Columns.Add(new DataColumn("tnpt_id", typeof(int)));        // tnpt_id

                if (Configs.Reports.UseReport21_2)
                    Map.Columns.Add(new DataColumn("Proprice", typeof(string)));

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dt3 = DbController.LoadData("SELECT value FROM param WHERE name = 'not_day'");
                    bool notDay = Convert.ToBoolean(dt3.Rows[0].ItemArray[0].ToString());
                    string stringDW = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = Map.NewRow();

                        AppGlobalVariables.IntTime = new int[0];
                        AppGlobalVariables.IntPriceMin = new int[0];
                        AppGlobalVariables.IntPriceHour = new int[0];
                        AppGlobalVariables.IntHourRound = new int[0];
                        AppGlobalVariables.IntExpense = new int[0];
                        AppGlobalVariables.IntOver = new int[0];

                        AppGlobalVariables.IntTime2 = new int[0];
                        AppGlobalVariables.IntPriceMin2 = new int[0];
                        AppGlobalVariables.IntPriceHour2 = new int[0];
                        AppGlobalVariables.IntHourRound2 = new int[0];
                        AppGlobalVariables.IntExpense2 = new int[0];
                        AppGlobalVariables.IntOver2 = new int[0];

                        object rawProId = dt.Rows[i]["proid"];
                        int currentProId = Convert.ToInt32(rawProId);
                        string currentProName = (AppGlobalVariables.PromotionNamesById.TryGetValue(currentProId, out var name))
                            ? AppGlobalVariables.PromotionNamesById[currentProId] 
                            : "Unknown";

                        double parkingTime = Convert.ToDouble(
                                                (Configs.Reports.UseReport21_1 || Configs.Reports.UseReport21_2 || Configs.Reports.UseReport21_3)
                                                    ? dt.Rows[i]["hm"]
                                                    : dt.Rows[i]["tdf"]);

                        double parkingTimeRoundedUp = Math.Ceiling(parkingTime);

                        #region ReportProsetPriceDayWeek (Currently unused)
                        if (Configs.Reports.ReportProsetPriceDayWeek) //Mac 2019/05/27
                        {
                            MessageBox.Show("UNHANDLED `ReportProsetPriceDayWeek`");
                            return null;

                            /*int SumData0 = 0;
                            var CalPriceZone = (dynamic)null;
                            DateTime dti = DateTime.Parse(dt.Rows[i]["datein"].ToString());
                            DateTime dto = DateTime.Parse(dt.Rows[i]["dateout"].ToString());
                            DateTime dtInOne;
                            DateTime dtOutOne;
                            TimeSpan diffInOut = DateTime.Parse(dto.ToShortDateString()) - DateTime.Parse(dti.ToShortDateString());
                            dr["PriceList"] = "0";
                            int intFM = 0;
                            int intLM = 0;

                            bool boolNoRound = false;
                            boolNoRound = false;
                            for (int x = 0; x < diffInOut.Days + 1; x++)
                            {
                                int intMin = 0;
                                int ZoneMin = 0;
                                int intTotal = 0;
                                TimeSpan diffIO;
                                string ZoneStart = "";
                                string ZoneStop = "";

                                if (diffInOut.Days == 0)
                                {
                                    boolNoRound = true;
                                    dtInOne = dti;
                                    dtOutOne = dto;
                                }
                                else if (x == 0)
                                {
                                    dtInOne = dti;
                                    dtOutOne = DateTime.Parse(dti.ToShortDateString() + " 23:59:59");
                                }
                                else if (x == diffInOut.Days)
                                {
                                    dtInOne = DateTime.Parse(dto.ToShortDateString() + " 00:00:00");
                                    dtOutOne = dto;
                                }
                                else
                                {
                                    dtInOne = DateTime.Parse(dti.ToShortDateString() + " 00:00:00");
                                    dtOutOne = DateTime.Parse(dti.AddDays(1).ToShortDateString() + " 00:00:00");
                                }

                                diffIO = dtOutOne - dtInOne;
                                if (diffIO.Days > 0)
                                    intMin += diffIO.Days * 24 * 60;
                                intMin += diffIO.Hours * 60;
                                intMin += diffIO.Minutes;

                                stringDW = dtInOne.DayOfWeek.ToString().ToLower().Substring(0, 2);

                                if ((diffInOut.Days == 0) || (x == 0))
                                {
                                    DataTable dtFM = DbController.LoadData("select freemin, pricelimit from cartype_freemin_prosetprice_dayweek where typeid = " + dt.Rows[i]["cartype"] + " and dayweek like '%" + stringDW + "%'");
                                    if (dtFM != null && dtFM.Rows.Count > 0)
                                    {
                                        intFM = Convert.ToInt32(dtFM.Rows[0][0]);
                                        intLM = Convert.ToInt32(dtFM.Rows[0][1]);
                                    }
                                }

                                AppGlobalVariables.IntTime = new int[0];
                                AppGlobalVariables.IntPriceMin = new int[0];
                                AppGlobalVariables.IntPriceHour = new int[0];
                                AppGlobalVariables.IntHourRound = new int[0];
                                AppGlobalVariables.IntExpense = new int[0];
                                AppGlobalVariables.IntOver = new int[0];
                                sqlQuery = "select * from prosetprice where PromotionId = " + currentProId + " and dayweek like '%" + stringDW + "%' order by no";
                                DataTable dt2 = DbController.LoadData(sqlQuery);
                                if (dt2 != null && dt2.Rows.Count > 0)
                                {
                                    AppGlobalVariables.IntTime = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntPriceMin = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntPriceHour = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntHourRound = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntExpense = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntOver = new int[dt2.Rows.Count];
                                    for (int j = 0; j < dt2.Rows.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[3].ToString());
                                        }
                                        else
                                        {
                                            AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[3].ToString()) - Convert.ToInt32(dt2.Rows[j - 1].ItemArray[3].ToString());
                                        }
                                        AppGlobalVariables.IntPriceMin[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[4].ToString());
                                        AppGlobalVariables.IntPriceHour[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[5].ToString());
                                        AppGlobalVariables.IntHourRound[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[6].ToString());
                                        AppGlobalVariables.IntExpense[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[7].ToString());
                                        AppGlobalVariables.IntOver[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[8].ToString());
                                    }
                                }

                                AppGlobalVariables.IntTime2 = new int[0];
                                AppGlobalVariables.IntPriceMin2 = new int[0];
                                AppGlobalVariables.IntPriceHour2 = new int[0];
                                AppGlobalVariables.IntHourRound2 = new int[0];
                                AppGlobalVariables.IntExpense2 = new int[0];
                                AppGlobalVariables.IntOver2 = new int[0];
                                DataTable dt4 = DbController.LoadData("select * from prosetprice_zone where PromotionID = " + currentProId + " and dayweek like '%" + stringDW + "%' order by no");
                                if (dt4 != null && dt4.Rows.Count > 0)
                                {
                                    AppGlobalVariables.IntTime2 = new int[dt4.Rows.Count];
                                    AppGlobalVariables.IntPriceMin2 = new int[dt4.Rows.Count];
                                    AppGlobalVariables.IntPriceHour2 = new int[dt4.Rows.Count];
                                    AppGlobalVariables.IntHourRound2 = new int[dt4.Rows.Count];
                                    AppGlobalVariables.IntExpense2 = new int[dt4.Rows.Count];
                                    AppGlobalVariables.IntOver2 = new int[dt4.Rows.Count];
                                    for (int y = 0; y < dt4.Rows.Count; y++)
                                    {
                                        if (y == 0)
                                        {
                                            AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[3].ToString());
                                        }
                                        else
                                        {
                                            AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[3].ToString()) - Convert.ToInt32(dt4.Rows[y - 1].ItemArray[3].ToString());
                                        }
                                        AppGlobalVariables.IntPriceMin2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[4].ToString());
                                        AppGlobalVariables.IntPriceHour2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[5].ToString());
                                        AppGlobalVariables.IntHourRound2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[6].ToString());
                                        AppGlobalVariables.IntExpense2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[7].ToString());
                                        AppGlobalVariables.IntOver2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[8].ToString());
                                    }

                                    ZoneStart = dt4.Rows[0]["zone_start"].ToString();
                                    ZoneStop = dt4.Rows[0]["zone_stop"].ToString();
                                }

                                CalPriceZone = CalculationsManager.CalPriceZoneOneDay(0, dtInOne.ToString(), dtOutOne.ToString(), ZoneStart, ZoneStop, 0, 0, 0, boolNoRound);
                                ZoneMin = CalPriceZone.Key;

                                if (ZoneMin > 0)
                                {
                                    intMin -= ZoneMin;

                                    ZoneMin -= intFM;
                                    if (ZoneMin > 0)
                                    {
                                        intFM = 0;
                                    }
                                    else
                                    {
                                        ZoneMin = 0;
                                        intFM -= CalPriceZone.Key;
                                    }

                                    SumData0 += CalculationsManager.CalPrice2(0, ZoneMin, notDay);
                                }

                                int tmpintMin = intMin;
                                intMin -= intFM;
                                if (intMin > 0)
                                {
                                    intFM = 0;
                                }
                                else
                                {
                                    intMin = 0;
                                    intFM -= tmpintMin;
                                }
                                SumData0 += CalculationsManager.CalPrice(0, intMin, notDay);

                                if ((SumData0 > intLM) && (intLM > 0))
                                    SumData0 = intLM;
                            }

                            dr["PriceList"] = SumData0;*/
                        }
                        else
                        {
                            //Mac 2022/07/26----------------
                            if (Configs.UseDayWeek == "I")
                            {
                                stringDW = DateTime.Parse(dt.Rows[i]["datein"].ToString()).DayOfWeek.ToString().ToLower().Substring(0, 2);
                            }
                            else if (Configs.UseDayWeek == "O")
                            {
                                stringDW = DateTime.Parse(dt.Rows[i]["dateout"].ToString()).DayOfWeek.ToString().ToLower().Substring(0, 2);
                            }

                            if (Configs.UseHoliday) //Mac 2020/08/04
                            {
                                string sqlHD = "SELECT * FROM holiday WHERE date(date) = '" + DateTime.Parse(dt.Rows[i]["datein"].ToString()).Year + "-" + DateTime.Parse(dt.Rows[i]["datein"].ToString()).Month + "-" + DateTime.Parse(dt.Rows[i]["datein"].ToString()).Day + "'";
                                DataTable dtHD = DbController.LoadData(sqlHD);
                                if (dtHD.Rows.Count > 0)
                                {
                                    stringDW = "hd";
                                }
                            }
                        }
                        #endregion

                        double totalParkingPrice = 0;   // ค่าจอดทั้งหมด
                        double discount = 0;            // ส่วนลด
                        double parkingPrice = 0;        // ชำระเงินเอง (ค่าจอดรถ)
                        double applicableChargeFee = 0; // ค่าบริการเรียกเก็บ

                        if (Configs.Reports.UseReportThanapoom)
                        {
                            (totalParkingPrice, discount, applicableChargeFee) = GetApplicableChargesThanapoom(stringDW, notDay, i, dt);
                            if (dt.Rows[i]["proid"].ToString() == "119")
                            {
                                parkingPrice = 0;
                            }
                            else
                            {
                                parkingPrice = totalParkingPrice - discount - applicableChargeFee;
                            }

                            /*
                            if (double.TryParse(dt.Rows[i]["price"]?.ToString(), out double parkingPriceValue))
                                parkingPrice = parkingPriceValue;

                            if (parkingPrice > 0)
                            {
                                int lossCardPrice = Convert.ToInt32(dt.Rows[i]?["losscard"] ?? 0);
                                int overDatePrice = Convert.ToInt32(dt.Rows[i]?["overdate"] ?? 0);
                                int totalFine = lossCardPrice + overDatePrice;

                                parkingPrice = Math.Max(totalFine - parkingPrice, 0);
                            }
                            */
                        }
                        else
                        {
                            /* Old (pre 29-07-2025) */
                            if (double.TryParse(dt.Rows[i]["price"]?.ToString(), out double parsedPrice))
                                parkingPrice = parsedPrice;
                            (totalParkingPrice, applicableChargeFee) = GetParkingPriceAndFee(stringDW, notDay, i, dt);
                            
                            //totalParkingFee = Math.Max(0, totalParkingFee - discount); // ผิด
                        }

                        #region Set dr
                        dr["หมายเลขบัตร"] = dt.Rows[i]["no"]?.ToString();
                        dr["เลขทะเบียน"] = dt.Rows[i]["license"]?.ToString();
                        dr["เวลาเข้า"] = dt.Rows[i]["datein"]?.ToString();
                        dr["เวลาออก"] = dt.Rows[i]["dateout"]?.ToString();
                        dr["รหัสใบเสร็จ"] = dt.Rows[i]["printno"]?.ToString();
                        dr["ชม.ที่จอด"] = parkingTime.ToString("F2");
                        dr["ชม.ปัด"] = parkingTimeRoundedUp.ToString("F2");
                        dr["รหัสส่วนลด"] = currentProId.ToString();
                        dr["ชื่อส่วนลด"] = currentProName;

                        dr["ค่าจอดทั้งหมด"] = totalParkingPrice.ToString("F2");
                        dr["ส่วนลด"] = discount.ToString("F2");
                        dr["ชำระเงินเอง"] = parkingPrice.ToString("F2");
                        dr["ค่าบริการเรียกเก็บ"] = applicableChargeFee.ToString("F2");
                        if (Configs.Reports.UseReportThanapoom)  
                            dr["tnpt_id"] = dt?.Rows?[i]?["tnpt_id_int"];
                        #endregion

                        if (paymentText == Constants.TextBased.PaymentStatusPaid)
                        {
                            if (applicableChargeFee > 0)
                                Map.Rows.Add(dr);
                        }
                        else if (paymentText == Constants.TextBased.PaymentStatusUnPaid)
                        {
                            if (applicableChargeFee <= 0)
                                Map.Rows.Add(dr);
                        }
                        else Map.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error occurred:\r\n{TextFormatters.ErrorStacktraceFromException(ex)}"); }

            return Map;
        }

        public static DataTable GetItemizedDailyPromotionUsage(
            DataTable usageData, 
            string promotionName, 
            string paymentStatus, 
            DateTime startDate, 
            DateTime endDate)
        {
            DataTable resultTable = new DataTable("");
            resultTable.Columns.Add("ชื่อตราประทับ", typeof(string));
            resultTable.Columns.Add("วันที่", typeof(string));
            resultTable.Columns.Add("จำนวนเงิน", typeof(string));
            resultTable.Columns.Add("จำนวนบัตร", typeof(string));
            resultTable.Columns.Add("จำนวนบัตร(เก็บเงินได้)", typeof(string));

            try
            {
                IEnumerable<KeyValuePair<int, string>> promotions;

                if (promotionName == Constants.TextBased.All)
                {
                    promotions = AppGlobalVariables.PromotionNamesById;
                }
                else
                {
                    var matched = AppGlobalVariables.PromotionNamesById
                                    .FirstOrDefault(p => p.Value == promotionName);

                    promotions = new[] { new KeyValuePair<int, string>(matched.Key != 0 ? matched.Key : -1, promotionName) };
                }

                foreach (var promo in promotions)
                {
                    for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                    {
                        var (totalFee, totalCards, totalChargedCards) = CalculateDailySummary(usageData, paymentStatus, date, promo.Key);

                        DataRow row = resultTable.NewRow();
                        row["ชื่อตราประทับ"] = promo.Value;
                        row["วันที่"] = date.ToString("dd/MM/yyyy");
                        row["จำนวนเงิน"] = totalFee.ToString("N2");
                        row["จำนวนบัตร"] = totalCards.ToString();
                        row["จำนวนบัตร(เก็บเงินได้)"] = totalChargedCards.ToString();

                        if (row["ชื่อตราประทับ"].ToString() == Constants.TextBased.All || row["ชื่อตราประทับ"].ToString() == Constants.TextBased.Error)
                            continue;

                        if (totalCards < 1)
                            continue;

                        resultTable.Rows.Add(row);
                    }
                }
            }
            catch { }

            return resultTable;
        }


        public static DataTable GetSummarizedDailyPromotionUsage(DataTable usageData)
        {
            DataTable summarizedDataTable = new DataTable("summarizedDataTable");
            summarizedDataTable.Columns.Add("ลำดับ", typeof(string));
            summarizedDataTable.Columns.Add("WPT Code", typeof(string));
            summarizedDataTable.Columns.Add("Customer", typeof(string));
            summarizedDataTable.Columns.Add("ค่าบริการก่อน VAT", typeof(double));
            summarizedDataTable.Columns.Add("VAT", typeof(double));
            summarizedDataTable.Columns.Add("ค่าบริการทั้งหมด", typeof(double));

            int iter = 1;
            decimal sumBeforeVat = 0;
            decimal sumVat = 0;
            decimal sumTotalCharge = 0;

            
            Dictionary<int, double> sums = new Dictionary<int, double>();
            foreach (DataRow row in usageData.Rows)
            {
                int tnptId = Convert.ToInt32(row["tnpt_id"]);
                double fee = Convert.ToDouble(row["ค่าบริการเรียกเก็บ"]);

                if (!sums.ContainsKey(tnptId))
                    sums[tnptId] = 0;

                sums[tnptId] += fee;
            }

            foreach (var kvp in sums)
            {
                Console.WriteLine($"tnpt_id: {kvp.Key}, Sum ค่าบริการเรียกเก็บ: {kvp.Value}");

                double currentPrice =  kvp.Value;
                var (beforeVatCharge, vatCharge, totalCharge) = CalculationsManager.CalculateVatFromFullPrice(currentPrice);
                string wptCode = kvp.Key.ToString();
                if (!AppGlobalVariables.VendorGroupMonthsById.TryGetValue(kvp.Key, out string companyName))
                {
                    companyName = $"(Unknown vendor {kvp.Key})";
                    System.Diagnostics.Debug.WriteLine($"VendorGroupMonthsById missing key {kvp.Key}");
                }

                DataRow row = summarizedDataTable.NewRow();
                row["ลำดับ"] = iter;
                row["WPT Code"] = wptCode;
                row["Customer"] = companyName;
                row["ค่าบริการก่อน VAT"] = beforeVatCharge;
                row["VAT"] = vatCharge;
                row["ค่าบริการทั้งหมด"] = totalCharge;

                iter++;
                sumBeforeVat += beforeVatCharge;
                sumVat += vatCharge;
                sumTotalCharge += totalCharge;

                summarizedDataTable.Rows.Add(row);
            }

            summarizedDataTable.Rows.Add("", "",
                    "รวม",
                sumBeforeVat,
                sumVat,
                sumTotalCharge);

            return summarizedDataTable;
        }

        internal static DataTable GetFeeAndVatSummaryFromMemberGroupPriceMonth(string sql)
        {
            DataTable dataTable = DbController.LoadData(sql);

            DataTable resultTable = new DataTable("");
            resultTable.Columns.Add("ลำดับ", typeof(string));
            resultTable.Columns.Add("WPT Code", typeof(string));
            resultTable.Columns.Add("Customer", typeof(string));
            resultTable.Columns.Add("ค่าบริการก่อน VAT", typeof(double));
            resultTable.Columns.Add("VAT", typeof(double));
            resultTable.Columns.Add("ค่าบริการทั้งหมด", typeof(double));

            int iteration = 0;
            double sumBeforeVat = 0;
            double sumVat = 0;
            double sumTotalCharge = 0;

            foreach (KeyValuePair<int, string> kvp in AppGlobalVariables.VendorGroupMonthsById)
            {
                try
                {
                    int currentVendorGroupId = kvp.Key;
                    string currentVendorGroupName = kvp.Value;
                    double currentPrice = 0;
                    //double currentPrice = double.TryParse(dataTable.Rows[i]["รวมค่าบัตรสมาชิก"]?.ToString(), out var price) ? price : 0;

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        int vendorGroupId = int.TryParse(dataTable.Rows[i]["vendor_id"]?.ToString(), out int vgId) ? vgId : 0;

                        if (vendorGroupId == currentVendorGroupId)
                            currentPrice += double.TryParse(dataTable.Rows[i]["memgrouppriceid_pay"]?.ToString(), out var price) ? price : 0;
                    }

                    if(currentPrice > 0)
                    {
                        var (beforeVatCharge, vatCharge, totalCharge) = CalculationsManager.CalculatePriceSummaryAndVat(currentPrice);

                        DataRow row = resultTable.NewRow();
                        row["ลำดับ"] = ++iteration;
                        row["WPT Code"] = currentVendorGroupId;
                        row["Customer"] = TextFormatters.RemoveBracketFromName(currentVendorGroupName);
                        row["ค่าบริการก่อน VAT"] = beforeVatCharge;
                        row["VAT"] = vatCharge;
                        row["ค่าบริการทั้งหมด"] = totalCharge;

                        resultTable.Rows.Add(row);

                        sumBeforeVat += beforeVatCharge;
                        sumVat += vatCharge;
                        sumTotalCharge += totalCharge;
                    }
                }
                catch (Exception exc) { MessageBox.Show($"{exc.GetType()} | {exc.Message} | {exc.StackTrace}"); }
            }

            resultTable.Rows.Add("", "",
                "รวม",
                sumBeforeVat,
                sumVat,
                sumTotalCharge);

            return resultTable;
        }


        /* public static DataTable GetBillingFeeAndVatSummary(DataTable usageData,string promotionRangeFrom, string promotionRangeTo, string paymentStatus)
        {
            DataTable resultTable = new DataTable("");

            resultTable.Columns.Add("ลำดับ", typeof(string));
            resultTable.Columns.Add("CIT Code", typeof(string));
            resultTable.Columns.Add("WPT Code", typeof(string));
            resultTable.Columns.Add("Customer", typeof(string));
            resultTable.Columns.Add("ค่าบริการก่อน VAT", typeof(string));
            resultTable.Columns.Add("VAT", typeof(string));
            resultTable.Columns.Add("ค่าบริการทั้งหมด", typeof(string));

            double sumBeforeVat = 0;
            double sumVat = 0;
            double sumTotalCharge = 0;
            int intRangeFrom = 0;
            int intRangeTo = 0;

            IEnumerable<KeyValuePair<int, string>> promotions = AppGlobalVariables.PromotionNamesById;

            if (int.TryParse(promotionRangeFrom, out int from))
                intRangeFrom = from;
            else
                intRangeFrom = 0;
            if (int.TryParse(promotionRangeTo, out int to))
                intRangeTo = to;
            else
                intRangeTo = 0;

            bool isPromotionRangeEmpty = (intRangeFrom == 0) || (intRangeTo == 0);
            bool isLegitRange = intRangeFrom < intRangeTo;
            
            string query = "SELECT id, tnpt_id, NAME from promotion ";

            if (!isPromotionRangeEmpty && isLegitRange)
            {
                query += $"WHERE (CASE WHEN tnpt_id IS NULL OR tnpt_id = '' THEN 0 WHEN tnpt_id REGEXP '^-?[0-9]+$' THEN CAST(tnpt_id AS UNSIGNED) ELSE 0 END)\r\n";
                query += $"BETWEEN {promotionRangeFrom} AND {promotionRangeTo}\r\n";
            }
            query += "ORDER BY tnpt_id, id, name";

            DataTable dtPromotionTnptIdName = DbController.LoadData(query);

            for (int i = 0; i < dtPromotionTnptIdName.Rows.Count; i++)
            {
                try
                {
                    int currentProId = Convert.ToInt32(dtPromotionTnptIdName.Rows[i]["id"]);
                    int currentWptCode = Convert.ToInt32(dtPromotionTnptIdName.Rows[i]["tnpt_id"]);
                    string currentProName = dtPromotionTnptIdName.Rows[i]["name"].ToString();

                    //var (beforeVatCharge, vatCharge, totalCharge) = CalculationsManager.CalculatePriceSummaryAndVat(usageData, currentProId);

                    DataRow row = resultTable.NewRow();
                    row["ลำดับ"] = (i + 1).ToString();
                    row["CIT Code"] = currentProId.ToString();
                    row["WPT Code"] = currentWptCode.ToString();
                    row["Customer"] = currentProName;
                    //row["ค่าบริการก่อน VAT"] = beforeVatCharge.ToString("F2");
                    //row["VAT"] = vatCharge.ToString("F2");
                    //row["ค่าบริการทั้งหมด"] = totalCharge.ToString("F2");


                    if (row["Customer"].ToString() == Constants.TextBased.All || row["Customer"].ToString() == Constants.TextBased.Error)
                        continue;

                    if (paymentStatus == Constants.TextBased.PaymentStatusUnPaid)
                    {
                        if (totalCharge <= 0)
                        {
                            resultTable.Rows.Add(row);
                        }
                    }
                    else
                    {
                        if (paymentStatus == Constants.TextBased.PaymentStatusPaid)
                        {
                            if (totalCharge > 0)
                            {
                                resultTable.Rows.Add(row);
                            }
                        }
                        else
                        {
                            resultTable.Rows.Add(row);
                        }

                        sumTotalCharge += totalCharge;
                    }
                }
                catch { }
            }

            sumVat = Math.Round(0.07 * sumTotalCharge, 2);
            sumBeforeVat = Math.Round(sumTotalCharge - sumVat, 2);

            resultTable.Rows.Add("", "", "", 
                "รวม", 
                sumBeforeVat.ToString("F2"), 
                sumVat.ToString("F2"), 
                sumTotalCharge.ToString("F2"));

            return resultTable;
        }
        */

        public static DataTable GetVehicleEarningSummary(string sqlQuery, DateTime startDate, DateTime endDate)
        {
            DataTable dtFromQuery = DbController.LoadData(sqlQuery);

            DataTable map = new DataTable();
            map.Columns.Add(new DataColumn("วันที่", typeof(string)));
            map.Columns.Add(new DataColumn("ชนิดรถ", typeof(string)));
            map.Columns.Add(new DataColumn("จำนวนรถเข้าออก-รถสมาชิก", typeof(string)));
            map.Columns.Add(new DataColumn("จำนวนรถเข้าออก-รถผู้มาติดต่อ", typeof(string)));
            map.Columns.Add(new DataColumn("จำนวนรถเข้าออก-จำนวนรวม", typeof(string)));
            map.Columns.Add(new DataColumn("จำนวนรถที่เก็บเงินได้-รถสมาชิก", typeof(string)));
            map.Columns.Add(new DataColumn("จำนวนรถที่เก็บเงินได้-รถผู้มาติดต่อ", typeof(string)));
            map.Columns.Add(new DataColumn("จำนวนรถที่เก็บเงินได้-จำนวนเงิน", typeof(string)));

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                string vehicleType = AppGlobalVariables.Database.VehicleTypeTh;
                int vehicleInOutMember = 0;
                int vehicleInOutVisitor = 0;
                int vehicleInOutTotal = 0;
                int vehicleWithEarningMember = 0;
                int vehicleWithEarningVisitor = 0;
                int vehicleWithEarningTotal = 0;

                foreach (DataRow usageRow in dtFromQuery.Rows)
                {
                    if (DateTime.TryParse(usageRow["dateout"].ToString(), out DateTime exitTime))
                    {
                        if (exitTime.Date == date.Date)
                        {
                            if (usageRow["cartype"].ToString() == "200")
                            {
                                vehicleInOutMember++;

                                if (Convert.ToInt32(usageRow["price"]) > 0)
                                {
                                    vehicleWithEarningMember++;
                                    vehicleWithEarningTotal += Convert.ToInt32(usageRow["price"]);
                                }
                            }
                            else
                            {
                                vehicleInOutVisitor++;

                                if (Convert.ToInt32(usageRow["price"]) > 0)
                                {
                                    vehicleWithEarningVisitor++;
                                    vehicleWithEarningTotal += Convert.ToInt32(usageRow["price"]);
                                }
                            }
                        }
                    }
                }

                vehicleInOutTotal = vehicleInOutMember + vehicleInOutVisitor;

                DataRow row = map.NewRow();
                row[0] = date.ToString("dd/MM/yyyy");
                row[1] = $"{vehicleType}";
                row[2] = $"{vehicleInOutMember}";
                row[3] = $"{vehicleInOutVisitor}";
                row[4] = $"{vehicleInOutTotal}";
                row[5] = $"{vehicleWithEarningMember}";
                row[6] = $"{vehicleWithEarningVisitor}";
                row[7] = $"{vehicleWithEarningTotal}";

                map.Rows.Add(row);
            }

            return map;
        }

        public static DataTable GetCardSortByCompanySummary(string sqlQuery)
        {
            DataTable dtFromQuery = DbController.LoadData(sqlQuery);

            DataTable map = new DataTable();
            map.Columns.Add("ลำดับ", typeof(int));
            map.Columns.Add("ตราประทับ", typeof(string));
            map.Columns.Add("หมายเหตุ", typeof(string));
            map.Columns.Add("บริษัท", typeof(string));
            map.Columns.Add("สิทธิได้รับบัตรฟรี", typeof(int));
            map.Columns.Add("จำนวนบัตร(ฟรีค่าเช่า)", typeof(int));
            map.Columns.Add("จำนวนบัตร(เสียค่าเช่า)", typeof(int));
            map.Columns.Add("รวมค่าเช่า", typeof(string));
            map.Columns.Add("จำนวนบัตรรวม", typeof(int));

            if (AppGlobalVariables.MemberGroupMonthsToId == null || AppGlobalVariables.MemberGroupMonthsToId.Count == 0)
                throw new InvalidOperationException("MemberGroupMonthsToId dictionary is not initialized.");

            int iteration = 0;
            var memberGroups = new Dictionary<string, int>(AppGlobalVariables.MemberGroupMonthsToId);

            foreach (var kvp in memberGroups)
            {
                string currentMemberGroupName = kvp.Key;
                int currentMemberGroupId = kvp.Value;

                int freeCardAmount = 0;
                int paidCardAmount = 0;
                int totalPaidAmount = 0;

                foreach (DataRow dataRow in dtFromQuery.Rows)
                {
                    int memberGroupIdFromDataRow = Convert.ToInt32(dataRow["membergroupprice_month_id"]);

                    if (memberGroupIdFromDataRow == currentMemberGroupId)
                    {
                        int paidAmount = Convert.ToInt32(dataRow["ค่าบัตรสมาชิก"]);

                        if (paidAmount == 0)
                            freeCardAmount++;
                        else
                        {
                            paidCardAmount++;
                            totalPaidAmount += paidAmount;
                        }
                    }
                }

                if (freeCardAmount == 0 && paidCardAmount == 0)
                    continue;

                DataRow row = map.NewRow();
                row["ลำดับ"] = ++iteration;
                row["ตราประทับ"] = currentMemberGroupId;
                row["บริษัท"] = TextFormatters.RemoveBracketFromName(currentMemberGroupName);
                row["หมายเหตุ"] = "";
                row["สิทธิได้รับบัตรฟรี"] = freeCardAmount;
                row["จำนวนบัตร(ฟรีค่าเช่า)"] = freeCardAmount;
                row["จำนวนบัตร(เสียค่าเช่า)"] = paidCardAmount;
                row["รวมค่าเช่า"] = totalPaidAmount.ToString("#,###,##0");
                row["จำนวนบัตรรวม"] = freeCardAmount + paidCardAmount;

                map.Rows.Add(row);
            }

            return map;
        }

        #endregion


        #region UPDATE

        #endregion


        #region DELETE

        #endregion


        #region COMPLEX (multi-functions)
        public static void LoadDataServer()
        {
            string sql = "SELECT * FROM cardmf";
            DataTable dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM cardmf";
                if (DbController.SaveData(sql, true) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO cardmf VALUES (" + dt.Rows[i].ItemArray[0].ToString() + "," + dt.Rows[i].ItemArray[1].ToString() + "," + dt.Rows[i].ItemArray[2].ToString() + ")";
                        DbController.SaveData(sql, true);
                    }
                }
            }
            sql = "SELECT * FROM cardpx";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM cardpx";
                if (DbController.SaveData(sql, true) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO cardpx VALUES (" + dt.Rows[i].ItemArray[0].ToString() + "," + dt.Rows[i].ItemArray[1].ToString() + "," + dt.Rows[i].ItemArray[2].ToString() + ")";
                        DbController.SaveData(sql, true);
                    }
                }
            }
            sql = "SELECT * FROM cartype";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM cartype";
                if (DbController.SaveData(sql, true) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO cartype VALUES (" + dt.Rows[i].ItemArray[0].ToString() + ",'" + dt.Rows[i].ItemArray[1].ToString() + "'," + dt.Rows[i].ItemArray[2].ToString() + "," + dt.Rows[i].ItemArray[3].ToString() + "," + dt.Rows[i].ItemArray[4].ToString() + ")";
                        DbController.SaveData(sql, true);
                    }
                }
            }
            sql = "SELECT * FROM member";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM member";
                if (DbController.SaveData(sql, true) == "")
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
                        DbController.SaveData(sql, true);
                    }
                }
            }

            sql = "SELECT * FROM param";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM param";
                if (DbController.SaveData(sql, true) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO param VALUES ('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "')";
                        DbController.SaveData(sql, true);
                    }
                }
            }

            sql = "SELECT * FROM promotion";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM promotion";
                if (DbController.SaveData(sql, true) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO promotion VALUES (" + dt.Rows[i].ItemArray[0].ToString() + ",'" + dt.Rows[i].ItemArray[1].ToString() + "'," + dt.Rows[i].ItemArray[2].ToString() + "," + dt.Rows[i].ItemArray[3].ToString() + ")";
                        DbController.SaveData(sql, true);
                    }
                }
            }

            sql = "SELECT * FROM user";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "DELETE FROM user";
                if (DbController.SaveData(sql, true) == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "INSERT INTO user VALUES (" + dt.Rows[i].ItemArray[0].ToString() + "," + dt.Rows[i].ItemArray[1].ToString() + "," + dt.Rows[i].ItemArray[2].ToString() + ",'" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','";
                        sql += dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "','" + dt.Rows[i].ItemArray[7].ToString() + "')";
                        DbController.SaveData(sql, true);
                    }
                }
            }
            sql = "SHOW TABLES";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string str = dt.Rows[i].ItemArray[0].ToString();
                    if (str.IndexOf("pricerate") >= 0)
                    {
                        sql = "SELECT * FROM " + str;
                        DataTable dt2 = DbController.LoadData(sql);
                        sql = "DELETE FROM " + str;
                        if (DbController.SaveData(sql, true) == "")
                        {
                            for (int i2 = 0; i2 < dt2.Rows.Count; i2++)
                            {
                                sql = "INSERT INTO " + str + " VALUES (" + dt2.Rows[i2].ItemArray[0].ToString() + "," + dt2.Rows[i2].ItemArray[1].ToString() + "," + dt2.Rows[i2].ItemArray[2].ToString() + "," + dt2.Rows[i2].ItemArray[3].ToString() + "," + dt2.Rows[i2].ItemArray[4].ToString() + ",";
                                sql += dt2.Rows[i2].ItemArray[5].ToString() + "," + dt2.Rows[i2].ItemArray[6].ToString() + ")";
                                DbController.SaveData(sql, true);
                            }
                        }
                    }
                }
            }
        }

        internal static DataTable GetMemberMonthSummary(DataTable dataTable)
        {
            DataTable Map = new DataTable();
            Map.Columns.Add(new DataColumn("บริษัท", typeof(string)));
            Map.Columns.Add(new DataColumn("เลขที่บัตร", typeof(string)));
            Map.Columns.Add(new DataColumn("ชื่อสมาชิก", typeof(string)));
            Map.Columns.Add(new DataColumn("ชนิดสมาชิก", typeof(string)));
            Map.Columns.Add(new DataColumn("วันที่สมัคร", typeof(string)));
            Map.Columns.Add(new DataColumn("วันที่หมดอายุ", typeof(string)));
            Map.Columns.Add(new DataColumn("เลขทะเบียนรถ1", typeof(string)));
            Map.Columns.Add(new DataColumn("เลขทะเบียนรถ2", typeof(string)));
            Map.Columns.Add(new DataColumn("เลขทะเบียนรถ3", typeof(string)));
            Map.Columns.Add(new DataColumn("เลขทะเบียนรถ4", typeof(string)));
            Map.Columns.Add(new DataColumn("ผู้เช่า", typeof(string)));
            Map.Columns.Add(new DataColumn("ค่าบัตรสมาชิก", typeof(string)));

            if (dataTable == null || dataTable.Rows.Count <= 0) return null;

            try
            {
                foreach (DataRow sourceRow in dataTable.Rows)
                {
                    DataRow newRow = Map.NewRow();

                    foreach (DataColumn col in Map.Columns)
                    {
                        if (col.ColumnName.StartsWith("เลขทะเบียนรถ"))
                            continue; // Jump to license plates handler below

                        if (dataTable.Columns.Contains(col.ColumnName))
                        {
                            newRow[col.ColumnName] = sourceRow[col.ColumnName];
                        }
                    }

                    // Handle license plates
                    if (dataTable.Columns.Contains("เลขทะเบียนรถ"))
                    {
                        string licenseData = sourceRow["เลขทะเบียนรถ"].ToString();
                        string[] licenses = licenseData.Split(',');

                        for (int i = 0; i < licenses.Length && i < 4; i++)
                        {
                            newRow[$"เลขทะเบียนรถ{i + 1}"] = licenses[i].Trim();
                        }
                    }

                    newRow["ชนิดสมาชิก"] = AppGlobalVariables.Database.VehicleTypeEn;

                    Map.Rows.Add(newRow);
                }
            }
            catch { }

            return Map;
        }

        #endregion


        #region HELPERS
        private static (double parkingPrice, double feeCharge) GetParkingPriceAndFee(string dw, bool notDay, int iteration, DataTable dataTable)
        {
            string sqlQuery = "select * from prosetprice where PromotionID = " + dataTable.Rows[iteration]["proid"] + " ";

            if (dw.Length > 1)
                sqlQuery += " and dayweek like '%" + dw + "%'";

            sqlQuery += " order by no";

            DataTable dt2 = DbController.LoadData(sqlQuery);
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                AppGlobalVariables.IntTime = new int[dt2.Rows.Count];
                AppGlobalVariables.IntPriceMin = new int[dt2.Rows.Count];
                AppGlobalVariables.IntPriceHour = new int[dt2.Rows.Count];
                AppGlobalVariables.IntHourRound = new int[dt2.Rows.Count];
                AppGlobalVariables.IntExpense = new int[dt2.Rows.Count];
                AppGlobalVariables.IntOver = new int[dt2.Rows.Count];
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    if (j == 0) //Mac 2016/04/05
                    {
                        AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[3].ToString());
                    }
                    else
                    {
                        AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[3].ToString()) - Convert.ToInt32(dt2.Rows[j - 1].ItemArray[3].ToString());
                    }
                    AppGlobalVariables.IntPriceMin[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[4].ToString());  // นาทีละ(บาท)
                    AppGlobalVariables.IntPriceHour[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[5].ToString()); // ชั่วโมงละ(บาท)
                    AppGlobalVariables.IntHourRound[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[6].ToString()); // ปัดเศษ(นาที)
                    AppGlobalVariables.IntExpense[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[7].ToString());   // เหมาจ่าย(บาท)
                    AppGlobalVariables.IntOver[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[8].ToString());      // จอดเกินกำหนด(บาท)
                }
                //--------------------------------- //Mac 2017/12/06

                //Mac 2022/07/29
                int FlatRateM = 0;
                int FlatRateP = 0;
                int FlatRateX = 0;

                if (Configs.UseFlatRateProSetPrice)
                {
                    try
                    {
                        FlatRateM = Convert.ToInt32(dt2.Rows[0]["flat_rate"].ToString().Split('|')[0]);
                        FlatRateP = Convert.ToInt32(dt2.Rows[0]["flat_rate"].ToString().Split('|')[1]);
                        FlatRateX = Convert.ToInt32(dt2.Rows[0]["flat_rate"].ToString().Split('|')[2]);
                    }
                    catch { }
                }

                int ZoneMin = 0;
                int intParkingPrice = 0;
                int intMin = 0;

                //Mac 2022/07/26 ---------------
                sqlQuery = "select * from prosetprice_zone where PromotionID = " + dataTable.Rows[iteration]["proid"] + " ";

                if (dw.Length > 1)
                    sqlQuery += " and dayweek like '%" + dw + "%'";

                sqlQuery += " order by no";

                DataTable dt4 = DbController.LoadData(sqlQuery);
                //------------------------------

                if (dt4 != null && dt4.Rows.Count > 0)
                {
                    AppGlobalVariables.IntTime2 = new int[dt4.Rows.Count];
                    AppGlobalVariables.IntPriceMin2 = new int[dt4.Rows.Count];
                    AppGlobalVariables.IntPriceHour2 = new int[dt4.Rows.Count];
                    AppGlobalVariables.IntHourRound2 = new int[dt4.Rows.Count];
                    AppGlobalVariables.IntExpense2 = new int[dt4.Rows.Count];
                    AppGlobalVariables.IntOver2 = new int[dt4.Rows.Count];
                    for (int y = 0; y < dt4.Rows.Count; y++)
                    {
                        if (y == 0)
                        {
                            AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[3].ToString());
                        }
                        else
                        {
                            AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[3].ToString()) - Convert.ToInt32(dt4.Rows[y - 1].ItemArray[3].ToString());
                        }
                        AppGlobalVariables.IntPriceMin2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[4].ToString());
                        AppGlobalVariables.IntPriceHour2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[5].ToString());
                        AppGlobalVariables.IntHourRound2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[6].ToString());
                        AppGlobalVariables.IntExpense2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[7].ToString());
                        AppGlobalVariables.IntOver2[y] = Convert.ToInt32(dt4.Rows[y].ItemArray[8].ToString());
                    }
                    string ZoneStart = dt4.Rows[0]["zone_start"].ToString();
                    string ZoneStop = dt4.Rows[0]["zone_stop"].ToString();

                    var CalPriceZone = (dynamic)null;
                    DateTime dti = DateTime.Parse(dataTable.Rows[iteration]["datein"].ToString());
                    DateTime dto = DateTime.Parse(dataTable.Rows[iteration]["dateout"].ToString());
                    DateTime dtInOne;
                    DateTime dtOutOne;
                    TimeSpan diffInOut = DateTime.Parse(dto.ToShortDateString()) - DateTime.Parse(dti.ToShortDateString());

                    bool boolNoRound = false; //Mac 2018/01/08
                    boolNoRound = false; //Mac 2018/01/08
                    for (int x = 0; x < diffInOut.Days + 1; x++)
                    {
                        if (diffInOut.Days == 0)
                        {
                            boolNoRound = true; //Mac 2018/01/08
                            dtInOne = dti;
                            dtOutOne = dto;
                        }
                        else if (x == 0)
                        {
                            dtInOne = dti;
                            dtOutOne = DateTime.Parse(dti.ToShortDateString() + " 23:59:59");
                        }
                        else if (x == diffInOut.Days)
                        {
                            dtInOne = DateTime.Parse(dto.ToShortDateString() + " 00:00:00");
                            dtOutOne = dto;
                        }
                        else
                        {
                            dtInOne = DateTime.Parse(dti.ToShortDateString() + " 00:00:00");
                            dtOutOne = DateTime.Parse(dti.AddDays(1).ToShortDateString() + " 00:00:00");
                        }

                        CalPriceZone = CalculationsManager.CalPriceZoneOneDay(0, dtInOne.ToString(), dtOutOne.ToString(), ZoneStart, ZoneStop, 0, 0, 0, boolNoRound);
                        ZoneMin += CalPriceZone.Key;
                    }
                }

                int intFee;

                if (ZoneMin > 0)
                {
                    intMin = Int32.Parse(dataTable.Rows[iteration]["tdf"].ToString()) - ZoneMin;
                    intParkingPrice = CalculationsManager.CalPrice2(0, ZoneMin, notDay);
                    intFee = CalculationsManager.CalPrice(0, intMin, notDay) + intParkingPrice;
                }
                else
                {
                    intFee = CalculationsManager.CalPrice(0, Int32.Parse(dataTable.Rows[iteration]["tdf"].ToString()), notDay);
                }

                if (Configs.UseFlatRateProSetPrice)
                {
                    intFee += CalculationsManager.CalFlatRate(DateTime.Parse(dataTable.Rows[iteration]["datein"].ToString()), DateTime.Parse(dataTable.Rows[iteration]["dateout"].ToString()), FlatRateM, FlatRateP, FlatRateX);
                }

                double parkingPrice = Convert.ToDouble(intParkingPrice); // dr["ค่าจอดทั้งหมด"]
                double feeCharge = Convert.ToDouble(intFee); // dr["ค่าบริการเรียกเก็บ"]

                return (parkingPrice, feeCharge);
            }
            return (0, 0);
        }

        private static (double parkingPrice, double discount, double feeCharge) GetApplicableChargesThanapoom(string dw, bool notDay, int iteration, DataTable dataTable)
        {
            int promotionId = Convert.ToInt16(dataTable.Rows[iteration]["proid"]);

            string sqlQuery = $"select * from prosetprice where PromotionID = {promotionId} ";

            if (dw.Length > 1)
                sqlQuery += "and dayweek like '%" + dw + "%' ";

            sqlQuery += "order by no";

            int intMinute = Int32.Parse(dataTable.Rows[iteration]["tdf"].ToString());
            int totalDiscountTimeMinute = 0;
            int intFee = 0;
            int intParkingPrice = 0;
            int intDiscount = 0;

            DataTable dt2 = DbController.LoadData(sqlQuery);
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                AppGlobalVariables.IntTime = new int[dt2.Rows.Count];
                AppGlobalVariables.IntPriceMin = new int[dt2.Rows.Count];
                AppGlobalVariables.IntPriceHour = new int[dt2.Rows.Count];
                AppGlobalVariables.IntHourRound = new int[dt2.Rows.Count];
                AppGlobalVariables.IntExpense = new int[dt2.Rows.Count];
                AppGlobalVariables.IntOver = new int[dt2.Rows.Count];
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    if (j == 0)
                    {
                        AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[3].ToString());
                    }
                    else
                    {
                        AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[3].ToString()) - Convert.ToInt32(dt2.Rows[j - 1].ItemArray[3].ToString());
                    }
                    AppGlobalVariables.IntPriceMin[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[4].ToString());  // นาทีละ(บาท)
                    AppGlobalVariables.IntPriceHour[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[5].ToString()); // ชั่วโมงละ(บาท)
                    AppGlobalVariables.IntHourRound[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[6].ToString()); // ปัดเศษ(นาที)
                    AppGlobalVariables.IntExpense[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[7].ToString());   // เหมาจ่าย(บาท)
                    AppGlobalVariables.IntOver[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[8].ToString());      // จอดเกินกำหนด(บาท)

                    /* DISCOUNT */
                    int startTime = Convert.ToInt32(dt2?.Rows[j]?.ItemArray[2]);
                    int finishTime = Convert.ToInt32(dt2?.Rows[j]?.ItemArray[3]);
                    int totalTimeMinute = finishTime - startTime + 1;
                    totalDiscountTimeMinute += totalTimeMinute * (AppGlobalVariables.IntPriceHour[j] == 0 ? 1 : 0);
                }

                intFee = CalculationsManager.CalPrice(0, intMinute, notDay);
                intParkingPrice = CalculationsManager.GetPriceThanapoom(intMinute);
                if (dataTable.Rows[iteration]["proid"].ToString() == "119")
                {
                    intDiscount = Math.Max(0, intParkingPrice - intFee);
                }
                else if (intParkingPrice <= 0)
                {
                    intDiscount = 0;
                }
                else
                {
                    intDiscount = CalculationsManager.GetDiscountThanapoom();
                }

                double parkingPrice = Convert.ToDouble(intParkingPrice); // dr["ค่าจอดทั้งหมด"]
                double discount = Convert.ToDouble(intDiscount); // dr["ส่วนลด"]
                double feeCharge = Convert.ToDouble(intFee); // dr["ค่าบริการเรียกเก็บ"]

                return (parkingPrice, discount, feeCharge);
            }
            else
            {
                double freePromotionPrice = CalculationsManager.GetPriceThanapoom(intMinute);
                double freeDiscountPrice = Math.Min(CalculationsManager.GetDiscountThanapoom(promotionId), freePromotionPrice);

                return (freePromotionPrice, freeDiscountPrice, 0);
            }
        }

        private static (decimal totalFee, int totalCards, int totalChargedCards) CalculateDailySummary(DataTable data, string paymentStatus, DateTime date, int? filterPromotionId = null)
        {
            decimal totalFee = 0;
            int totalCards = 0;
            int totalChargedCards = 0;

            foreach (DataRow row in data.Rows)
            {
                if (!DateTime.TryParse(row["เวลาออก"]?.ToString(), out DateTime exitTime) || exitTime.Date != date)
                    continue;

                if (filterPromotionId.HasValue &&
                    int.TryParse(row["รหัสส่วนลด"]?.ToString(), out int rowPromoId) &&
                    rowPromoId != filterPromotionId.Value)
                    continue;

                if (paymentStatus != Constants.TextBased.All)
                {
                    if (paymentStatus == Constants.TextBased.PaymentStatusPaid)
                    {
                        if (decimal.TryParse(row["ค่าบริการเรียกเก็บ"]?.ToString(), out decimal fee))
                        {
                            totalFee += fee;
                            if (fee > 0)
                            {
                                totalCards++;
                                totalChargedCards++;
                            }
                        }
                    }
                    else if (paymentStatus == Constants.TextBased.PaymentStatusUnPaid)
                    {
                        if (decimal.TryParse(row["ค่าบริการเรียกเก็บ"]?.ToString(), out decimal fee))
                        {
                            if (fee <= 0)
                            {
                                totalCards++;
                            }
                        }
                    }
                }
                else
                {
                    totalCards++;

                    if (decimal.TryParse(row["ค่าบริการเรียกเก็บ"]?.ToString(), out decimal fee))
                    {
                        totalFee += fee;
                        if (fee > 0) totalChargedCards++;
                    }
                }
            }

            return (totalFee, totalCards, totalChargedCards);
        }
        #endregion HELPERS_END
    }
}

/* Unused ???
     public bool GetDispensers(int intNo)
     {
         bool result = false;
         string sql = "SELECT * FROM dispenser";
         sql += " WHERE no=" + intNo.ToString();
         DataTable dt = DbController.LoadData(sql);
         Configs.Hardwares.DispenserIP = "NO";
         if (dt.Rows.Count > 0)
         {
             Configs.Hardwares.DispenserName = dt.Rows[0].ItemArray[1].ToString();
             Configs.Hardwares.DispenserIP = dt.Rows[0].ItemArray[2].ToString();
             Configs.Hardwares.DispenserNoCard = Convert.ToInt32(dt.Rows[0].ItemArray[3]);
             result = true;
         }
         return result;
     }

     public bool GetDispensersByIP(string strIP)
     {
         string sql = $"SELECT * FROM dispenser WHERE ip='{strIP}'";

         DataTable dt = DbController.LoadData(sql);
         if (dt.Rows.Count > 0)
         {
             Configs.Hardwares.DispenserNo = dt.Rows[0].ItemArray[0].ToString();
             Configs.Hardwares.DispenserName = dt.Rows[0].ItemArray[1].ToString();
             Configs.Hardwares.DispenserNoCard = Convert.ToInt32(dt.Rows[0].ItemArray[3]);
             return true;
         }
         else
             return false;
     }

     public bool UpdateDispenserIP(string strIP)
     {
         bool result = false;
         string sql = "UPDATE dispenser SET nocard=" + Configs.Hardwares.DispenserNoCard;
         sql += " WHERE ip='" + strIP + "'";
         if (DbController.SaveData(sql) == "")
             result = true;
         return result;
     }

     public bool UpdateDispenser(string strNo)
     {
         bool result = false;
         string sql = "UPDATE dispenser SET nocard=" + Configs.Hardwares.DispenserNoCard;
         sql += " WHERE no=" + strNo;
         if (DbController.SaveData(sql) == "")
             result = true;
         return result;
     }

        public bool UpdateNoRecord(string strTable, uint intID)
        {
            bool result = false;

            if (booOffline)
                return true;

            else
            {
                string sql = "UPDATE card" + strTable + " SET no =" + RecordNo.ToString();
                sql += " WHERE name=" + intID.ToString();
                if (Controller.SaveData(sql) == "")
                    result = true;
            }
            return result;
        }

        public bool UpdateProCarIn(int intNo, int intID)
        {
            bool result = false;
            string sql = "UPDATE recordin SET proid =" + intID.ToString();
            sql += " WHERE no=" + intNo.ToString();
            if (Controller.SaveData(sql) == "")
                result = true;
            return result;
        }

        public bool UpdateLicenseCarIn(int intNo, string strLicense)
        {
            bool result = false;
            string sql = "UPDATE recordin SET license='" + strLicense;
            sql += "' WHERE no=" + intNo.ToString();
            if (Controller.SaveData(sql) == "")
                result = true;
            return result;
        }

        public bool InsertProRecord(uint intID, string strLicense, string strProName, string strName)
        {
            bool result = false;
            string sql = "INSERT INTO recordproname (id,license,proname,daterec,username) VALUES ( " + intID + ",'" + strLicense + "','" + strProName + "',Now(),'" + strName + "')";
            if (booOffline)
            {
                result = true;
            }
            else
            {
                if (Controller.SaveData(sql) == "")
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
            string AppGlobalVariables.Printings.Headerer = "sql,no,id,cartype,license,rankey,picdiv,piclic,datein,userin";
            string sql = "INSERT INTO recordin VALUES (";//
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            sql += RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + ",'" + strLicense + "','" + strRandKey + "','" + strImageD + "','" + strImageL + "',NOW()," + user.ID + "," + proID.ToString() + ");";//
            //string strRecord = RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + "," + strLicense + "," + strRandKey + "," + strImageD + "," + strImageL + "," + now.ToString() + "," + user.ID + ")";//
            if (!File.Exists(strFile))
            {
                StreamWriter sw = File.CreateText(strFile);
                sw.WriteLine(AppGlobalVariables.Printings.Headerer);
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

        public void GetRecordNo()
        {
            if (UseOfflineMode)
                RecordNo = dbLocal.LoadNO();
            else
            {
                string sql = "SELECT MAX(no) FROM recordin";
                DataTable dt = DbController.LoadData(sql);
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
                if (Controller.SaveData(sql) == "")
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
            string AppGlobalVariables.Printings.Headerer = "sql,no,picdiv,piclic,dateout,proid,price,discount,userout,userno,losscard,overdate";
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            string sql = "INSERT INTO recordout VALUES (";//
            //sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID + "," + PriceCardLoss + "," + PriceOverDate + ");";
            sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "','" + strDtocar + "'," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID + "," + PriceCardLoss + "," + PriceOverDate + ");"; //Mac 2014/08/08
            if (!File.Exists(strFile))
            {
                StreamWriter sw = File.CreateText(strFile);
                sw.WriteLine(AppGlobalVariables.Printings.Headerer);
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

        public bool CarOutRecord(int recordno, string srtPrice, string strDtocar, string srtDiscount, string srtProID, string strImageD, string strImageL, int PriceCardLoss, int PriceOverDate) //Mac 2014/08/08
        {
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
                if (Controller.SaveData(sql) == "")
                {
                    result = true;
                }
            }
            return result;
        }
       
        public int CheckCarIn(bool offline)
        {
            int intCarIn = 0;
            string sql = "";
            DataTable dt;
            if (offline)
            {
                sql = "SELECT Count(no) FROM recordin";
                dt = DbController.LoadData(sql);
                int intIn = 0;
                int intOut = 0;
                if (dt.Rows.Count > 0)
                {
                    intIn = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                }
                sql = "SELECT Count(no) FROM recordout";
                dt = DbController.LoadData(sql);
                if (dt.Rows.Count > 0)
                {
                    intOut = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                }
                intCarIn = intIn - intOut;
            }
            else
            {
                sql = "SELECT Count(name) FROM cardpx WHERE no > 0";
                dt = DbController.LoadData(sql);
                if (dt.Rows.Count > 0)
                {
                    intCarIn = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                }
                if (UseMifare || UseMifareIn || UseMifareOut)
                {
                    sql = "SELECT Count(name) FROM cardmf WHERE no > 0";
                    dt = DbController.LoadData(sql);
                    if (dt.Rows.Count > 0)
                    {
                        intCarIn += Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                    }
                }
            }

            return intCarIn;
        }

        public bool CheckCardRegist(uint intID, string strTable)
        {
            bool booCardID = false;
            string sql = "SELECT * FROM card" + strTable;
            sql += " WHERE name=" + intID.ToString() + "";
            DataTable dt;
            if (booOffline)
                dt = Database.Controller.LoadData(sql, true);
            else
                dt = DbController.LoadData(sql);
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
                dt = Database.Controller.LoadData(sql, true);
            else
                dt = DbController.LoadData(sql);
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
                dt = Database.Controller.LoadData(sql, true);
            else
                dt = DbController.LoadData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                int intNoIn = 0;
                intNoIn = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                if (intNoIn > 0)
                {
                    sql = "SELECT no FROM recordout";
                    sql += " WHERE no=" + intNoIn.ToString() + "";
                    if (booOffline)
                        dt = Database.Controller.LoadData(sql, true);
                    else
                        dt = DbController.LoadData(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        booCardID = true;
                    }
                }          
            }

            return booCardID;
        }


        public bool DBConnect(string IP, string DBNAME = "carpark2") //Mac 2016/11/10
        {
            bool result = false;
            return result = Controller.Connect(IP, DBNAME); //Mac 2016/11/10
        }

        public bool DBLocalConnect()
        {
            return dbLocal.Connect();
        }

       
        public string SaveDataLocal(string sql)
        {
            return DbController.SaveData(sql, true);
        }

        public string SaveData(string sql)
        {
            return Controller.SaveData(sql);
        }

        public DataTable LoadData(string sql)
        {
            return DbController.LoadData(sql);
        }

        public DataTable LoadDataLocal(string sql)
        {
            return Database.Controller.LoadData(sql, true);
        }

     */

/* public static void Total(int selectedReportId, DataGridView ResultGridView)
 {
     if (ResultGridView.Rows.Count <= 0)
         return;

     int intNo = ResultGridView.Rows.Count - 1;
     int sPrice = 0, sDiscount = 0, i = 0;
     int iVil = 0;
     int colCount = ResultGridView.Rows[0].Cells.Count; //Mac 2021/07/23
     int sExcess = 0; //Mac 2021/10/20

     if (Configs.IsVillage && (selectedReportId == 0 || selectedReportId == 1 || selectedReportId == 8 || selectedReportId == 90 || selectedReportId == 91) && Configs.Use2Camera)
         iVil = 5;

     if ((selectedReportId == 0 || selectedReportId == 1 || selectedReportId == 8 || selectedReportId == 90 || selectedReportId == 91) && Configs.NoPanelUp2U == "2") //Mac 2020/10/26
         iVil += 4;

     if (selectedReportId == 0 || selectedReportId == 1 || selectedReportId == 8 || selectedReportId == 90 || selectedReportId == 91) //Mac 2020/10/26
     {
         if ((selectedReportId == 1 || selectedReportId == 91) && (Configs.Reports.UseReport1_6)) //Mac 2020/10/26
             iVil = 1;
         else if (Configs.Reports.UseReport1_4) //Mac 2018/12/17
             iVil = 3;
         else if (Configs.Reports.UseReport1_6 || Configs.Reports.UseReport1_8) //Mac 2021/12/17
             iVil = 1;

         for (i = 0; i < intNo; i++)
         {
             string x = ResultGridView.Rows[i].Cells[6 + iVil].Value.ToString();
             if (int.TryParse(x, out int value))
                 sPrice += value;

             x = ResultGridView.Rows[i].Cells[7 + iVil].Value.ToString();
             value = 0;
             if (int.TryParse(x, out value))
                 sDiscount += value;
         }
         ResultGridView.Rows[intNo].Cells[3 + iVil].Value = "จำนวนรถ";
         ResultGridView.Rows[intNo].Cells[4 + iVil].Value = intNo.ToString("#,###,##0") + " คัน";
         ResultGridView.Rows[intNo].Cells[5 + iVil].Value = "รวม";
         ResultGridView.Rows[intNo].Cells[6 + iVil].Value = "" + sPrice.ToString("#,###,##0");
         ResultGridView.Rows[intNo].Cells[7 + iVil].Value = "" + sDiscount.ToString("#,###,##0");
     }

     else if (selectedReportId == 2)
     {
         for (i = 0; i < intNo; i++)
         {
             sPrice += Int32.Parse(ResultGridView.Rows[i].Cells[5].Value.ToString());
             sDiscount += Int32.Parse(ResultGridView.Rows[i].Cells[6].Value.ToString());
         }
         ResultGridView.Rows[intNo].Cells[4].Value = "รวม";
         ResultGridView.Rows[intNo].Cells[5].Value = "" + sPrice.ToString("#,###,##0");
         ResultGridView.Rows[intNo].Cells[6].Value = "" + sDiscount.ToString("#,###,##0");
     }

     else if (selectedReportId == 29)
     {
         int col1 = 0, col2 = 0, col3 = 0;
         for (i = 0; i < intNo; i++)
         {
             col1 += Int32.Parse(ResultGridView.Rows[i].Cells[1].Value.ToString());
             col2 += Int32.Parse(ResultGridView.Rows[i].Cells[2].Value.ToString());
             col3 += Int32.Parse(ResultGridView.Rows[i].Cells[3].Value.ToString());
             sPrice += Int32.Parse(ResultGridView.Rows[i].Cells[4].Value.ToString());
         }
         ResultGridView.Rows[intNo].Cells[0].Value = "รวม";
         ResultGridView.Rows[intNo].Cells[1].Value = "" + col1.ToString("#,###,##0");
         ResultGridView.Rows[intNo].Cells[2].Value = "" + col2.ToString("#,###,##0");
         ResultGridView.Rows[intNo].Cells[3].Value = "" + col3.ToString("#,###,##0");
         ResultGridView.Rows[intNo].Cells[4].Value = "" + sPrice.ToString("#,###,##0");
     }

     else if (selectedReportId == 3 || selectedReportId == 10) //Mac 2021/05/21
     {
         ResultGridView.Rows[intNo].Cells[3].Value = "รวม";
         ResultGridView.Rows[intNo].Cells[4].Value = intNo.ToString("#,###,##0") + " ครั้ง";
     }

     else if (selectedReportId == 4 || selectedReportId == 30 || selectedReportId == 31 || selectedReportId == 92 || selectedReportId == 93) //Mac 2020/10/26
     {
         ResultGridView.Rows[intNo].Cells[3].Value = "จำนวนรถ";
         ResultGridView.Rows[intNo].Cells[4].Value = intNo.ToString("#,###,##0") + " คัน";
     }

     else if (selectedReportId == 5)
     {
         iVil = 0;
         if (Configs.Reports.UseReport1_6 || Configs.Reports.UseReport1_8)
             iVil = 1;

         for (i = 0; i < intNo; i++)
         {
             sPrice += Int32.Parse(ResultGridView.Rows[i].Cells[6 + iVil].Value.ToString());
             sDiscount += Int32.Parse(ResultGridView.Rows[i].Cells[7 + iVil].Value.ToString());
         }

         ResultGridView.Rows[intNo].Cells[3 + iVil].Value = "จำนวนรถ";
         ResultGridView.Rows[intNo].Cells[4 + iVil].Value = intNo.ToString("#,###,##0") + " คัน";
         ResultGridView.Rows[intNo].Cells[5 + iVil].Value = "รวม";
         ResultGridView.Rows[intNo].Cells[6 + iVil].Value = sPrice.ToString("#,###,##0");
         ResultGridView.Rows[intNo].Cells[7 + iVil].Value = sDiscount.ToString("#,###,##0");
     }

     else if (selectedReportId == 7)
     {
         ResultGridView.Rows[intNo].Cells[2].Value = "รวม";
         ResultGridView.Rows[intNo].Cells[3].Value = intNo.ToString("#,###,##0") + " ครั้ง";
     }

     else if (selectedReportId == 9)
     {
         ResultGridView[3, intNo].Value = "จำนวนรถ";
         ResultGridView[4, intNo].Value = intNo.ToString() + " คัน";
         ResultGridView[6, intNo].Value = "ยอดรวม";
         ResultGridView[7, intNo].Value = tr.ToString("#0.00");
         ResultGridView[8, intNo].Value = td.ToString("#0.00");
         ResultGridView[9, intNo].Value = tt.ToString("#0.00");
         ResultGridView[10, intNo].Value = tb.ToString("#0.00");
         ResultGridView[11, intNo].Value = tv.ToString("#0.00");
     }

     else if (selectedReportId == 12 || selectedReportId == 13 || selectedReportId == 14)
     {
         if (selectedReportId == 13 && Configs.Reports.UseReport13_12) //Mac 2023/08/09
         {
             ResultGridView[6, intNo].Value = "จำนวนรถ";
             ResultGridView[7, intNo].Value = intNo.ToString() + " คัน";
             ResultGridView[11, intNo].Value = "รายได้รวม";
             ResultGridView[12, intNo].Value = tl.ToString();
             ResultGridView[13, intNo].Value = to.ToString();
             if (selectedReportId == 13)
             {
                 ResultGridView[14, intNo].Value = tb.ToString("#,###,##0.00");
                 ResultGridView[15, intNo].Value = tv.ToString("#,###,##0.00");
                 ResultGridView[16, intNo].Value = tp.ToString("#,###,##0.00");
             }
             else
             {
                 ResultGridView[14, intNo].Value = tp.ToString("#,###,##0");
                 ResultGridView[15, intNo].Value = td.ToString("#,###,##0");
             }
         }
         else
         {
             ResultGridView[5, intNo].Value = "จำนวนรถ";
             ResultGridView[6, intNo].Value = intNo.ToString() + " คัน";
             ResultGridView[10, intNo].Value = "รายได้รวม";
             ResultGridView[11, intNo].Value = tl.ToString();
             ResultGridView[12, intNo].Value = to.ToString();
             if (selectedReportId == 13)
             {
                 ResultGridView[13, intNo].Value = tb.ToString("#,###,##0.00");
                 ResultGridView[14, intNo].Value = tv.ToString("#,###,##0.00");
                 ResultGridView[15, intNo].Value = tp.ToString("#,###,##0.00");
             }
             else
             {
                 ResultGridView[13, intNo].Value = tp.ToString("#,###,##0");
                 ResultGridView[14, intNo].Value = td.ToString("#,###,##0");
             }
         }
     }

     else if (selectedReportId == 22) //Mac 2019/05/07
     {
         ResultGridView.Rows[intNo].Cells[5].Value = "จำนวนสมาชิก";
         ResultGridView.Rows[intNo].Cells[6].Value = intNo.ToString("#,###,##0") + " ท่าน";
     }

     else if (selectedReportId == 25 || selectedReportId == 26) //Mac 2019/05/14
     {
         int sum1 = 0;
         int sum2 = 0;
         int sum3 = 0;
         int sum4 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToInt32(ResultGridView.Rows[i].Cells[1].Value.ToString());
             sum2 += Convert.ToInt32(ResultGridView.Rows[i].Cells[2].Value.ToString());
             sum3 += Convert.ToInt32(ResultGridView.Rows[i].Cells[3].Value.ToString());
             sum4 += Convert.ToInt32(ResultGridView.Rows[i].Cells[4].Value.ToString());
         }

         ResultGridView[0, intNo].Value = "รวม";
         ResultGridView[1, intNo].Value = sum1.ToString("#,###,##0");
         ResultGridView[2, intNo].Value = sum2.ToString("#,###,##0");
         ResultGridView[3, intNo].Value = sum3.ToString("#,###,##0");
         ResultGridView[4, intNo].Value = sum4.ToString("#,###,##0");
     }

     else if (selectedReportId == 32) //Mac 2019/05/16
     {
         double sum1 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToDouble(ResultGridView.Rows[i].Cells[8].Value.ToString());
         }

         ResultGridView.Rows[intNo].Cells[6].Value = "รวม";
         ResultGridView.Rows[intNo].Cells[7].Value = intNo.ToString("#,###,##0") + " รายการ";
         ResultGridView.Rows[intNo].Cells[8].Value = sum1.ToString("#,###,##0.00");
     }

     else if (selectedReportId == 36) //Mac 2019/05/14
     {
         int sum1 = 0;
         int sum2 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToInt32(ResultGridView.Rows[i].Cells[1].Value.ToString());
             sum2 += Convert.ToInt32(ResultGridView.Rows[i].Cells[2].Value.ToString());
         }

         ResultGridView[0, intNo].Value = "รวม";
         ResultGridView[1, intNo].Value = sum1.ToString("#,###,##0");
         ResultGridView[2, intNo].Value = sum2.ToString("#,###,##0");
     }

     else if (selectedReportId == 38)
     {
         double sum1;
         double sum2;
         double sum3;
         double sum4;
         double sum5;
         int sum6;
         int sum7;
         int sum8;
         int sum9;

         sum1 = 0;
         sum2 = 0;
         sum3 = 0;
         sum4 = 0;
         sum5 = 0;
         sum6 = 0;
         sum7 = 0;
         sum8 = 0;
         sum9 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToDouble(ResultGridView.Rows[i].Cells[2].Value.ToString());
             sum2 += Convert.ToDouble(ResultGridView.Rows[i].Cells[3].Value.ToString());
             sum3 += Convert.ToDouble(ResultGridView.Rows[i].Cells[4].Value.ToString());
             sum4 += Convert.ToDouble(ResultGridView.Rows[i].Cells[5].Value.ToString());
             sum5 += Convert.ToDouble(ResultGridView.Rows[i].Cells[6].Value.ToString());
             sum6 += Convert.ToInt32(ResultGridView.Rows[i].Cells[7].Value.ToString());
             sum7 += Convert.ToInt32(ResultGridView.Rows[i].Cells[8].Value.ToString());
             sum8 += Convert.ToInt32(ResultGridView.Rows[i].Cells[9].Value.ToString());
             sum9 += Convert.ToInt32(ResultGridView.Rows[i].Cells[10].Value.ToString());
         }

         ResultGridView[1, intNo].Value = "รวม";
         ResultGridView[2, intNo].Value = sum1.ToString("#,###,##0.00");
         ResultGridView[3, intNo].Value = sum2.ToString("#,###,##0.00");
         ResultGridView[4, intNo].Value = sum3.ToString("#,###,##0.00");
         ResultGridView[5, intNo].Value = sum4.ToString("#,###,##0.00");
         ResultGridView[6, intNo].Value = sum5.ToString("#,###,##0.00");
         ResultGridView[7, intNo].Value = sum6;
         ResultGridView[8, intNo].Value = sum7;
         ResultGridView[9, intNo].Value = sum8;
         ResultGridView[10, intNo].Value = sum9;
     }
     else if (selectedReportId == 39)
     {
         double sum1;
         double sum2;
         double sum3;
         double sum4;
         double sum5;

         sum1 = 0;
         sum2 = 0;
         sum3 = 0;
         sum4 = 0;
         sum5 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToDouble(ResultGridView.Rows[i].Cells[2].Value.ToString());
             sum2 += Convert.ToDouble(ResultGridView.Rows[i].Cells[3].Value.ToString());
             sum3 += Convert.ToDouble(ResultGridView.Rows[i].Cells[4].Value.ToString());
             sum4 += Convert.ToDouble(ResultGridView.Rows[i].Cells[5].Value.ToString());
             sum5 += Convert.ToDouble(ResultGridView.Rows[i].Cells[6].Value.ToString());
         }

         ResultGridView[1, intNo].Value = "รวม  " + intNo.ToString("#,###,##0") + " คัน";
         ResultGridView[2, intNo].Value = sum1.ToString("#,###,##0.00");
         ResultGridView[3, intNo].Value = sum2.ToString("#,###,##0.00");
         ResultGridView[4, intNo].Value = sum3.ToString("#,###,##0.00");
         ResultGridView[5, intNo].Value = sum4.ToString("#,###,##0.00");
         ResultGridView[6, intNo].Value = sum5.ToString("#,###,##0.00");
     }

     else if ((selectedReportId == 40) || (selectedReportId == 76)) //Mac 2018/02/21
     {
         ResultGridView[5, intNo].Value = "รวม  " + intNo.ToString("#,###,##0") + " คัน";
     }

     else if (selectedReportId == 23)
     {
         for (i = 0; i < intNo; i++)
         {
             if (Configs.Reports.UseReport24_3) //Mac 2021/10/20
             {
                 sPrice += Int32.Parse(ResultGridView.Rows[i].Cells[colCount - 5].Value.ToString());
                 sDiscount += Int32.Parse(ResultGridView.Rows[i].Cells[colCount - 4].Value.ToString());
                 sExcess += Int32.Parse(ResultGridView.Rows[i].Cells[colCount - 3].Value.ToString());
             }
             else
             {
                 //Mac 2021/07/23
                 sPrice += Int32.Parse(ResultGridView.Rows[i].Cells[colCount - 4].Value.ToString());
                 sDiscount += Int32.Parse(ResultGridView.Rows[i].Cells[colCount - 3].Value.ToString());
             }
         }
         if (Configs.Reports.UseReport24_3) //Mac 2021/10/20
         {
             ResultGridView.Rows[intNo].Cells[colCount - 8].Value = "ทั้งหมด";
             ResultGridView.Rows[intNo].Cells[colCount - 7].Value = intNo.ToString("#,###,##0") + " ครั้ง";
             ResultGridView.Rows[intNo].Cells[colCount - 6].Value = "รวม";
             ResultGridView.Rows[intNo].Cells[colCount - 5].Value = "" + sPrice.ToString("#,###,##0");
             ResultGridView.Rows[intNo].Cells[colCount - 4].Value = "" + sDiscount.ToString("#,###,##0");
             ResultGridView.Rows[intNo].Cells[colCount - 3].Value = "" + sExcess.ToString("#,###,##0");
         }
         else
         {
             //Mac 2021/07/23
             ResultGridView.Rows[intNo].Cells[colCount - 7].Value = "ทั้งหมด";
             ResultGridView.Rows[intNo].Cells[colCount - 6].Value = intNo.ToString("#,###,##0") + " ครั้ง";
             ResultGridView.Rows[intNo].Cells[colCount - 5].Value = "รวม";
             ResultGridView.Rows[intNo].Cells[colCount - 4].Value = "" + sPrice.ToString("#,###,##0");
             ResultGridView.Rows[intNo].Cells[colCount - 3].Value = "" + sDiscount.ToString("#,###,##0");
         }
     }

     else if (selectedReportId == 70 || selectedReportId == 71) //Mac 2019/05/16
     {
         double sum1 = 0;
         double sum2 = 0;
         double sum3 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToDouble(ResultGridView.Rows[i].Cells[4].Value.ToString());
             sum2 += Convert.ToDouble(ResultGridView.Rows[i].Cells[5].Value.ToString());
             sum3 += Convert.ToDouble(ResultGridView.Rows[i].Cells[6].Value.ToString());
         }

         ResultGridView[3, intNo].Value = "รวม";
         ResultGridView[4, intNo].Value = sum1.ToString("#,###,##0.00");
         ResultGridView[5, intNo].Value = sum2.ToString("#,###,##0.00");
         ResultGridView[6, intNo].Value = sum3.ToString("#,###,##0.00");
     }

     else if (selectedReportId == 78 || selectedReportId == 82) //Mac 2019/05/16
     {
         int sum1 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToInt32(ResultGridView.Rows[i].Cells[2].Value.ToString());
         }

         ResultGridView[1, intNo].Value = "รวม";
         ResultGridView[2, intNo].Value = sum1.ToString("#,###,##0");
     }

     else if (selectedReportId == 81) //Mac 2019/05/16
     {
         int sum1 = 0;
         int sum2 = 0;
         int sum3 = 0;
         int sum4 = 0;
         int sum5 = 0;
         int sum6 = 0;
         int sum7 = 0;
         int sum8 = 0;
         int sum9 = 0;
         double sum10 = 0;
         double sum11 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToInt32(ResultGridView.Rows[i].Cells[4].Value.ToString());
             sum2 += Convert.ToInt32(ResultGridView.Rows[i].Cells[5].Value.ToString());
             sum3 += Convert.ToInt32(ResultGridView.Rows[i].Cells[6].Value.ToString());
             sum4 += Convert.ToInt32(ResultGridView.Rows[i].Cells[7].Value.ToString());
             sum5 += Convert.ToInt32(ResultGridView.Rows[i].Cells[8].Value.ToString());
             sum6 += Convert.ToInt32(ResultGridView.Rows[i].Cells[9].Value.ToString());
             sum7 += Convert.ToInt32(ResultGridView.Rows[i].Cells[10].Value.ToString());
             sum8 += Convert.ToInt32(ResultGridView.Rows[i].Cells[11].Value.ToString());
             sum9 += Convert.ToInt32(ResultGridView.Rows[i].Cells[12].Value.ToString());
             sum10 += Convert.ToDouble(ResultGridView.Rows[i].Cells[13].Value.ToString());
             sum11 += Convert.ToDouble(ResultGridView.Rows[i].Cells[14].Value.ToString());
         }

         ResultGridView[3, intNo].Value = "รวม";
         ResultGridView[4, intNo].Value = sum1.ToString("#,###,##0");
         ResultGridView[5, intNo].Value = sum2.ToString("#,###,##0");
         ResultGridView[6, intNo].Value = sum3.ToString("#,###,##0");
         ResultGridView[7, intNo].Value = sum4.ToString("#,###,##0");
         ResultGridView[8, intNo].Value = sum5.ToString("#,###,##0");
         ResultGridView[9, intNo].Value = sum6.ToString("#,###,##0");
         ResultGridView[10, intNo].Value = sum7.ToString("#,###,##0");
         ResultGridView[11, intNo].Value = sum8.ToString("#,###,##0");
         ResultGridView[12, intNo].Value = sum9.ToString("#,###,##0");
         ResultGridView[13, intNo].Value = sum10.ToString("#,###,##0.00");
         ResultGridView[14, intNo].Value = sum1.ToString("#,###,##0.00");
     }
     else if (selectedReportId == 84 || selectedReportId == 85 || selectedReportId == 86 || selectedReportId == 87) //Mac 2019/11/15
     {
         int sum1 = 0;
         int sum2 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToInt32(ResultGridView.Rows[i].Cells[1].Value.ToString());
             sum2 += Convert.ToInt32(ResultGridView.Rows[i].Cells[2].Value.ToString());
         }

         ResultGridView[0, intNo].Value = "รวม";
         ResultGridView[1, intNo].Value = sum1.ToString("#,###,##0");
         ResultGridView[2, intNo].Value = sum2.ToString("#,###,##0");
     }
     else if (selectedReportId == 154) //Mac 2021/11/23
     {
         int sum1 = 0;
         double sum2 = 0;

         for (i = 0; i < intNo; i++)
         {
             sum1 += Convert.ToInt32(ResultGridView.Rows[i].Cells[2].Value.ToString());
             sum2 += Convert.ToDouble(ResultGridView.Rows[i].Cells[3].Value.ToString());
         }

         ResultGridView[1, intNo].Value = "รวม";
         ResultGridView[2, intNo].Value = sum1.ToString("#,###,##0");
         ResultGridView[3, intNo].Value = sum2.ToString("#,###,##0.00");
     }
 }*/