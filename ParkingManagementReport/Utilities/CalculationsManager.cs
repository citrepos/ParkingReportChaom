using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ParkingManagementReport.Common;
using ParkingManagementReport.Utilities.Database;
using ParkingManagementReport.Utilities.Formatters;

namespace ParkingManagementReport.Utilities
{
    internal class CalculationsManager
    {
        #region Cal
        internal static int CalFlatRate(DateTime dti, DateTime dto, int M, int P, int X)
        {
            int Price = 0;
            int totalM = 0;
            TimeSpan diffInOut;

            if (M == 0)
                return 0;

            diffInOut = dto - dti;
            totalM = diffInOut.Minutes;
            totalM += diffInOut.Hours * 60;
            totalM += diffInOut.Days * 24 * 60;

            Price = ((totalM - X) / M) * P;

            return Price;
        }

        internal static int CalPrice(int intHour, int intMin, bool UseNoDay)
        {
            int intPrice = 0;

            intMin += intHour * 60;

            int intDay = 1;
            if (!UseNoDay)
            {
                if (intMin > 1440)
                {
                    intDay = (intMin / 1440) + 1;
                }
            }

            int intMinDay = 0;
            for (int d = 0; d < intDay; d++)
            {
                int intMinCal = 0;
                int intHourCal = 0;

                if (!UseNoDay)
                {
                    if (intMin > 1440)
                    {
                        intMinDay = 1440;
                        intMin = intMin - 1440;
                    }
                    else
                    {
                        intMinDay = intMin;
                        intMin = 0;
                    }
                }
                else
                {
                    intMinDay = intMin;
                }

                int intPriceB = intPrice;

                for (int i = 0; i < AppGlobalVariables.IntTime.Count(); i++)
                {
                    if (intMinDay > AppGlobalVariables.IntTime[i])
                    {
                        intMinCal = AppGlobalVariables.IntTime[i];
                        intMinDay = intMinDay - intMinCal;
                    }
                    else
                    {
                        intMinCal = intMinDay;
                        intMinDay = 0;
                    }

                    if (UseNoDay)
                    {
                        intPriceB += intMinCal;
                        if (intPriceB >= 1440)
                        {
                            intMinCal += intMinDay;
                            intMinDay = 0;
                        }
                    }
                    if (AppGlobalVariables.IntPriceMin[i] > 0)
                    {
                        intPrice += AppGlobalVariables.IntPriceMin[i] * intMinCal;
                    }
                    else if (AppGlobalVariables.IntPriceHour[i] > 0)
                    {
                        if (intMinCal > 60)
                        {
                            intHourCal = intMinCal / 60;
                            intMinCal = intMinCal - (intHourCal * 60);
                        }
                        if (intMinCal >= AppGlobalVariables.IntHourRound[i])
                            intHourCal++;
                        intPrice += AppGlobalVariables.IntPriceHour[i] * intHourCal;
                        intHourCal = 0;
                    }
                    else if (AppGlobalVariables.IntExpense[i] > 0)
                    {
                        intPrice += AppGlobalVariables.IntExpense[i];
                    }

                    if (AppGlobalVariables.IntOver[i] > 0 && intMinDay > 0)
                    {
                        intPrice = intPriceB + AppGlobalVariables.IntOver[i];
                        intMinDay = 0;
                    }
                    if (intMinDay < 1)
                        break;
                }
            }
            if (intPrice < 0)
                intPrice = 0;
            return intPrice;
        }

        internal static int CalPrice2(int intHour, int intMin, bool UseNoDay)
        {
            int intPrice = 0;

            intMin += intHour * 60;

            int intDay = 1;
            if (!UseNoDay)
            {
                if (intMin > 1440)
                {
                    intDay = (intMin / 1440) + 1;
                }
            }

            int intMinDay = 0;
            for (int d = 0; d < intDay; d++)
            {
                int intMinCal = 0;
                int intHourCal = 0;

                if (!UseNoDay)
                {
                    if (intMin > 1440)
                    {
                        intMinDay = 1440;
                        intMin = intMin - 1440;
                    }
                    else
                    {
                        intMinDay = intMin;
                        intMin = 0;
                    }
                }
                else
                {
                    intMinDay = intMin;
                }

                int intPriceB = intPrice;

                for (int i = 0; i < AppGlobalVariables.IntTime2.Count(); i++)
                {
                    if (intMinDay > AppGlobalVariables.IntTime2[i])
                    {
                        intMinCal = AppGlobalVariables.IntTime2[i];
                        intMinDay = intMinDay - intMinCal;
                    }
                    else
                    {
                        intMinCal = intMinDay;
                        intMinDay = 0;
                    }

                    if (UseNoDay)
                    {
                        intPriceB += intMinCal;
                        if (intPriceB >= 1440)
                        {
                            intMinCal += intMinDay;
                            intMinDay = 0;
                        }
                    }

                    if (AppGlobalVariables.IntPriceMin2[i] > 0)
                    {
                        intPrice += AppGlobalVariables.IntPriceMin2[i] * intMinCal;
                    }
                    else if (AppGlobalVariables.IntPriceHour2[i] > 0)
                    {
                        if (intMinCal > 60)
                        {
                            intHourCal = intMinCal / 60;
                            intMinCal = intMinCal - (intHourCal * 60);
                        }
                        if (intMinCal >= AppGlobalVariables.IntHourRound2[i])
                            intHourCal++;
                        intPrice += AppGlobalVariables.IntPriceHour2[i] * intHourCal;
                        intHourCal = 0;
                    }
                    else if (AppGlobalVariables.IntExpense2[i] > 0)
                    {
                        intPrice += AppGlobalVariables.IntExpense2[i];
                    }

                    if (AppGlobalVariables.IntOver2[i] > 0 && intMinDay > 0)
                    {
                        intPrice = intPriceB + AppGlobalVariables.IntOver2[i];
                        intMinDay = 0;
                    }
                    if (intMinDay < 1)
                        break;
                }
            }
            if (intPrice < 0)
                intPrice = 0;
            return intPrice;
        }

        internal static KeyValuePair<int, int> CalPriceZoneOneDay(int intCarType, string strdti = "02/01/0001 00:00:00", string strdto = "02/01/0001 00:00:00", string ZoneStart = "", string ZoneStop = "", int ZonePrice = 0, int ZonePricePerHour = 0, int ZoneMinFree = 0, bool NoRound = false)
        {
            int intHourTmp = 0;
            int intMinTmp = 0;
            int intMinZone = 0;
            int intPriceZone = 0;
            //bool boolZoneStopStart = true;
            DateTime dti;
            DateTime dto;
            TimeSpan diffTime;

            if (ZoneStart != "" && ZoneStart != null && ZoneStart != ZoneStop)
            {
                bool boolZoneStopStart = false;
                bool booldtodti = false;

                //Mac 2017/03/21 --------
                if (!NoRound) //Mac 2018/01/08
                {
                    strdti = strdti.Substring(0, strdti.Length - 2) + "00";
                    if (strdto.Substring(strdto.Length - 8) != "23:59:59")
                        strdto = strdto.Substring(0, strdto.Length - 2) + "00";
                }
                //-----------------------

                dti = DateTime.Parse(strdti);
                dto = DateTime.Parse(strdto);
                diffTime = dto - dti;
                intHourTmp = diffTime.Hours;
                if (diffTime.Days > 0)
                    intHourTmp += diffTime.Days * 24;
                if (diffTime.Minutes > 0)
                    intHourTmp++;

                TimeSpan diffTimeZone;
                if (dto.ToShortDateString() == dti.ToShortDateString())
                    booldtodti = true;
                if (DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) >= DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart))
                {
                    diffTimeZone = DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) -
                    DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart);
                    boolZoneStopStart = true;
                }
                else
                {
                    diffTimeZone = DateTime.Parse(dti.AddDays(1).ToShortDateString() + " " + ZoneStop) -
                    DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart);
                    boolZoneStopStart = false;
                }

                intMinTmp = diffTimeZone.Minutes;
                intMinTmp += diffTimeZone.Hours * 60;
                intMinTmp += diffTimeZone.Days * 24 * 60;


                if (boolZoneStopStart) //stop > start
                {
                    if (booldtodti) //dto == dti
                    {
                        if (dti <= DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart)
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dto
                            && dto < DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop))
                        {
                            diffTime = dto - DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart);
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                        else if (dti <= DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart)
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) <= dto)
                        {
                            intMinZone = intMinTmp;
                            intPriceZone = ZonePrice;
                        }
                        else if (DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dti
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dto
                            && dto < DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop))
                        {
                            diffTime = dto - dti;
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                        else if (DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dti
                            && dti < DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop)
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) <= dto)
                        {
                            diffTime = DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) - dti;
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }

                    }
                    else //dto != dti
                    {
                        if (dti <= DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart)
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) < dto)
                        {
                            intMinZone = intMinTmp;
                            intPriceZone = ZonePrice;
                        }
                        else if (DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dti
                            && dti <= DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop)
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) < dto)
                        {
                            diffTime = DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) - dti;
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                        else if (dti <= DateTime.Parse(dto.ToShortDateString() + " " + ZoneStart)
                            && DateTime.Parse(dto.ToShortDateString() + " " + ZoneStart) <= dto
                            && DateTime.Parse(dto.ToShortDateString() + " " + ZoneStop) < dto)
                        {
                            intMinZone = intMinTmp;
                            intPriceZone = ZonePrice;
                        }
                        else if (dti <= DateTime.Parse(dto.ToShortDateString() + " " + ZoneStart)
                            && DateTime.Parse(dto.ToShortDateString() + " " + ZoneStart) <= dto
                            && dto <= DateTime.Parse(dto.ToShortDateString() + " " + ZoneStop))
                        {
                            diffTime = dto - DateTime.Parse(dto.ToShortDateString() + " " + ZoneStart);
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                    }
                }
                else //start > stop
                {
                    if (booldtodti) //dto == dti
                    {
                        if (DateTime.Parse(dti.AddDays(-1).ToShortDateString() + " " + ZoneStart) < dti
                            && dti < DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop)
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) < dto)
                        {
                            diffTime = DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) - dti;
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                        else if (DateTime.Parse(dti.AddDays(-1).ToShortDateString() + " " + ZoneStart) < dti
                            && dti < DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop)
                            && dto <= DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop))
                        {
                            diffTime = dto - dti;
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                        else if (dti <= DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart)
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dto)
                        {
                            diffTime = dto - DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart);
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                        else if (DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dti
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dto)
                        {
                            diffTime = dto - dti;
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                    }
                    else //dto != dti
                    {
                        if (dti < DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop)
                            && dto <= DateTime.Parse(dto.ToShortDateString() + " " + ZoneStop))
                        {
                            diffTime = DateTime.Parse(dti.ToShortDateString() + " " + ZoneStop) - dti;
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                        else if (DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dti
                            && DateTime.Parse(dto.ToShortDateString() + " " + ZoneStop) <= dto)
                        {
                            diffTime = DateTime.Parse(dto.ToShortDateString() + " " + ZoneStop) - dti;
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                        else if (dti <= DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart)
                            && DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dto
                            && dto < DateTime.Parse(dto.ToShortDateString() + " " + ZoneStop))
                        {
                            diffTime = dto - DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart);
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                        else if (dti <= DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart)
                            && DateTime.Parse(dto.ToShortDateString() + " " + ZoneStop) <= dto)
                        {
                            intMinZone = intMinTmp;
                            intPriceZone = ZonePrice;
                        }
                        else if (DateTime.Parse(dti.ToShortDateString() + " " + ZoneStart) < dti
                            && dto < DateTime.Parse(dto.ToShortDateString() + " " + ZoneStop))
                        {
                            diffTime = dto - dti;
                            intMinZone = diffTime.Minutes;
                            intMinZone += diffTime.Hours * 60;
                            intMinZone += diffTime.Days * 24 * 60;
                            intPriceZone = ZonePrice;
                        }
                    }
                }
            }

            if (intMinZone <= ZoneMinFree) //Mac 2015/05/09
            {
                intPriceZone = 0;
            }
            else
            {
                intPriceZone += ((intMinZone - ZoneMinFree) / 60) * ZonePricePerHour;
                if (((intMinZone - ZoneMinFree) % 60) > 0)
                    intPriceZone += ZonePricePerHour;
            }

            return new KeyValuePair<int, int>(intMinZone, intPriceZone);
        }

        internal static int CalculateColumnOffset()
        {
            int offset = 0;

            if (Configs.IsVillage && Configs.Use2Camera) offset = 5;
            if (Configs.NoPanelUp2U == "2") offset += 4;
            if (Configs.Reports.UseReport1_6 || Configs.Reports.UseReport1_8) offset = 1;

            return offset;
        }

        internal static (double parkingPrice, double feeCharge) CalculateParkingPriceAndFee(string dw, bool notDay, int iteration, DataTable dataTable)
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

        internal static (decimal totalFee, int totalCards, int totalChargedCards) CalculateDailySummary(DataTable data, string paymentStatus, DateTime date, int? filterPromotionId = null)
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
        internal static (double beforeVatCharge, double vatCharge, double totalCharge) CalculatePriceSummaryAndVat(double totalPrice)
        {
            double currentBeforeVat = totalPrice;
            double currentVat = 0.07 * totalPrice;
            double currentTotal = currentBeforeVat + currentVat;

            double beforeVatCharge = Math.Round(currentBeforeVat, 2);
            double vatCharge = Math.Round(currentVat, 2);
            double totalCharge = Math.Round(currentTotal, 2);

            return (beforeVatCharge, vatCharge, totalCharge);
        }

        internal static DataTable GetFeeAndVatSummaryFromMemberGroupPriceMonth(string sql, string promotionRangeFrom, string promotionRangeTo)
        {
            DataTable dataTable = DbController.LoadData(sql);

            DataTable resultTable = new DataTable("");
            resultTable.Columns.Add("ลำดับ", typeof(string));
            //resultTable.Columns.Add("CIT Code", typeof(string));
            resultTable.Columns.Add("WPT Code", typeof(string));
            resultTable.Columns.Add("Customer", typeof(string));
            resultTable.Columns.Add("ค่าบริการก่อน VAT", typeof(double));
            resultTable.Columns.Add("VAT", typeof(double));
            resultTable.Columns.Add("ค่าบริการทั้งหมด", typeof(double));

            double sumBeforeVat = 0;
            double sumVat = 0;
            double sumTotalCharge = 0;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                try
                {
                    string currentWptCode = dataTable.Rows[i]["WPT Code"].ToString();
                    string currentCustomerName = dataTable.Rows[i]["บริษัท"].ToString();
                    double currentPrice = double.TryParse(dataTable.Rows[i]["รวมค่าบัตรสมาชิก"]?.ToString(), out var price) ? price : 0;

                    var (beforeVatCharge, vatCharge, totalCharge) = CalculationsManager.CalculatePriceSummaryAndVat(currentPrice);

                    DataRow row = resultTable.NewRow();
                    row["ลำดับ"] = (i + 1).ToString();
                    //row["CIT Code"] = "";
                    row["WPT Code"] = currentWptCode;
                    row["Customer"] = TextFormatters.RemoveBracketFromName(currentCustomerName);
                    row["ค่าบริการก่อน VAT"] = beforeVatCharge;
                    row["VAT"] = vatCharge;
                    row["ค่าบริการทั้งหมด"] = totalCharge;

                    sumBeforeVat += beforeVatCharge;
                    sumVat += vatCharge;
                    sumTotalCharge += totalCharge;

                    resultTable.Rows.Add(row);
                }
                catch { }
            }

            resultTable.Rows.Add("", "",
                "รวม",
                sumBeforeVat,
                sumVat,
                sumTotalCharge);

            return resultTable;
        }
        #endregion

        internal static void AddTotalToGridView(int selectedReportId, DataGridView ResultGridView)
        {
            if (ResultGridView.Rows.Count <= 0)
                return;

            int rowCount = ResultGridView.Rows.Count - 1;
            int totalPrice = 0, totalDiscount = 0;
            int villageOffset = 0;
            int colCount = ResultGridView.Rows[0].Cells.Count;
            int totalExcess = 0;

            if (Configs.IsVillage && (selectedReportId == 0 || selectedReportId == 1 ||
                selectedReportId == 8 || selectedReportId == 90 || selectedReportId == 91) && Configs.Use2Camera)
            {
                villageOffset = 5;
            }

            if ((selectedReportId == 0 || selectedReportId == 1 || selectedReportId == 8 ||
                selectedReportId == 90 || selectedReportId == 91) && Configs.NoPanelUp2U == "2")
            {
                villageOffset += 4;
            }

            switch (selectedReportId)
            {
                case 0:
                case 1:
                case 8:
                case 90:
                case 91:
                    CalculateStandardReport(rowCount, villageOffset);
                    break;

                case 2:
                    for (int i = 0; i < rowCount; i++)
                    {
                        totalPrice += int.Parse(ResultGridView.Rows[i].Cells[5].Value.ToString());
                        totalDiscount += int.Parse(ResultGridView.Rows[i].Cells[6].Value.ToString());
                    }

                    ResultGridView.Rows[rowCount].Cells[4].Value = "รวม";
                    ResultGridView.Rows[rowCount].Cells[5].Value = totalPrice.ToString("#,###,##0");
                    ResultGridView.Rows[rowCount].Cells[6].Value = totalDiscount.ToString("#,###,##0");
                    break;

                case 29:
                    int col1 = 0, col2 = 0, col3 = 0;

                    for (int i = 0; i < rowCount; i++)
                    {
                        col1 += int.Parse(ResultGridView.Rows[i].Cells[1].Value.ToString());
                        col2 += int.Parse(ResultGridView.Rows[i].Cells[2].Value.ToString());
                        col3 += int.Parse(ResultGridView.Rows[i].Cells[3].Value.ToString());
                        totalPrice += int.Parse(ResultGridView.Rows[i].Cells[4].Value.ToString());
                    }

                    ResultGridView[0, rowCount].Value = "รวม";
                    ResultGridView[1, rowCount].Value = col1.ToString("#,###,##0");
                    ResultGridView[2, rowCount].Value = col2.ToString("#,###,##0");
                    ResultGridView[3, rowCount].Value = col3.ToString("#,###,##0");
                    ResultGridView[4, rowCount].Value = totalPrice.ToString("#,###,##0");
                    break;

                case 3:
                case 10:
                    ResultGridView.Rows[rowCount].Cells[3].Value = "รวม";
                    ResultGridView.Rows[rowCount].Cells[4].Value = $"{rowCount:#,###,##0} ครั้ง";
                    break;

                case 4:
                case 30:
                case 31:
                case 92:
                case 93:
                    ResultGridView.Rows[rowCount].Cells[3].Value = "จำนวนรถ";
                    ResultGridView.Rows[rowCount].Cells[4].Value = $"{rowCount:#,###,##0} คัน";
                    break;

                default:
                    break;
            }

            void CalculateStandardReport(int count, int offset)
            {
                if ((selectedReportId == 1 || selectedReportId == 91) && Configs.Reports.UseReport1_6)
                    offset = 1;
                else if (Configs.Reports.UseReport1_4)
                    offset = 3;
                else if (Configs.Reports.UseReport1_6 || Configs.Reports.UseReport1_8)
                    offset = 1;

                for (int i = 0; i < count; i++)
                {
                    if (int.TryParse(ResultGridView.Rows[i].Cells[6 + offset].Value?.ToString(), out int price))
                        totalPrice += price;

                    if (int.TryParse(ResultGridView.Rows[i].Cells[7 + offset].Value?.ToString(), out int discount))
                        totalDiscount += discount;
                }

                ResultGridView.Rows[count].Cells[3 + offset].Value = "จำนวนรถ";
                ResultGridView.Rows[count].Cells[4 + offset].Value = $"{count:#,###,##0} คัน";
                ResultGridView.Rows[count].Cells[5 + offset].Value = "รวม";
                ResultGridView.Rows[count].Cells[6 + offset].Value = totalPrice.ToString("#,###,##0");
                ResultGridView.Rows[count].Cells[7 + offset].Value = totalDiscount.ToString("#,###,##0");
            }
        }
    }
}