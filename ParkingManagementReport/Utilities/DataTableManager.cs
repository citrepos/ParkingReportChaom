using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ParkingManagementReport.Common;
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

        public static DataTable ConvertedDataGridView(DataGridView dataGridView)
        {
            try
            {
                DataTable dtNew = new DataTable();

                // Create columns using HeaderText
                foreach (DataGridViewColumn col in dataGridView.Columns)
                {
                    dtNew.Columns.Add(col.HeaderText);
                }

                // Copy rows
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataRow newRow = dtNew.NewRow();

                        for (int i = 0; i < dataGridView.Columns.Count; i++)
                        {
                            newRow[i] = row.Cells[i].Value ?? DBNull.Value;
                        }

                        dtNew.Rows.Add(newRow);
                    }
                }
                return dtNew;
            }
            catch
            {
                MessageBox.Show("Unable to convert DataGridViewData to DataTable");
                return null;
            }
        }

        public static DataTable ConvertedTableType(DataTable dt)
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

        #region Reports
        public static DataTable สถิติการเข้าออก(DataTable dt, DataGridView dataGridView)
        {
            DataTable dtTmp = new DataTable();
            DataColumn dc = new DataColumn("ลำดับ", typeof(string));
            dtTmp.Columns.Add(dc);
            dc = new DataColumn("ประเภท", typeof(string));
            dtTmp.Columns.Add(dc);
            dc = new DataColumn("ทะเบียน", typeof(string));
            dtTmp.Columns.Add(dc);
            dc = new DataColumn("จำนวนเข้า", typeof(int));
            dtTmp.Columns.Add(dc);
            dc = new DataColumn("จำนวนออก", typeof(int));
            dtTmp.Columns.Add(dc);
            dc = new DataColumn("จำนวนเข้าออก", typeof(int));
            dtTmp.Columns.Add(dc);
            dc = new DataColumn("รายได้", typeof(int));
            dtTmp.Columns.Add(dc);
            dc = new DataColumn("ส่วนลด", typeof(int));
            dtTmp.Columns.Add(dc);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    string strLicense = dt.Rows[i].ItemArray[0].ToString();
                    DataRow dr = dtTmp.NewRow();
                    string strNo = (i + 1).ToString();
                    dr[0] = strNo;
                    strNo = "0";
                    string strCarType = "";
                    string sql = "SELECT no,cartype FROM recordin WHERE license = '" + strLicense + "'";
                    DataTable dt1 = DbController.LoadData(sql);
                    int intNoOut = 0;
                    int intPrice = 0;
                    int intDiscount = 0;
                    if (dt1.Rows.Count > 0)
                    {
                        strNo = dt1.Rows.Count.ToString();
                        int intCarType = Convert.ToInt32(dt1.Rows[0].ItemArray[1]);
                        strCarType = AppGlobalVariables.CarTypesById[intCarType];
                        for (int n = 0; n < dt1.Rows.Count; n++)
                        {
                            try
                            {
                                string strNoOut = dt1.Rows[n].ItemArray[0].ToString();

                                sql = "SELECT price,discount FROM recordout WHERE no=" + strNoOut;//
                                DataTable dt2 = DbController.LoadData(sql);
                                if (dt2.Rows.Count > 0)
                                {
                                    intNoOut++;
                                    int intTmp = Convert.ToInt32(dt2.Rows[n].ItemArray[0]);
                                    intPrice += intTmp;
                                    intTmp = Convert.ToInt32(dt2.Rows[n].ItemArray[1]);
                                    intDiscount += intTmp;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    dr[1] = strCarType;
                    dr[2] = strLicense;
                    dr[3] = strNo;
                    dr[4] = intNoOut.ToString();
                    intNoOut += Convert.ToInt32(strNo);

                    dr[5] = intNoOut.ToString();
                    sql = "SELECT  sum(recordout.price), sum(recordout.discount) "
                    + "FROM recordin INNER JOIN recordout on recordin.no = recordout.no WHERE recordin.license = '" + strLicense + "'";
                    DataTable dt3 = DbController.LoadData(sql);
                    dr[6] = dt3.Rows[0].ItemArray[0];
                    dr[7] = dt3.Rows[0].ItemArray[1];

                    dtTmp.Rows.Add(dr); //this will add the row at the end of the datatable
                }
                catch { }
            }
            dtTmp.DefaultView.Sort = dtTmp.Columns[5].ColumnName + " DESC";
            dataGridView.DataSource = dtTmp;

            return dtTmp;
        }

        public static DataTable สรุปรถยนต์เข้าออกตามชั่วโมง(DataTable dt)
        {
            #region safe getters
            DateTime GetDate(DataRow r, string name)
            {
                try { return Convert.ToDateTime(r[name]); }
                catch { return DateTime.MinValue; }
            }

            int GetInt(DataRow r, string name)
            {
                try { return Convert.ToInt32(r[name]); }
                catch { return 0; }
            }
            #endregion

            // สร้างตารางสำหรับผลลัพธ์
            DataTable perHour = new DataTable();
            perHour.Columns.Add(new DataColumn("ชั่วโมง", typeof(string)));            // 00:00 - 00:59
            perHour.Columns.Add(new DataColumn("ลูกค้าทั่วไปเข้า", typeof(int)));
            perHour.Columns.Add(new DataColumn("สมาชิกเข้า", typeof(int)));
            perHour.Columns.Add(new DataColumn("ลูกค้าทั่วไปออก", typeof(int)));
            perHour.Columns.Add(new DataColumn("สมาชิกออก", typeof(int)));

            // เตรียมข้อมูลเข้า & ออก แยกง่ายๆ
            var recordIn = dt.AsEnumerable()
                .Where(r => r.Table.Columns.Contains("datein"));

            var recordOut = dt.AsEnumerable()
                .Where(r => r.Table.Columns.Contains("dateout"));

            // Loop 0 - 23 ชั่วโมง
            for (int hour = 0; hour < 24; hour++)
            {
                string range = $"{hour:00}:00 - {hour:00}:59";

                // ----------- Count In ----------
                int inVisitor = recordIn.Count(r =>
                    GetDate(r, "datein").Hour == hour &&
                    GetInt(r, "cartype") < 200);

                int inMember = recordIn.Count(r =>
                    GetDate(r, "datein").Hour == hour &&
                    GetInt(r, "cartype") == 200);

                // ----------- Count Out ----------
                int outVisitor = recordOut.Count(r =>
                    GetDate(r, "dateout").Hour == hour &&
                    GetInt(r, "cartype") < 200);

                int outMember = recordOut.Count(r =>
                    GetDate(r, "dateout").Hour == hour &&
                    GetInt(r, "cartype") == 200);

                // Add row
                perHour.Rows.Add(range, inVisitor, inMember, outVisitor, outMember);
            }

            return perHour;
        }


        public static DataTable สรุปรถยนต์เข้าออกตามวันที่(DataTable dt)
        {
            #region safe getters
            DateTime GetDate(DataRow r)
            {
                try { return Convert.ToDateTime(r["out_date"]); }
                catch { return DateTime.MinValue; }
            }

            int GetInt(DataRow r, string name)
            {
                try { return Convert.ToInt32(r[name]); }
                catch { return 0; }
            }

            decimal GetDecimal(DataRow r, string name)
            {
                try { return Convert.ToDecimal(r[name]); }
                catch { return 0; }
            }
            #endregion

            var grouped =
                from row in dt.AsEnumerable()
                let outDate = GetDate(row)
                group row by outDate.Date into g
                orderby g.Key
                select new
                {
                    DayRange = $"{g.Key:dd-MM-yyyy} ถึง {g.Key.AddDays(1).AddSeconds(-1):dd-MM-yyyy}",
                    Visitor = g.Count(r => GetInt(r, "cartype") < 200),
                    Member = g.Count(r => GetInt(r, "cartype") == 200),
                    GotPro = g.Count(r => GetInt(r, "proid") != 0),
                    LostPro = g.Count(r => GetInt(r, "proid") == 0),
                    Income = g.Sum(r => GetDecimal(r, "price"))
                };

            DataTable summarizedTable = new DataTable();
            summarizedTable.Columns.Add("วันที่", typeof(string));
            summarizedTable.Columns.Add("ผู้มาติดต่อ", typeof(int));
            summarizedTable.Columns.Add("สมาชิก", typeof(int));
            summarizedTable.Columns.Add("ประทับตรา", typeof(int));
            summarizedTable.Columns.Add("ไม่ได้ประทับตรา", typeof(int));
            summarizedTable.Columns.Add("รายได้", typeof(decimal));

            foreach (var g in grouped)
            {
                summarizedTable.Rows.Add(g.DayRange, g.Visitor, g.Member, g.GotPro, g.LostPro, g.Income);
            }

            return summarizedTable;
        }


        public static DataTable การเข้าออกMemberแสดงรูปภาพ(DataTable dt)
        {
            DataTable Map = new DataTable();

            Map.Columns.Add(new DataColumn("ลำดับ", typeof(string)));
            Map.Columns.Add(new DataColumn("ชื่อ", typeof(string)));
            Map.Columns.Add(new DataColumn("ทะเบียน", typeof(string)));
            Map.Columns.Add(new DataColumn("วันที่", typeof(string)));
            Map.Columns.Add(new DataColumn("ประตู", typeof(string)));
            Map.Columns.Add(new DataColumn("picdiv", typeof(System.Byte[])));
            Map.Columns.Add(new DataColumn("piclic", typeof(System.Byte[])));

            ///////////////////////////////////////////////////
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow dr = Map.NewRow();
                try
                {
                    dr["ลำดับ"] = dt.Rows[j]["ลำดับ"];
                    dr["ชื่อ"] = dt.Rows[j]["ชื่อ"];
                    dr["ทะเบียน"] = dt.Rows[j]["ทะเบียน"];
                    dr["วันที่"] = dt.Rows[j]["วันที่"];
                    dr["ประตู"] = dt.Rows[j]["ประตู"];
                }
                catch (Exception) { }
                FileStream fiStream;
                BinaryReader binReader;
                byte[] pic = { };

                try
                {
                    fiStream = new FileStream(dt.Rows[j]["picdiv"].ToString(), FileMode.Open);
                    binReader = new BinaryReader(fiStream);
                    pic = binReader.ReadBytes((int)fiStream.Length);
                    dr["picdiv"] = pic;
                    fiStream.Close();
                    binReader.Close();
                }
                catch (Exception)
                {
                    dr["picdiv"] = null;
                }


                try
                {
                    fiStream = new FileStream(dt.Rows[j]["piclic"].ToString(), FileMode.Open);
                    binReader = new BinaryReader(fiStream);
                    pic = binReader.ReadBytes((int)fiStream.Length);
                    dr["piclic"] = pic;
                    fiStream.Close();
                    binReader.Close();
                }
                catch (Exception)
                {
                    dr["piclic"] = null;
                }

                Map.Rows.Add(dr);
            }

            return Map;
        }

        public static DataTable คงค้างแสดงรูปภาพ(DataTable dt)
        {
            DataTable Map = new DataTable("myMember");  //*** DataTable Map DataSet.xsd ***//

            Map.Columns.Add(new DataColumn("ลำดับ", typeof(string)));
            Map.Columns.Add(new DataColumn("ประเภท", typeof(string)));
            Map.Columns.Add(new DataColumn("ทะเบียน", typeof(string)));
            Map.Columns.Add(new DataColumn("เวลาเข้า", typeof(string)));
            Map.Columns.Add(new DataColumn("เจ้าหน้าที่ขาเข้า", typeof(string)));
            Map.Columns.Add(new DataColumn("picdiv", typeof(System.Byte[])));
            Map.Columns.Add(new DataColumn("piclic", typeof(System.Byte[])));

            if (Configs.UseNameOnCard) //Mac 2018/12/13
                Map.Columns.Add(new DataColumn("ชื่อบัตร", typeof(string)));

            ///////////////////////////////////////////////////
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow dr = Map.NewRow();
                dr["ลำดับ"] = dt.Rows[j]["ลำดับ"];
                dr["ประเภท"] = dt.Rows[j]["ประเภท"];
                dr["เวลาเข้า"] = dt.Rows[j]["เวลาเข้า"];
                dr["เจ้าหน้าที่ขาเข้า"] = dt.Rows[j]["เจ้าหน้าที่ขาเข้า"];
                try
                {
                    dr["ทะเบียน"] = dt.Rows[j]["ทะเบียน"];
                }
                catch (Exception) { }
                FileStream fiStream;
                BinaryReader binReader;
                byte[] pic = { };

                try
                {
                    fiStream = new FileStream(dt.Rows[j]["picdiv"].ToString(), FileMode.Open);
                    binReader = new BinaryReader(fiStream);
                    pic = binReader.ReadBytes((int)fiStream.Length);
                    dr["picdiv"] = pic;
                    fiStream.Close();
                    binReader.Close();
                }
                catch (Exception)
                {
                    dr["picdiv"] = null;
                }

                try
                {
                    fiStream = new FileStream(dt.Rows[j]["piclic"].ToString(), FileMode.Open);
                    binReader = new BinaryReader(fiStream);
                    pic = binReader.ReadBytes((int)fiStream.Length);
                    dr["piclic"] = pic;
                    fiStream.Close();
                    binReader.Close();
                }
                catch (Exception ex)
                {
                    dr["piclic"] = null;
                }

                if (Configs.UseNameOnCard) //Mac 2018/12/13
                    dr["ชื่อบัตร"] = dt.Rows[j]["ชื่อบัตร"];

                Map.Rows.Add(dr);
            }

            return Map;
        }

        public static DataTable การยกไม้แสดงรูปภาพ(DataTable dt)
        {
            DataTable Map = new DataTable("myMember");  //*** DataTable Map DataSet.xsd ***//

            DataRow dr = null;
            Map.Columns.Add(new DataColumn("เวลายก", typeof(string)));
            Map.Columns.Add(new DataColumn("พนักงาน", typeof(string)));
            Map.Columns.Add(new DataColumn("ประตู", typeof(string)));
            Map.Columns.Add(new DataColumn("บันทึก", typeof(string)));
            Map.Columns.Add(new DataColumn("picdiv", typeof(System.Byte[])));
            Map.Columns.Add(new DataColumn("piclic", typeof(System.Byte[])));

            ///////////////////////////////////////////////////
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                dr = Map.NewRow();
                dr["เวลายก"] = dt.Rows[j]["เวลายก"];
                dr["พนักงาน"] = dt.Rows[j]["พนักงาน"];
                dr["ประตู"] = dt.Rows[j]["ประตู"];
                try
                {
                    dr["บันทึก"] = dt.Rows[j]["บันทึก"];
                }
                catch (Exception) { }
                FileStream fiStream;
                BinaryReader binReader;
                byte[] pic = { };

                try
                {
                    fiStream = new FileStream(dt.Rows[j]["picdiv"].ToString(), FileMode.Open);
                    binReader = new BinaryReader(fiStream);
                    pic = binReader.ReadBytes((int)fiStream.Length);
                    dr["picdiv"] = pic;
                    fiStream.Close();
                    binReader.Close();
                }
                catch (Exception)
                {
                    dr["picdiv"] = null;
                }

                try
                {
                    fiStream = new FileStream(dt.Rows[j]["piclic"].ToString(), FileMode.Open);
                    binReader = new BinaryReader(fiStream);
                    pic = binReader.ReadBytes((int)fiStream.Length);
                    dr["piclic"] = pic;
                    fiStream.Close();
                    binReader.Close();
                }
                catch (Exception ex)
                {
                    dr["piclic"] = null;
                }
                Map.Rows.Add(dr);
            }

            return Map;
        }

        public static DataTable การเข้าออกแสดงรูปภาพ(DataTable dt)
        {
            DataTable Map = new DataTable("myMember");  //*** DataTable Map DataSet.xsd ***//

            DataRow dr = null;
            Map.Columns.Add(new DataColumn("ลำดับ", typeof(string)));
            Map.Columns.Add(new DataColumn("ประเภท", typeof(string)));
            Map.Columns.Add(new DataColumn("ทะเบียน", typeof(string)));
            if (Configs.IsVillage && Configs.Use2Camera)
            {
                Map.Columns.Add(new DataColumn("ชื่อผู้มาติดต่อ", typeof(string)));
                Map.Columns.Add(new DataColumn("ประเภทบัตร", typeof(string)));
                Map.Columns.Add(new DataColumn("เบอร์โทรศัพท์", typeof(string)));
                Map.Columns.Add(new DataColumn("ติดต่อ", typeof(string)));
                Map.Columns.Add(new DataColumn("ที่อยู่", typeof(string)));
            }
            Map.Columns.Add(new DataColumn("เวลาเข้า", typeof(string)));
            Map.Columns.Add(new DataColumn("เจ้าหน้าที่ขาเข้า", typeof(string)));
            Map.Columns.Add(new DataColumn("เวลาออก", typeof(string)));
            Map.Columns.Add(new DataColumn("รายได้", typeof(string)));
            Map.Columns.Add(new DataColumn("ส่วนลด", typeof(string)));
            Map.Columns.Add(new DataColumn("เจ้าหน้าที่ขาออก", typeof(string)));
            Map.Columns.Add(new DataColumn("il", typeof(System.Byte[]))); // เข้า-ทะเบียน
            Map.Columns.Add(new DataColumn("ol", typeof(System.Byte[]))); // ออก-ทะเบียน
            Map.Columns.Add(new DataColumn("iv", typeof(System.Byte[]))); // เข้า-หน้าคน
            Map.Columns.Add(new DataColumn("ov", typeof(System.Byte[]))); // ออก-หน้าคน

            if (Configs.IsVillage && Configs.Use2Camera)
                Map.Columns.Add(new DataColumn("vi", typeof(System.Byte[])));
            else if (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0)
                Map.Columns.Add(new DataColumn("io", typeof(System.Byte[])));
            int i = 0;
            ///////////////////////////////////////////////////
            for (i = 0; i < dt.Rows.Count; i++)
            {
                FileStream fiStream;
                BinaryReader binReader;
                byte[] pic = { };
                try
                {
                    dr = Map.NewRow();
                    dr["ลำดับ"] = dt.Rows[i]["ลำดับ"];
                    dr["ประเภท"] = dt.Rows[i]["ประเภท"];
                    dr["ทะเบียน"] = dt.Rows[i]["ทะเบียน"];
                    dr["เวลาเข้า"] = dt.Rows[i]["เวลาเข้า"];
                    dr["เจ้าหน้าที่ขาเข้า"] = dt.Rows[i]["เจ้าหน้าที่ขาเข้า"];
                    dr["เวลาออก"] = dt.Rows[i]["เวลาออก"];
                    dr["รายได้"] = dt.Rows[i]["รายได้"];
                    dr["ส่วนลด"] = dt.Rows[i]["ส่วนลด"];
                    dr["เจ้าหน้าที่ขาออก"] = dt.Rows[i]["เจ้าหน้าที่ขาออก"];
                    if (Configs.IsVillage && Configs.Use2Camera)
                    {
                        dr["ชื่อผู้มาติดต่อ"] = dt.Rows[i]["ชื่อผู้มาติดต่อ"];
                        dr["ประเภทบัตร"] = dt.Rows[i]["ประเภทบัตร"];
                        dr["เบอร์โทรศัพท์"] = dt.Rows[i]["เบอร์โทรศัพท์"];
                        dr["ติดต่อ"] = dt.Rows[i]["ติดต่อ"];
                        dr["ที่อยู่"] = dt.Rows[i]["ที่อยู่"];
                    }

                    try
                    {
                        fiStream = new FileStream(dt.Rows[i]["il"].ToString(), FileMode.Open);
                        binReader = new BinaryReader(fiStream);
                        pic = binReader.ReadBytes((int)fiStream.Length);
                        dr["il"] = pic;
                        fiStream.Close();
                        binReader.Close();

                    }
                    catch (Exception ex)
                    {
                        dr["il"] = null;
                    }

                    try
                    {
                        fiStream = new FileStream(dt.Rows[i]["ol"].ToString(), FileMode.Open);
                        binReader = new BinaryReader(fiStream);
                        pic = binReader.ReadBytes((int)fiStream.Length);
                        dr["ol"] = pic;
                        fiStream.Close();
                        binReader.Close();
                    }
                    catch (Exception ex)
                    {
                        dr["ol"] = null;
                    }

                    try
                    {
                        fiStream = new FileStream(dt.Rows[i]["iv"].ToString(), FileMode.Open);
                        binReader = new BinaryReader(fiStream);
                        pic = binReader.ReadBytes((int)fiStream.Length);
                        dr["iv"] = pic;
                        fiStream.Close();
                        binReader.Close();
                    }
                    catch (Exception ex)
                    {
                        dr["iv"] = null;
                    }

                    try
                    {
                        fiStream = new FileStream(dt.Rows[i]["ov"].ToString(), FileMode.Open);
                        binReader = new BinaryReader(fiStream);
                        pic = binReader.ReadBytes((int)fiStream.Length);
                        dr["ov"] = pic;
                        fiStream.Close();
                        binReader.Close();
                    }
                    catch (Exception ex)
                    {
                        dr["ov"] = null;
                    }
                    if (Configs.IsVillage && Configs.Use2Camera)
                    {
                        try
                        {
                            fiStream = new FileStream(dt.Rows[i]["vi"].ToString(), FileMode.Open);
                            binReader = new BinaryReader(fiStream);
                            pic = binReader.ReadBytes((int)fiStream.Length);
                            dr["vi"] = pic;
                            fiStream.Close();
                            binReader.Close();
                        }
                        catch (Exception ex)
                        {
                            dr["vi"] = null;
                        }
                    }

                    if (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0)
                    {
                        try //Mac 2015/02/04
                        {
                            fiStream = new FileStream(dt.Rows[i]["io"].ToString(), FileMode.Open);
                            binReader = new BinaryReader(fiStream);
                            pic = binReader.ReadBytes((int)fiStream.Length);
                            dr["io"] = pic;
                            fiStream.Close();
                            binReader.Close();
                        }
                        catch (Exception)
                        {
                            dr["io"] = null;
                        }
                    }

                    Map.Rows.Add(dr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return Map;
        }
        #endregion


        #region DataGridView managers
        public static void CaseReportPricePromotion(int selectedReportId, DataGridView dataGridView)
        {
            dataGridView.Columns[0].HeaderText = dataGridView.Columns[0].Name = "เลขที่ใบเสร็จ/ใบกำกับภาษี";
            dataGridView.Columns[1].HeaderText = dataGridView.Columns[1].Name = "ลำดับ";
            dataGridView.Columns[2].HeaderText = dataGridView.Columns[2].Name = "ประเภท";
            dataGridView.Columns[3].HeaderText = dataGridView.Columns[3].Name = "ทะเบียน";
            dataGridView.Columns[4].HeaderText = dataGridView.Columns[4].Name = "เวลาเข้า";
            dataGridView.Columns[5].HeaderText = dataGridView.Columns[5].Name = "เจ้าหน้าที่ขาออก";
            dataGridView.Columns[6].HeaderText = dataGridView.Columns[6].Name = "เวลาออก";
            dataGridView.Columns[7].HeaderText = dataGridView.Columns[7].Name = "ชม.จอด";
            dataGridView.Columns[8].HeaderText = dataGridView.Columns[8].Name = "ชม.ส่วนลดผู้มาติดต่อ";
            dataGridView.Columns[9].HeaderText = dataGridView.Columns[9].Name = "ชม.ลด";
            dataGridView.Columns[10].HeaderText = dataGridView.Columns[10].Name = "ชม.จ่าย";
            dataGridView.Columns[11].HeaderText = dataGridView.Columns[11].Name = "ค่าปรับบัตรหาย";
            dataGridView.Columns[12].HeaderText = dataGridView.Columns[12].Name = "ค่าปรับข้ามวัน";
            dataGridView.Columns[13].HeaderText = dataGridView.Columns[13].Name = "รายได้";
            dataGridView.Columns[14].HeaderText = dataGridView.Columns[14].Name = "ส่วนลด";
            dataGridView.Columns[15].HeaderText = dataGridView.Columns[15].Name = "E-Stamp";

            if (Configs.UseMemo)
            {
                dataGridView.Columns[16].HeaderText = "บันทึกเพิ่มเติม";
                dataGridView.Columns[16].Width = 160;
            }

            if (selectedReportId == 14)
            {
                dataGridView.Columns[13].HeaderText = dataGridView.Columns[13].Name = "รายได้ก่อนภาษี";
                dataGridView.Columns[14].HeaderText = dataGridView.Columns[14].Name = "ภาษี 7%";
                dataGridView.Columns[15].HeaderText = dataGridView.Columns[15].Name = "รายได้";
                dataGridView.Columns[16].HeaderText = dataGridView.Columns[16].Name = "E-Stamp";
            }

            int intNo = dataGridView.Rows.Count - 1;
            dataGridView.Columns[11].Width = 105;
            dataGridView.Columns[15].Width = 160;
            if (selectedReportId == 14) dataGridView.Columns[16].Width = 160;
            int intSumPrice = 0;
            int intSumPriceLoss = 0;
            int intSumPriceOver = 0;
            int intSumDiscount = 0;
            double doubleSumBeforeVat = 0;
            double doubleSumVat = 0;


            for (int i = 0; i < intNo; i++)
            {
                int intID = Convert.ToInt32(dataGridView[0, i].Value);
                DateTime dto = DateTime.Parse(dataGridView[6, i].Value.ToString());
                if (intID > 0)
                {
                    string fontSlip13 = !string.IsNullOrEmpty(AppGlobalVariables.Printings.ReceiptName)
                        ? AppGlobalVariables.Printings.ReceiptName
                        : (!Configs.UseReceiptName) ? "IV" : String.Empty;

                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth)
                            dataGridView[0, i].Value = dataGridView[dataGridView.ColumnCount - 1, i].Value.ToString() + dto.ToString("yyMM") + intID.ToString("00000#");
                        else
                            dataGridView[0, i].Value = dataGridView[dataGridView.ColumnCount - 1, i].Value.ToString() + dto.ToString("yy") + intID.ToString("00000#");
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            dataGridView[0, i].Value = fontSlip13 + dto.ToString("yyMM") + intID.ToString("00000#");
                        }
                        else
                        {
                            dataGridView[0, i].Value = fontSlip13 + dto.ToString("yy") + intID.ToString("00000#");
                        }
                    }
                }

                try
                {
                    string x = dataGridView[2, i].Value.ToString();
                    int value;
                    if (int.TryParse(x, out value))
                    {
                        intID = value;
                        dataGridView[2, i].Value = AppGlobalVariables.CarTypesById[intID];
                    }
                    else
                        dataGridView[2, i].Value = x;

                }
                catch
                {
                    dataGridView[2, i].Value = "";
                }

                try
                {
                    intID = Convert.ToInt32(dataGridView[5, i].Value);
                    if (intID == 0)
                        dataGridView[5, i].Value = "";
                    else
                        dataGridView[5, i].Value = AppGlobalVariables.UsersById[intID];
                }
                catch
                {
                    dataGridView[5, i].Value = "";
                }

                DateTime dti = DateTime.Parse(dataGridView[4, i].Value.ToString());
                TimeSpan diffTime = dto - dti;
                int intHour = diffTime.Hours;
                if (diffTime.Days > 0)
                    intHour += diffTime.Days * 24;
                if (diffTime.Minutes > 0)
                    intHour++;
                dataGridView[7, i].Value = intHour.ToString();

                string totalInOut = "";
                if (diffTime.Days == 0 && diffTime.Hours == 0 && diffTime.Minutes == 0)
                    totalInOut = "0";
                else
                    totalInOut = (diffTime.Days * 24) + diffTime.Hours + "." + diffTime.Minutes.ToString("00");

                dataGridView[7, i].Value = totalInOut;

                try
                {
                    if (Configs.UseProIDAll)
                    {
                        string[] ProIDAll;
                        int intHourPro = 0;

                        if (selectedReportId == 14)
                        {
                            ProIDAll = dataGridView[16, i].Value.ToString().Split(',');
                            dataGridView[16, i].Value = "";
                        }
                        else
                        {
                            ProIDAll = dataGridView[15, i].Value.ToString().Split(',');
                            dataGridView[15, i].Value = "";
                        }

                        for (int n = 0; n < ProIDAll.Length; n++)
                        {
                            if (ProIDAll[n].Length > 0)
                            {
                                intHourPro += AppGlobalVariables.PromotionNamesMinuteMap[Convert.ToInt16(ProIDAll[n])];

                                if (selectedReportId == 14)
                                    dataGridView[16, i].Value += AppGlobalVariables.PromotionNamesById[Convert.ToInt16(ProIDAll[n])];
                                else
                                    dataGridView[15, i].Value += AppGlobalVariables.PromotionNamesById[Convert.ToInt16(ProIDAll[n])];

                                if (n < (ProIDAll.Length - 2))
                                {
                                    if (selectedReportId == 14)
                                        dataGridView[16, i].Value += "|";
                                    else
                                        dataGridView[15, i].Value += "|";
                                }

                            }
                        }

                        intHourPro = intHourPro / 60;

                        dataGridView[8, i].Value = intHourPro.ToString();
                        if (intHourPro < intHour)
                        {
                            dataGridView[9, i].Value = intHourPro.ToString();//ลด
                            dataGridView[10, i].Value = (intHour - intHourPro).ToString();//จ่าย
                        }
                        else
                        {
                            dataGridView[9, i].Value = intHour.ToString();//ลด
                            dataGridView[10, i].Value = "0";//จ่าย
                        }
                    }
                    else
                    {
                        if (selectedReportId == 14)
                            intID = Convert.ToInt32(dataGridView[16, i].Value);
                        else
                            intID = Convert.ToInt32(dataGridView[15, i].Value);

                        int intHourPro = 0;
                        if (intID > 0)
                            intHourPro = AppGlobalVariables.PromotionNamesMinuteMap[intID] / 60;
                        dataGridView[8, i].Value = intHourPro.ToString();
                        if (intHourPro < intHour)
                        {
                            dataGridView[9, i].Value = intHourPro.ToString();//ลด
                            dataGridView[10, i].Value = (intHour - intHourPro).ToString();//จ่าย
                        }
                        else
                        {
                            dataGridView[9, i].Value = intHour.ToString();//ลด
                            dataGridView[10, i].Value = "0";//จ่าย
                        }

                        if (intID > 0)
                        {
                            if (selectedReportId == 14)
                                dataGridView[16, i].Value = AppGlobalVariables.PromotionNamesById[intID];
                            else
                                dataGridView[15, i].Value = AppGlobalVariables.PromotionNamesById[intID]; //Mac 2016/03/05
                        }
                        else
                        {
                            if (selectedReportId == 14)
                                dataGridView[16, i].Value = "";
                            else
                                dataGridView[15, i].Value = "";
                        }
                    }
                }
                catch
                {
                    dataGridView[8, i].Value = "0";
                    dataGridView[9, i].Value = "0";
                    dataGridView[10, i].Value = "0";

                    if (selectedReportId == 14)
                        dataGridView[16, i].Value = "";
                    else
                        dataGridView[15, i].Value = "";
                }

                if (selectedReportId == 14)
                {
                    try
                    {
                        /*double beforeVat = double.Parse(dataGridView[15, i].Value.ToString()) * 100 / 107;
                        beforeVat = Math.Round(beforeVat, 2);
                        double vat = double.Parse(dataGridView[15, i].Value.ToString()) - beforeVat;*/

                        double vat = (double.Parse(dataGridView[15, i].Value.ToString()) * 7) / 107;

                        if (Configs.Reports.Report3Decimal)
                            vat = Math.Round(vat, 3);
                        else
                            vat = Math.Round(vat, 2);

                        double beforeVat = double.Parse(dataGridView[15, i].Value.ToString()) - vat;

                        if (Configs.Reports.Report3Decimal)
                        {
                            dataGridView[13, i].Value = beforeVat.ToString("#,###,##0.000");
                            dataGridView[14, i].Value = vat.ToString("#,###,##0.000");
                        }
                        else
                        {
                            dataGridView[13, i].Value = beforeVat.ToString("#,###,##0.00");
                            dataGridView[14, i].Value = vat.ToString("#,###,##0.00");
                        }

                        doubleSumBeforeVat += beforeVat;
                        doubleSumVat += vat;
                    }
                    catch (Exception) { }
                }


                intSumPriceLoss += Convert.ToInt32(dataGridView[11, i].Value);
                if (selectedReportId == 13)
                {

                }
                else
                    intSumPriceOver += Convert.ToInt32(dataGridView[12, i].Value);
                if (selectedReportId == 14)
                {
                    intSumPrice += Convert.ToInt32(dataGridView[15, i].Value);

                    if (Convert.ToInt32(dataGridView[15, i].Value) == 0)
                        dataGridView[10, i].Value = "0";
                }
                else
                {
                    intSumPrice += Convert.ToInt32(dataGridView[13, i].Value);
                    intSumDiscount += Convert.ToInt32(dataGridView[14, i].Value);

                    if (Convert.ToInt32(dataGridView[13, i].Value) == 0)
                        dataGridView[10, i].Value = "0";
                }
            }
            dataGridView[5, intNo].Value = "จำนวนรถ";
            dataGridView[6, intNo].Value = intNo.ToString("#,###,##0") + " คัน";
            dataGridView[10, intNo].Value = "รายได้รวม";
            dataGridView[11, intNo].Value = intSumPriceLoss.ToString("#,###,##0");
            dataGridView[12, intNo].Value = intSumPriceOver.ToString("#,###,##0");

            if (selectedReportId == 14)
            {
                if (Configs.UseCalVatFromTotal)
                {
                    dataGridView[13, intNo].Value = (Convert.ToDouble(intSumPrice) - (Convert.ToDouble(intSumPrice) * 7 / 107)).ToString("#,###,##0.00");
                    dataGridView[14, intNo].Value = (Convert.ToDouble(intSumPrice) * 7 / 107).ToString("#,###,##0.00");
                    dataGridView[15, intNo].Value = intSumPrice.ToString("#,###,##0");
                }
                else
                {
                    if (Configs.Reports.Report3Decimal)
                    {
                        dataGridView[13, intNo].Value = doubleSumBeforeVat.ToString("#,###,##0.000");
                        dataGridView[14, intNo].Value = doubleSumVat.ToString("#,###,##0.000");
                    }
                    else
                    {
                        dataGridView[13, intNo].Value = doubleSumBeforeVat.ToString("#,###,##0.00");
                        dataGridView[14, intNo].Value = doubleSumVat.ToString("#,###,##0.00");
                    }
                    dataGridView[15, intNo].Value = intSumPrice.ToString("#,###,##0");
                }
            }
            else
            {
                dataGridView[13, intNo].Value = intSumPrice.ToString("#,###,##0");
                dataGridView[14, intNo].Value = intSumDiscount.ToString("#,###,##0");
            }

            int totalLoss = intSumPriceLoss;
            int totalOver = intSumPriceOver;
            int totalPrice = intSumPrice;
            int totalDiscount = intSumDiscount;
            double totalBeforeVat = doubleSumBeforeVat;
            double totalVat = doubleSumVat;

            if (Configs.UseReceiptFor1Out)
                dataGridView.Columns[dataGridView.ColumnCount - 1].Visible = false;
        }

        public static void CaseReportTax(DataGridView dataGridView)
        {
            dataGridView.Columns[0].HeaderText = dataGridView.Columns[0].Name = "ลำดับ";
            dataGridView.Columns[1].HeaderText = dataGridView.Columns[1].Name = "ประเภท";
            dataGridView.Columns[2].HeaderText = dataGridView.Columns[2].Name = "ทะเบียน";
            dataGridView.Columns[3].HeaderText = dataGridView.Columns[3].Name = "เวลาเข้า";
            dataGridView.Columns[4].HeaderText = dataGridView.Columns[4].Name = "เจ้าหน้าที่ขาเข้า";
            dataGridView.Columns[5].HeaderText = dataGridView.Columns[5].Name = "เวลาออก";
            dataGridView.Columns[6].HeaderText = dataGridView.Columns[6].Name = "เวลาจอดรวม";
            dataGridView.Columns[7].HeaderText = dataGridView.Columns[7].Name = "ยอดรับ";
            dataGridView.Columns[8].HeaderText = dataGridView.Columns[8].Name = "ส่วนลด";
            dataGridView.Columns[9].HeaderText = dataGridView.Columns[9].Name = "ยอดรับสุทธิ";
            dataGridView.Columns[10].HeaderText = dataGridView.Columns[10].Name = "ยอดก่อนภาษี";
            dataGridView.Columns[11].HeaderText = dataGridView.Columns[11].Name = "ภาษี 7%";
            dataGridView.Columns[12].HeaderText = dataGridView.Columns[12].Name = "เจ้าหน้าที่ขาออก";

            int intNo = dataGridView.Rows.Count - 1;

            int intSumR = 0;
            int intSumD = 0;
            int intSumT = 0;
            double intSumBV = 0;
            double intSumV = 0;

            for (int i = 0; i < intNo; i++)
            {
                try
                {
                    int intT = Convert.ToInt32(dataGridView[6, i].Value);
                    int intD = Convert.ToInt32(dataGridView[7, i].Value);
                    int intR = intT + intD;
                    int intID = Convert.ToInt32(dataGridView[1, i].Value);
                    dataGridView[1, i].Value = AppGlobalVariables.CarTypesById[intID];
                    intID = Convert.ToInt32(dataGridView[4, i].Value);
                    if (intID == 0)
                        dataGridView[4, i].Value = "";
                    else
                        dataGridView[4, i].Value = AppGlobalVariables.UsersById[intID];
                    intID = Convert.ToInt32(dataGridView[8, i].Value);
                    if (intID == 0)
                        dataGridView[12, i].Value = "";
                    else
                        dataGridView[12, i].Value = AppGlobalVariables.UsersById[intID];
                    DateTime dti = DateTime.Parse(dataGridView[3, i].Value.ToString());
                    DateTime dto = DateTime.Parse(dataGridView[5, i].Value.ToString());
                    TimeSpan diffTime = dto - dti;
                    int intHour = diffTime.Hours;
                    if (diffTime.Days > 0)
                        intHour += diffTime.Days * 24;
                    dataGridView[6, i].Value = intHour.ToString() + "." + diffTime.Minutes.ToString();
                    dataGridView[7, i].Value = intR.ToString("#0.00");
                    dataGridView[8, i].Value = intD.ToString("#0.00");
                    dataGridView[9, i].Value = intT.ToString("#0.00");
                    float floT = (float)intT;
                    double floBV = (floT * 100) / 107;
                    floBV = Math.Round(floBV, 2);
                    double floV = floT - floBV;
                    dataGridView[10, i].Value = floBV.ToString("#0.00");
                    dataGridView[11, i].Value = floV.ToString("#0.00");
                    intSumR += intR;
                    intSumD += intD;
                    intSumT += intT;
                    intSumBV += floBV;
                    intSumV += floV;
                }
                catch (Exception) { }
            }

            dataGridView[3, intNo].Value = "จำนวนรถ";
            dataGridView[4, intNo].Value = intNo.ToString() + " คัน";
            dataGridView[6, intNo].Value = "ยอดรวม";
            dataGridView[7, intNo].Value = intSumR.ToString("#0.00");
            dataGridView[8, intNo].Value = intSumD.ToString("#0.00");
            dataGridView[9, intNo].Value = intSumT.ToString("#0.00");
            dataGridView[10, intNo].Value = intSumBV.ToString("#0.00");
            dataGridView[11, intNo].Value = intSumV.ToString("#0.00");

            int totalReceived = intSumR;
            int totalDiscount = intSumD;
            int totalAmount = intSumT;
            double totalBeforeVat = intSumBV;
            double totalVat = intSumV;
        }
        #endregion
    }
}