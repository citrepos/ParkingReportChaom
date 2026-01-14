using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagementReport.Common
{
    internal class Configs
    {
        #region GENERIC
        public static int PriceCardLoss { get; internal set; }
        public static int NoCar { get; internal set; }

        public static string NoPanelUp2U { get; internal set; }
        public static string NotShowNoString { get; internal set; }
        public static string Mode { get; internal set; }
        public static string UseDayWeek { get; internal set; }
        public static string Discount { get; internal set; }

        public static bool IsModeDispenser { get; internal set; }
        public static bool IsOutReceiptNameMonth { get; internal set; }
        public static bool IsOffline { get; internal set; }

        public static bool UseOfflineMode { get; internal set; }
        public static bool Use2Camera { get; internal set; }
        public static bool UseCarType { get; internal set; }
        public static bool UseMemberType { get; internal set; }
        public static bool UseMemLicense { get; internal set; }
        public static bool UseReceiptFor1Out { get; internal set; }
        public static bool UseVisitorDetail { get; internal set; }
        public static bool UseLogReport { get; internal set; }
        public static bool UseProIDAll { get; internal set; }
        public static bool UsePaymentBeam { get; internal set; }
        public static bool UsePaymentKsher { get; internal set; }
        public static bool UsePaymentRabbit { get; internal set; }

        public static bool UseMemberGroupPriceMonth { get; internal set; }
        public static bool UseActivePromotion { get; internal set; }
        public static bool UseGroupPrice { get; internal set; }
        public static bool UseSlipRecord { get; internal set; }
        public static bool UseHoliday { get; internal set; }
        public static bool UseFlatRateProSetPrice { get; internal set; }
        public static bool UseNameOnCard { get; internal set; }
        public static bool UseAsciiMember { get; internal set; }
        public static bool UseMemberLicensePlate { get; internal set; }
        public static bool UseMemo { get; internal set; }
        public static bool UseMifare { get; internal set; }
        public static bool UseSettingNewMember { get; internal set; }
        public static bool UseReceiptName { get; internal set; }
        public static bool Member2Cartype { get; internal set; }
        public static bool OutReceiptNameMonth { get; internal set; }
        public static bool IsVillage { get; internal set; }
        public static bool IsSwitch { get; internal set; }
        public static bool VillageState { get; internal set; }
        public static bool ShowConditionMemberGroupPriceMonth { get; internal set; }


        public static string IPIn1 { get; internal set; }
        public static string IPIn2 { get; internal set; }
        public static string IPIn3 { get; internal set; }
        public static string IPOut1 { get; internal set; }
        public static string IPOut2 { get; internal set; }
        public static string IPOut3 { get; internal set; }
        public static string ServerIP { get; internal set; }
        public static string[] NoshowSelectTime { get; internal set; }

        public static bool UseVoidSlip { get; internal set; }
        public static bool IsPrintCarIn { get; internal set; }
        public static bool UseGroupPromotion { get; internal set; }
        public static bool VisitorFillDetail { get; internal set; }
        public static bool UseAsiaTriqPrice { get; internal set; }
        public static bool UsePrintQRCode { get; internal set; }
        public static bool UsePrintBarcode { get; internal set; }
        public static bool UsePrintOfficer { get; internal set; }
        public static bool UseCalVatFromTotal { get; internal set; }
        public static bool UseReceiptFor1Mem { get; internal set; }
        public static bool UseQRCodeNew { get; internal set; }
        public static bool UseNotDay { get; internal set; }
        public static bool UseMemberHourBalance { get; internal set; }
        public static bool UseFormVoidSlip { get; internal set; }
        public static bool UsePDFOnly { get; internal set; }
        public static bool UseTax { get; internal set; }
        public static int CarInRepeat { get; internal set; }
        public static int ParkingFreeMinutes { get; internal set; }
        public static bool UseCardLossPrice { get; internal set; }
        public static bool UseLastPromotion { get; internal set; }
        #endregion

        public class Hardwares
        {
            #region Mifare
            public static bool IsMFPassiveInProx { get; internal set; }
            public static bool UseMFReaderIn { get; internal set; }
            public static bool UseMFReaderOut { get; internal set; }
            #endregion

            #region Bluetooth
            public static bool UseBluetoothReaderIn { get; internal set; }
            public static bool UseBluetoothReaderOut { get; internal set; }
            #endregion

            #region Prox (Bluetooth)
            public static bool UsePXReaderIn { get; internal set; }
            public static bool UsePXReaderOut { get; internal set; }
            public static string PortControl { get; internal set; }
            public static string PortMifare { get; internal set; }
            public static string PortProxIn { get; internal set; }
            public static string PortProxOut { get; internal set; }
            #endregion

            #region Dispensers
            public static bool ModeDispenser { get; internal set; }
            public static string DispenserIP { get; internal set; }
            public static string DispenserNo { get; internal set; }
            public static string DispenserName { get; internal set; }
            public static string PortMifareIn { get; internal set; }
            public static string PortMifareOut { get; internal set; }
            public static int DispenserNoCard { get; internal set; }
            #endregion
        }

        public class Paths
        {
            public static string BackupDirectory { get; internal set; }
            public static string ServerDirectory { get; internal set; }
        }

        public class Reports
        {
            public static bool UseReportDateString { get; internal set; }
            public static bool UseReportHourUse { get; internal set; }
            //public static bool UseReportThanapoom { get; internal set; }

            public static bool Report3Decimal { get; internal set; }
            public static bool ReportPriceSplitLosscard { get; internal set; }
            public static bool ReportProsetPriceDayWeek { get; internal set; }
            public static bool ReportSearchMemberGroup { get; internal set; }
            public static bool ReportSearchMemGroup { get; internal set; }
            public static bool ReportNoRunning { get; internal set; }
            public static bool ReportCartypeFree15Min { get; internal set; }
        }

    }
}