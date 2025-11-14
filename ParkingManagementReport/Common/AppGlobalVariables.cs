using System;
using System.Collections.Generic;

namespace ParkingManagementReport.Common
{
    internal class AppGlobalVariables
    {
        //public static int selectedReportId { get; set; } = 0;
        
        public static string IdText { get; set; }
        public static string IdTextUser { get; set; }
        public static string ConditionText { get; set; }
  
        public static string CurrentUserId { get; set; }
        public static string CurrentPaytype { get; set; }
        public static string CurrentCartype { get; set; }
        public static string startDateTimeText { get; set; }
        public static string endDateTimeText { get; set; }

        public static int[] IntTime;
        public static int[] IntPriceMin;
        public static int[] IntPriceHour;
        public static int[] IntHourRound;
        public static int[] IntExpense;
        public static int[] IntOver;
        public static int[] IntTime2;
        public static int[] IntPriceMin2;
        public static int[] IntPriceHour2;
        public static int[] IntHourRound2;
        public static int[] IntExpense2;
        public static int[] IntOver2;
        public static int[] CartypeZonePrice = new int[11];
        public static int[] CartypeZonePricePerHour = new int[11];
        public static string[] CartypeName = new string[11];
        public static string[] CartypeZoneStart = new string[11];
        public static string[] CartypeZoneStop = new string[11];
        public static string[] CartypeZoneStart2 = new string[11];
        public static string[] CartypeZoneStop2 = new string[11];
        public static int[] CartypeZonePrice2 = new int[11];
        public static int[] CartypeZonePricePerHour2 = new int[11];
        public static int[] CartypeZoneMinFree = new int[11];
        public static int[] CartypeZoneMinFree2 = new int[11];
        public static string[] CartypeDayWeek = new string[11];
        public static string[] CartypeIDHoliday = new string[11];
        public static int[] CartypeCardLoss = new int[11];
        public static bool[] CartypeOverDateUse = new bool[11];
        public static bool[] CartypeDriveAround = new bool[11];

        #region DICTIONARIES
        public static Dictionary<int, int> PromotionNamesMinuteMap { get; set; }
        public static Dictionary<int, string> CarTypesById { get; set; }
        public static Dictionary<int, string> DispensersById { get; set; }
        public static Dictionary<int, string> PromotionNamesById { get; set; }
        public static Dictionary<int, string> UsersById { get; set; }
        public static Dictionary<int, string> ReportsById { get; set; }
        public static Dictionary<string, string> ParamsLookup { get; set; }
        public static Dictionary<string, string> MemberStatusesLookup { get; set; }
        public static Dictionary<string, int> MemberGroupsToId { get; set; }
        public static Dictionary<string, int> MemberGroupMonthsToId { get; set; }
        public static Dictionary<string, int> RenewMemberGroupsToId { get; set; }

        /*
        public static Dictionary<string, int> PromotionNamesToId { get; set; }
        public static Dictionary<string, int> UsersToId { get; set; }
        public static Dictionary<string, int> CarTypesToId { get; set; }
        public static Dictionary<int, KeyValuePair<int,string>> PromotionKeyValuePairsByIndex { get; set; } 
        public static Dictionary<int, KeyValuePair<int, string>> UserKeyValuePairsByIndex { get; set; } 
        public static Dictionary<int, KeyValuePair<int, string>> CarTypeKeyValuePairsByIndex { get; set; }
        */
        #endregion DICTIONARIES_END

        public static class Database
        {
            public static string Name { get; internal set; }
            public static string VehicleTypeTh { get; internal set; }
            public static string VehicleTypeEn { get; internal set; }
            public static Dictionary<string, string> LookupList { get; set; }
        }

        public class Printings
        {
            public static string Header { get; internal set; }
            public static string Building { get; internal set; }
            public static string Company1 { get; internal set; }
            public static string Company2 { get; internal set; }
            public static string Address1 { get; internal set; }
            public static string Address2 { get; internal set; }
            public static string Tax1 { get; internal set; }
            public static string Tax2 { get; internal set; }
            public static string Office { get; internal set; }
            public static string Telephone { get; internal set; }
            public static string ReportFooter1 { get; internal set; }
            public static string ReportFooter2 { get; internal set; }
            public static string ReportFooter3 { get; internal set; }
            public static string ReportFooter4 { get; internal set; }
            public static string ReportFooter5 { get; internal set; }

            public static string ReceiptName { get; internal set; }
            public static string ReceiptNameVoidPay { get; internal set; }
            public static string PrintingFixedFormat { get; set; }
        }

        public static class OperatingUser
        {
            public static string Id { get; set; }
            public static string UserName { get; set; }
            public static string Name { get; set; }
            public static string Address { get; set; }
            public static string Tel { get; set; }
            public static string WorkId { get; set; }
            public static int Level { get; set; } = 0;
            public static bool LoginReady { get; set; } = false;
            public static int Price { get; set; } = 0;
            public static int Discount { get; set; } = 0;
            public static int TSumPrice { get; set; } = 0;
            public static int TSumDiscount { get; set; } = 0;
            public static int GroupReport { get; set; } = 0;
        }
    }
}