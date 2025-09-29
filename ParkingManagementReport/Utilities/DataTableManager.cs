using System;
using System.Data;

namespace ParkingManagementReport.Utilities
{
    internal class DataTableManager
    {
        internal static DataTable ConvertTableType(DataTable dt)
        {
            DataTable newDt = dt.Clone();
            foreach (DataColumn dc in newDt.Columns)
            {
                dc.DataType = Type.GetType("System.String");
            }
            foreach (DataRow dr in dt.Rows)
            {
                newDt.ImportRow(dr);
            }
            dt.Dispose();

            return newDt;
        }

    }
}