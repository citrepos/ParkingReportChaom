using System;
using System.Data;
using ParkingManagementReport.Utilities.Database;

namespace ParkingManagementReport.Utilities
{
    internal class DataTableManager
    {
        public static DataTable EditedThanapoomDataTable(DataTable dataTable, int selectedReportId)
        {
            DataTable newDt = dataTable;

            switch (selectedReportId)
            {
                case 0:
                    if (newDt.Columns.Contains("เจ้าหน้าที่ขาเข้า"))
                        newDt.Columns.Remove("เจ้าหน้าที่ขาเข้า");

                    string indexColumnName = "ลำดับ";
                    if (!newDt.Columns.Contains(indexColumnName))
                        newDt.Columns.Add(indexColumnName, typeof(int));

                    for (int i = 0; i < newDt.Rows.Count; i++)
                    {
                        newDt.Rows[i][indexColumnName] = i + 1;  // นับจาก 1
                    }
                    break;
                case 6:
                    if (newDt.Columns.Contains("เจ้าหน้าที่ขาเข้า"))
                        newDt.Columns.Remove("เจ้าหน้าที่ขาเข้า");
                    if (newDt.Columns.Contains("เจ้าหน้าที่ขาออก"))
                        newDt.Columns.Remove("เจ้าหน้าที่ขาออก");
                    break;

                case 46:
                    newDt = CRUDManager.GetMemberMonthSummary(newDt);
                    break;

                default:
                    break;
            }

            return newDt;
        }
    }
}
