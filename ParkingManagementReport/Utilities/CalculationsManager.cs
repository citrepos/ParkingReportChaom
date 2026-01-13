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
        #endregion

        internal static void AddTotalToGridView(int selectedReportId, DataGridView ResultGridView)
        {
            if (ResultGridView.Rows.Count <= 0)
                return;

            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(TextFormatters.ErrorStacktraceFromException(ex));
            }
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

        internal static (decimal beforeVatCharge, decimal vatCharge, decimal totalCharge) CalculateVatFromFullPrice(double totalPrice)
        {
            const decimal VatMultiplier = 107m; // 100% + 7% VAT

            decimal total = Convert.ToDecimal(totalPrice);

            decimal beforeVat = total * (100m / VatMultiplier);
            decimal vat = total - beforeVat;

            decimal beforeVatCharge = Math.Round(beforeVat, 2, MidpointRounding.AwayFromZero);
            decimal vatCharge = Math.Round(vat, 2, MidpointRounding.AwayFromZero);
            decimal totalCharge = Math.Round(total, 2, MidpointRounding.AwayFromZero);

            return (beforeVatCharge, vatCharge, totalCharge);
        }


        internal static int GetPriceThanapoom(int totalMinute)
        {
            if (totalMinute - (Configs.ParkingFreeMinutes - 1) <= 0)
            {
                return 0;
            }
            else
            {
                bool isCarParking = AppGlobalVariables.Database.VehicleTypeEn == "Car";
                int hourlyRate = isCarParking ? 40 : 20;
                int realParkingMinute = totalMinute;

                // ปัดเศษขึ้นเป็นชั่วโมงถ้ามีเศษ
                int parkingHour = (realParkingMinute + 59) / 60;

                return hourlyRate * parkingHour;
            }
        }

        internal static int GetDiscountThanapoom(int promotionId = 0, int totalMinute = 0)
        {
            int hourlyRate = AppGlobalVariables.Database.VehicleTypeEn == "Car" ? 40 : 20;

            if (promotionId > 0)
            {
                string sql = $"SELECT * FROM promotion WHERE id = {promotionId}";
                DataTable dt = DbController.LoadData(sql);

                if (dt.Rows.Count == 0)
                    return 0;

                DataRow row = dt.Rows[0];
                int discountMinute = Convert.ToInt32(row["minute"]);
                int discountPrice = Convert.ToInt32(row["price"]);

                if (discountPrice > 0)
                    return discountPrice;

                int discountHour = (discountMinute + 59) / 60;
                return hourlyRate * discountHour;
            }

            if (totalMinute > 0)
            {
                int discountHour = (totalMinute + 59) / 60;
                return hourlyRate * discountHour;
            }

            return hourlyRate;
        }
    }
}