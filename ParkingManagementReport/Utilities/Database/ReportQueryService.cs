using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ParkingManagementReport.Common;
using ParkingManagementReport.Utilities.Formatters;

namespace ParkingManagementReport.Utilities.Database
{
    internal class ReportQueryService
    {
        private int selectedReportId;
        private string paymentChannel;
        private string recordNumber;
        private string user;
        private string carType;
        private string licensePlate;
        private string promotionName;
        private string cardId;
        private string nameOnCard;
        private string memberType;
        private string memberGroupMonth;
        private string memberName;
        private string memberRenewalType;
        private string memberProcessState;
        private string memberCardType;
        private string guardhouse;
        private string paymentStatus;
        private string address;
        private string up2UName;
        private string up2UStaffId;
        private string up2UStickerNumber;
        private string up2UCarType;
        private string memberGroup;
        private string memberId;
        private string memberStatus;
        private string memberBirthMonth;
        private string memberParkingCountStart;
        private string memberParkingCountEnd;
        private string parkingGreater;
        private string parkingLesser;
        private string parkingBetweenFrom;
        private string parkingBetweenTo;
        private string startDateTimeText;
        private string endDateTimeText;
        private string paymentGateWay;
        private bool isRegistrationDateChecked;
        private bool isExpirationDateChecked;
        private bool isParkingGreaterChecked;
        private bool isParkingLesserChecked;
        private bool isParkingBetweenChecked;
        private bool isLegitPromotionRange;
        private DateTime startDate;
        private DateTime endDate;
        private DateTime startTime;
        private DateTime endTime;
        private DateTime memberExpirationStartDate;
        private DateTime memberExpirationEndDate;
        private DateTime firstDayOfMonth;
        private DateTime lastDayOfMonth;
        private DateTime firstDayOfMonthEnd;
        private DateTime lastDayOfMonthEnd;

        private int carTypeId;
        private int userId;
        private int promotionId;
        private int memberTypeId;
        private int memberGroupMonthId;
        private int memberTypeSelectedIndex;
        private int iteration;
        private int sumCalQuota109;
        private int promotionRangeFrom;
        private int promotionRangeTo;

        public string BuildReportQuery(int selectedReportId,
            string paymentChannel,
            string recordNumber,
            string user,
            string carType,
            string licensePlate,
            string promotionName,
            string cardId,
            string nameOnCard,
            string memberType,
            string memberGroupMonth,
            string memberName,
            string memberRenewalType,
            string memberProcessState,
            string memberCardType,
            string guardhouse,
            string paymentStatus,
            string address,
            string up2UName,
            string up2UStaffId,
            string up2UStickerNumber,
            string up2UCarType,
            string memberGroup,
            string memberId,
            string memberStatus,
            //string memberBirthMonth,
            string memberParkingCountStart,
            string memberParkingCountEnd,
            int memberTypeSelectedIndex,
            string parkingGreater,
            string parkingLesser,
            string parkingBetweenFrom,
            string parkingBetweenTo,
            string promotionRangeFrom,
            string promotionRangeTo,
            bool isRegistrationDateChecked, 
            bool isExpirationDateChecked, bool isParkingGreaterChecked, bool isParkingLesserChecked, bool isParkingBetweenChecked,
            DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime, DateTime memberExpirationStartDate, DateTime memberExpirationEndDate)
        {
            this.selectedReportId = selectedReportId;
            this.paymentChannel = TextFormatters.RemoveSpecialCharacters(paymentChannel);
            this.recordNumber = TextFormatters.RemoveSpecialCharacters(recordNumber);
            this.user = TextFormatters.RemoveSpecialCharacters(user);
            this.carType = TextFormatters.RemoveSpecialCharacters(carType);
            this.licensePlate = TextFormatters.RemoveSpecialCharacters(licensePlate);
            this.promotionName = TextFormatters.RemoveSpecialCharacters(promotionName);
            this.cardId = TextFormatters.RemoveSpecialCharacters(cardId);
            this.nameOnCard = TextFormatters.RemoveSpecialCharacters(nameOnCard);
            this.memberType = TextFormatters.RemoveSpecialCharacters(memberType);
            this.memberGroupMonth = TextFormatters.RemoveSpecialCharacters(memberGroupMonth);
            this.memberName = TextFormatters.RemoveSpecialCharacters(memberName);
            this.memberRenewalType = TextFormatters.RemoveSpecialCharacters(memberRenewalType);
            this.memberProcessState = TextFormatters.RemoveSpecialCharacters(memberProcessState);
            this.memberCardType = TextFormatters.RemoveSpecialCharacters(memberCardType);
            this.guardhouse = TextFormatters.RemoveSpecialCharacters(guardhouse);
            this.paymentStatus = TextFormatters.RemoveSpecialCharacters(paymentStatus);
            this.address = TextFormatters.RemoveSpecialCharacters(address);
            this.up2UName = TextFormatters.RemoveSpecialCharacters(up2UName);
            this.up2UStaffId = TextFormatters.RemoveSpecialCharacters(up2UStaffId);
            this.up2UStickerNumber = TextFormatters.RemoveSpecialCharacters(up2UStickerNumber);
            this.up2UCarType = TextFormatters.RemoveSpecialCharacters(up2UCarType);
            this.memberGroup = TextFormatters.RemoveSpecialCharacters(memberGroup);
            this.memberId = TextFormatters.RemoveSpecialCharacters(memberId);
            this.memberStatus = TextFormatters.RemoveSpecialCharacters(memberStatus);
            this.memberBirthMonth = TextFormatters.RemoveSpecialCharacters(memberBirthMonth);
            this.memberParkingCountStart = TextFormatters.RemoveSpecialCharacters(memberParkingCountStart);
            this.memberParkingCountEnd = TextFormatters.RemoveSpecialCharacters(memberParkingCountEnd);
            this.parkingGreater = TextFormatters.RemoveSpecialCharacters(parkingGreater);
            this.parkingLesser = TextFormatters.RemoveSpecialCharacters(parkingLesser);
            this.parkingBetweenFrom = TextFormatters.RemoveSpecialCharacters(parkingBetweenFrom);
            this.parkingBetweenTo = TextFormatters.RemoveSpecialCharacters(parkingBetweenTo);
            this.isRegistrationDateChecked = isRegistrationDateChecked;
            this.isExpirationDateChecked = isExpirationDateChecked;
            this.isParkingGreaterChecked = isParkingGreaterChecked;
            this.isParkingLesserChecked = isParkingLesserChecked;
            this.isParkingBetweenChecked = isParkingBetweenChecked;
            this.startDate = startDate;
            this.endDate = endDate;
            this.startTime = startTime;
            this.endTime = endTime;
            this.memberExpirationStartDate = memberExpirationStartDate;
            this.memberExpirationEndDate = memberExpirationEndDate;
            this.memberTypeSelectedIndex = memberTypeSelectedIndex;

            firstDayOfMonth = new DateTime(startDate.Year, startDate.Month, 1);
            lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            firstDayOfMonthEnd = new DateTime(endDate.Year, endDate.Month, 1);
            lastDayOfMonthEnd = firstDayOfMonthEnd.AddMonths(1).AddDays(-1);

            iteration = 0;

            this.carTypeId = AppGlobalVariables.CarTypesById.First(kvp => kvp.Value == carType).Key;
            this.promotionId = AppGlobalVariables.PromotionNamesById.First(kvp => kvp.Value == promotionName).Key;
            this.userId = AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
            this.memberTypeId = AppGlobalVariables.MemberGroupsToId[memberType];
            this.memberGroupMonthId = AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
            this.startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " " + startTime.ToLongTimeString();
            this.endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " " + endTime.ToLongTimeString();

            this.isLegitPromotionRange = CheckAndUpdatePromotionRange(promotionRangeFrom, promotionRangeTo);
           
            string reportQuery = GenerateReportQuery();

            //Configs.WriteLogFile(
            //    $"SearchButton_Click | ReportName = {ReportComboBox.Text}, Start = {strstartDate}, Stop = {strendDate}, Staff = {user}, Cartype = {carType}, " +
            //    $"License = {LicensePlateTextBox.Text}, Promotion = {promotionName}, CardID = {CardIdTextBox.Text}, TypeMem = {memberType}",
            //    AppGlobalVariables.OperatingUser.Name
            //);

            return reportQuery;
        }

        private string GenerateReportQuery()
        {
            string sql = "";


            switch (selectedReportId)
            {
                case 0:
                case 1:
                case 5:
                case 9:
                case 90:
                case 91:
                    sql = GetGenericReport();
                    break;
                case 2:
                    sql = "SELECT A.no as ลำดับ,C.name as ชื่อเจ้าหน้าที่,";
                    sql += "case  when A.gate = 'I' then 'ขาเข้า' when A.gate = 'O' then 'ขาออก' when A.gate = 'B' then 'ขาเข้า/ขาออก' end as ประตู,";
                    sql += "date_format(A.datein, '%d/%m/%Y %H:%i:%s') as เวลาเข้า,date_format(A.dateout, '%d/%m/%Y %H:%i:%s') as เวลาออก,";
                    sql += " CASE WHEN B.price > 0 THEN B.price ELSE 0 END as รายได้,";
                    sql += " CASE WHEN B.discount > 0 THEN B.discount ELSE 0 END as ส่วนลด FROM";
                    sql += " user C,user_record A";
                    sql += " LEFT JOIN(";
                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0)
                    {
                        sql += " SELECT SUM(recordout.price) AS price";
                        sql += " ,SUM(recordout.discount) AS discount";
                        sql += " ,recordout.userno FROM recordout left join recordin on recordout.no = recordin.no";
                        sql += " where recordin.notshow = 'N'";
                        sql += " GROUP BY recordout.userno) B";
                    }
                    else
                    {
                        sql += " SELECT SUM(price) AS price";
                        sql += " ,SUM(discount) AS discount";
                        sql += " ,userno FROM recordout";
                        sql += " GROUP BY userno) B";
                    }
                    sql += " ON A.no = B.userno";
                    sql += " WHERE C.id=A.id";
                    sql += " AND A.datein BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    //if (UserComboBox.SelectedIndex > 0)
                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND A.id =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }
                    //if (GuardhouseComboBox.SelectedIndex > 0)
                    if (guardhouse != String.Empty)
                        sql += " and A.guardhouse = '" + guardhouse + "' ";
                    sql += " ORDER BY A.no";
                    break;
                case 3:
                    sql = "SELECT liftrecord.no as ลำดับ, date_format(liftrecord.datelift, '%d/%m/%Y %H:%i:%s') as เวลายก,user.name as เจ้าหน้าที่," //Mac 2018/12/21
                        + "case  when liftrecord.gate LIKE 'I%' then 'ขาเข้า' when liftrecord.gate LIKE 'O%' then 'ขาออก' when liftrecord.gate = 'B' then 'ขาเข้า/ขาออก' end as ประตู ,"
                        + "liftrecord.license as บันทึก FROM ";
                    sql += "user,liftrecord";
                    sql += " WHERE user.id=liftrecord.userid";
                    sql += " AND liftrecord.datelift BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND liftrecord.userid =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }
                    //if (licensePlate != "")
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND liftrecord.license LIKE '%" + licensePlate + "%'";

                    sql += " ORDER BY liftrecord.datelift";
                    break;
                case 4:
                case 92:
                    sql = GetCarIn();
                    break;
                case 6:
                    sql = GetReportDistinct();
                    break;
                case 7:
                    sql = "SELECT date_format(liftrecord.datelift, '%d/%m/%Y %H:%i:%s') as เวลายก ,user.name as พนักงาน,"
                        + "case  when liftrecord.gate LIKE 'I%' then 'ขาเข้า' when liftrecord.gate LIKE 'O%' then 'ขาออก' when liftrecord.gate = 'B' then 'ขาเข้า/ขาออก' end as ประตู ,"
                        + "liftrecord.license as บันทึก,liftrecord.picdiv,liftrecord.piclic FROM ";
                    sql += "user,liftrecord";
                    sql += " WHERE user.id=liftrecord.userid";
                    sql += " AND liftrecord.datelift BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND liftrecord.userid =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND liftrecord.license LIKE '%" + licensePlate + "%'";

                    sql += " ORDER BY liftrecord.datelift";
                    break;
                case 8:
                    sql = AllCarIn();
                    break;
                case 10:
                    sql = "select no, proid as 'รหัสโปรโมชั่น', (select name from promotion where id = recordin.proid) as 'ชื่อโปรโมชั่น', license as 'ทะเบียน', date_format(time_estamp, '%d/%m/%Y %H:%i:%s') as 'เวลาที่ทำรายการ', user_estamp as 'ผู้ทำรายการ' from recordin";
                    sql += " where proid > 0 and time_estamp BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    if (user != Constants.TextBased.All)
                        sql += " AND user_estamp = '" + user + "'";

                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " AND cartype =" + carTypeId;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " AND cartype != 200";

                    if (promotionName != Constants.TextBased.All)
                        sql += " AND proid =" + promotionId;

                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND license LIKE '%" + licensePlate + "%'";


                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND id = " + cardId;

                    sql += " order by time_estamp, proid";
                    break;
                case 11:
                    sql = GetReportGroupPriceData();
                    break;
                case 12:
                    sql = PricePromotion();
                    break;
                case 13:
                    sql = PricePromotion();
                    break;
                case 14:
                    sql = "SELECT recordout.printno,recordout.no,recordin.cartype,case when recordin.license = 'NO' then recordin.id when recordin.license = '' then recordin.id when recordin.license = '' then recordin.id else recordin.license end,recordin.datein,recordout.userout,recordout.dateout,recordout.proid,recordout.discount,recordout.userout,recordout.userout,recordout.losscard,recordout.overdate,recordout.price,recordout.proid";
                    sql += " FROM recordin,recordout";
                    sql += " WHERE dateout BETWEEN";

                    sql += " (SELECT user_record.datein FROM ";
                    sql += "user,user_record";
                    sql += " WHERE user.id=user_record.id";
                    sql += " AND dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND user_record.id =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }
                    sql += " ORDER BY user_record.datein LIMIT 1)";

                    sql += " AND '" + endDateTimeText + "'";
                    sql += " AND recordin.no = recordout.no";
                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND recordout.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    {
                        sql += " AND recordin.cartype =" + carTypeId;
                    }
                    if (carType == Constants.TextBased.Visitor)
                    {
                        sql += " AND recordin.cartype != 200";
                    }
                    if (promotionName != Constants.TextBased.All)
                    {
                        sql += " AND recordout.proid =" + promotionId;
                    }
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND recordin.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND recordin.id = " + cardId;
                    sql += " ORDER BY recordout.no";
                    break;

                case 15:
                    sql = "SELECT recordout.proid, COUNT(recordout.proid) ";
                    if (Configs.UseMemberLicensePlate)
                        sql += "from recordin left join recordout on recordin.no = recordout.no LEFT JOIN member ON member.license = recordin.license";
                    //sql += " from recordin left join recordout on recordin.no = recordout.no left join member on member.license like concat('%',recordin.license,'%')"; //Mac 2025/03/14
                    else
                        sql += "from recordin left join recordout on recordin.no = recordout.no left join member on recordin.id = member.cardid"; //Mac 2016/05/21

                    sql += " WHERE dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and recordin.notshow = 'N'";

                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND recordout.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }
                    if (Configs.Reports.ReportSearchMemberGroup)
                    {
                        if (memberType != Constants.TextBased.All)
                            sql += " and member.memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND recordin.cartype != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND recordin.cartype =" + carTypeId;
                    }
                    else if (Configs.Member2Cartype)
                    {
                        if (memberType == Constants.TextBased.All)
                        {
                            if (carType != Constants.TextBased.All)
                                sql += $" AND (recordin.cartype = {carTypeId} OR member.typeid = {carTypeId})";
                        }
                        else if (memberType == Constants.TextBased.Visitor)
                        {
                            sql += " AND recordin.cartype != 200";

                            if (carType != Constants.TextBased.All)
                                sql += $" AND recordin.cartype = {carTypeId}";
                        }
                        else if (memberType == Constants.TextBased.Member)
                        {
                            sql += " AND recordin.cartype = 200";
                            if (carType != Constants.TextBased.All)
                                sql += $" AND member.typeid = {carTypeId}";
                        }
                        else
                        {
                            sql += $" AND member.memgroupid = {AppGlobalVariables.MemberGroupsToId[memberType]}";
                            if (carType != Constants.TextBased.All)
                                sql += $" AND member.typeid = {carTypeId}";
                        }
                    }
                    else
                    {
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND recordin.cartype != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND recordin.typeid =" + carTypeId;
                        if (carType == Constants.TextBased.All)
                        {
                            if (memberType != Constants.TextBased.All)
                                sql += " AND member.typeid =" + AppGlobalVariables.CarTypesById[memberTypeId];
                        }
                    }
                    if (promotionName != Constants.TextBased.All)
                        sql += " AND recordout.proid =" + promotionId;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND recordin.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND recordin.id = " + cardId;
                    if (guardhouse != String.Empty)
                        sql += " and recordout.guardhouse = '" + guardhouse + "' ";

                    sql += " GROUP BY recordout.proid ORDER BY recordout.proid";
                    break;
                case 20:
                case 21:
                    sql = GetPromotionUsage();
                    break;
                case 22:
                    sql = "SELECT CAST(member.cardid AS char) AS หมายเลขบัตร, member.name AS 'ชื่อ - นามสกุล', (SELECT groupname FROM membergroup WHERE id = member.memgroupid) AS กลุ่มสมาชิก, ";
                    sql += " member.license AS ทะเบียนรถ, member.tel AS เบอร์โทรศัพท์,";
                    if (Configs.Reports.UseReport23_1)
                        sql += " member.memkey as รหัสสมาชิก,";
                    sql += " date_format(member.datestart, '%d/%m/%Y %H:%i:%s') AS วันที่สมัคร, ";
                    sql += " date_format(member.dateexprie, '%d/%m/%Y %H:%i:%s') AS วันหมดอายุ, member.enable FROM member ";

                    sql += " left join cardmf t1 on member.cardid = t1.name";
                    sql += " left join cardpx t2 on member.cardid = t2.name";

                    sql += " WHERE member.license LIKE '%" + licensePlate + "%' "; //Mac 2018/12/21

                    if (nameOnCard.Length > 0)
                        sql += " and (t1.name_on_card like '%" + nameOnCard + "%' or t2.name_on_card like '%" + nameOnCard + "%')";

                    if (Configs.Reports.ReportSearchMemberGroup) //Mac 2021/06/23
                    {
                        if (memberType != Constants.TextBased.All)
                            sql += " and member.memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND member.typeid != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND member.typeid =" + carTypeId;
                    }
                    else
                    {
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND member.typeid != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND member.typeid =" + carTypeId;
                        if (carType == Constants.TextBased.All)
                        {
                            if (memberType != Constants.TextBased.All)
                                sql += " AND member.typeid =" + AppGlobalVariables.CarTypesById.First(kvp => kvp.Value == memberType).Key;
                        }
                    }

                    if (isRegistrationDateChecked)
                        sql += " and datestart BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    else if (isExpirationDateChecked)
                        sql += " and dateexprie BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    sql += " ORDER BY member.name";

                    break;
                case 23:
                    sql = "SELECT id AS ลำดับ";
                    if (Configs.Reports.UseReport24_2) //Mac 2021/07/22
                    {
                        sql += ", case when status = 'A' then 'สมัครใหม่' when status = 'C' then 'ยกเลิก'";
                        sql += " when status = 'E' then 'อัพเดต/ต่ออายุ' when status = 'B' then 'ระงับ'  when status = 'CB' then 'ยกเลิกระงับ' else 'Unknow' end as 'ประเภท'";
                        sql += ", (SELECT name FROM paytype WHERE id = paytypeid) as 'ประเภทการชำระ'";
                    }
                    sql += ", name AS 'ชื่อ - นามสกุล', license AS ทะเบียนรถ";
                    sql += ", case when printno = 0 then '' else"; //Mac 2019/05/14
                    if (Configs.UseReceiptFor1Mem) //Mac 2021/07/08
                        sql += " concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6,'0'))) end AS 'เลขที่ใบเสร็จ'";
                    else
                        sql += " CONCAT('IM', DATE_FORMAT(datepay, '%y'), LPAD(printno, 6, '0')) end AS 'เลขที่ใบเสร็จ'";

                    if (Configs.Reports.UseReport24_2) //Mac 2021/07/22
                    {
                        sql += ", (SELECT typename FROM cartype WHERE typeid = cartype) as 'ประเภทสมาชิก'";
                        sql += ", (SELECT groupname FROM membergroup WHERE id = memgroupid) as 'กลุ่มสมาชิก'";
                    }
                    sql += " , date_format(datepay, '%d/%m/%Y %H:%i:%s') AS วันที่ชำระ, date_format(dateexpire, '%d/%m/%Y %H:%i:%s') AS วันหมดอายุ";
                    if (Configs.Reports.UseReport24_1)
                    {
                        sql += ", format(price - ROUND(price*7/107, 6), 2) as 'ยอดก่อน VAT'";
                        sql += ", format(ROUND(price*7/107, 6), 2) as 'VAT'";
                    }
                    sql += ", price AS รายได้, discount as ส่วนลด ";
                    if (Configs.Reports.UseReport24_3) //Mac 2021/10/20
                        sql += ", excess_money as ยอดเงินเกิน";

                    sql += ", (SELECT name FROM user WHERE id = user) AS เจ้าหน้าที่";
                    sql += ", enable";
                    sql += " FROM member_record ";

                    sql += " WHERE datepay BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (user != Constants.TextBased.All)
                        sql += " AND user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND license LIKE '%" + licensePlate + "%'";

                    if (Configs.Reports.UseReport24_2)
                    {
                        if (memberProcessState == Constants.TextBased.CreateNewMemberProcessState)
                            sql += " and status = 'A'";
                        else if (memberProcessState == Constants.TextBased.UpdateMemberProcessState)
                            sql += " and status = 'E'";

                        if (memberType != Constants.TextBased.All)
                            sql += " and memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];

                        if (carType == Constants.TextBased.Visitor)
                            sql += " and cartype != 200";
                        else if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " and cartype =" + carTypeId;
                    }

                    sql += " ORDER BY id";

                    break;
                case 24:
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    sql = "DROP PROCEDURE IF EXISTS dowhile; "
                        + " CREATE PROCEDURE dowhile(IN date_select DATE) "
                        + " BEGIN "
                        + "   DECLARE i INT DEFAULT 0; "
                        + "   CREATE TABLE perHour (hours varchar(30),inVisitor INT(1),inMember INT(1), outVisitor INT(1), outMember INT(1)); "
                        + "   WHILE i < 24 DO "
                        + "     INSERT INTO perHour VALUES ( CONCAT(DATE_FORMAT(MAKETIME(i,0,0),'%H:%i'),' - ',DATE_FORMAT(MAKETIME(i,59,0),'%H:%i')), "
                        + "     (SELECT COUNT(no)FROM recordin WHERE HOUR(datein) = i AND datein LIKE CONCAT(date_select,'%') AND cartype < 200 ), "
                        + "     (SELECT COUNT(no)FROM recordin WHERE HOUR(datein) = i AND datein LIKE CONCAT(date_select,'%') AND cartype = 200 ), "
                        + "     (SELECT COUNT(ro.no)FROM recordout as ro JOIN recordin as ri ON ri.no = ro.no  WHERE HOUR(dateout) = i AND dateout LIKE CONCAT(date_select,'%') AND ri.cartype  < 200), "
                        + "     (SELECT COUNT(ro.no)FROM recordout as ro JOIN recordin as ri ON ri.no = ro.no  WHERE HOUR(dateout) = i AND dateout LIKE CONCAT(date_select,'%') AND ri.cartype = 200)); "
                        + "     SET i = i + 1; "
                        + "   END WHILE; "
                        + "   SELECT hours as ช่วงเวลา, inVisitor as 'ลูกค้าทั่วไป(เข้า)', inMember as 'สมาชิก(เข้า)', outVisitor as 'ลูกค้าทั่วไป(ออก)', outMember as 'สมาชิก(ออก)' FROM perHour; "
                        + " END; "

                        + " DROP TABLE IF EXISTS perHour; "
                        + " CALL dowhile('" + startDateTimeText + "');"
                        + " DROP TABLE IF EXISTS perHour; ";
                    break;
                case 25:
                    sql = "DROP PROCEDURE IF EXISTS dowhile2; "
                    + " CREATE PROCEDURE dowhile2(IN date_select DATETIME, IN date_finish DATETIME) "
                    + " BEGIN "
                    + "   DECLARE num INT DEFAULT 0; "
                    + "   CREATE TABLE perHour (hours varchar(30),inVisitor INT(1),inMember INT(1), outVisitor INT(1), outMember INT(1)); "
                    + "   WHILE num < 24 DO "
                    + "     INSERT INTO perHour VALUES ( CONCAT(DATE_FORMAT(MAKETIME(num,0,0),'%H:%i'),' - ',DATE_FORMAT(MAKETIME(num,59,0),'%H:%i')), ";
                    if (Configs.UseSettingNewMember && memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/03
                    {
                        sql += " (SELECT COUNT(t1.no)FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE HOUR(t1.datein) = num AND t1.datein BETWEEN date_select AND date_finish AND t1.cartype < 200";
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), ";
                        sql += " (SELECT COUNT(t1.no)FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE HOUR(t1.datein) = num AND t1.datein BETWEEN date_select AND date_finish AND t1.cartype = 200";
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), ";
                        sql += " (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype < 200";
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), ";
                        sql += " (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype = 200";
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += ")); ";
                    }
                    else
                    {
                        sql += "     (SELECT COUNT(no)FROM recordin WHERE HOUR(datein) = num AND datein BETWEEN date_select AND date_finish AND cartype < 200), "
                        + "     (SELECT COUNT(no)FROM recordin WHERE HOUR(datein) = num AND datein BETWEEN date_select AND date_finish AND cartype = 200), "
                        + "     (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype < 200), "
                        + "     (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype = 200)); ";
                    }
                    sql += "     SET num = num + 1; "
                    + "   END WHILE; "
                    + "   SELECT hours as ชั่วโมง, inVisitor as ลูกค้าทั่วไปเข้า, inMember as สมาชิกเข้า, outVisitor as ลูกค้าทั่วไปออก, outMember as สมาชิกออก FROM perHour; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perHour; "
                    + " CALL dowhile2('" + startDateTimeText + "','" + endDateTimeText + "');";
                    break;
                case 26:
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");
                    sql = "DROP PROCEDURE IF EXISTS dowhile3; "
                    + " CREATE PROCEDURE dowhile3(IN date_select DATE, IN date_finish DATE) "
                    + " BEGIN "
                    + "   CREATE TABLE perDay (days varchar(30),inVisitor INT(1),inMember INT(1), outVisitor INT(1), outMember INT(1)); "
                    + "   WHILE DATE(date_select) <= DATE(date_finish) DO "
                    + "     INSERT INTO perDay VALUES(date_select, ";

                    if (Configs.UseSettingNewMember && (memberGroupMonth != Constants.TextBased.All))
                    {
                        sql += " (SELECT count(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE t1.datein LIKE CONCAT(date_select,'%') AND t1.cartype < 200";
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), ";
                        sql += " (SELECT count(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE t1.datein LIKE CONCAT(date_select,'%') AND t1.cartype = 200";
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), ";
                        sql += " (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype < 200";
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), ";
                        sql += " (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype = 200";
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += ")); ";
                    }
                    else
                    {
                        sql += "     (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND cartype < 200), "
                        + "     (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND cartype = 200), "
                        + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype < 200), "
                        + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype = 200)); ";
                    }
                    sql += "     SET date_select = DATE_ADD(date_select,INTERVAL 1 DAY); "
                    + "   END WHILE; "
                    //+ "   SELECT days as วันที่ ,carin as รถเข้า ,carout as รถออก FROM perDay; "
                    + "   SELECT days as วันที่, inVisitor as ลูกค้าทั่วไปเข้า, inMember as สมาชิกเข้า, outVisitor as ลูกค้าทั่วไปออก, outMember as สมาชิกออก FROM perDay; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perDay; "
                    + " CALL dowhile3('" + startDateTimeText + "','" + endDateTimeText + "');";
                    break;
                case 27:
                    sql = "select id as รหัส, name as รายการ,(SELECT count(no) "
                    + "FROM recordout WHERE proid = promotion.id AND recordout.dateout "
                    + " BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "') "
                    + " as จำนวนคูปอง from promotion;";
                    break;
                case 28:
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");
                    sql = "DROP PROCEDURE IF EXISTS dowhile4;  "
                    + " CREATE PROCEDURE dowhile4(IN date_select DATE, IN date_finish DATE)   "
                    + " BEGIN   "
                    + "   CREATE TABLE perDay (days varchar(30),carin1 INT(1),carin2 INT(1), carin3 INT(1),carout1 INT(1), carout2 INT(1), carout3 INT(1)); "
                    + "     WHILE DATE(date_select) <= DATE(date_finish) DO      "
                    + "      INSERT INTO perDay VALUES(date_select,   "
                    + "       (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND guardhouse = (SELECT guardhouse FROM recordin GROUP BY guardhouse ORDER BY guardhouse limit 1)),    "
                    + "       (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND guardhouse = (SELECT guardhouse FROM (SELECT guardhouse FROM recordin GROUP BY guardhouse ORDER BY guardhouse limit 2) t1 ORDER BY guardhouse DESC limit 1)), "
                    + "       (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND guardhouse = (SELECT guardhouse FROM (SELECT guardhouse FROM recordin GROUP BY guardhouse ORDER BY guardhouse limit 3) t1 ORDER BY guardhouse DESC limit 1)), "
                    + "       (SELECT count(no) FROM recordout WHERE dateout LIKE CONCAT(date_select,'%') AND guardhouse = (SELECT guardhouse FROM recordout GROUP BY guardhouse ORDER BY guardhouse limit 1)), "
                    + "       (SELECT count(no) FROM recordout WHERE dateout LIKE CONCAT(date_select,'%') AND guardhouse = (SELECT guardhouse FROM (SELECT guardhouse FROM recordout GROUP BY guardhouse ORDER BY guardhouse limit 2) t2 ORDER BY guardhouse DESC limit 1)), "
                    + "       (SELECT count(no) FROM recordout WHERE dateout LIKE CONCAT(date_select,'%') AND guardhouse = (SELECT guardhouse FROM (SELECT guardhouse FROM recordout GROUP BY guardhouse ORDER BY guardhouse limit 3) t2 ORDER BY guardhouse DESC limit 1)));   "
                    + "       SET date_select = DATE_ADD(date_select,INTERVAL 1 DAY);  "
                    + "      END WHILE;    "
                    + "      SELECT days as วันที่ ,carin1 as รถเข้า1, carin2  as รถเข้า2, carin3 as รถเข้า3, carout1 as รถออก1, carout2 as รถออก2, carout3 as รถออก3 "
                    + "      FROM perDay; "
                    + " END;  "

                    + " DROP TABLE IF EXISTS perDay; "
                    + " DROP TABLE IF EXISTS guardhousein;  "
                    + " DROP TABLE IF EXISTS guardhouseout;  "
                    + " CALL dowhile4('" + startDateTimeText + "','" + endDateTimeText + "');";
                    break;
                case 29:
                    sql = "DROP PROCEDURE IF EXISTS dowhile4; "
                    + " CREATE PROCEDURE dowhile4(IN date_select DATETIME, IN date_finish DATE) "
                    + " BEGIN "
                     + "  CREATE TABLE perDay (days varchar(50),caroutVisitor INT(1),caroutMember INT(1),lostPro INT(1),price INT(1)); "
                     + "  WHILE DATE(date_select) <= DATE(date_finish) DO "
                     + "    INSERT INTO perDay VALUES(CONCAT(DATE_FORMAT(date_select,'%d-%m-%Y'),' ถึง ',DATE_FORMAT( DATE_SUB(DATE_ADD(date_select,INTERVAL 1 DAY),INTERVAL 1 SECOND),'%d-%m-%Y')), "
                     + "    (SELECT count(recordout.no) FROM recordout JOIN recordin ON recordin.no = recordout.no "
                     + "      WHERE dateout BETWEEN date_select AND  "
                     + "      DATE_SUB(DATE_ADD(date_select,INTERVAL 1 DAY),INTERVAL 1 SECOND)  "
                     + "      AND recordin.cartype < 200  ), "

                     + "    (SELECT count(recordout.no) FROM recordout JOIN recordin ON recordin.no = recordout.no "
                     + "      WHERE dateout BETWEEN date_select AND  "
                     + "      DATE_SUB(DATE_ADD(date_select,INTERVAL 1 DAY),INTERVAL 1 SECOND)  "
                     + "      AND recordin.cartype = 200), "

                    + "       (SELECT count(no) FROM recordout WHERE proid = 0  "
                    + "       AND dateout BETWEEN date_select AND  "
                    + "       DATE_SUB(DATE_ADD(date_select,INTERVAL 1 DAY),INTERVAL 1 SECOND)), "

                    + "      (SELECT SUM(price) FROM recordout WHERE "
                    + "       dateout BETWEEN date_select AND  "
                    + "       DATE_SUB(DATE_ADD(date_select,INTERVAL 1 DAY),INTERVAL 1 SECOND))); "

                    + "     SET date_select = DATE_ADD(date_select,INTERVAL 1 DAY); "
                    + "   END WHILE; "
                    + "   SELECT days as วันที่ ,caroutVisitor as ผู้มาติดต่อ, caroutMember as สมาชิก, lostPro as ไม่ได้ประทับตรา,  "
                    + "          CASE WHEN price IS NOT NULL  "
                    + "               THEN price "
                    + "               ELSE 0 "
                    + "          END as  รายได้"
                    + "   FROM perDay; "
                    + " END; "

                    + " DROP TABLE IF EXISTS perDay; "
                    + " CALL dowhile4('" + startDateTimeText + "','" + endDateTimeText + "');";
                    break;
                case 30:
                    if (Configs.UseMemberType) //Mac 2018/01/16
                    {
                        sql = "SELECT t1.no AS ลำดับ, case when t1.cartype = 200 then ifnull((SELECT typename FROM cartype WHERE typeid = t3.typeid), 'Member') else "; //Mac 2022/03/02
                        if (Configs.Reports.ReportCartypeFree15Min) //Mac 2018/01/16
                        {
                            sql += " case when TIMESTAMPDIFF(second,t1.datein,t2.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                            sql += " (SELECT typename FROM cartype WHERE typeid = t1.cartype) end end AS ประเภท ";
                        }
                        else
                            sql += " (SELECT typename FROM cartype WHERE typeid = t1.cartype) end AS ประเภท ";
                    }
                    else
                    {
                        sql = "SELECT t1.no AS ลำดับ, ";
                        if (Configs.Reports.ReportCartypeFree15Min) //Mac 2018/01/16
                        {
                            sql += " case when (t1.cartype != 200) and TIMESTAMPDIFF(second,t1.datein,t2.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                            sql += "  (SELECT typename FROM cartype WHERE typeid = t1.cartype) end AS ประเภท  ";
                        }
                        else
                            sql += " (SELECT typename FROM cartype WHERE typeid = t1.cartype) AS ประเภท ";
                    }

                    sql += ", case when t1.license = 'NO' then t1.id when t1.license = '' then t1.id else t1.license end AS ทะเบียน ";

                    if (Configs.UseNameOnCard) //Mac 2020/12/16
                    {
                        sql += ", cast(ifnull((select name_on_card from cardpx where cardpx.name = t1.id), (select name_on_card from cardmf where cardmf.name = t1.id)) as char)  as 'เลขที่บัตร'";
                    }

                    if (Configs.NoPanelUp2U == "2") //Mac 2017/03/13
                    {
                        sql = "select t1.no as ลำดับ,(select typename from cartype where typeid = t1.cartype) as ประเภท,t1.license as ทะเบียน";
                        sql += ",t1.id as หมายเลขบัตร,(select memid from member_up2u where cardid = t1.id) as เลขสมาชิก";
                        sql += ",(select name from member_up2u where cardid = t1.id) as ชื่อสมาชิก,(select date_format(dateexpire, '%d/%m/%Y %H:%i:%s') from member_up2u where cardid = t1.id) as วันหมดอายุ"; //Mac 2018/12/21
                    }
                    sql += " , date_format(t1.datein, '%d/%m/%Y %H:%i:%s') AS เวลาเข้า, (SELECT name FROM user WHERE id = t1.userin) AS เจ้าหน้าที่ขาเข้า, date_format(t2.dateout, '%d/%m/%Y %H:%i:%s') AS เวลาเคลียร์บัตร "; //Mac 2018/12/21
                    sql += " , (SELECT name FROM user WHERE id = t2.userout) AS เจ้าหน้าที่เคลียร์บัตร, t2.clearcard AS เหตุผล ";
                    sql += " FROM recordin t1 LEFT JOIN recordout t2 ON t1.no = t2.no ";

                    if (Configs.UseMemberLicensePlate) //Mac 2018/09/03
                        sql += " LEFT JOIN member t3 ON t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " LEFT JOIN member t3 ON t1.id = t3.cardid";

                    sql += " left join cardmf t4 on t1.id = t4.name";
                    sql += " left join cardpx t5 on t1.id = t5.name";

                    sql += " WHERE dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "' AND char_length(trim(t2.clearcard)) > 0 ";

                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and t1.notshow = 'N'";

                    if (nameOnCard.Trim().Length > 0) //Mac 2022/03/01
                        sql += " and (t4.name_on_card like '%" + nameOnCard + "%' or t5.name_on_card like '%" + nameOnCard + "%')";

                    if (user != Constants.TextBased.All)
                        sql += " AND t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (Configs.Reports.ReportSearchMemberGroup) //Mac 2021/03/11
                    {
                        if (memberType != Constants.TextBased.All)
                            sql += " and t3.memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND t1.cartype != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND t1.cartype =" + carTypeId;
                    }
                    else if (Configs.Member2Cartype) //Mac 2016/05/03
                    {
                        if (memberType == Constants.TextBased.All)
                        {
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND (t1.cartype =" + carTypeId + " or t3.typeid =" + carTypeId + ")";
                            }
                        }
                        else if (memberType == Constants.TextBased.Visitor)
                        {
                            sql += " AND t1.cartype != 200";
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND t1.cartype =" + carTypeId;
                            }
                        }
                        else if (memberType == Constants.TextBased.Member)
                        {
                            sql += " AND t1.cartype = 200";
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND t3.typeid =" + carTypeId;
                            }
                        }
                        else
                        {
                            //sql += " AND t1.cartype = 200";
                            sql += " AND t3.memgroupid =" + AppGlobalVariables.MemberGroupsToId[memberType];
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND t3.typeid =" + carTypeId;
                            }
                        }
                    }
                    else
                    {
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND t1.cartype != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND t1.typeid =" + carTypeId;
                        if (carType == Constants.TextBased.All) //Mac 2015/02/10
                        {
                            if (memberType != Constants.TextBased.All)
                                sql += " AND t3.typeid =" + AppGlobalVariables.CarTypesById.First(kvp => kvp.Value == memberType).Key;
                        }
                    }

                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t1.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t1.id = " + cardId;
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";

                    if (Configs.UseSettingNewMember && memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/06
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " ORDER BY t1.no";

                    break;
                case 31:
                case 93:
                    sql = "SELECT recordin.no as ลำดับ,";
                    if (Configs.UseMemberType) //Mac 2018/01/16
                        sql += " case when recordin.cartype = 200 then ifnull((SELECT typename FROM cartype WHERE typeid = member.typeid), 'Member') else (SELECT typename FROM cartype WHERE typeid = recordin.cartype) end AS ประเภท,"; //Mac 2022/03/02
                                                                                                                                                                                                                                          //sql += " case when recordin.cartype = 200 then (SELECT typename FROM cartype WHERE typeid = member.typeid) else (SELECT typename FROM cartype WHERE typeid = recordin.cartype) end AS ประเภท,";
                    else
                        sql += "(select typename from cartype where typeid = recordin.cartype) as ประเภท,";

                    sql += "case when recordin.license = 'NO' then recordin.id when recordin.license = '' then recordin.id else recordin.license end as ทะเบียน,";
                    if (Configs.NoPanelUp2U == "2") //Mac 2017/03/13
                    {
                        sql = "select recordin.no as ลำดับ,(select typename from cartype where typeid = recordin.cartype) as ประเภท,recordin.license as ทะเบียน";
                        sql += ",recordin.id as หมายเลขบัตร,(select memid from member_up2u where cardid = recordin.id) as เลขสมาชิก";
                        sql += ",(select name from member_up2u where cardid = recordin.id) as ชื่อสมาชิก,(select date_format(dateexpire, '%d/%m/%Y %H:%i:%s') from member_up2u where cardid = recordin.id) as วันหมดอายุ,"; //Mac 2018/12/21
                    }
                    sql += "date_format(recordin.datein, '%d/%m/%Y %H:%i:%s') as เวลาเข้า,(select name from user where id = recordin.userin) as เจ้าหน้าที่ขาเข้า"; //Mac 2018/12/21
                    sql += " ,recordin.picdiv, recordin.piclic";
                    if (Configs.UseAsciiMember) //Mac 2016/07/11
                        sql += ",cast(CONCAT(CHAR(left(recordin.id,2)),mid(recordin.id,3)) as char) as รหัสบัตร";

                    if (Configs.UseNameOnCard) //Mac 2018/12/13
                        sql += ", IFNULL(t1.name_on_card,t2.name_on_card) as 'ชื่อบัตร'"; //Mac 2022/03/02
                                                                                          //sql += ", IFNULL(cardpx.name_on_card,cardmf.name_on_card) as 'ชื่อบัตร'";
                    else if (Configs.Reports.UseReport5_4) //Mac 2019/05/16
                        sql += ", cast(recordin.id as char) as เลขที่บัตร";

                    sql += " FROM recordin recordin left join recordout ON recordin.no = recordout.no";

                    if (Configs.UseMemberLicensePlate) //Mac 2018/09/03
                        sql += " LEFT JOIN member ON member.license like concat('%',recordin.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " LEFT JOIN member ON recordin.id = member.cardid";

                    sql += " left join cardmf t1 on recordin.id = t1.name";
                    sql += " left join cardpx t2 on recordin.id = t2.name";

                    sql += " WHERE datein BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " AND recordout.no IS NULL";

                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and recordin.notshow = 'N'";

                    if (nameOnCard.Trim().Length > 0) //Mac 2022/03/01
                        sql += " and (t1.name_on_card like '%" + nameOnCard + "%' or t2.name_on_card like '%" + nameOnCard + "%')";

                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND recordin.userin =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key + " OR recordout.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }

                    if (Configs.Reports.ReportSearchMemberGroup) //Mac 2021/03/11
                    {
                        if (memberType != Constants.TextBased.All)
                            sql += " and member.memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND recordin.cartype != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND recordin.typeid =" + carTypeId;
                    }
                    else if (Configs.Member2Cartype) //Mac 2016/05/03
                    {
                        if (memberType == Constants.TextBased.All)
                        {
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND (recordin.typeid =" + carTypeId + " or member.typeid =" + carTypeId + ")";
                            }
                        }
                        else if (memberType == Constants.TextBased.Visitor)
                        {
                            sql += " AND recordin.cartype != 200";
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND recordin.typeid =" + carTypeId;
                            }
                        }
                        else if (memberType == Constants.TextBased.Member)
                        {
                            sql += " AND recordin.cartype = 200";
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND member.typeid =" + carTypeId;
                            }
                        }
                        else
                        {
                            sql += " AND member.memgroupid =" + AppGlobalVariables.MemberGroupsToId[memberType];
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND member.typeid =" + carTypeId;
                            }
                        }
                    }
                    else
                    {
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND recordin.typeid =" + carTypeId;
                        if (carType == Constants.TextBased.Visitor) //Mac 2015/02/10
                            sql += " AND recordin.cartype != 200";
                        if (carType == Constants.TextBased.All) //Mac 2015/02/10
                        {
                            if (memberType != Constants.TextBased.All)
                                sql += " AND member.typeid =" + AppGlobalVariables.CarTypesById.First(kvp => kvp.Value == memberType).Key;
                        }
                    }

                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND recordin.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND recordin.id = " + cardId;
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and recordin.guardhouse = '" + guardhouse + "' ";

                    if (selectedReportId == 93) //Mac 2020/10/26
                        sql += " and recordin.no mod 2 = 1";

                    sql += " ORDER BY recordin.no";

                    break;

                case 32:
                    string fontSlip32 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip32 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName)
                            fontSlip32 = "IV";
                    }

                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0)
                                sql = "select concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno_second,6,'0'))) as เลขที่ใบกำกับภาษี";
                            else
                                sql = "select concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,6,'0'))) as เลขที่ใบกำกับภาษี";
                        }
                        else
                        {
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0)
                                sql = "select concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(t2.printno_second,6,'0'))) as เลขที่ใบกำกับภาษี";
                            else
                                sql = "select concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(t2.printno,6,'0'))) as เลขที่ใบกำกับภาษี";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0)
                                sql = "select concat('" + fontSlip32 + "', concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno_second,6,'0'))) as เลขที่ใบกำกับภาษี";
                            else
                                sql = "select concat('" + fontSlip32 + "', concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,6,'0'))) as เลขที่ใบกำกับภาษี";
                        }
                        else
                        {
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0)
                                sql = "select concat('" + fontSlip32 + "', concat(date_format(t2.dateout,'%y') ,lpad(t2.printno_second,6,'0'))) as เลขที่ใบกำกับภาษี";
                            else
                                sql = "select concat('" + fontSlip32 + "', concat(date_format(t2.dateout,'%y') ,lpad(t2.printno,6,'0'))) as เลขที่ใบกำกับภาษี";
                        }
                    }

                    sql += ", cast(case when t3.cartype = 200 and t4.id is not null then t4.id else t3.no end as char) as 'หมายเลขบัตร'";
                    sql += ", (select typename from cartype where typeid = t3.cartype) AS ประเภท";
                    sql += ", t3.license as ทะเบียน";
                    sql += ", concat(date_format(t3.datein,'%d/%m/'), date_format(t3.datein,'%Y'), date_format(t3.datein,' %H:%i:%s')) as เวลาเข้า";
                    sql += ", (select name from user where id = t2.userout) AS เจ้าหน้าที่ขาออก";
                    sql += ", concat(date_format(t2.dateout,'%d/%m/'), date_format(t2.dateout,'%Y'), date_format(t2.dateout,' %H:%i:%s')) as เวลาออก";
                    sql += ", t1.name as 'ผู้ทำรายการ Void'";
                    sql += ", format(t2.price, 2) as จำนวนเงิน";
                    sql += ", t1.remark as 'หมายเหตุ'";

                    if (Configs.UseFormVoidSlip)
                        sql += " from voidslip t1 left join recordoutvoidslip t2 on t1.no = t2.no left join recordin t3 on t2.no = t3.no";
                    else
                        sql += " from voidslip t1 left join recordout t2 on t1.no = t2.no left join recordin t3 on t2.no = t3.no";

                    sql += " left join member t4 on t3.cartype = 200 and t3.license = t4.license";

                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0)
                        sql += " and t3.notshow = 'N'";

                    sql += " order by t1.id";
                    break;

                case 33:
                    sql = "select concat(date_format(t2.dateout,'%d/%m/'), date_format(t2.dateout,'%Y') + 543) as วันที่";
                    sql += " , t2.guardhouse as จุดที่ออก, t2.posid as เลขที่อนุญาต";
                    if (Configs.UseReceiptFor1Out) //Mac 2019/02/15
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(min(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีเริ่มต้น";
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(max(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีสิ้นสุด";
                        }
                        else
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(min(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีเริ่มต้น";
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(max(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีสิ้นสุด";
                        }
                        sql += " , format(cast(count(t2.no) as CHAR(50)),0) as จำนวนใบ";
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m') ,lpad(min(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีเริ่มต้น";
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m') ,lpad(max(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีสิ้นสุด";
                        }
                        else
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y') ,lpad(min(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีเริ่มต้น";
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y') ,lpad(max(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีสิ้นสุด";
                        }
                        sql += " , format(cast(count(t2.no) as CHAR(50)),0) as จำนวนใบ";
                    }

                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2018/12/10
                    {
                        sql += " , format(sum((t2.price-t2.losscard)) - ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ค่าบริการ";
                        sql += " , format(ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as VAT";
                        sql += " , format(sum((t2.price-t2.losscard)), 2) as รวมเงิน";
                    }
                    else
                    {
                        sql += " , format(sum(t2.price) - ROUND(sum(t2.price)*7/107, 6), 2) as ค่าบริการ";
                        sql += " , format(ROUND(sum(t2.price)*7/107, 6), 2) as VAT";
                        sql += " , format(sum(t2.price), 2) as รวมเงิน";
                    }
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    sql += " where date_format(t2.dateout,'%Y-%m-%d') = '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    if (Configs.UseReceiptFor1Out) //Mac 2019/02/15
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d'), t2.guardhouse, t2.posid, t2.receipt";
                        sql += " order by t2.guardhouse, t2.receipt";
                    }
                    else
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d'), t2.guardhouse, t2.posid";
                        sql += " order by t2.guardhouse";
                    }
                    break;

                case 34:
                    sql = "select concat(date_format(t2.dateout,'%d/%m/'), date_format(t2.dateout,'%Y') + 543) as วันที่";
                    if (Configs.UseReceiptFor1Out) //Mac 2019/02/15
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(min(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีเริ่มต้น";
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(max(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีสิ้นสุด";
                        }
                        else
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(min(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีเริ่มต้น";
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(max(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีสิ้นสุด";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m') ,lpad(min(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีเริ่มต้น";
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m') ,lpad(max(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีสิ้นสุด";
                        }
                        else
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y') ,lpad(min(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีเริ่มต้น";
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y') ,lpad(max(t2.printno),6,'0'))) as เลขที่ใบกำกับภาษีสิ้นสุด";
                        }
                    }

                    sql += " , format(cast(count(t2.no) as CHAR(50)),0) as จำนวนใบ";
                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2018/12/10
                    {
                        sql += " , format(sum((t2.price-t2.losscard)) - ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ค่าบริการ";
                        sql += " , format(ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as VAT";
                        sql += " , format(sum((t2.price-t2.losscard)), 2) as รวมเงิน";
                    }
                    else
                    {
                        sql += " , format(sum(t2.price) - ROUND(sum(t2.price)*7/107, 6), 2) as ค่าบริการ";
                        sql += " , format(ROUND(sum(t2.price)*7/107, 6), 2) as VAT";
                        sql += " , format(sum(t2.price), 2) as รวมเงิน";
                    }

                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    if (Configs.UseReceiptFor1Out) //Mac 2019/02/15
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d'), t2.receipt";
                        sql += " order by date_format(t2.dateout,'%Y-%m-%d'), t2.receipt";
                    }
                    else
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d')";
                        sql += " order by date_format(t2.dateout,'%Y-%m-%d')";
                    }
                    break;

                case 35:
                    sql = "select concat(date_format(t2.dateout,'%d/%m/'), date_format(t2.dateout,'%Y') + 543) as วันที่";

                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2019/08/27
                    {
                        sql += " , format((sum(t2.price-t2.losscard) - ROUND(sum(t2.price-t2.losscard)*7/107, 2)) - (sum(t2.overdate) - ROUND(sum(t2.overdate)*7/107, 2)), 2) as ค่าจอดรถ";
                        sql += " , format(sum(t2.losscard), 2) as ค่าปรับบัตรหาย";
                        sql += " , format(sum(t2.overdate) - ROUND(sum(t2.overdate)*7/107, 6), 2) as ค่าปรับค้างคืน";
                        sql += " , format(sum((t2.price-t2.losscard)) - ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ค่าบริการ";
                        sql += " , format(ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as VAT";
                        sql += " , format(sum((t2.price-t2.losscard)), 2) as รวมเงิน";
                    }
                    else
                    {
                        sql += " , format((sum(t2.price) - ROUND(sum(t2.price)*7/107, 2)) - ((sum(t2.losscard) - ROUND(sum(t2.losscard)*7/107, 2)) + (sum(t2.overdate) - ROUND(sum(t2.overdate)*7/107, 2))), 2) as ค่าจอดรถ";
                        sql += " , format(sum(t2.losscard) - ROUND(sum(t2.losscard)*7/107, 6), 2) as ค่าปรับบัตรหาย";
                        sql += " , format(sum(t2.overdate) - ROUND(sum(t2.overdate)*7/107, 6), 2) as ค่าปรับค้างคืน";
                        sql += " , format(sum(t2.price) - ROUND(sum(t2.price)*7/107, 6), 2) as ค่าบริการ";
                        sql += " , format(ROUND(sum(t2.price)*7/107, 6), 2) as VAT";
                        sql += " , format(sum(t2.price), 2) as รวมเงิน";
                    }
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    sql += " group by date_format(t2.dateout,'%Y-%m-%d')";
                    sql += " order by date_format(t2.dateout,'%Y-%m-%d')";
                    break;

                case 36:
                    sql = "DROP PROCEDURE IF EXISTS dowhile5; "
                    + " CREATE PROCEDURE dowhile5(IN date_select DATETIME, IN date_finish DATETIME) "
                    + " BEGIN "
                    + "   DECLARE num INT DEFAULT 0; "
                    + "   CREATE TABLE perHour (hours varchar(30),Visitor INT(1), Member INT(1)); "
                    + "   WHILE num < 8 DO "
                    + "     INSERT INTO perHour VALUES ( CASE WHEN num = 7 THEN CONCAT(DATE_FORMAT(MAKETIME(num,0,0),'%H:%i'),' + ') ELSE CONCAT(DATE_FORMAT(MAKETIME(num,0,0),'%H:%i'),' - ',DATE_FORMAT(MAKETIME(num,59,0),'%H:%i')) END, ";

                    if (Configs.UseSettingNewMember && memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/09
                    {
                        sql += " (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout BETWEEN date_select AND date_finish AND t2.cartype < 200 AND CASE WHEN num = 7 THEN TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) >= num ELSE TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) = num END";
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " and t2.notshow = 'N'";
                        sql += "), ";
                        sql += " (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout BETWEEN date_select AND date_finish AND t2.cartype = 200 AND CASE WHEN num = 7 THEN TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) >= num ELSE TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) = num END";
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " and t2.notshow = 'N'";
                        sql += ")); ";
                    }
                    else
                    {
                        sql += "     (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE t1.dateout BETWEEN date_select AND date_finish AND t2.cartype < 200 AND CASE WHEN num = 7 THEN TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) >= num ELSE TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) = num END";
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " and t2.notshow = 'N'";
                        sql += " ), ";
                        sql += "     (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE t1.dateout BETWEEN date_select AND date_finish AND t2.cartype = 200 AND CASE WHEN num = 7 THEN TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) >= num ELSE TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) = num END";
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " and t2.notshow = 'N'";
                        sql += " )); ";
                    }
                    sql += "     SET num = num + 1; "
                    + "   END WHILE; "
                    + "   SELECT hours as ชั่วโมง, Visitor as ลูกค้าทั่วไป, Member as สมาชิก FROM perHour; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perHour; "
                    + " CALL dowhile5('" + startDateTimeText + "','" + endDateTimeText + "');";
                    break;
                case 37:
                    sql = "select t2.no AS 'ID', date_format(t2.dateout, '%Y-%m-%d') as 'Date', date_format(t1.datein, '%H:%i') as 'Check In Time'";
                    sql += " , date_format(t2.dateout, '%H:%i') as 'Check Out Time', t1.license as 'Car License', t2.proid as 'No of Coupon', t2.price as 'Actual Payment'";
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    sql += " where (t2.proid between 1 and 9) and date_format(t2.dateout,'%Y-%m-%d') = '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + "'";
                    sql += " order by date_format(t2.dateout, '%H:%i')";
                    break;
                case 38:
                    sql = "select (select name from user where id = userout) as ชื่อพนักงาน, date(dateout) as วันที่ปฏิบัติงาน";
                    sql += ", format(sum(price) + sum(discount), 2) as 'จำนวนเรียกเก็บ(บาท)', format(sum(discount), 2) as 'ยอดส่วนลด(บาท)' ";
                    sql += ", format(sum(price) - ROUND(sum(price)*7/107, 6), 2) as 'ยอดก่อน VAT', format(ROUND(sum(price)*7/107, 6), 2) as VAT";
                    sql += ", format(sum(price), 2) as 'จำนวนที่ได้รับ(บาท)', count(no) as 'รถออก(คัน)', sum(case when price > 0 then 1 else 0 end) as 'เสียค่าบริการ(คัน)'";
                    sql += ", sum(case when proid > 0 then 1 else 0 end) as 'Estamp(ครั้ง)'";
                    sql += ", (select count(*) from liftrecord where userid = userout and date(datelift) = date(dateout) and gate = 'O' ) as 'ยกไม้ฉุกเฉิน(ครั้ง)' from recordout";
                    sql += " where date_format(dateout,'%Y-%m-%d') = '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + "'";
                    sql += " group by userout, date(dateout) order by (select name from user where id = userout)";
                    break;
                case 39:
                    sql = "select t1.no as 'ลำดับที', concat((select value from slipoutformat where name = 'receiptname'), date_format(t1.dateout,'%y'),lpad(t1.printno,6,'0')) AS 'เลขที่ใบกำกับภาษี'";
                    sql += ", format(t1.price + t1.discount, 2) as 'จำนวนเรียกเก็บ(บาท)', format(t1.discount, 2) as 'ยอดส่วนลด(บาท)'";
                    sql += ", format(t1.price - ROUND(t1.price*7/107, 6), 2) as 'ยอดก่อน VAT', format(ROUND(t1.price*7/107, 6), 2) as VAT";
                    sql += ", format(t1.price, 2) as 'จำนวนที่ได้รับ(บาท)', date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วันที่บันทึก', t3.name as 'ชื่อสมาชิก'"; //Mac 2018/12/21
                    sql += ", case when t2.license = 'NO' then t2.id else t2.license end as 'เลขทะเบียน', (select name from user where id = userout) as 'ผู้บันทึก', t2.id as 'หมายเลขบัตร'";
                    if (Configs.UseMemberLicensePlate)
                        sql += " from recordout t1 left join recordin t2 on t1.no = t2.no left join member t3 on t3.license like concat('%',t2.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " from recordout t1 left join recordin t2 on t1.no = t2.no left join member t3 on t2.id = t3.cardid";

                    sql += " where t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "' and t1.printno > 0";

                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND t1.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }

                    if (Configs.Reports.ReportSearchMemberGroup)
                    {
                        if (memberType != Constants.TextBased.All)
                            sql += " and t3.memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND t2.cartype != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND t2.typeid =" + carTypeId;
                    }
                    else if (Configs.Member2Cartype) //Mac 2016/05/03
                    {
                        if (memberType == Constants.TextBased.All)
                        {
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND (t2.typeid =" + carTypeId + " or t3.typeid =" + carTypeId + ")";
                            }
                        }
                        else if (memberType == Constants.TextBased.Visitor)
                        {
                            sql += " AND t2.cartype != 200";
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND t2.typeid =" + carTypeId;
                            }
                        }
                        else if (memberType == Constants.TextBased.Member)
                        {
                            sql += " AND t2.cartype = 200";
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND t3.typeid =" + carTypeId;
                            }
                        }
                        else
                        {
                            sql += " AND t3.memgroupid =" + AppGlobalVariables.MemberGroupsToId[memberType];
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND t3.typeid =" + carTypeId;
                            }
                        }
                    }
                    else
                    {
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND t2.cartype != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND t2.typeid =" + carTypeId;
                        if (carType == Constants.TextBased.All) //Mac 2015/02/10
                        {
                            if (memberType != Constants.TextBased.All)
                                sql += " AND t3.typeid =" + AppGlobalVariables.CarTypesById.First(kvp => kvp.Value == memberType).Key;
                        }
                    }
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t2.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t2.id = " + cardId;
                    sql += " order by t1.dateout, t1.printno";
                    break;
                case 40:
                //case 41:
                //    sql = "select no as ลำดับ, name as ชื่อ, license as ทะเบียน, picdiv, piclic, date as วันที่";
                //    sql += ", case when gate = 'I' then 'ขาเข้า' when gate = 'O' then 'ขาออก' when gate = 'B' then 'ขาเข้า/ขาออก' end as ประตู";
                //    sql += " from recordmember";
                //    sql += " WHERE date BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                //    if (!String.IsNullOrEmpty(licensePlate))
                //        sql += " AND license LIKE '%" + licensePlate + "%'";
                //    if (!String.IsNullOrEmpty(cardId))
                //        sql += " AND id = " + cardId;
                //    if (recordNumber != "")
                //        sql += " AND no_recordin = " + recordNumber;
                //    sql += " ORDER BY no";

                //    string testImage = @"D:\ImagePicture_POS\POS_User\2024\06\25\18446744073609551874_25062024_11463995458.png";
                //    break;
                case 41:
                    string testImage = @"D:/ImagePicture_POS/POS_User/2024/06/25/18446744073609551874_25062024_11463995458.png";

                    sql = "SELECT " +
                          "no AS ลำดับ, " +
                          "name AS ชื่อ, " +
                          "license AS ทะเบียน, " +
                          $"'{testImage}' AS picdiv, " +
                          $"'{testImage}' AS piclic, " +
                          "date AS วันที่, " +
                          "CASE " +
                          "   WHEN gate = 'I' THEN 'ขาเข้า' " +
                          "   WHEN gate = 'O' THEN 'ขาออก' " +
                          "   WHEN gate = 'B' THEN 'ขาเข้า/ขาออก' " +
                          "END AS ประตู " +
                          "FROM recordmember " +
                          "WHERE date BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (!string.IsNullOrEmpty(licensePlate))
                        sql += " AND license LIKE '%" + licensePlate + "%'";

                    if (!string.IsNullOrEmpty(cardId))
                        sql += " AND id = " + cardId;

                    if (!string.IsNullOrEmpty(recordNumber))
                        sql += " AND no_recordin = " + recordNumber;

                    sql += " ORDER BY no";
                    break;

                case 42:
                    sql = "select id as 'No.', company as บริษัท, staffid as รหัสพนักงาน, name as 'ชื่อ-นามสกุล', cardid as 'ID Card', stickerno as 'Sticker No.'";
                    sql += ", license as ทะเบียนรถ, cartype as ประเภทรถ, tel as เบอร์ติดต่อ from member_up2u";
                    sql += " where 1 = 1";
                    if (up2UName != "")
                        sql += " AND name LIKE '%" + up2UName + "%'";
                    if (up2UStaffId != "")
                        sql += " AND staffid LIKE '%" + up2UStaffId + "%'";
                    if (up2UStickerNumber != "")
                        sql += " AND stickerno LIKE '%" + up2UStickerNumber + "%'";
                    if (up2UCarType != "")
                        sql += " AND cartype LIKE '%" + up2UCarType + "%'";

                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND cardid = " + cardId;

                    sql += " order by id";
                    break;
                case 43:
                    sql = "select t3.stickerno as 'Sticker No.', t3.license as ทะเบียนรถ, t3.staffid as รหัสพนักงาน";
                    sql += " ,date_format(t1.datein, '%d/%m/%Y %H:%i:%s') as เวลาเข้า, date_format(t2.dateout, '%d/%m/%Y %H:%i:%s') as เวลาออก"; //Mac 2018/12/21
                    sql += " from recordin t1";
                    sql += " left join recordout t2 on t1.no = t2.no";
                    sql += " left join member_up2u t3 on t1.id = t3.cardid";
                    sql += " where t2.no is not null";
                    sql += " and t2.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (up2UName != "")
                        sql += " AND t3.name LIKE '%" + up2UName + "%'";
                    if (up2UStaffId != "")
                        sql += " AND t3.staffid LIKE '%" + up2UStaffId + "%'";
                    if (up2UStickerNumber != "")
                        sql += " AND t3.stickerno LIKE '%" + up2UStickerNumber + "%'";
                    if (up2UCarType != "")
                        sql += " AND t3.cartype LIKE '%" + up2UCarType + "%'";

                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t3.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t1.id = " + cardId;

                    sql += " order by t1.no";
                    break;

                case 44:
                    sql = "select t3.stickerno as 'Sticker No.', t3.license as ทะเบียนรถ, t3.staffid as รหัสพนักงาน";
                    sql += " ,date_format(min(t1.datein), '%d/%m/%Y %H:%i:%s') as เวลาเข้า, date_format(max(t2.dateout), '%d/%m/%Y %H:%i:%s') as เวลาออก"; //Mac 2018/12/21
                    sql += " from recordin t1";
                    sql += " left join recordout t2 on t1.no = t2.no";
                    sql += " left join member_up2u t3 on t1.id = t3.cardid";
                    sql += " where t2.no is not null";
                    sql += " and t2.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (up2UName != "")
                        sql += " AND t3.name LIKE '%" + up2UName + "%'";
                    if (up2UStaffId != "")
                        sql += " AND t3.staffid LIKE '%" + up2UStaffId + "%'";
                    if (up2UStickerNumber != "")
                        sql += " AND t3.stickerno LIKE '%" + up2UStickerNumber + "%'";
                    if (up2UCarType != "")
                        sql += " AND t3.cartype LIKE '%" + up2UCarType + "%'";

                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t3.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t1.id = " + cardId;

                    sql += " group by t1.id";
                    sql += " order by t1.no";
                    break;

                case 45: //Mac 2016/03/18
                    sql = "select t3.id as ลำดับบัตร";//Mac 2016/05/19
                    sql += " , cast(t1.id as char) as หมายเลขบัตร";
                    sql += " , t3.name as 'ชื่อ - นามสกุล'";
                    sql += " , t3.license as ทะเบียนรถ";
                    sql += " , t3.address as ที่อยู่";
                    sql += " , t3.tel as เบอร์โทรศัพท์";
                    sql += " , sum(t2.discount) as เครดิต";
                    sql += " , date_format(min(t1.datein), '%d/%m/%Y %H:%i:%s') as เข้าครั้งแรก, date_format(max(t2.dateout), '%d/%m/%Y %H:%i:%s') as ออกล่าสุด"; //Mac 2018/12/21
                    sql += " from recordin t1";
                    sql += " left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2018/09/03
                        sql += " left join member t3 on t3.license = like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid";

                    sql += " where t1.cartype = 200";
                    sql += " and t2.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (Configs.Reports.ReportSearchMemberGroup) //Mac 2021/03/11
                    {
                        if (memberType != Constants.TextBased.All)
                            sql += " and t3.memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND t1.cartype != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND t1.typeid =" + carTypeId;
                    }
                    else if (Configs.Member2Cartype) //Mac 2016/05/03
                    {
                        if (memberType == Constants.TextBased.All)
                        {
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND (t1.typeid =" + carTypeId + " or t3.typeid =" + carTypeId + ")";
                            }
                        }
                        else if (memberType == Constants.TextBased.Visitor)
                        {
                            sql += " AND t1.cartype != 200";
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND t1.typeid =" + carTypeId;
                            }
                        }
                        else if (memberType == Constants.TextBased.Member)
                        {
                            sql += " AND t1.cartype = 200";
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND t3.typeid =" + carTypeId;
                            }
                        }
                        else
                        {
                            //sql += " AND t1.cartype = 200";
                            sql += " AND t3.memgroupid =" + AppGlobalVariables.MemberGroupsToId[memberType];
                            if (carType != Constants.TextBased.All)
                            {
                                sql += " AND t3.typeid =" + carTypeId;
                            }
                        }
                    }
                    else
                    {
                        if (carType == Constants.TextBased.Visitor)
                            sql += " AND t1.cartype != 200";
                        if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                            sql += " AND t1.typeid =" + carTypeId;
                        if (carType == Constants.TextBased.All) //Mac 2015/02/10
                        {
                            if (memberType != Constants.TextBased.All)
                                sql += " AND t3.typeid =" + AppGlobalVariables.CarTypesById.First(kvp => kvp.Value == memberType).Key;
                        }
                    }
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t1.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t1.id = " + cardId;
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t2.guardhouse = '" + guardhouse + "' ";

                    sql += " group by t3.cardid, t3.name, t3.license, t3.address, t3.tel";
                    sql += " order by t3.id";
                    break;

                case 46:
                    StringBuilder strBuilder = new StringBuilder();

                    strBuilder.AppendLine("SELECT DISTINCT");
                    strBuilder.AppendLine("    mgp.groupname AS บริษัท,");

                    if (Configs.UseAsciiMember)
                        strBuilder.AppendLine("    CAST(CONCAT(CHAR(LEFT(m.cardid, 2)), MID(m.cardid, 3)) AS CHAR) AS เลขที่บัตร,");
                    else
                        strBuilder.AppendLine("    CAST(m.address AS CHAR) AS เลขที่บัตร,");

                    strBuilder.AppendLine(" m.name AS ชื่อสมาชิก,");
                    strBuilder.AppendLine(" m.license AS เลขทะเบียนรถ,");
                    strBuilder.AppendLine(" DATE_FORMAT(m.datestart, '%d/%m/%y') AS วันที่สมัคร,");
                    strBuilder.AppendLine(" DATE_FORMAT(m.dateexprie, '%d/%m/%y') AS วันที่หมดอายุ,");
                    strBuilder.AppendLine(" CASE WHEN m.memgrouppriceid_pay = 0 THEN 'N' ELSE 'Y' END AS ผู้เช่า,");
                    strBuilder.AppendLine(" m.memgrouppriceid_pay AS ค่าบัตรสมาชิก,");
                    strBuilder.AppendLine(" mgp.id AS membergroupprice_month_id");
                    strBuilder.AppendLine(" FROM member m");
                    strBuilder.AppendLine(" LEFT JOIN membergroupprice_month mgp");
                    strBuilder.AppendLine(" ON mgp.id = m.memgrouppriceid_month");
                    strBuilder.AppendLine(" WHERE 1 = 1");

                    try
                    {
                        if (memberGroupMonth != Constants.TextBased.All)
                            strBuilder.AppendLine($"AND m.memgrouppriceid_month = {AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth]}");
                    }
                    catch
                    {
                        MessageBox.Show($"ไม่มีชื่อบริษัท หรือชื่อบริษัทไม่ถูกต้อง:\r\n{memberGroupMonth}");
                    }

                    if (paymentStatus == Constants.TextBased.PaymentStatusPaid)
                        strBuilder.AppendLine("AND m.memgrouppriceid_pay > 0");

                    if (paymentStatus == Constants.TextBased.PaymentStatusUnPaid)
                        strBuilder.AppendLine("AND m.memgrouppriceid_pay = 0");

                    if (memberName != Constants.TextBased.All)
                        strBuilder.AppendLine($"AND m.name = '{memberName}'");

                    if (!string.IsNullOrEmpty(cardId))
                        strBuilder.AppendLine($"AND m.address LIKE '%{cardId.Trim()}%'");

                    if (isLegitPromotionRange)
                        strBuilder.AppendLine($"AND mgp.id BETWEEN {promotionRangeFrom} AND {promotionRangeTo}");

                    strBuilder.Append("ORDER BY mgp.nogroup,");
                    if (Configs.UseAsciiMember)
                        strBuilder.Append(" CAST(CONCAT(CHAR(LEFT(m.cardid, 2)), MID(m.cardid, 3)) AS CHAR)");
                    else                    
                        strBuilder.Append(" m.address");

                    sql = strBuilder.ToString();

                    break;

                case 47:
                    sql = "select m.groupname as บริษัท";
                    sql += " , if(v.cnt >= 1,v.cnt,0) as จำนวน";
                    sql += " from membergroupprice_month m";
                    sql += " left join";
                    sql += " (select t1.groupname as name";
                    sql += " , count(t2.memgrouppriceid_month) as cnt";
                    sql += " from membergroupprice_month t1";
                    sql += " left  join member t2 on t1.id = t2.memgrouppriceid_month";
                    sql += " where 1 = 1 ";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and t1.id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (paymentStatus == Constants.TextBased.PaymentStatusPaid)
                        sql += " and t2.memgrouppriceid_pay > 0";
                    if (paymentStatus == Constants.TextBased.PaymentStatusUnPaid)
                        sql += " and t2.memgrouppriceid_pay = 0";
                    sql += " group by t1.id) v on m.groupname = v.name";
                    sql += " order by CONVERT(m.nogroup,UNSIGNED INTEGER)";
                    break;

                case 48:
                    string fontSlip48 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip48 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName) //Mac 2017/09/11
                            fontSlip48 = "IV";
                    }

                    bool useDatetime = Configs.Reports.UseReport49_1 && !Configs.Reports.Report49_LossCard_NoVat;
                    sql = useDatetime
                        ? "SELECT DATE_FORMAT(recordout.dateout,'%d/%m/%Y %H:%i') AS วันที่"
                        : "SELECT DATE_FORMAT(recordout.dateout,'%d/%m/%Y') AS วันที่";

                    if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            sql += " , concat(recordout.receipt, concat(date_format(recordout.dateout,'%y%m') ,lpad(recordout.printno,6,'0'))) as เลขที่ใบกำกับภาษี"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , concat(recordout.receipt, concat(date_format(recordout.dateout,'%y') ,lpad(recordout.printno,6,'0'))) as เลขที่ใบกำกับภาษี";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2016/04/27
                        {
                            sql += " , concat('" + fontSlip48 + "', concat(date_format(recordout.dateout,'%y%m') ,lpad(recordout.printno,6,'0'))) as เลขที่ใบกำกับภาษี"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , concat('" + fontSlip48 + "', concat(date_format(recordout.dateout,'%y') ,lpad(recordout.printno,6,'0'))) as เลขที่ใบกำกับภาษี";
                        }
                    }

                    if (Configs.Reports.Report49_LossCard_NoVat) //Mac 2021/05/28
                    {
                        if (Configs.IsSwitch)
                        {
                            sql += " , case when recordout.status = 'V' then '* ยกเลิก *' else 'ค่าปรับ' end as รายการ";
                            sql += " , cast(recordout.no as char) as หมายเลขบัตร";

                            sql += " , case when recordout.status = 'V' then format(0, 2) else format(recordout.losscard, 2) end as ค่าบริการ";
                            sql += " , format(0, 2) as VAT";
                            sql += " , case when recordout.status = 'V' then format(0, 2) else format(recordout.losscard, 2) end as จำนวนเงิน";

                            sql += " from recordout";
                            sql += " where recordout.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                            sql += " and recordout.printno > 0 and recordout.losscard > 0";

                            Configs.IsSwitch = false;
                        }
                        else
                        {
                            sql += " , case when recordout.status = 'V' then '* ยกเลิก *' else 'ค่าบริการจอดรถ' end as รายการ";
                            sql += " , cast(recordout.no as char) as หมายเลขบัตร";

                            if (Configs.Reports.ReportPriceSplitLosscard)
                            {
                                sql += " , case when recordout.status = 'V' then format(0, 2) else format((recordout.price-recordout.losscard) - ROUND((recordout.price-recordout.losscard)*7/107, 6), 2) end as ค่าบริการ";
                                sql += " , case when recordout.status = 'V' then format(0, 2) else format(ROUND((recordout.price-recordout.losscard)*7/107, 6), 2) end as VAT";
                                sql += " , case when recordout.status = 'V' then format(0, 2) else format((recordout.price - recordout.losscard), 2) end as จำนวนเงิน";
                            }
                            else
                            {
                                sql += " , case when recordout.status = 'V' then format(0, 2) else format(recordout.price - ROUND(recordout.price*7/107, 6), 2) end as ค่าบริการ";
                                sql += " , case when recordout.status = 'V' then format(0, 2) else format(ROUND(recordout.price*7/107, 6), 2) end as VAT";
                                sql += " , case when recordout.status = 'V' then format(0, 2) else format(recordout.price, 2) end as จำนวนเงิน";
                            }

                            sql += " from recordout";
                            sql += " where recordout.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                            sql += " and recordout.printno > 0";

                            Configs.IsSwitch = true;
                        }
                    }
                    else
                    {
                        sql += " , case when recordout.status = 'V' then '* ยกเลิก *' else 'ค่าบริการจอดรถ' end as รายการ";
                        if (Configs.Reports.UseReport49_1) //Mac 2021/10/14
                        {
                            sql += " , CONCAT(FLOOR(TIMESTAMPDIFF(MINUTE, DATE_FORMAT(recordin.datein,'%Y-%m-%d %H:%i:%s'), " +
                                   "DATE_FORMAT(recordout.dateout,'%Y-%m-%d %H:%i:%s')) / 60), ' ชม. ', " +
                                   "LPAD(MOD(TIMESTAMPDIFF(MINUTE, DATE_FORMAT(recordin.datein,'%Y-%m-%d %H:%i:%s'), " +
                                   "DATE_FORMAT(recordout.dateout,'%Y-%m-%d %H:%i:%s')), 60), 2, '0'), ' นาที') AS รวมเวลาจอด ";
                        }
                        else
                        {
                            sql += " , CAST(recordout.no AS CHAR) AS หมายเลขบัตร";
                        }

                        if (Configs.Reports.ReportPriceSplitLosscard)
                        {
                            sql += " , CASE WHEN recordout.status = 'V' THEN FORMAT(0,2) ELSE FORMAT((recordout.price - recordout.losscard) - ROUND((recordout.price - recordout.losscard) * 7 / 107, 6), 2) END AS ค่าบริการ";
                            sql += " , CASE WHEN recordout.status = 'V' THEN FORMAT(0,2) ELSE FORMAT(ROUND((recordout.price - recordout.losscard) * 7 / 107, 6), 2) END AS VAT";
                            sql += " , CASE WHEN recordout.status = 'V' THEN FORMAT(0,2) ELSE FORMAT(recordout.price - recordout.losscard, 2) END AS จำนวนเงิน";
                        }
                        else
                        {
                            sql += " , CASE WHEN recordout.status = 'V' THEN FORMAT(0,2) ELSE FORMAT(recordout.price - ROUND(recordout.price * 7 / 107, 6), 2) END AS ค่าบริการ";
                            sql += " , CASE WHEN recordout.status = 'V' THEN FORMAT(0,2) ELSE FORMAT(ROUND(recordout.price * 7 / 107, 6), 2) END AS VAT";
                            sql += " , CASE WHEN recordout.status = 'V' THEN FORMAT(0,2) ELSE FORMAT(recordout.price, 2) END AS จำนวนเงิน";
                        }

                        sql += Configs.Reports.UseReport49_1
                            ? " FROM recordout LEFT JOIN recordin ON recordout.no = recordin.no"
                            : " FROM recordout";

                        if (Configs.UsePaymentKsher)
                            sql += " LEFT JOIN (SELECT MAX(t1.no) AS max_no, t1.no_recordin, t1.mch_order_no, t1.channel, t1.status, t2.ksher_order_no FROM ksherpay_post t1 LEFT JOIN ksherpay_get t2 ON t1.mch_order_no = t2.mch_order_no WHERE status = 'Y' GROUP BY no_recordin) t3 ON recordout.no = t3.no_recordin";
                        else if (Configs.UsePaymentBeam)
                            sql += " LEFT JOIN (SELECT MAX(t1.no) AS max_no, t1.no_recordin, t1.beam_id, t1.status, t2.qr FROM beam_post t1 LEFT JOIN beam_get t2 ON t1.beam_id = t2.beam_id WHERE status = 'Y' GROUP BY no_recordin) t3 ON recordout.no = t3.no_recordin";
                        else if (Configs.UsePaymentRabbit)
                            sql += " LEFT JOIN (SELECT MAX(t1.no) AS max_no, t1.no_recordin, t1.rabbit_id, t1.status, t2.qr FROM rabbit_post t1 LEFT JOIN rabbit_get t2 ON t1.rabbit_id = t2.rabbit_id WHERE status = 'Y' GROUP BY no_recordin) t3 ON recordout.no = t3.no_recordin";

                        sql += " where recordout.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                        /* OLD - payment channel 
                         * 
                        if (Configs.UsePrintQRCode) //Mac 2025/03/07
                        {
                            if (paymentChannel == Constants.TextBased.PaymentChannelPromptPay)
                            {
                                if (Configs.UsePaymentKsher)
                                    sql += " AND t3.channel = 'promptpay'";
                                else if (Configs.UsePaymentBeam)
                                    sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'PromptPay'";
                                else if (Configs.UsePaymentRabbit)
                                    sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'PromptPay'";
                            }
                            else if (paymentChannel == Constants.TextBased.PaymentChannelTrueMoney)
                            {
                                sql += " AND t3.channel = 'TrueMoney'";
                            }
                            else if (paymentChannel == Constants.TextBased.PaymentChannelCash)
                            {
                                if (Configs.UsePaymentKsher)
                                    sql += " AND t3.channel is null AND recordout.pay_type = 'C'";
                                else if (Configs.UsePaymentBeam)
                                    sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'เงินสด'";
                                else if (Configs.UsePaymentRabbit)
                                    sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'เงินสด'";
                            }
                            else if (paymentChannel == Constants.TextBased.PaymentChannelEDC)
                            {
                                if (Configs.UsePaymentKsher)
                                    sql += " AND t3.channel is null AND recordout.pay_type = 'EDC'";
                                else if (Configs.UsePaymentBeam)
                                    sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'EDC'";
                                else if (Configs.UsePaymentRabbit)
                                    sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'EDC'";
                            }
                        }*/

                        /* NEW - payment channel */
                        if (paymentChannel == Constants.TextBased.PaymentChannelPromptPay)
                        {
                            if (Configs.UsePaymentKsher)
                                sql += " AND t3.channel = 'promptpay'";
                            else if (Configs.UsePaymentBeam)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'PromptPay'";
                            else if (Configs.UsePaymentRabbit)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'PromptPay'";
                        }
                        else if (paymentChannel == Constants.TextBased.PaymentChannelTrueMoney)
                        {
                            sql += " AND t3.channel = 'TrueMoney'";
                        }
                        else if (paymentChannel == Constants.TextBased.PaymentChannelCash)
                        {
                            if (Configs.UsePaymentKsher)
                                sql += " AND t3.channel is null AND recordout.pay_type = 'C'";
                            else if (Configs.UsePaymentBeam)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'เงินสด'";
                            else if (Configs.UsePaymentRabbit)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'เงินสด'";
                        }
                        else if (paymentChannel == Constants.TextBased.PaymentChannelEDC)
                        {
                            if (Configs.UsePaymentKsher)
                                sql += " AND t3.channel is null AND recordout.pay_type = 'EDC'";
                            else if (Configs.UsePaymentBeam)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'EDC'";
                            else if (Configs.UsePaymentRabbit)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN recordout.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'EDC'";

                        }
                        sql += " and recordout.printno > 0";
                    }

                    if (paymentStatus != Constants.TextBased.All)
                    {
                        if (paymentStatus == Constants.TextBased.PaymentStatusPaid)
                        {
                            sql += " and recordout.price > 0";
                        }
                        else if (paymentStatus == Constants.TextBased.PaymentStatusUnPaid)
                        {
                            sql += " and recordout.price = 0";
                        }
                    }

                    if (Configs.UseReceiptFor1Out)
                    {
                        sql += Configs.OutReceiptNameMonth
                            ? " ORDER BY CONCAT(recordout.receipt, DATE_FORMAT(recordout.dateout,'%y%m'), LPAD(recordout.printno,6,'0'))"
                            : " ORDER BY CONCAT(recordout.receipt, DATE_FORMAT(recordout.dateout,'%y'), LPAD(recordout.printno,6,'0'))";
                    }
                    else
                    {
                        sql += Configs.OutReceiptNameMonth
                            ? " ORDER BY CONCAT('" + fontSlip48 + "', DATE_FORMAT(recordout.dateout,'%y%m'), LPAD(recordout.printno,6,'0'))"
                            : " ORDER BY CONCAT('" + fontSlip48 + "', DATE_FORMAT(recordout.dateout,'%y'), LPAD(recordout.printno,6,'0'))";
                    }
                    break;

                case 49:
                    sql = " DROP TABLE IF EXISTS `vatmonth`;";
                    sql += " CREATE TABLE `vatmonth` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `dateslip` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,";
                    sql += "  `slip` varchar(1000) COLLATE utf8_unicode_ci DEFAULT NULL,";
                    sql += "  `beforevat` decimal(20,2) DEFAULT NULL,";
                    sql += "  `vat` decimal(20,2) DEFAULT NULL,";
                    sql += "  `total` decimal(20,2) DEFAULT NULL,";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    DataTable dTable;
                    string dateSlip49 = "";
                    string slip49 = "";
                    string fontSlip49 = "";
                    double beforeVat49 = 0;
                    double vat49 = 0;
                    double total49 = 0;
                    int tmpSlip = 0;
                    int id49 = 0;

                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip49 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName) //Mac 2017/09/11
                            fontSlip49 = "IV";
                    }

                    if (Configs.OutReceiptNameMonth) //Mac 2016/04/27
                        fontSlip49 += startDate.ToString("yyMM");
                    else
                        fontSlip49 += startDate.ToString("yy");

                    for (var day = startDate.Date; day.Date <= endDate.Date; day = day.AddDays(1))
                    {
                        id49++;
                        sql = "select concat(date_format(t2.dateout,'%d/%m/'), date_format(t2.dateout,'%Y'))";
                        if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2018/07/05
                        {
                            sql += " , cast(((t2.price-t2.losscard) - ROUND((t2.price-t2.losscard)*7/107, 6)) as DECIMAL(10,2)) ";
                            sql += " , cast((ROUND((t2.price-t2.losscard)*7/107, 6)) as DECIMAL(10,2)) ";
                            sql += " , cast(((t2.price-t2.losscard)) as DECIMAL(10,2)) ";
                        }
                        else
                        {
                            sql += " , cast((t2.price - ROUND(t2.price*7/107, 6)) as DECIMAL(10,2)) ";
                            sql += " , cast((ROUND(t2.price*7/107, 6)) as DECIMAL(10,2)) ";
                            sql += " , cast((t2.price) as DECIMAL(10,2)) ";
                        }
                        sql += " , t2.printno ";
                        if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                        {
                            sql += ", t2.receipt ";
                        }
                        sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                        if (Configs.UsePrintQRCode) //Mac 2025/03/07
                            sql += " left join (select max(t1.no), t1.no_recordin, t1.mch_order_no, t1.channel, t1.status, t2.ksher_order_no from ksherpay_post t1 left join ksherpay_get t2 on t1.mch_order_no = t2.mch_order_no where t1.status = 'Y' group by t1.no_recordin) t3 on t2.no = t3.no_recordin";
                        //sql += " where date(t2.dateout) = '" + day.Date.ToString("yyyy-MM-dd") + "'";
                        sql += " where date(t2.dateout) = '" + day.Year.ToString() + "-" + day.ToString("MM'-'dd") + "'";
                        sql += " and t2.no is not null";

                        if (Configs.UsePrintQRCode) //Mac 2025/03/07
                        {
                            if (paymentChannel == Constants.TextBased.PaymentChannelPromptPay)
                                sql += " AND t3.channel = 'PromptPay'";
                            else if (paymentChannel == Constants.TextBased.PaymentChannelTrueMoney)
                                sql += " AND t3.channel = 'TrueMoney'";
                            else if (paymentChannel == Constants.TextBased.PaymentChannelCash)
                                sql += " AND t3.channel is null AND t2.pay_type = 'C'";
                            else if (paymentChannel == Constants.TextBased.PaymentChannelEDC)
                                sql += " AND t3.channel is null AND t2.pay_type = 'EDC'";
                        }
                        sql += " and t2.printno > 0";
                        sql += " and t2.status = 'N'";
                        if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                            sql += " order by t2.receipt, t2.printno";
                        else
                            sql += " order by t2.printno";

                        dTable = DbController.LoadData(sql);
                        if (dTable != null && dTable.Rows.Count > 0)
                        {
                            dateSlip49 = dTable.Rows[0].ItemArray[0].ToString();
                            if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                            {
                                if (Configs.OutReceiptNameMonth)
                                {
                                    //slip49 = dTable.Rows[0]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[0].ItemArray[4]).ToString("000#");
                                    slip49 = dTable.Rows[0]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[0].ItemArray[4]).ToString("00000#"); //Mac 2022/04/26
                                }
                                else
                                    slip49 = dTable.Rows[0]["receipt"].ToString() + startDate.ToString("yy") + Convert.ToInt32(dTable.Rows[0].ItemArray[4]).ToString("00000#");
                            }
                            else
                            {
                                if (Configs.OutReceiptNameMonth) //Mac 2016/04/27
                                {
                                    //slip49 = fontSlip49 + Convert.ToInt32(dTable.Rows[0].ItemArray[4]).ToString("000#");
                                    slip49 = fontSlip49 + Convert.ToInt32(dTable.Rows[0].ItemArray[4]).ToString("00000#"); //Mac 2022/04/26
                                }
                                else
                                    slip49 = fontSlip49 + Convert.ToInt32(dTable.Rows[0].ItemArray[4]).ToString("00000#");
                            }
                            beforeVat49 = Convert.ToDouble(dTable.Rows[0].ItemArray[1]);
                            vat49 = Convert.ToDouble(dTable.Rows[0].ItemArray[2]);
                            total49 = Convert.ToDouble(dTable.Rows[0].ItemArray[3]);
                            tmpSlip = 0;
                            for (int i49 = 1; i49 < dTable.Rows.Count; i49++)
                            {
                                beforeVat49 += Convert.ToDouble(dTable.Rows[i49].ItemArray[1]);
                                vat49 += Convert.ToDouble(dTable.Rows[i49].ItemArray[2]);
                                total49 += Convert.ToDouble(dTable.Rows[i49].ItemArray[3]);


                                if ((Convert.ToInt32(dTable.Rows[i49].ItemArray[4]) - 1) == Convert.ToInt32(dTable.Rows[i49 - 1].ItemArray[4]))
                                {
                                    tmpSlip++;
                                    if (i49 + 1 == dTable.Rows.Count)
                                    {
                                        if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                                        {
                                            if (Configs.OutReceiptNameMonth)
                                            {
                                                //slip49 += "-" + dTable.Rows[i49]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("000#");
                                                slip49 += "-" + dTable.Rows[i49]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#"); //Mac 2022/04/26
                                            }
                                            else
                                                slip49 += "-" + dTable.Rows[i49]["receipt"].ToString() + startDate.ToString("yy") + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#");
                                        }
                                        else
                                        {
                                            if (Configs.OutReceiptNameMonth) //Mac 2016/04/27
                                            {
                                                //slip49 += "-" + fontSlip49 + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("000#");
                                                slip49 += "-" + fontSlip49 + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#"); //Mac 2022/04/26
                                            }
                                            else
                                                slip49 += "-" + fontSlip49 + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#");
                                        }
                                    }
                                }
                                else
                                {
                                    if (tmpSlip > 0)
                                    {
                                        if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                                        {
                                            if (Configs.OutReceiptNameMonth)
                                            {
                                                //slip49 += "-" + dTable.Rows[i49 - 1]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[i49 - 1].ItemArray[4]).ToString("000#") + "," + dTable.Rows[i49]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("000#");
                                                slip49 += "-" + dTable.Rows[i49 - 1]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[i49 - 1].ItemArray[4]).ToString("00000#") + "," + dTable.Rows[i49]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#"); //Mac 2022/04/26
                                            }
                                            else
                                                slip49 += "-" + dTable.Rows[i49 - 1]["receipt"].ToString() + startDate.ToString("yy") + Convert.ToInt32(dTable.Rows[i49 - 1].ItemArray[4]).ToString("00000#") + "," + dTable.Rows[i49]["receipt"].ToString() + startDate.ToString("yy") + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#");
                                        }
                                        else
                                        {
                                            if (Configs.OutReceiptNameMonth) //Mac 2016/04/27
                                            {
                                                //slip49 += "-" + fontSlip49 + Convert.ToInt32(dTable.Rows[i49 - 1].ItemArray[4]).ToString("000#") + "," + fontSlip49 + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("000#");
                                                slip49 += "-" + fontSlip49 + Convert.ToInt32(dTable.Rows[i49 - 1].ItemArray[4]).ToString("00000#") + "," + fontSlip49 + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#");
                                            }
                                            else
                                                slip49 += "-" + fontSlip49 + Convert.ToInt32(dTable.Rows[i49 - 1].ItemArray[4]).ToString("00000#") + "," + fontSlip49 + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#");
                                        }
                                    }
                                    else
                                    {
                                        if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                                        {
                                            if (Configs.OutReceiptNameMonth)
                                            {
                                                //slip49 += "," + dTable.Rows[i49]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("000#");
                                                slip49 += "," + dTable.Rows[i49]["receipt"].ToString() + startDate.ToString("yyMM") + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#"); //Mac 2022/04/26
                                            }
                                            else
                                                slip49 += "," + dTable.Rows[i49]["receipt"].ToString() + startDate.ToString("yy") + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#");
                                        }
                                        else
                                        {
                                            if (Configs.OutReceiptNameMonth) //Mac 2016/04/27
                                            {
                                                //slip49 += "," + fontSlip49 + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("000#");
                                                slip49 += "," + fontSlip49 + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#"); //Mac 2022/04/26
                                            }
                                            else
                                                slip49 += "," + fontSlip49 + Convert.ToInt32(dTable.Rows[i49].ItemArray[4]).ToString("00000#");
                                        }
                                    }

                                    tmpSlip = 0;
                                }
                            }
                        }
                        else
                        {
                            dateSlip49 = day.ToString("dd/MM/") + (Convert.ToInt32(day.Year));
                            slip49 = "";
                            beforeVat49 = 0;
                            vat49 = 0;
                            total49 = 0;
                        }
                        sql = "insert into vatmonth (id,dateslip,slip,beforevat,vat,total)";
                        sql += " values (";
                        sql += id49;
                        sql += " ,'" + dateSlip49 + "'";
                        sql += " ,'" + slip49 + "'";
                        if (Configs.UseCalVatFromTotal) //Mac 2022/10/04
                        {
                            sql += " ," + (total49 - (total49 * 7 / 107));
                            sql += " ," + (total49 * 7 / 107);
                        }
                        else
                        {
                            sql += " ," + beforeVat49;
                            sql += " ," + vat49;
                        }
                        sql += " ," + total49;
                        sql += ")";
                        DbController.SaveData(sql);
                    }
                    sql = "select id as ลำดับ";
                    sql += " ,dateslip as 'วัน/เดือน/ปี'";
                    sql += " ,slip as เลขที่ใบกำกับภาษี";
                    sql += " ,format(beforevat, 2) as ค่าบริการ";
                    sql += " ,format(vat, 2) as VAT";
                    sql += " ,format(total, 2) as รวมเงิน";
                    sql += " from vatmonth";
                    break;
                case 50:
                    sql = "select s as ประเภทรถ, if(LENGTH(ss) > 0,(select t2.typename from cartype t2 where t2.typeid = t1.ss),'') as ประเภทการเก็บเงิน, sss as จำนวนคัน";
                    sql += " from (";
                    sql += " select vehicletype as s,'' as ss,count(vehicletype) as sss from recordin where datein BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "' group by vehicletype";
                    sql += " union";
                    sql += " select '' as s,cartype as ss,count(cartype) as sss from recordin where datein BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "' group by cartype";
                    sql += " ) t1;";
                    sql += "";
                    break;
                case 51:
                    AppGlobalVariables.ConditionText = "";
                    string fontSlip51 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip51 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName) //Mac 2017/09/11
                            fontSlip51 = "IV";
                    }

                    sql = "select (select name from user where id = t1.userout) as 'เจ้าหน้าที่ขาออก'";
                    if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , concat(t1.receipt, concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,4,'0'))) as 'เลขที่ใบกำกับภาษี'";
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                                sql += " , concat(t1.receipt, concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno_second,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                            else
                                sql += " , concat(t1.receipt, concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'"; //Mac 2022/04/26
                        }
                        else
                        {
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                                sql += " , concat(t1.receipt, concat(date_format(t1.dateout,'%y') ,lpad(t1.printno_second,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                            else
                                sql += " , concat(t1.receipt, concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , concat('" + fontSlip51 + "', concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,4,'0'))) as 'เลขที่ใบกำกับภาษี'";
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                                sql += " , concat('" + fontSlip51 + "', concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno_second,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                            else
                                sql += " , concat('" + fontSlip51 + "', concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'"; //Mac 2022/04/26
                        }
                        else
                        {
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                                sql += " , concat('" + fontSlip51 + "', concat(date_format(t1.dateout,'%y') ,lpad(t1.printno_second,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                            else
                                sql += " , concat('" + fontSlip51 + "', concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                        }
                    }
                    sql += " , date_format(t2.datein, '%d/%m/%Y %H:%i:%s') as 'วันเวลาเข้า', date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วันเวลาออก', t2.license as 'ทะเบียน'";
                    //sql += " , timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i'), date_format(t1.dateout, '%Y-%m-%d %H:%i')) as 'เวลาจอด'";
                    sql += ", concat(floor(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s'))/60)";
                    sql += ", '.', lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0')) as 'เวลาจอด'";
                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2019/08/27
                    {
                        sql += " , format((t1.price-t1.losscard) - ROUND((t1.price-t1.losscard)*7/107, 6), 2) as 'ค่าจอด'";
                        sql += " , format(t1.losscard, 2) as 'ค่าปรับ'";
                        sql += " , format(ROUND((t1.price-t1.losscard)*7/107, 6), 2) as 'ภาษี'";
                        sql += " , format(t1.price-t1.losscard, 2) as 'รวมเงิน'";
                    }
                    else
                    {
                        sql += " , format(t1.price - ROUND(t1.price*7/107, 6) - (t1.losscard  - ROUND(t1.losscard*7/107, 6)), 2) as 'ค่าจอด'";
                        sql += " , format(t1.losscard - ROUND(t1.losscard*7/107, 6), 2) as 'ค่าปรับ'";
                        sql += " , format(ROUND(t1.price*7/107, 6), 2) as 'ภาษี'";
                        sql += " , format(t1.price, 2) as 'รวมเงิน'";
                    }
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and t1.printno_second > 0";
                    else
                        sql += " and t1.printno > 0";

                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and t2.notshow = 'N'";

                    if (Configs.UseVoidSlip)
                        sql += " and t1.status = 'N'";

                    if (user != Constants.TextBased.All)
                        sql += " and t1.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (carType == Constants.TextBased.Visitor)
                        sql += " AND t2.cartype != 200";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " AND t2.typeid =" + carTypeId;
                    if (promotionName != Constants.TextBased.All)
                    {
                        if (Configs.UseProIDAll)
                        {
                            sql += " AND t1.proid_all like '%" + promotionId + ",%'";
                        }
                        else
                        {
                            sql += " AND t1.proid =" + promotionId;
                        }
                    }
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t2.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t2.id = " + cardId;
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";

                    if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " order by t1.receipt, concat(concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,4,'0')))";
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                                sql += " order by t1.receipt, concat(concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno_second,6,'0')))";
                            else
                                sql += " order by t1.receipt, concat(concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0')))"; //Mac 2022/04/26
                        }
                        else
                        {
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                                sql += " ORDER BY t1.receipt, t1.printno_second";
                            else
                                sql += " ORDER BY t1.receipt, t1.printno";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " order by concat(concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,4,'0')))";
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                                sql += " order by concat(concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno_second,6,'0')))";
                            else
                                sql += " order by concat(concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0')))"; //Mac 2022/04/26
                        }
                        else
                        {
                            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                                sql += " ORDER BY t1.printno_second";
                            else
                                sql += " ORDER BY t1.printno";
                        }
                    }

                    AppGlobalVariables.ConditionText = "ประจำวันที่ " + startDate.ToString("dd/MM/yyyy") + " " + startTime.ToLongTimeString() + " ถึงวันที่ " + endDate.ToString("dd/MM/yyyy") + " " + endTime.ToLongTimeString();
                    break;
                case 52:
                    AppGlobalVariables.ConditionText = "";
                    //Mac 2022/04/28
                    sql = "select date_format(t1.dateout, '%d/%m/%Y') as 'วัน-เดือน-ปี'";
                    sql += ", count(t1.no) as 'รวมรถ', format(sum(t1.losscard), 2) as 'ค่าบัตรหาย', format((sum(t1.price) - sum(t1.losscard)), 2) as 'ค่าจอดรถ', format(sum(t1.price), 2) as 'รวมเงิน'";
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where t1.dateout between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59'";
                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and t2.notshow = 'N'";
                    sql += " group by date(t1.dateout)";
                    sql += " order by date(t1.dateout)";

                    AppGlobalVariables.ConditionText = "ประจำวันที่ " + startDate.ToString("dd/MM/yyyy") + " ถึงวันที่ " + endDate.ToString("dd/MM/yyyy");
                    break;
                case 53:
                    AppGlobalVariables.ConditionText = "";
                    //Mac 2022/04/28
                    sql = "select date_format(t1.dateout, '%m') as 'month', date_format(t1.dateout, '%Y') as 'year'";
                    sql += ", count(t1.no) as 'รวมรถ', format(sum(t1.losscard), 2) as 'ค่าบัตรหาย', format((sum(t1.price) - sum(t1.losscard)), 2) as 'ค่าจอดรถ', format(sum(t1.price), 2) as 'รวมเงิน'";
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where t1.dateout between '" + firstDayOfMonth.Year.ToString() + "-" + firstDayOfMonth.ToString("MM'-'dd") + " 00:00:00' and '" + lastDayOfMonthEnd.Year.ToString() + "-" + lastDayOfMonthEnd.ToString("MM'-'dd") + " 23:59:59'";
                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and t2.notshow = 'N'";
                    sql += " group by date_format(t1.dateout, '%m %Y')";
                    sql += " order by date(t1.dateout)";

                    AppGlobalVariables.ConditionText = "ประจำเดือน " + startDate.ToString("MMMM yyyy") + " ถึงเดือน " + endDate.ToString("MMMM yyyy");
                    break;
                case 54:
                    AppGlobalVariables.ConditionText = "";
                    sql = "select t1.license as 'ทะเบียน', cast(t1.id as char) as 'หมายเลขบัตร'";
                    sql += ", (select typename from cartype t where t.typeid = t1.cartype) as 'ประเภทบัตร'";
                    sql += ", t1.guardhouse as 'ประตู', date_format(t1.datein, '%d/%m/%Y %H:%i:%s') as 'วันเวลาเข้า'";
                    sql += ", (select name from user where id = t1.userin) as 'เจ้าหน้าที่ขาเข้า'";
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no where t2.no is null";
                    sql += " and t1.datein between '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and t1.notshow = 'N'";

                    if (user != Constants.TextBased.All)
                        sql += " and t1.userin =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (carType == Constants.TextBased.Visitor)
                        sql += " AND t1.cartype != 200";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " AND t1.typeid =" + carTypeId;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t1.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t1.id = " + cardId;
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";
                    sql += " order by t1.datein";

                    AppGlobalVariables.ConditionText = "ประจำวันที่ " + startDate.ToString("dd/MM/yyyy") + " " + startTime.ToLongTimeString() + " ถึงวันที่ " + endDate.ToString("dd/MM/yyyy") + " " + endTime.ToLongTimeString();
                    break;
                case 55:
                    AppGlobalVariables.ConditionText = "";
                    string fontSlip55 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip55 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName) //Mac 2017/09/11
                            fontSlip55 = "IV";
                    }

                    sql = "select t2.license as 'ทะเบียน', cast(t2.id as char) as 'หมายเลขบัตร'";
                    sql += ", date_format(t2.datein, '%d/%m/%Y %H:%i:%s') as 'วันเวลาเข้า', t2.guardhouse as 'ประตู'";
                    sql += ", date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วันเวลาออก', t1.guardhouse as 'ประตู.'";
                    sql += ", (select name from user where id = t1.userout) as 'เจ้าหน้าที่ขาออก'";
                    sql += ", format(t1.losscard, 2) as 'มูลค่าบัตรหาย'";
                    if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , concat(t1.receipt, concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,4,'0'))) as 'Ref No.'";
                            sql += " , concat(t1.receipt, concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'Ref No.'"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , concat(t1.receipt, concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'Ref No.'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , concat('" + fontSlip55 + "', concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,4,'0'))) as 'Ref No.'";
                            sql += " , concat('" + fontSlip55 + "', concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'Ref No.'"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , concat('" + fontSlip55 + "', concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'Ref No.'";
                        }
                    }
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t1.losscard > 0";

                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and t2.notshow = 'N'";

                    if (user != Constants.TextBased.All)
                        sql += " and t1.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (carType == Constants.TextBased.Visitor)
                        sql += " AND t2.cartype != 200";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " AND t2.typeid =" + carTypeId;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t2.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t2.id = " + cardId;
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";

                    sql += " order by dateout";

                    AppGlobalVariables.ConditionText = "ประจำวันที่ " + startDate.ToString("dd/MM/yyyy") + " " + startTime.ToLongTimeString() + " ถึงวันที่ " + endDate.ToString("dd/MM/yyyy") + " " + endTime.ToLongTimeString();
                    break;
                case 56:
                    AppGlobalVariables.ConditionText = "";
                    sql = " DROP TABLE IF EXISTS `report57`;";
                    sql += " CREATE TABLE `report57` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `hours` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `invi` int(11) DEFAULT '0',";
                    sql += "  `inmem` int(11) DEFAULT '0',";
                    sql += "  `outvi` int(11) DEFAULT '0',";
                    sql += "  `outmem` int(11) DEFAULT '0',";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    for (int i = 0; i < 24; i++)
                    {
                        sql = "insert into report57 values (" + (i + 1) + ", '" + i.ToString().PadLeft(2, '0') + ":00-" + i.ToString().PadLeft(2, '0') + ":59'";
                        sql += ", (select count(no) from recordin where datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and cartype < 200 and hour(datein) = " + i;
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " and recordin.notshow = 'N'";
                        sql += ")";
                        sql += ", (select count(no) from recordin where datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and cartype = 200 and hour(datein) = " + i;
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " and recordin.notshow = 'N'";
                        sql += ")";
                        sql += ", (select count(t1.no) from recordout t1 left join recordin t2 on t1.no = t2.no where t1.dateout between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and t2.cartype < 200 and hour(t1.dateout) = " + i;
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " and t2.notshow = 'N'";
                        sql += ")";
                        sql += ", (select count(t1.no) from recordout t1 left join recordin t2 on t1.no = t2.no where t1.dateout between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and t2.cartype = 200 and hour(t1.dateout) = " + i;
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " and t2.notshow = 'N'";
                        sql += ")";
                        sql += ")";
                        DbController.SaveData(sql);
                    }

                    sql = "select `hours` as 'ช่วงเวลา', `invi` as 'ลูกค้าทั่วไป', `inmem` as 'สมาชิก', `outvi` as 'ลูกค้าทั่วไป.', `outmem` as 'สมาชิก.' from report57";
                    sql += " order by Id";

                    AppGlobalVariables.ConditionText = "ประจำวันที่ " + startDate.ToString("dd/MM/yyyy") + " ถึงวันที่ " + endDate.ToString("dd/MM/yyyy");
                    break;
                case 57:
                    AppGlobalVariables.ConditionText = "";
                    sql = "select concat(left(memid, 2), ' ', mid(memid, 3)) as 'เลขสมาชิก', name as 'ชื่อ-นามสกุล', gender as 'เพศ', date_format(birthday, '%d/%m/%Y') as 'วันเกิด', date_format(dateexpire, '%d/%m/%Y %H:%i:%s') as 'บัตรหมดอายุ'";
                    sql += ", case when status = 'Y' then 'ใช้งาน' when status = 'C' then 'ยกเลิก' else 'บัตรหาย' end as 'สถานะ'";
                    sql += ", recordby as 'บันทึกโดย', date_format(recorddate, '%d/%m/%Y %H:%i:%s') as 'วันที่บันทึก'";
                    sql += " from member_up2u where 1 = 1";

                    if (memberGroup != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " กลุ่ม : " + memberGroup;
                        sql += " and memgroup = '" + memberGroup + "'";
                    }
                    if (memberStatus != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " สถานะ : " + memberStatus;
                        sql += " and status = '" + AppGlobalVariables.MemberStatusesLookup[memberStatus] + "'";
                    }
                    if (!String.IsNullOrEmpty(cardId))
                    {
                        AppGlobalVariables.ConditionText += " เลขบัตร : " + cardId;
                        sql += " and cardid = '" + cardId + "'";
                    }
                    if (memberId != "")
                    {
                        AppGlobalVariables.ConditionText += " เลขสมาชิก : " + memberId;
                        sql += " and memid = '" + memberId + "'";
                    }


                    if (isExpirationDateChecked)
                    {
                        AppGlobalVariables.ConditionText += " วันหมดอายุตั้งวันที่ : " + memberExpirationStartDate.ToString("dd/MM/yyyy") + " - " + memberExpirationEndDate.ToString("dd/MM/yyyy");
                        sql += " and dateexpire between '" + memberExpirationStartDate.Year.ToString() + "-" + memberExpirationStartDate.ToString("MM'-'dd") + " 00:00:00' and '" + memberExpirationEndDate.Year.ToString() + "-" + memberExpirationEndDate.ToString("MM'-'dd") + " 23:59:59'";
                    }

                    sql += " order by memid";
                    if (AppGlobalVariables.ConditionText.Length > 0)
                        AppGlobalVariables.ConditionText = AppGlobalVariables.ConditionText.Substring(1);
                    break;
                case 58:
                    AppGlobalVariables.ConditionText = "";
                    sql = "select concat(left(memid, 2), ' ', mid(memid, 3)) as 'เลขสมาชิก', name as 'ชื่อ-นามสกุล', gender as 'เพศ', date_format(birthday, '%d/%m/%Y') as 'วันเกิด', date_format(dateexpire, '%d/%m/%Y %H:%i:%s') as 'บัตรหมดอายุ'";
                    sql += ", case when status = 'Y' then 'ใช้งาน' when status = 'C' then 'ยกเลิก' else 'บัตรหาย' end as 'สถานะ'";
                    sql += ", recordby as 'บันทึกโดย', date_format(recorddate, '%d/%m/%Y %H:%i:%s') as 'วันที่บันทึก'";
                    sql += " from member_up2u_record where 1 = 1";
                    sql += " and recorddate between '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                    AppGlobalVariables.ConditionText += " วันที่ : " + startDate.ToString("dd/MM/yyyy") + " " + startTime.ToLongTimeString() + " - " + endDate.ToString("dd/MM/yyyy") + " " + endTime.ToLongTimeString();
                    if (memberGroup != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " กลุ่ม : " + memberGroup;
                        sql += " and memgroup = '" + memberGroup + "'";
                    }
                    if (memberStatus != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " สถานะ : " + memberStatus;
                        sql += " and status = '" + AppGlobalVariables.MemberStatusesLookup[memberStatus] + "'";
                    }
                    if (!String.IsNullOrEmpty(cardId))
                    {
                        AppGlobalVariables.ConditionText += " เลขบัตร : " + cardId;
                        sql += " and cardid = '" + cardId + "'";
                    }
                    if (memberId != "")
                    {
                        AppGlobalVariables.ConditionText += " เลขสมาชิก : " + memberId;
                        sql += " and memid = '" + memberId + "'";
                    }
                    if (isExpirationDateChecked)
                    {
                        AppGlobalVariables.ConditionText += " วันหมดอายุตั้งวันที่ : " + memberExpirationStartDate.ToString("dd/MM/yyyy") + " - " + memberExpirationEndDate.ToString("dd/MM/yyyy");
                        sql += " and dateexpire between '" + memberExpirationStartDate.Year.ToString() + "-" + memberExpirationStartDate.ToString("MM'-'dd") + " 00:00:00' and '" + memberExpirationEndDate.Year.ToString() + "-" + memberExpirationEndDate.ToString("MM'-'dd") + " 23:59:59'";
                    }

                    sql += " order by memid";

                    if (AppGlobalVariables.ConditionText.Length > 0)
                        AppGlobalVariables.ConditionText = AppGlobalVariables.ConditionText.Substring(1);
                    break;
                case 59:
                    AppGlobalVariables.ConditionText = "";
                    if (Configs.IsSwitch)
                        sql = "select concat(left(t3.memid, 2), ' ', mid(t3.memid, 3)) as 'เลขสมาชิก', date_format(t1.datein,'%d/%m/%Y') as 'วันที่ใช้บริการ'";
                    else
                        sql = "select date_format(t1.datein,'%d/%m/%Y') as 'วันที่ใช้บริการ', concat(left(t3.memid, 2), ' ', mid(t3.memid, 3)) as 'เลขสมาชิก'";
                    sql += ", date_format(t1.datein, '%H.%i') as 'เวลาเข้า', date_format(t2.dateout, '%H.%i') as 'เวลาออก'";
                    sql += ", concat(floor(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i'))/60)";
                    sql += ", '.', lpad(mod(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i')), 60), 2, '0')) as 'จำนวน ชม.'";
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no left join member_up2u t3 on t1.id = t3.cardid where t2.no is not null and t3.cardid is not null";
                    sql += " and t1.datein between '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                    AppGlobalVariables.ConditionText += " วันที่ : " + startDate.ToString("dd/MM/yyyy") + " " + startTime.ToLongTimeString() + " - " + endDate.ToString("dd/MM/yyyy") + " " + endTime.ToLongTimeString();
                    if (memberGroup != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " กลุ่ม : " + memberGroup;
                        sql += " and t3.memgroup = '" + memberGroup + "'";
                    }
                    if (!String.IsNullOrEmpty(cardId))
                    {
                        AppGlobalVariables.ConditionText += " เลขบัตร : " + cardId;
                        sql += " and t3.cardid = '" + cardId + "'";
                    }
                    if (memberId != "")
                    {
                        AppGlobalVariables.ConditionText += " เลขสมาชิก : " + memberId;
                        sql += " and t3.memid = '" + memberId + "'";
                    }

                    if (Configs.IsSwitch)
                    {
                        sql += " order by t1.no";
                    }
                    else
                    {
                        sql += " order by t3.memid, t1.no";
                    }
                    if (AppGlobalVariables.ConditionText.Length > 0)
                        AppGlobalVariables.ConditionText = AppGlobalVariables.ConditionText.Substring(1);
                    break;
                case 60:
                    AppGlobalVariables.ConditionText = "";
                    /*int intSumInMonth = 0;
                    int[] intSumIn;
                    string strMemID = "";
                    int intDayOfMonth = 0;*/
                    /*sql = " DROP TABLE IF EXISTS `report61`;";
                    sql += " CREATE TABLE `report61` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `memid` varchar(10) CHARACTER SET utf8 DEFAULT NULL,";
                    for (var day = firstDayOfMonth; day.Date <= lastDayOfMonth; day = day.AddDays(1))
                    {
                        sql += "  `" + day.Day + "` int(11) DEFAULT '0',";
                        intDayOfMonth++;
                    }
                    sql += "  `sum` int(11) DEFAULT '0',";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    sql = "select memid,cardid from member_up2u order by memid";
                    dTable = DbController.LoadData(sql);
                    if (dTable != null && dTable.Rows.Count > 0)
                    {                     
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            intSumInMonth = 0;
                            intSumIn = new int[intDayOfMonth];
                            strMemID = dTable.Rows[i]["memid"].ToString();
                            for (var day = firstDayOfMonth; day.Date <= lastDayOfMonth; day = day.AddDays(1))
                            {
                                sql = "select count(datein) from recordin where date(datein) = '" + day.Year.ToString() + "-" + day.ToString("MM'-'dd") + "' and id = " + dTable.Rows[i]["cardid"].ToString();
                                DataTable dt = DbController.LoadData(sql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    intSumIn[day.Day - 1] = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                                    intSumInMonth += Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                                }
                                else
                                {
                                    intSumIn[day.Day - 1] = 0;
                                }
                            }
                            sql = "insert into report61";
                            sql += " values (";
                            sql += i + 1;
                            sql += " ,'" + strMemID + "'";
                            for (int j = 0; j < intDayOfMonth; j++)
                            {
                                sql += " ," + intSumIn[j];
                            }
                            sql += " ," + intSumInMonth;
                            sql += ")";
                            DbController.SaveData(sql);
                        }
                    }
                    sql += "";*/


                    sql = "select concat(left(t1.memid, 2), ' ', mid(t1.memid, 3)) as 'หมายเลขบัตรสมาชิก8หลัก' ";
                    for (var day = firstDayOfMonth; day.Date <= lastDayOfMonth; day = day.AddDays(1))
                    {
                        sql += ", SUM(case when date(t2.datein) = '" + day.Year.ToString() + "-" + day.ToString("MM'-'dd") + "' then 1 else 0 end) as '" + day.Day + "'";
                    }
                    sql += " , count(*) as 'Total'";
                    sql += " from member_up2u t1 left join recordin t2 on t1.cardid = t2.id";
                    sql += " where t2.datein between '" + firstDayOfMonth.Year.ToString() + "-" + firstDayOfMonth.ToString("MM'-'dd") + " 00:00:00' and '" + lastDayOfMonth.Year.ToString() + "-" + lastDayOfMonth.ToString("MM'-'dd") + " 23:59:59'";

                    AppGlobalVariables.ConditionText += " วันที่ : " + firstDayOfMonth.ToString("dd/MM/yyyy") + " - " + lastDayOfMonth.ToString("dd/MM/yyyy");
                    if (memberGroup != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " กลุ่ม : " + memberGroup;
                        sql += " and t1.memgroup = '" + memberGroup + "'";
                    }
                    if (!String.IsNullOrEmpty(cardId))
                    {
                        AppGlobalVariables.ConditionText += " เลขบัตร : " + cardId;
                        sql += " and t1.cardid = '" + cardId + "'";
                    }
                    if (memberId != "")
                    {
                        AppGlobalVariables.ConditionText += " เลขสมาชิก : " + memberId;
                        sql += " and t1.memid = '" + memberId + "'";
                    }

                    sql += " group by t1.memid";
                    if (memberParkingCountStart.Length > 0 && memberParkingCountEnd.Length > 0)
                    {
                        AppGlobalVariables.ConditionText += " จำนวนครั้งที่จอด : " + memberParkingCountStart + " - " + memberParkingCountEnd;
                        sql += " having count(*) between " + memberParkingCountStart + " and " + memberParkingCountEnd;
                    }
                    else if (memberParkingCountStart.Length > 0 && memberParkingCountEnd.Length == 0)
                    {
                        AppGlobalVariables.ConditionText += " จำนวนครั้งที่จอด : >= " + memberParkingCountStart;
                        sql += " having count(*) >= " + memberParkingCountStart;
                    }
                    else if (memberParkingCountStart.Length == 0 && memberParkingCountEnd.Length > 0)
                    {
                        AppGlobalVariables.ConditionText += " จำนวนครั้งที่จอด : <= " + memberParkingCountEnd;
                        sql += " having count(*) <= " + memberParkingCountEnd;
                    }

                    sql += " order by t1.memid";
                    if (AppGlobalVariables.ConditionText.Length > 0)
                        AppGlobalVariables.ConditionText = AppGlobalVariables.ConditionText.Substring(1);
                    break;
                case 61:
                    AppGlobalVariables.ConditionText = "";
                    sql = "select x.memgroup as 'กลุ่ม', if(y.a is null, 0, y.a) as 'หญิง', if(y.b is null, 0, y.b) as 'ชาย', if(y.c is null, 0, y.c) as 'จำนวนครั้ง', if(y.d is null, 0, y.d) as '%', if(y.e is null, 0, y.e) as 'เฉลี่ยชั่วโมง' from member_up2u x left join (";
                    sql += " select t1.memgroup as 'memgroup', SUM(case when t1.gender = 'F' then 1 else 0 end) as 'a', SUM(case when t1.gender = 'M' then 1 else 0 end) as 'b', count(*) as 'c'";
                    sql += " , format((count(*) / (SELECT count(*) from member_up2u t1 left join recordin t2 on t1.cardid = t2.id left join recordout t3 on t2.no = t3.no where t3.no is not null and t2.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59')) * 100, 2) AS 'd'";
                    sql += " , concat(floor(avg(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i'), date_format(t3.dateout, '%Y-%m-%d %H:%i')))/60), '.', lpad(floor(mod(avg(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i'), date_format(t3.dateout, '%Y-%m-%d %H:%i'))), 60)), 2, '0')) as 'e'";
                    sql += " from member_up2u t1 left join recordin t2 on t1.cardid = t2.id left join recordout t3 on t2.no = t3.no";
                    sql += " where t3.no is not null and t2.datein between '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                    AppGlobalVariables.ConditionText += " วันที่ : " + startDate.ToString("dd/MM/yyyy") + " " + startTime.ToLongTimeString() + " - " + endDate.ToString("dd/MM/yyyy") + " " + endTime.ToLongTimeString();
                    if (memberGroup != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " กลุ่ม : " + memberGroup;
                        sql += " and t1.memgroup = '" + memberGroup + "'";
                    }

                    sql += " group by t1.memgroup) y on x.memgroup = y.memgroup group by x.memgroup order by x.memgroup";
                    /*sql = "select t1.memgroup as 'กลุ่ม', SUM(case when t1.gender = 'F' then 1 else 0 end) as 'หญิง', SUM(case when t1.gender = 'M' then 1 else 0 end) as 'ชาย', count(*) as 'จำนวนครั้ง'";
                    sql += " , format((count(*) / (SELECT count(*) from member_up2u t1 left join recordin t2 on t1.cardid = t2.id left join recordout t3 on t2.no = t3.no where t3.no is not null and t2.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59')) * 100, 2) AS '%'";
                    sql += " , concat(floor(avg(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i'), date_format(t3.dateout, '%Y-%m-%d %H:%i')))/60), '.', lpad(floor(mod(avg(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i'), date_format(t3.dateout, '%Y-%m-%d %H:%i'))), 60)), 2, '0')) as 'เฉลี่ยชั่วโมง'";
                    sql += " from member_up2u t1 left join recordin t2 on t1.cardid = t2.id left join recordout t3 on t2.no = t3.no";
                    sql += " where t3.no is not null and t2.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59'";
                    sql += " group by t1.memgroup";*/
                    if (AppGlobalVariables.ConditionText.Length > 0)
                        AppGlobalVariables.ConditionText = AppGlobalVariables.ConditionText.Substring(1);
                    break;
                case 62:
                    AppGlobalVariables.ConditionText = "";
                    sql = "select x.memgroup as 'กลุ่ม', if(y.a is null, 0, y.a) as 'weekday', if(y.b is null, 0, y.b) as 'weekend', if(y.c is null, 0, y.c) as 'จำนวนครั้ง', if(y.d is null, 0, y.d) as '%', if(y.e is null, 0, y.e) as 'เฉลี่ยชั่วโมง' from member_up2u x left join (";
                    sql += "select t1.memgroup as 'memgroup', sum(if(weekday(t2.datein) < 5,1,0)) as 'a', sum(if(weekday(t2.datein) >= 5,1,0)) as 'b', count(*) as 'c'";
                    sql += " , format((count(*) / (SELECT count(*) from member_up2u t1 left join recordin t2 on t1.cardid = t2.id left join recordout t3 on t2.no = t3.no where t3.no is not null and t2.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59')) * 100, 2) AS 'd'";
                    sql += " , concat(floor(avg(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i'), date_format(t3.dateout, '%Y-%m-%d %H:%i')))/60), '.', lpad(floor(mod(avg(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i'), date_format(t3.dateout, '%Y-%m-%d %H:%i'))), 60)), 2, '0')) as 'e'";
                    sql += " from member_up2u t1 left join recordin t2 on t1.cardid = t2.id left join recordout t3 on t2.no = t3.no";
                    sql += " where t3.no is not null and t2.datein between '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                    AppGlobalVariables.ConditionText += " วันที่ : " + startDate.ToString("dd/MM/yyyy") + " " + startTime.ToLongTimeString() + " - " + endDate.ToString("dd/MM/yyyy") + " " + endTime.ToLongTimeString();
                    if (memberGroup != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " กลุ่ม : " + memberGroup;
                        sql += " and t1.memgroup = '" + memberGroup + "'";
                    }

                    sql += " group by t1.memgroup) y on x.memgroup = y.memgroup group by x.memgroup order by x.memgroup";
                    /*sql = "select t1.memgroup as 'กลุ่ม', sum(if(weekday(t2.datein) < 5,1,0)) as 'weekday', sum(if(weekday(t2.datein) >= 5,1,0)) as 'weekend', count(*) as 'จำนวนครั้ง'";
                    sql += " , format((count(*) / (SELECT count(*) from member_up2u t1 left join recordin t2 on t1.cardid = t2.id left join recordout t3 on t2.no = t3.no where t3.no is not null and t2.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59')) * 100, 2) AS '%'";
                    sql += " , concat(floor(avg(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i'), date_format(t3.dateout, '%Y-%m-%d %H:%i')))/60), '.', lpad(floor(mod(avg(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i'), date_format(t3.dateout, '%Y-%m-%d %H:%i'))), 60)), 2, '0')) as 'เฉลี่ยชั่วโมง'";
                    sql += " from member_up2u t1 left join recordin t2 on t1.cardid = t2.id left join recordout t3 on t2.no = t3.no";
                    sql += " where t3.no is not null and t2.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59'";
                    sql += " group by t1.memgroup";*/
                    if (AppGlobalVariables.ConditionText.Length > 0)
                        AppGlobalVariables.ConditionText = AppGlobalVariables.ConditionText.Substring(1);
                    break;
                case 63:
                    AppGlobalVariables.ConditionText = "";
                    sql = "select concat(left(memid, 2), ' ', mid(memid, 3)) as 'เลขสมาชิก', name as 'ชื่อ-นามสกุล', gender as 'เพศ', date_format(birthday, '%d/%m/%Y') as 'วันเกิด', date_format(dateexpire, '%d/%m/%Y %H:%i:%s') as 'บัตรหมดอายุ', case when status = 'Y' then 'ใช้งาน' when status = 'C' then 'ยกเลิก' else 'บัตรหาย' end as 'สถานะ'";
                    sql += " from member_up2u where memid not in (";
                    sql += " select t1.memid from member_up2u t1 left join recordin t2 on t1.cardid = t2.id where t2.datein between '" + startDateTimeText + "' and '" + endDateTimeText + "')";

                    AppGlobalVariables.ConditionText += " วันที่ : " + startDate.ToString("dd/MM/yyyy") + " " + startTime.ToLongTimeString() + " - " + endDate.ToString("dd/MM/yyyy") + " " + endTime.ToLongTimeString();
                    if (memberGroup != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " กลุ่ม : " + memberGroup;
                        sql += " and memgroup = '" + memberGroup + "'";
                    }

                    sql += " order by memid";
                    if (AppGlobalVariables.ConditionText.Length > 0)
                        AppGlobalVariables.ConditionText = AppGlobalVariables.ConditionText.Substring(1);
                    break;
                case 64:
                    AppGlobalVariables.ConditionText = "";
                    sql = "select x.memgroup as 'กลุ่ม', if(y.a is null, 0, y.a) as 'ใช้บริการ'";
                    sql += " , format((if(y.a is null, 0, y.a) / (select count(*) from member_up2u where memgroup = x.memgroup)) * 100, 2) as '%'";
                    sql += " , (select count(*) from member_up2u where memgroup = x.memgroup) - if(y.a is null, 0, y.a) as 'ไม่ใช้บริการ'";
                    sql += " , format(100.00 - format((if(y.a is null, 0, y.a) / (select count(*) from member_up2u where memgroup = x.memgroup)) * 100, 2), 2) AS '%.'";
                    sql += " , (select count(*) from member_up2u where memgroup = x.memgroup) as 'ทั้งหมด' from member_up2u x left join (";
                    sql += " select t1.memgroup as 'memgroup', count(distinct(t1.id)) as 'a' from member_up2u t1 left join recordin t2 on t1.cardid = t2.id";
                    sql += " where t2.datein between '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                    AppGlobalVariables.ConditionText += " วันที่ : " + startDate.ToString("dd/MM/yyyy") + " " + startTime.ToLongTimeString() + " - " + endDate.ToString("dd/MM/yyyy") + " " + endTime.ToLongTimeString();
                    if (memberGroup != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " กลุ่ม : " + memberGroup;
                        sql += " and t1.memgroup = '" + memberGroup + "'";
                    }

                    sql += " group by t1.memgroup) y on x.memgroup = y.memgroup group by x.memgroup order by x.memgroup";
                    if (AppGlobalVariables.ConditionText.Length > 0)
                        AppGlobalVariables.ConditionText = AppGlobalVariables.ConditionText.Substring(1);
                    break;
                case 65:
                    AppGlobalVariables.ConditionText = "";
                    sql = " DROP TABLE IF EXISTS `report66`;";
                    sql += " CREATE TABLE `report66` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `hours` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `in` int(11) DEFAULT '0',";
                    sql += "  `out` int(11) DEFAULT '0',";
                    sql += "  `average` float(20,2) DEFAULT '0',";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    AppGlobalVariables.ConditionText += " วันที่ : " + startDate.ToString("dd/MM/yyyy") + " - " + endDate.ToString("dd/MM/yyyy");
                    if (memberGroup != Constants.TextBased.All)
                    {
                        AppGlobalVariables.ConditionText += " กลุ่ม : " + memberGroup;
                    }

                    for (int i = 10; i < 22; i++)
                    {
                        if (i == 10)
                        {
                            sql = "insert into report66 values (" + i + ", '8:30-" + i + ":59'";
                            sql += ", (select count(t1.no) from recordin t1 left join member_up2u t2 on t1.id = t2.cardid where t2.cardid is not null";
                            if (memberGroup != Constants.TextBased.All)
                            {
                                sql += " and t2.memgroup = '" + memberGroup + "'";
                            }
                            sql += " and t1.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(t1.datein) between 0 and " + i + ")";
                            sql += ", (select count(t1.no) from recordout t1 left join recordin t2 on t1.no = t2.no left join member_up2u t3 on t2.id = t3.cardid where t3.cardid is not null";
                            if (memberGroup != Constants.TextBased.All)
                            {
                                sql += " and t3.memgroup = '" + memberGroup + "'";
                            }
                            sql += " and t1.dateout between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(t1.dateout) between 0 and " + i + ")";
                            sql += ", (select concat(floor(avg(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i')))/60), '.', lpad(floor(mod(avg(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i'))), 60)), 2, '0'))";
                            sql += " from recordin t1 left join recordout t2 on t1.no = t2.no left join member_up2u t3 on t1.id = t3.cardid where t2.no is not null and t3.cardid is not null";
                            if (memberGroup != Constants.TextBased.All)
                            {
                                sql += " and t3.memgroup = '" + memberGroup + "'";
                            }
                            sql += " and t1.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(t1.datein) between 0 and " + i + ")";
                            sql += ")";
                        }
                        else if (i == 21)
                        {
                            sql = "insert into report66 values (" + i + ", '" + i + ":00-23:59'";
                            sql += ", (select count(t1.no) from recordin t1 left join member_up2u t2 on t1.id = t2.cardid where t2.cardid is not null";
                            if (memberGroup != Constants.TextBased.All)
                            {
                                sql += " and t2.memgroup = '" + memberGroup + "'";
                            }
                            sql += " and t1.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(t1.datein) between " + i + " and 23)";
                            sql += ", (select count(t1.no) from recordout t1 left join recordin t2 on t1.no = t2.no left join member_up2u t3 on t2.id = t3.cardid where t3.cardid is not null";
                            if (memberGroup != Constants.TextBased.All)
                            {
                                sql += " and t3.memgroup = '" + memberGroup + "'";
                            }
                            sql += " and t1.dateout between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(t1.dateout) between " + i + " and 23)";
                            sql += ", (select concat(floor(avg(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i')))/60), '.', lpad(floor(mod(avg(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i'))), 60)), 2, '0'))";
                            sql += " from recordin t1 left join recordout t2 on t1.no = t2.no left join member_up2u t3 on t1.id = t3.cardid where t2.no is not null and t3.cardid is not null";
                            if (memberGroup != Constants.TextBased.All)
                            {
                                sql += " and t3.memgroup = '" + memberGroup + "'";
                            }
                            sql += " and t1.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(t1.datein) between " + i + " and 23)";
                            sql += ")";
                        }
                        else
                        {
                            sql = "insert into report66 values (" + i + ", '" + i + ":00-" + i + ":59'";
                            sql += ", (select count(t1.no) from recordin t1 left join member_up2u t2 on t1.id = t2.cardid where t2.cardid is not null";
                            if (memberGroup != Constants.TextBased.All)
                            {
                                sql += " and t2.memgroup = '" + memberGroup + "'";
                            }
                            sql += " and t1.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(t1.datein) = " + i + ")";
                            sql += ", (select count(t1.no) from recordout t1 left join recordin t2 on t1.no = t2.no left join member_up2u t3 on t2.id = t3.cardid where t3.cardid is not null";
                            if (memberGroup != Constants.TextBased.All)
                            {
                                sql += " and t3.memgroup = '" + memberGroup + "'";
                            }
                            sql += " and t1.dateout between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(t1.dateout) = " + i + ")";
                            sql += ", (select concat(floor(avg(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i')))/60), '.', lpad(floor(mod(avg(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i'))), 60)), 2, '0'))";
                            sql += " from recordin t1 left join recordout t2 on t1.no = t2.no left join member_up2u t3 on t1.id = t3.cardid where t2.no is not null and t3.cardid is not null";
                            if (memberGroup != Constants.TextBased.All)
                            {
                                sql += " and t3.memgroup = '" + memberGroup + "'";
                            }
                            sql += " and t1.datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(t1.datein) = " + i + ")";
                            sql += ")";
                        }
                        DbController.SaveData(sql);
                    }

                    sql = "select `hours` as 'ช่วงเวลาเข้า', `in` as 'จำนวนคัน', `hours` as 'ช่วงเวลาออก', `out` as 'จำนวนคัน.', format(ifnull(`average`, 0), 2) as 'เฉลี่ยชั่วโมง' from report66";
                    sql += "";
                    if (AppGlobalVariables.ConditionText.Length > 0)
                        AppGlobalVariables.ConditionText = AppGlobalVariables.ConditionText.Substring(1);
                    break;
                case 66:
                    AppGlobalVariables.ConditionText = "";
                    AppGlobalVariables.ConditionText = "ตั้งแต่วันที่ " + startDate.ToString("d MMMM ") + (startDate.Year + 543) + " เวลา  " + startTime.ToLongTimeString() + " น. ถึง วันที่ " + endDate.ToString("d MMMM ") + (endDate.Year + 543) + " เวลา  " + endTime.ToLongTimeString() + " น.";
                    string fontSlip66 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip66 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName) //Mac 2017/09/11
                            fontSlip66 = "IV";
                    }

                    sql = "select CAST(t1.no AS char) as 'Time Slip No.', t2.posid as 'รหัสเครื่อง', date_format(t1.datein,'%d/%m/%Y') as 'วันที่'";
                    if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,4,'0'))) as 'เลขที่ใบกำกับภาษี'";
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(t2.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , concat('" + fontSlip66 + "', concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,4,'0'))) as 'เลขที่ใบกำกับภาษี'";
                            sql += " , concat('" + fontSlip66 + "', concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , concat('" + fontSlip66 + "', concat(date_format(t2.dateout,'%y') ,lpad(t2.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                        }
                    }

                    sql += ", t1.license as 'ทะเบียนรถ', concat(date_format(t1.datein,'%d/%m/%Y'), ' เวลา  ', date_format(t1.datein,'%H:%i:%s')) as 'วันเวลาเข้า'";
                    sql += ", concat(date_format(t2.dateout,'%d/%m/%Y'), ' เวลา  ', date_format(t2.dateout,'%H:%i:%s')) as 'วันเวลาออก'";
                    sql += ", concat(floor(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i:%s'), date_format(t2.dateout, '%Y-%m-%d %H:%i:%s'))/60)";
                    sql += ", '.', lpad(mod(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i:%s'), date_format(t2.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0')) as 'รวมเวลา'";
                    sql += ", case when t2.proid = 0 then '' else CAST(t2.proid AS char) end as 'ประเภทส่วนลด/E-Stamp'";
                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2018/12/10
                    {
                        sql += ", format((t2.price-t2.losscard) - ROUND((t2.price-t2.losscard)*7/107, 6), 2) as 'ค่าบริการที่จอดรถ'";
                        sql += ", format(ROUND((t2.price-t2.losscard)*7/107, 6), 2) as 'ภาษีมูลค่าเพิ่ม', format((t2.price-t2.losscard), 2) as 'รวม'";
                    }
                    else
                    {
                        sql += ", format(t2.price - ROUND(t2.price*7/107, 6), 2) as 'ค่าบริการที่จอดรถ'";
                        sql += ", format(ROUND(t2.price*7/107, 6), 2) as 'ภาษีมูลค่าเพิ่ม', format(t2.price, 2) as 'รวม'";
                    }

                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    sql += " where t2.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (carType == Constants.TextBased.Visitor)
                        sql += " AND t1.cartype != 200";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " AND t1.typeid =" + carTypeId;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t1.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t1.id = " + cardId;
                    if (promotionName != Constants.TextBased.All)
                        sql += " AND t2.proid =" + promotionId;
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t2.guardhouse = '" + guardhouse + "' ";

                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            sql += " order by concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,6,'0')))"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " order by concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(t2.printno,6,'0')))";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            sql += " order by concat('" + fontSlip66 + "', concat(date_format(dateout,'%y%m') ,lpad(printno,6,'0')))"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " order by concat('" + fontSlip66 + "', concat(date_format(dateout,'%y') ,lpad(printno,6,'0')))";
                        }
                    }
                    break;
                case 67:
                    AppGlobalVariables.ConditionText = "";
                    AppGlobalVariables.ConditionText = "เดือน....." + startDate.ToString("MMMM") + ".....พ.ศ. ....." + (endDate.Year + 543) + ".....";
                    if ((promotionName == null || promotionName == "ALL") && (memberGroupMonth == null || memberGroupMonth == "ALL"))
                        return "";

                    sql = " DROP TABLE IF EXISTS `report68`;";
                    sql += " CREATE TABLE `report68` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `no` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `date` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `namecard` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `license` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `datein` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `dateout` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `time` float(20,2) DEFAULT '0',";
                    sql += "  `beforevat` decimal(20,2) DEFAULT '0',";
                    sql += "  `vat` decimal(20,2) DEFAULT '0',";
                    sql += "  `total` int(11) DEFAULT '0',";
                    sql += "  `estamp` int(11) DEFAULT '0',";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    sql = "select t1.no as 'c1', date_format(t1.datein,'%d/%m/%Y') as 'c2'";
                    sql += ", case when t3.name_on_card = '' then CAST(t1.id AS char) when t3.name_on_card is null then CAST(t1.id AS char) else t3.name_on_card end as 'c3'";
                    sql += ", t1.license as 'c4'";
                    sql += ", date_format(t1.datein,'%d/%m/%Y %H:%i') as 'c5'";
                    sql += ", date_format(t2.dateout,'%d/%m/%Y %H:%i') as 'c6'";
                    sql += ", concat(floor(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i'))/60)";
                    sql += ", '.', lpad(mod(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i')), 60), 2, '0')) as 'c7'";
                    sql += ", t2.proid as 'c11'";
                    sql += ", timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i'), date_format(t2.dateout, '%Y-%m-%d %H:%i')) as c12";
                    sql += ", t1.datein as 'c13' , t2.dateout as 'c14'";
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    sql += " left join card";
                    if (Configs.UseMifare)
                        sql += "mf";
                    else
                        sql += "px";
                    sql += " t3 on t1.id = t3.name";

                    sql += " where t2.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and ";
                    if (memberGroupMonth != Constants.TextBased.All)
                    {
                        string sql67 = "select id";
                        sql67 += " from promotion";
                        sql67 += " where groupro = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        DataTable dt67t = DbController.LoadData(sql67);
                        if (dt67t != null && dt67t.Rows.Count > 0)
                        {
                            if (dt67t.Rows.Count == 1)
                            {
                                sql += " t2.proid = " + dt67t.Rows[0]["id"];
                            }
                            else
                            {
                                for (int i = 0; i < dt67t.Rows.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        sql += " (t2.proid = " + dt67t.Rows[i]["id"];
                                    }
                                    else
                                    {
                                        sql += " or t2.proid = " + dt67t.Rows[i]["id"];
                                    }
                                }
                                sql += ")";
                            }
                        }

                    }
                    else
                    {
                        sql += " t2.proid = " + promotionId;
                    }

                    sql += " order by t1.no";
                    DataTable dt67 = DbController.LoadData(sql);
                    if (dt67 != null && dt67.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt67.Rows.Count; i++)
                        {
                            sql = "select * from prosetprice where PromotionId = " + Convert.ToInt32(dt67.Rows[i]["c11"]);
                            DataTable dt67s = DbController.LoadData(sql);
                            if (dt67s != null && dt67s.Rows.Count > 0)
                            {
                                AppGlobalVariables.IntTime = new int[dt67s.Rows.Count];
                                AppGlobalVariables.IntPriceMin = new int[dt67s.Rows.Count];
                                AppGlobalVariables.IntPriceHour = new int[dt67s.Rows.Count];
                                AppGlobalVariables.IntHourRound = new int[dt67s.Rows.Count];
                                AppGlobalVariables.IntExpense = new int[dt67s.Rows.Count];
                                AppGlobalVariables.IntOver = new int[dt67s.Rows.Count];
                                for (int j = 0; j < dt67s.Rows.Count; j++)
                                {
                                    if (j == 0)
                                    {
                                        AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt67s.Rows[j].ItemArray[3].ToString());
                                    }
                                    else
                                    {
                                        AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt67s.Rows[j].ItemArray[3].ToString()) - Convert.ToInt32(dt67s.Rows[j - 1].ItemArray[3].ToString());
                                    }
                                    AppGlobalVariables.IntPriceMin[j] = Convert.ToInt32(dt67s.Rows[j].ItemArray[4].ToString());
                                    AppGlobalVariables.IntPriceHour[j] = Convert.ToInt32(dt67s.Rows[j].ItemArray[5].ToString());
                                    AppGlobalVariables.IntHourRound[j] = Convert.ToInt32(dt67s.Rows[j].ItemArray[6].ToString());
                                    AppGlobalVariables.IntExpense[j] = Convert.ToInt32(dt67s.Rows[j].ItemArray[7].ToString());
                                    AppGlobalVariables.IntOver[j] = Convert.ToInt32(dt67s.Rows[j].ItemArray[8].ToString());
                                }
                            }

                            double Price67 = 0;
                            if (dt67s != null && dt67s.Rows.Count > 0)
                            {
                                //--------------------------------- //Mac 2017/12/06
                                int ZoneMin = 0;
                                int intTotal = 0;
                                int intMin = 0;
                                DataTable dt3 = DbController.LoadData("select * from prosetprice_zone where PromotionID = " + Convert.ToInt32(dt67.Rows[i]["c11"]) + " order by no");
                                if (dt3 != null && dt3.Rows.Count > 0)
                                {
                                    AppGlobalVariables.IntTime2 = new int[dt3.Rows.Count];
                                    AppGlobalVariables.IntPriceMin2 = new int[dt3.Rows.Count];
                                    AppGlobalVariables.IntPriceHour2 = new int[dt3.Rows.Count];
                                    AppGlobalVariables.IntHourRound2 = new int[dt3.Rows.Count];
                                    AppGlobalVariables.IntExpense2 = new int[dt3.Rows.Count];
                                    AppGlobalVariables.IntOver2 = new int[dt3.Rows.Count];
                                    for (int y = 0; y < dt3.Rows.Count; y++)
                                    {
                                        if (y == 0)
                                        {
                                            AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt3.Rows[y].ItemArray[3].ToString());
                                        }
                                        else
                                        {
                                            AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt3.Rows[y].ItemArray[3].ToString()) - Convert.ToInt32(dt3.Rows[y - 1].ItemArray[3].ToString());
                                        }
                                        AppGlobalVariables.IntPriceMin2[y] = Convert.ToInt32(dt3.Rows[y].ItemArray[4].ToString());
                                        AppGlobalVariables.IntPriceHour2[y] = Convert.ToInt32(dt3.Rows[y].ItemArray[5].ToString());
                                        AppGlobalVariables.IntHourRound2[y] = Convert.ToInt32(dt3.Rows[y].ItemArray[6].ToString());
                                        AppGlobalVariables.IntExpense2[y] = Convert.ToInt32(dt3.Rows[y].ItemArray[7].ToString());
                                        AppGlobalVariables.IntOver2[y] = Convert.ToInt32(dt3.Rows[y].ItemArray[8].ToString());
                                    }
                                    string ZoneStart = dt3.Rows[0]["zone_start"].ToString();
                                    string ZoneStop = dt3.Rows[0]["zone_stop"].ToString();

                                    var calPriceZone = (dynamic)null;
                                    DateTime dti = DateTime.Parse(dt67.Rows[i]["c13"].ToString());
                                    DateTime dto = DateTime.Parse(dt67.Rows[i]["c14"].ToString());
                                    DateTime dtInOne;
                                    DateTime dtOutOne;
                                    TimeSpan diffInOut = DateTime.Parse(dto.ToShortDateString()) - DateTime.Parse(dti.ToShortDateString());

                                    bool isNoRounded = false; //Mac 2018/01/08
                                    isNoRounded = false; //Mac 2018/01/08
                                    for (int x = 0; x < diffInOut.Days + 1; x++)
                                    {
                                        if (diffInOut.Days == 0)
                                        {
                                            isNoRounded = true; //Mac 2018/01/08
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

                                        calPriceZone = CalculationsManager.CalPriceZoneOneDay(0, dtInOne.ToString(), dtOutOne.ToString(), ZoneStart, ZoneStop, 0, 0, 0, isNoRounded);
                                        ZoneMin += calPriceZone.Key;
                                    }
                                }
                                if (ZoneMin > 0)
                                {
                                    intMin = Convert.ToInt32(dt67.Rows[i]["c12"]) - ZoneMin;
                                    intTotal = CalculationsManager.CalPrice2(0, ZoneMin, Configs.UseNotDay);
                                    Price67 = CalculationsManager.CalPrice(0, intMin, Configs.UseNotDay) + intTotal;
                                }
                                else
                                    Price67 = CalculationsManager.CalPrice(0, Convert.ToInt32(dt67.Rows[i]["c12"]), Configs.UseNotDay);
                                //--------------------------------- //Mac 2017/12/06
                                //Price67 = CalculationsManager.CalPrice(0, Convert.ToInt32(dt67.Rows[i]["c12"]), Configs.UseNotDay);
                            }

                            sql = "insert into report68";
                            sql += " values (";
                            sql += i + 1;
                            sql += " ,'" + dt67.Rows[i]["c1"].ToString() + "'";
                            sql += " ,'" + dt67.Rows[i]["c2"].ToString() + "'";
                            sql += " ,'" + dt67.Rows[i]["c3"].ToString() + "'";
                            sql += " ,'" + dt67.Rows[i]["c4"].ToString() + "'";
                            sql += " ,'" + dt67.Rows[i]["c5"].ToString() + "'";
                            sql += " ,'" + dt67.Rows[i]["c6"].ToString() + "'";
                            sql += " ," + Convert.ToDouble(dt67.Rows[i]["c7"]);
                            sql += " ," + (Price67 - (Price67 * 7 / 107)).ToString("#.00");
                            sql += " ," + (Price67 * 7 / 107).ToString("#.00");
                            sql += " ," + Price67;
                            sql += " ," + Convert.ToInt32(dt67.Rows[i]["c11"]);
                            sql += ")";
                            DbController.SaveData(sql);
                        }
                    }

                    sql = "select no as 'Time Slip No.', Id as 'ลำดับที่', date as 'วันที่', namecard as 'เลขที่บัตร', license as 'ทะเบียนรถ'";
                    sql += " , datein as 'วันเวลาที่ทำรายการเข้า', dateout as 'วันเวลาที่ทำรายการออก', format(time, 2) as 'รวมเวลา', format(beforevat, 2) as 'ค่าบริการ'";
                    //sql += " , date_format(datein, '%d/%m/%Y %H:%i:%s') as 'วันเวลาที่ทำรายการเข้า', date_format(dateout, '%d/%m/%Y %H:%i:%s') as 'วันเวลาที่ทำรายการออก', format(time, 2) as 'รวมเวลา', format(beforevat, 2) as 'ค่าบริการ'"; //Mac 2018/12/21
                    sql += " , format(vat, 2) as 'ภาษีมูลค่าเพิ่ม', total as 'รวม', CAST(estamp AS char) as 'ประเภทส่วนลด/E-Stamp'";
                    sql += " from report68";
                    break;
                case 68:
                    AppGlobalVariables.ConditionText = "";
                    AppGlobalVariables.ConditionText = "ตั้งแต่วันที่ " + startDate.ToString("d MMMM yyyy") + " เวลา  " + startTime.ToLongTimeString() + " น. ถึง วันที่ " + endDate.ToString("d MMMM yyyy") + " เวลา  " + endTime.ToLongTimeString() + " น.";
                    string fontSlip68 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip68 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName) //Mac 2017/09/11
                            fontSlip68 = "IV";
                    }

                    sql = "select CAST(t1.no AS char) as 'Time Slip No.', date_format(t1.datein,'%d/%m/%Y') as 'วันที่'";
                    sql += ", t1.license as 'ทะเบียนรถ', date_format(t1.datein,'%d/%m/%Y %H:%i:%s') as 'เวลาเข้า'";
                    sql += ", date_format(t2.dateout,'%d/%m/%Y %H:%i:%s') as 'เวลาออก'";
                    sql += ", concat(floor(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i:%s'), date_format(t2.dateout, '%Y-%m-%d %H:%i:%s'))/60)";
                    sql += ", '.', lpad(mod(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i:%s'), date_format(t2.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0')) as 'รวมเวลา'";
                    sql += ", case when t2.proid = 0 then '' else CAST(t2.proid AS char) end as 'ประเภทส่วนลด/E-Stamp'";
                    sql += ", case when t3.name_on_card = '' then CAST(t1.id AS char) when t3.name_on_card is null then CAST(t1.id AS char) else t3.name_on_card end as 'หมายเลขบัตร'";
                    if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , case when t2.printno = 0 then '' else concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,4,'0'))) end as 'เลขที่ใบกำกับภาษี'";
                            sql += " , case when t2.printno = 0 then '' else concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,6,'0'))) end as 'เลขที่ใบกำกับภาษี'"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , case when t2.printno = 0 then '' else concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(t2.printno,6,'0'))) end as 'เลขที่ใบกำกับภาษี'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , case when t2.printno = 0 then '' else concat('" + fontSlip68 + "', concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,4,'0'))) end as 'เลขที่ใบกำกับภาษี'";
                            sql += " , case when t2.printno = 0 then '' else concat('" + fontSlip68 + "', concat(date_format(t2.dateout,'%y%m') ,lpad(t2.printno,6,'0'))) end as 'เลขที่ใบกำกับภาษี'"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , case when t2.printno = 0 then '' else concat('" + fontSlip68 + "', concat(date_format(t2.dateout,'%y') ,lpad(t2.printno,6,'0'))) end as 'เลขที่ใบกำกับภาษี'";
                        }
                    }
                    sql += ", (select name from user where id =  t2.userout) as 'พนักงาน'";
                    sql += ", case when t1.cartype = 200 then 'Member' else 'Visitor' end as 'หมายเหตุ'";
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    sql += " left join card";
                    if (Configs.UseMifare)
                        sql += "mf";
                    else
                        sql += "px";
                    sql += " t3 on t1.id = t3.name";

                    sql += " where t2.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (carType == Constants.TextBased.Visitor)
                        sql += " AND t1.cartype != 200";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " AND t1.typeid =" + carTypeId;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t1.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t1.id = " + cardId;
                    if (promotionName != Constants.TextBased.All)
                        sql += " AND t2.proid =" + promotionId;
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t2.guardhouse = '" + guardhouse + "' ";

                    sql += " and t2.no is not null";
                    sql += " order by t1.no";
                    break;
                case 69:
                    AppGlobalVariables.ConditionText = "";
                    AppGlobalVariables.ConditionText = "ตั้งแต่วันที่ " + startDate.ToString("d MMMM yyyy") + " เวลา  " + startTime.ToLongTimeString() + " น. ถึง วันที่ " + endDate.ToString("d MMMM yyyy") + " เวลา  " + endTime.ToLongTimeString() + " น.";
                    sql = "select CAST(t1.no AS char) as 'Time Slip No.', date_format(t1.datein,'%d/%m/%Y') as 'วันที่'";
                    sql += ", t1.license as 'ทะเบียนรถ', date_format(t1.datein,'%d/%m/%Y %H:%i:%s') as 'เวลาเข้า'";
                    sql += ", '' as 'เวลาออก', '' as 'รวมเวลา', '' as 'ประเภทส่วนลด/E-Stamp'";
                    sql += ", case when t3.name_on_card = '' then CAST(t1.id AS char) when t3.name_on_card is null then CAST(t1.id AS char) else t3.name_on_card end as 'หมายเลขบัตร'";
                    sql += ", case when t1.cartype = 200 then 'Member' else 'Visitor' end as 'หมายเหตุ'";
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    sql += " left join card";
                    if (Configs.UseMifare)
                        sql += "mf";
                    else
                        sql += "px";
                    sql += " t3 on t1.id = t3.name";

                    sql += " where t1.datein BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " and DATE_SUB(now(), INTERVAL 24 HOUR) >= t1.datein";

                    if (user != Constants.TextBased.All)
                        sql += " and t1.userin =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (carType == Constants.TextBased.Visitor)
                        sql += " AND t1.cartype != 200";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " AND t1.typeid =" + carTypeId;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t1.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t1.id = " + cardId;
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";

                    sql += " and t2.no is null";
                    sql += " order by t1.no";
                    break;
                case 70:
                    AppGlobalVariables.ConditionText = "";
                    AppGlobalVariables.ConditionText = "ตั้งแต่วันที่ : " + startDate.ToString("d MMMM ") + (startDate.Year + 543) + " เวลา  " + startTime.ToLongTimeString() + " น. ถึง " + endDate.ToString("d MMMM ") + (endDate.Year + 543) + " เวลา  " + endTime.ToLongTimeString() + " น.";
                    //strCondition = "ตั้งแต่วันที่ : ";
                    string fontSlip70 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip70 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName) //Mac 2017/09/11
                            fontSlip70 = "IV";
                    }

                    if (Configs.UseReceiptFor1Out) //Mac 2018/11/19
                    {
                        sql = "select aa as 'ว.ด.ป.', bb as 'รหัสเครื่อง', cc as 'เลขที่ใบกำกับภาษี', dd as 'ชื่อผู้รับบริการ'";
                        sql += ", ee as 'ค่าบริการ', ff as 'ภาษีมูลค่าเพิ่ม', gg as 'รวม' from ";
                        sql += "(select no, date_format(dateout,'%Y/%m/%d') as a, date_format(dateout,'%d/%m/%Y') as aa, posid as bb ";
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += ", concat(receipt, date_format(dateout,'%y%m'), lpad(printno,4,'0')) as cc";
                            sql += ", concat(receipt, date_format(dateout,'%y%m'), lpad(printno,6,'0')) as cc"; //Mac 2022/04/26
                        }
                        else
                            sql += ", concat(receipt, date_format(dateout,'%y'), lpad(printno,6,'0')) as cc";
                        sql += ", concat('ค่าบริการที่จอดรถวันที่',date_format(dateout,'%d/%m/%y')) as dd";
                        if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2018/12/10
                        {
                            sql += ", cast(((price-losscard) - ROUND((price-losscard)*7/107, 6)) as DECIMAL(10,2)) as ee";
                            sql += ", cast((ROUND((price-losscard)*7/107, 6)) as DECIMAL(10,2)) as ff, cast(((price-losscard)) as DECIMAL(10,2)) as gg, 'a' as hh";
                        }
                        else
                        {
                            sql += ", cast((price - ROUND(price*7/107, 6)) as DECIMAL(10,2)) as ee";
                            sql += ", cast((ROUND(price*7/107, 6)) as DECIMAL(10,2)) as ff, cast((price) as DECIMAL(10,2)) as gg, 'a' as hh";
                        }
                        sql += " from recordout where dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "' and printno > 0";
                        sql += " union";
                        sql += " select t2.no, date_format(t1.datevoid,'%Y/%m/%d') as a, date_format(t1.datevoid,'%d/%m/%Y') as aa, t2.posid as bb";
                        sql += ", concat('CN',lpad(t1.id,8,'0')) as cc";
                        if (Configs.OutReceiptNameMonth)
                        {
                            sql += ", concat('ยกเลิก ',t2.receipt, date_format(t2.dateout,'%y%m'), lpad(t2.printno,6,'0')) as dd"; //Mac 2022/04/26
                        }
                        else
                            sql += ", concat('ยกเลิก ',t2.receipt, date_format(t2.dateout,'%y'), lpad(t2.printno,6,'0')) as dd";

                        if (Configs.Reports.ReportPriceSplitLosscard)
                        {
                            sql += ", -cast(((t2.price-t2.losscard) - ROUND((t2.price-t2.losscard)*7/107, 6)) as DECIMAL(10,2)) as ee";
                            sql += ", -cast((ROUND((t2.price-t2.losscard)*7/107, 6)) as DECIMAL(10,2)) as ff, -cast(((t2.price-t2.losscard)) as DECIMAL(10,2)) as gg, 'b' as hh";
                        }
                        else
                        {
                            sql += ", -cast((t2.price - ROUND(t2.price*7/107, 6)) as DECIMAL(10,2)) as ee";
                            sql += ", -cast((ROUND(t2.price*7/107, 6)) as DECIMAL(10,2)) as ff, -cast((t2.price) as DECIMAL(10,2)) as gg, 'b' as hh";
                        }

                        sql += " from voidslip t1 left join recordout t2 on t1.no = t2.no";
                        sql += " where t1.datevoid BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                        sql += " union";
                        sql += " select id, date_format(datepay,'%Y/%m/%d') as a, date_format(datepay,'%d/%m/%Y') as aa, posid as bb ";
                        if (Configs.UseReceiptFor1Mem)
                            sql += ", concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6,'0'))) as cc";
                        else
                            sql += ", concat('IM', date_format(datepay, '%y'), lpad(printno, 6, '0')) as cc";
                        sql += ", concat('ค่าสมาชิกรายเดือนวันที่',date_format(datepay,'%d/%m/%y')) as dd";
                        sql += ", cast((price - ROUND(price*7/107, 6)) as DECIMAL(10,2)) as ee";
                        sql += ", cast((ROUND(price*7/107, 6)) as DECIMAL(10,2)) as ff, cast((price) as DECIMAL(10,2)) as gg, 'c' as hh";
                        sql += " from member_record";
                        sql += " where datepay BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "' and printno > 0";
                        sql += ") tt";
                        sql += " order by a, hh, cc";
                    }
                    else
                    {
                        sql = "select aa as 'ว.ด.ป.', bb as 'รหัสเครื่อง', cc as 'เลขที่ใบกำกับภาษี', dd as 'ชื่อผู้รับบริการ'";
                        sql += ", ee as 'ค่าบริการ', ff as 'ภาษีมูลค่าเพิ่ม', gg as 'รวม' from ";

                        sql += " (select date_format(dateout,'%Y/%m/%d') as a, date_format(dateout,'%d/%m/%Y') as aa, posid as bb";

                        if (Configs.OutReceiptNameMonth)
                        {
                            sql += " , concat('" + fontSlip70 + "', concat(date_format(dateout,'%y%m') ,lpad(printno,6,'0'))) as cc"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , concat('" + fontSlip70 + "', concat(date_format(dateout,'%y') ,lpad(printno,6,'0'))) as cc";
                        }

                        sql += ", 'ค่าบริการที่จอดรถ' as dd";
                        if (Configs.Reports.ReportPriceSplitLosscard)
                        {
                            sql += ", format((price-losscard) - ROUND((price-losscard)*7/107, 6), 2) as ee";
                            sql += ", format(ROUND((price-losscard)*7/107, 6), 2) as ff, format((price-losscard), 2) as gg";
                        }
                        else
                        {
                            sql += ", format(price - ROUND(price*7/107, 6), 2) as ee";
                            sql += ", format(ROUND(price*7/107, 6), 2) as ff, format(price, 2) as gg";
                        }
                        sql += ", 'a' as hh";
                        sql += " from recordout";
                        sql += " where dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                        sql += " and printno > 0";

                        if (Configs.UseVoidSlip)
                            sql += " and status = 'N'";

                        sql += " union ";

                        sql += "select date_format(datepay,'%Y/%m/%d') as a, date_format(datepay,'%d/%m/%Y') as aa, posid as bb";
                        if (Configs.UseReceiptFor1Mem)
                            sql += ", concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6,'0'))) as cc";
                        else
                            sql += ", concat('IM', date_format(datepay, '%y'), lpad(printno, 6, '0')) as cc";

                        sql += ", 'ค่าสมาชิกรายเดือน' as dd";
                        sql += ", format(price - ROUND(price*7/107, 6), 2) as ee";
                        sql += ", format(ROUND(price*7/107, 6), 2) as ff, format(price, 2) as gg, 'b' as hh";
                        sql += " from member_record";
                        sql += " where datepay BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "' and printno > 0";
                        sql += ") tt";
                        sql += " order by a, hh, cc";
                    }
                    break;
                case 71:
                    AppGlobalVariables.ConditionText = "";
                    if (Configs.Reports.UseReport72_1)
                        AppGlobalVariables.ConditionText = "ตั้งแต่วันที่ : 1 " + startDate.ToString("MMMM ") + (startDate.Year + 543) + " เวลา  0:00:00 น. ถึง " + lastDayOfMonth.ToString("dd") + startDate.ToString(" MMMM ") + (endDate.Year + 543) + " เวลา  23:59:59 น.";
                    else
                        AppGlobalVariables.ConditionText = "ตั้งแต่วันที่ : 1 " + startDate.ToString("MMMM ") + (startDate.Year + 543) + " ถึง " + lastDayOfMonth.ToString("dd") + startDate.ToString(" MMMM ") + (endDate.Year + 543);

                    AppGlobalVariables.ConditionText = "ตั้งแต่วันที่ : " + startDate.ToString("d MMMM ") + (startDate.Year + 543) + " เวลา  " + startTime.ToLongTimeString() + " น. ถึง " + endDate.ToString("d MMMM ") + (endDate.Year + 543) + " เวลา  " + endTime.ToLongTimeString() + " น.";
                    string fontSlip71 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip71 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName) //Mac 2017/09/11
                            fontSlip71 = "IV";
                    }

                    if (Configs.UseReceiptFor1Out)
                    {
                        sql = " DROP TABLE IF EXISTS `report72`;";
                        sql += " CREATE TABLE `report72` (";
                        sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                        sql += "  `dateslip` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                        sql += "  `posid` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                        sql += "  `slip` varchar(100) CHARACTER SET utf8 DEFAULT NULL,";
                        sql += "  `detail` varchar(200) CHARACTER SET utf8 DEFAULT NULL,";
                        sql += "  `beforevat` decimal(20,2) DEFAULT '0',";
                        sql += "  `vat` decimal(20,2) DEFAULT '0',";
                        sql += "  `total` int(11) DEFAULT '0',";
                        sql += "  `type` varchar(10) CHARACTER SET utf8 DEFAULT NULL,";
                        sql += "  `receipt` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                        sql += "  PRIMARY KEY (`Id`)";
                        sql += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                        DbController.LoadData(sql);

                        sql = "insert into `report72` (`dateslip`, `posid`, `slip`, `detail`, `beforevat`, `vat`, `total`, `type`, `receipt`)";

                        sql += "select aa as 'ว.ด.ป.', bb as 'รหัสเครื่อง', cc as 'เลขที่ใบกำกับภาษี', dd as 'ชื่อผู้รับบริการ'";
                        sql += ", ee as 'ค่าบริการ', ff as 'ภาษีมูลค่าเพิ่ม', gg as 'รวม', hh, ii from ";
                        sql += "(select no, date_format(dateout,'%d/%m/%Y') as aa, posid as bb ";
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += ", concat(receipt, date_format(dateout,'%y%m'), lpad(printno,4,'0')) as cc";
                            sql += ", concat(receipt, date_format(dateout,'%y%m'), lpad(printno,6,'0')) as cc"; //Mac 2022/04/26
                        }
                        else
                            sql += ", concat(receipt, date_format(dateout,'%y'), lpad(printno,6,'0')) as cc";
                        sql += ", concat('ค่าบริการที่จอดรถวันที่',date_format(dateout,'%d/%m/%y')) as dd";
                        if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2018/12/10
                        {
                            sql += ", cast(((price-losscard) - ROUND((price-losscard)*7/107, 6)) as DECIMAL(10,2)) as ee";
                            sql += ", cast((ROUND((price-losscard)*7/107, 6)) as DECIMAL(10,2)) as ff, cast(((price-losscard)) as DECIMAL(10,2)) as gg, 'a' as hh, receipt as ii";
                        }
                        else
                        {
                            sql += ", cast((price - ROUND(price*7/107, 6)) as DECIMAL(10,2)) as ee";
                            sql += ", cast((ROUND(price*7/107, 6)) as DECIMAL(10,2)) as ff, cast((price) as DECIMAL(10,2)) as gg, 'a' as hh, receipt as ii";
                        }
                        sql += " from recordout where dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "' and printno > 0";
                        sql += " union";
                        sql += " select t2.no, date_format(t1.datevoid,'%d/%m/%Y') as aa, t2.posid as bb";
                        sql += ", concat('CN',lpad(t1.id,8,'0')) as cc";
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += ", concat('ยกเลิก ',t2.receipt, date_format(t2.dateout,'%y%m'), lpad(t2.printno,4,'0')) as dd";
                            sql += ", concat('ยกเลิก ',t2.receipt, date_format(t2.dateout,'%y%m'), lpad(t2.printno,6,'0')) as dd"; //Mac 2022/04/26
                        }
                        else
                            sql += ", concat('ยกเลิก ',t2.receipt, date_format(t2.dateout,'%y'), lpad(t2.printno,6,'0')) as dd";

                        if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2018/12/10
                        {
                            sql += ", -cast(((t2.price-t2.losscard) - ROUND((t2.price-t2.losscard)*7/107, 6)) as DECIMAL(10,2)) as ee";
                            sql += ", -cast((ROUND((t2.price-t2.losscard)*7/107, 6)) as DECIMAL(10,2)) as ff, -cast(((t2.price-t2.losscard)) as DECIMAL(10,2)) as gg, 'b' as hh, t2.receipt as ii";
                        }
                        else
                        {
                            sql += ", -cast((t2.price - ROUND(t2.price*7/107, 6)) as DECIMAL(10,2)) as ee";
                            sql += ", -cast((ROUND(t2.price*7/107, 6)) as DECIMAL(10,2)) as ff, -cast((t2.price) as DECIMAL(10,2)) as gg, 'b' as hh, t2.receipt as ii";
                        }
                        sql += " from voidslip t1 left join recordout t2 on t1.no = t2.no";
                        sql += " where t1.datevoid BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                        //Mac 2020/08/14-------
                        sql += " union";
                        sql += " select id, date_format(datepay,'%d/%m/%Y') as aa, posid as bb ";
                        if (Configs.UseReceiptFor1Mem)
                            sql += ", concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6,'0'))) as cc";
                        else
                            sql += ", concat('IM', date_format(datepay, '%y'), lpad(printno, 6, '0')) as cc";
                        sql += ", concat('ค่าสมาชิกรายเดือนวันที่',date_format(datepay,'%d/%m/%y')) as dd";
                        sql += ", cast((price - ROUND(price*7/107, 6)) as DECIMAL(10,2)) as ee";
                        sql += ", cast((ROUND(price*7/107, 6)) as DECIMAL(10,2)) as ff, cast((price) as DECIMAL(10,2)) as gg, 'c' as hh, receipt as ii";
                        sql += " from member_record";
                        sql += " where datepay BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "' and printno > 0";
                        //---------------------
                        sql += ") tt";
                        sql += " order by aa, hh, cc";

                        DbController.SaveData(sql);

                        sql = "select `dateslip` as 'ว.ด.ป.', `posid` as 'รหัสเครื่อง'";
                        sql += ", case when `type` = 'a' then ";
                        sql += " concat(min(`slip`), '-', max(`slip`))";
                        //Mac 2020/08/14-------
                        sql += " when `type` = 'c' then ";
                        sql += " concat(min(`slip`), '-', max(`slip`))";
                        //---------------------
                        sql += " else `slip` end as 'เลขที่ใบกำกับภาษี'";
                        sql += ", `detail` as 'ชื่อผู้รับบริการ'";
                        sql += ", sum(`beforevat`) as 'ค่าบริการ', sum(`vat`) as 'ภาษีมูลค่าเพิ่ม', sum(`total`) as 'รวม' from `report72`";
                        sql += " group by `dateslip`, `detail`, `posid`, `type`, `receipt`";
                        sql += " order by `dateslip`, `type`, concat(min(`slip`), '-', receipt, max(`slip`))";
                    }
                    else
                    {
                        sql = "select aa as 'ว.ด.ป.', bb as 'รหัสเครื่อง', cc as 'เลขที่ใบกำกับภาษี', dd as 'ชื่อผู้รับบริการ'";
                        sql += ", ee as 'ค่าบริการ', ff as 'ภาษีมูลค่าเพิ่ม', gg as 'รวม' from ";

                        sql += "(select date_format(dateout,'%d/%m/%Y') as aa, posid as bb ";

                        if (Configs.OutReceiptNameMonth)
                        {
                            sql += " , concat('" + fontSlip71 + "', concat(date_format(dateout,'%y%m') ,lpad(min(printno),6,'0')) ";
                            sql += " ,'-', '" + fontSlip71 + "', concat(date_format(dateout,'%y%m') ,lpad(max(printno),6,'0'))) as cc";
                        }
                        else
                        {
                            sql += " , concat('" + fontSlip71 + "', concat(date_format(dateout,'%y') ,lpad(min(printno),6,'0')) ";
                            sql += " ,'-', '" + fontSlip71 + "', concat(date_format(dateout,'%y') ,lpad(max(printno),6,'0'))) as cc";
                        }

                        sql += ", 'ค่าบริการที่จอดรถ' as dd";
                        if (Configs.Reports.ReportPriceSplitLosscard)
                        {
                            sql += ", format(sum((price-losscard)) - ROUND(sum((price-losscard))*7/107, 6), 2) as ee";
                            sql += ", format(ROUND(sum((price-losscard))*7/107, 6), 2) as ff, format(sum((price-losscard)), 2) as gg";
                        }
                        else
                        {
                            sql += ", format(sum(price) - ROUND(sum(price)*7/107, 6), 2) as ee";
                            sql += ", format(ROUND(sum(price)*7/107, 6), 2) as ff, format(sum(price), 2) as gg";
                        }
                        sql += ", 'a' as hh";
                        sql += " from recordout";
                        sql += " where dateout between '" + firstDayOfMonth.Year.ToString() + "-" + firstDayOfMonth.ToString("MM'-'dd") + " 00:00:00' and '" + lastDayOfMonthEnd.Year.ToString() + "-" + lastDayOfMonthEnd.ToString("MM'-'dd") + " 23:59:59'";
                        sql += " and printno > 0";

                        if (Configs.UseVoidSlip)
                            sql += " and status = 'N'";

                        sql += " group by date_format(dateout,'%d/%m/%Y'), posid";

                        sql += " union ";
                        sql += " select date_format(datepay,'%d/%m/%Y') as aa, posid as bb ";
                        if (Configs.UseReceiptFor1Mem)
                        {
                            sql += ", concat(concat(receipt, concat(date_format(datepay, '%y'), lpad(min(printno), 6,'0'))) ";
                            sql += ", '-', concat(receipt, concat(date_format(datepay, '%y'), lpad(max(printno), 6,'0')))) as cc";
                        }
                        else
                        {
                            sql += ", concat(concat('IM', date_format(datepay, '%y'), lpad(min(printno), 6, '0')) ";
                            sql += ", '-', concat('IM', date_format(datepay, '%y'), lpad(max(printno), 6, '0'))) as cc";
                        }
                        sql += ", 'ค่าสมาชิกรายเดือน' as dd";
                        sql += ", format(sum(price) - ROUND(sum(price)*7/107, 6), 2) as ee";
                        sql += ", format(ROUND(sum(price)*7/107, 6), 2) as ff, format(sum(price), 2) as gg, 'b' as hh";

                        sql += " from member_record";
                        sql += " where datepay between '" + firstDayOfMonth.Year.ToString() + "-" + firstDayOfMonth.ToString("MM'-'dd") + " 00:00:00' and '" + lastDayOfMonthEnd.Year.ToString() + "-" + lastDayOfMonthEnd.ToString("MM'-'dd") + " 23:59:59' and printno > 0";
                        sql += " group by date_format(datepay,'%d/%m/%Y'), posid";
                        sql += ") tt";
                        sql += " order by aa, hh, cc";
                    }
                    break;
                case 72:
                    sql = "select t1.license as 'ทะเบียน', date_format(t1.datein, '%d/%m/%Y %H:%i:%s') as 'เวลาเข้า', date_format(t2.dateout, '%d/%m/%Y %H:%i:%s') as 'เวลาออก'"; //Mac 2018/12/21
                    sql += ",concat(floor(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i:%s'), date_format(t2.dateout, '%Y-%m-%d %H:%i:%s'))/60)";
                    sql += ", '.', lpad(mod(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i:%s'), date_format(t2.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0')) as 'ชม.จอด'";
                    sql += ", (select name from user where id =  t1.userin) as 'เจ้าหน้าที่ขาเข้า'";
                    sql += ", (select name from user where id =  t2.userout) as 'เจ้าหน้าที่ขาออก'";
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    sql += " where t2.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " and t1.proid = 0 and t2.proid = 0 and LENGTH(trim(t2.clearcard)) = 0";
                    sql += " and TIMESTAMPDIFF(second,t1.datein,t2.dateout) <= 959 and t2.price = 0 and t1.cartype = 0";
                    if (user != Constants.TextBased.All)
                        sql += " AND t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t1.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t1.id = " + cardId;
                    sql += " order by t1.no";
                    break;
                case 73:
                    sql = " DROP TABLE IF EXISTS `report74`;";
                    sql += " CREATE TABLE `report74` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `hours` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `sumpro` int(11) DEFAULT '0',";
                    sql += "  `namepro` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `status` varchar(10) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ")ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    int j73 = 1;
                    for (int i = 0; i < 24; i++)
                    {
                        sql = "insert into report74 values (" + j73 + ", '" + i.ToString("00") + ":00-" + i.ToString("00") + ":59'";
                        sql += ", (select count(no) from recordin where cartype < 200";
                        sql += " and datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(datein) = " + i + ")";
                        sql += ", 'ลูกค้า', 'เข้า'";
                        sql += ")";
                        j73++;
                        DbController.LoadData(sql);

                        sql = "insert into report74 values (" + j73 + ", '" + i.ToString("00") + ":00-" + i.ToString("00") + ":59'";
                        sql += ", (select count(no) from recordin where cartype = 200";
                        sql += " and datein between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(datein) = " + i + ")";
                        sql += ", 'สมาชิก', 'เข้า'";
                        sql += ")";
                        j73++;
                        DbController.LoadData(sql);

                        sql = "insert into report74 values (" + j73 + ", '" + i.ToString("00") + ":00-" + i.ToString("00") + ":59'";
                        sql += ", (select count(no) from recordout where proid = 0 and LENGTH(trim(clearcard)) = 0 ";
                        sql += " and dateout between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(dateout) = " + i + ")";
                        sql += ", ' ไม่ประทับตรา', 'ออก'";
                        sql += ")";
                        j73++;
                        DbController.LoadData(sql);

                        foreach (KeyValuePair<int, string> entry in AppGlobalVariables.PromotionNamesById)
                        {
                            sql = "insert into report74 values (" + j73 + ", '" + i.ToString("00") + ":00-" + i.ToString("00") + ":59'";
                            sql += ", (select count(no) from recordout where proid = " + entry.Key + " and LENGTH(trim(clearcard)) = 0 ";
                            sql += " and dateout between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59' and hour(dateout) = " + i + ")";
                            sql += ", '" + entry.Value + "', 'ออก'";
                            sql += ")";
                            j73++;
                            DbController.LoadData(sql);
                        }
                    }

                    sql = "select status,namepro, hours,sumpro from report74 order by id";
                    break;
                case 74:
                    sql = "select aa as 'Promotion', bb as 'ยอดรวม' from";
                    sql += " (select (case when TIMESTAMPDIFF(second,t2.datein,t1.dateout) <= 959 THEN 'ฟรี 15 นาที' ELSE 'คิดเงิน' end) as 'aa', count(t2.cartype) as 'bb'";
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where LENGTH(trim(t1.clearcard)) = 0 and t2.cartype = 0 and t1.proid = 0 and t2.proid = 0 ";
                    sql += " and t1.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " group by TIMESTAMPDIFF(second,t2.datein,t1.dateout) <= 959";
                    sql += " UNION";
                    sql += " select (select typename from cartype where typeid = t2.cartype) as 'aa', count(t2.cartype) as 'bb'";
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where LENGTH(trim(t1.clearcard)) = 0 and t2.cartype < 200 and t2.cartype > 0";
                    sql += " and t1.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " group by t2.cartype) t";
                    sql += " order by aa";
                    break;
                case 75:
                    string voidSlip75 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        voidSlip75 = AppGlobalVariables.Printings.ReceiptNameVoidPay;

                    sql = "select concat('" + voidSlip75 + "', concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'เลขที่ใบเสร็จ/ใบกำกับภาษี'";

                    string fontSlip75 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip75 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName) //Mac 2017/09/11
                            fontSlip75 = "IV";
                    }
                    sql += " , case when t2.cartype = 200 then t3.typeid ";
                    sql += " else case when TIMESTAMPDIFF(second,t2.datein,t1.dateout) <= 959 then 'ฟรี 15 นาที' else (select typename from cartype where typeid = t2.cartype) end end as 'ประเภท'";
                    sql += " , t2.license as ทะเบียน, concat(date_format(t2.datein,'%d/%m/'), date_format(t2.datein,'%Y') + 543,date_format(t2.datein,' %H:%i:%s')) as เวลาเข้า";
                    sql += " , (select name from user where id = t1.userout) AS 'เจ้าหน้าที่ขาออก'";
                    sql += " , concat(date_format(t1.dateout,'%d/%m/'), date_format(t1.dateout,'%Y') + 543,date_format(t1.dateout,' %H:%i:%s')) as เวลาออก";
                    sql += " , (select name from user where id = t1.userapprove) AS 'Approve'";
                    if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , concat(t4.receipt, concat(date_format(t4.dateout,'%y%m') ,lpad(t4.printno,4,'0'))) as 'อ้างถึงเลขใบเสร็จ'";
                            sql += " , concat(t4.receipt, concat(date_format(t4.dateout,'%y%m') ,lpad(t4.printno,6,'0'))) as 'อ้างถึงเลขใบเสร็จ'"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , concat(t4.receipt, concat(date_format(t4.dateout,'%y') ,lpad(t4.printno,6,'0'))) as 'อ้างถึงเลขใบเสร็จ'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            //sql += " , concat('" + fontSlip75 + "', concat(date_format(t4.dateout,'%y%m') ,lpad(t4.printno,4,'0'))) as 'อ้างถึงเลขใบเสร็จ'";
                            sql += " , concat('" + fontSlip75 + "', concat(date_format(t4.dateout,'%y%m') ,lpad(t4.printno,6,'0'))) as 'อ้างถึงเลขใบเสร็จ'"; //Mac 2022/04/26
                        }
                        else
                        {
                            sql += " , concat('" + fontSlip75 + "', concat(date_format(t4.dateout,'%y') ,lpad(t4.printno,6,'0'))) as 'อ้างถึงเลขใบเสร็จ'";
                        }
                    }
                    sql += " , (t4.price - t1.price) as จำนวนเงิน";
                    sql += " from recordoutvoidpay t1 left join recordin t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2018/09/03
                        //sql += " left join member t3 on t2.license = t3.license";
                        sql += " left join member t3 on t3.license like concat('%',t2.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t2.id = t3.cardid";

                    sql += " left join recordout t4 on t1.no = t4.no";
                    sql += " where t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " order by t1.no";
                    break;
                case 76:
                case 77:
                    sql = "select no as ลำดับ, license as ทะเบียน, picdiv, piclic, date as วันที่";
                    sql += ", case when gate = 'I' then 'ขาเข้า' when gate = 'O' then 'ขาออก' when gate = 'B' then 'ขาเข้า/ขาออก' end as ประตู";
                    sql += ", guardhouse as 'ชื่อจุดผ่าน', no_recordin as 'ลำดับทางเข้าหลัก'";
                    sql += " from recordguardhouse";
                    sql += " WHERE date BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND id = " + cardId;
                    if (recordNumber != "")
                        sql += " AND no_recordin = " + recordNumber;

                    sql += " ORDER BY no";

                    break;
                case 78:
                    sql = "";
                    sql = " DROP TABLE IF EXISTS `sumprice`;";
                    sql += " CREATE TABLE `sumprice` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `datesum` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,";
                    sql += "  `sumprice` int(11) DEFAULT '0',";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    string datesum78 = "";
                    int sumprice78 = 0;
                    int id78 = 0;

                    for (var day = startDate.Date; day.Date <= startDate.Date; day = day.AddDays(1))
                    {
                        id78++;
                        datesum78 = (Convert.ToInt32(day.Year) + day.ToString("-MM-dd"));
                        sql = "select date_format(dateout,'%Y-%m-%d')";
                        sql += ", sum(price)";
                        sql += " from recordout";
                        sql += " where date(dateout) = '" + day.Year.ToString() + "-" + day.ToString("MM'-'dd") + "'";
                        sql += " group by date(dateout)";
                        sql += " order by date(dateout)";

                        dTable = DbController.LoadData(sql);
                        if (dTable != null && dTable.Rows.Count > 0)
                        {
                            //datesum78 = dTable.Rows[0].ItemArray[0].ToString();
                            sumprice78 = Convert.ToInt32(dTable.Rows[0].ItemArray[1]);
                        }
                        else
                        {
                            sumprice78 = 0;
                        }

                        sql = "insert into sumprice (id,datesum,sumprice)";
                        sql += " values (";
                        sql += id78;
                        sql += " ,'" + datesum78 + "'";
                        sql += " ," + sumprice78;
                        sql += ")";
                        DbController.SaveData(sql);
                    }
                    sql = "select id as ลำดับ";
                    sql += " ,datesum as 'วันที่'";
                    sql += " ,sumprice as 'รวมเงิน'";
                    sql += " from sumprice";

                    break;
                case 79:
                    sql = "select t1.no as ลำดับ, date_format(t2.datein, '%d/%m/%Y %H:%i:%s') as 'วันที่ - เวลาเข้า', date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วันที่ - เวลาออก'"; //Mac 2018/12/21
                    sql += ", concat(floor(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s'))/60)";
                    sql += ", '.', lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0')) as 'เวลาจอด'";
                    sql += ", t2.license as ทะเบียนรถ, cast(t1.proid as char) as รหัสตราประทับ, t1.discount as มูลค่าคูปอง, t1.price as จ่ายเงินสด";
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where t1.proid > 0 and t1.discount > 0";
                    sql += " and t1.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t2.license LIKE '%" + licensePlate + "%'";
                    if (promotionName != Constants.TextBased.All)
                        sql += " AND t1.proid =" + promotionId;
                    sql += " order by t1.dateout";

                    break;
                case 80:
                    if (Configs.UseMemberHourBalance)
                    {
                        sql = "SELECT no AS 'ลำดับ', case when id = 0 then (SELECT name FROM member WHERE member.license = recordmember_hourbalance.license limit 1) else (SELECT name FROM member WHERE member.cardid = recordmember_hourbalance.id limit 1) end AS 'ชื่อ - นามสกุล', license AS 'ทะเบียนรถ', guardhouse AS 'ป้อม', date AS 'วันที่' ";
                        sql += " , (SELECT name FROM user WHERE id = user) AS เจ้าหน้าที่, increase_hour AS 'เพิ่ม', decrease_hour AS 'ลด' FROM recordmember_hourbalance ";
                    }
                    else
                    {
                        sql = "SELECT no AS 'ลำดับ', case when id = 0 then (SELECT name FROM member WHERE member.license = recordmember_cashbalance.license limit 1) else (SELECT name FROM member WHERE member.cardid = recordmember_cashbalance.id limit 1) end AS 'ชื่อ - นามสกุล', license AS 'ทะเบียนรถ', guardhouse AS 'ป้อม', date AS 'วันที่' ";
                        sql += " , (SELECT name FROM user WHERE id = user) AS เจ้าหน้าที่, increase_cash AS 'เพิ่ม', decrease_cash AS 'ลด' FROM recordmember_cashbalance ";
                    }

                    sql += " WHERE date BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (user != Constants.TextBased.All)
                        sql += " AND user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND license LIKE '%" + licensePlate + "%'";

                    sql += " ORDER BY no";

                    break;
                case 81:
                    AppGlobalVariables.ConditionText = "";
                    sql = " DROP TABLE IF EXISTS `report82`;";
                    sql += " CREATE TABLE `report82` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `userout` int(11) DEFAULT '0',";
                    sql += "  `userno` int(11) DEFAULT '0',";
                    sql += "  `beforevat` decimal(20,2) DEFAULT '0',";
                    sql += "  `vat` decimal(20,2) DEFAULT '0',";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    sql = "insert into `report82` (`userout`, `userno`, `beforevat`, `vat`)";
                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2019/08/27
                        sql += "select userout, userno, cast((price - ROUND((price-losscard)*7/107, 6)) as DECIMAL(10,2)), cast((ROUND((price-losscard)*7/107, 6)) as DECIMAL(10,2))";
                    else
                        sql += "select userout, userno, cast((price - ROUND(price*7/107, 6)) as DECIMAL(10,2)), cast((ROUND(price*7/107, 6)) as DECIMAL(10,2))";
                    sql += " from recordout";
                    sql += " where dateout >= '" + startDateTimeText + "'";
                    DbController.SaveData(sql);

                    sql = "select t1.guardhouse as 'ตำแหน่งป้อม', (select name from user where id = t1.id) as 'ชื่อเจ้าหน้าที่'";
                    //sql += ", t1.datein as 'วันที่เวลาเข้า', t1.dateout as 'วันที่เวลาออก', (select count(t2.no) from recordout t2 left join recordin t3 on t2.no = t3.no where t3.cartype = 200 and t2.userout = t1.id and t2.userno = t1.no) as 'Member'";
                    sql += ", date_format(t1.datein, '%d/%m/%Y %H:%i:%s') as 'วันที่เวลาเข้า', date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วันที่เวลาออก', (select count(t2.no) from recordout t2 left join recordin t3 on t2.no = t3.no where t3.cartype = 200 and t2.userout = t1.id and t2.userno = t1.no) as 'Member'"; //Mac 2018/12/21
                    sql += ", (select count(t2.no) from recordout t2 left join recordin t3 on t2.no = t3.no where t3.cartype < 200 and t2.userout = t1.id and t2.userno = t1.no) as 'Visitor'";
                    sql += ", (select count(t2.no) from recordout t2 where proid > 0 and t2.userout = t1.id and t2.userno = t1.no) as 'E-Stamp'";
                    sql += ", (select count(*) from liftrecord where gate = 'O' and userid = t1.id and datelift >= t1.datein and datelift <= t1.dateout) as 'ยกไม้ฉุกเฉิน(ครั้ง)'";
                    sql += ", ifnull((select sum(t2.price) - sum(t2.losscard) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ค่าจอด'";
                    sql += ", ifnull((select sum(t2.losscard) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ค่าปรับ'";
                    sql += ", ifnull((select sum(t2.price) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ยอดรวมจากระบบ(บาท)'";
                    sql += ", t1.sendmoney as 'ยอดรวมเงินนำส่ง(บาท)', (t1.sendmoney) - ifnull((select sum(t2.price) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ส่วนต่าง'";
                    sql += ", ifnull((select sum(beforevat) from report82 where userno = t1.no and userout = t1.id), 0) as 'ค่าบริการ'";
                    sql += ", ifnull((select sum(vat) from report82 where userno = t1.no and userout = t1.id), 0) as 'ภาษี'";
                    sql += " from user_record t1";
                    /*sql += " where t1.dateout is not null and t1.guardhouse is not null and t1.guardhouse <> ''";
                    sql += " and t1.datein BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "'"; */
                    sql += " where t1.datein BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "' and t1.guardhouse is not null and t1.guardhouse <> '' and upper(t1.guardhouse) <> 'SERVER'";
                    sql += " order by t1.guardhouse, t1.datein,(select name from user where id = t1.id)";

                    AppGlobalVariables.ConditionText = " จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;
                case 82:
                    sql = "";
                    sql = " DROP TABLE IF EXISTS `sumc`;";
                    sql += " CREATE TABLE `sumc` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `datesum` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,";
                    sql += "  `sumc` int(11) DEFAULT '0',";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    string datesum82 = "";
                    int sumc82 = 0;
                    int id82 = 0;

                    for (var day = startDate.Date; day.Date <= endDate.Date; day = day.AddDays(1))
                    {
                        id82++;
                        datesum82 = (Convert.ToInt32(day.Year) + day.ToString("-MM-dd"));
                        sql = "select date_format(t1.datein,'%Y-%m-%d')";
                        sql += ", count(t1.no)";
                        sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                        sql += " where t2.no is null and date(t1.datein) = '" + day.Year.ToString() + "-" + day.ToString("MM'-'dd") + "'";
                        sql += " group by date(t1.datein)";
                        sql += " order by date(t1.datein)";

                        dTable = DbController.LoadData(sql);
                        if (dTable != null && dTable.Rows.Count > 0)
                        {
                            sumc82 = Convert.ToInt32(dTable.Rows[0].ItemArray[1]);
                        }
                        else
                        {
                            sumc82 = 0;
                        }

                        sql = "insert into sumc (id,datesum,sumc)";
                        sql += " values (";
                        sql += id82;
                        sql += " ,'" + datesum82 + "'";
                        sql += " ," + sumc82;
                        sql += ")";
                        DbController.SaveData(sql);
                    }
                    sql = "select id as ลำดับ";
                    sql += " ,datesum as 'วันที่'";
                    sql += " ,sumc as 'รถคงค้าง'";
                    sql += " from sumc";
                    break;
                case 83:
                    sql = "select aa as 'Code', bb as 'รายละเอียด', cc as 'จำนวน (คัน)', dd as 'จำนวนเงิน (บาท)' from";
                    sql += " (select t1.id as 'aa', t1.name as 'bb'";
                    sql += " , IFNULL(t2.a, 0) as 'cc', IFNULL(t2.b, 0) as 'dd'";
                    sql += " from promotion t1 left join";
                    sql += " (select count(*) as 'a', sum(price) as 'b', proid as 'c' from recordout";
                    sql += " where dateout BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and proid > 0 and proid != 9999";
                    sql += " group by proid) t2 on t1.id = t2.c where t1.id != 9999";
                    sql += " union";
                    sql += " select '' as 'aa', 'จอดไม่เกิน 15 นาที' as 'bb'";
                    sql += " , count(*) as 'cc', IFNULL(sum(t1.price), 0) as 'dd'";
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where dateout BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t1.proid = 0 and t2.cartype < 200 and TIMESTAMPDIFF(second,t2.datein,t1.dateout) <= 959";
                    sql += " union";
                    sql += " select '' as 'aa', 'จอดเกิน 15 นาที' as 'bb'";
                    sql += " , count(*) as 'cc', IFNULL(sum(t1.price), 0) as 'dd'";
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where dateout BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t1.proid = 0 and t2.cartype < 200 and TIMESTAMPDIFF(second,t2.datein,t1.dateout) > 959) t";
                    DbController.LoadData(sql);
                    break;
                case 84: //Mac 2019/11/15
                    sql = "DROP PROCEDURE IF EXISTS dowhile2; "
                    + " CREATE PROCEDURE dowhile2(IN date_select DATETIME, IN date_finish DATETIME) "
                    + " BEGIN "
                    + "   DECLARE num INT DEFAULT 0; "
                    + "   CREATE TABLE perHour (hours varchar(30),inVisitor INT(1),inMember INT(1)); "
                    + "   WHILE num < 24 DO "
                    + "     INSERT INTO perHour VALUES ( CONCAT(DATE_FORMAT(MAKETIME(num,0,0),'%H:%i'),' - ',DATE_FORMAT(MAKETIME(num,59,0),'%H:%i')), "
                    + "     (SELECT COUNT(no)FROM recordin WHERE HOUR(datein) = num AND datein BETWEEN date_select AND date_finish AND cartype < 200 ";
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "' ";
                    sql += "),"
                    + "     (SELECT COUNT(no)FROM recordin WHERE HOUR(datein) = num AND datein BETWEEN date_select AND date_finish AND cartype = 200 ";
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "' ";
                    sql += "));"
                    + "     SET num = num + 1; "
                    + "   END WHILE; "
                    + "   SELECT hours as ชั่วโมง, inVisitor as ลูกค้าทั่วไปเข้า, inMember as สมาชิกเข้า FROM perHour; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perHour; "
                    + " CALL dowhile2('" + startDateTimeText + "','" + endDateTimeText + "');";
                    break;
                case 85: //Mac 2019/11/15
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");
                    sql = "DROP PROCEDURE IF EXISTS dowhile3; "
                    + " CREATE PROCEDURE dowhile3(IN date_select DATE, IN date_finish DATE) "
                    + " BEGIN "
                    + "   CREATE TABLE perDay (days varchar(30),inVisitor INT(1),inMember INT(1)); "
                    + "   WHILE DATE(date_select) <= DATE(date_finish) DO "
                    + "     INSERT INTO perDay VALUES(date_select, "
                    + "     (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND cartype < 200 ";
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "' ";
                    sql += "),"
                    + "     (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND cartype = 200 ";
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "' ";
                    sql += "));"
                    + "     SET date_select = DATE_ADD(date_select,INTERVAL 1 DAY); "
                    + "   END WHILE; "
                    + "   SELECT days as วันที่, inVisitor as ลูกค้าทั่วไปเข้า, inMember as สมาชิกเข้า FROM perDay; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perDay; "
                    + " CALL dowhile3('" + startDateTimeText + "','" + endDateTimeText + "');";
                    break;
                case 86: //Mac 2019/11/15
                    sql = "DROP PROCEDURE IF EXISTS dowhile2; "
                    + " CREATE PROCEDURE dowhile2(IN date_select DATETIME, IN date_finish DATETIME) "
                    + " BEGIN "
                    + "   DECLARE num INT DEFAULT 0; "
                    + "   CREATE TABLE perHour (hours varchar(30), outVisitor INT(1), outMember INT(1)); "
                    + "   WHILE num < 24 DO "
                    + "     INSERT INTO perHour VALUES ( CONCAT(DATE_FORMAT(MAKETIME(num,0,0),'%H:%i'),' - ',DATE_FORMAT(MAKETIME(num,59,0),'%H:%i')), "
                    + "     (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype < 200 ";
                    if (guardhouse != String.Empty)
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";
                    sql += "),"
                    + "     (SELECT COUNT(t1.no)FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype = 200 ";
                    if (guardhouse != String.Empty)
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";
                    sql += "));"
                    + "     SET num = num + 1; "
                    + "   END WHILE; "
                    + "   SELECT hours as ชั่วโมง, outVisitor as ลูกค้าทั่วไปออก, outMember as สมาชิกออก FROM perHour; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perHour; "
                    + " CALL dowhile2('" + startDateTimeText + "','" + endDateTimeText + "');";
                    break;
                case 87: //Mac 2019/11/15
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");
                    sql = "DROP PROCEDURE IF EXISTS dowhile3; "
                    + " CREATE PROCEDURE dowhile3(IN date_select DATE, IN date_finish DATE) "
                    + " BEGIN "
                    + "   CREATE TABLE perDay (days varchar(30), outVisitor INT(1), outMember INT(1)); "
                    + "   WHILE DATE(date_select) <= DATE(date_finish) DO "
                    + "     INSERT INTO perDay VALUES(date_select, "
                    + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype < 200 ";
                    if (guardhouse != String.Empty)
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";
                    sql += "),"
                    + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype = 200 ";
                    if (guardhouse != String.Empty)
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";
                    sql += "));"
                    + "     SET date_select = DATE_ADD(date_select,INTERVAL 1 DAY); "
                    + "   END WHILE; "
                    + "   SELECT days as วันที่, outVisitor as ลูกค้าทั่วไปออก, outMember as สมาชิกออก FROM perDay; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perDay; "
                    + " CALL dowhile3('" + startDateTimeText + "','" + endDateTimeText + "');";
                    break;
                case 88: //Mac 2020/01/11
                    sql = "SELECT no AS 'ลำดับ', name AS 'ชื่อโปรโมชั่น', (SELECT license FROM recordin WHERE no = recordpromotion_minbalance.no_recordin) AS 'ทะเบียนรถ', guardhouse AS 'ป้อม', date AS 'วันที่' ";
                    sql += " , (SELECT name FROM user WHERE id = user) AS เจ้าหน้าที่, increase_min AS 'เพิ่ม', decrease_min AS 'ลด' FROM recordpromotion_minbalance ";





                    sql += " WHERE date BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (user != Constants.TextBased.All)
                        sql += " AND user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND license LIKE '%" + licensePlate + "%'";
                    if (promotionName != Constants.TextBased.All)
                        sql += " AND id =" + promotionId;

                    sql += " ORDER BY no";

                    break;
                case 89: //Mac 2020/09/08
                    if (iteration == 4)
                        iteration = 1;
                    else
                        iteration++;

                    if (iteration == 1)
                    {
                        sql = "select id, SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 1), '|', -1) as 'ช่องจอด/ชั้น'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(member.license, ',', numbers.n), ',', -1) as 'ทะเบียนรถ'";
                        sql += ", member.name as 'เจ้าของบัตร'";
                        sql += ", SUBSTRING_INDEX(address, '|', -1) as 'บริษัท'";
                        sql += ", case when SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 3), '|', -1) = SUBSTRING_INDEX(address, '|', -1) then '' else SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 3), '|', -1) end as 'ชั้น'";
                        sql += ", cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char)  as 'เลขที่บัตร'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 2), '|', -1) as 'เลขที่ห้องชุด'";
                        sql += ", tel as 'โทรศัพท์ติดต่อ'";
                        sql += ", concat(ifnull(date_format(datestart,'%d/%m/%Y'), ''), ' - ', ifnull(date_format(dateexprie,'%d/%m/%Y'), '')) as 'วันที่อนุญาต'";
                        sql += " from";
                        sql += " (select 1 n union all";
                        sql += " select 2 union all select 3 union all";
                        sql += " select 4 union all select 5 union all";
                        sql += " select 6 union all select 7 union all";
                        sql += " select 8 union all select 9 union all";
                        sql += " select 10 union all select 11 union all";
                        sql += " select 12 union all select 13 union all";
                        sql += " select 14 union all select 15) numbers INNER JOIN member";
                        sql += " on CHAR_LENGTH(member.license)";
                        sql += " -CHAR_LENGTH(REPLACE(member.license, ',', ''))>=numbers.n-1";
                        sql += " left join cardmf t1 on member.cardid = t1.name";
                        sql += " left join cardpx t2 on member.cardid = t2.name";
                        sql += " where 1 = 1";
                        if (!String.IsNullOrEmpty(licensePlate))
                            sql += " and license LIKE '%" + licensePlate + "%'";
                        if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        {
                            if (cardId.Trim().Length == 1) //Mac 2020/10/12
                                sql += " and (t1.name_on_card like '" + cardId + "%' or t2.name_on_card like '" + cardId + "%')";
                            else
                            {
                                if (cardId.Trim().IndexOf("-") > 0) //Mac 2021/02/05
                                {
                                    sql += " and ((t1.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t1.name_on_card, 4) between " + cardId.Substring(cardId.Trim().Length - 9, 4) + " and " + cardId.Substring(cardId.Trim().Length - 4, 4) + ")";
                                    sql += " or (t2.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t2.name_on_card, 4) between " + cardId.Substring(cardId.Trim().Length - 9, 4) + " and " + cardId.Substring(cardId.Trim().Length - 4, 4) + "))";
                                }
                                else
                                    sql += " and (t1.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t1.name_on_card, 4) = " + cardId.Substring(cardId.Trim().Length - 4, 4) + ") or (t2.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t2.name_on_card, 4) = " + cardId.Substring(cardId.Trim().Length - 4, 4) + ")";
                            }
                        }
                        if (address != "") //Mac 2020/10/14
                            sql += " and address like '%" + address + "%'";
                        sql += " order by SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 1), '|', -1), id, n";
                    }
                    else if (iteration == 2)
                    {
                        sql = "select id, cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char)  as 'เลขที่บัตร'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 1), '|', -1) as 'ช่องจอด/ชั้น'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(member.license, ',', numbers.n), ',', -1) as 'ทะเบียนรถ'";
                        sql += ", member.name as 'เจ้าของบัตร'";
                        sql += ", SUBSTRING_INDEX(address, '|', -1) as 'บริษัท'";
                        sql += ", case when SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 3), '|', -1) = SUBSTRING_INDEX(address, '|', -1) then '' else SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 3), '|', -1) end as 'ชั้น'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 2), '|', -1) as 'เลขที่ห้องชุด'";
                        sql += ", tel as 'โทรศัพท์ติดต่อ'";
                        sql += ", concat(ifnull(date_format(datestart,'%d/%m/%Y'), ''), ' - ', ifnull(date_format(dateexprie,'%d/%m/%Y'), '')) as 'วันที่อนุญาต'";
                        sql += " from";
                        sql += " (select 1 n union all";
                        sql += " select 2 union all select 3 union all";
                        sql += " select 4 union all select 5 union all";
                        sql += " select 6 union all select 7 union all";
                        sql += " select 8 union all select 9 union all";
                        sql += " select 10 union all select 11 union all";
                        sql += " select 12 union all select 13 union all";
                        sql += " select 14 union all select 15) numbers INNER JOIN member";
                        sql += " on CHAR_LENGTH(member.license)";
                        sql += " -CHAR_LENGTH(REPLACE(member.license, ',', ''))>=numbers.n-1";
                        sql += " left join cardmf t1 on member.cardid = t1.name";
                        sql += " left join cardpx t2 on member.cardid = t2.name";
                        sql += " where 1 = 1";
                        if (!String.IsNullOrEmpty(licensePlate))
                            sql += " and license LIKE '%" + licensePlate + "%'";
                        if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        {
                            if (cardId.Trim().Length == 1) //Mac 2020/10/12
                                sql += " and (t1.name_on_card like '" + cardId + "%' or t2.name_on_card like '" + cardId + "%')";
                            else
                            {
                                //sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";
                                if (cardId.Trim().IndexOf("-") > 0) //Mac 2021/02/05
                                {
                                    //Mac 2021/01/27
                                    sql += " and ((t1.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t1.name_on_card, 4) between " + cardId.Substring(cardId.Trim().Length - 9, 4) + " and " + cardId.Substring(cardId.Trim().Length - 4, 4) + ")";
                                    sql += " or (t2.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t2.name_on_card, 4) between " + cardId.Substring(cardId.Trim().Length - 9, 4) + " and " + cardId.Substring(cardId.Trim().Length - 4, 4) + "))";
                                }
                                else
                                    sql += " and (t1.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t1.name_on_card, 4) = " + cardId.Substring(cardId.Trim().Length - 4, 4) + ") or (t2.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t2.name_on_card, 4) = " + cardId.Substring(cardId.Trim().Length - 4, 4) + ")";
                            }
                        }
                        if (address != "") //Mac 2020/10/14
                            sql += " and address like '%" + address + "%'";
                        sql += " order by cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char), id, n";
                    }
                    else if (iteration == 3)
                    {
                        sql = "select id, SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 2), '|', -1) as 'เลขที่ห้องชุด'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 1), '|', -1) as 'ช่องจอด/ชั้น'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(member.license, ',', numbers.n), ',', -1) as 'ทะเบียนรถ'";
                        sql += ", member.name as 'เจ้าของบัตร'";
                        sql += ", SUBSTRING_INDEX(address, '|', -1) as 'บริษัท'";
                        sql += ", case when SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 3), '|', -1) = SUBSTRING_INDEX(address, '|', -1) then '' else SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 3), '|', -1) end as 'ชั้น'";
                        sql += ", cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char)  as 'เลขที่บัตร'";
                        sql += ", tel as 'โทรศัพท์ติดต่อ'";
                        sql += ", concat(ifnull(date_format(datestart,'%d/%m/%Y'), ''), ' - ', ifnull(date_format(dateexprie,'%d/%m/%Y'), '')) as 'วันที่อนุญาต'";
                        sql += " from";
                        sql += " (select 1 n union all";
                        sql += " select 2 union all select 3 union all";
                        sql += " select 4 union all select 5 union all";
                        sql += " select 6 union all select 7 union all";
                        sql += " select 8 union all select 9 union all";
                        sql += " select 10 union all select 11 union all";
                        sql += " select 12 union all select 13 union all";
                        sql += " select 14 union all select 15) numbers INNER JOIN member";
                        sql += " on CHAR_LENGTH(member.license)";
                        sql += " -CHAR_LENGTH(REPLACE(member.license, ',', ''))>=numbers.n-1";
                        sql += " left join cardmf t1 on member.cardid = t1.name";
                        sql += " left join cardpx t2 on member.cardid = t2.name";
                        sql += " where 1 = 1";
                        if (!String.IsNullOrEmpty(licensePlate))
                            sql += " and license LIKE '%" + licensePlate + "%'";
                        if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        {
                            if (cardId.Trim().Length == 1) //Mac 2020/10/12
                                sql += " and (t1.name_on_card like '" + cardId + "%' or t2.name_on_card like '" + cardId + "%')";
                            else
                            {
                                //sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";
                                if (cardId.Trim().IndexOf("-") > 0) //Mac 2021/02/05
                                {
                                    //Mac 2021/01/27
                                    sql += " and ((t1.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t1.name_on_card, 4) between " + cardId.Substring(cardId.Trim().Length - 9, 4) + " and " + cardId.Substring(cardId.Trim().Length - 4, 4) + ")";
                                    sql += " or (t2.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t2.name_on_card, 4) between " + cardId.Substring(cardId.Trim().Length - 9, 4) + " and " + cardId.Substring(cardId.Trim().Length - 4, 4) + "))";
                                }
                                else
                                    sql += " and (t1.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t1.name_on_card, 4) = " + cardId.Substring(cardId.Trim().Length - 4, 4) + ") or (t2.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t2.name_on_card, 4) = " + cardId.Substring(cardId.Trim().Length - 4, 4) + ")";
                            }
                        }
                        if (address != "") //Mac 2020/10/14
                            sql += " and address like '%" + address + "%'";
                        sql += " order by  SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 2), '|', -1), id, n";
                    }
                    else if (iteration == 4)
                    {
                        sql = "select member.id, cast(ifnull((select name_on_card from cardpx where name = member.cardid), (select name_on_card from cardmf where name = member.cardid)) as char)  as 'เลขที่บัตร'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 1), '|', -1) as 'ช่องจอด/ชั้น'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(member.license, ',', numbers.n), ',', -1) as 'ทะเบียนรถ'";
                        sql += ", member.name as 'เจ้าของบัตร'";
                        sql += ", SUBSTRING_INDEX(address, '|', -1) as 'บริษัท'";
                        sql += ", case when SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 3), '|', -1) = SUBSTRING_INDEX(address, '|', -1) then '' else SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 3), '|', -1) end as 'ชั้น'";
                        sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(address, '|', 2), '|', -1) as 'เลขที่ห้องชุด'";
                        sql += ", tel as 'โทรศัพท์ติดต่อ'";
                        sql += ", date_format(t3.datepay,'%d/%m/%Y') as 'วันที่ทำรายการ'";
                        sql += " from";
                        sql += " (select 1 n union all";
                        sql += " select 2 union all select 3 union all";
                        sql += " select 4 union all select 5 union all";
                        sql += " select 6 union all select 7 union all";
                        sql += " select 8 union all select 9 union all";
                        sql += " select 10 union all select 11 union all";
                        sql += " select 12 union all select 13 union all";
                        sql += " select 14 union all select 15) numbers INNER JOIN member";
                        sql += " on CHAR_LENGTH(member.license)";
                        sql += " -CHAR_LENGTH(REPLACE(member.license, ',', ''))>=numbers.n-1";
                        sql += " left join cardmf t1 on member.cardid = t1.name";
                        sql += " left join cardpx t2 on member.cardid = t2.name";
                        sql += " right join member_record t3 on member.cardid = t3.cardid";
                        sql += " where 1 = 1 and t3.enable = 'False' and t3.cardid > 0";
                        sql += " and t3.datepay BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                        if (!String.IsNullOrEmpty(licensePlate))
                            sql += " and license LIKE '%" + licensePlate + "%'";
                        if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        {
                            if (cardId.Trim().Length == 1) //Mac 2020/10/12
                                sql += " and (t1.name_on_card like '" + cardId + "%' or t2.name_on_card like '" + cardId + "%')";
                            else
                            {
                                //sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";
                                if (cardId.Trim().IndexOf("-") > 0) //Mac 2021/02/05
                                {
                                    //Mac 2021/01/27
                                    sql += " and ((t1.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t1.name_on_card, 4) between " + cardId.Substring(cardId.Trim().Length - 9, 4) + " and " + cardId.Substring(cardId.Trim().Length - 4, 4) + ")";
                                    sql += " or (t2.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t2.name_on_card, 4) between " + cardId.Substring(cardId.Trim().Length - 9, 4) + " and " + cardId.Substring(cardId.Trim().Length - 4, 4) + "))";
                                }
                                else
                                    sql += " and (t1.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t1.name_on_card, 4) = " + cardId.Substring(cardId.Trim().Length - 4, 4) + ") or (t2.name_on_card like '" + cardId.Substring(0, 1) + "%' and right(t2.name_on_card, 4) = " + cardId.Substring(cardId.Trim().Length - 4, 4) + ")";
                            }
                        }
                        if (address != "") //Mac 2020/10/14
                            sql += " and address like '%" + address + "%'";
                        sql += " order by cast(ifnull((select name_on_card from cardpx where name = member.cardid), (select name_on_card from cardmf where name = member.cardid)) as char), id, n";
                    }
                    break;

                case 94: //Mac 2021/02/05
                    sql = "SELECT id AS ลำดับ, name AS 'ชื่อ - นามสกุล', license AS ทะเบียนรถ, date_format(datepay, '%d/%m/%Y %H:%i:%s') AS วันที่ทำรายการ, date_format(dateexpire, '%d/%m/%Y %H:%i:%s') AS วันหมดอายุ";
                    sql += " , case when status = 'C' then 'ลบ' else 'ระงับการใช้งาน' end as 'สถานะ'";
                    sql += " , (SELECT name FROM user WHERE id = user) AS เจ้าหน้าที่";
                    sql += " FROM member_record ";

                    sql += " WHERE datepay BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " and (enable = 'False' or status = 'C') ";
                    if (user != Constants.TextBased.All)
                        sql += " AND user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND cardid = " + cardId;

                    sql += " ORDER BY id";
                    break;
                case 95://Mac 2022/03/16
                    AppGlobalVariables.ConditionText = "";
                    AppGlobalVariables.ConditionText = "ประจำวันที่ " + startDate.ToString("dd/MM/yyyy") + " ถึงวันที่ " + endDate.ToString("dd/MM/yyyy");

                    sql = "select date_format(t1.dateout, '%d/%m/%Y') as 'วัน-เดือน-ปี', count(t1.no) as 'จำนวนรถ (คัน)'";
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where t1.dateout between '" + startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " 00:00:00' and '" + endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " 23:59:59'";

                    if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                        sql += " and t2.notshow = 'N'";
                    if (isParkingGreaterChecked)
                    {
                        sql += " and TIMESTAMPDIFF(minute, t2.datein, t1.dateout) > " + parkingGreater;
                        AppGlobalVariables.ConditionText += "   เงื่อนไข: > " + parkingGreater + " นาที";
                    }
                    else if (isParkingLesserChecked)
                    {
                        sql += " and TIMESTAMPDIFF(minute, t2.datein, t1.dateout) < " + parkingLesser;
                        AppGlobalVariables.ConditionText += "   เงื่อนไข: < " + parkingLesser + " นาที";
                    }
                    else if (isParkingBetweenChecked)
                    {
                        sql += " and (TIMESTAMPDIFF(minute, t2.datein, t1.dateout) between " + parkingBetweenFrom + " and " + parkingBetweenTo + ")";
                        AppGlobalVariables.ConditionText += "   เงื่อนไข: Between " + parkingBetweenFrom + " นาที ถึง " + parkingBetweenTo + " นาที";
                    }

                    sql += " group by date(t1.dateout)";
                    sql += " order by date(t1.dateout)";

                    break;
                case 96: //Mac 2022/03/29
                    AppGlobalVariables.ConditionText = "";
                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();

                    sql = "select date_format(date, '%d/%m/%Y') as 'วัน-เดือน-ปี', date_format(date, '%H:%i:%s') as 'เวลา'";
                    sql += " , event as 'รายละเอียด', name as 'ผู้แก้ไข'";
                    sql += " from log_event";
                    sql += " where date BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " order by date";
                    break;
                case 97:
                    sql = "select taxinvoice as 'เลขที่ใบเสร็จ'";
                    sql += " , (select store_name from m_store where store_id = storeid) as 'บริษัท/ร้านค้า'";
                    sql += " , cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char)  as 'เลขบัตร'";
                    sql += " , case when cardfee > 0 then 'ค่าบัตรสมาชิก' else (select name from cardtype where id = cardtypeid) end as 'ประเภทบัตร'";
                    sql += " , license as 'ทะเบียนรถ'";
                    sql += " , name as 'ผู้ถือบัตร'";
                    sql += " , date_format(datepay, '%d/%m/%Y') as 'วันที่ทำรายการ'";
                    sql += " , date_format(dateexpire, '%d/%m/%Y') as 'วันหมดอายุ'";
                    sql += " , format(((price + discount) / addmonth), 2) as 'ราคา/เดือน'";
                    sql += " , addmonth as 'เดือน'";
                    sql += " , format((price + discount), 2) as 'จำนวนเงิน'";
                    sql += " , format(discount, 2) as 'ส่วนลด'";
                    sql += " , format(price, 2) as 'เงินสุทธิ'";
                    sql += " , (select name from user where id = user) as 'ผู้ทำรายการ'";
                    sql += " from member_record";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and price > 0";
                    sql += " and status in ('A', 'E')";
                    sql += " and length(trim(ifnull(taxinvoice, ''))) > 0";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (memberRenewalType != Constants.TextBased.All)
                        if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10 
                            sql += " and renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];

                    if (memberProcessState == Constants.TextBased.CreateNewMemberProcessState) //Mac 2020/08/10
                        sql += " and status = 'A'";
                    else if (memberProcessState == Constants.TextBased.UpdateMemberProcessState)
                        sql += " and status = 'E'";
                    if (memberCardType == Constants.TextBased.MemberCardTypeWithPayment) //Mac 2021/01/27
                        sql += " and cardfee > 0";
                    else if (memberCardType == Constants.TextBased.MemberCardTypeNonPayment)
                        sql += " and cardfee = 0";

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += " order by datepay";
                    break;
                case 98:
                    sql = "select case when printno = 0 then '' else concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6,'0'))) end as 'เลขที่ใบเสร็จ'";
                    sql += " , (select store_name from m_store where store_id = storeid) as 'บริษัท/ร้านค้า'";
                    sql += " , cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char)  as 'เลขบัตร'";
                    sql += " , case when cardfee > 0 then 'ค่าบัตรสมาชิก' else (select name from cardtype where id = cardtypeid) end as 'ประเภทบัตร'";
                    sql += " , license as 'ทะเบียนรถ'";
                    sql += " , name as 'ผู้ถือบัตร'";
                    sql += " , date_format(datepay, '%d/%m/%Y') as 'วันที่ทำรายการ'";
                    sql += " , date_format(dateexpire, '%d/%m/%Y') as 'วันหมดอายุ'";
                    sql += " , format(((price + discount) / addmonth), 2) as 'ราคา/เดือน'";
                    sql += " , addmonth as 'เดือน'";
                    sql += " , format((price + discount), 2) as 'จำนวนเงิน'";
                    sql += " , format(discount, 2) as 'ส่วนลด'";
                    sql += " , format(price, 2) as 'เงินสุทธิ'";
                    sql += " , (select name from user where id = user) as 'ผู้ทำรายการ'";
                    sql += " from member_record";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and price > 0";
                    sql += " and status in ('A', 'E')";
                    sql += " and printno > 0";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];

                    if (memberProcessState == Constants.TextBased.CreateNewMemberProcessState) //Mac 2020/08/10
                        sql += " and status = 'A'";
                    else if (memberProcessState == Constants.TextBased.UpdateMemberProcessState)
                        sql += " and status = 'E'";

                    if (memberCardType == Constants.TextBased.MemberCardTypeWithPayment) //Mac 2021/01/27
                        sql += " and cardfee > 0";
                    else if (memberCardType == Constants.TextBased.MemberCardTypeNonPayment)
                        sql += " and cardfee = 0";

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += " order by datepay";
                    break;
                case 99:
                    sql = "select case when printno = 0 then taxinvoice else concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6,'0'))) end as 'เลขที่ใบเสร็จ'";
                    sql += " , (select store_name from m_store where store_id = storeid) as 'บริษัท/ร้านค้า'";
                    sql += " , cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char)  as 'เลขบัตร'";
                    sql += " , case when cardfee > 0 then 'ค่าบัตรสมาชิก' else (select name from cardtype where id = cardtypeid) end as 'ประเภทบัตร'";
                    sql += " , license as 'ทะเบียนรถ'";
                    sql += " , name as 'ผู้ถือบัตร'";
                    sql += " , date_format(datepay, '%d/%m/%Y') as 'วันที่ทำรายการ'";
                    sql += " , date_format(dateexpire, '%d/%m/%Y') as 'วันหมดอายุ'";
                    sql += " , format(((price + discount) / addmonth), 2) as 'ราคา/เดือน'";
                    sql += " , addmonth as 'เดือน'";
                    sql += " , format((price + discount), 2) as 'จำนวนเงิน'";
                    sql += " , format(discount, 2) as 'ส่วนลด'";
                    sql += " , format(price, 2) as 'เงินสุทธิ'";
                    sql += " , (select name from user where id = user) as 'ผู้ทำรายการ'";
                    sql += " from member_record";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and price > 0";
                    sql += " and status in ('A', 'E')";
                    sql += " and (length(trim(ifnull(taxinvoice, ''))) > 0 or printno > 0)";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];

                    if (memberProcessState == Constants.TextBased.CreateNewMemberProcessState) //Mac 2020/08/10
                        sql += " and status = 'A'";
                    else if (memberProcessState == Constants.TextBased.UpdateMemberProcessState)
                        sql += " and status = 'E'";

                    if (memberCardType == Constants.TextBased.MemberCardTypeWithPayment) //Mac 2021/01/27
                        sql += " and cardfee > 0";
                    else if (memberCardType == Constants.TextBased.MemberCardTypeNonPayment)
                        sql += " and cardfee = 0";

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += " order by datepay";
                    break;
                case 100:
                    sql = "select date_format(aa,'%d/%m/%Y') as 'วันที่', bb as 'เลขที่ใบกำกับภาษีเริ่มต้น', cc as 'เลขที่ใบกำกับภาษีสิ้นสุด', dd as 'รหัสเครื่องคิดเงิน'";
                    sql += ", ee as 'ค่าบริการ', ff as 'ภาษีมูลค่าเพิ่ม', gg as 'รวมเงิน' from ";
                    sql += "(select t2.dateout as 'aa'";
                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(min(t2.printno),6,'0'))) as 'bb'";
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(max(t2.printno),6,'0'))) as 'cc'";
                        }
                        else
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(min(t2.printno),6,'0'))) as 'bb'";
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(max(t2.printno),6,'0'))) as 'cc'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m') ,lpad(min(t2.printno),6,'0'))) as 'bb'";
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m') ,lpad(max(t2.printno),6,'0'))) as 'cc'";
                        }
                        else
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y') ,lpad(min(t2.printno),6,'0'))) as 'bb'";
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y') ,lpad(max(t2.printno),6,'0'))) as 'cc'";
                        }
                    }
                    sql += " , t2.posid as 'dd'";

                    if (Configs.Reports.ReportPriceSplitLosscard)
                    {
                        sql += " , format(sum((t2.price-t2.losscard)) - ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ee";
                        sql += " , format(ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ff";
                        sql += " , format(sum((t2.price-t2.losscard)), 2) as gg";
                    }
                    else
                    {
                        sql += " , format(sum(t2.price) - ROUND(sum(t2.price)*7/107, 6), 2) as 'ee'";
                        sql += " , format(ROUND(sum(t2.price)*7/107, 6), 2) as 'ff'";
                        sql += " , format(sum(t2.price), 2) as 'gg'";
                    }
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";

                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/09

                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    if (guardhouse != String.Empty)
                        sql += " and t2.guardhouse = '" + guardhouse + "' ";

                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and t1.cartype != 200";

                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/09
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    if (Configs.UseReceiptFor1Out)
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d'), t2.posid, t2.receipt";
                    }
                    else
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d'), t2.posid";
                    }

                    sql += " union";
                    sql += " select datepay as 'aa'";
                    sql += " , concat(receipt, concat(date_format(datepay, '%y'), lpad(min(printno), 6, '0'))) as 'bb'";
                    sql += " , concat(receipt, concat(date_format(datepay, '%y'), lpad(max(printno), 6, '0'))) as 'cc'";
                    sql += " , posid as 'dd'";
                    sql += " , format(sum(price) - ROUND(sum(price)*7/107, 6), 2) as 'ee'";
                    sql += " , format(ROUND(sum(price)*7/107, 6), 2) as 'ff'";
                    sql += " , format(sum(price), 2) as 'gg'";
                    sql += " from member_record ";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and printno > 0";
                    if (Configs.UseVoidSlip)
                        sql += " and status != 'V'";
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (user != Constants.TextBased.All)
                        sql += " and user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/09
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " group by date_format(datepay,'%Y-%m-%d'), posid";

                    sql += ") tt order by date_format(aa,'%Y-%m-%d'), dd, bb";

                    break;
                case 101:
                    sql = "select concat(aa, ' ', (select name from user where id = zz)) as 'จุดที่ออก', bb as 'เลขที่ใบกำกับภาษี', cc as 'รหัสเครื่องคิดเงิน', dd as 'หมายเลขบัตร', ee as 'ทะเบียนรถ'";
                    sql += ", date_format(ff, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า', date_format(gg, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก', hh as 'ประเภท/รายการ', ii as 'เวลาจอด/จำนวนเดือน', jj as 'ตราประทับ (E-Stamp)'";
                    sql += ", kk as 'ค่าบริการ', ll as 'ภาษีมูลค่าเพิ่ม', mm as 'รวมเงิน' from ";

                    sql += "(select t2.userout as zz, concat('จุดที่ออก ', t2.guardhouse) as aa";
                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m'), lpad(t2.printno,6,'0'))) as bb";
                        }
                        else
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y'), lpad(t2.printno,6,'0'))) as bb";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m'), lpad(t2.printno,6,'0'))) as bb";
                        }
                        else
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y'), lpad(t2.printno,6,'0'))) as bb";
                        }
                    }
                    sql += " , t2.posid as cc, cast(ifnull((select name_on_card from cardpx where name = t1.id), (select name_on_card from cardmf where name = t1.id)) as char) as dd, t1.license as ee";
                    sql += " , t1.datein as ff, t2.dateout as gg";
                    sql += " , 'รายวัน(Visiter)' as hh";
                    sql += " ,ifnull(concat(floor(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i:%s'), date_format(t2.dateout, '%Y-%m-%d %H:%i:%s'))/60)";
                    sql += " , '.', lpad(mod(timestampdiff(minute, date_format(t1.datein, '%Y-%m-%d %H:%i:%s'), date_format(t2.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0')),'-') as ii";
                    sql += " , lpad(t2.proid,7,'0') as jj";

                    if (Configs.Reports.ReportPriceSplitLosscard)
                    {
                        sql += " , format((t2.price-t2.losscard) - ROUND((t2.price-t2.losscard)*7/107, 6), 2) as kk";
                        sql += " , format(ROUND((t2.price-t2.losscard)*7/107, 6), 2) as ll";
                        sql += " , format((t2.price-t2.losscard), 2) as mm";
                    }
                    else
                    {
                        sql += " , format(t2.price - ROUND(t2.price*7/107, 6), 2) as kk";
                        sql += " , format(ROUND(t2.price*7/107, 6), 2) as ll";
                        sql += " , format(t2.price, 2) as mm";
                    }

                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        //sql += " left join member t3 on t1.license = t3.license";
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/09
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    if (guardhouse != String.Empty)
                        sql += " and t2.guardhouse = '" + guardhouse + "'";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and t1.cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and t1.license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/09
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and t1.id = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += " union";
                    sql += " select user as zz, concat('จุดที่ออก ', guardhouse) as aa";
                    sql += " , concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6, '0'))) as 'bb'";
                    sql += " , posid as cc, cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char) as dd, license as ee";
                    sql += " , datepay as ff, datepay as gg";
                    sql += " , case when cardfee > 0 then 'ค่าบัตรสมาชิก' else (select name from cardtype where id = cardtypeid) end as hh";
                    sql += " , addmonth as ii, '0000000' as jj";
                    sql += " , format(price - ROUND(price*7/107, 6), 2) as kk";
                    sql += " , format(ROUND(price*7/107, 6), 2) as ll";
                    sql += " , format(price, 2) as mm";
                    sql += " from member_record";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and printno > 0";
                    if (Configs.UseVoidSlip)
                        sql += " and status != 'V'";
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberCardType == Constants.TextBased.MemberCardTypeWithPayment) //Mac 2021/01/27
                        sql += " and cardfee > 0";
                    else if (memberCardType == Constants.TextBased.MemberCardTypeNonPayment)
                        sql += " and cardfee = 0";

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/09
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += ") tt order by aa, date_format(gg,'%Y-%m-%d %H:%i:%s'), bb";

                    break;
                case 102: //Mac 2020/01/23
                    sql = "select concat(aa, ' ', (select name from user where id = zz)) as 'จุดที่ออก', bb as 'เลขที่ใบกำกับภาษี', cc as 'รหัสเครื่องคิดเงิน', dd as 'หมายเลขบัตร', ee as 'ทะเบียนรถ'";
                    sql += ", date_format(ff, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า', date_format(gg, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก', hh as 'ประเภท/รายการ', ii as 'สาเหตุยกเลิก'";
                    sql += ", jj as 'ค่าบริการ', kk as 'ภาษีมูลค่าเพิ่ม', ll as 'รวมเงิน' from ";

                    sql += "(select t2.userout as zz, concat('จุดที่ออก ', t2.guardhouse) as aa";
                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m'), lpad(t2.printno,6,'0'))) as bb";
                        }
                        else
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y'), lpad(t2.printno,6,'0'))) as bb";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m'), lpad(t2.printno,6,'0'))) as bb";
                        }
                        else
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y'), lpad(t2.printno,6,'0'))) as bb";
                        }
                    }
                    sql += " , t2.posid as cc, cast(ifnull((select name_on_card from cardpx where name = t1.id), (select name_on_card from cardmf where name = t1.id)) as char) as dd, t1.license as ee";
                    sql += " , t1.datein as ff, t2.dateout as gg";
                    sql += " , 'รายวัน(Visiter)' as hh";
                    sql += " , t2.voidslip as ii";

                    if (Configs.Reports.ReportPriceSplitLosscard)
                    {
                        sql += " , format((t2.price-t2.losscard) - ROUND((t2.price-t2.losscard)*7/107, 6), 2) as jj";
                        sql += " , format(ROUND((t2.price-t2.losscard)*7/107, 6), 2) as kk";
                        sql += " , format((t2.price-t2.losscard), 2) as ll";
                    }
                    else
                    {
                        sql += " , format(t2.price - ROUND(t2.price*7/107, 6), 2) as jj";
                        sql += " , format(ROUND(t2.price*7/107, 6), 2) as kk";
                        sql += " , format(t2.price, 2) as ll";
                    }

                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        //sql += " left join member t3 on t1.license = t3.license";
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/09
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";
                    sql += " and t2.status = 'V'";

                    if (guardhouse != String.Empty)
                        sql += " and t2.guardhouse = '" + guardhouse + "' ";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and t1.cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and t1.license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/09
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and t1.id = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += " union";
                    sql += " select user as zz, concat('จุดที่ออก ', guardhouse) as aa";
                    sql += " , concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6, '0'))) as 'bb'";
                    sql += " , posid as cc, cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char) as dd, license as ee";
                    sql += " , datepay as ff, datepay as gg";
                    sql += " , case when cardfee > 0 then 'ค่าบัตรสมาชิก' else (select name from cardtype where id = cardtypeid) end as hh";
                    sql += " , voidslip as ii";
                    sql += " , format(price - ROUND(price*7/107, 6), 2) as jj";
                    sql += " , format(ROUND(price*7/107, 6), 2) as kk";
                    sql += " , format(price, 2) as ll";
                    sql += " from member_record";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and printno > 0";
                    sql += " and status = 'V'";

                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberCardType == Constants.TextBased.MemberCardTypeWithPayment) //Mac 2021/01/27
                        sql += " and cardfee > 0";
                    else if (memberCardType == Constants.TextBased.MemberCardTypeNonPayment)
                        sql += " and cardfee = 0";

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/09
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += ") tt order by aa, date_format(gg,'%Y-%m-%d %H:%i:%s'), bb";
                    break;
                case 103: //Mac 2020/01/23
                    sql = "select date_format(t2.dateout,'%d/%m/%Y') as 'วันที่'";
                    sql += " , 'ค่าปรับบัตรหาย' as 'ประเภท/รายการ'";
                    sql += " , concat(t2.guardhouse, ' ', (select name from user where id = t2.userout)) as 'จุดที่ออก'";
                    sql += " , date_format(t1.datein, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า', cast(ifnull((select name_on_card from cardpx where name = t1.id), (select name_on_card from cardmf where name = t1.id)) as char) as 'หมายเลขบัตร'";
                    sql += " , t1.license as 'ทะเบียนรถ', date_format(t2.dateout, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก'";
                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m'), lpad(t2.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                        }
                        else
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y'), lpad(t2.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m'), lpad(t2.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                        }
                        else
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y'), lpad(t2.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี'";
                        }
                    }

                    sql += " , format(t2.losscard - ROUND(t2.losscard*7/107, 6), 2) as 'ค่าบริการ'";
                    sql += " , format(ROUND(t2.losscard*7/107, 6), 2) as 'ภาษีมูลค่าเพิ่ม'";
                    sql += " , format(t2.losscard, 2) as 'รวมเงิน'";
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        //sql += " left join member t3 on t1.license = t3.license";
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/09
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";
                    sql += " and t2.losscard > 0";

                    if (guardhouse != String.Empty)
                        sql += " and t2.guardhouse = '" + guardhouse + "' ";
                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    //if(!String.IsNullOrEmpty(licensePlate))
                    //    sql += " and t1.license LIKE '%" + licensePlate + "%'";
                    //if (!String.IsNullOrEmpty(cardId))
                    //    sql += " and t1.id = " + cardId;

                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and t1.cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and t1.license LIKE '%" + licensePlate + "%'";

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/09
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and t1.id = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    if (Configs.UseReceiptFor1Out)
                    {
                        sql += " order by date_format(t2.dateout,'%Y-%m-%d'), t2.posid, t2.receipt";
                    }
                    else
                    {
                        sql += " order by date_format(t2.dateout,'%Y-%m-%d'), t2.posid";
                    }
                    break;
                case 104: //Mac 2020/01/24
                    sql = "select date_format(datepay,'%d/%m/%Y') as 'วันที่'";
                    sql += " , cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char) as 'หมายเลขบัตร', license as 'ทะเบียนรถ'";
                    sql += " , case when cardfee > 0 then 'ค่าบัตรสมาชิก' else (select name from cardtype where id = cardtypeid) end as 'ประเภท/รายการ'";
                    sql += " , name as 'ชื่อ-นามสกุลผู้ถือบัตร', date_format(dateexpire, '%d/%m/%Y') as 'วันที่บัตรหมดอายุ'";
                    sql += " , date_format(datepay, '%d/%m/%Y') as 'วันที่ใบกำกับภาษี'";
                    //sql += " , case when printno = 0 then '' else concat('IM', DATE_FORMAT(datepay, '%y'), LPAD(printno, 6, '0')) end AS 'เลขที่ใบกำกับภาษี'";
                    sql += " , case when printno = 0 then '' else concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6, '0'))) end as 'เลขที่ใบกำกับภาษี'";
                    sql += " , format(price - ROUND(price*7/107, 6), 2) as 'ค่าบริการ'";
                    sql += " , format(ROUND(price*7/107, 6), 2) as 'ภาษีมูลค่าเพิ่ม'";
                    sql += " , format(price, 2) as 'รวมเงิน'";

                    sql += " from member_record";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and price > 0";
                    sql += " and status in ('A', 'E')";

                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and user = " + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];
                    //if(!String.IsNullOrEmpty(licensePlate))
                    //    sql += " and license LIKE '%" + licensePlate + "%'";
                    //if (!String.IsNullOrEmpty(cardId))
                    //    sql += " and cardid = " + cardId;

                    if (memberProcessState == Constants.TextBased.CreateNewMemberProcessState) //Mac 2020/08/10
                        sql += " and status = 'A'";
                    else if (memberProcessState == Constants.TextBased.UpdateMemberProcessState)
                        sql += " and status = 'E'";

                    if (memberCardType == Constants.TextBased.MemberCardTypeWithPayment) //Mac 2021/01/27
                        sql += " and cardfee > 0";
                    else if (memberCardType == Constants.TextBased.MemberCardTypeNonPayment)
                        sql += " and cardfee = 0";

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += " order by date_format(datepay,'%Y-%m-%d'), concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6, '0')))";

                    break;
                case 105: //Mac 2020/01/28 
                    sql = "select concat('จุดที่ออก ', guardhouse, ' ', (select name from user where id = user)) as 'จุดที่ออก'";
                    sql += " , date_format(datepay, '%d/%m/%Y') as 'วันที่ใบกำกับภาษี'";
                    //sql += " , case when printno = 0 then '' else concat('IM', DATE_FORMAT(datepay, '%y'), LPAD(printno, 6, '0')) end AS 'เลขที่ใบกำกับภาษี'";
                    sql += " , case when printno = 0 then '' else concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6,'0'))) end as 'เลขที่ใบกำกับภาษี'";
                    sql += " , (select lpad(store_id, 4, '0') from m_store where store_id = storeid) as 'รหัสบริษัท'";
                    sql += " , (select customer_code from m_store where store_id = storeid) as 'รหัสลูกค้า CV-CODE'";
                    sql += " , (select store_name from m_store where store_id = storeid) as 'บริษัท/ร้านค้า'";
                    sql += " , cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char) as 'หมายเลขบัตร', license as 'ทะเบียนรถ'";
                    sql += " , case when cardfee > 0 then 'ค่าบัตรสมาชิก' else (select name from cardtype where id = cardtypeid) end as 'ประเภท/รายการ'";
                    sql += " , name as 'ชื่อ-นามสกุลผู้ถือบัตร'";
                    sql += " , date_format(datepay, '%d/%m/%Y') as 'วันที่ทำรายการ', date_format(dateexpire, '%d/%m/%Y') as 'วันที่บัตรหมดอายุ'";
                    sql += " , format(((price + discount) / addmonth), 2) as 'ราคา/เดือน'";
                    sql += " , addmonth as 'จำนวนเดือน'";
                    sql += " , format((price + discount), 2) as 'จำนวนเงิน'";
                    sql += " , format(discount, 2) as 'ส่วนลด'";
                    sql += " , format(price - ROUND(price*7/107, 6), 2) as 'ค่าบริการ'";
                    sql += " , format(ROUND(price*7/107, 6), 2) as 'ภาษีมูลค่าเพิ่ม'";
                    sql += " , format(price, 2) as 'รวมเงิน'";

                    sql += " from member_record";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and price > 0";
                    sql += " and status in ('A', 'E')";

                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and user = " + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];
                    //if(!String.IsNullOrEmpty(licensePlate))
                    //    sql += " and license LIKE '%" + licensePlate + "%'";
                    //if (!String.IsNullOrEmpty(cardId))
                    //    sql += " and cardid = " + cardId;

                    if (memberProcessState == Constants.TextBased.CreateNewMemberProcessState) //Mac 2020/08/10
                        sql += " and status = 'A'";
                    else if (memberProcessState == Constants.TextBased.UpdateMemberProcessState)
                        sql += " and status = 'E'";

                    if (memberCardType == Constants.TextBased.MemberCardTypeWithPayment) //Mac 2021/01/27
                        sql += " and cardfee > 0";
                    else if (memberCardType == Constants.TextBased.MemberCardTypeNonPayment)
                        sql += " and cardfee = 0";

                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += " order by date_format(datepay,'%Y-%m-%d'), concat(receipt, concat(date_format(datepay, '%y'), lpad(printno, 6,'0')))";

                    break;
                case 106: //Mac 2020/03/09
                    //Mac 2021/03/01
                    sql = "select (select lpad(store_id, 4, '0') from m_store where store_id = storeid) as 'รหัสบริษัท'";
                    sql += " , (select customer_code from m_store where store_id = storeid) as 'รหัสลูกค้า CV-CODE'";
                    sql += " , (select store_name from m_store where store_id = storeid) as 'บริษัท/ร้านค้า'";
                    sql += " , concat('(โควต้า ', cast((select store_car_quota from m_store where m_store.store_id = storeid) as char), ' คัน)') as 'โควต้า (คัน)'";
                    sql += " , cast(ifnull((select name_on_card from cardpx where name = cardid), (select name_on_card from cardmf where name = cardid)) as char)  as 'หมายเลขบัตร', license as 'ทะเบียนรถ'";
                    sql += " , (select name from cardtype where id = cardtypeid) as 'ประเภท/รายการ'";
                    sql += " , name as 'ชื่อ-นามสกุลผู้ถือบัตร'";
                    sql += " , date_format('" + startDateTimeText + "', '%d/%m/%Y') as 'วันที่ทำรายการ', date_format(dateexprie, '%d/%m/%Y') as 'วันที่บัตรหมดอายุ'";
                    sql += " , format((select price from renew_mem_rate where renewID = renew_memid and month = 1), 2) as 'ราคา/เดือน'";
                    sql += " , format((select price from renew_mem_rate where renewID = renew_memid and month = 1) - ROUND((select price from renew_mem_rate where renewID = renew_memid and month = 1)*7/107, 6), 2) as 'ค่าบริการ'";
                    sql += " , format(ROUND((select price from renew_mem_rate where renewID = renew_memid and month = 1)*7/107, 6), 2) as 'ภาษีมูลค่าเพิ่ม'";
                    sql += " , format((select price from renew_mem_rate where renewID = renew_memid and month = 1), 2) as 'รวมเงิน'";
                    sql += " from member where enable = 'True'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and user = " + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];
                    if (!String.IsNullOrEmpty(cardId)) //Mac 2020/08/11
                        sql += " and cardid = cast(ifnull(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')), '" + cardId + "') as char)";

                    sql += " order by (select lpad(store_id, 4, '0') from m_store where store_id = storeid)";
                    break;

                case 107: //Mac 2020/06/04
                    sql = "select aa as 'รหัสบริษัท', bb as 'รหัสลูกค้า CV-CODE', cc as 'บริษัท/ร้านค้า', dd as 'โควต้า (ชม.)', ee as 'วันที่', ff as 'รหัสตราประทับ (E-stamp)'";
                    if (Configs.Reports.UseReportHourUse) //Mac 2023/02/22
                        sql += ", gg as 'ตราประทับ/E-stamp (ชม.)ฟรี', hh as 'ส่วนลดเวลา (ชม.)แรกฟรี', ii as 'จำนวนชั่วโมงที่คิดจริง' from";
                    else
                        sql += ", gg as 'ตราประทับ/E-stamp (ชม.)ฟรี', hh as 'ส่วนลดเวลา (ชม.)แรกฟรี', ii as 'ส่วนเกินส่วนลด (ชม.)เรียกเก็บ' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";
                    }
                    else if (Configs.Reports.UseReportHourUse) //Mac 2023/02/22
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(parking_out_hour_use) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }
                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')), date_format(parking_out_time, '%d/%m/%Y')";
                    //sql += " order by lpad(store_id, 4, '0'), date_format(parking_out_time, '%Y-%m-%d'), concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0'))";

                    if (!Configs.Reports.UseReportHourUse) //Mac 2023/02/22
                    {
                        sql += " union ";

                        sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                        sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                        sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                        sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                        sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                        sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                        if (Configs.Reports.UseReport108_110_1)
                        {
                            sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                            sql += " , 0 as 'hh'";
                            sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                        }
                        else
                        {
                            sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                            sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                            sql += " , case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                        }
                        sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                        sql += " where (t1.proid between 1 and 255) and ";
                        sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                        if (memberGroupMonth != Constants.TextBased.All)
                            sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += " group by concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')), date_format(t1.dateout, '%d/%m/%Y')";
                    }
                    //sql += " order by lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), date_format(t1.dateout, '%Y-%m-%d'), concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0'))";
                    sql += ") tt order by aa, ee, ff";

                    break;
                case 108: //Mac 2020/06/09
                    sql = "select aa as 'รหัสบริษัท', bb as 'รหัสลูกค้า CV-CODE', cc as 'บริษัท/ร้านค้า', dd as 'พื้นที่เช่า (ตรม.)', ee as 'โควต้า (คัน)', ff as 'โควต้า (ชม.)'";
                    sql += ", gg as 'รหัสตราประทับ (E-stamp)', hh as 'ตราประทับ/E-stamp (ชม.)ฟรี', ii as 'ส่วนลดเวลา (ชม.)แรกฟรี', jj as 'ส่วนลดเวลา (ชม.)แรกฟรี1' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , '' as 'dd'";
                    sql += " , cast((select store_car_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'ee'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'ff'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'gg'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'hh'";
                        sql += " , 0 as 'ii'";
                        sql += " , 0 as 'jj'";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'hh'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 0 when DAYOFWEEK(parking_out_time) not in (1, 7) then 1 else 0 end) as 'ii'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 0 end) as 'jj'";
                    }
                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0'))";
                    //sql += " order by lpad(store_id, 4, '0'), concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0'))";

                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , '' as 'dd'";
                    sql += " , cast((select store_car_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'ee'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'ff'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'gg'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'hh'";
                        sql += " , 0 as 'ii'";
                        sql += " , 0 as 'jj'";
                    }
                    else
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'hh'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 0 when DAYOFWEEK(t1.dateout) not in (1, 7) then 1 else 0 end) as 'ii'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 0 end) as 'jj'";
                    }
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0'))";
                    //sql += " order by lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0'))";
                    sql += ") tt order by aa, gg";
                    break;
                case 109: //Mac 2020/06/09
                    sumCalQuota109 = 0;
                    sql = " DROP TABLE IF EXISTS `report110`;";
                    sql += " CREATE TABLE `report110` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `store_id` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `customer_code` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `store_name` varchar(500) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `store_estamp_quota` int(11) DEFAULT '0',";
                    sql += "  `estamp_id` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `estamp_hour` int(11) DEFAULT '0',";
                    sql += "  `x` int(11) DEFAULT '0',";
                    sql += "  `y` int(11) DEFAULT '0',";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    sql = "insert into `report110` (`store_id`, `customer_code`, `store_name`, `store_estamp_quota`, `estamp_id`, `estamp_hour`, `x`, `y`)";
                    sql += " select aa as 'รหัสบริษัท', bb as 'รหัสลูกค้า CV-CODE', cc as 'บริษัท/ร้านค้า', dd as 'โควต้า (ชม.)', ff as 'รหัสตราประทับ (E-stamp)'";
                    sql += ", gg as 'ตราประทับ/E-stamp (ชม.)ฟรี', hh as 'ส่วนลดเวลา (ชม.)แรกฟรี', ii as 'ส่วนเกินส่วนลด (ชม.)เรียกเก็บ' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%Y-%m-%d') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }
                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')), date_format(parking_out_time, '%d/%m/%Y')";

                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%Y-%m-%d') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }
                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')), date_format(t1.dateout, '%d/%m/%Y')";
                    sql += ") tt order by aa, ff, ee";

                    DbController.LoadData(sql);

                    if (Configs.IsSwitch)
                    {
                        sql = "select lpad(t.store_id, 4, '0'), case when LENGTH(ifnull(t.customer_code, '')) = 0 then 'NO CV' else t.customer_code end, t.store_name";
                        sql += " , case when ifnull(v.a, 0) > t.store_estamp_quota then ((ifnull(v.a, 0) - t.store_estamp_quota) * 10) else 0 end ";
                        sql += " from m_store t left join ";
                        sql += " (select t1.store_id, t1.customer_code, t1.store_name, t1.store_estamp_quota, sum(t2.y) as 'a' ";
                        sql += " from m_store t1 left join `report110` t2 on t1.store_id = t2.store_id";
                        sql += " group by t2.store_id, t2.customer_code, t2.store_name, t1.store_estamp_quota) v on t.store_id = v.store_id";
                        sql += " where t.store_status_flag = 'Y'"; //Mac 2020/12/16
                        sql += " order by t.store_id";
                        DataTable dt109 = DbController.LoadData(sql);
                        if (dt109.Rows.Count < 1)
                        {
                            sql = "";
                            Configs.IsSwitch = false;
                            break;
                        }
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Text Documents (*.txt)|*.txt";
                        sfd.FileName = "*.txt";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            SaveToTextFile(dt109, sfd.FileName);
                        }
                        sql = "";
                        Configs.IsSwitch = false;
                    }
                    else
                    {
                        sql = "select sum(case when ifnull(v.a, 0) > t.store_estamp_quota then (ifnull(v.a, 0) - t.store_estamp_quota) else 0 end)";
                        sql += " from m_store t left join ";
                        sql += " (select t1.store_id, t1.customer_code, t1.store_name, t1.store_estamp_quota, sum(t2.y) as 'a' ";
                        sql += " from m_store t1 left join `report110` t2 on t1.store_id = t2.store_id ";
                        sql += " group by t2.store_id, t2.customer_code, t2.store_name, t1.store_estamp_quota) v on t.store_id = v.store_id";
                        if (memberGroupMonth == Constants.TextBased.All) //Mac 2020/12/16
                        {
                            sql += " where t.store_status_flag = 'Y'";
                        }
                        else if (memberGroupMonth != Constants.TextBased.All)
                            sql += " where t.store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        DataTable dt109 = DbController.LoadData(sql);
                        if (dt109 != null && dt109.Rows.Count > 0)
                        {
                            sumCalQuota109 = Convert.ToInt32(dt109.Rows[0][0]);
                        }
                        else
                            sumCalQuota109 = 0;


                        sql = " select t.store_id as 'รหัสบริษัท', t.customer_code as 'รหัสลูกค้า CV-CODE', t.store_name as 'บริษัท/ร้านค้า', t.store_estamp_quota as 'โควต้า (ชม.)', v.ff as 'รหัสตราประทับ (E-stamp)'";
                        sql += ", v.gg as 'ตราประทับ/E-stamp (ชม.)ฟรี', v.hh as 'ส่วนลดเวลา (ชม.)แรกฟรี', v.ii as 'ส่วนเกินส่วนลด (ชม.)เรียกเก็บ' from m_store t left join";
                        sql += " (select store_id as 'aa'";
                        sql += " , customer_code as 'bb'";
                        sql += " , store_name as 'cc'";
                        sql += " , store_estamp_quota as 'dd'";
                        sql += " , estamp_id as 'ff'";
                        sql += " , sum(estamp_hour) as 'gg'";
                        sql += " , sum(x) as 'hh'";
                        sql += " , sum(y) as 'ii'";
                        sql += " from `report110` ";
                        sql += " group by estamp_id";
                        sql += " order by store_id, estamp_id) v on t.store_id = v.aa";
                        if (memberGroupMonth == Constants.TextBased.All) //Mac 2020/12/16
                        {
                            sql += " where t.store_status_flag = 'Y'";
                        }
                        else if (memberGroupMonth != Constants.TextBased.All)
                            sql += " where t.store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += " order by t.store_id, v.ff";

                        Configs.IsSwitch = true;
                    }
                    break;
                case 110:
                    sql = "SELECT lpad(cast(cardname as char),10,0) as 'รหัสพนักงาน',case when level = '1' THEN 'ผู้ใช้ทั่วไป' WHEN level = '2' ";
                    sql += "then 'ผู้จัดการ' when level ='3' then 'ผู้ดูแลระบบ' else 'Unknow' end as 'สถานะผู้ใช้งาน',";
                    sql += "name as 'ชื่อ - สกุล',tel as 'เบอร์โทรศัพท์' from user order by level,name";
                    break;
                case 111:
                    sql = "select cast(ifnull((select name_on_card from cardpx where name = member.cardid),";
                    sql += "(select name_on_card from cardmf where name = member.cardid)) as char) as 'หมายเลขบัตร',";
                    sql += "concat(member_record.license) as 'ทะเบียนรถ',concat(member_record.cardid,'   ', member_record.name) as 'ผู้ถือบัตร', ";
                    sql += "member.address as 'ชั้น/ห้อง',";
                    sql += "date_format(member_record.datepay,'%d/%m/%Y %H:%i:%s') as 'วันที่ทำรายการ',";
                    sql += "date_format(member_record.dateexpire,'%d/%m/%Y %H:%i:%s')  as 'วันที่บัตรหมดอายุ', ";
                    sql += "(select name from user where id = member_record.user) as 'ผู้ทำรายการ',price as 'จำนวนเงิน',";
                    sql += "ifnull((select name from cardtype where cardtype.id = member_record.cardtypeid),'None') as 'ประเภทบัตร',";
                    sql += "case when status = 'A' then 'เพิ่มบัตร' when status = 'C' then 'ยกเลิกบัตร' when status = 'E' then 'อัพเดต'";
                    sql += " when member_record.status = 'B' then 'ระงับบัตร'  when member_record.status = 'CB' then 'ยกเลิกระงับบัตร' when member_record.status = 'V' then 'ยกเลิก' else 'Unknow' end as 'ประเภทรายการ' ,";
                    sql += "ifnull((select store_name from m_store where store_id = member_record.storeid),'None') as 'กลุ่มสมาชิก',";
                    sql += "case when member.enable = 'True' then 'ใช้งาน' else 'ไม่ใช้งาน' end as 'สถานะ'";
                    sql += " from member_record left join member on member_record.cardid = member.cardid";
                    sql += " where member_record.datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (user != Constants.TextBased.All)
                        sql += " and member_record.user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and member_record.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " and member_record.cardid = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    if (carType != Constants.TextBased.All)
                        sql += "  and member.typeid = (select typeid from cartype where typename LIKE '%" + carType + "%')";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and member_record.storeid = (select store_id from m_store where store_name = '" + memberGroupMonth + "')";
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and member_record.cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and member_record.renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];
                    if (memberProcessState == Constants.TextBased.CreateNewMemberProcessState) //Mac 2020/08/10
                        sql += " and member_record.status = 'A'";
                    else if (memberProcessState == Constants.TextBased.UpdateMemberProcessState)
                        sql += " and member_record.status = 'E'";
                    sql += " order by member_record.id ASC;";
                    break;

                case 112:
                    sql = "select date_format(liftrecord.datelift,'%d/%m/%Y') as 'วันที่',recordout.guardhouse as 'สถานีปฏิบัติงาน',";
                    sql += "concat(lpad((select user.cardname from user where user.id = liftrecord.userid),10,0),'   ',(select user.name from user where user.id = liftrecord.userid)) as 'รหัส-ชื่อพนักงาน',sum(recordout.price) as 'จำนวนเงิน' from ";
                    sql += "liftrecord left join recordout on liftrecord.no = recordout.no  where liftrecord.datelift between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and liftrecord.gate LIKE 'O'";

                    if (user != Constants.TextBased.All)
                        sql += " and liftrecord.userid =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    sql += " group by recordout.guardhouse,date_format(liftrecord.datelift,'%d-%m-%Y')";
                    sql += " order by recordout.guardhouse,liftrecord.datelift ASC;";
                    break;
                case 113:
                    sql = "select recordout.guardhouse as 'สถานีปฏิบัติงาน' ,";
                    sql += "(select user.name from user where user.id = liftrecord.userid) as 'รหัส-ชื่อพนักงาน',";
                    sql += "cast((select recordin.id from recordin where recordin.no = recordout.no) as char) as 'เลขที่บัตร',";
                    sql += "liftrecord.license as 'ทะเบียนรถ',date_format(recordin.datein, '%d/%m/%Y %H:%i:%s') as 'วันที่-เวลาเข้า',";
                    sql += "date_format(liftrecord.datelift, '%d/%m/%Y %H:%i:%s') as 'วันที่-เวลาออก' ";
                    sql += ",recordout.proid as 'รหัสตราประทับ',recordout.price as 'จำนวนเงิน',date_format(liftrecord.datelift, '%d/%m/%Y') as 'วันที่ออก' ";
                    sql += " from liftrecord left join recordout on liftrecord.no = recordout.no left";
                    sql += " join recordin on recordout.no = recordin.no where liftrecord.datelift between '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                    if (user != Constants.TextBased.All)
                        sql += " and liftrecord.userid =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    sql += " order by recordout.guardhouse,liftrecord.datelift ASC;";
                    break;

                case 114:
                    sql = "select (select cardtype.name from cardtype where cardtype.id = cardtypeid) as 'ประเภทบัตร',";
                    sql += "cast(ifnull((select name_on_card from cardpx where name = member.cardid),(select name_on_card from cardmf where name = member.cardid)) as char) as 'หมายเลขบัตร',concat(lpad(member.memkey,10,'0'),(' : '),(member.name)) as 'ผู้ถือบัตร',";
                    sql += "member.license as 'ทะเบียนรถ',(select max(price) from member_record where cardid = member.cardid) as 'จำนวนเงิน',";
                    sql += "case WHEN enable='True' then  'ใช้งาน' else 'ยกเลิกบัตร' end as 'สถานะผู้ถือบัตร',";
                    sql += "(select m_store.store_name from m_store where m_store.store_id = member.storeid) as 'บริษัท/ร้านค้า'";
                    sql += " from member  where id is not null";
                    if (!String.IsNullOrWhiteSpace(licensePlate))
                    {
                        sql += " and license LIKE '%" + licensePlate + "%'";
                    }
                    if (!String.IsNullOrWhiteSpace(cardId))
                    {
                        sql += " and cardid = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    }
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and member.cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];

                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and member.renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];

                    if (carType != Constants.TextBased.All)
                        sql += "  and member.typeid = (select typeid from cartype where typename LIKE '%" + carType + "%')";

                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and member.storeid = (select store_id from m_store where store_name = '" + memberGroupMonth + "')";

                    sql += " order by member.cardtypeid,member.cardid,member.memkey,member.name";
                    break;
                case 115:
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");
                    sql = "DROP PROCEDURE IF EXISTS dowhileLosscard;"
                    + " CREATE PROCEDURE dowhileLosscard(IN date_select DATE, IN date_finish DATE)"
                    + " BEGIN"
                    + " CREATE TABLE losscardperDay (days varchar(30), Lostcard INT(5) unsigned NOT NULL DEFAULT '0',Price INT(11)  DEFAULT '0');"
                    + " WHILE DATE(date_select) <= DATE(date_finish) DO"
                    + " IF (SELECT count(t1.no) FROM recordout t1 left join recordin t2 on t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout LIKE CONCAT(date_select, '%') AND t1.losscard != 0";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += ") > 0 THEN"
                    + " INSERT INTO losscardperDay VALUES(date_select,"
                    + " (SELECT count(t1.no) FROM recordout t1 left join recordin t2 on t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout LIKE CONCAT(date_select, '%') AND t1.losscard != 0";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "),"
                    + " (SELECT sum(t1.losscard) FROM recordout t1 left join recordin t2 on t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t1.losscard != 0";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "));"
                    + " END IF;"
                    + " SET date_select = DATE_ADD(date_select, INTERVAL 1 DAY);"
                    + " END WHILE;"
                    + " select date_format(days,'%d/%m/%Y') as 'วันที่',Lostcard as 'จำนวนรถ',format(Price - ROUND(Price*7/107, 6), 2)  as 'ค่าปรับบัตรหาย',format(ROUND(Price*7/107, 6), 2) as VAT,Price as 'รายได้รวมVAT'  from losscardperDay where Lostcard > 0 order by days ASC;"
                    + " END;"
                    + " DROP TABLE IF EXISTS losscardperDay;"
                    + " CALL dowhileLosscard('" + startDateTimeText + "', '" + endDateTimeText + "');";

                    break;
                case 116:
                    sql = "select recordout.guardhouse as 'สถานี',(select name from user where user.id = recordout.userout) as 'เจ้าหน้าที่',recordin.license as 'ทะเบียนรถ'";
                    sql += ",cast(ifnull((select name_on_card from cardpx where name = recordin.id),(select name_on_card from cardmf where name = recordin.id)) as char) as 'หมายเลขบัตร',date_format(recordout.dateout,'%H:%i:%s') as 'เวลาออก',";
                    sql += "recordout.losscard as 'จำนวนเงิน',date_format(recordout.dateout,'%d/%m/%Y')  as 'วันที่' ";
                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat(recordout.receipt, concat(date_format(recordout.dateout,'%y%m'), lpad(recordout.printno,6,'0'))) as 'เลขที่ใบเสร็จ'";
                        }
                        else
                        {
                            sql += " , concat(recordout.receipt, concat(date_format(recordout.dateout,'%y'), lpad(recordout.printno,6,'0'))) as 'เลขที่ใบเสร็จ'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(recordout.dateout,'%y%m'), lpad(recordout.printno,6,'0'))) as 'เลขที่ใบเสร็จ'";
                        }
                        else
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(recordout.dateout,'%y'), lpad(recordout.printno,6,'0'))) as 'เลขที่ใบเสร็จ'";
                        }
                    }
                    sql += " from recordout left join recordin on recordout.no = recordin.no ";
                    sql += " left join member on recordin.id = member.cardid "; //Mac 2021/08/10
                    sql += " where recordout.losscard > 0";
                    sql += " AND recordout.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    if (!String.IsNullOrWhiteSpace(cardId))
                    {
                        sql += " and recordin.id = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    }
                    if (guardhouse != String.Empty)
                    {
                        sql += " and recordout.guardhouse LIKE '%" + guardhouse + "%'";
                    }
                    if (user != Constants.TextBased.All)
                        sql += " and recordout.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and recordin.license LIKE '%" + licensePlate + "%'";

                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and member.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " and recordout.printno > 0";
                    sql += " order by dateout ASC";
                    break;
                case 117:
                    sql = "select (select concat(lpad(m_store.store_id,4,0),' : ',m_store.store_name) from m_store where m_store.store_id = member.storeid) as 'ร้านค้า' , ";
                    sql += "cast(ifnull((select name_on_card from cardpx where name = member.cardid),(select name_on_card from cardmf where name = member.cardid)) as char) as 'หมายเลขบัตร',";
                    sql += "(select cardtype.name from cardtype where cardtype.id = member.cardtypeid) as 'ประเภทบัตร',";
                    sql += "member.name as 'ผู้ถือบัตร',member.license as 'ทะเบียนรถ',date_format(datestart,'%d/%m/%Y %H:%i:%s') as 'วันที่ออกบัตร',";
                    sql += "date_format(dateexprie,'%d/%m/%Y %H:%i:%s') as 'วันที่บัตรหมดอายุ',case when enable = 'True' then 'ใช้งาน' else 'ยกเลิกบัตร' end as 'สถานะ',";
                    sql += "( select name from user where user.id = member_record.user order by member_record.datepay) as 'เจ้าหน้าที่ออกบัตร'";
                    sql += "  from member_record left join member on member_record.cardid = member.cardid where member_record.datepay BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    sql += " and member_record.status = 'A'";
                    if (!String.IsNullOrWhiteSpace(cardId))
                    {
                        sql += " and member.cardid = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    }
                    if (!String.IsNullOrWhiteSpace(licensePlate))
                    {
                        sql += " and member.license LIKE '%" + licensePlate + "%'";
                    }
                    if (memberGroupMonth != Constants.TextBased.All)
                    {
                        sql += " and member.storeid = (select m_store.store_id from m_store where store_name = '" + memberGroupMonth + "')";
                    }
                    sql += "  order by member.storeid, member.cardid ,member.name  ASC";
                    break;

                case 118:
                    //startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    //endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");
                    //sql = "DROP PROCEDURE IF EXISTS dowhile10; "
                    //+ " CREATE PROCEDURE dowhile10(IN date_select DATETIME, IN date_finish DATETIME) "
                    //+ " BEGIN "
                    //+ "   DECLARE num INT DEFAULT 0; "
                    //    //+ "   CREATE TABLE perHour (hours varchar(30),carin INT(1),carout INT(1)); "
                    //+ "   CREATE TABLE perHour (hours varchar(30),inVisitor INT(1),inMember INT(1), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                    //+ "   WHILE num < 24 DO "
                    //+ "     INSERT INTO perHour VALUES ( CONCAT(DATE_FORMAT(MAKETIME(num,0,0),'%H:%i'),' - ',DATE_FORMAT(MAKETIME(num,59,0),'%H:%i')), "
                    //+ "     (SELECT COUNT(no) FROM recordin WHERE HOUR(datein) = num AND datein BETWEEN date_select AND date_finish AND cartype < 200), "
                    //+ "     (SELECT COUNT(no) FROM recordin WHERE HOUR(datein) = num AND datein BETWEEN date_select AND date_finish AND cartype = 200), "
                    //+ "     (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype < 200), "
                    //+ "     (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype = 200), "
                    //+ "     (SELECT COUNT(no) FROM recordin WHERE HOUR(datein) = num AND datein BETWEEN date_select AND date_finish AND cartype > 200));"
                    //+ "     SET num = num + 1; "
                    //+ "   END WHILE; "
                    //    //+ "   SELECT hours as ชั่วโมง ,carin as รถเข้า ,carout as รถออก FROM perHour; "
                    //    //+ "   SELECT hours as ชั่วโมง, inVisitor as ลูกค้าทั่วไปเข้า, inMember as สมาชิกเข้า, outVisitor as ลูกค้าทั่วไปออก, outMember as สมาชิกออก FROM perHour; "
                    //+ " END; "
                    //+ " DROP TABLE IF EXISTS perHour; "
                    //+ " CALL dowhile10('" + startDateTimeText + "','" + endDateTimeText + "');";

                    sql = "DROP PROCEDURE IF EXISTS dowhile10; "
                    + " CREATE PROCEDURE dowhile10(IN date_select DATETIME, IN date_finish DATETIME) "
                    + " BEGIN "
                    + "   DECLARE num INT DEFAULT 0; "
                    + "   CREATE TABLE perHour (hours varchar(30),inVisitor INT(1),inMember INT(1), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                    + "   WHILE num < 24 DO "
                    + "     INSERT INTO perHour VALUES ( CONCAT(DATE_FORMAT(MAKETIME(num,0,0),'%H:%i'),' - ',DATE_FORMAT(MAKETIME(num,59,0),'%H:%i')), "
                    + "     (SELECT COUNT(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE HOUR(t1.datein) = num AND t1.datein BETWEEN date_select AND date_finish AND t1.cartype < 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT COUNT(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE HOUR(t1.datein) = num AND t1.datein BETWEEN date_select AND date_finish AND t1.cartype = 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype < 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE HOUR(t1.dateout) = num AND t1.dateout BETWEEN date_select AND date_finish AND t2.cartype = 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT COUNT(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE HOUR(t1.datein) = num AND t1.datein BETWEEN date_select AND date_finish AND t1.cartype > 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "));"
                    + "     SET num = num + 1; "
                    + "   END WHILE; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perHour; "
                    + " CALL dowhile10('" + startDateTimeText + "','" + endDateTimeText + "');";

                    sql += "   SELECT hours as 'เวลา', inVisitor as 'ปริมาณรถชั่วคราว', inMember as 'ปริมาณรถประจำ', othercar as 'ปริมาณรถUnknowType',(inVisitor+inMember+othercar) as 'รวมจำนวนรถ' FROM perHour; ";
                    break;

                case 119:
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");
                    //sql = "DROP PROCEDURE IF EXISTS dowhile11; "
                    //+ " CREATE PROCEDURE dowhile11(IN date_select DATE, IN date_finish DATE) "
                    //+ " BEGIN "
                    //    //+ "   CREATE TABLE perDay (days varchar(30),carin INT(1),carout INT(1)); "
                    //+ "   CREATE TABLE perDay (days varchar(30),inVisitor INT(1),inMember INT(1), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                    //+ "   WHILE DATE(date_select) <= DATE(date_finish) DO "
                    //+ "   IF (select count(*) from recordin WHERE date_format(datein,'%Y-%m-%d') = date_select ) > 0 THEN"
                    //+ "     INSERT INTO perDay VALUES(date_select, "
                    //+ "     (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND cartype < 200), "
                    //+ "     (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND cartype = 200), "
                    //+ "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype < 200), "
                    //+ "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype = 200), "
                    //+ "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype > 200)); "
                    //+ "     END IF;"
                    //+ "     SET date_select = DATE_ADD(date_select,INTERVAL 1 DAY); "

                    //+ "   END WHILE; "
                    //    //+ "   SELECT days as วันที่ ,carin as รถเข้า ,carout as รถออก FROM perDay; "
                    //    //+ "   SELECT days as วันที่, inVisitor as ลูกค้าทั่วไปเข้า, inMember as สมาชิกเข้า, outVisitor as ลูกค้าทั่วไปออก, outMember as สมาชิกออก FROM perDay; "
                    //+ " END; "
                    //+ " DROP TABLE IF EXISTS perDay; "
                    //+ " CALL dowhile11('" + startDateTimeText + "','" + endDateTimeText + "');";

                    sql = "DROP PROCEDURE IF EXISTS dowhile11; "
                    + " CREATE PROCEDURE dowhile11(IN date_select DATE, IN date_finish DATE) "
                    + " BEGIN "
                    + "   CREATE TABLE perDay (days varchar(30),inVisitor INT(1),inMember INT(1), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                    + "   WHILE DATE(date_select) <= DATE(date_finish) DO "
                    + "   IF (select count(*) from recordin WHERE date_format(datein,'%Y-%m-%d') = date_select ) > 0 THEN"
                    + "     INSERT INTO perDay VALUES(date_select, "
                    + "     (SELECT count(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE t1.datein LIKE CONCAT(date_select,'%') AND t1.cartype < 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT count(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE t1.datein LIKE CONCAT(date_select,'%') AND t1.cartype = 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype < 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype = 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE t1.dateout LIKE CONCAT(date_select,'%') AND t2.cartype > 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += ")); "
                    + "     END IF;"
                    + "     SET date_select = DATE_ADD(date_select,INTERVAL 1 DAY); "

                    + "   END WHILE; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perDay; "
                    + " CALL dowhile11('" + startDateTimeText + "','" + endDateTimeText + "');";

                    sql += "   SELECT days as 'วันที่', inVisitor as 'ปริมาณรถชั่วคราว', inMember as 'ปริมาณรถประจำ',othercar as 'ปริมาณรถUnknowType',(inVisitor+inMember+othercar) as 'รวมจำนวนรถ' FROM perDay where (inVisitor+inMember+othercar) > 0;";
                    break;

                case 120:
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");
                    //sql = "DROP PROCEDURE IF EXISTS dowhile12; "
                    //+ " CREATE PROCEDURE dowhile12(IN date_select DATETIME, IN date_finish DATETIME) "
                    //+ " BEGIN "
                    //+ "   DECLARE num INT DEFAULT 0; "
                    //    //+ "   CREATE TABLE perHour (hours varchar(30),carin INT(1),carout INT(1)); "
                    //+ "   CREATE TABLE perMonth (hours varchar(30),inVisitor INT(1),inMember INT(1), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                    //+ "   WHILE num < 24 DO "
                    //+ "     INSERT INTO perMonth VALUES ( CONCAT(DATE_FORMAT(MAKETIME(num,0,0),'%H:%i'),' - ',DATE_FORMAT(MAKETIME(num,59,0),'%H:%i')), "
                    //+ "     (SELECT COUNT(no) FROM recordin WHERE HOUR(datein) = num AND MONTH(datein) = MONTH(date_select) AND YEAR(datein) = YEAR(date_select) AND cartype < 200), "
                    //+ "     (SELECT COUNT(no) FROM recordin WHERE HOUR(datein) = num AND MONTH(datein) = MONTH(date_select) AND YEAR(datein) = YEAR(date_select) AND cartype = 200), "
                    //+ "     (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE HOUR(t1.dateout) = num AND MONTH(t1.dateout) = MONTH(date_select) AND YEAR(t1.dateout) = YEAR(date_select) AND t2.cartype < 200), "
                    //+ "     (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no WHERE HOUR(t1.dateout) = num AND MONTH(t1.dateout) = MONTH(date_select) AND YEAR(t1.dateout) = YEAR(date_select) AND t2.cartype = 200), "
                    //+ "     (SELECT COUNT(no) FROM recordin WHERE HOUR(datein) = num AND MONTH(datein) = MONTH(date_select) AND YEAR(datein) = YEAR(date_select) AND cartype > 200));"
                    //+ "     SET num = num + 1; "
                    //+ "   END WHILE; "
                    //    //+ "   SELECT hours as ชั่วโมง ,carin as รถเข้า ,carout as รถออก FROM perHour; "
                    //    //+ "   SELECT hours as ชั่วโมง, inVisitor as ลูกค้าทั่วไปเข้า, inMember as สมาชิกเข้า, outVisitor as ลูกค้าทั่วไปออก, outMember as สมาชิกออก FROM perHour; "
                    //+ " END; "
                    //+ " DROP TABLE IF EXISTS perMonth; "
                    //+ " CALL dowhile12('" + startDateTimeText + "','" + endDateTimeText + "');";

                    sql = "DROP PROCEDURE IF EXISTS dowhile12; "
                    + " CREATE PROCEDURE dowhile12(IN date_select DATETIME, IN date_finish DATETIME) "
                    + " BEGIN "
                    + "   DECLARE num INT DEFAULT 0; "
                    + "   CREATE TABLE perMonth (hours varchar(30),inVisitor INT(1),inMember INT(1), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                    + "   WHILE num < 24 DO "
                    + "     INSERT INTO perMonth VALUES ( CONCAT(DATE_FORMAT(MAKETIME(num,0,0),'%H:%i'),' - ',DATE_FORMAT(MAKETIME(num,59,0),'%H:%i')), "
                    + "     (SELECT COUNT(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE HOUR(t1.datein) = num AND MONTH(t1.datein) = MONTH(date_select) AND YEAR(t1.datein) = YEAR(date_select) AND t1.cartype < 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT COUNT(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE HOUR(t1.datein) = num AND MONTH(t1.datein) = MONTH(date_select) AND YEAR(t1.datein) = YEAR(date_select) AND t1.cartype = 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE HOUR(t1.dateout) = num AND MONTH(t1.dateout) = MONTH(date_select) AND YEAR(t1.dateout) = YEAR(date_select) AND t2.cartype < 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid WHERE HOUR(t1.dateout) = num AND MONTH(t1.dateout) = MONTH(date_select) AND YEAR(t1.dateout) = YEAR(date_select) AND t2.cartype = 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "), "
                    + "     (SELECT COUNT(t1.no) FROM recordin t1 left join member t2 on t1.id = t2.cardid WHERE HOUR(t1.datein) = num AND MONTH(t1.datein) = MONTH(date_select) AND YEAR(t1.datein) = YEAR(date_select) AND t1.cartype > 200";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t2.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "));"
                    + "     SET num = num + 1; "
                    + "   END WHILE; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perMonth; "
                    + " CALL dowhile12('" + startDateTimeText + "','" + endDateTimeText + "');";

                    sql += "   SELECT hours as 'เวลา', inVisitor as 'ปริมาณรถชั่วคราว', inMember as 'ปริมาณรถประจำ', othercar as 'ปริมาณรถUnknowType',(inVisitor+inMember+othercar) as 'รวมจำนวนรถ' FROM perMonth; ";
                    break;

                case 121:
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");
                    //sql = "DROP PROCEDURE IF EXISTS dowhile13;"
                    //+ " CREATE PROCEDURE dowhile13(IN date_select DATETIME, IN date_finish DATETIME)"
                    //+ " BEGIN DECLARE num INT DEFAULT 0;"
                    //+ " CREATE TABLE perDaySumTime(hours varchar(30), outVisitor INT(1), outMember INT(1), othercar INT(1),dateselect varchar(30));"
                    //+ " WHILE num < 25 DO"
                    //+ " if num < 24 then"
                    //+ " INSERT INTO perDaySumTime VALUES(CONCAT(num, ' ชั่วโมง - ', num + 1, ' ชั่วโมง'),"
                    //+ " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no"

                    //+ " WHERE t2.cartype < 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) = num  AND  t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'),  "
                    //+ " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no"
                    //+ " WHERE t2.cartype = 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) = num AND t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'),  "
                    //+ " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no"
                    //+ " WHERE t2.cartype > 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) = num AND t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'),null);"

                    //+ " ELSE"
                    // + " INSERT INTO perDaySumTime VALUES('มากกว่า 24 ชั่วโมง',"
                    //+ " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no"

                    //+ " WHERE t2.cartype < 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) >= num  AND  t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'),  "
                    //+ " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no"
                    //+ " WHERE t2.cartype = 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) >= num AND t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'),  "
                    //+ " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no"
                    //+ " WHERE t2.cartype > 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) >= num AND t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'),null);"

                    //+ " END IF;"
                    //+ " SET num = num + 1;"

                    //+ " END WHILE;"
                    //+ " END;"
                    //+ " DROP TABLE IF EXISTS perDaySumTime; CALL dowhile13('" + startDateTimeText + " 00:00:00', '" + endDateTimeText + " 23:59:59');";

                    sql = "DROP PROCEDURE IF EXISTS dowhile13;"
                    + " CREATE PROCEDURE dowhile13(IN date_select DATETIME, IN date_finish DATETIME)"
                    + " BEGIN DECLARE num INT DEFAULT 0;"
                    + " CREATE TABLE perDaySumTime(hours varchar(30), outVisitor INT(1), outMember INT(1), othercar INT(1),dateselect varchar(30));"
                    + " WHILE num < 25 DO"
                    + " if num < 24 then"
                    + " INSERT INTO perDaySumTime VALUES(CONCAT(num, ' ชั่วโมง - ', num + 1, ' ชั่วโมง'),"
                    + " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid "
                    + " WHERE t2.cartype < 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) = num  AND  t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "),  "
                    + " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid "
                    + " WHERE t2.cartype = 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) = num AND t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "),  "
                    + " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid "
                    + " WHERE t2.cartype > 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) = num AND t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "),null);"

                    + " ELSE"
                     + " INSERT INTO perDaySumTime VALUES('มากกว่า 24 ชั่วโมง',"
                    + " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid "
                    + " WHERE t2.cartype < 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) >= num  AND  t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "),  "
                    + " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid "
                    + " WHERE t2.cartype = 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) >= num AND t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "),  "
                    + " (SELECT COUNT(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t2.id = t3.cardid "
                    + " WHERE t2.cartype > 200 AND format((((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein)) / 60) / 60), 0) >= num AND t1.dateout BETWEEN '" + startDateTimeText + " 00:00:00' AND '" + endDateTimeText + " 23:59:59'";
                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += "),null);"

                    + " END IF;"
                    + " SET num = num + 1;"

                    + " END WHILE;"
                    + " END;"
                    + " DROP TABLE IF EXISTS perDaySumTime; CALL dowhile13('" + startDateTimeText + " 00:00:00', '" + endDateTimeText + " 23:59:59');";

                    sql += " SELECT hours as 'เวลา',outVisitor as 'ปริมาณรถประจำ' ,outMember as 'ปริมาณรถชั่วคราว',othercar as 'ปริมาณรถUnknow',(outVisitor+outMember+othercar) as 'รวมจำนวนรถ',dateselect as 'วันที่' from perDaySumTime";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();

                    break;

                case 122:
                    AppGlobalVariables.ConditionText = "";
                    sql = "SELECT A.no as ลำดับ,C.name as ชื่อเจ้าหน้าที่,";
                    sql += "B.guardhouse as 'ตำแหน่งป้อม',";
                    sql += "date_format(A.datein, '%d/%m/%Y %H:%i:%s') as วันที่เวลาเข้า,date_format(A.dateout, '%d/%m/%Y %H:%i:%s') as วันที่เวลาออก,"; //Mac 2018/12/21
                    sql += " CASE WHEN B.price > 0 THEN B.price ELSE 0 END as 'ยอดรวมจากระบบ(บาท)',";
                    sql += " CASE WHEN B.price > 0 THEN B.price ELSE 0 END as 'ยอดรวมเงินนำส่ง(บาท)',";
                    sql += " CASE WHEN B.discount > 0 THEN B.discount ELSE 0 END as ส่วนลด FROM";
                    sql += " user C,user_record A";
                    sql += " LEFT JOIN(";
                    sql += " SELECT SUM(price) AS price";
                    sql += " ,SUM(discount) AS discount";
                    sql += " ,userno,guardhouse FROM recordout";
                    sql += " GROUP BY userno) B";
                    sql += " ON A.no = B.userno";
                    sql += " WHERE C.id=A.id";
                    sql += " AND A.datein BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND A.id =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and B.guardhouse = '" + guardhouse + "' ";
                    sql += " ORDER BY A.no";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 123:
                    AppGlobalVariables.ConditionText = "";

                    sql = "select ifnull(t1.guardhouse,'-') as 'ตำแหน่งป้อม', ifnull((select name from user where id = t1.id),'-') as 'ชื่อเจ้าหน้าที่'";
                    sql += ", date_format(t1.datein, '%d/%m/%Y %H:%i:%s') as 'วันที่เวลาเข้า', date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วันที่เวลาออก', (select count(t2.no) from recordout t2 left join recordin t3 on t2.no = t3.no where t3.cartype = 200 and t2.userout = t1.id and t2.userno = t1.no) as 'Member'"; //Mac 2018/12/21
                    sql += ", (select count(t2.no) from recordout t2 left join recordin t3 on t2.no = t3.no where t3.cartype < 200 and t2.userout = t1.id and t2.userno = t1.no) as 'Visitor'";
                    sql += ", (select count(t2.no) from recordout t2 where proid > 0 and t2.userout = t1.id and t2.userno = t1.no) as 'E-Stamp'";
                    sql += ", (select count(*) from liftrecord where gate = 'O' and userid = t1.id and datelift >= t1.datein and datelift <= t1.dateout) as 'ยกไม้ฉุกเฉิน(ครั้ง)'";
                    sql += ", ifnull((select sum(t2.price) - sum(t2.losscard) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ค่าจอด'";
                    sql += ", ifnull((select sum(t2.losscard) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ค่าปรับ'";
                    sql += ", ifnull((select sum(t2.price) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ยอดรวมจากระบบ(บาท)'";
                    sql += ", t1.price as 'ยอดรวมเงินนำส่ง(บาท)',ABS( (t1.price) - ifnull((select sum(t2.price) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0)) as 'ส่วนต่าง'";
                    sql += " from user_record t1";
                    sql += " where t1.datein BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "' and t1.guardhouse is not null and t1.guardhouse <> '' and upper(t1.guardhouse) <> 'SERVER'";
                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND t1.id =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";

                    sql += " order by t1.no";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 124:
                    AppGlobalVariables.ConditionText = "";
                    sql = "select ifnull(t1.guardhouse,'-') as 'ตำแหน่งป้อม', ifnull((select name from user where id = t1.id),'-') as 'ชื่อเจ้าหน้าที่'";
                    sql += ", date_format(t1.datein, '%d/%m/%Y %H:%i:%s') as 'วันที่เวลาเข้า', date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วันที่เวลาออก', (select count(t2.no) from recordout t2 left join recordin t3 on t2.no = t3.no where t3.cartype = 200 and t2.userout = t1.id and t2.userno = t1.no) as 'Member'"; //Mac 2018/12/21
                    sql += ", (select count(t2.no) from recordout t2 left join recordin t3 on t2.no = t3.no where t3.cartype < 200 and t2.userout = t1.id and t2.userno = t1.no) as 'Visitor'";
                    sql += ", (select count(t2.no) from recordout t2 where proid > 0 and t2.userout = t1.id and t2.userno = t1.no) as 'E-Stamp'";
                    sql += ", (select count(*) from liftrecord where gate = 'O' and userid = t1.id and datelift >= t1.datein and datelift <= t1.dateout) as 'ยกไม้ฉุกเฉิน(ครั้ง)'";
                    sql += ", ifnull((select sum(t2.price) - sum(t2.losscard) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ค่าจอด'";
                    sql += ", ifnull((select sum(t2.losscard) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ค่าปรับ'";
                    sql += ", ifnull((select sum(t2.price) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0) as 'ยอดรวมจากระบบ(บาท)'";
                    sql += ", t1.price as 'ยอดรวมเงินนำส่ง(บาท)',ABS( (t1.price) - ifnull((select sum(t2.price) from recordout t2 where t2.userout = t1.id and t2.userno = t1.no), 0)) as 'ส่วนต่าง'";
                    sql += ", TIME_FORMAT(TIMEDIFF(t1.dateout,t1.datein),'%H.%i') as 'จำนวนชั่วโมง'";
                    sql += " from user_record t1";
                    sql += " where t1.datein BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "' and t1.guardhouse is not null and t1.guardhouse <> '' and upper(t1.guardhouse) <> 'SERVER'";
                    if (user != Constants.TextBased.All)
                    {
                        sql += " AND t1.id =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    }
                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";

                    sql += " order by t1.no";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 125:
                    sql = "select concat(date_format(t2.dateout,'%d/%m/'), date_format(t2.dateout,'%Y') + 543) as วันที่";

                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2019/08/27
                    {
                        sql += " , format((sum(t2.price-t2.losscard) - ROUND(sum(t2.price-t2.losscard)*7/107, 2)) - (sum(t2.overdate) - ROUND(sum(t2.overdate)*7/107, 2)), 2) as ค่าจอดรถ";
                        sql += " , format(sum(t2.losscard), 2) as ค่าปรับบัตรหาย";
                        sql += " , format(sum(t2.overdate) - ROUND(sum(t2.overdate)*7/107, 6), 2) as ค่าปรับค้างคืน";
                        sql += " , format(sum((t2.price-t2.losscard)) - ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ค่าบริการ";
                        sql += " , format(ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as VAT";
                        sql += " , format(sum((t2.price-t2.losscard)), 2) as รวมเงิน";
                    }
                    else
                    {
                        sql += " , format((sum(t2.price) - ROUND(sum(t2.price)*7/107, 2)) - ((sum(t2.losscard) - ROUND(sum(t2.losscard)*7/107, 2)) + (sum(t2.overdate) - ROUND(sum(t2.overdate)*7/107, 2))), 2) as ค่าจอดรถ";
                        sql += " , sum(t2.price) - (sum(t2.losscard) + sum(t2.overdate)) as ค่าจอดรถNOVAT";
                        sql += " , format(sum(t2.losscard) - ROUND(sum(t2.losscard)*7/107, 6), 2) as ค่าปรับบัตรหาย";
                        sql += " , sum(t2.losscard) as ค่าปรับบัตรหายNOVAT";
                        sql += " , format(sum(t2.overdate) - ROUND(sum(t2.overdate)*7/107, 6), 2) as ค่าปรับค้างคืน";
                        sql += " , sum(t2.overdate) as ค่าปรับค้างคืนNOVAT";
                        sql += " , format(sum(t2.price) - ROUND(sum(t2.price)*7/107, 6), 2) as ค่าบริการ";
                        sql += " , format(ROUND(sum(t2.price)*7/107, 6), 2) as VAT";
                        sql += " , format(sum(t2.price), 2) as รวมเงิน";
                    }

                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/10
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " group by date_format(t2.dateout,'%Y-%m-%d')";
                    sql += " order by date_format(t2.dateout,'%Y-%m-%d')";
                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 126:
                    //sql = "select memkey as 'รหัสประจำตัว',cast(cardid as char) as 'หมายเลขบัตร',name as 'ชื่อ-สกุลผู้ถือบัตร',address as 'ที่อยู่'";
                    //sql += ",license as 'ทะเบียนรถ',(select typename from cartype where cartype.typeid = member.cartype) as 'ประเภทผู้ถือบัตร'";
                    //sql += ",tel as 'เบอร์โทรศัพท์',DATE_FORMAT(datestart,'%d/%m/%Y %H:%i:%s') as 'วันที่สร้างบัตร',DATE_FORMAT(dateexprie,'%d/%m/%Y %H:%i:%s') as 'วันที่บัตรหมดอายุ',case when enable = 'True' then 'ใช้งาน' else 'ยกเลิก' end as 'สถานะ'";
                    //sql += ",(select groupname from membergroup where membergroup.id = member.memgroupid) as 'กลุ่มพนักงาน'  from member";

                    sql = "select lpad(memkey,10,0) as 'รหัสประจำตัว',cast(ifnull((select name_on_card from cardpx where name = member.cardid),(select name_on_card from cardmf where name = member.cardid)) as char) as 'หมายเลขบัตร',";
                    sql += "name as 'ชื่อ-สกุลผู้ถือบัตร',address as 'ที่อยู่',license as 'ทะเบียนรถ',";
                    sql += "(select cardtype.name from cardtype where cardtype.id = member.cardtypeid) as 'ประเภทผู้ถือบัตร',";
                    sql += "tel as 'เบอร์โทรศัพท์',DATE_FORMAT(datestart,'%d/%m/%Y %H:%i:%s') as 'วันที่สร้างบัตร',DATE_FORMAT(dateexprie,'%d/%m/%Y %H:%i:%s') as 'วันที่บัตรหมดอายุ',";
                    sql += "case when enable = 'True' then 'ใช้งาน' else 'ยกเลิก' end as 'สถานะ',";
                    sql += "(select store_name from m_store where m_store.store_id = member.storeid) as 'กลุ่มพนักงาน' ";
                    sql += " from member ";
                    if (cardId.Length > 0)
                        sql += "  WHERE member.cardid = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and member.storeid = (select store_id from m_store where store_name = '" + memberGroupMonth + "')";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and member.license LIKE '%" + licensePlate + "%'";
                    sql += " order by member.name";
                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 127:
                    sql = "select  cast(ifnull((select name_on_card from cardpx where name = t1.id),(select name_on_card from cardmf where name = t1.id)) as char) as 'เลขบัตร',";
                    sql += "case when t1.cartype <> '200' then 'Visitor Card' else (select member.name from member where member.id = t1.id) end as 'ชื่อ-สกุลผู้ถือบัตร',";
                    sql += "t1.license as 'ทะเบียน',(select cartype.typename from cartype where t1.cartype  =  cartype.typeid) as 'ประเภท',date_format(t1.datein, '%d/%m/%Y %H:%i:%s') as 'วันที่-เวลาเข้า',";
                    sql += "date_format(t2.dateout, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก',";
                    sql += "t1.guardhouse as 'ประตูเข้า', (select lpad(user.cardname,4,0) from user where user.id = t1.userin) as 'พนักงานเข้า',";
                    sql += "t2.guardhouse as 'ประตูออก', (select lpad(user.cardname,4,0) from user where user.id = t2.userout) as 'พนักงานออก',ROUND(TIME_FORMAT(TIMEDIFF(t2.dateout,t1.datein),'%H.%i'),2) as 'เวลาจอด',";
                    sql += "cast(t2.proid as char) as 'ตราประทับ'";
                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2019/08/27
                    {
                        sql += " , format(((t2.price-t2.losscard) - ROUND((t2.price-t2.losscard)*7/107, 2)) - ((t2.overdate) - ROUND((t2.overdate)*7/107, 2)), 2) as ค่าจอดรถ";
                        sql += " , format((t2.losscard), 2) as ค่าปรับบัตรหาย";
                        sql += " , format((t2.overdate) - ROUND((t2.overdate)*7/107, 6), 2) as ค่าปรับค้างคืน";
                        sql += " , format((t2.price), 2) as เงินเรียกเก็บ";
                        sql += " , format(ROUND(((t2.price-t2.losscard))*7/107, 6), 2) as VAT";
                        sql += " , format(((t2.price)), 2) as รวมเงินสด";
                    }
                    else
                    {
                        sql += " , format(((t2.price) - ROUND((t2.price)*7/107, 2)) - (((t2.losscard) - ROUND((t2.losscard)*7/107, 2)) + ((t2.overdate) - ROUND((t2.overdate)*7/107, 2))), 2) as ค่าจอดรถ";
                        sql += " , format((t2.price  - cast((t2.overdate + t2.losscard) as signed)),2) as ค่าจอดรถNOVAT";
                        sql += " , format((t2.losscard) - ROUND((t2.losscard)*7/107, 6), 2) as ค่าปรับบัตรหาย";
                        sql += " , t2.losscard as ค่าปรับบัตรหายNOVAT";
                        sql += " , format((t2.overdate) - ROUND((t2.overdate)*7/107, 6), 2) as ค่าปรับค้างคืน";
                        sql += " , t2.overdate  as ค่าปรับค้างคืนNOVAT";
                        sql += " , format((t2.price), 2) as เงินเรียกเก็บ";
                        sql += " , format(ROUND((t2.price)*7/107, 6), 2) as VAT";
                        sql += " , t2.price as รวมเงินสด";
                    }


                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        //sql += " left join member t3 on t1.license = t3.license";
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/10
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;

                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and t1.license LIKE '%" + licensePlate + "%'";

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    if (cardId.Length > 0)
                        sql += "  and t1.id = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')) as char)";

                    //sql += " group by date_format(t2.dateout,'%Y-%m-%d')";
                    sql += " order by t1.datein";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 128:
                    sql = "select  cast(ifnull((select name_on_card from cardpx where name = t1.id),(select name_on_card from cardmf where name = t1.id)) as char) as 'เลขบัตร',";
                    sql += "case when t1.cartype <> '200' then 'Visitor Card' else (select member.name from member where member.id = t1.id) end as 'ชื่อ-สกุลผู้ถือบัตร',";
                    sql += "t1.license as 'ทะเบียน',(select cartype.typename from cartype where t1.cartype  =  cartype.typeid) as 'ประเภท',date_format(t1.datein, '%d/%m/%Y %H:%i:%s') as 'วันที่-เวลาเข้า',";
                    sql += "date_format(t2.dateout, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก',";
                    sql += "t1.guardhouse as 'ประตูเข้า', (select lpad(user.cardname,4,0) from user where user.id = t1.userin) as 'พนักงานเข้า',";
                    sql += "t2.guardhouse as 'ประตูออก', (select lpad(user.cardname,4,0) from user where user.id = t2.userout) as 'พนักงานออก',TIME_FORMAT(TIMEDIFF(t2.dateout,t1.datein),'%H.%i') as 'เวลาจอด',";
                    sql += "cast(t2.proid as char) as 'ตราประทับ'";
                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2019/08/27
                    {
                        sql += " , format(((t2.price-t2.losscard) - ROUND((t2.price-t2.losscard)*7/107, 2)) - ((t2.overdate) - ROUND((t2.overdate)*7/107, 2)), 2) as ค่าจอดรถ";
                        sql += " , format((t2.losscard), 2) as ค่าปรับบัตรหาย";
                        sql += " , format((t2.overdate) - ROUND((t2.overdate)*7/107, 6), 2) as ค่าปรับค้างคืน";
                        sql += " , format((t2.price), 2) as เงินเรียกเก็บ";
                        sql += " , format(ROUND(((t2.price-t2.losscard))*7/107, 6), 2) as VAT";
                        sql += " , format(((t2.price)), 2) as รวมเงินสด";
                    }
                    else
                    {
                        sql += " , format(((t2.price) - ROUND((t2.price)*7/107, 2)) - (((t2.losscard) - ROUND((t2.losscard)*7/107, 2)) + ((t2.overdate) - ROUND((t2.overdate)*7/107, 2))), 2) as ค่าจอดรถ";
                        sql += " , (t2.price  - (t2.losscard + t2.overdate)) as ค่าจอดรถNOVAT";
                        sql += " , format((t2.losscard) - ROUND((t2.losscard)*7/107, 6), 2) as ค่าปรับบัตรหาย";
                        sql += " , t2.losscard as ค่าปรับบัตรหายNOVAT";
                        sql += " , format((t2.overdate) - ROUND((t2.overdate)*7/107, 6), 2) as ค่าปรับค้างคืน";
                        sql += " , t2.overdate  as ค่าปรับค้างคืนNOVAT";
                        sql += " , format((t2.price), 2) as เงินเรียกเก็บ";
                        sql += " , format(ROUND((t2.price)*7/107, 6), 2) as VAT";
                        sql += " , t2.price as รวมเงินสด";
                    }



                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        //sql += " left join member t3 on t1.license = t3.license";
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/10
                    sql += " where t1.datein between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.dateout is null";


                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;

                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and t1.license LIKE '%" + licensePlate + "%'";

                    if (cardId.Length > 0)
                        sql += "  and t1.id = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'), (select name from cardmf where name_on_card = '" + cardId + "')) as char)";

                    if (guardhouse != String.Empty) //Mac 2019/11/14
                        sql += " and t1.guardhouse = '" + guardhouse + "' ";

                    if (user != Constants.TextBased.All)
                        sql += " and t1.userin =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    //sql += " group by date_format(t2.dateout,'%Y-%m-%d')";
                    sql += " order by t1.datein";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();

                    break;

                case 129:
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd HH:mm:ss");
                    sql = "select cast(ifnull((select name_on_card from cardpx where name = member.cardid),(select name_on_card from cardmf where name = member.cardid)) as char) as 'หมายเลขบัตร',(select cardtype.name from cardtype where cardtype.id = member.cardtypeid) as 'ประเภทบัตร',";
                    sql += "lpad(member.memkey,10,0) as 'รหัสประจำตัว',member.name as 'ผู้ถือบัตร',date_format(datestart,'%d/%m/%Y %H:%i:%s') as 'วันที่ออกบัตร',date_format(dateexprie,'%d/%m/%Y %H:%i:%s') as 'วันที่หมดอายุ'  ";
                    sql += ",(select (select user.name from user where member_record.user = user.id ) from member_record where member_record.cardid = member.cardid order by member_record.id DESC LIMIT 1) as 'พนักงานออกบัตร'";
                    sql += " from member where dateexprie BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";


                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and member_record.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " and member.cardid = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    if (carType != Constants.TextBased.All)
                        sql += "  and member.typeid = (select typeid from cartype where typename LIKE '%" + carType + "%')";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and member.storeid = (select store_id from m_store where store_name = '" + memberGroupMonth + "')";
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and member.cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and member.renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];
                    if (memberProcessState == Constants.TextBased.CreateNewMemberProcessState) //Mac 2020/08/10



                        sql += " order by member.dateexprie,member.id ASC";
                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;
                case 130:
                    sql = "select cast(ifnull((select name_on_card from cardpx where name = member.cardid),(select name_on_card from cardmf where name = member.cardid)) as char) as 'หมายเลขบัตร'";
                    sql += ",(select cardtype.name from cardtype where cardtype.id = member.cardtypeid ) as 'ประเภทบัตร' ";
                    sql += ",member.memkey as 'รหัสประจำตัว' ,member_record.name as 'ผู้ถือบัตร',member.datestart as 'วันที่ออกบัตร',member_record.datepay as 'วันที่หมดอายุ',";
                    sql += "(select user.name from user where user.id = member_record.user and status = 'A') as 'พนักงานออกบัตร'";
                    sql += ",(select user.name from user where user.id = member_record.user and status = 'C') as 'พนักงานที่ยกเลิก'";
                    sql += " from member_record left JOIN member on member_record.cardid = member.cardid where  member_record.status = 'C'";
                    sql += " and member_record.datepay  BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if (user != Constants.TextBased.All)
                        sql += " and member_record.user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and member_record.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " and member_record.cardid = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    if (carType != Constants.TextBased.All)
                        sql += "  and member.typeid = (select typeid from cartype where typename LIKE '%" + carType + "%')";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and member.storeid = (select store_id from m_store where store_name = '" + memberGroupMonth + "')";
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and member.cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and member.renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];

                    sql += " order by member_record.datepay ASC";
                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 131:
                    startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd");
                    endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd");

                    //sql = "DROP PROCEDURE IF EXISTS dowhile11; "
                    //+ " CREATE PROCEDURE dowhile11(IN date_select DATE, IN date_finish DATE) "
                    //+ " BEGIN "
                    //    //+ "   CREATE TABLE perDay (days varchar(30),carin INT(1),carout INT(1)); "
                    //+ "   CREATE TABLE perDay (days varchar(30),timeVis decimal(10,2),timeMem decimal(10,2),timeOth decimal(10,2), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                    //+ "   WHILE DATE(date_select) <= DATE(date_finish) DO "
                    //+ "     INSERT INTO perDay VALUES(date_select, "
                    //    //+ "     (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND cartype < 200), "
                    //    //+ "     (SELECT count(no) FROM recordin WHERE datein LIKE CONCAT(date_select,'%') AND cartype = 200), "
                    //+ "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout  AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select) AND t2.cartype < 200),"
                    //+ "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype = 200), "
                    //+ "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype > 200), "
                    //+ "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype < 200), "
                    //+ "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype = 200), "
                    //+ "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype > 200)); "
                    //+ "     SET date_select = DATE_ADD(date_select,INTERVAL 1 DAY); "
                    //+ "   END WHILE; "
                    //    //+ "   SELECT days as วันที่ ,carin as รถเข้า ,carout as รถออก FROM perDay; "
                    //    //+ "   SELECT days as วันที่, inVisitor as ลูกค้าทั่วไปเข้า, inMember as สมาชิกเข้า, outVisitor as ลูกค้าทั่วไปออก, outMember as สมาชิกออก FROM perDay; "
                    //+ " END; "
                    //+ " DROP TABLE IF EXISTS perDay; "
                    //+ " CALL dowhile11('" + startDateTimeText + "','" + endDateTimeText + "');";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                    {
                        //sql = "DROP PROCEDURE IF EXISTS dowhile11; "
                        //+ " CREATE PROCEDURE dowhile11(IN date_select DATE, IN date_finish DATE) "
                        //+ " BEGIN "
                        //+ "   CREATE TABLE perDay (days varchar(30),timeVis decimal(10,2),timeMem decimal(10,2),timeOth decimal(10,2), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                        //+ "   WHILE DATE(date_select) <= DATE(date_finish) DO "
                        //+ "     INSERT INTO perDay VALUES(date_select, "
                        //+ "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.license = t3.license WHERE t1.dateout  AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select) AND t2.cartype < 200";
                        //if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        //    sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        //sql += "),"
                        //+ "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.license = t3.license WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype = 200";
                        //if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        //    sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        //sql += "), "
                        //+ "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.license = t3.license WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype > 200";
                        //if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        //    sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        //sql += "), "
                        //+ "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.license = t3.license WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype < 200";
                        //if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        //    sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        //sql += "), "
                        //+ "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.license = t3.license WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype = 200";
                        //if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        //    sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        //sql += "), "
                        //+ "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.license = t3.license WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype > 200";
                        //if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        //    sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                        //Mac 2025/03/14
                        sql = "DROP PROCEDURE IF EXISTS dowhile11; "
                        + " CREATE PROCEDURE dowhile11(IN date_select DATE, IN date_finish DATE) "
                        + " BEGIN "
                        + "   CREATE TABLE perDay (days varchar(30),timeVis decimal(10,2),timeMem decimal(10,2),timeOth decimal(10,2), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                        + "   WHILE DATE(date_select) <= DATE(date_finish) DO "
                        + "     INSERT INTO perDay VALUES(date_select, "
                        + "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t3.license like concat('%',t1.license,'%') WHERE t1.dateout  AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select) AND t2.cartype < 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "),"
                        + "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t3.license like concat('%',t1.license,'%') WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype = 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), "
                        + "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t3.license like concat('%',t1.license,'%') WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype > 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), "
                        + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t3.license like concat('%',t1.license,'%') WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype < 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), "
                        + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t3.license like concat('%',t1.license,'%') WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype = 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), "
                        + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t3.license like concat('%',t1.license,'%') WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype > 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    }
                    else
                    {
                        sql = "DROP PROCEDURE IF EXISTS dowhile11; "
                        + " CREATE PROCEDURE dowhile11(IN date_select DATE, IN date_finish DATE) "
                        + " BEGIN "
                        + "   CREATE TABLE perDay (days varchar(30),timeVis decimal(10,2),timeMem decimal(10,2),timeOth decimal(10,2), outVisitor INT(1), outMember INT(1),othercar INT(1)); "
                        + "   WHILE DATE(date_select) <= DATE(date_finish) DO "
                        + "     INSERT INTO perDay VALUES(date_select, "
                        + "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout  AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select) AND t2.cartype < 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "),"
                        + "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype = 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), "
                        + "     (SELECT concat(floor((sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor(sum(UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype > 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), "
                        + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype < 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), "
                        + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype = 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                        sql += "), "
                        + "     (SELECT count(t1.no) FROM recordout t1 LEFT JOIN recordin t2 ON t1.no = t2.no left join member t3 on t1.id = t3.cardid WHERE t1.dateout AND Date(t1.dateout ) = Date(date_select) AND Month(t1.dateout ) = Month(date_select) AND Year(t1.dateout ) = Year(date_select)   AND t2.cartype > 200";
                        if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                            sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    }
                    sql += ")); "
                    + "     SET date_select = DATE_ADD(date_select,INTERVAL 1 DAY); "
                    + "   END WHILE; "
                    + " END; "
                    + " DROP TABLE IF EXISTS perDay; "
                    + " CALL dowhile11('" + startDateTimeText + "','" + endDateTimeText + "');";

                    sql += "   SELECT days as 'วันที่',case when timeVis is null then 0 else timeVis end as 'เวลาจอดทั้งหมดชั่วคราว',";
                    sql += " case when timeMem is null then 0 else timeMem end as 'เวลาจอดทั้งหมดประจำ',";
                    sql += " case when timeOth is null then 0 else timeOth end as 'เวลาจอดทั้งหมดUnknow'";
                    sql += ",outVisitor as 'ปริมาณรถชั่วคราว',outMember as 'ปริมาณรถประจำ',othercar as 'ปริมาณรถUnknow',";
                    sql += "  case when timeVis is null then 0 else timeVis /outVisitor end as 'เวลาจอดเฉลี่ยชั่วคราว',";
                    sql += "  case when timeMem is null then 0 else timeMem /outMember end as 'เวลาจอดเฉลี่ยประจำ',";
                    sql += "  case when timeOth is null then 0 else timeOth /othercar end as 'เวลาจอดเฉลี่ยUnknow'";
                    sql += " FROM perDay where (outVisitor+outMember+othercar) > 0;";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 132:

                    sql = "select date_format(aa,'%d/%m/%Y') as 'วันที่', bb as 'เลขที่ใบกำกับภาษีเริ่มต้น', cc as 'เลขที่ใบกำกับภาษีสิ้นสุด', dd as 'รหัสเครื่องคิดเงิน'";
                    sql += ", ee as 'ค่าบริการ', ff as 'VAT', gg as 'รวมเงิน' from ";


                    sql += "(select t2.dateout as 'aa'";
                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(min(t2.printno),6,'0'))) as 'bb'";
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad(max(t2.printno),6,'0'))) as 'cc'";
                        }
                        else
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(min(t2.printno),6,'0'))) as 'bb'";
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad(max(t2.printno),6,'0'))) as 'cc'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m') ,lpad(min(t2.printno),6,'0'))) as 'bb'";
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m') ,lpad(max(t2.printno),6,'0'))) as 'cc'";
                        }
                        else
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y') ,lpad(min(t2.printno),6,'0'))) as 'bb'";
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y') ,lpad(max(t2.printno),6,'0'))) as 'cc'";
                        }
                    }
                    sql += " , t2.posid as 'dd'";

                    if (Configs.Reports.ReportPriceSplitLosscard)
                    {
                        sql += " , ROUND(sum((t2.price-t2.losscard)) - ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ee";
                        sql += " , ROUND(ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ff";
                        sql += " , ROUND(sum((t2.price-t2.losscard)), 2) as gg";
                    }
                    else
                    {
                        sql += " , ROUND(sum(t2.price) - ROUND(sum(t2.price)*7/107, 6), 2) as 'ee'";
                        sql += " , ROUND(ROUND(sum(t2.price)*7/107, 6), 2) as 'ff'";
                        sql += " , ROUND(sum(t2.price), 2) as 'gg'";
                    }
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        //sql += " left join member t3 on t1.license = t3.license";
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/10
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    if (guardhouse != String.Empty)
                        sql += " and t2.guardhouse = '" + guardhouse + "' ";

                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and t1.cartype != 200";

                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    if (Configs.UseReceiptFor1Out)
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d'), t2.posid, t2.receipt";
                    }
                    else
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d'), t2.posid";
                    }

                    sql += " union";
                    sql += " select datepay as 'aa'";
                    sql += " , concat(receipt, concat(date_format(datepay, '%y'), lpad(min(printno), 6, '0'))) as 'bb'";
                    sql += " , concat(receipt, concat(date_format(datepay, '%y'), lpad(max(printno), 6, '0'))) as 'cc'";
                    sql += " , posid as 'dd'";
                    sql += " , ROUND(sum(price) - ROUND(sum(price)*7/107, 6), 2) as 'ee'";
                    sql += " , ROUND(ROUND(sum(price)*7/107, 6), 2) as 'ff'";
                    sql += " , ROUND(sum(price), 2) as 'gg'";
                    sql += " from member_record ";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and printno > 0";
                    if (Configs.UseVoidSlip)
                        sql += " and status != 'V'";
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (user != Constants.TextBased.All)
                        sql += " and user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " group by date_format(datepay,'%Y-%m-%d'), posid";

                    sql += ") tt order by date_format(aa,'%Y-%m-%d'), dd,cc";



                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();

                    break;


                case 133:
                    sql = "select date_format(aa,'%d/%m/%Y') as 'วันที่', bb as 'ชื่อเจ้าหน้าที่', dd as 'รหัสเครื่องคิดเงิน'";
                    sql += ", ee as 'ค่าบริการ', ff as 'VAT', gg as 'รวมเงิน' from ";


                    sql += "(select t2.dateout as 'aa'";
                    sql += " ,(select name from user where id = t2.userout) as 'bb'";
                    sql += " , t2.posid as 'dd'";


                    if (Configs.Reports.ReportPriceSplitLosscard)
                    {
                        sql += " , ROUND(sum((t2.price-t2.losscard)) - ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ee";
                        sql += " , ROUND(ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ff";
                        sql += " , ROUND(sum((t2.price-t2.losscard)), 2) as gg";
                    }
                    else
                    {
                        sql += " , ROUND(sum(t2.price) - ROUND(sum(t2.price)*7/107, 6), 2) as 'ee'";
                        sql += " , ROUND(ROUND(sum(t2.price)*7/107, 6), 2) as 'ff'";
                        sql += " , ROUND(sum(t2.price), 2) as 'gg'";
                    }
                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/10
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    if (guardhouse != String.Empty)
                        sql += " and t2.guardhouse = '" + guardhouse + "' ";

                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and t1.cartype != 200";

                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    if (Configs.UseReceiptFor1Out)
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d') ,t2.userout";
                    }
                    else
                    {
                        sql += " group by date_format(t2.dateout,'%Y-%m-%d'),t2.userout";
                    }

                    sql += " union";
                    sql += " select datepay as 'aa'";
                    sql += ", (select name from user where id = user) as 'bb'";
                    sql += " , posid as 'dd'";
                    sql += " , ROUND(sum(price) - ROUND(sum(price)*7/107, 6), 2) as 'ee'";
                    sql += " , ROUND(ROUND(sum(price)*7/107, 6), 2) as 'ff'";
                    sql += " , ROUND(sum(price), 2) as 'gg'";
                    sql += " from member_record ";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and printno > 0";
                    if (Configs.UseVoidSlip)
                        sql += " and status != 'V'";
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (user != Constants.TextBased.All)
                        sql += " and user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " group by date_format(datepay,'%Y-%m-%d'),user";

                    sql += ") tt order by date_format(aa,'%Y-%m-%d'), dd,bb";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                            + startDate.ToLongDateString()
                            + " เวลา " + startTime.ToLongTimeString()
                            + " ถึงวันที่ " + endDate.ToLongDateString()
                            + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 134:
                    sql = "select date_format(aa,'%d/%m/%Y') as 'วันที่', bb as 'เลขที่ใบกำกับภาษี', dd as 'รหัสเครื่องคิดเงิน'";
                    sql += ", ee as 'ค่าบริการ', ff as 'VAT', gg as 'รวมเงิน',hh as 'เลขบัตร' ,ii as 'ทะเบียน' ,jj as 'วัน-เวลาเข้า' ,kk as 'วัน-เวลาออก' ,ll as 'เวลาจอด'";
                    sql += ", mm as 'รหัสส่วนลด' , nn as 'พนักงาน'";
                    sql += " from ";


                    sql += "(select t2.dateout as 'aa'";
                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y%m') ,lpad((t2.printno),6,'0'))) as 'bb'";
                        }
                        else
                        {
                            sql += " , concat(t2.receipt, concat(date_format(t2.dateout,'%y') ,lpad((t2.printno),6,'0'))) as 'bb'";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y%m') ,lpad((t2.printno),6,'0'))) as 'bb'";
                        }
                        else
                        {
                            sql += " , concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(t2.dateout,'%y') ,lpad((t2.printno),6,'0'))) as 'bb'";
                        }
                    }
                    sql += " , t2.posid as 'dd'";


                    if (Configs.Reports.ReportPriceSplitLosscard)
                    {
                        sql += " , ROUND(((t2.price-t2.losscard)) - ROUND(((t2.price-t2.losscard))*7/107, 6), 2) as ee";
                        sql += " , ROUND(ROUND(sum((t2.price-t2.losscard))*7/107, 6), 2) as ff";
                        sql += " , ROUND(((t2.price-t2.losscard)), 2) as gg";
                    }
                    else
                    {
                        sql += " , ROUND((t2.price) - ROUND((t2.price)*7/107, 6), 2) as 'ee'";
                        sql += " , ROUND(ROUND((t2.price)*7/107, 6), 2) as 'ff'";
                        sql += " , ROUND((t2.price), 2) as 'gg'";
                    }


                    sql += ",cast(ifnull((select name_on_card from cardpx where name = t1.id),";
                    sql += "(select name_on_card from cardmf where name = t1.id)) as char) as 'hh'";
                    sql += ",t1.license as 'ii'";
                    sql += ",DATE_FORMAT(t1.datein,'%d/%m/%Y %H:%i:%s') as 'jj'";
                    sql += ",DATE_FORMAT(t2.dateout,'%d/%m/%Y %H:%i:%s') as 'kk'";
                    sql += ",ROUND(concat(floor(((UNIX_TIMESTAMP(t2.dateout) - UNIX_TIMESTAMP(t1.datein))/60/60)),'.',   lpad((mod(floor((UNIX_TIMESTAMP(t2.dateout) - UNIX_TIMESTAMP(t1.datein))/60),60)),2,'0')),2) as 'll'";
                    sql += ",lpad(t2.proid,6,0) as 'mm'";
                    sql += ",(select concat(lpad(user.cardname,10,0),' ',user.name) from user where user.id = t2.userout) as 'nn'";

                    sql += " from recordin t1 left join recordout t2 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        //sql += " left join member t3 on t1.license = t3.license";
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/10
                    sql += " where t2.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and t2.no is not null";
                    sql += " and t2.printno > 0";

                    if (Configs.UseVoidSlip)
                        sql += " and t2.status = 'N'";

                    if (guardhouse != String.Empty)
                        sql += " and t2.guardhouse = '" + guardhouse + "' ";

                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and t1.typeid =" + carTypeId;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and t1.cartype != 200";

                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " union";
                    sql += " select datepay as 'aa'";
                    sql += " , concat(receipt, concat(date_format(datepay, '%y'), lpad((printno), 6, '0'))) as 'bb'";
                    sql += " , posid as 'dd'";
                    sql += " , ROUND((price) - ROUND((price)*7/107, 6), 2) as 'ee'";
                    sql += " , ROUND(ROUND((price)*7/107, 6), 2) as 'ff'";
                    sql += " , ROUND((price), 2) as 'gg'";
                    sql += ", cast(ifnull((select name_on_card from cardpx where name = member_record.cardid),";
                    sql += "(select name_on_card from cardmf where name = member_record.cardid)) as char) as 'hh'";
                    sql += ",license as 'ii'";
                    sql += ",DATE_FORMAT(datepay,'%d/%m/%Y %H:%i:%s') as 'jj'";
                    sql += ",DATE_FORMAT(datepay,'%d/%m/%Y %H:%i:%s') as 'kk'";
                    sql += ",'None' as 'll'";
                    sql += ",'None'as 'mm'";
                    sql += ",(select concat(lpad(user.cardname,10,0),' ',user.name) from user where user.id = member_record.user) as 'nn'";

                    sql += " from member_record ";
                    sql += " where datepay between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and printno > 0";
                    if (Configs.UseVoidSlip)
                        sql += " and status != 'V'";
                    if (guardhouse != String.Empty)
                        sql += " and guardhouse = '" + guardhouse + "'";
                    if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " and cartype =" + carType;
                    else if (carType == Constants.TextBased.Visitor)
                        sql += " and cartype != 200";
                    if (user != Constants.TextBased.All)
                        sql += " and user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += ") tt order by aa, dd";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;
                case 135:
                    sql = "select ";
                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " concat(recordout.receipt, concat(date_format(recordout.dateout,'%y%m'), lpad(recordout.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี',";
                        }
                        else
                        {
                            sql += " concat(recordout.receipt, concat(date_format(recordout.dateout,'%y'), lpad(recordout.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี',";
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2024/11/25
                        {
                            sql += " concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(recordout.dateout,'%y%m'), lpad(recordout.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี',";
                        }
                        else
                        {
                            sql += " concat((select value from slipoutformat where name = 'receiptname'), concat(date_format(recordout.dateout,'%y'), lpad(recordout.printno,6,'0'))) as 'เลขที่ใบกำกับภาษี',";
                        }
                    }

                    sql += "cast(recordin.id as char) as 'หมายเลขบัตร', recordin.license as 'ทะเบียน',recordout.posid as 'รหัสเครื่องคิดเงิน',";
                    sql += "DATE_FORMAT(recordin.datein,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า',DATE_FORMAT(recordout.dateout,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก',";
                    sql += "ROUND(concat(floor(((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60/60)),'.',   lpad((mod(floor((UNIX_TIMESTAMP(dateout) - UNIX_TIMESTAMP(datein))/60),60)),2,'0')),2) as 'เวลาจอด',";
                    sql += "lpad(recordout.proid,6,0) as 'รหัสส่วนลด',ROUND(recordout.price-(recordout.price*7)/107,2) as 'ค่าบริการ',";
                    sql += "ROUND((recordout.price*7)/107,2) as 'VAT',ROUND(recordout.price,2) as 'รวมเงิน',";
                    sql += " (select concat(lpad(user.cardname,10,0),' ',user.name) from user where user.id = recordout.userout) as 'พนักงาน'";
                    sql += " from recordout left join recordin on recordout.no = recordin.no left join user on recordout.userout = user.id ";
                    sql += " left join member on recordin.id = member.cardid"; //Mac 2021/08/10
                    sql += " where recordout.dateout between '" + startDateTimeText + "' AND '" + endDateTimeText + "' ";

                    if (user != Constants.TextBased.All)
                    {
                        sql += " and user.name LIKE '%" + user + "%'";
                    }
                    if (!String.IsNullOrWhiteSpace(licensePlate))
                    {
                        sql += " and recordin.license LIKE '%" + licensePlate + "%'";
                    }
                    if (!String.IsNullOrWhiteSpace(cardId))
                    {
                        sql += " and recordin.id LIKE '%" + cardId + "%'";
                    }
                    if (guardhouse != String.Empty)
                    {
                        sql += " and recordout.guardhouse LIKE '%" + guardhouse + "%'";
                    }

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and member.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " and recordout.printno > 0";
                    sql += " order by recordout.printno,recordin.datein";
                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;
                case 136:
                    sql = "select cast(ifnull((select name_on_card from cardpx where name = member.cardid),";
                    sql += "  (select name_on_card from cardmf where name = member.cardid)) as char) as 'หมายเลขบัตร' ";
                    sql += ",(select typename from cartype where typeid = member.cartype) as 'ประเภทบัตร'";
                    sql += ",member_record.license as 'ทะเบียนรถ',member_record.name as 'ผู้ถือบัตร'";
                    sql += ",member_record.datepay as 'วันที่ทำรายการ',member_record.dateexpire as 'วันที่บัตรหมดอายุ'";
                    sql += ",case when member_record.status = 'A' then 'เพิ่มบัตร'";
                    sql += " when member_record.status = 'C' then 'ยกเลิกบัตร' when member_record.status = 'P' then 'ต่ออายุบัตร' ";
                    sql += " when member_record.status = 'E' then 'แก้ไขบัตร' when member_record.status = 'B' then 'ระงับบัตร'  when member_record.status = 'CB' then 'ยกเลิกระงับบัตร' else 'Unknow' end as 'ประเภทรายการ'";
                    sql += ",member_record.price as 'จำนวนเงิน',";
                    sql += "(select name from user where user.id = member_record.user) as 'ผุ้ทำรายการ'";
                    sql += ",case when member.enable = 'True' then 'ใช้งาน' else 'ไม่ได้ใช้งาน' end as 'สถานะ'";
                    sql += ",(select groupname from membergroup where membergroup.id=member.memgroupid) as 'กลุ่มสมาชิก'";
                    sql += " from member_record left join member on member_record.cardid = member.cardid ";
                    sql += " where member_record.datepay BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "'";

                    if (cardId.Length > 0)
                        sql += " and member_record.cardid = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    if (licensePlate.Length > 0)
                        sql += " and member_record.license LIKE '%" + licensePlate + "%'";
                    if (user != Constants.TextBased.All)
                        sql += " and member_record.user = (select id from user where name LIKE '%" + user + "%')";
                    if (carType != Constants.TextBased.All)
                        sql += "  and member.cartype = (select typeid from cartype where typename LIKE '%" + carType + "%')";

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and member.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " order by member_record.id DESC ";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();

                    break;

                case 137:
                    sql = "select cast(ifnull((select name_on_card from cardpx where name = member.cardid), ";
                    sql += "(select name_on_card from cardmf where name = member.cardid)) as char) as 'หมายเลขบัตร',";
                    sql += "member.license as 'ทะเบียน',(select cardtype.name from cardtype where cardtype.id =member.cardtypeid) as 'ประเภทบัตร',";
                    sql += "lpad(member.memkey,10,0) as 'รหัสประจำตัว',member.name as 'ผู้ถือบัตร',";
                    sql += "(select m_store.store_name from m_store where m_store.store_id = member.storeid) as 'บริษัท',";
                    sql += "DATE_FORMAT(member_record.datepay,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาระงับ',";
                    sql += "DATE_FORMAT(member.dateexprie,'%d/%m/%Y %H:%i:%s') as 'วันที่บัตรหมดอายุ',";
                    sql += "(select lpad(user.cardname,10,0) from user where user.id = member_record.user) as 'รหัสผู้ระงับ',";
                    sql += "case when member_record.status = 'C' then 'ยกเลิกบัตร' else 'ปกติ' end as 'หมายเหตุ'";
                    sql += " from member_record left join member on member_record.cardid = member.cardid ";
                    sql += " where member_record.datepay BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "' and member_record.status ='C'";


                    if (user != Constants.TextBased.All)
                        sql += " and member_record.user =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and member_record.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " and member_record.cardid = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    if (carType != Constants.TextBased.All)
                        sql += "  and member.typeid = (select typeid from cartype where typename LIKE '%" + carType + "%')";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and member_record.storeid = (select store_id from m_store where store_name = '" + memberGroupMonth + "')";
                    if (paymentStatus != Constants.TextBased.All)
                        sql += " and member_record.cardtypeid = " + AppGlobalVariables.MemberGroupsToId[paymentStatus];
                    if (memberRenewalType != Constants.TextBased.All) //Mac 2020/08/10
                        sql += " and member_record.renew_memid = " + AppGlobalVariables.RenewMemberGroupsToId[memberRenewalType];


                    sql += " order by member_record.datepay";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 138:
                    sql = "select cast(ifnull((select name_on_card from cardpx where name = t1.id),(select name_on_card from cardmf where name = t1.id)) as char) as 'เลขบัตร',(select name from member where member.cardid = t1.id) as 'ชื่อ-สกุลผู้ถือบัตร'";
                    sql += ",t1.license as 'ทะเบียนรถ',t1.cartype as 'ประเภท'";
                    sql += ",date_format(t1.datein,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า',t1.guardhouse as 'ประตูเข้า'";
                    sql += ",(select username from user where user.id = t1.userin) as 'พนักงานเข้า'";
                    sql += ",date_format(t2.dateout,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก',t2.guardhouse as 'ประตูออก '";
                    sql += ",(select username from user where user.id = t2.userout) as 'พนักงานออก'";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(t2.dateout) - UNIX_TIMESTAMP(t1.datein))/60/60)),'.',   lpad((mod(floor((UNIX_TIMESTAMP(t2.dateout) - UNIX_TIMESTAMP(t1.datein))/60),60)),2,'0'))  as 'เวลาจอด'";
                    sql += ",lpad(t2.proid,5,0) as 'ตราประทับ'";
                    if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2019/08/27
                    {
                        sql += ",format(((t2.price-t2.losscard)) - ROUND(((t2.price-t2.losscard))*7/107, 6), 2) as 'ค่าจอดรถ'";
                        sql += ",format(((t2.price-t2.losscard)), 2) as 'เงินเรียกเก็บ'";
                        sql += ",format(ROUND((t2.losscard), 6), 2) as 'ค่าปรับบัตรหาย'";
                        sql += ",format(ROUND((t2.price), 6), 2) as 'รวมเงินสด'";
                    }
                    else
                    {
                        sql += ",format((t2.price) - ROUND((t2.price)*7/107, 6), 2) as 'ค่าจอดรถ'";
                        sql += ",format((t2.price), 2) as 'เงินเรียกเก็บ'";
                        sql += ",format(ROUND((t2.losscard), 6), 2) as 'ค่าปรับบัตรหาย'";
                        sql += ",format(ROUND((t2.price + t2.losscard), 6), 2) as 'รวมเงินสด'";
                    }

                    sql += " from recordout t2 left join recordin t1 on t1.no = t2.no";
                    if (Configs.UseMemberLicensePlate) //Mac 2024/06/27
                        sql += " left join member t3 on t3.license like concat('%',t1.license,'%')"; //Mac 2025/03/14
                    else
                        sql += " left join member t3 on t1.id = t3.cardid"; //Mac 2021/08/10
                    sql += " where t1.datein BETWEEN '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (carType != Constants.TextBased.All)
                    {
                        sql += " and t1.cartype = (select cartype.typeid from cartype where typename = '" + carType + "')";
                    }
                    if (user != Constants.TextBased.All)
                        sql += " and t2.userout = (select user.id from user where user.name = '%" + user + "%')";
                    if (!String.IsNullOrWhiteSpace(cardId))
                        sql += " and t1.id = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    if (!String.IsNullOrWhiteSpace(licensePlate))
                        sql += " and t1.license LIKE '%" + licensePlate + "%'";

                    if (memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/10
                        sql += " and t3.storeid = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " order by t2.dateout DESC";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 139:
                    sql = "select concat(lpad(store_id,4,0),' / ',customer_code) as 'รหัส/CV'";
                    sql += ",concat(lpad(store_id,4,0),' : ',store_name) as 'ชื่อบริษัท/ร้านค้า'";
                    sql += ",store_address as 'ที่อยู่'";
                    sql += ",store_estamp_quota as 'โควต้า' ";
                    sql += ",DATE_FORMAT(store_start_date,'%d/%m/%Y %H:%i:%s') as 'วันที่เริ่มสัญญา'";
                    sql += ",DATE_FORMAT(store_end_date,'%d/%m/%Y %H:%i:%s') as 'วันที่ยกเลิกสัญญา'";
                    sql += " from m_store where store_id is not null";

                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_name LIKE '%" + memberGroupMonth + "%'";
                    sql += " order by store_id ASC";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 140:
                    sql = "select aa as store_id,cc as 'บริษัท/ร้านค้า', ee as 'วันที่', ff as 'รหัสคูปอง' ";
                    sql += ", gg as 'จำนวนชั่วโมงฟรี', ii as 'จำนวนชั่วโมงที่ใช้', kk as 'จำนวนคูปอง' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }
                    sql += ",count(*) as kk";

                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')), date_format(parking_out_time, '%d/%m/%Y')";


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",count(*) as kk";

                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')), date_format(t1.dateout, '%d/%m/%Y')";

                    sql += ") tt order by aa, ee, ff";



                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();

                    break;

                case 141:
                    if (Configs.Reports.UseReportHourUse) //Mac 2023/03/17
                    {
                        sql = "select aa as 'store_id', cc as 'บริษัท/ร้านค้า', ee as 'วันที่', ff as 'รหัสคูปอง',";
                        sql += "gg as 'ตราประทับ (ชม.) ฟรี', ii as 'เวลาที่คิดค่าใช้จ่ายหน่วยงาน/ผู้เช่า (ชม.)', kk as 'ทะเบียน', ll as 'เลขslot', mm as 'รหัส QR',";
                        sql += "nn as 'วันเวลาเข้า', oo as 'วันเวลาออก', pp as 'เวลาจอด',";
                        sql += "qq as 'จำนวนเงินที่คิดกับผู้มาติดต่อ', rr as 'เวลาที่คิดค่าใช้จ่ายผู้มาติดต่อ (ชม.)'  from ";
                    }
                    else
                    {
                        sql = "select aa as 'store_id',cc as 'บริษัท/ร้านค้า', ee as 'วันที่', ff as 'รหัสคูปอง' ,";
                        sql += "gg as 'จำนวนชั่วโมงฟรี', ii as 'จำนวนชั่วโมงที่ใช้', kk as 'ทะเบียน',ll as 'เลขslot',mm as 'รหัส QR',";
                        sql += " nn as 'วันเวลาเข้า',oo as 'วันเวลาออก',pp as 'เวลาจอด',";
                        sql += "qq as 'ค่าบริการส่วนเพิ่ม'  from ";
                    }

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";
                    }
                    else if (Configs.Reports.UseReportHourUse) //Mac 2023/03/17
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (parking_out_hour_use) as 'ii'";
                    }
                    else
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",(select license from recordin where no = view_estamp.parking_out_id) as 'kk'";
                    sql += ",lot_uuid as ll,wei_estamp_code as mm";
                    sql += ",parking_in_time as nn,parking_out_time as oo";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += "lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as pp";
                    sql += ",parking_out_price as qq";
                    if (Configs.Reports.UseReportHourUse)
                        sql += ", (ceiling(timestampdiff(minute, date_format(parking_in_time, '%Y-%m-%d %H:%i:%s'), date_format(parking_out_time, '%Y-%m-%d %H:%i:%s'))/60) - parking_out_hour_use) as rr";

                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];


                    if (!Configs.Reports.UseReportHourUse)
                    {
                        sql += " union ";

                        sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                        sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                        sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                        sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                        sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                        sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                        if (Configs.Reports.UseReport108_110_1)
                        {
                            sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                            sql += " , 0 as 'hh'";
                            sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                        }
                        else
                        {
                            sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                            sql += " , (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                            sql += " , case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                        }

                        sql += ",t2.license as kk";
                        sql += ",'' as ll,'' as mm";
                        sql += ",t2.datein as nn,t1.dateout as oo";
                        sql += ",concat(floor(((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                        sql += " lpad((mod(floor((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as pp";
                        sql += ",t1.price as qq";

                        sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                        sql += " where (t1.proid between 1 and 255) and ";
                        sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                        if (memberGroupMonth != Constants.TextBased.All)
                            sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    }

                    sql += ") tt order by aa, ee,oo, ff";


                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 142:
                    //sql = "select (select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as 'รหัส-ชื่อเจ้าหน้าที่'";
                    //sql += ",concat(lpad(store_id,4,'0'),' : ',store_name) as 'บริษัท/ร้านค้า',date_format(parking_out_time,'%d/%m/%Y') as 'วันที่'";
                    //sql += ",concat(lpad(store_id,4,'0'),lpad(estamp_id,2,'0')) as 'รหัสคูปอง',count(*) as 'จำนวนคูปอง',sum(estamp_hour) as 'จำนวนชั่วโมงฟรี'";
                    //sql += ",sum(case when  estamp_hour != 9999 then estamp_hour else parking_out_hour_use end) as 'จำนวนชั่วโมงที่ใช้'";
                    //sql += ",sum(parking_out_price) as 'จำนวนเงินที่เรียกเก็บ',store_id";
                    //sql += " from view_estamp left join recordin on view_estamp.parking_in_id = recordin.no";
                    //sql += " where parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where store_name = '" + memberGroupMonth + "' limit 1)";
                    //if (user != Constants.TextBased.All)
                    //    sql += " and parking_use_by = (select id from user where name = '" + user + "')";
                    //if (carType != Constants.TextBased.All)
                    //    sql += " and recordin.cartype = (select typeid from cartype where typename = '" + carType + "')";
                    //sql += " group by  DATE(parking_out_time),store_id,store_name,estamp_id ";
                    //sql += " order by store_id,estamp_id ASC";

                    sql = "select ll as 'รหัส-ชื่อเจ้าหน้าที่',aa as store_id,cc as 'บริษัท/ร้านค้า', ee as 'วันที่', ff as 'รหัสคูปอง' ";
                    sql += ", gg as 'จำนวนชั่วโมงฟรี', ii as 'จำนวนชั่วโมงที่ใช้', kk as 'จำนวนคูปอง', mm as 'จำนวนเงินที่เรียกเก็บ' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",count(*) as kk";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as ll";
                    sql += ",sum(parking_out_price) as mm";

                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')), date_format(parking_out_time, '%d/%m/%Y')";


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }


                    sql += ",count(*) as kk";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as ll";
                    sql += ",sum(t1.price) as mm";

                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')), date_format(t1.dateout, '%d/%m/%Y')";

                    sql += ") tt order by aa, ee, ff";


                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;


                case 143:
                    //sql = "select (select concat(lpad(cardname,10,0),' : ',name) from user where user.id = parking_use_by) as 'เจ้าหน้าที่'";
                    //sql += ",concat(lpad(store_id,4,'0'),' : ',store_name) as 'บริษัท/ร้านค้า'";
                    //sql += ",cast(ifnull((select name_on_card from cardpx where name = (select id from recordin where recordin.no = parking_in_id)), (select name_on_card from cardmf where name = (select id from recordin where recordin.no = parking_in_id))) as char) as 'หมายเลขบัตร'";
                    //sql += ",(select license from recordin where recordin.no = parking_in_id) as 'ทะเบียนรถ'";
                    //sql += ",parking_in_time as 'วัน-เวลาเข้า',parking_out_time as 'วัน-เวลาออก'";
                    //sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    //sql += "   lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as 'เวลาจอด'";

                    //sql += ",wei_estamp_code as 'เลขQR'";
                    //sql += ",lot_uuid as 'เลขslot'";
                    //sql += ",concat(lpad(store_id,4,'0'),lpad(estamp_id,2,'0')) as 'รหัสคูปอง'";
                    //sql += ",estamp_hour as 'จำนวนชั่วโมงฟรี'";
                    //sql += ",case when estamp_hour != 9999 then estamp_hour else parking_out_hour_use end as 'จำนวนชั่วโมงที่ใช้'";
                    //sql += ",parking_out_price as 'ค่าบริการส่วนเพิ่ม'";
                    //sql += ",store_id";
                    //sql += " from view_estamp left join recordin on view_estamp.parking_in_id = recordin.no WHERE parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where store_name = '" + memberGroupMonth + "' limit 1)";
                    //if (user != Constants.TextBased.All)
                    //    sql += " and parking_use_by = (select id from user where name = '" + user + "')";
                    //if (carType != Constants.TextBased.All)
                    //    sql += " and recordin.cartype = (select typeid from cartype where typename = '" + carType + "')";
                    //if (!String.IsNullOrWhiteSpace(licensePlate))
                    //    sql += " and recordin.license LIKE '%"+licensePlate+"%'";
                    //if (!String.IsNullOrWhiteSpace(cardId))
                    //    sql += " and recordin.id LIKE '%"+cardId+"%'";

                    //sql += " order by store_id,estamp_id ASC";
                    sql = "select aa as 'store_id',rr as 'เจ้าหน้าที่',cc as 'บริษัท/ร้านค้า',ss as 'หมายเลขบัตร', ee as 'วันที่', ff as 'รหัสคูปอง' ,";
                    sql += "gg as 'จำนวนชั่วโมงฟรี', ii as 'จำนวนชั่วโมงที่ใช้', kk as 'ทะเบียนรถ',ll as 'เลขslot',mm as 'เลขQR',";
                    sql += "nn as 'วัน-เวลาเข้า',oo as 'วัน-เวลาออก',pp as 'เวลาจอด',";
                    sql += "qq as 'ค่าบริการส่วนเพิ่ม'  from ";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";
                    }
                    else
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",(select license from recordin where no = view_estamp.parking_out_id) as 'kk'";
                    sql += ",lot_uuid as ll,wei_estamp_code as mm";
                    sql += ",parking_in_time as nn,parking_out_time as oo";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += "lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as pp";
                    sql += ",parking_out_price as qq";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as rr";
                    sql += ",cast(ifnull((select name_on_card from cardpx where name = (select id from recordin where recordin.no = parking_in_id)), (select name_on_card from cardmf where name = (select id from recordin where recordin.no = parking_in_id))) as char) as 'ss'";


                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (user != Constants.TextBased.All)
                        sql += " and parking_use_by = (select id from user where name = '" + user + "')";



                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                    }
                    else
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",t2.license as kk";
                    sql += ",'' as ll,'' as mm";
                    sql += ",t2.datein as nn,t1.dateout as oo";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                    sql += " lpad((mod(floor((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as pp";
                    sql += ",t1.price as qq";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as rr";
                    sql += ",cast(ifnull((select name_on_card from cardpx where name = (select id from recordin where recordin.no = t2.id)), (select name_on_card from cardmf where name = (select id from recordin where recordin.no = t2.id))) as char) as 'ss'";


                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (user != Constants.TextBased.All)
                        sql += " and t1.userout = (select id from user where name = '" + user + "')";

                    sql += ") tt order by aa, ee,oo, ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 144:
                    //sql = "select (select guardhouse from recordout where recordout.no =  view_estamp.parking_out_id) as 'สถานิปฏิบัติงาน'";
                    //sql += ",date_format(parking_out_time,'%d/%m/%Y') as 'วันที่' ";
                    //sql += ",(select concat(lpad(cardname,10,0),' - ',name) from user where user.id = parking_use_by) as 'เจ้าหน้าที่'";
                    //sql += ",concat(lpad(store_id,4,'0'),lpad(estamp_id,2,'0')) as 'รหัสคูปอง'";
                    //sql += ",count(*) as 'จำนวนคูปอง'";
                    //sql += " from view_estamp left join recordout on view_estamp.parking_out_id = recordout.no ";
                    //sql += " WHERE parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where store_name = '" + memberGroupMonth + "' limit 1)";
                    //if (user != Constants.TextBased.All)
                    //    sql += " and parking_use_by = (select id from user where name = '" + user + "')";
                    //if (guardhouse != String.Empty)
                    //    sql += " and recordout.guardhouse LIKE '%" + guardhouse + "%'";
                    //sql += " group by (select guardhouse from recordout where recordout.no =  view_estamp.parking_out_id)";
                    //sql += " ,date_format(parking_out_time,'%d/%m/%Y')";
                    //sql += " ,(select name from user where user.id = parking_use_by)";
                    //sql += " ,concat(lpad(store_id,4,'0'),lpad(estamp_id,2,'0'))";
                    //sql += " order by store_id,estamp_id,parking_out_time ASC";
                    sql = "select aa as 'store_id',tt as 'สถานิปฏิบัติงาน',rr as 'เจ้าหน้าที่', ee as 'วันที่', ff as 'รหัสคูปอง' ,";
                    sql += "ss as 'จำนวนคูปอง'";
                    sql += "  from ";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";


                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as rr";
                    sql += ",(select guardhouse from recordout where recordout.no =  view_estamp.parking_out_id) as tt";
                    sql += ",count(*) as ss";

                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (user != Constants.TextBased.All)
                        sql += " and parking_use_by = (select id from user where name = '" + user + "')";

                    sql += " group by(select guardhouse from recordout where recordout.no =  view_estamp.parking_out_id),(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by)";
                    sql += ",date_format(parking_out_time, '%d/%m/%Y') ,concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) ";


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";

                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as rr";
                    sql += ",t1.guardhouse as tt";
                    sql += ",count(*) as ss";

                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (user != Constants.TextBased.All)
                        sql += " and t1.userout = (select id from user where name = '" + user + "')";

                    sql += " group by t1.guardhouse,(select name from user where user.id = t1.userout), date_format(t1.dateout, '%d/%m/%Y'),lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0')";
                    sql += ") zz order by tt, ee, ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 145:
                    //sql = "select concat(lpad(store_id,4,'0'),' / ',store_code,' : ',store_name) as 'บริษัท/ร้านค้า'";
                    //sql += ",(select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as 'โค้วต้าจอดฟรี'";
                    //sql += ",sum(case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) as 'เวลาจอดฟรี'";
                    //sql += ",store_id";
                    //sql += " from view_estamp ";
                    //sql += " WHERE parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where store_name = '" + memberGroupMonth + "' limit 1)";
                    //sql += " group by  store_id";
                    //sql += " order by store_id,estamp_id ASC";

                    sql = "select aa as store_id,cc as 'บริษัท/ร้านค้า',CAST(ifnull(dd,0) as UNSIGNED ) as 'โค้วต้าจอดฟรี', ff as 'รหัสคูปอง' ";
                    sql += ", CAST(ifnull(gg,0) as UNSIGNED ) as 'เวลาจอดฟรี' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }
                    sql += ",count(*) as kk";

                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0'))";


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",count(*) as kk";

                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0'))";

                    sql += ") tt order by aa, ee, ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 146:
                    //sql = "select (select concat(lpad(store_id,4,'0'),' / ',customer_code,' : ',store_name) from m_store where m_store.store_id = view_estamp.store_id) as 'บริษัท/ร้านค้า',";
                    //sql += " cast(cast(ifnull((select name_on_card from cardpx where name = recordin.id),(select name_on_card from cardmf where name = recordin.id)) as char) as char) as 'หมายเลขบัตร',";
                    //sql += "(select CASE when recordin.license is null then 'NO' else recordin.license end from recordin where recordin.no = parking_in_id) as 'ทะเบียน',concat(lpad(store_id,4,'0'),";
                    //sql += "lpad(estamp_id,2,'0')) as 'รหัสส่วนลด',date_format(parking_out_time,'%d/%m/%Y') as 'วันที่',";
                    //sql += "date_format(parking_in_time,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า',date_format(parking_out_time,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก',";
                    //sql += "case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end as 'เวลาฟรีลด',store_id,(select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as 'โค้วต้าจอดฟรี'";
                    //sql += " from view_estamp left join recordin on view_estamp.parking_in_id = recordin.no WHERE parking_use_flag ='Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (carType != Constants.TextBased.All)
                    //{
                    //    sql += " and recordin.cartype = (select cartype.typeid from cartype where typename = '" + carType + "')";
                    //}
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where m_store.store_name = '" + memberGroupMonth + "')";
                    ////if (user != Constants.TextBased.All)
                    ////    sql += " and parking_out_id = (select user.id from user where user.name = '%" + user + "%')";
                    //if (!String.IsNullOrWhiteSpace(cardId))
                    //    sql += " and recordin.id = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    //if (!String.IsNullOrWhiteSpace(licensePlate))
                    //    sql += " and recordin.license LIKE '%" + licensePlate + "%'";
                    //sql += " order by store_id,estamp_id,parking_out_time ASC";

                    sql = "select aa as 'store_id',cc as 'บริษัท/ร้านค้า',ss as 'หมายเลขบัตร', ee as 'วันที่', ff as 'รหัสส่วนลด' ,";
                    sql += "gg as 'โค้วต้าจอดฟรี', ii as 'เวลาฟรีลด', kk as 'ทะเบียน',";
                    sql += "nn as 'วัน-เวลาเข้า',oo as 'วัน-เวลาออก',pp as 'เวลาจอด'";
                    sql += "  from ";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";//
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";//
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";//
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";//
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";//
                        sql += " , 0 as 'hh'";
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";//
                    }
                    else
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";//
                        sql += " , (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",(select license from recordin where no = view_estamp.parking_out_id) as 'kk'";//
                    sql += ",lot_uuid as ll,wei_estamp_code as mm";
                    sql += ",parking_in_time as nn,parking_out_time as oo";//
                    sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += "lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as pp";
                    sql += ",parking_out_price as qq";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as rr";
                    sql += ",cast(ifnull((select name_on_card from cardpx where name = (select id from recordin where recordin.no = parking_in_id)), (select name_on_card from cardmf where name = (select id from recordin where recordin.no = parking_in_id))) as char) as 'ss'"; //


                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (user != Constants.TextBased.All)
                        sql += " and parking_use_by = (select id from user where name = '" + user + "')";



                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'"; //
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'"; //
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";//
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";//
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";//
                        sql += " , 0 as 'hh'";
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                    }
                    else
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";//
                        sql += " , (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",t2.license as kk";//
                    sql += ",'' as ll,'' as mm";
                    sql += ",t2.datein as nn,t1.dateout as oo";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                    sql += " lpad((mod(floor((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as pp";
                    sql += ",t1.price as qq";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as rr";
                    sql += ",cast(ifnull((select name_on_card from cardpx where name = (select id from recordin where recordin.no = t2.id)), (select name_on_card from cardmf where name = (select id from recordin where recordin.no = t2.id))) as char) as 'ss'"; //


                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    if (user != Constants.TextBased.All)
                        sql += " and t1.userout = (select id from user where name = '" + user + "')";

                    sql += ") tt order by aa, ee, oo,ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 147:
                    //sql = "select store_id,concat(lpad(store_id,4,'0'),' / ',store_code,' : ',store_name) as 'บริษัท/ร้านค้า'";
                    //sql += ",concat(floor(sum((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    //sql += "   lpad((mod(floor(sum(UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as 'เวลาจอดจริง'";
                    //sql += ",(select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as 'โควต้าจอดฟรี'";
                    //sql += ",sum(case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) as 'เวลาฟรีของส่วนลด'";
                    //sql += ",sum(parking_out_price) as 'ค่าบริการส่วนเพิ่ม'";
                    //sql += " from view_estamp";
                    //sql += " where parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where store_name = '" + memberGroupMonth + "')";
                    //sql += " group by store_id";
                    //sql += " order by store_id";
                    sql = "select ll as 'รหัส-ชื่อเจ้าหน้าที่',aa as store_id,cc as 'บริษัท/ร้านค้า', ee as 'วันที่', ff as 'รหัสคูปอง' ";
                    sql += ",ifnull(nn,0) as 'เวลาจอดจริง'";
                    sql += ", CAST(ifnull(dd,0) as UNSIGNED) as 'โควต้าจอดฟรี', CAST(ifnull(ii,0) as UNSIGNED) as 'เวลาฟรีของส่วนลด', ifnull(mm,0) as 'ค่าบริการส่วนเพิ่ม' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as ll";
                    sql += ",sum(parking_out_price) as mm";
                    sql += ",concat(floor(sum((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += "   lpad((mod(floor(sum(UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as nn";

                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by lpad(store_id, 4, '0')";


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                    }
                    else
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }


                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as ll";
                    sql += ",sum(t1.price) as mm";
                    sql += ",concat(floor(sum((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                    sql += "   lpad((mod(floor(sum(UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as nn";

                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0')";

                    sql += ") tt order by aa, ee, ff";



                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 148:
                    //sql = "select store_id,concat(lpad(store_id,4,'0'),' / ',store_code,' : ',store_name) as 'บริษัท/ร้านค้า'";
                    //sql += ",cast(cast(ifnull((select name_on_card from cardpx where name = recordin.id),(select name_on_card from cardmf where name = recordin.id)) as char) as char) as 'หมายเลขบัตร'";
                    //sql += ",cast((select license from recordin where recordin.no = parking_in_id) as char) as 'ทะเบียน'";
                    //sql += ",date_format(parking_in_time,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า'";
                    //sql += ",date_format(parking_out_time,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก'";
                    //sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    //sql += "   lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as 'เวลาจอดจริง'";
                    //sql += ",case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end as 'เวลาจอดฟรี'";
                    //sql += ",parking_out_price as 'ค่าบริการส่วนเพิ่ม'";
                    //sql += ",date_format(parking_out_time,'%d/%m/%Y') as 'วันที่'";
                    //sql += ",concat(lpad(store_id,4,'0'),lpad(estamp_id,2,'0')) as 'รหัสส่วนลด'";
                    //sql += " from view_estamp left join recordin on view_estamp.parking_in_id = recordin.no";
                    //sql += " where parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (carType != Constants.TextBased.All)
                    //{
                    //    sql += " and recordin.cartype = (select cartype.typeid from cartype where typename = '" + carType + "')";
                    //}
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where m_store.store_name = '" + memberGroupMonth + "')";
                    ////if (user != Constants.TextBased.All)
                    ////    sql += " and parking_out_id = (select user.id from user where user.name = '%" + user + "%')";
                    //if (!String.IsNullOrWhiteSpace(cardId))
                    //    sql += " and recordin.id = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    //if (!String.IsNullOrWhiteSpace(licensePlate))
                    //    sql += " and recordin.license LIKE '%" + licensePlate + "%'";

                    //sql += " ORDER BY store_id,estamp_id,parking_out_time";

                    sql = "select ll as 'รหัส-ชื่อเจ้าหน้าที่',aa as store_id,cc as 'บริษัท/ร้านค้า', ee as 'วันที่', ff as 'รหัสส่วนลด',oo as 'หมายเลขบัตร' ";
                    sql += ",kk as 'ทะเบียน',pp as 'วัน-เวลาเข้า',qq as 'วัน-เวลาออก'";
                    sql += ",ifnull(nn,0) as 'เวลาจอดจริง'";
                    sql += ", CAST(ifnull(ii,0) as UNSIGNED) as 'เวลาจอดฟรี', ifnull(mm,0) as 'ค่าบริการส่วนเพิ่ม' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";
                    }
                    else
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",(select license from recordin where no = view_estamp.parking_out_id) as 'kk'";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as ll";
                    sql += ",(parking_out_price) as mm";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += "   lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as nn";

                    sql += ",cast(ifnull((select name_on_card from cardpx where name = (select id from recordin where recordin.no = parking_in_id)), (select name_on_card from cardmf where name = (select id from recordin where recordin.no = parking_in_id))) as char) as 'oo'"; //
                    sql += ",parking_in_time as pp,parking_out_time as qq";

                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by lpad(store_id, 4, '0')";


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";
                    }
                    else
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";
                    }

                    sql += ",t2.license as kk";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as ll";
                    sql += ",(t1.price) as mm";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                    sql += "   lpad((mod(floor((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as nn";

                    sql += ",cast(cast(ifnull((select name_on_card from cardpx where name = t2.id),(select name_on_card from cardmf where name = t2.id)) as char) as char) as oo";
                    sql += ",t2.datein as pp,t1.dateout as qq";

                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += ") tt order by aa, ee, ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;
                /**********************สรุปค่าเสียโอกาศที่จะได้รับเงิน คือ หนังจาก 4 ชม. จะบวกเพิ่มชั่วโมงละ 20 บาท ถ้าต่ำกว่า 4 ชม. จะเท่ากับ 20 บาท (จะต้องใส่ค่า parking_out_price คือเงินที่ลูกค้าจ่าย และ parking_out_hour_use คือ จำนวนชั่วโมงส่วนลดที่ใช้)***********************/
                case 149:
                    //sql = "select concat(lpad(store_id,4,'0'),' : ',store_name) as 'บริษัท/ร้านค้า',date_format(parking_out_time,'%d/%m/%Y') as 'วันที่'";
                    //sql += ",concat(lpad(store_id,4,'0'),lpad(estamp_id,2,'0')) as 'รหัสส่วนลด'";
                    //sql += ",sum(case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) as 'จำนวนชั่วโมงฟรี'";
                    //sql += ",count(*) as 'จำนวนส่วนลด',sum( case when (case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) < 5 then 20";
                    //sql += " else ((case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end)-3)*10   end) as 'จำนวนเงินที่ฟรี'";
                    //sql += ",sum( case when (case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) < 5 then ";
                    //sql += "1 else ((case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end)-3)   end) as 'จำนวนชั่วโมงที่ใช้'";
                    //sql += ",sum(parking_out_price) as 'ค่าบริการส่วนเพิ่ม',store_id ";
                    //sql += " from view_estamp left join recordin on view_estamp.parking_in_id = recordin.no";
                    //sql += " where parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    //if (carType != Constants.TextBased.All)
                    //{
                    //    sql += " and recordin.cartype = (select cartype.typeid from cartype where typename = '" + carType + "')";
                    //}
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where m_store.store_name = '" + memberGroupMonth + "')";

                    //sql += " group by store_id,date_format(parking_out_time,'%d/%m/%Y'),estamp_id";
                    //sql += " order by store_id,estamp_id,date_format(parking_out_time,'%Y-%m-%d') ASC";

                    sql = "select ll as 'รหัส-ชื่อเจ้าหน้าที่',aa as store_id,cc as 'บริษัท/ร้านค้า', ee as 'วันที่', ff as 'รหัสส่วนลด' ";
                    sql += ", CAST(ifnull(ii,0) as UNSIGNED) as 'จำนวนชั่วโมงฟรี'";
                    sql += ",xx as 'จำนวนชั่วโมงที่ใช้', yy as 'จำนวนเงินที่ฟรี' ";
                    sql += ",ifnull(mm,0) as 'ค่าบริการส่วนเพิ่ม' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";

                        sql += ",case when (sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end)) < 5 then 1 else ";
                        sql += "(sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end)) end as xx";

                        sql += ",case when (sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end)) < 5 then 20 else ";
                        sql += "(sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end)) * 10 end as yy";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += ",sum(";
                        sql += "case when ";
                        sql += "( case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end) < 5 then 1 else ";
                        sql += "( case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end)";
                        sql += " end ";
                        sql += ") as xx";

                        sql += ",sum(";
                        sql += "case when ";
                        sql += "( case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end) < 5 then 20 else ";
                        sql += "( case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end) * 10 ";
                        sql += " end ";
                        sql += ") as yy";
                    }

                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as ll";
                    sql += ",sum(parking_out_price) as mm";
                    sql += ",concat(floor(sum((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += "   lpad((mod(floor(sum(UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as nn";

                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by lpad(store_id, 4, '0'),date_format(parking_out_time, '%d/%m/%Y')";


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";

                        sql += ",sum(";
                        sql += " case when ";
                        sql += "(  (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)) < 5 then 1 else ";
                        sql += "( (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) )";
                        sql += " end ";
                        sql += ") as xx";

                        sql += ",sum(";
                        sql += " case when ";
                        sql += "(  (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)) < 5 then 20 else ";
                        sql += "( (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) ) * 10 ";
                        sql += " end ";
                        sql += ") as yy";


                    }
                    else
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += ",sum(";
                        sql += " case when ";
                        sql += "( case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ) < 5 then 1 else ";
                        sql += "( case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end )";
                        sql += " end ";
                        sql += ") as xx";

                        sql += ",sum(";
                        sql += " case when ";
                        sql += "( case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ) < 5 then 20 else ";
                        sql += "( case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ) * 10 ";
                        sql += " end ";
                        sql += ") as yy";
                    }


                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as ll";
                    sql += ",sum(t1.price) as mm";
                    sql += ",concat(floor(sum((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                    sql += "   lpad((mod(floor(sum(UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as nn";

                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'),date_format(t1.dateout, '%d/%m/%Y')";

                    sql += ") tt order by aa, ee, ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;
                /**********************ค่าเสียโอกาศที่จะได้รับเงิน คือ หนังจาก 4 ชม. จะบวกเพิ่มชั่วโมงละ 20 บาท ถ้าต่ำกว่า 4 ชม. จะเท่ากับ 20 บาท***********************/
                case 150:
                    //sql = "select store_id,concat(lpad(store_id,4,'0'),' : ',store_name) as 'บริษัท/ร้านค้า'";
                    //sql += ",cast(cast(ifnull((select name_on_card from cardpx where name = recordin.id),(select name_on_card from cardmf where name = recordin.id)) as char) as char) as 'หมายเลขบัตร'";
                    //sql += ",cast((select license from recordin where recordin.no = parking_in_id) as char) as 'ทะเบียน'";
                    //sql += ",date_format(parking_in_time,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า'";
                    //sql += ",date_format(parking_out_time,'%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก'";
                    //sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    //sql += "   lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as 'เวลาจอด'";
                    //sql += ",parking_out_price as 'ค่าบริการส่วนเพิ่ม'";
                    //sql += ",case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end as 'เวลาจอดฟรี'";
                    //sql += ",case when (case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) < 5 then 20 else (case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end-3)*10 end as 'จำนวนเงินที่ฟรี' ";
                    //sql += ",case when (case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) < 5 then 1 else (case when  estamp_hour != '9999' then estamp_hour else parking_out_hour_use end-3) end as 'จำนวนชั่วโมงที่ใช้' ";
                    //sql += ",date_format(parking_out_time,'%d/%m/%Y') as 'วันที่'";
                    //sql += ",concat(lpad(store_id,4,'0'),lpad(estamp_id,2,'0')) as 'รหัสส่วนลด'";
                    //sql += " from view_estamp left join recordin on view_estamp.parking_in_id = recordin.no";
                    //sql += " where parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (carType != Constants.TextBased.All)
                    //{
                    //    sql += " and recordin.cartype = (select cartype.typeid from cartype where typename = '" + carType + "')";
                    //}
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where m_store.store_name = '" + memberGroupMonth + "')";
                    ////if (user != Constants.TextBased.All)
                    ////    sql += " and parking_out_id = (select user.id from user where user.name = '%" + user + "%')";
                    //if (!String.IsNullOrWhiteSpace(cardId))
                    //    sql += " and recordin.id = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    //if (!String.IsNullOrWhiteSpace(licensePlate))
                    //    sql += " and recordin.license LIKE '%" + licensePlate + "%'";

                    //sql += " ORDER BY store_id,estamp_id,parking_out_time";

                    sql = "select ll as 'รหัส-ชื่อเจ้าหน้าที่',aa as store_id,cc as 'บริษัท/ร้านค้า', oo as 'หมายเลขบัตร',kk as 'ทะเบียน',ee as 'วันที่', ff as 'รหัสส่วนลด' ";
                    sql += ",pp as 'วัน-เวลาเข้า',qq as 'วัน-เวลาออก',nn as 'เวลาจอด'";
                    sql += ", CAST(ifnull(ii,0) as UNSIGNED) as 'เวลาจอดฟรี'";
                    sql += ",xx as 'จำนวนชั่วโมงที่ใช้', yy as 'จำนวนเงินที่ฟรี' ";
                    sql += ",ifnull(mm,0) as 'ค่าบริการส่วนเพิ่ม' from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";

                        sql += ",case when ((case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end)) < 5 then 1 else ";
                        sql += "((case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end)) end as xx";

                        sql += ",case when ((case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end)) < 5 then 20 else ";
                        sql += "((case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end)) * 10 end as yy";
                    }
                    else
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += ",(";
                        sql += "case when ";
                        sql += "( case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end) < 5 then 1 else ";
                        sql += "( case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end)";
                        sql += " end ";
                        sql += ") as xx";

                        sql += ",(";
                        sql += "case when ";
                        sql += "( case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end) < 5 then 20 else ";
                        sql += "( case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end) * 10 ";
                        sql += " end ";
                        sql += ") as yy";
                    }
                    sql += ",(select license from recordin where no = view_estamp.parking_out_id) as 'kk'";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as ll";
                    sql += ",(parking_out_price) as mm";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += "   lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as nn";

                    sql += ",cast(ifnull((select name_on_card from cardpx where name = (select id from recordin where recordin.no = parking_in_id)), (select name_on_card from cardmf where name = (select id from recordin where recordin.no = parking_in_id))) as char) as 'oo'"; //
                    sql += ",parking_in_time as pp,parking_out_time as qq";

                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";

                        sql += ",(";
                        sql += " case when ";
                        sql += "(  (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)) < 5 then 1 else ";
                        sql += "( (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) )";
                        sql += " end ";
                        sql += ") as xx";

                        sql += ",(";
                        sql += " case when ";
                        sql += "(  (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)) < 5 then 20 else ";
                        sql += "( (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) ) * 10 ";
                        sql += " end ";
                        sql += ") as yy";


                    }
                    else
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += ",(";
                        sql += " case when ";
                        sql += "( case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ) < 5 then 1 else ";
                        sql += "( case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end )";
                        sql += " end ";
                        sql += ") as xx";

                        sql += ",(";
                        sql += " case when ";
                        sql += "( case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ) < 5 then 20 else ";
                        sql += "( case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ) * 10 ";
                        sql += " end ";
                        sql += ") as yy";
                    }

                    sql += ",t2.license as kk";
                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as ll";
                    sql += ",(t1.price) as mm";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                    sql += "   lpad((mod(floor((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as nn";

                    sql += ",cast(cast(ifnull((select name_on_card from cardpx where name = t2.id),(select name_on_card from cardmf where name = t2.id)) as char) as char) as oo";
                    sql += ",t2.datein as pp,t1.dateout as qq";

                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += ") tt order by aa, ee, ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                           + startDate.ToLongDateString()
                           + " เวลา " + startTime.ToLongTimeString()
                           + " ถึงวันที่ " + endDate.ToLongDateString()
                           + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 151:
                    //sql = "select concat(lpad(store_id,4,'0'),' : ',store_name) as 'บริษัท/ร้านค้า',";
                    //sql += "count(*) as 'จำนวนรถ' ,sum(case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end)  as 'จำนวนชั่วโมง',";
                    //sql += "(select store_estamp_quota from m_store where view_estamp.store_id = store_id )  as 'โควต้า',";
                    //sql += "sum(parking_out_price) as 'ค่าบริการส่วนเพิ่ม',";
                    //sql += "case when (select sum(case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) from view_estamp v1 left join m_store on  v1.store_id = m_store.store_id WHERE v1.store_id = view_estamp.store_id AND v1.estamp_hour = view_estamp.estamp_hour";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "' AND parking_use_flag = 'Y'";
                    //sql += ") - ";
                    //sql += "(select store_estamp_quota from m_store where view_estamp.store_id = store_id ) < 0 then 0";
                    //sql += " else (select sum(case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) from view_estamp v1 left join m_store on  v1.store_id = m_store.store_id WHERE v1.store_id = view_estamp.store_id AND v1.estamp_hour = view_estamp.estamp_hour";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'  AND parking_use_flag = 'Y'";
                    //sql += ") - ";
                    //sql += "(select store_estamp_quota from m_store where view_estamp.store_id = store_id ) end  as 'ชั่วโมงเรียกเก็บ',";

                    //sql += "case when (select sum(case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) from view_estamp v1 left join m_store on  v1.store_id = m_store.store_id WHERE v1.store_id = view_estamp.store_id AND v1.estamp_hour = view_estamp.estamp_hour";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'  AND parking_use_flag = 'Y'";
                    //sql +=") - ";
                    //sql += "(select store_estamp_quota from m_store where view_estamp.store_id = store_id ) < 0 then 0";
                    //sql += " else ((select sum(case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end) from view_estamp v1 left join m_store on  v1.store_id = m_store.store_id WHERE v1.store_id = view_estamp.store_id AND v1.estamp_hour = view_estamp.estamp_hour";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'  AND parking_use_flag = 'Y'";
                    //sql += ") - ";
                    //sql += "(select store_estamp_quota from m_store where view_estamp.store_id = store_id ))*10 end  as 'เงินเรียกเก็บ',";
                    //sql += "store_id";
                    //sql += " from view_estamp left join recordin on view_estamp.parking_in_id = recordin.no";
                    //sql += " where parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'  AND parking_use_flag = 'Y'";
                    //if (carType != Constants.TextBased.All)
                    //{
                    //    sql += " and recordin.cartype = (select cartype.typeid from cartype where typename = '" + carType + "')";
                    //}
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where m_store.store_name = '" + memberGroupMonth + "')";
                    //sql += " group by store_id";
                    //sql += " order by store_id";

                    sql = "select ll as 'รหัส-ชื่อเจ้าหน้าที่',aa as store_id,cc as 'บริษัท/ร้านค้า', ee as 'วันที่', ff as 'รหัสคูปอง' ";
                    sql += ",qq as 'จำนวนรถ' ,ifnull(nn,0) as 'เวลาจอดจริง'";
                    sql += ", CAST(ifnull(dd,0) as UNSIGNED) as 'โควต้า', CAST(ifnull(ii,0) as UNSIGNED) as 'จำนวนชั่วโมง', ifnull(mm,0) as 'ค่าบริการส่วนเพิ่ม'";
                    sql += ",case when oo < 0 then 0 else oo end as 'ชั่วโมงเรียกเก็บ',case when pp < 0 then 0 else pp end as 'เงินเรียกเก็บ'";
                    sql += " from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";

                        sql += ", sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0) as oo";
                        sql += ", (sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0))*10 as pp";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += ",ifnull(case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end , 0)";
                        sql += "  - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0) as oo";

                        sql += ",(ifnull(case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end , 0)";
                        sql += "  - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0))*10 as pp";
                    }

                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as ll";
                    sql += ",sum(parking_out_price) as mm";
                    sql += ",concat(floor(sum((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += " lpad((mod(floor(sum(UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as nn";
                    sql += ",count(*) as qq";



                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by lpad(store_id, 4, '0')";


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";

                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0) as oo";

                        sql += " , (sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0))*10 as pp";
                    }
                    else
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += " , ifnull(case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ,0)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0) as 'oo'";

                        sql += " , (ifnull(case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ,0)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0))*10 as 'pp'";
                    }


                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as ll";
                    sql += ",sum(t1.price) as mm";
                    sql += ",concat(floor(sum((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                    sql += "   lpad((mod(floor(sum(UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as nn";
                    sql += ",count(*) as qq";


                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0')";

                    sql += ") tt order by aa, ee, ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                          + startDate.ToLongDateString()
                          + " เวลา " + startTime.ToLongTimeString()
                          + " ถึงวันที่ " + endDate.ToLongDateString()
                          + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 152:
                    //sql = "select concat(lpad(store_id,4,'0'),lpad(estamp_id,2,'0')) as 'รหัสเรียกเก็บ'";
                    //sql += ",concat(lpad(store_id,4,'0'),' : ',store_name) as 'บริษัท/ร้านค้า'";
                    //sql += ",(select license from recordin where recordin.no= parking_in_id) as 'ทะเบียน'";
                    //sql += ",parking_in_time as 'วัน-เวลาเข้า'";
                    //sql += ",parking_out_time as 'วัน-เวลาออก'";
                    //sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    //sql += "   lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as 'เวลาจอด'";
                    //sql += ",case when estamp_hour != '9999' then estamp_hour else parking_out_hour_use end as 'ชั่วโมงที่ใช้'";
                    //sql += ",parking_out_price as 'เงินเรียกเก็บ'";
                    //sql += ",parking_use_by as 'รหัสพนักงาน'";
                    //sql += ",store_id";
                    //sql += " from view_estamp left join recordin on view_estamp.parking_in_id = recordin.no";
                    //sql += " where parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (carType != Constants.TextBased.All)
                    //{
                    //    sql += " and recordin.cartype = (select cartype.typeid from cartype where typename = '" + carType + "')";
                    //}
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where m_store.store_name = '" + memberGroupMonth + "')";
                    ////if (user != Constants.TextBased.All)
                    ////    sql += " and parking_out_id = (select user.id from user where user.name = '%" + user + "%')";
                    //if (!String.IsNullOrWhiteSpace(cardId))
                    //    sql += " and recordin.id = cast(ifnull((select name from cardpx where name_on_card = '" + cardId + "'),(select name from cardmf where name_on_card = '" + cardId + "')) as char)";
                    //if (!String.IsNullOrWhiteSpace(licensePlate))
                    //    sql += " and recordin.license LIKE '%" + licensePlate + "%'";
                    //sql += " order by store_id,estamp_id,parking_out_time";

                    sql = "select ll as 'รหัสพนักงาน',aa as store_id,cc as 'บริษัท/ร้านค้า', ff as 'รหัสเรียกเก็บ' ";
                    sql += " ,qq as 'ทะเบียน' ,rr as 'วัน-เวลาเข้า',ss as 'วัน-เวลาออก'";
                    sql += ",ifnull(nn,0) as 'เวลาจอด'";
                    sql += ", CAST(ifnull(dd,0) as UNSIGNED) as 'โควต้า', CAST(ifnull(ii,0) as UNSIGNED) as 'ชั่วโมงที่ใช้', ifnull(mm,0) as 'ค่าบริการส่วนเพิ่ม'";
                    sql += ",case when oo < 0 then 0 else oo end as 'ชั่วโมงเรียกเก็บ',case when pp < 0 then 0 else pp end as 'เงินเรียกเก็บ'";
                    sql += " from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";

                        sql += ", (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0) as oo";
                        sql += ", ((case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0))*10 as pp";
                    }
                    else
                    {
                        sql += " , (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += ",ifnull(case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end , 0)";
                        sql += "  - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0) as oo";

                        sql += ",(ifnull(case when (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else (case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - (case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end , 0)";
                        sql += "  - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0))*10 as pp";
                    }

                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as ll";
                    sql += ",(parking_out_price) as mm";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += " lpad((mod(floor((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as nn";

                    sql += ",(select license from recordin where no = view_estamp.parking_out_id) as 'qq'";
                    sql += ",parking_in_time as rr,parking_out_time as ss";


                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";

                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0) as oo";

                        sql += " , ((case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0))*10 as pp";
                    }
                    else
                    {
                        sql += " , (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += " , ifnull(case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ,0)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0) as 'oo'";

                        sql += " , (ifnull(case when (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else (case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - (case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ,0)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0))*10 as 'pp'";
                    }


                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as ll";
                    sql += ",(t1.price) as mm";
                    sql += ",concat(floor(((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                    sql += "   lpad((mod(floor((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as nn";

                    sql += ",t2.license as qq";
                    sql += ",t2.datein as rr,t1.dateout as ss";


                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];


                    sql += ") tt order by aa, ee, ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                         + startDate.ToLongDateString()
                         + " เวลา " + startTime.ToLongTimeString()
                         + " ถึงวันที่ " + endDate.ToLongDateString()
                         + " เวลา " + endTime.ToLongTimeString();

                    break;

                case 153:
                    //sql = "select concat(lpad(store_id,2,'0'),' : ',store_name) as 'บริษัท/ร้านค้า'";
                    //sql += ",date_format(parking_out_time,'%d/%m/%Y') as 'วันที่' ";
                    //sql += ",count(*) as 'จำนวนรถ'";
                    //sql += ",concat(floor(sum((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    //sql += "   lpad((mod(floor(sum(UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as 'เวลาจอดรวม'";
                    //sql += "  ,concat(floor(sum((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time)))/count(*)/60/60),'.',";
                    //sql += "  lpad((mod(floor(sum(UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/count(*)/60),60)),2,'0')) as 'เวลาจอดเฉลี่ย'";
                    //sql += " ,sum(parking_out_price) as 'จำนวนเงิน'";
                    //sql += " ,store_id";
                    //sql += " from view_estamp left join recordin on view_estamp.parking_in_id = recordin.no";
                    //sql += " where parking_use_flag = 'Y'";
                    //sql += " AND date_format(parking_out_time,'%Y-%m-%d') BETWEEN  '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
                    //if (carType != Constants.TextBased.All)
                    //{
                    //    sql += " and recordin.cartype = (select cartype.typeid from cartype where typename = '" + carType + "')";
                    //}
                    //if (memberGroupMonth != Constants.TextBased.All)
                    //    sql += " and store_id = (select store_id from m_store where m_store.store_name = '" + memberGroupMonth + "')";

                    //sql += " GROUP by store_id,date_format(parking_out_time,'%d/%m/%Y')";
                    //sql += " order by store_id,parking_in_time";

                    sql = "select ll as 'รหัส-ชื่อเจ้าหน้าที่',aa as store_id,cc as 'บริษัท/ร้านค้า', ee as 'วันที่', ff as 'รหัสคูปอง' ";
                    sql += ",qq as 'จำนวนรถ' ,ROUND(nn/qq,2) as 'เวลาจอดเฉลี่ย',ifnull(nn,0) as 'เวลาจอดรวม'";
                    sql += ", CAST(ifnull(ii,0) as UNSIGNED) as 'จำนวนชั่วโมง', ifnull(mm,0) as 'จำนวนเงิน'";
                    sql += " from";

                    sql += " (select lpad(store_id, 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = view_estamp.store_id) as 'bb'";
                    sql += " , store_name as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) as char) as 'dd'";
                    sql += " , date_format(parking_out_time, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad(store_id, 4, '0'), lpad(estamp_id, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'ii'";

                        sql += ", sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0) as oo";
                        sql += ", (sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0))*10 as pp";
                    }
                    else
                    {
                        sql += " , sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += ",ifnull(case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end , 0)";
                        sql += "  - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0) as oo";

                        sql += ",(ifnull(case when sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) > sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) THEN 0 else sum(case when estamp_hour = 9999 then parking_out_hour_use else estamp_hour end) - sum(case when (select id from holiday where date = date(parking_out_time)) > 0 then 3 when DAYOFWEEK(parking_out_time) in (1, 7) then 3 else 1 end) end , 0)";
                        sql += "  - ifnull((select store_estamp_quota from m_store where m_store.store_id = view_estamp.store_id) ,0))*10 as pp";
                    }

                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = view_estamp.parking_use_by) as ll";
                    sql += ",sum(parking_out_price) as mm";
                    sql += ",concat(floor(sum((UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60/60)),'.',";
                    sql += " lpad((mod(floor(sum(UNIX_TIMESTAMP(parking_out_time) - UNIX_TIMESTAMP(parking_in_time))/60),60)),2,'0')) as nn";
                    sql += ",count(*) as qq";



                    sql += " from view_estamp ";
                    sql += " where parking_out_time between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    sql += " and parking_use_flag = 'Y'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and store_id = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by lpad(store_id, 4, '0'),date_format(parking_out_time, '%d/%m/%Y')";


                    sql += " union ";

                    sql += " select lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0') as 'aa'";
                    sql += " , (select customer_code from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'bb'";
                    sql += " , (select store_name from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as 'cc'";
                    sql += " , cast((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) as char) as 'dd'";
                    sql += " , date_format(t1.dateout, '%d/%m/%Y') as 'ee'";
                    sql += " , concat(lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'), lpad(t1.proid, 3, '0')) as 'ff'";
                    if (Configs.Reports.UseReport108_110_1)
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , 0 as 'hh'";
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'ii'";

                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0) as oo";

                        sql += " , (sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0))*10 as pp";
                    }
                    else
                    {
                        sql += " , sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0) else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) as 'gg'";
                        sql += " , sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) as 'hh'";
                        sql += " , case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end as 'ii'";

                        sql += " , ifnull(case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ,0)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0) as 'oo'";

                        sql += " , (ifnull(case when sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) > sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) THEN 0 else sum(case when (select price from promotion where promotion.id = t1.proid) = 9999 then TIMESTAMPDIFF(HOUR, t2.datein, t1.dateout) + if(lpad(mod(timestampdiff(minute, date_format(t2.datein, '%Y-%m-%d %H:%i:%s'), date_format(t1.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0') > 0, 1, 0)  else (select FLOOR(minute/60) from promotion where promotion.id = t1.proid) end) - sum(case when (select id from holiday where date = date(t1.dateout)) > 0 then 3 when DAYOFWEEK(t1.dateout) in (1, 7) then 3 else 1 end) end ,0)";
                        sql += " - ";
                        sql += "ifnull((select store_estamp_quota from m_store where m_store.store_id = (select groupro from promotion where promotion.id = t1.proid)) ,0))*10 as 'pp'";
                    }


                    sql += ",(select concat(lpad(cardname,10,0),' : ',name) from user where user.id = t1.userout) as ll";
                    sql += ",sum(t1.price) as mm";
                    sql += ",concat(floor(sum((UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60/60)),'.',";
                    sql += "   lpad((mod(floor(sum(UNIX_TIMESTAMP(t1.dateout) - UNIX_TIMESTAMP(t2.datein))/60),60)),2,'0')) as nn";
                    sql += ",count(*) as qq";


                    sql += " from recordout t1 left join recordin t2 on t1.no = t2.no";
                    sql += " where (t1.proid between 1 and 255) and ";
                    sql += " t1.dateout between '" + startDateTimeText + "' and '" + endDateTimeText + "'";
                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and (select groupro from promotion where promotion.id = t1.proid) = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];
                    sql += " group by lpad((select groupro from promotion where promotion.id = t1.proid), 4, '0'),date_format(t1.dateout, '%d/%m/%Y')";

                    sql += ") tt order by aa, ee, ff";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                         + startDate.ToLongDateString()
                         + " เวลา " + startTime.ToLongTimeString()
                         + " ถึงวันที่ " + endDate.ToLongDateString()
                         + " เวลา " + endTime.ToLongTimeString();
                    break;
                case 154: //Mac 2021/11/23
                    sql = " DROP TABLE IF EXISTS `report155`;";
                    sql += " CREATE TABLE `report155` (";
                    sql += "  `Id` int(11) NOT NULL AUTO_INCREMENT,";
                    sql += "  `price` varchar(50) CHARACTER SET utf8 DEFAULT NULL,";
                    sql += "  `totalcar` int(11) DEFAULT '0',";
                    sql += "  `totalincome` decimal(20,2) DEFAULT '0',";
                    sql += "  PRIMARY KEY (`Id`)";
                    sql += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                    DbController.LoadData(sql);

                    for (int i = 0; i < 31; i++)
                    {
                        if (i == 30)
                        {
                            sql = "insert into report155 values (" + (i + 1) + ", '300 ขึ้นไป'";
                            sql += ", (select count(no) from recordout where dateout between '" + startDateTimeText + "' AND '" + endDateTimeText + "' and price > 300)";
                            sql += ", (select ifnull(sum(price), 0) from recordout where dateout between '" + startDateTimeText + "' AND '" + endDateTimeText + "' and price > 300)";
                            sql += ")";
                        }
                        else
                        {
                            sql = "insert into report155 values (" + (i + 1) + ", '" + (i + 1) * 10 + "'";
                            sql += ", (select count(no) from recordout where dateout between '" + startDateTimeText + "' AND '" + endDateTimeText + "' and price = " + (i + 1) * 10 + ")";
                            sql += ", (select ifnull(sum(price), 0) from recordout where dateout between '" + startDateTimeText + "' AND '" + endDateTimeText + "' and price = " + (i + 1) * 10 + ")";
                            sql += ")";
                        }
                        DbController.SaveData(sql);
                    }
                    sql = "select `Id` as 'ลำดับที่', `price` as 'รายได้', `totalcar` as 'จำนวนรายการ (คัน)', cast(`totalincome` as DECIMAL(10,2)) as 'จำนวนเงิน' from report155";
                    sql += " order by Id";
                    break;
                case 155: //Mac 2021/11/25
                    sql = " select (select groupname from membergroupprice_month where id = memgrouppriceid_month) as 'กลุ่มรายเดือน'";
                    sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(member.address, '|', 1), '|', -1) as 'รหัสประจำตัว'";
                    sql += ", member.name as 'ชื่อ-สกุลผู้ถือบัตร'";
                    sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(member.address, '|', 2), '|', -1) as 'แผนก'";
                    sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(member.address, '|', 3), '|', -1) as 'ตำแหน่ง'";
                    sql += ", SUBSTRING_INDEX(SUBSTRING_INDEX(member.address, '|', 4), '|', -1)  as 'ที่อยู่'";
                    sql += ", member.license as 'ทะเบียนรถ'";
                    sql += ", cast(ifnull((select name_on_card from cardpx where cardpx.name = member.cardid), (select name_on_card from cardmf where cardmf.name = member.cardid)) as char)  as 'หมายเลขบัตร'";
                    sql += ", member.tel as 'เบอร์โทรศัพท์'";
                    sql += ", date_format(member.datestart, '%d/%m/%Y') as 'วันที่สร้างข้อมูล'";
                    sql += ", date_format(member.dateexprie, '%d/%m/%Y') as 'วันที่หมดอายุ'";
                    sql += " from member";
                    sql += " left join cardmf t1 on member.cardid = t1.name";
                    sql += " left join cardpx t2 on member.cardid = t2.name";
                    sql += " where 1 = 1";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " and member.license LIKE '%" + licensePlate + "%'";

                    if (!String.IsNullOrEmpty(cardId))
                        sql += " and (t1.name_on_card like '" + cardId + "%' or t2.name_on_card like '" + cardId + "%')";

                    if (address != "")
                        sql += " and member.address like '%" + address + "%'";

                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += " and member.memgrouppriceid_month = " + AppGlobalVariables.MemberGroupMonthsToId[memberGroupMonth];

                    sql += " order by (select nogroup from membergroupprice_month where id = member.memgrouppriceid_month), SUBSTRING_INDEX(SUBSTRING_INDEX(member.address, '|', 1), '|', -1)";
                    break;

                case 156:
                    sql = "SELECT ";
                    sql += "m_store.customer_code AS 'รหัสลูกค้า (CV-CODE)', ";
                    sql += "m_store.store_name AS 'บริษัท/ร้านค้า', ";
                    sql += "CAST(m_store.store_estamp_quota AS CHAR) AS 'โควต้า (ชั่วโมง)', ";
                    sql += "DATE_FORMAT(tgei_date,'%d/%m/%Y') AS 'วันที่', ";
                    sql += "CAST(tran_gen_estamp_info.create_by AS CHAR) AS 'เจ้าหน้าที่', ";
                    sql += "tran_gen_estamp_info.lot_uuid AS 'รหัสสร้าง QR CODE', ";
                    sql += "tran_gen_estamp_info.estamp_name AS 'ชื่อโปรโมชั่น', ";
                    sql += "tran_gen_estamp_info.estamp_amount AS 'จำนวน QR CODE', ";
                    sql += "CASE ";
                    sql += "WHEN estamp_name = 'รถยนต์ 1 ชั่วโมง' THEN estamp_amount*1 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 2 ชั่วโมง' THEN estamp_amount*2 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 3 ชั่วโมง' THEN estamp_amount*3 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 4 ชั่วโมง' THEN estamp_amount*4 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 5 ชั่วโมง' THEN estamp_amount*5 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 6 ชั่วโมง' THEN estamp_amount*6 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 7 ชั่วโมง' THEN estamp_amount*7 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 8 ชั่วโมง' THEN estamp_amount*8 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 9 ชั่วโมง' THEN estamp_amount*9 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 10 ชั่วโมง' THEN estamp_amount*10 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 11 ชั่วโมง' THEN estamp_amount*11 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 12 ชั่วโมง' THEN estamp_amount*12 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 13 ชั่วโมง' THEN estamp_amount*13 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 14 ชั่วโมง' THEN estamp_amount*14 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 15 ชั่วโมง' THEN estamp_amount*15 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 16 ชั่วโมง' THEN estamp_amount*16 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 17 ชั่วโมง' THEN estamp_amount*17 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 18 ชั่วโมง' THEN estamp_amount*18 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 19 ชั่วโมง' THEN estamp_amount*19 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 20 ชั่วโมง' THEN estamp_amount*20 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 21 ชั่วโมง' THEN estamp_amount*21 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 22 ชั่วโมง' THEN estamp_amount*22 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 23 ชั่วโมง' THEN estamp_amount*23 ";
                    sql += "WHEN estamp_name = 'รถยนต์ 24 ชั่วโมง' THEN estamp_amount*24 ";
                    sql += "ELSE Null ";
                    sql += "END AS 'รวมโควต้า (ชั่วโมง)' ";
                    sql += "FROM tran_gen_estamp_info INNER JOIN m_store ON m_store.store_id = tran_gen_estamp_info.store_id ";
                    sql += "WHERE tgei_date BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.ToString("yyyy-MM-dd") + "' ";
                    sql += "AND tran_gen_estamp_info.estamp_amount <> 0 AND m_store.store_name NOT LIKE '%ซื้อเพิ่ม%' ";
                    if (memberGroupMonth != Constants.TextBased.All)
                    {
                        sql += "AND m_store.store_name = '" + memberGroupMonth + "' ";
                    }
                    sql += "ORDER BY m_store.customer_code, tgei_date, tran_gen_estamp_info.lot_uuid, CAST(SUBSTRING_INDEX(estamp_name, ' ', -2) AS UNSIGNED), estamp_name";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                         + startDate.ToLongDateString()
                         + " เวลา " + startTime.ToLongTimeString()
                         + " ถึงวันที่ " + endDate.ToLongDateString()
                         + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 157:
                    sql = "SELECT ";
                    sql += "m_store.customer_code AS 'รหัสลูกค้า (CV-CODE)', ";
                    sql += "m_store.store_name AS 'บริษัท/ร้านค้า', ";
                    sql += "CAST(m_store.store_estamp_quota AS CHAR) AS 'โควต้า (ชั่วโมง)', ";
                    sql += "DATE_FORMAT(tgei_date,'%d/%m/%Y') AS 'วันที่', ";
                    sql += "CAST(tran_gen_estamp_info.create_by AS CHAR) AS 'เจ้าหน้าที่', ";
                    sql += "tran_gen_estamp_info.lot_uuid AS 'รหัสสร้าง QR CODE', ";
                    sql += "tran_gen_estamp_info.estamp_name AS 'ชื่อโปรโมชั่น', ";
                    sql += "tran_gen_estamp_info.estamp_amount AS 'จำนวน QR CODE', ";
                    sql += "CASE ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 1 ชั่วโมง' THEN estamp_amount*1 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 2 ชั่วโมง' THEN estamp_amount*2 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 3 ชั่วโมง' THEN estamp_amount*3 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 4 ชั่วโมง' THEN estamp_amount*4 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 5 ชั่วโมง' THEN estamp_amount*5 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 6 ชั่วโมง' THEN estamp_amount*6 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 7 ชั่วโมง' THEN estamp_amount*7 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 8 ชั่วโมง' THEN estamp_amount*8 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 9 ชั่วโมง' THEN estamp_amount*9 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 10 ชั่วโมง' THEN estamp_amount*10 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 11 ชั่วโมง' THEN estamp_amount*11 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 12 ชั่วโมง' THEN estamp_amount*12 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 13 ชั่วโมง' THEN estamp_amount*13 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 14 ชั่วโมง' THEN estamp_amount*14 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 15 ชั่วโมง' THEN estamp_amount*15 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 16 ชั่วโมง' THEN estamp_amount*16 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 17 ชั่วโมง' THEN estamp_amount*17 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 18 ชั่วโมง' THEN estamp_amount*18 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 19 ชั่วโมง' THEN estamp_amount*19 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 20 ชั่วโมง' THEN estamp_amount*20 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 21 ชั่วโมง' THEN estamp_amount*21 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 22 ชั่วโมง' THEN estamp_amount*22 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 23 ชั่วโมง' THEN estamp_amount*23 ";
                    sql += "WHEN estamp_name = 'รถจักรยานยนต์ 24 ชั่วโมง' THEN estamp_amount*24 ";
                    sql += "ELSE Null ";
                    sql += "END AS 'รวมโควต้า (ชั่วโมง)' ";
                    sql += "FROM tran_gen_estamp_info INNER JOIN m_store ON m_store.store_id = tran_gen_estamp_info.store_id ";
                    sql += "WHERE tgei_date BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.ToString("yyyy-MM-dd") + "' ";
                    sql += "AND tran_gen_estamp_info.estamp_amount <> 0 AND m_store.store_name NOT LIKE '%ซื้อเพิ่ม%' ";
                    if (memberGroupMonth != Constants.TextBased.All)
                    {
                        sql += "AND m_store.store_name = '" + memberGroupMonth + "' ";
                    }
                    sql += "ORDER BY m_store.customer_code, tgei_date, tran_gen_estamp_info.lot_uuid, CAST(SUBSTRING_INDEX(estamp_name, ' ', -2) AS UNSIGNED), estamp_name";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                         + startDate.ToLongDateString()
                         + " เวลา " + startTime.ToLongTimeString()
                         + " ถึงวันที่ " + endDate.ToLongDateString()
                         + " เวลา " + endTime.ToLongTimeString();
                    break;

                // กรณี ซื้อเพิ่ม 

                case 158: // รายงานสรุปจำนวนการสร้าง QR CODE รถจักรยานยนต์ ซื้อเพิ่ม
                    //if (MemberGroupMonthComboBox.SelectedIndex <= 0) MessageBox.Show("กรุณาเลือกกลุ่ม 'บริษัทผู้ถือบัตร' ก่อนทำรายการค้นหา");

                    sql = "SELECT ";
                    sql += "m_store.customer_code AS 'รหัสลูกค้า (CV-CODE)', ";
                    sql += "m_store.store_name AS 'บริษัท/ร้านค้า', ";
                    sql += "CAST(m_store.store_estamp_quota AS CHAR) AS 'โควต้า (ชั่วโมง)', ";
                    sql += "DATE_FORMAT(tgei_date,'%d/%m/%Y') AS 'วันที่', ";
                    sql += "CAST(tran_gen_estamp_info.create_by AS CHAR) AS 'เจ้าหน้าที่', ";
                    sql += "tran_gen_estamp_info.lot_uuid AS 'รหัสสร้าง QR CODE', ";
                    sql += "tran_gen_estamp_info.estamp_name AS 'ชื่อโปรโมชั่น', ";
                    sql += "tran_gen_estamp_info.estamp_amount AS 'จำนวน QR CODE', ";
                    sql += "CASE ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 2 ชั่วโมง' THEN 20 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 3 ชั่วโมง' THEN 40 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 4 ชั่วโมง' THEN 60 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 5 ชั่วโมง' THEN 80 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 6 ชั่วโมง' THEN 100 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 7 ชั่วโมง' THEN 120 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 8 ชั่วโมง' THEN 140 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 9 ชั่วโมง' THEN 160 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 10 ชั่วโมง' THEN 180 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 11 ชั่วโมง' THEN 200 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 12 ชั่วโมง' THEN 220 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 13 ชั่วโมง' THEN 240 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 14 ชั่วโมง' THEN 260 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 15 ชั่วโมง' THEN 280 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 16 ชั่วโมง' THEN 300 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 17 ชั่วโมง' THEN 320 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 18 ชั่วโมง' THEN 340 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 24 ชั่วโมง' THEN 460 ";
                    sql += "   ELSE NULL ";
                    sql += "END AS 'มูลค่า QR CODE ต่อใบ', ";
                    sql += "CASE ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 2 ชั่วโมง' THEN estamp_amount*20 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 3 ชั่วโมง' THEN estamp_amount*40 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 4 ชั่วโมง' THEN estamp_amount*60 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 5 ชั่วโมง' THEN estamp_amount*80 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 6 ชั่วโมง' THEN estamp_amount*100 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 7 ชั่วโมง' THEN estamp_amount*120 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 8 ชั่วโมง' THEN estamp_amount*140 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 9 ชั่วโมง' THEN estamp_amount*160 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 10 ชั่วโมง' THEN estamp_amount*180 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 11 ชั่วโมง' THEN estamp_amount*200 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 12 ชั่วโมง' THEN estamp_amount*220 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 13 ชั่วโมง' THEN estamp_amount*240 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 14 ชั่วโมง' THEN estamp_amount*260 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 15 ชั่วโมง' THEN estamp_amount*280 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 16 ชั่วโมง' THEN estamp_amount*300 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 17 ชั่วโมง' THEN estamp_amount*320 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 18 ชั่วโมง' THEN estamp_amount*340 ";
                    sql += "   WHEN estamp_name = 'รถยนต์ 24 ชั่วโมง' THEN estamp_amount*460 ";
                    sql += "   ELSE NULL ";
                    sql += "END AS 'รวมเป็นเงิน' ";
                    sql += "FROM tran_gen_estamp_info INNER JOIN m_store ON m_store.store_id=tran_gen_estamp_info.store_id ";
                    sql += "WHERE tgei_date BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.ToString("yyyy-MM-dd") + "' ";
                    sql += "AND tran_gen_estamp_info.estamp_amount <> 0 AND m_store.store_name LIKE '%ซื้อเพิ่ม%' ";
                    sql += "AND m_store.store_name = '" + memberGroupMonth + "' ";
                    sql += "ORDER BY tgei_date, tran_gen_estamp_info.lot_uuid, CAST(SUBSTRING_INDEX(estamp_name, ' ', -2) AS UNSIGNED), estamp_name";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                         + startDate.ToLongDateString()
                         + " เวลา " + startTime.ToLongTimeString()
                         + " ถึงวันที่ " + endDate.ToLongDateString()
                         + " เวลา " + endTime.ToLongTimeString();
                    break;


                case 159: // รายงานสรุปจำนวนการสร้าง QR CODE รถจักรยานยนต์ ซื้อเพิ่ม
                    //if (MemberGroupMonthComboBox.SelectedIndex <= 0) MessageBox.Show("กรุณาเลือกกลุ่ม 'บริษัทผู้ถือบัตร' ก่อนทำรายการค้นหา");

                    sql = "SELECT ";
                    sql += "m_store.customer_code AS 'รหัสลูกค้า (CV-CODE)', ";
                    sql += "m_store.store_name AS 'บริษัท/ร้านค้า', ";
                    sql += "CAST(m_store.store_estamp_quota AS CHAR) AS 'โควต้า (ชั่วโมง)', ";
                    sql += "DATE_FORMAT(tgei_date,'%d/%m/%Y') AS 'วันที่', ";
                    sql += "CAST(tran_gen_estamp_info.create_by AS CHAR) AS 'เจ้าหน้าที่', ";
                    sql += "tran_gen_estamp_info.lot_uuid AS 'รหัสสร้าง QR CODE', ";
                    sql += "tran_gen_estamp_info.estamp_name AS 'ชื่อโปรโมชั่น', ";
                    sql += "tran_gen_estamp_info.estamp_amount AS 'จำนวน QR CODE', ";
                    sql += "CASE ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 1 ชั่วโมง' THEN 10 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 2 ชั่วโมง' THEN 20 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 3 ชั่วโมง' THEN 30 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 4 ชั่วโมง' THEN 40 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 5 ชั่วโมง' THEN 50 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 6 ชั่วโมง' THEN 60 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 7 ชั่วโมง' THEN 70 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 8 ชั่วโมง' THEN 80 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 9 ชั่วโมง' THEN 90 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 10 ชั่วโมง' THEN 100 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 11 ชั่วโมง' THEN 110 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 12 ชั่วโมง' THEN 120 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 13 ชั่วโมง' THEN 130 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 14 ชั่วโมง' THEN 140 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 15 ชั่วโมง' THEN 150 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 16 ชั่วโมง' THEN 160 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 17 ชั่วโมง' THEN 170 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 18 ชั่วโมง' THEN 180 ";
                    sql += "   ELSE NULL ";
                    sql += "END AS 'มูลค่า QR CODE ต่อใบ', ";
                    sql += "CASE ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 1 ชั่วโมง' THEN estamp_amount*10 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 2 ชั่วโมง' THEN estamp_amount*20 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 3 ชั่วโมง' THEN estamp_amount*30 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 4 ชั่วโมง' THEN estamp_amount*40 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 5 ชั่วโมง' THEN estamp_amount*50 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 6 ชั่วโมง' THEN estamp_amount*60 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 7 ชั่วโมง' THEN estamp_amount*70 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 8 ชั่วโมง' THEN estamp_amount*80 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 9 ชั่วโมง' THEN estamp_amount*90 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 10 ชั่วโมง' THEN estamp_amount*100 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 11 ชั่วโมง' THEN estamp_amount*110 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 12 ชั่วโมง' THEN estamp_amount*120 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 13 ชั่วโมง' THEN estamp_amount*130 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 14 ชั่วโมง' THEN estamp_amount*140 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 15 ชั่วโมง' THEN estamp_amount*150 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 16 ชั่วโมง' THEN estamp_amount*160 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 17 ชั่วโมง' THEN estamp_amount*170 ";
                    sql += "   WHEN estamp_name = 'รถจักรยานยนต์ 18 ชั่วโมง' THEN estamp_amount*180 ";
                    sql += "   ELSE NULL ";
                    sql += "END AS 'รวมเป็นเงิน' ";
                    sql += "FROM tran_gen_estamp_info INNER JOIN m_store ON m_store.store_id=tran_gen_estamp_info.store_id ";
                    sql += "WHERE tgei_date BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.ToString("yyyy-MM-dd") + "' ";
                    sql += "AND tran_gen_estamp_info.estamp_amount <> 0 AND m_store.store_name LIKE '%ซื้อเพิ่ม%' ";
                    sql += "AND m_store.store_name = '" + memberGroupMonth + "' ";
                    sql += "ORDER BY tgei_date, tran_gen_estamp_info.lot_uuid, CAST(SUBSTRING_INDEX(estamp_name, ' ', -2) AS UNSIGNED), estamp_name";

                    AppGlobalVariables.ConditionText = "จากวันที่ "
                       + startDate.ToLongDateString()
                       + " เวลา " + startTime.ToLongTimeString()
                       + " ถึงวันที่ " + endDate.ToLongDateString()
                       + " เวลา " + endTime.ToLongTimeString();
                    break;

                case 160: // ธนภูมิ #33.รายงาน E-Stamp รถยนต์
                    sql = "select t1.no as 'ลำดับ', (SELECT typename FROM cartype WHERE typeid = t2.cartype) as 'ประเภท'";
                    sql += ", t2.license as 'ทะเบียน', date_format(t2.datein, '%d/%m/%Y %H:%i:%s') as 'เวลาเข้า'";
                    sql += ", t1.user_name as 'ชื่อผู้ให้ส่วนลด', t1.promotion_name as 'ชื่อโปรโมชั่น', date_format(t1.date_estamp, '%d/%m/%Y %H:%i:%s') as 'เวลาให้ส่วนลด'";
                    sql += " from estamprecord t1 left join recordin t2 on t1.record_no = t2.no";
                    sql += " WHERE t1.date_estamp BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                    if ((user != "ALL") && (user.Length > 0))
                        sql += " AND t1.user_name = '" + user + "'";
                    if (promotionName != Constants.TextBased.All)
                        sql += " AND t1.promotion_name = '" + promotionName + "'";
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t2.license LIKE '%" + licensePlate + "%'";
                    if (carType == Constants.TextBased.Visitor)
                        sql += " AND t2.cartype != 200";
                    else if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                        sql += " AND t2.typeid =" + carTypeId;
                    sql += " ORDER BY t1.no";
                    break;

                case 161: // ธนภูมิ #34.สรุปค่าบริการรายเดือน Member รถยนต์
                    if (Configs.Reports.UseReportThanapoom)
                    {
                        StringBuilder sqlBuilder = new StringBuilder();
                        sqlBuilder.AppendLine("SELECT DISTINCT");
                        sqlBuilder.AppendLine("m.id AS member_id,");
                        sqlBuilder.AppendLine("m.name AS member_name,");
                        sqlBuilder.AppendLine("m.address AS member_address,");
                        sqlBuilder.AppendLine("m.license AS member_license,");
                        sqlBuilder.AppendLine("vg.id AS vendor_id,");
                        sqlBuilder.AppendLine("vg.vendor_name AS vendor_name,");
                        sqlBuilder.AppendLine("m.memgrouppriceid_pay AS memgrouppriceid_pay");
                        sqlBuilder.AppendLine("FROM member m");
                        sqlBuilder.AppendLine("JOIN vendor_group vg");
                        sqlBuilder.AppendLine("ON vg.id = m.memgrouppriceid_month");
                        sqlBuilder.AppendLine("WHERE 1 = 1");

                        if (memberGroupMonth != Constants.TextBased.All)
                        {
                            sqlBuilder.AppendLine($"AND vg.id = '{memberGroupMonthId}'");
                        }
                        if (isLegitPromotionRange)
                            sqlBuilder.AppendLine($"AND vg.id BETWEEN {promotionRangeFrom} AND {promotionRangeTo}");

                        sqlBuilder.AppendLine("ORDER BY vg.id;");

                        sql = sqlBuilder.ToString();
                    }
                    else
                    {
                        string fontSlip161 = "";
                        if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                            fontSlip161 = AppGlobalVariables.Printings.ReceiptName;
                        else
                        {
                            if (!Configs.UseReceiptName)
                                fontSlip161 = "IV";
                        }

                        sql = "select cast(t2.id as char) as 'หมายเลขบัตร', t2.license as 'ทะเบียนรถ', date_format(t2.datein, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า', date_format(t3.dateout, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก'";
                        if (Configs.UseReceiptFor1Out)
                        {
                            if (Configs.OutReceiptNameMonth)
                            {
                                sql += ", concat(t3.receipt, concat(date_format(t3.dateout,'%y%m') ,lpad(t3.printno,6,'0'))) as 'ใบกำกับภาษี'";
                            }
                            else
                            {
                                sql += ", concat(t3.receipt, concat(date_format(t3.dateout,'%y') ,lpad(t3.printno,6,'0'))) as 'ใบกำกับภาษี'";
                            }
                        }
                        else
                        {
                            if (Configs.OutReceiptNameMonth)
                            {
                                sql += ", concat('" + fontSlip161 + "', concat(date_format(t3.dateout,'%y%m') ,lpad(t3.printno,6,'0'))) as 'ใบกำกับภาษี'";
                            }
                            else
                            {

                                sql += ", concat('" + fontSlip161 + "', concat(date_format(t3.dateout,'%y') ,lpad(t3.printno,6,'0'))) as 'ใบกำกับภาษี'";
                            }
                        }
                        sql += ", t1.price_old as 'จำนวนเดิม', t1.price as 'จำนวนใหม่', (select name from user where id = t1.user) as 'ปรับปรุงโดย'";
                        sql += ", date_format(t1.datemodify, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาปรับปรุง', t1.reason as 'สาเหตุ'";
                        sql += " from modify_amount t1 left join recordin t2 on t1.no_recordin = t2.no left join recordout t3 on t1.no_recordin = t3.no";
                        sql += " WHERE t1.datemodify BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                        if (user != Constants.TextBased.All)
                            sql += " AND t1.user = " + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                        if (promotionName != Constants.TextBased.All)
                            sql += " AND t3.proid =" + promotionId;
                        if (!String.IsNullOrEmpty(licensePlate))
                            sql += " AND t2.license LIKE '%" + licensePlate + "%'";
                        if (!String.IsNullOrEmpty(cardId))
                            sql += " AND t2.id = " + cardId;

                        sql += " ORDER BY t1.no";

                        sql = "select cast(t2.id as char) as 'หมายเลขบัตร', t2.license as 'ทะเบียนรถ', date_format(t2.datein, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า', date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก'";
                        if (Configs.UseReceiptFor1Out)
                        {
                            if (Configs.OutReceiptNameMonth)
                            {
                                sql += ", concat(t1.receipt, concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี'";
                            }
                            else
                            {
                                sql += ", concat(t1.receipt, concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี'";
                            }
                        }
                        else
                        {
                            if (Configs.OutReceiptNameMonth)
                            {
                                sql += ", concat('" + fontSlip161 + "', concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี'";
                            }
                            else
                            {

                                sql += ", concat('" + fontSlip161 + "', concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี'";
                            }
                        }
                        sql += ", t3.price_old as 'จำนวนเดิม', t3.price as 'จำนวนใหม่', (select name from user where id = t3.user) as 'ปรับปรุงโดย'";
                        sql += ", date_format(t3.datemodify, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาปรับปรุง', t3.reason as 'สาเหตุ'";
                        sql += " from recordout t1 left join recordin t2 on t1.no = t2.no left join modify_amount t3 on t1.no_modify = t3.no";
                        sql += " WHERE t3.datemodify BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

                        if (user != Constants.TextBased.All)
                            sql += " AND t3.user = " + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                        if (promotionName != Constants.TextBased.All)
                            sql += " AND t1.proid =" + promotionId;
                        if (!String.IsNullOrEmpty(licensePlate))
                            sql += " AND t2.license LIKE '%" + licensePlate + "%'";
                        if (!String.IsNullOrEmpty(cardId))
                            sql += " AND t2.id = " + cardId;
                        sql += " ORDER BY t3.no";
                    }
                    break;

                case 162: // ธนภูมิ #35.สรุปค่าบริการรายวัน
                    if (Configs.Reports.UseReportThanapoom)
                    {
                        promotionRangeFrom = 700;
                        promotionRangeTo = 999;

                        sql = GetPromotionUsage();
                    }
                    else
                    {
                        string fontSlip162 = "";
                        if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                            fontSlip162 = AppGlobalVariables.Printings.ReceiptName;
                        else
                        {
                            if (!Configs.UseReceiptName)
                                fontSlip162 = "IV";
                        }

                        sql = "select cast(t2.no as char) as 'หมายเลขบัตร', t2.license as 'ทะเบียนรถ', date_format(t2.datein, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า', date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก',\n";
                        if (Configs.UseReceiptFor1Out)
                        {
                            if (Configs.OutReceiptNameMonth)
                                sql += "concat(t1.receipt, concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี',\n";
                            else
                                sql += "concat(t1.receipt, concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี',\n";
                        }
                        else
                        {
                            if (Configs.OutReceiptNameMonth)
                                sql += "concat('" + fontSlip162 + "', concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี',\n";
                            else
                                sql += "concat('" + fontSlip162 + "', concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี',\n";
                        }

                        sql += "format(t1.price, 2) as 'รายได้',\n";
                        if (Configs.UsePaymentKsher)
                        {
                            sql += "case when t3.channel = 'TrueMoney' then 'TrueMoney' when t3.channel = 'promptpay' then 'PromptPay' when t1.pay_type = 'EDC' then 'EDC' else 'เงินสด' end as 'ช่องทางการชำระเงิน',\n";
                            sql += "t3.ksher_order_no as 'ksher order no', t3.mch_order_no as 'mch order no' ";
                            sql += "from recordout t1 left join recordin t2 on t1.no = t2.no ";
                            sql += "left join (select max(t1.no), t1.no_recordin, t1.mch_order_no, t1.channel, t1.status, t2.ksher_order_no\n";
                            sql += "from ksherpay_post t1 left join ksherpay_get t2 on t1.mch_order_no = t2.mch_order_no where t1.status = 'Y' group by t1.no_recordin) t3 on t1.no = t3.no_recordin\n";
                        }
                        else if (Configs.UsePaymentBeam)
                        {
                            sql += "CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END AS 'ช่องทางการชำระเงิน',\n";
                            sql += "t3.qr as 'QR', t3.beam_id as 'Beam ID'\n";
                            sql += "from recordout t1 left join recordin t2 on t1.no = t2.no ";
                            sql += "left join (select max(t1.no), t1.no_recordin, t1.beam_id, t1.status, t2.qr\n";
                            sql += "from beam_post t1 left join beam_get t2 on t1.beam_id = t2.beam_id where t1.status = 'Y' group by t1.no_recordin) t3 on t1.no = t3.no_recordin\n";
                        }
                        else if (Configs.UsePaymentRabbit)
                        {
                            sql += "CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END AS 'ช่องทางการชำระเงิน',\n";
                            sql += "t3.qr as 'QR', t3.rabbit_id as 'rabbit ID'\n";
                            sql += "from recordout t1 left join recordin t2 on t1.no = t2.no ";
                            sql += "left join (select max(t1.no), t1.no_recordin, t1.rabbit_id, t1.status, t2.qr\n";
                            sql += "from rabbit_post t1 left join rabbit_get t2 on t1.rabbit_id = t2.rabbit_id where t1.status = 'Y' group by t1.no_recordin) t3 on t1.no = t3.no_recordin\n";
                        }

                        sql += "WHERE t1.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'\n";
                        sql += "AND t1.price > 0";

                        if (user != Constants.TextBased.All)
                            sql += " AND t1.userout = " + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                        if (promotionName != Constants.TextBased.All)
                            sql += " AND t1.proid =" + promotionId;
                        if (!String.IsNullOrEmpty(licensePlate))
                            sql += " AND t2.license LIKE '%" + licensePlate + "%'";
                        if (!String.IsNullOrEmpty(cardId))
                            sql += " AND t2.id = " + cardId;

                        if (paymentChannel == Constants.TextBased.PaymentChannelPromptPay)
                        {
                            if (Configs.UsePaymentKsher)
                                sql += " AND t3.channel = 'PromptPay'";
                            else if (Configs.UsePaymentBeam)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'PromptPay'";
                            else if (Configs.UsePaymentBeam)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'PromptPay'";
                        }
                        else if (paymentChannel == Constants.TextBased.PaymentChannelCash)
                        {
                            if (Configs.UsePaymentKsher)
                                sql += " AND t3.channel is null AND t1.pay_type = 'C'";
                            else if (Configs.UsePaymentRabbit)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'เงินสด'";
                            else if (Configs.UsePaymentBeam)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'เงินสด'";
                        }
                        else if (paymentChannel == Constants.TextBased.PaymentChannelEDC)
                        {
                            if (Configs.UsePaymentKsher)
                                sql += " AND t3.channel is null AND t1.pay_type = 'EDC'";
                            else if (Configs.UsePaymentRabbit)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'EDC'";
                            else if (Configs.UsePaymentBeam)
                                sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'EDC'";
                        }
                        else if (paymentChannel == Constants.TextBased.PaymentChannelTrueMoney)
                            sql += " AND t3.channel = 'TrueMoney'";

                        sql += " ORDER BY t1.dateout";
                    }
                    break;

                case 163: // ธนภูมิ #36.การเข้าออกของรถยนต์แสดงช่องทางการชำระเงิน
                    string fontSlip163 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip163 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName)
                            fontSlip163 = "IV";
                    }

                    sql = "select cast(t2.no as char) as 'หมายเลขบัตร', t2.license as 'ทะเบียนรถ', date_format(t2.datein, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาเข้า', date_format(t1.dateout, '%d/%m/%Y %H:%i:%s') as 'วัน-เวลาออก',\n";
                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth)
                            sql += "concat(t1.receipt, concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี',\n";
                        else
                            sql += "concat(t1.receipt, concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี',\n";
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                            sql += "concat('" + fontSlip163 + "', concat(date_format(t1.dateout,'%y%m') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี',\n";
                        else
                            sql += "concat('" + fontSlip163 + "', concat(date_format(t1.dateout,'%y') ,lpad(t1.printno,6,'0'))) as 'ใบกำกับภาษี',\n";
                    }

                    sql += "format(t1.price, 2) as 'รายได้',\n";
                    if (Configs.UsePaymentKsher)
                    {
                        sql += "case when t3.channel = 'TrueMoney' then 'TrueMoney' when t3.channel = 'promptpay' then 'PromptPay' when t1.pay_type = 'EDC' then 'EDC' else 'เงินสด' end as 'ช่องทางการชำระเงิน',\n";
                        sql += "t3.ksher_order_no as 'ksher order no', t3.mch_order_no as 'mch order no' ";
                        sql += "from recordout t1 left join recordin t2 on t1.no = t2.no ";
                        sql += "left join (select max(t1.no), t1.no_recordin, t1.mch_order_no, t1.channel, t1.status, t2.ksher_order_no\n";
                        sql += "from ksherpay_post t1 left join ksherpay_get t2 on t1.mch_order_no = t2.mch_order_no where t1.status = 'Y' group by t1.no_recordin) t3 on t1.no = t3.no_recordin\n";
                    }
                    else if (Configs.UsePaymentBeam)
                    {
                        sql += "CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END AS 'ช่องทางการชำระเงิน',\n";
                        sql += "t3.qr as 'QR', t3.beam_id as 'Beam ID'\n";
                        sql += "from recordout t1 left join recordin t2 on t1.no = t2.no ";
                        sql += "left join (select max(t1.no), t1.no_recordin, t1.beam_id, t1.status, t2.qr\n";
                        sql += "from beam_post t1 left join beam_get t2 on t1.beam_id = t2.beam_id where t1.status = 'Y' group by t1.no_recordin) t3 on t1.no = t3.no_recordin\n";
                    }
                    else if (Configs.UsePaymentRabbit)
                    {
                        sql += "CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END AS 'ช่องทางการชำระเงิน',\n";
                        sql += "t3.qr as 'QR', t3.rabbit_id as 'rabbit ID'\n";
                        sql += "from recordout t1 left join recordin t2 on t1.no = t2.no ";
                        sql += "left join (select max(t1.no), t1.no_recordin, t1.rabbit_id, t1.status, t2.qr\n";
                        sql += "from rabbit_post t1 left join rabbit_get t2 on t1.rabbit_id = t2.rabbit_id where t1.status = 'Y' group by t1.no_recordin) t3 on t1.no = t3.no_recordin\n";
                    }

                    sql += "WHERE t1.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'\n";
                    sql += "AND t1.price > 0";

                    if (user != Constants.TextBased.All)
                        sql += " AND t1.userout = " + AppGlobalVariables.UsersById.First(kvp => kvp.Value == user).Key;
                    if (promotionName != Constants.TextBased.All)
                        sql += " AND t1.proid =" + promotionId;
                    if (!String.IsNullOrEmpty(licensePlate))
                        sql += " AND t2.license LIKE '%" + licensePlate + "%'";
                    if (!String.IsNullOrEmpty(cardId))
                        sql += " AND t2.id = " + cardId;

                    if (paymentChannel == Constants.TextBased.PaymentChannelPromptPay)
                    {
                        if (Configs.UsePaymentKsher)
                            sql += " AND t3.channel = 'PromptPay'";
                        else if (Configs.UsePaymentBeam)
                            sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'PromptPay'";
                        else if (Configs.UsePaymentBeam)
                            sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'PromptPay'";
                    }
                    else if (paymentChannel == Constants.TextBased.PaymentChannelCash)
                    {
                        if (Configs.UsePaymentKsher)
                            sql += " AND t3.channel is null AND t1.pay_type = 'C'";
                        else if (Configs.UsePaymentRabbit)
                            sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'เงินสด'";
                        else if (Configs.UsePaymentBeam)
                            sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'เงินสด'";
                    }
                    else if (paymentChannel == Constants.TextBased.PaymentChannelEDC)
                    {
                        if (Configs.UsePaymentKsher)
                            sql += " AND t3.channel is null AND t1.pay_type = 'EDC'";
                        else if (Configs.UsePaymentRabbit)
                            sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.rabbit_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'EDC'";
                        else if (Configs.UsePaymentBeam)
                            sql += " AND (CASE WHEN t3.qr IS NOT NULL AND t3.beam_id IS NOT NULL THEN 'PromptPay' WHEN t1.pay_type = 'EDC' THEN 'EDC' ELSE 'เงินสด' END) = 'EDC'";
                    }
                    else if (paymentChannel == Constants.TextBased.PaymentChannelTrueMoney)
                        sql += " AND t3.channel = 'TrueMoney'";

                    sql += " ORDER BY t1.dateout";
                    break;

                case 164: // ธนภูมิ #37.รายงานสรุปจำนวนรถและรายได้
                    sql = "SELECT recordin.no, recordin.cartype,recordout.price,recordout.discount, recordin.datein, recordout.dateout\n";
                    sql += "FROM recordin\n";
                    sql += "JOIN recordout ON recordin.no = recordout.no\n";
                    sql += $"WHERE dateout BETWEEN '{startDate.ToString("yyyy-MM-dd")}' AND '{endDate.AddDays(1).ToString("yyyy-MM-dd")}'";
                    break;

                case 165: // ธนภูมิ #38.สรุปจำนวนบัตรทั้งหมดตามบริษัท
                    sql = "SELECT m.id AS member_id, m.memgrouppriceid_pay AS ค่าบัตรสมาชิก, mgp.groupname AS บริษัท, mgp.id AS membergroupprice_month_id\n";
                    sql += "FROM member m\n";
                    sql += "LEFT JOIN membergroupprice_month mgp\n";
                    sql += "    ON mgp.id = m.memgrouppriceid_month\n";
                    sql += "WHERE 1 = 1 \n";

                    if (memberGroupMonth != Constants.TextBased.All)
                        sql += $"AND mgp.id = {memberGroupMonthId} \n";
                    if(isLegitPromotionRange)
                        sql += $"AND mgp.id BETWEEN {promotionRangeFrom} AND {promotionRangeTo} \n";

                    sql += "ORDER BY mgp.nogroup";
                    break;
                case 166: // ธนภูมิ #39.สรุปจำนวนบัตรทั้งหมดตามบริษัท
                    break;
                case 167: // ธนภูมิ #40.ค่าบริการจอดเรียกเก็บกับบริษัท รถยนต์-รายเดือน
                    break;
            }

            return sql;
        }

        #region HELPERS
        private string GetGenericReport()
        {
            string sql = "";
            if (Configs.UseMemberType)
            {
                sql = "SELECT recordout.no as ลำดับ, case when recordin.cartype = 200 then ifnull((SELECT typename FROM cartype WHERE typeid = member.typeid), 'Member') else "; //Mac 2022/03/01
                if (Configs.Reports.ReportCartypeFree15Min)
                {
                    sql += " case when TIMESTAMPDIFF(second,recordin.datein,recordout.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                    sql += " (SELECT typename FROM cartype WHERE typeid = recordin.cartype) end end AS ประเภท";
                }
                else
                {
                    sql += " (SELECT typename FROM cartype WHERE typeid = recordin.cartype) end AS ประเภท";
                }
            }
            else
            {
                sql = "SELECT recordout.no as ลำดับ, ";
                if (Configs.Reports.ReportCartypeFree15Min)
                {
                    sql += " case when (recordin.cartype != 200) and TIMESTAMPDIFF(second,recordin.datein,recordout.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                    sql += "  (SELECT typename FROM cartype WHERE typeid = recordin.cartype) end AS ประเภท  ";
                }
                else
                {
                    sql += " (SELECT typename FROM cartype WHERE typeid = recordin.cartype) AS ประเภท ";
                }
            }
            if (Configs.Reports.UseReport1_6)
                sql += " , case when recordin.license = 'NO' then recordin.id when recordin.license = '' then recordin.id else recordin.license end as ทะเบียน, cast(recordin.id as char) as เลขที่บัตร,"; //Mac 2024/07/25
            else if (Configs.Reports.UseReport1_8)
                sql += " , case when recordin.license = 'NO' then recordin.id when recordin.license = '' then recordin.id else recordin.license end as ทะเบียน, member.name as 'ผู้ถือบัตร', ";
            else
                sql += " , case when recordin.license = 'NO' then recordin.id when recordin.license = '' then recordin.id else recordin.license end as ทะเบียน,";

            sql += " recordin.no AS หมายเลขบัตร,";

            sql += "date_format(recordin.datein, '%d/%m/%Y %H:%i:%s') as เวลาเข้า";
            if (selectedReportId == 0 || selectedReportId == 90)
            {
                if (Configs.Reports.UseReport1_4)
                {
                    sql += ", date_format(recordin.datein_sub, '%d/%m/%Y %H:%i:%s') as 'เวลาเข้ารอง', recordin.no_car_down as 'จำนวนที่จอดว่าง'";
                }
            }
            sql += ",(select name from user where id = recordin.userin) as เจ้าหน้าที่ขาเข้า";
            if (selectedReportId == 0 || selectedReportId == 90)
            {
                if (Configs.Reports.UseReport1_4)
                    sql += ", date_format(recordin.dateout_sub, '%d/%m/%Y %H:%i:%s') as 'เวลาออกรอง'";
            }
            sql += ",date_format(recordout.dateout, '%d/%m/%Y %H:%i:%s') as เวลาออก,"; //Mac 2018/12/21
            if (selectedReportId == 5)
            {
                if (Configs.Reports.UseReport6)
                {
                    sql += "recordout.losscard as ค่าปรับ, (recordout.price - recordout.losscard) as ค่าจอด";
                }
                else
                {
                    if (Configs.Reports.ReportPriceSplitLosscard)
                        sql += "recordout.losscard as รายได้";
                    else
                        sql += "recordout.price as รายได้";

                    sql += ",recordout.discount as ส่วนลด"; //Mac 2019/05/03
                }
            }
            else
            {
                sql += " recordout.price as รายได้,recordout.discount as ส่วนลด"; //Mac 2019/05/03
            }

            sql += ",(select name from user where id = recordout.userout) as เจ้าหน้าที่ขาออก"; //Mac 2019/05/03
            if (Configs.IsVillage && Configs.Use2Camera)
            {
                if (Configs.UseMemberType)
                {
                    sql = "SELECT recordout.no as ลำดับ, case when recordin.cartype = 200 then ifnull((SELECT typename FROM cartype WHERE typeid = member.typeid), 'Member') else "; //Mac 2022/03/01
                    if (Configs.Reports.ReportCartypeFree15Min)
                    {
                        sql += " case when TIMESTAMPDIFF(second,recordin.datein,recordout.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                        sql += " (SELECT typename FROM cartype WHERE typeid = recordin.cartype) end end AS ประเภท";
                    }
                    else
                    {
                        sql += " (SELECT typename FROM cartype WHERE typeid = recordin.cartype) end AS ประเภท";
                    }
                }
                else
                {
                    sql = "SELECT recordout.no as ลำดับ, ";
                    if (Configs.Reports.ReportCartypeFree15Min) //Mac 2018/01/16
                    {
                        sql += " case when (recordin.cartype != 200) and TIMESTAMPDIFF(second,recordin.datein,recordout.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                        sql += "  (SELECT typename FROM cartype WHERE typeid = recordin.cartype) end AS ประเภท  ";
                    }
                    else
                    {
                        sql += " (SELECT typename FROM cartype WHERE typeid = recordin.cartype) AS ประเภท ";
                    }
                }
                sql += " , case when recordin.license = 'NO' then recordin.id when recordin.license = '' then recordin.id else recordin.license end as ทะเบียน,";

                sql += " IFNULL(recordin.visitor_name,'') as ชื่อผู้มาติดต่อ,IFNULL(recordin.type_card,'') as ประเภทบัตร, IFNULL(recordin.visitor_tel,'') as เบอร์โทรศัพท์, IFNULL(recordin.contact_name,'') as ติดต่อ, IFNULL(recordin.address,'') as ที่อยู่,";
                sql += " date_format(recordin.datein, '%d/%m/%Y %H:%i:%s') as เวลาเข้า,(select name from user where id = recordin.userin) as เจ้าหน้าที่ขาเข้า,date_format(recordout.dateout, '%d/%m/%Y %H:%i:%s') as เวลาออก,"; //Mac 2018/12/21
                sql += " recordout.price as รายได้,recordout.discount as ส่วนลด,(select name from user where id = recordout.userout) as เจ้าหน้าที่ขาออก";
            }

            if (Configs.NoPanelUp2U == "2")
            {
                sql = "select recordout.no as ลำดับ,(select typename from cartype where typeid = recordin.cartype) as ประเภท,recordin.license as ทะเบียน";
                sql += ",recordin.id as หมายเลขบัตร,(select memid from member_up2u where cardid = recordin.id) as เลขสมาชิก";
                sql += ",(select name from member_up2u where cardid = recordin.id) as ชื่อสมาชิก,(select date_format(dateexpire, '%d/%m/%Y %H:%i:%s') from member_up2u where cardid = recordin.id) as วันหมดอายุ"; //Mac 2018/12/21
                sql += ",date_format(recordin.datein, '%d/%m/%Y %H:%i:%s') as เวลาเข้า,(select name from user where id = recordin.userin) as เจ้าหน้าที่ขาเข้า,date_format(recordout.dateout, '%d/%m/%Y %H:%i:%s') as เวลาออก"; //Mac 2018/12/21
                sql += ",recordout.price as รายได้,recordout.discount as ส่วนลด,(select name from user where id = recordout.userout) as เจ้าหน้าที่ขาออก";
            }

            if (selectedReportId == 0 || selectedReportId == 90)
            {

                if (Configs.Reports.UseReport1_3)
                {
                    sql += ",concat(floor(timestampdiff(minute, date_format(recordin.datein, '%Y-%m-%d %H:%i:%s'), date_format(recordout.dateout, '%Y-%m-%d %H:%i:%s'))/60)";
                    sql += ", '.', lpad(mod(timestampdiff(minute, date_format(recordin.datein, '%Y-%m-%d %H:%i:%s'), date_format(recordout.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0')) as 'ชม.จอด'";
                }
                else if (Configs.Reports.UseReport1_5)
                    sql += ", recordin.contact as 'ติดต่อ'";
                else if (Configs.Reports.UseReport1_7)
                    sql += ", recordin.contact as 'ติดต่อ', (select (select groupname from membergroupprice_month where id = member.memgrouppriceid_month) from member where member.cardid = recordin.id) as บริษัท";

                if (Configs.IsVillage || Configs.VisitorFillDetail)
                    sql += ",recordin.address as ที่อยู่";
            }
            if (selectedReportId == 1 || selectedReportId == 91)
            {
                sql += ",recordin.piclic as il,recordout.piclic as ol";
                if (Configs.Use2Camera)
                    sql += ",recordin.picdiv as iv,recordout.picdiv as ov";
                if (Configs.IsVillage && Configs.Use2Camera) sql += ", recordin.picvis vi";
                else if (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0)
                    sql += ", recordin.picother AS io";

                if (Configs.Reports.UseReport2_4)
                {
                    sql += ", member.name as 'ผู้ถือบัตร'";
                    sql += ",concat(floor(timestampdiff(minute, date_format(recordin.datein, '%Y-%m-%d %H:%i:%s'), date_format(recordout.dateout, '%Y-%m-%d %H:%i:%s'))/60)";
                    sql += ", '.', lpad(mod(timestampdiff(minute, date_format(recordin.datein, '%Y-%m-%d %H:%i:%s'), date_format(recordout.dateout, '%Y-%m-%d %H:%i:%s')), 60), 2, '0')) as 'ชม.จอด'";
                }
            }
            if (selectedReportId == 9)
            {
                sql = "SELECT recordout.no,recordin.cartype, case when recordin.license = 'NO' then recordin.id when recordin.license = '' then recordin.id else recordin.license end,recordin.datein,recordin.userin,recordout.dateout,recordout.price,recordout.discount,recordout.userout";
                sql += ",recordout.userout,recordout.userout,recordout.userout,recordout.userout";
            }
            if (Configs.UseMemberLicensePlate) //Mac 2018/09/03
                sql += " from recordin left join recordout on recordin.no = recordout.no left join member on member.license like concat('%',recordin.license,'%')"; //Mac 2025/03/14
            else
                sql += " from recordin left join recordout on recordin.no = recordout.no left join member on recordin.id = member.cardid"; //Mac 2016/05/21

            sql += " left join cardmf t1 on recordin.id = t1.name";
            sql += " left join cardpx t2 on recordin.id = t2.name";

            string startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " " + startTime.ToLongTimeString();
            string endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " " + endTime.ToLongTimeString();// + " 23:59:59";
            sql += " WHERE dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                sql += " and recordin.notshow = 'N'";

            if (nameOnCard.Trim().Length > 0) //Mac 2022/03/01
                sql += " and (t1.name_on_card like '%" + nameOnCard + "%' or t2.name_on_card like '%" + nameOnCard + "%')";

            if (user != Constants.TextBased.All)
                sql += " AND recordout.userout =" + userId;

            if (Configs.Reports.ReportSearchMemGroup) //Mac 2021/03/11
            {
                if (memberType != Constants.TextBased.All)
                    sql += " and member.memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];

                if (carType == Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype != 200";
                if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype =" + carTypeId;
            }
            else if (Configs.Member2Cartype) //Mac 2016/05/03
            {
                if (memberType == Constants.TextBased.All)
                    if (memberType == Constants.TextBased.All)
                    {
                        if (carType != Constants.TextBased.All)
                        {
                            sql += " AND (recordin.cartype =" + carTypeId + " or member.typeid =" + carTypeId + ")";
                        }
                    }
                    else if (memberTypeSelectedIndex == 1)
                    {
                        sql += " AND recordin.cartype != 200";
                        if (carType != Constants.TextBased.All)
                        {
                            sql += " AND recordin.cartype =" + carTypeId;
                        }
                    }
                    else if (memberTypeSelectedIndex == 2)
                    {
                        sql += " AND recordin.cartype = 200";
                        if (carType != Constants.TextBased.All)
                        {
                            sql += " AND member.typeid =" + carTypeId;
                        }
                    }
                    else
                    {
                        //sql += " AND recordin.cartype = 200";
                        sql += " AND member.memgroupid =" + AppGlobalVariables.MemberGroupsToId[memberType];
                        if (carType != Constants.TextBased.All)
                        {
                            sql += " AND member.typeid =" + carTypeId;
                        }
                    }
            }
            else
            {
                if (carType == Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype != 200";
                if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype =" + carTypeId;
                if (carType == Constants.TextBased.All) //Mac 2015/02/10
                {
                    if (memberType != Constants.TextBased.All)
                        sql += " AND member.typeid =" + memberTypeId;
                }
            }
            if (licensePlate != "")
                sql += " AND recordin.license LIKE '%" + licensePlate + "%'";
            if (cardId != "")
                sql += " AND recordin.id = " + cardId;
            if (selectedReportId == 5)
                sql += " AND recordout.losscard > 0 ";
            if (Configs.UseSettingNewMember)
            {
                if (memberGroupMonth != Constants.TextBased.All)
                    sql += " and member.storeid = " + memberGroupMonthId;
            }
            else if ((selectedReportId == 0 || selectedReportId == 90) && Configs.Reports.UseReport1_7) //Mac 2020/10/26
            {
                if (memberGroupMonth != Constants.TextBased.All)
                    sql += " and member.memgrouppriceid_month = " + memberGroupMonthId;
            }
            if (guardhouse != String.Empty) //Mac 2019/11/14
                sql += " and recordout.guardhouse = '" + guardhouse + "' ";
            if (selectedReportId == 90 || selectedReportId == 91) //Mac 2020/10/26
                sql += " and recordin.no mod 2 = 1";

            sql += " GROUP BY recordout.no ORDER BY recordout.no";

            Console.WriteLine(sql);
            return sql;
        }

        private string GetReportGroupPriceData()
        {
            string sql = "SELECT recordout.no, recordin.license, recordin.datein, recordin.userin, recordout.dateout, recordout.userout, " +
                     "recordout.price, recordout.discount, recordout.userout, recordout.userout, recordout.userout, " +
                     "recordin.cartype, recordout.proid";

            if (Configs.UseMemberLicensePlate) // Match by license
            {
                sql += " FROM recordin LEFT JOIN recordout ON recordin.no = recordout.no " +
                       "LEFT JOIN member ON member.license LIKE CONCAT('%', recordin.license, '%')";
            }
            else // Match by cardid
            {
                sql += " FROM recordin LEFT JOIN recordout ON recordin.no = recordout.no " +
                       "LEFT JOIN member ON recordin.id = member.cardid";
            }

            // Date filtering
            string strdst = $"{startDate:yyyy-MM-dd} {startTime:HH:mm:ss}";
            string strdfn = $"{endDate:yyyy-MM-dd} {endTime:HH:mm:ss}";
            sql += $" WHERE dateout BETWEEN '{strdst}' AND '{strdfn}'";

            // User filter
            if (user != Constants.TextBased.All)
            {
                sql += $" AND recordout.userout = {userId}";
            }

            // Member group or type filter
            if (Configs.Reports.ReportSearchMemGroup)
            {
                if (memberType != Constants.TextBased.All)
                    sql += $" AND member.memgroupid = {memberTypeId}";

                if (carType == Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype != 200";

                if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql += $" AND recordin.cartype = {carTypeId}";
            }
            else if (Configs.Member2Cartype)
            {
                if (memberType == Constants.TextBased.All)
                {
                    if (carType != Constants.TextBased.All)
                        sql += $" AND (recordin.cartype = {carTypeId} OR member.typeid = {memberTypeId})";
                }
                else if (carType == Constants.TextBased.Visitor)
                {
                    sql += " AND recordin.cartype != 200";
                    if (carType != Constants.TextBased.All)
                        sql += $" AND recordin.cartype = {carTypeId}";
                }
                else if (memberType == Constants.TextBased.Member)
                {
                    sql += " AND recordin.cartype = 200";
                    if (carType != Constants.TextBased.All)
                        sql += $" AND member.typeid = {carTypeId}";
                }
                else
                {
                    sql += $" AND member.memgroupid = {AppGlobalVariables.MemberGroupsToId[carType]}";
                    if (carType != Constants.TextBased.All)
                        sql += $" AND member.typeid = {carTypeId}";
                }
            }
            else
            {
                if (carType == Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype != 200";

                if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql += $" AND recordin.cartype = {carType}";

                if (carType == Constants.TextBased.All && memberType != Constants.TextBased.All)
                    sql += $" AND member.typeid = {carTypeId}";
            }

            // License filter
            if (!string.IsNullOrWhiteSpace(licensePlate))
                sql += $" AND recordin.license LIKE '%{licensePlate}%'";

            // Card ID filter
            if (!string.IsNullOrWhiteSpace(cardId))
                sql += $" AND recordin.id = {cardId}";

            sql += " ORDER BY recordout.no";

            return sql;
        }

        public string GetPromotionUsage()
        {
            StringBuilder sqlBuilder = new StringBuilder();

            string receiptPrefix = string.IsNullOrEmpty(AppGlobalVariables.Printings.ReceiptName) ? (!Configs.UseReceiptName ? "IV" : "") : AppGlobalVariables.Printings.ReceiptName;

            sqlBuilder.AppendLine("SELECT recordin.no, recordin.license, recordin.datein, recordout.dateout,");
            sqlBuilder.AppendLine("    TRUNCATE(TIMESTAMPDIFF(MINUTE, recordin.datein, recordout.dateout), 0) AS tdf,");

            if (Configs.Reports.UseReport21_1 || Configs.Reports.UseReport21_2 || Configs.Reports.UseReport21_3)
            {
                sqlBuilder.AppendLine("    CONCAT(");
                sqlBuilder.AppendLine("        FLOOR(TIMESTAMPDIFF(MINUTE, recordin.datein, recordout.dateout) / 60), '.',");
                sqlBuilder.AppendLine("        LPAD(MOD(TIMESTAMPDIFF(MINUTE, recordin.datein, recordout.dateout), 60), 2, '0')");
                sqlBuilder.AppendLine("    ) AS hm,");
            }

            if (Configs.UseReceiptFor1Out)
            {
                string dateFormat = Configs.OutReceiptNameMonth ? "%y%m" : "%y";
                sqlBuilder.AppendLine($@"recordout.proid,
                                    CASE
                                        WHEN recordout.printno = 0 THEN '0'
                                        ELSE CONCAT(recordout.receipt, CONCAT(DATE_FORMAT(dateout, '{dateFormat}'), LPAD(recordout.printno, 6, '0')))
                                    END AS printno,");
            }
            else
            {
                string dateFormat = Configs.OutReceiptNameMonth ? "%y%m" : "%y";
                sqlBuilder.AppendLine($@"recordout.proid,
                                    CASE
                                        WHEN recordout.printno = 0 THEN '0'
                                        ELSE CONCAT('{receiptPrefix}', CONCAT(DATE_FORMAT(dateout, '{dateFormat}'), LPAD(recordout.printno, 6, '0')))
                                    END AS printno,");
            }

            sqlBuilder.AppendLine("    recordout.price,");

            if (Configs.Reports.UseReportThanapoom)
            {
                sqlBuilder.AppendLine("    CASE");
                sqlBuilder.AppendLine("        WHEN p.tnpt_id IS NULL OR p.tnpt_id = '' THEN 0");
                sqlBuilder.AppendLine("        WHEN p.tnpt_id REGEXP '^-?[0-9]+$' THEN CAST(p.tnpt_id AS UNSIGNED)");
                sqlBuilder.AppendLine("        ELSE 0");
                sqlBuilder.AppendLine("    END AS tnpt_id_int,");

                sqlBuilder.AppendLine("    recordout.losscard,");
                sqlBuilder.AppendLine("    recordout.overdate,");
                sqlBuilder.AppendLine("recordin.cartype");
                sqlBuilder.AppendLine("FROM recordin");
                sqlBuilder.AppendLine("JOIN recordout ON recordin.no = recordout.no");
                sqlBuilder.AppendLine("LEFT JOIN promotion p ON p.id = recordout.proid");
            }
            else
            {
                sqlBuilder.AppendLine("recordin.cartype");
                sqlBuilder.AppendLine("FROM recordin");
                sqlBuilder.AppendLine("JOIN recordout ON recordin.no = recordout.no");
            }

            sqlBuilder.AppendLine($"WHERE dateout BETWEEN '{startDateTimeText:yyyy-MM-dd HH:mm:ss}' AND '{endDateTimeText:yyyy-MM-dd HH:mm:ss}' AND");

            if (memberGroupMonth != Constants.TextBased.All && !String.IsNullOrEmpty(memberGroupMonth))
            {
                int groupId = memberGroupMonthId;
                string promoSql = $"SELECT id FROM promotion WHERE groupro = {groupId}";
                DataTable promotionIds = DbController.LoadData(promoSql);

                if (promotionIds?.Rows.Count > 0)
                {
                    if (promotionIds.Rows.Count == 1)
                    {
                        sqlBuilder.AppendLine($"recordout.proid = {promotionIds.Rows[0]["id"]}");
                    }
                    else
                    {
                        StringBuilder proidConditions = new StringBuilder();
                        proidConditions.Append("(");
                        for (int i = 0; i < promotionIds.Rows.Count; i++)
                        {
                            if (i > 0)
                                proidConditions.Append(" OR ");

                            proidConditions.Append($"recordout.proid = {promotionIds.Rows[i]["id"]}");
                        }
                        proidConditions.Append(")");

                        sqlBuilder.AppendLine(proidConditions.ToString());
                    }

                    sqlBuilder.AppendLine("ORDER BY recordout.proid, recordout.dateout");
                }
            }

            if (isLegitPromotionRange)
            {
                if (Configs.Reports.UseReportThanapoom)
                {
                        sqlBuilder.AppendLine("   (");
                        sqlBuilder.AppendLine("      CASE");
                        sqlBuilder.AppendLine("          WHEN p.tnpt_id IS NULL OR p.tnpt_id = '' THEN 0");
                        sqlBuilder.AppendLine("          WHEN p.tnpt_id REGEXP '^-?[0-9]+$' THEN CAST(p.tnpt_id AS UNSIGNED)");
                        sqlBuilder.AppendLine("          ELSE 0");
                        sqlBuilder.AppendLine("      END");
                        sqlBuilder.AppendLine($"  ) BETWEEN {promotionRangeFrom} AND {promotionRangeTo}");
                }
                else
                {
                    sqlBuilder.AppendLine($"recordout.proid BETWEEN {promotionRangeFrom} AND {promotionRangeTo}");
                }
            }
            else if (promotionName != Constants.TextBased.All)
            {
                sqlBuilder.AppendLine($"recordout.proid = {promotionId}");
            }
            else
            {
                sqlBuilder.AppendLine("recordout.proid > 0 AND recordout.proid < 9999");
            }

            if (Configs.Reports.UseReportThanapoom)
            {
                sqlBuilder.AppendLine("ORDER BY tnpt_id_int, recordout.proid, recordout.dateout");
            }
            else
            {
                sqlBuilder.AppendLine("ORDER BY recordout.proid, recordout.dateout");
            }

            return sqlBuilder.ToString();
        }

        private string AllCarIn()
        {
            var sql = new StringBuilder();

            if (Configs.NoPanelUp2U == "2")
            {
                sql.AppendLine("SELECT recordin.no AS ลำดับ,");
                sql.AppendLine("  (SELECT typename FROM cartype WHERE typeid = recordin.cartype) AS ประเภท,");
                sql.AppendLine("  recordin.license AS ทะเบียน,");
                sql.AppendLine("  recordin.id AS หมายเลขบัตร,");
                sql.AppendLine("  (SELECT memid FROM member_up2u WHERE cardid = recordin.id) AS เลขสมาชิก,");
                sql.AppendLine("  (SELECT name FROM member_up2u WHERE cardid = recordin.id) AS ชื่อสมาชิก,");
                sql.AppendLine("  (SELECT DATE_FORMAT(dateexpire, '%d/%m/%Y %H:%i:%s') FROM member_up2u WHERE cardid = recordin.id) AS วันหมดอายุ,");
                sql.AppendLine("  DATE_FORMAT(recordin.datein, '%d/%m/%Y %H:%i:%s') AS เวลาเข้า,");
                sql.AppendLine("  (SELECT name FROM user WHERE id = recordin.userin) AS เจ้าหน้าที่ขาเข้า,");
                sql.AppendLine("  DATE_FORMAT(recordout.dateout, '%d/%m/%Y %H:%i:%s') AS เวลาออก,");
                sql.AppendLine("  recordout.price AS รายได้,");
                sql.AppendLine("  recordout.discount AS ส่วนลด,");
                sql.AppendLine("  (SELECT name FROM user WHERE id = recordout.userout) AS เจ้าหน้าที่ขาออก");
            }
            else
            {
                sql.AppendLine("SELECT recordin.no AS ลำดับ,");

                if (Configs.UseMemberType)
                {
                    sql.AppendLine("  CASE WHEN recordin.cartype = 200 THEN");
                    sql.AppendLine("    (SELECT typename FROM cartype WHERE typeid = member.typeid)");
                    sql.AppendLine("  ELSE");
                    sql.AppendLine("    (SELECT typename FROM cartype WHERE typeid = recordin.cartype)");
                    sql.AppendLine("  END AS ประเภท,");
                }
                else
                {
                    sql.AppendLine("  (SELECT typename FROM cartype WHERE typeid = recordin.cartype) AS ประเภท,");
                }

                sql.AppendLine("  CASE");
                sql.AppendLine("    WHEN recordin.license = 'NO' THEN recordin.id");
                sql.AppendLine("    WHEN recordin.license = '' THEN recordin.id");
                sql.AppendLine("    ELSE recordin.license");
                sql.AppendLine("  END AS ทะเบียน,");

                if (Configs.VillageState && Configs.Use2Camera)
                {
                    sql.AppendLine("  IFNULL(recordin.visitor_name, '') AS ชื่อผู้มาติดต่อ,");
                    sql.AppendLine("  IFNULL(recordin.type_card, '') AS ประเภทบัตร,");
                    sql.AppendLine("  IFNULL(recordin.visitor_tel, '') AS เบอร์โทรศัพท์,");
                    sql.AppendLine("  IFNULL(recordin.contact_name, '') AS ติดต่อ,");
                    sql.AppendLine("  IFNULL(recordin.address, '') AS ที่อยู่,");
                }

                sql.AppendLine("  DATE_FORMAT(recordin.datein, '%d/%m/%Y %H:%i:%s') AS เวลาเข้า,");
                sql.AppendLine("  (SELECT name FROM user WHERE id = recordin.userin) AS เจ้าหน้าที่ขาเข้า,");
                sql.AppendLine("  DATE_FORMAT(recordout.dateout, '%d/%m/%Y %H:%i:%s') AS เวลาออก,");
                sql.AppendLine("  recordout.price AS รายได้,");
                sql.AppendLine("  recordout.discount AS ส่วนลด,");
                sql.AppendLine("  (SELECT name FROM user WHERE id = recordout.userout) AS เจ้าหน้าที่ขาออก");

                if (Configs.IsVillage || Configs.UseVisitorDetail)
                {
                    sql.AppendLine("  ,recordin.address AS ที่อยู่");
                }
            }

            sql.AppendLine("FROM recordin");
            sql.AppendLine("LEFT JOIN recordout ON recordin.no = recordout.no");

            if (Configs.UseMemLicense)
            {
                sql.AppendLine("LEFT JOIN member ON member.license LIKE CONCAT('%', recordin.license, '%')");
            }
            else
            {
                sql.AppendLine("LEFT JOIN member ON recordin.id = member.cardid");
            }

            string strStart = $"{startDate:yyyy-MM-dd} {startTime:HH:mm:ss}";
            string strEnd = $"{endDate:yyyy-MM-dd} {endTime:HH:mm:ss}";
            sql.AppendLine($"WHERE datein BETWEEN '{strStart}' AND '{strEnd}'");

            if (user != Constants.TextBased.All)
            {
                sql.AppendLine($"AND recordin.userin = {userId}");
            }

            if (Configs.Reports.ReportSearchMemGroup)
            {
                if (memberType != Constants.TextBased.All)
                    sql.AppendLine($"AND member.memgroupid = {AppGlobalVariables.MemberGroupsToId[memberType]}");

                if (carType == Constants.TextBased.Visitor)
                    sql.AppendLine("AND recordin.cartype != 200");
                else if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql.AppendLine($"AND recordin.cartype = {carTypeId}");
            }
            else if (Configs.Member2Cartype)
            {
                switch (memberTypeSelectedIndex)
                {
                    case 0:
                        if (carType != Constants.TextBased.All)
                            sql.AppendLine($"AND (recordin.cartype = {carTypeId} OR member.typeid = {carTypeId})");
                        break;
                    case 2:
                        sql.AppendLine("AND recordin.cartype = 200");
                        if (carType != Constants.TextBased.All)
                            sql.AppendLine($"AND member.typeid = {carTypeId}");
                        break;
                    default:
                        sql.AppendLine($"AND member.memgroupid = {AppGlobalVariables.MemberGroupsToId[memberType]}");
                        if (carType != Constants.TextBased.All)
                            sql.AppendLine($"AND member.typeid = {carTypeId}");
                        break;
                }
            }
            else
            {
                if (carType == Constants.TextBased.Visitor)
                    sql.AppendLine("AND recordin.cartype != 200");
                else if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql.AppendLine($"AND recordin.cartype = {carTypeId}");

                if (carType == Constants.TextBased.All && memberType != Constants.TextBased.All)
                    sql.AppendLine($"AND member.typeid = {memberTypeId}");
            }

            if (!string.IsNullOrWhiteSpace(licensePlate))
            {
                sql.AppendLine($"AND recordin.license LIKE '%{licensePlate}%'");
            }

            if (!string.IsNullOrWhiteSpace(cardId))
            {
                sql.AppendLine($"AND recordin.id = {cardId}");
            }

            sql.AppendLine("ORDER BY recordin.no");

            return sql.ToString();
        }

        private string GetCarIn()
        {
            string startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " " + startTime.ToLongTimeString();
            string endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " " + endTime.ToLongTimeString();

            string sql = "SELECT recordin.no as ลำดับ,";
            if (Configs.UseMemberType)
                sql += " case when recordin.cartype = 200 then ifnull((SELECT typename FROM cartype WHERE typeid = member.typeid), 'Member') else (SELECT typename FROM cartype WHERE typeid = recordin.cartype) end AS ประเภท,"; //Mac 2022/03/02

            else
                sql += "(select typename from cartype where typeid = recordin.cartype) as ประเภท,";

            if (Configs.IsVillage) //Mac 2024/12/18
                sql += "recordin.address as 'ที่อยู่',";
            else if (Configs.Reports.UseReport5_1) //Mac 2017/06/19
                sql += "member.address as 'ที่อยู่',";

            if (Configs.UseNameOnCard) //Mac 2018/12/13
                sql += "recordin.license as ทะเบียน, IFNULL(t1.name_on_card,t2.name_on_card) as 'ชื่อบัตร',";

            else if (Configs.Reports.UseReport5_4) //Mac 2018/11/12
                sql += "recordin.license as ทะเบียน, cast(recordin.id as char) as เลขที่บัตร,";
            else
                sql += "case when recordin.license = 'NO' then recordin.id when recordin.license = '' then recordin.id else recordin.license end as ทะเบียน,";

            if (Configs.NoPanelUp2U == "2") //Mac 2017/03/13
            {
                sql = "select recordin.no as ลำดับ,(select typename from cartype where typeid = recordin.cartype) as ประเภท,recordin.license as ทะเบียน";
                sql += ",recordin.id as หมายเลขบัตร,(select memid from member_up2u where cardid = recordin.id) as เลขสมาชิก";
                sql += ",(select name from member_up2u where cardid = recordin.id) as ชื่อสมาชิก,(select date_format(dateexpire, '%d/%m/%Y %H:%i:%s') from member_up2u where cardid = recordin.id) as วันหมดอายุ,"; //Mac 2018/12/21
            }
            sql += "date_format(recordin.datein, '%d/%m/%Y %H:%i:%s') as เวลาเข้า"; //Mac 2018/12/21
            if (Configs.Reports.UseReport5_2) //Mac 2018/02/23
                sql += ", date_format(recordin.datein_sub, '%d/%m/%Y %H:%i:%s') as 'เวลาเข้ารอง', recordin.no_car_down as 'จำนวนที่จอดว่าง', date_format(recordin.dateout_sub, '%d/%m/%Y %H:%i:%s') as 'เวลาออกรอง'"; //Mac 2018/12/21

            sql += ",(select name from user where id = recordin.userin) as เจ้าหน้าที่ขาเข้า";
            if (Configs.UseAsciiMember) //Mac 2016/07/11
                sql += ",cast(CONCAT(CHAR(left(recordin.id,2)),mid(recordin.id,3)) as char) as รหัสบัตร";
            if (Configs.Reports.UseReport5_3) //Mac 2018/02/28
                sql += ", recordin.contact as 'ติดต่อ'";

            sql += " FROM recordin recordin left join recordout ON recordin.no = recordout.no";

            if (Configs.UseMemberLicensePlate) //Mac 2018/09/03
                sql += " LEFT JOIN member ON member.license like concat('%',recordin.license,'%')"; //Mac 2025/03/14
            else
                sql += " LEFT JOIN member ON recordin.id = member.cardid";

            sql += " left join cardmf t1 on recordin.id = t1.name";
            sql += " left join cardpx t2 on recordin.id = t2.name";

            sql += " WHERE datein BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
            sql += " AND recordout.no IS NULL";

            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                sql += " and recordin.notshow = 'N'";

            if (nameOnCard.Trim().Length > 0) //Mac 2022/03/01
                sql += " and (t1.name_on_card like '%" + nameOnCard + "%' or t2.name_on_card like '%" + nameOnCard + "%')";

            if (user != Constants.TextBased.All)
            {
                sql += " AND recordin.userin =" + userId; //Mac 2017/12/06
            }

            if (Configs.Reports.ReportSearchMemGroup) //Mac 2021/03/11
            {
                if (memberType != Constants.TextBased.All)
                    sql += " and member.memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];
                if (carType == Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype != 200";
                if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype =" + carTypeId;
            }
            else if (Configs.Member2Cartype) //Mac 2016/05/03
            {
                if (memberType == Constants.TextBased.All)
                {
                    if (carType != Constants.TextBased.All)
                    {
                        sql += " AND (recordin.cartype =" + carTypeId + " or member.typeid =" + carTypeId + ")";
                    }
                }
                else if (memberTypeSelectedIndex == 1)
                {
                    sql += " AND recordin.cartype != 200";
                    if (carType != Constants.TextBased.All)
                    {
                        sql += " AND recordin.cartype =" + carTypeId;
                    }
                }
                else if (memberTypeSelectedIndex == 2)
                {
                    sql += " AND recordin.cartype = 200";
                    if (carType != Constants.TextBased.All)
                    {
                        sql += " AND member.typeid =" + carTypeId;
                    }
                }
                else
                {
                    sql += " AND member.memgroupid =" + AppGlobalVariables.MemberGroupsToId[memberType];
                    if (carType != Constants.TextBased.All)
                        sql += " AND member.typeid =" + carTypeId;
                }
            }
            else
            {
                if (carType == Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype != 200";
                if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype =" + carTypeId;
                if (carType == Constants.TextBased.All) //Mac 2015/02/10
                {
                    if (memberType != Constants.TextBased.All)
                        sql += " AND member.typeid =" + memberTypeId;
                }
            }

            if (licensePlate != "")
                sql += " AND recordin.license LIKE '%" + licensePlate + "%'";
            if (cardId != "")
                sql += " AND recordin.id = " + cardId;
            if (guardhouse != String.Empty) //Mac 2019/11/14
                sql += " and recordin.guardhouse = '" + guardhouse + "' ";

            if (selectedReportId == 92) //Mac 2020/10/26
                sql += " and recordin.no mod 2 = 1";

            if (Configs.UseSettingNewMember && memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/03
                sql += " and member.storeid = " + memberGroupMonthId;

            sql += " ORDER BY recordin.no";
            return sql;
        }

        private string GetReportDistinct()
        {
            string sql = "SELECT DISTINCT license FROM recordin";

            string startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " " + startTime.ToLongTimeString();
            string endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " " + endTime.ToLongTimeString();

            sql += " WHERE datein BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";
            if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
            {
                sql += " AND recordin.cartype =" + carTypeId;
            }
            if (licensePlate != "")
                sql += " AND recordin.license LIKE '%" + licensePlate + "%'";
            if (cardId != "")
                sql += " AND recordin.id = " + cardId;
            sql += " ORDER BY license";
            return sql;
        }

        private string PricePromotion()
        {
            try
            {
                Configs.UseMemo = Convert.ToBoolean(AppGlobalVariables.ParamsLookup["use_memo"]);
            }
            catch
            {
                Configs.UseMemo = false;
            }
            string sql = "SELECT DISTINCT ";
            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                sql += " recordout.printno_second";
            else
                sql += " recordout.printno";

            sql += " ,recordout.no, ";
            if (Configs.UseMemberType) //Mac 2018/01/16
            {
                sql += " case when recordin.cartype = 200 then ifnull(member.typeid, '200') else "; //Mac 2022/03/02
                if (Configs.Reports.ReportCartypeFree15Min) //Mac 2018/01/16
                {
                    sql += " case when TIMESTAMPDIFF(second,recordin.datein,recordout.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                    sql += " recordin.cartype end end";
                }
                else
                {
                    sql += " recordin.cartype end";
                }
            }
            else
            {
                if (Configs.Reports.ReportCartypeFree15Min) //Mac 2018/01/16
                {
                    sql += " case when (recordin.cartype != 200) and TIMESTAMPDIFF(second,recordin.datein,recordout.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                    sql += "  recordin.cartype end";
                }
                else
                {
                    sql += " recordin.cartype ";
                }
            }
            sql += " ,case when recordin.license = 'NO' then recordin.id when recordin.license = '' then recordin.id else recordin.license end,recordin.datein,recordout.userout,recordout.dateout,recordout.proid,recordout.discount,recordout.userout,recordout.userout,recordout.losscard";

            if (selectedReportId == 12 && Configs.Reports.UseReport13_11)
                sql += " ,recordout.clearcard";
            else
                sql += " ,recordout.overdate";

            if (Configs.UseProIDAll)
            {
                if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2018/05/13
                    sql += " ,(recordout.price - recordout.losscard)";
                else
                    sql += " ,recordout.price";
                sql += " ,recordout.discount,recordout.proid_all";
            }
            else
            {
                if (Configs.Reports.ReportPriceSplitLosscard) //Mac 2018/05/13
                    sql += " ,(recordout.price - recordout.losscard)";
                else
                    sql += " ,recordout.price";
                sql += " ,recordout.discount,recordout.proid";
            }

            if (Configs.UseMemo)
            {
                sql += " ,recordin.memo";
            }

            if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                sql += ", recordout.receipt";

            if (Configs.UseMemberLicensePlate) //Mac 2018/09/03
                sql += " from recordin left join recordout on recordin.no = recordout.no left join member on member.license like concat('%',recordin.license,'%')"; //Mac 2025/03/14
            else
                sql += " from recordin left join recordout on recordin.no = recordout.no left join member on recordin.id = member.cardid"; //Mac 2016/05/21

            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) //Mac 2018/02/23
            {
                sql = "SELECT DISTINCT ";
                if (Configs.Reports.UseReport13_12) //Mac 2023/08/09
                {
                    sql += " recordout.posid, ";
                }
                if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                    sql += " recordout.printno_second";
                else
                    sql += " recordout.printno";

                sql += " ,recordout.no,";
                if (Configs.UseMemberType) //Mac 2018/01/16
                {
                    sql += " case when recordin.cartype = 200 then ifnull(member.typeid, '200') else "; //Mac 2022/03/02
                    if (Configs.Reports.ReportCartypeFree15Min) //Mac 2018/01/16
                    {
                        sql += " case when TIMESTAMPDIFF(second,recordin.datein,recordout.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                        sql += " recordin.cartype end end";
                    }
                    else
                    {
                        sql += " recordin.cartype end";
                    }
                }
                else
                {
                    if (Configs.Reports.ReportCartypeFree15Min)
                    {
                        sql += " case when (recordin.cartype != 200) and TIMESTAMPDIFF(second,recordin.datein,recordout.dateout) <= 959 then 'ฟรี 15 นาที' else ";
                        sql += "  recordin.cartype end";
                    }
                    else
                    {
                        sql += " recordin.cartype ";
                    }
                }
                sql += " ,recordin.license,recordin.datein,recordout.userout,recordout.dateout,recordout.proid,recordout.discount,recordout.userout,recordout.userout,recordout.losscard,recordout.overdate,recordout.price,recordout.price"; //Mac 2018/05/13
                if (Configs.Reports.ReportPriceSplitLosscard)
                    sql += " ,(recordout.price - recordout.losscard)";
                else
                    sql += " ,recordout.price";

                if (Configs.UseProIDAll)
                {
                    sql += " ,recordout.proid_all";
                }
                else
                    sql += " ,recordout.proid";
                if (Configs.Reports.UseReport13_3)
                    sql += " ,recordoutvoidpay.price";

                if (Configs.UseReceiptFor1Out)
                    sql += ", recordout.receipt";

                if (Configs.UseMemberLicensePlate)
                    sql += " from recordin left join recordout on recordin.no = recordout.no left join member on member.license like concat('%',recordin.license,'%')";
                else
                    sql += " from recordin left join recordout on recordin.no = recordout.no left join member on recordin.id = member.cardid";

                if (Configs.Reports.UseReport13_3)
                    sql += " left join recordoutvoidpay on recordout.no = recordoutvoidpay.no";
            }

            sql += " left join cardmf t1 on recordin.id = t1.name";
            sql += " left join cardpx t2 on recordin.id = t2.name";

            string startDateTimeText = startDate.Year.ToString() + "-" + startDate.ToString("MM'-'dd") + " " + startTime.ToLongTimeString();
            string endDateTimeText = endDate.Year.ToString() + "-" + endDate.ToString("MM'-'dd") + " " + endTime.ToLongTimeString();

            sql += " WHERE recordout.dateout BETWEEN '" + startDateTimeText + "' AND '" + endDateTimeText + "'";

            if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                sql += " and recordin.notshow = 'N'";

            if (nameOnCard.Trim().Length > 0)
                sql += " and (t1.name_on_card like '%" + nameOnCard + "%' or t2.name_on_card like '%" + nameOnCard + "%')";

            if (selectedReportId == 12 && Configs.Reports.Report13Pro_SwitchPriceNot0)
            {
                if (MessageBox.Show("ต้องการรายงานเข้าออกของรถแสดงโปรโมชั่น แบบค่าจอดมากกว่า 0 ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    sql += " AND recordout.price > 0 ";
                }
            }

            if (user != Constants.TextBased.All)
            {
                sql += " AND recordout.userout =" + userId;
            }

            if (Configs.Reports.ReportSearchMemGroup) //Mac 2021/03/11
            {
                if (memberType != Constants.TextBased.All)
                    sql += " and member.memgroupid = " + AppGlobalVariables.MemberGroupsToId[memberType];
                if (carType == Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype != 200";
                if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype =" + carTypeId;
            }
            else if (Configs.Member2Cartype) //Mac 2016/05/03
            {
                if (memberType == Constants.TextBased.All)
                {
                    if (carType != Constants.TextBased.All)
                    {
                        sql += " AND (recordin.cartype =" + carTypeId + " or member.typeid =" + carTypeId + ")";
                    }
                }
                else if (memberTypeSelectedIndex == 1)
                {
                    sql += " AND recordin.cartype != 200";
                    if (carType != Constants.TextBased.All)
                    {
                        sql += " AND recordin.cartype =" + carTypeId;
                    }
                }
                else if (memberTypeSelectedIndex == 2)
                {
                    sql += " AND recordin.cartype = 200";
                    if (carType != Constants.TextBased.All)
                    {
                        sql += " AND member.typeid =" + carTypeId;
                    }
                }
                else
                {
                    sql += " AND member.memgroupid =" + AppGlobalVariables.MemberGroupsToId[memberType];
                    if (carType != Constants.TextBased.All)
                    {
                        sql += " AND member.typeid =" + carTypeId;
                    }
                }
            }
            else
            {
                if (carType == Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype != 200";
                if (carType != Constants.TextBased.All && carType != Constants.TextBased.Visitor)
                    sql += " AND recordin.cartype =" + carTypeId;
                if (carType == Constants.TextBased.All) //Mac 2015/02/10
                {
                    if (memberType != Constants.TextBased.All)
                        sql += " AND member.typeid =" + memberTypeId;
                }
            }

            if (promotionName != Constants.TextBased.All)
            {
                if (Configs.UseProIDAll) //Mac 2015/10/06
                {
                    sql += " AND recordout.proid_all like '%" + promotionId + ",%'";
                }
                else
                {
                    sql += " AND recordout.proid =" + promotionId;
                }
            }
            if (Configs.UseTax && (!Configs.UseAsiaTriqPrice)) //Mac 2016/10/04
            {
                if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                    sql += " AND recordout.printno_second > 0";
                else
                    sql += " AND recordout.printno > 0";
            }
            if (licensePlate != "")
                sql += " AND recordin.license LIKE '%" + licensePlate + "%'";
            if (cardId != "")
                sql += " AND recordin.id = " + cardId;
            if (guardhouse != String.Empty) //Mac 2019/11/14
                sql += " and recordout.guardhouse = '" + guardhouse + "' ";

            if (Configs.UseSettingNewMember && memberGroupMonth != Constants.TextBased.All) //Mac 2021/08/03
                sql += " and member.storeid = " + memberGroupMonthId;

            if (paymentStatus != Constants.TextBased.All)
            {
                if (paymentStatus == Constants.TextBased.PaymentStatusPaid)
                {
                    sql += " and recordout.price > 0";
                }
                else if (paymentStatus == Constants.TextBased.PaymentStatusUnPaid)
                {
                    sql += " and recordout.price = 0";
                }
            }

            if (selectedReportId == 13)
            {
                if (Configs.UseVoidSlip)
                    sql += " AND recordout.status = 'N'";

                if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                {
                    if (Configs.OutReceiptNameMonth)
                    {
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " order by recordout.receipt, concat(concat(date_format(dateout,'%y%m') ,lpad(printno_second,6,'0')))";
                        else
                            sql += " order by recordout.receipt, concat(concat(date_format(dateout,'%y%m') ,lpad(printno,6,'0')))"; //Mac 2022/04/26
                    }
                    else
                    {
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " ORDER BY recordout.receipt, recordout.printno_second";
                        else
                            sql += " ORDER BY recordout.receipt, recordout.printno";
                    }
                }
                else
                {
                    if (Configs.OutReceiptNameMonth) //Mac 2016/04/27
                    {
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " order by concat(concat(date_format(dateout,'%y%m') ,lpad(printno_second,6,'0')))";
                        else
                            sql += " order by concat(concat(date_format(dateout,'%y%m') ,lpad(printno,6,'0')))"; //Mac 2022/04/26
                    }
                    else
                    {
                        if (Configs.NotShowNoString.Trim().Length > 0 && AppGlobalVariables.OperatingUser.Level == 0) //Mac 2022/04/22
                            sql += " ORDER BY recordout.printno_second";
                        else
                            sql += " ORDER BY recordout.printno";
                    }
                }
            }
            else
                sql += " GROUP BY recordout.no ORDER BY recordout.dateout";

            return sql;
        }

        private bool CheckAndUpdatePromotionRange(string promotionRangeFrom, string promotionRangeTo)
        {
            if (int.TryParse(promotionRangeFrom, out int from))
                this.promotionRangeFrom = from;
            else
                this.promotionRangeFrom = 0;
            if (int.TryParse(promotionRangeTo, out int to))
                this.promotionRangeTo = to;
            else
                this.promotionRangeTo = 0;

            bool isPromotionRangeEmpty = (this.promotionRangeFrom == 0) || (this.promotionRangeTo == 0);
            bool isLegitPromotionRange = !isPromotionRangeEmpty && (this.promotionRangeFrom <= this.promotionRangeTo);

            return isLegitPromotionRange;
        }
        private void SaveToTextFile(DataTable dt, string filename)
        {
            string stOutput = "";
            // Export data.
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string stLine = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j == (dt.Columns.Count - 1))
                        stLine = stLine.ToString() + Convert.ToString(dt.Rows[i].ItemArray[j].ToString());
                    else
                        stLine = stLine.ToString() + Convert.ToString(dt.Rows[i].ItemArray[j].ToString()) + ",";
                }
                stOutput += stLine + "\r\n";
            }
            //Encoding utf8 = Encoding.GetEncoding(1254);
            Encoding utf8 = Encoding.GetEncoding("TIS-620");
            byte[] output = utf8.GetBytes(stOutput);

            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
            MessageBox.Show("Text file Save Complete!");
        }
        #endregion
    }
}