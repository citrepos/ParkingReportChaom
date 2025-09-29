using System;

namespace ParkingManagementReport.Common
{
    internal class Enumerators
    {
        #region REPORTS
        public enum ReportDesignator
        {                                             // id |                   - designator -               | name
            VehicleHistory,                           // 1  | vehicle_history                                | การเข้าออกของรถ
            VehicleHistoryWithImage,                  // 2  | vehicle_history_with_image                     | การเข้าออกแบบแสดงรูปภาพ
            OfficerOperation,                         // 3  | officer_operation                              | การทำงานของเจ้าหน้าที่
            BarrierLift,                              // 4  | barrier_lift                                   | การยกไม้
            OutstandingVehicle,                       // 5  | outstanding_vehicle                            | รถคงค้าง
            LossCard,                                 // 6  | loss_card                                      | บัตรหาย
            VehicleHistoryStatistic,                  // 7  | vehicle_history_statistic                      | สถิติการเข้าออกรถ
            BarrierLiftWithImage,                     // 8  | barrier_lift_with_image                        | การยกไม้แบบแสดงรูปภาพ
            PromotionList,                            // 9  | promotion_list                                 | โปรโมชั่น
            RevenueWithVatSeparation,                 // 10 | revenue_with_vat_separation                    | รายได้แยกภาษี
            RevenueFromMember,                        // 11 | revenue_from_member                            | รายได้ของ Member
            RevenueDividedByGroup,                    // 12 | revenue_divided_by_group                       | รายได้แบบแยกกลุ่ม
            VehicleHistoryWithPromotion,              // 13 | vehicle_history_with_promotion                 | การเข้าออกของรถแสดงโปรโมชั่น
            RevenueWithTaxInvoice,                    // 14 | revenue_with_tax_invoice                       | รายได้ค่าจอดรถตามเลขที่ใบเสร็จ/ใบกำกับภาษี
            VehicleHistoryWithPromotionPerOfficer,    // 15 | vehicle_history_with_promotion_per_officer     | การเข้าออกของรถแสดงโปรโมชั่นตามการทำงานของเจ้าหน้าที่
            PromotionSummary,                         // 16 | promotion_summary                              | ยอดรวม E-Stamp
            VehicleHistorySummary,                    // 17 | vehicle_history_summary                        | รายงานสรุปการเข้า-ออก
            VehicleHistoryDaily,                      // 18 | vehicle_history_daily                          | รายงานการเข้า-ออกประจำวัน
            VehicleHistoryStatisticByParking,         // 19 | vehicle_history_statistic_by_parking           | รายงานสถิติการเข้าออกที่จอดรถ   
            PromotionAmountSummary,                   // 20 | promotion_amount_summary                       | รายงานสรุปจำนวนตราประทับ
            EnumeratedPromotion,                      // 21 | enumerated_promotion                           | รายงานจำนวนตราประทับ แบบแจกแจง
            DailyEnumeratedPromotion,                 // 22 | daily_enumerated_promotion                     | รายงานรายวันจำนวนตราประทับ แบบแจกแจง
            MemberList,                               // 23 | member_list                                    | รายชื่อสมาชิก                  
            MemberRevenue,                            // 24 | member_revenue                                 | รายได้จากสมาชิก                
            VehicleHistoryTimeRangeDailySummary,      // 25 | vehicle_history_time_range_daily_summary       | สรุปรถเข้า-ออกตามช่วงเวลา เฉพาะวัน
            VehicleHistoryTimeRange,                  // 26 | vehicle_history_time_range                     | รายงานรถเข้าออก ตามช่วงเวลา      
            VehicleHistoryByDate,                     // 27 | vehicle_history_by_date                        | รายงานรถเข้าออก ตามวันที่        
            Promotion,                                // 28 | promotion                                      | รายงานตราประทับ                
            VehicleHistoryWithGuardhouse,             // 29 | vehicle_history_with_guardhouse                | รายงานรถเข้าออก แสดงช่องทาง      
            VehicleHistorySummaryByDate,              // 30 | vehicle_history_summary_by_date                | สรุปรถเข้าออก ตามวันที่         
            VoidCard,                                 // 31 | void_card                                      | การเคลียร์ข้อมูลบัตร            
            OutstandingVehicleWithImage,              // 32 | outstanding_vehicle_with_image                 | รถคงค้างแสดงรูปภาพ             
            VoidTaxInvoice,                           // 33 | void_tax_invoice                               | ยกเลิกใบกำกับภาษีอย่างย่อ       
            DailyVehicleRevenueWithVat,               // 34 | daily_vehicle_revenue_with_vat                 | ภาษีขายค่าบริการที่จอดรถประจำวัน 
            MonthlyVehicleRevenueWithVat,             // 35 | monthly_vehicle_revenue_with_vat               | ภาษีขายค่าบริการที่จอดรถประจำเดือน
            TotalRevenueSummary,                      // 36 | total_revenue_summary                          | สรุปรายได้                    
            HourlyVehicleHistory,                     // 37 | hourly_vehicle_history                         | รายงานการจอดตามรายชั่วโมง       
            Coupon,                                   // 38 | coupon                                         | รายงานคูปอง                   
            RevenuePerOfficerDailySummary,            // 39 | revenue_per_officer_daily_summary              | สรุปยอดของเจ้าหน้าที่ประจำวัน    
                                                      // 40 |                                                |
            MemberHistoryNoAntiPassback,              // 41 | member_history_no_anti_passback                | การเข้าออก Member (No Anti Passback)
            MemberHistoryWithImage,                   // 42 | member_history_with_image                      | การเข้าออก Member แสดงรูปภาพ    
            MemberListWithName,                       // 43 | member_list_with_name                          | รายชื่อ Member                
            MemberHistory,                            // 44 | member_history                                 | การเข้าออก Member             
            MemberHistorySummary,                     // 45 | member_history_summary                         | การเข้าออก Member สรุป    
            MemberCredit,                             // 46 | member_credit                                  | เครดิต Member           
            MemberListByGroup,                        // 47 | member_list_by_group                           | รายชื่อ Member แบบกลุ่ม    
            MemberListByGroupSummary,                 // 48 | member_list_by_group_summary                   | สรุป Member แบบกลุ่ม      
            RevenueDaily,                             // 49 | revenue_daily                                  | ภาษีขายประจำวัน           
            RevenueMonthly,                           // 50 | revenue_monthly                                | ภาษีขายประจำเดือน         
            VehicleHistorySummaryByCarType,           // 51 | vehicle_history_summary_by_car_type            | 51สรุปตามประเภทรถ        
            RevenueWithVat,                           // 52 | revenue_with_vat                               | รายงานภาษีขาย            
            RevenueDailySummary,                      // 53 | revenue_daily_summary                          | รายงานสรุปรายรับ (รายวัน)   
            RevenueMonthlySummary,                    // 54 | revenue_monthly_summary                        | รายงานสรุปรายรับ (รายเดือน) 
                                                      // 55 |                                                |
                                                      // 56 |                                                |
            ParkingHoursStatistic,                    // 57 | parking_hours_statistic                        | รายงานสถิติชั่วโมงการจอด     
            MemberCard,                               // 58 | member_card                                    | รายงานข้อมูลบัตรสมาชิก      
            CardRegistrationHistory,                  // 59 | card_registration_history                      | รายงานประวัติการบันทึกบัตร    
            ParkingHistoryPerDay,                     // 60 | parking_history_per_day                        | รายงานการใช้บริการลานจอด/วัน 
            ParkingHistoryPerMonth,                   // 61 | parking_history_per_month                      | รายงานการใช้บริการสรุปรายเดือน
            MemberHistoryByGender,                    // 62 | member_history_by_gender                       | รายงานการใช้บริการลานจอดของกลุ่มสมาชิก (เพศ)
            MemberHistoryByHoliday,                   // 63 | member_history_by_holiday                      | รายงานการใช้บริการลานจอดของกลุ่มสมาชิก (วันหยุด)
            NeverUsedServiceMember,                   // 64 | never_used_service_member                      | รายงานสมาชิกที่ไม่เคยใช้บริการ   
            NeverUsedServiceMemberSummary,            // 65 | never_used_service_member_summary              | รายงานสรุปสมาชิกที่ไม่เคยใช้บริการ
            ParkingUsageByTime,                       // 66 | parking_usage_by_time                          | รายงานการใช้บริการลานจอดแยกตามช่วงเวลา       
            RevenueDailySummaryWithTaxInvoice,        // 67 | revenue_daily_summary_with_tax_invoice         | รายงานสรุปการขายประจำวัน/สำเนาใบกำกับภาษีอย่างย่อ
            ParkingUsageForInvoicedItems,             // 68 | parking_usage_for_invoiced_items               | รายงานการใช้บริการที่จอดรถ เฉพาะรายการแจ้งหนี้
                                                      // 69 |                                                |
            OvernightVehicle,                         // 70 | overnight_vehicle                              | รายงานรถค้างคืน              
                                                      // 71 |                                                |
            RevenueWithVatSummary,                    // 72 | revenue_with_vat_summary                       | รายงานภาษีขาย (แบบสรุป)      
            VehicleHistoryByTime,                     // 74 | vehicle_history_by_time                        | รถเข้าออก ตามช่วงเวลา         
            VehicleHistoryStatisticByPromotionChart,  // 75 | vehicle_history_statistic_by_promotion_chart   | สถิติรถเข้าออก(กราฟ) ตาม Promotion 
            VoidRevenue,                              // 76 | void_revenue                                   | ยกเลิกรายได้ค่าจอดรถ               
            VehicleHistoryBuilding,                   // 77 | vehicle_history_building                       | การเข้าออก Member ที่ตึก           
            VehicleHistoryBuildingWithImage,          // 78 | vehicle_history_building_with_image            | การเข้าออก Member ที่ตึก แสดงรูปภาพ
            RevenueSummaryByDate,                     // 79 | revenue_summary_by_date                        | รายงานสรุปรายได้ ตามวันที่        
            CouponUsage,                              // 80 | coupon_usage                                   | การใช้คูปอง
            MemberPrepaid,                            // 81 | member_prepaid                                 | Member Prepaid            
            GuardhouseRevenueSummary,                 // 82 | guardhouse_revenue_summary                     | สรุปเงินนำส่งรายได้ค่าที่จอดรถประจำวัน
            OutstandingVehicleByDate,                 // 83 | outstanding_vehicle_by_date                    | รายงานรถคงค้าง ตามวันที่        
            RevenueAndHistorySummary,                 // 84 | revenue_and_history_summary                    | รายงานสรุปรายได้และการเข้าออกลานจอดรถ
            VehicleEntranceByTime,                    // 85 | vehicle_entrance_by_time                       | รายงานรถเข้าตามช่วงเวลา            
            VehicleEntranceByDate,                    // 86 | vehicle_entrance_by_date                       | รายงานรถเข้า ตามวันที่             
            VehicleExitByTime,                        // 87 | vehicle_exit_by_time                           | รายงานรถออก ตามช่วงเวลา          
            VehicleExitByDate,                        // 88 | vehicle_exit_by_date                           | รายงานรถออก ตามวันที่            
            PromotionSetPriceHistory,                 // 89 | promotion_set_price_history                    | รายงานการเพิ่ม-ลดนาทีของโปรโมชั่น    
            MemberCardHistory,                        // 90 | member_card_history                            | รายงานบัตรสมาชิก                    
                                                      // 91 |                                                |
                                                      // 92 |                                                |
                                                      // 93 |                                                |
                                                      // 94 |                                                |
            MemberCardCancellationAndSuspension,      // 95 | member_card_cancellation_and_suspension        | รายงานการยกเลิก และระงับการใช้งานบัตรสมาชิก
            ConditionalParking,                       // 96 | conditional_parking                            | รายงานการจอดรถแบบมีเงื่อนไข     
            LoggingHistory,                           // 97 | logging_history                                | รายงานการเก็บ Log            
            MemberGroupPriceMonthPaymentFullReceipt,  // 98 | membergroupprice_month_payment_full_receipt    | รายงานการรับชำระค่าบริการที่จอดรถรายเดือน (ใบเสร็จเต็มรูปแบบ)
            MemberGroupPriceMonthPaymentCash,         // 99 | membergroupprice_month_payment_cash            | รายงานการรับชำระค่าบริการที่จอดรถรายเดือน (CASH)        
            MemberGroupPriceMonthPayment,            // 100 | membergroupprice_month_payment                 | รายงานการรับชำระค่าบริการที่จอดรถรายเดือน
            VehicleEntrance,                         // 101 | vehicle_entrance                               | รายงานการเข้าของรถ           
            VehicleExit,                             // 102 | vehicle_exit                                   | รายงานการออกของรถ          
            PromotionUsagePerPerson,                 // 103 | promotion_usage_per_person                     | รายงานจำนวนการใช้ตราประทับรายบุคคล รายงานตราประทับ
            LossCardFee,                             // 104 | loss_card_fee                                  | รายงานค่าปรับบัตรหาย (รถยนต์) 
            MemberCardRevenue,                       // 105 | member_card_venenue                            | รายงานค่าบัตรสมาชิก (รถยนต์)  
            MonthlyParkingFeeCash,                   // 106 | monthly_parking_fee_cash                       | รายงานค่าบริการจอดรถรายเดือน (Cash) 
            MonthlyParkingFeeInvoice,                // 107 | monthly_parking_fee_invoice                    | รายงานค่าบริการจอดรถรายเดือน (แจ้งหนี้)
            EStampOvertimeFee,                       // 108 | e_stamp_overtime_fee                           | รายงานค่าบริการ (รถยนต์) ส่วนเกินโควต้าชม. ตราประทับ(E-stamp)
            EStampCodeAndQuota,                      // 109 | e_stamp_code_and_quota                         | รายงานค่าบริการ (รถยนต์) ทะเบียนโควต้าและรหัสตราประทับ        
            UserList,                                // 111 | user_list                                      | รายชื่อผู้ใช้                
            VoidSummary,                             // 113 | void_summary                                   | รายงานสรุปการยกเลิกรับเงืน    
            VoidRevenueWithDetails,                  // 114 | void_revenue_with_details                      | รายงานรายละเอียดการยกเลิกรับเงืน
            UsersCardByCardTypes,                    // 115 | users_card_by_card_types                       | รายงานรายละเอียดบัตรพนักงานตามประเภทบัตร  
            LossCardSummaryDaily,                    // 116 | loss_card_summary_daily                        | รายงานสรุปบัตรหาย(รายวัน)              
            LossCardFeeWithDetails,                  // 117 | loss_card_fee_with_details                     | รายงานรายละเอียดค่าปรับบัตรหาย           
            VehicleAmountHourly,                     // 119 | vehicle_amount_hourly                          | รายงานปริมาณรถในแต่ละชั่วโมง            
            VehicleAmountDaily,                      // 120 | vehicle_amount_daily                           | รายงานปริมาณรถในแต่ละวัน              
            VehicleAmountDailyPerMonth,              // 121 | vehicle_amount_daily_per_month                 | รายงานปริมาณรถแต่ละชั่วโมงต่อเดือน        
            ParkingAmountAndTimeSummaryDaily,        // 122 | parking_amount_and_time_summary_daily          | รายงานสรุปจำนวนรถและระยะเวลาจอดรถ(รายวัน)
            UserSummary,                             // 123 | user_summary                                   | รายงานสรุปปฏิบัติงาน         
                                                     // 124 |                                                |
                                                     // 125 |                                                |
            RevenueSummary,                          // 126 | revenue_summary                                | รายงานสรุปรายได้            
            UserDetails,                             // 127 | user_details                                   | รายงานข้อมูลพนักงาน         
            ParkingHistory,                          // 128 | parking_history                                | รายงานการการจอดรถ         
            OverdateDetails,                         // 129 | overdate_details                               | รายงานรายละเอียดรถจอดค้าง    
            ExpiredMember,                           // 130 | expired_member                                 | รายงานข้อมูลหมดอายุ         
            MemberCancellation,                      // 131 | member_cancellation                            | รายงานข้อมูลยกเลิกบัตร        
            AverageParkingTimeDaily,                 // 132 | average_parking_time_daily                     | รายงานสรุปเวลาจอดรถเฉลี่ย(รายวัน)ชม./นาที
            RevenueWithVatSummary2,                  // 133 | revenue_with_vat_summary_                      | สรปุรายงานภาษีขายค่าบริการจอดรถยนต์     
            RevenueWithVatPerUserSummary,            // 134 | revenue_with_vat_per_user_summary              | รายงานสรุปภาษี รายพนักงาน            
            RevenueWithVatPerUser,                   // 135 | revenue_with_vat_per_user                      | รายงานภาษีขายค่าบริการ<แยกตามพนักงาน>  
            RevenueWithTaxInvoicePerUser,            // 136 | revenue_with_tax_invoice_per_user              | ใบกำกับภาษีอย่างย่อ(VAT INCLUDED)<แยกตามรายพนักงาน>   
            CardCreationDetails,                     // 137 | card_creation_details                          | รายงานรายละเอียดการทำบัตร                       
            CardCancellation,                        // 138 | card_cancellation                              | รายงานข้อมูลบัตรระงับการใช้        
                                                     // 139 |                                                |
            CompanyMerchantData,                     // 140 | company_merchant_data                          | รายงานข้อมูลบริษัท-ร้านค้า         
            CouponAmountPerMerchantSummary,          // 141 | coupon_amount_per_merchant_summary             | รายงานสรุปจำนวนคูปองแยกตามร้านค้า 
            CouponAmountByMerchantWithDetails,       // 142 | coupon_amount_by_merchant_with_details         | รายงานรายละเอียดจำนวนคูปองแยกตามกลุ่มร้านค้า 
            CouponAmountByUserByDateSummary,         // 143 | coupon_amount_by_user_by_date_summary          | รายงานสรุปจำนวนคูปองแยกตามเจ้าหน้าที่/ตามวันที่
            CouponAmountByUserWithDetails,           // 144 | coupon_amount_by_user_with_details             | รายงานรายละเอียดจำนวนคูปองแยกตามเจ้าหน้าที่  
            CouponOvertimeByMerchant,                // 146 | coupon_overtime_by_merchant                    | รายงานสรุปเรียกเก็บส่วนเกินโควต้าแยกตามกลุ่มร้านค้า
            CouponDetailsByMerchant,                 // 147 | coupon_details_by_merchant                     | รายงานรายละเอียดเรียกเก็บโควต้าแยกตามกลุ่มร้านค้า 
            FreeCouponByMerchant,                    // 148 | free_coupon_by_merchant                        | รายงานสรุปส่วนลดจอดฟรีแยกตามกลุ่มร้านค้า
            FreeCouponByMerchantWithDetails,         // 149 | free_coupon_by_merchant_with_details           | รายงานรายละเอียดส่วนลดจอดฟรีแยกตามกลุ่มร้านค้า
            OpportunityCostSummaryByMerchant,        // 150 | opportunity_cost_summary_by_merchant           | รายงานสรุปค่าเสียโอกาศทั้งหมดที่เกิดขึ้นจริงแยกตามกลุ่มร้านค้า
            OpportunityCostByMerchantWithDetails,    // 151 | opportunity_cost_by_merchant_with_details      | รายงานรายละเอียดค่าเสียโอกาสทั้งหมดที่เกิดขึ่นจริงแยกตามกลุ่มร้านค้า
            ProsetpriceSummary,                      // 152 | prosetprice_summary                            | รายงานสรุปยอดเรียกเก็บเงิน
            ProsetpriceWithDetails,                  // 153 | prosetprice_with_details                       | รายงานรายละเอียดเงินเรียกเก็บ
            ParkingAverageByMerchant,                // 154 | parking_average_by_merchant                    | รายงานเวลาเฉลี่ยการจอดรถแยกตามร้านค้า
                                                     // 155 |                                                |
                                                     // 156 |                                                |
            QrCodeCreationQuotaByRentalSpaceSummary, // 157 | qr_code_creation_quota_by_rental_space_summary | รายงานสรุปจำนวนการสร้าง QR CODE โควต้าตามสิทธิ์ขนาดพื้นที่เช่า
                                                     // 158 |                                                |
            QrCodeAdditionalCreationSummary,         // 159 | qr_code_additional_creation_summary            | รายงานสรุปจำนวนการสร้าง QR CODE ซื้อเพิ่ม
                                                     // 160 |                                                |
                                                     // 161 |                                                |
                                                     // 162 |                                                | 
            VehicleHistoryWithPaymentChannel         // 163 | vehicle_history_with_payment_channel           | การเข้าออกของรถแสดงช่องทางการชำระเงิน
        }
        
        #endregion REPORTS_END
    }
}