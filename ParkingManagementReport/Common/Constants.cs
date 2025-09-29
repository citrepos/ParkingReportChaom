
namespace ParkingManagementReport.Common
{
    internal class Constants
    {
        public static class TextBased
        {
            public static readonly string All = "ALL";
            public static readonly string Error = "ERROR";
            public static readonly string Visitor = "Visitor";
            public static readonly string Member = "Member";

            #region Member
            public static readonly string MemberCardTypeWithPayment = "ค่าบัตรสมาชิก";
            public static readonly string MemberCardTypeNonPayment = "ไม่ใช่ค่าบัตรสมาชิก";

            public static readonly string MemberStatusActive = "ใช้งาน";
            public static readonly string MemberStatusCanceled = "ยกเลิก";
            public static readonly string MemberStatusLossCard = "บัตรหาย";

            public static readonly string CreateNewMemberProcessState = "สร้างใหม่";
            public static readonly string UpdateMemberProcessState = "อัพเดต";
            #endregion

            #region Payment
            public static readonly string PaymentStatusPaid = "ชำระเงิน";
            public static readonly string PaymentStatusUnPaid = "ไม่ชำระเงิน";

            public static readonly string PaymentChannelPromptPay = "PromptPay";
            public static readonly string PaymentChannelTrueMoney = "TrueMoney";
            public static readonly string PaymentChannelCash = "เงินสด";
            public static readonly string PaymentChannelEDC = "EDC";
            #endregion

            public static class Generic
            {
                public static readonly string ContactHeaderName = "ติดต่อ (เรื่อง/ชื่อ)";
                public static readonly string MemberGroup = "กลุ่มสมาชิก";
            }

            /* public static class FormulaFields
            {
                public static readonly string ReportName = "ReportName";
                public static readonly string CompanyName = "CompanyName";

            } */
        }

        public static class ColumnNames
        {

        }
    }
}
