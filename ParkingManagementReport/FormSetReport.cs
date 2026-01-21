using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ParkingManagementReport.Common;
using ParkingManagementReport.Utilities.Database;

namespace ParkingManagementReport
{
    public partial class FormSetReport : Form
    {
        public FormSetReport()
        {
            InitializeComponent();
        }

        private void FormSetReport_Load(object sender, EventArgs e)
        {
            txtFatPay.Enabled = false;
            txtFinishTime.Enabled = false;
            txtLossCard.Enabled = false;
            txtMinuteToHour.Enabled = false;
            txtMoreOne.Enabled = false;
            txtPayHour.Enabled = false;
            txtPayMinute.Enabled = false;
            txtStartTime.Enabled = false;
            txtDayWeek.Enabled = false; //Mac 2019/05/27

            string sql = "SELECT id as ลำดับที่, name as ชื่อโปรโมชั่น,"
                + " minute as นาทีส่วนลด, price as 'ราคาส่วนเกินต่อ 1 ชั่วโมง(บาท)'"
                + " FROM promotion";
            DataTable dt = DbController.LoadData(sql);
            dataGridView2.DataSource = dt;
            int widthColumn = dataGridView2.Width / dataGridView2.Columns.Count;
            for (int i = 0; i < dataGridView2.Columns.Count; i++)
            {
                dataGridView2.Columns[i].Width = widthColumn - 15;
            }
        }
        string reportId = "";

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label1.Text = "";
            add = false;
            txtFatPay.Enabled = false;
            txtFinishTime.Enabled = false;
            txtLossCard.Enabled = false;
            txtMinuteToHour.Enabled = false;
            txtMoreOne.Enabled = false;
            txtPayHour.Enabled = false;
            txtPayMinute.Enabled = false;
            txtDayWeek.Enabled = false; //Mac 2019/05/27
            //txtStartTime.Text = txtFinishTime.Text = txtPayHour.Text = txtPayMinute.Text = txtLossCard.Text = txtMoreOne.Text = txtFatPay.Text = txtMinuteToHour.Text = "";
            txtStartTime.Text = txtFinishTime.Text = txtPayHour.Text = txtPayMinute.Text = txtLossCard.Text = txtMoreOne.Text = txtFatPay.Text = txtMinuteToHour.Text = txtDayWeek.Text = ""; //Mac 2019/05/27
            if (e.RowIndex > -1)
            {
                reportId = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                string sql = "select no as ลำดับ, StartTime as นาทีเริ่ม, "
                        + " FinishTime as นาทีสิ้นสุด,  "
                        + " PayMinute as 'นาทีละ(บาท)', "
                        + " PayHour as 'ชั่วโมงละ(บาท)',  "
                        + " MinuteToHour as 'ปัดเศษ(นาที)', "
                        + " FlatPay as 'เหมาจ่าย(บาท)', "
                        + " MoreOne as 'จอดเกินกำหนด(บาท)', "
                        + " LoseCard as 'บัตรหาย(บาท)' ";

                if (Configs.Reports.ReportProsetPriceDayWeek || Configs.UseDayWeek.Trim().Length > 0) //Mac 2022/07/26
                    sql += ", DayWeek as 'วันในสัปดาห์' ";

                sql += " from prosetprice ";
                sql += " WHERE PromotionID = " + reportId;

                DataTable dt = DbController.LoadData(sql);

                if (dt == null || dt.Columns.Count == 0)
                {
                    dataGridView1.DataSource = null;
                    return;
                }

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Count > 0)
                {
                    int widthColumn = dataGridView1.Width / dataGridView1.Columns.Count;

                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {
                        col.Width = widthColumn - 3;
                    }
                }
            }
        }

        bool edit = false;
        string no = "";
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            add = false;
            txtFatPay.Enabled = false;
            txtFinishTime.Enabled = false;
            txtLossCard.Enabled = false;
            txtMinuteToHour.Enabled = false;
            txtMoreOne.Enabled = false;
            txtPayHour.Enabled = false;
            txtPayMinute.Enabled = false;
            txtDayWeek.Enabled = false; //Mac 2019/05/27
            //txtStartTime.Text = txtFinishTime.Text = txtPayHour.Text = txtPayMinute.Text = txtLossCard.Text = txtMoreOne.Text = txtFatPay.Text = txtMinuteToHour.Text = "";
            txtStartTime.Text = txtFinishTime.Text = txtPayHour.Text = txtPayMinute.Text = txtLossCard.Text = txtMoreOne.Text = txtFatPay.Text = txtMinuteToHour.Text = txtDayWeek.Text = ""; //Mac 2019/05/27
            if (e.RowIndex > -1 && dataGridView1.Rows.Count > 0)
            {
                edit = true;
                txtFatPay.Enabled = true;
                txtFinishTime.Enabled = true;
                txtLossCard.Enabled = true;
                txtMinuteToHour.Enabled = true;
                txtMoreOne.Enabled = true;
                txtPayHour.Enabled = true;
                txtPayMinute.Enabled = true;
                txtDayWeek.Enabled = true; //Mac 2019/05/27
                no = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtStartTime.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtFinishTime.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtPayMinute.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtPayHour.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtMinuteToHour.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtFatPay.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                txtMoreOne.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                txtLossCard.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                //if (Configs.Reports.ReportProsetPriceDayWeek) //Mac 2019/05/27
                if (Configs.Reports.ReportProsetPriceDayWeek || Configs.UseDayWeek.Trim().Length > 0) //Mac 2022/07/26
                    txtDayWeek.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            }
            label1.Text = "แก้ไขข้อมูล";
            label1.ForeColor = Color.Red;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int n1, n2;
            n1 = Int32.Parse(txtStartTime.Text);
            n2 = Int32.Parse(txtFinishTime.Text);
            if (n2 <= n1)
            {
                MessageBox.Show("เวลาสิ้นสุดต้องมากกว่าเวลาเริ่มต้น!");
                return;
            }

            if (edit)
            {
                string sql = "update prosetprice SET ";
                sql += "StartTime = '" + txtStartTime.Text + "',";
                sql += "FinishTime = '" + txtFinishTime.Text + "',";
                sql += "PayHour = '" + txtPayHour.Text + "',";
                sql += "PayMinute = '" + txtPayMinute.Text + "',";
                sql += "LoseCard = '" + txtLossCard.Text + "',";
                sql += "MoreOne = '" + txtMoreOne.Text + "',";
                sql += "FlatPay = '" + txtFatPay.Text + "',";
                sql += "MinuteToHour = '" + txtMinuteToHour.Text + "' ";
                //if (Configs.Reports.ReportProsetPriceDayWeek) //Mac 2019/05/27
                if (Configs.Reports.ReportProsetPriceDayWeek || Configs.UseDayWeek.Trim().Length > 0) //Mac 2022/07/26
                    sql += ", DayWeek = '" + txtDayWeek.Text + "' ";
                sql += " WHERE PromotionID = " + reportId;
                sql += " AND no = " + no;
                if (no != "")
                {
                    if (DbController.SaveData(sql) == "")
                    {
                        if (txtFinishTime.Text != "1440") //Mac 2019/05/27
                        {
                            sql = "update prosetprice SET ";
                            sql += "StartTime = '" + (Int32.Parse(txtFinishTime.Text) + 1) + "'";
                            sql += " WHERE PromotionID = " + reportId;
                            sql += " AND no = " + (Int32.Parse(no) + 1);
                            DbController.SaveData(sql);
                        }

                        /*txtStartTime.Text = txtFinishTime.Text = txtPayHour.Text = txtPayMinute.Text = txtLossCard.Text
                            = txtMoreOne.Text = txtFatPay.Text = txtMinuteToHour.Text = "";*/
                        txtStartTime.Text = txtFinishTime.Text = txtPayHour.Text = txtPayMinute.Text = txtLossCard.Text
                            = txtMoreOne.Text = txtFatPay.Text = txtMinuteToHour.Text = txtDayWeek.Text = ""; //Mac 2019/05/27
                        no = "";
                        //////////////////////////////

                        MessageBox.Show("บันทึกสำเร็จ");
                    }
                    else
                    {
                        MessageBox.Show("บันทึกไม่สำเร็จ");
                    }
                }
            }//End if Edit
            if (add)
            {
                /*string sql = "INSERT INTO prosetprice (PromotionID,no,StartTime,FinishTime,PayMinute,PayHour,MinuteToHour,FlatPay,MoreOne,LoseCard)VALUES ";
                sql += "(";*/
                //Mac 2019/05/27
                string sql = "INSERT INTO prosetprice (PromotionID,no,StartTime,FinishTime,PayMinute,PayHour,MinuteToHour,FlatPay,MoreOne,LoseCard";
                //if (Configs.Reports.ReportProsetPriceDayWeek)
                if (Configs.Reports.ReportProsetPriceDayWeek || Configs.UseDayWeek.Trim().Length > 0) //Mac 2022/07/26
                    sql += ", DayWeek";
                sql += ")VALUES (";
                sql += "'" + reportId + "',";
                sql += "'" + no + "',";
                sql += "'" + txtStartTime.Text + "',";
                sql += "'" + txtFinishTime.Text + "',";
                sql += "'" + txtPayMinute.Text + "',";
                sql += "'" + txtPayHour.Text + "',";
                sql += "'" + txtMinuteToHour.Text + "',";
                sql += "'" + txtFatPay.Text + "',";
                sql += "'" + txtMoreOne.Text + "',";
                sql += "'" + txtLossCard.Text + "'";
                //if (Configs.Reports.ReportProsetPriceDayWeek) //Mac 2019/05/27
                if (Configs.Reports.ReportProsetPriceDayWeek || Configs.UseDayWeek.Trim().Length > 0) //Mac 2022/07/26
                    sql += ", '" + txtDayWeek.Text + "'";
                sql += ")";

                no = "";
                if (DbController.SaveData(sql) == "")
                {
                    MessageBox.Show("บันทึกสำเร็จ");
                }
                else MessageBox.Show("บันทึกไม่สำเร็จ");

            }//Endif Add


            string sqld = "select no as ลำดับ, StartTime as นาทีเริ่ม, "
                   + " FinishTime as นาทีสิ้นสุด,  "
                   + " PayMinute as 'นาทีละ(บาท)', "
                   + " PayHour as 'ชั่วโมงละ(บาท)',  "
                   + " MinuteToHour as 'ปัดเศษ(นาที)', "
                   + " FlatPay as 'เหมาจ่าย(บาท)', "
                   + " MoreOne as 'จอดเกินกำหนด(บาท)', "
                   + " LoseCard as 'บัตรหาย(บาท)' ";
            /*+ " from prosetprice "
            + " WHERE PromotionID = " + reportId;*/
            //if (Configs.Reports.ReportProsetPriceDayWeek) //Mac 2019/05/27
            if (Configs.Reports.ReportProsetPriceDayWeek || Configs.UseDayWeek.Trim().Length > 0) //Mac 2022/07/26
                sqld += ", DayWeek as 'วันในสัปดาห์' ";

            sqld += " from prosetprice ";
            sqld += " WHERE PromotionID = " + reportId;

            DataTable dt = DbController.LoadData(sqld);

            dataGridView1.DataSource = dt;
            int widthColumn = dataGridView1.Width / dataGridView1.Columns.Count;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = widthColumn - 3;
            }
            //////////////////////////////
            txtFatPay.Enabled = false;
            txtFinishTime.Enabled = false;
            txtLossCard.Enabled = false;
            txtMinuteToHour.Enabled = false;
            txtMoreOne.Enabled = false;
            txtPayHour.Enabled = false;
            txtPayMinute.Enabled = false;
            txtDayWeek.Enabled = false; //Mac 2019/05/27

            edit = false;
            add = false;
        }
        bool add = false;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (reportId.Trim() != "")
            {
                add = true;
                /*txtStartTime.Text = txtFinishTime.Text = txtPayHour.Text = txtPayMinute.Text = txtLossCard.Text
                                = txtMoreOne.Text = txtFatPay.Text = txtMinuteToHour.Text = "";*/
                txtStartTime.Text = txtFinishTime.Text = txtPayHour.Text = txtPayMinute.Text = txtLossCard.Text
                                = txtMoreOne.Text = txtFatPay.Text = txtMinuteToHour.Text = txtDayWeek.Text = ""; //Mac 2019/05/27
                no = "";
                string noAdd = "";
                label1.Text = "เพิ่มข้อมูล";
                label1.ForeColor = Color.Green;
                string sql = "SELECT MAX(no) FROM prosetprice where promotionId = " + reportId;
                DataTable dt = DbController.LoadData(sql);
                noAdd = dt.Rows[0].ItemArray[0].ToString();
                txtFatPay.Enabled = true;
                txtFinishTime.Enabled = true;
                txtLossCard.Enabled = true;
                txtMinuteToHour.Enabled = true;
                txtMoreOne.Enabled = true;
                txtPayHour.Enabled = true;
                txtPayMinute.Enabled = true;
                txtDayWeek.Enabled = true; //Mac 2019/05/27
                if (noAdd.Trim() == "" || noAdd == null)
                {
                    txtStartTime.Text = "1";
                    no = "1";
                    txtFinishTime.Text = "2";
                }
                else
                {
                    sql = "SELECT FinishTime FROM prosetprice WHERE promotionId = " + reportId + " AND no = " + noAdd;
                    dt = DbController.LoadData(sql);
                    txtStartTime.Text = (Int32.Parse(dt.Rows[0].ItemArray[0].ToString()) + 1).ToString();
                    txtFinishTime.Text = (Int32.Parse(dt.Rows[0].ItemArray[0].ToString()) + 2).ToString();
                    no = (Int32.Parse(noAdd) + 1).ToString();
                }
            }
            else
            {
                MessageBox.Show("กรุณาคลิกเลือกร้าน/บริษัท ด้านล่างก่อน");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
