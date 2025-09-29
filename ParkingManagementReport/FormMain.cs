using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using CrystalDecisions.CrystalReports.Engine;
using ParkingManagementReport.Common;
using ParkingManagementReport.Utilities;
using ParkingManagementReport.Utilities.Database;
using ParkingManagementReport.Utilities.Formatters;
using ParkingManagementReport.Utilities.Hardwares;

namespace ParkingManagementReport
{
    public partial class FormMain : Form
    {
        #region FIELDS
        TabPage tabUser;
        DataTable dtUser;
        DataTable dtName;

        MifareReader mfReader;
        string startDateTime, endDateTime;
        int selectedReportId;
        int intCase;
        int intCase162 = 0;
        int SumCalQuota109 = 0;
        int FlatRateM = 0;
        int FlatRateP = 0;
        int FlatRateX = 0;
        int totalReceived, totalDiscount, totalAmount, totalLoss, totalOver, totalPrice;
        double totalBeforeVat, totalVat;
        int dgvX, dgvY, dgvH;
        #endregion

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            ConfigsManager.LoadConfigsFromXml();

            if (!DbController.Connect(Configs.ServerIP, AppGlobalVariables.Database.Name))
            {
                MessageBox.Show("Can not connect database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FormLogin frmLogin = new FormLogin();
            frmLogin.ShowDialog();

            if (AppGlobalVariables.OperatingUser.Level > 2)
                optionBox.Visible = true;

            AppGlobalVariables.ParamsLookup = ConfigsManager.LoadParametersFromDatabase();
            ConfigsManager.SetConfigsParamsFromLookupData(AppGlobalVariables.ParamsLookup);

            ConfigsManager.LoadConfigsFromDb();

            InitializeHardwares();

            InitializeUIElements();

            /* FOR TEST กำหนดวัน ช่วงวัน ดึงข้อมูล
            StartDatePicker.Value = new DateTime(day: 01, month: 4, year: 2025);
            EndDatePicker.Value = new DateTime(day: 15, month: 4, year: 2025);
            */
        }

        #region INIT_SETTINGS
        private void InitializeUIElements()
        {
            InitializeComponents();

            SetInitialControlStates();

            LoadComboBoxData();

            ConfigureVisibilitySettings();
        }

        private void InitializeComponents()
        {
            InitializeTabControl();
            InitializeTimeControls();
            ConfigureCameraVisibility();
        }

        private void LoadComboBoxData()
        {
            LoadCarTypes();
            LoadUsers();
            LoadPromotions();
            LoadReports();
            LoadMemberGroupReport();

            CheckMemberUp2UTables();
            CheckGuardhouseConfiguration();
            LoadMemberSpecificData();

            PaymentChannelComboBox.Items.AddRange(new object[] {
            Constants.TextBased.All,
            Constants.TextBased.PaymentChannelPromptPay,
            Constants.TextBased.PaymentChannelTrueMoney,
            Constants.TextBased.PaymentChannelCash,
            Constants.TextBased.PaymentChannelEDC
            });

            MemberCardTypeComboBox.Items.AddRange(new object[] {
                Constants.TextBased.All,
                Constants.TextBased.MemberCardTypeWithPayment,
                Constants.TextBased.MemberCardTypeNonPayment});

            MemberProcessStateComboBox.Items.AddRange(new object[] {
                Constants.TextBased.All,
                Constants.TextBased.CreateNewMemberProcessState,
                Constants.TextBased.UpdateMemberProcessState
            });

            GuardhouseComboBox.Items.AddRange(new object[] {
                Constants.TextBased.All});

            PaymentStatusComboBox.Items.AddRange(new object[] {
                Constants.TextBased.All,
                Constants.TextBased.PaymentStatusPaid,
                Constants.TextBased.PaymentStatusUnPaid
            });
            PaymentStatusComboBox.Text = Constants.TextBased.All;

            /*MemberBirthMonthComboBox.Items.AddRange(new object[] {
            "ทั้งหมด",
            "ไม่มี",
            "มกราคม",
            "กุมภาพันธ์",
            "มีนาคม",
            "เมษายน",
            "พฤษภาคม",
            "มิถุนายน",
            "กรกฎาคม",
            "สิงหาคม",
            "กันยายน",
            "ตุลาคม",
            "พฤศจิกายน",
            "ธันวาคม"});*/
        }

        private void ConfigureVisibilitySettings()
        {
            // Configure camera visibility
            bool showSecondCamera = (Configs.IsVillage && Configs.Use2Camera) ||
                                  (Configs.Use2Camera && !string.IsNullOrWhiteSpace(Configs.IPIn3));
            pictureBox5.Visible = showSecondCamera;
            lbPic5.Visible = showSecondCamera;

            // Configure report condition visibility
            if (Configs.Reports.ReportSearchMemberGroup || Configs.Reports.UseReport24_2)
            {
                label17.Text = Constants.TextBased.Generic.MemberGroup;
                MemberTypeComboBox.Visible = true;
                label17.Visible = true;
            }

            // Configure member group visibility
            if (Configs.UseSettingNewMember)
            {
                MemberGroupMonthComboBox.Visible = true;
                PaymentStatusComboBox.Visible = true;
                AdditionalMemberInfoPanel.Visible = true;
            }
            else if ((Configs.UseMemberGroupPriceMonth || Configs.UseGroupPromotion) &&
                     Configs.ShowConditionMemberGroupPriceMonth)
            {
                MemberGroupMonthComboBox.Visible = true;
                PaymentStatusComboBox.Visible = true;
                MemberNameComboBox.Visible = true;
            }

            // Configure Up2U panel visibility
            if (Configs.NoPanelUp2U == "2")
            {
                UserUp2UPanel.Visible = false;
                StickerUp2UPanel.Visible = false;
                CarTypeUp2UPanel.Visible = false;
                MemberParkingUp2UPanel.Visible = true;
            }

            // Configure label visibility
            label19.Visible = Configs.UseSettingNewMember ||
                             (Configs.UseMemberGroupPriceMonth || Configs.UseGroupPromotion);
            label20.Visible = label19.Visible;
            label21.Visible = (Configs.UseMemberGroupPriceMonth || Configs.UseGroupPromotion) &&
                              Configs.ShowConditionMemberGroupPriceMonth;

            PromotionIdRangePanel.Visible = false;
        }

        private void CheckMemberUp2UTables()
        {
            var dt = DbController.LoadData(
                $"SELECT table_name FROM information_schema.tables " +
                $"WHERE table_schema = '{AppGlobalVariables.Database.Name}' " +
                $"AND table_name = 'member_up2u'");

            if (dt.Rows.Count > 0)
            {
                UserUp2UPanel.Visible = true;
                StickerUp2UPanel.Visible = true;
                CarTypeUp2UPanel.Visible = true;
            }
        }

        private void CheckGuardhouseConfiguration()
        {
            var dt = DbController.LoadData(
                $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
                $"WHERE TABLE_SCHEMA = '{AppGlobalVariables.Database.Name}' " +
                $"AND TABLE_NAME = 'guardhouse' " +
                $"AND COLUMN_NAME = 'guardhouse'");

            if (dt.Rows.Count > 0)
            {
                InitializeGuardhouseComboBox();
            }
        }

        private void InitializeGuardhouseComboBox()
        {
            GuardhouseComboBox.Items.Clear();
            GuardhouseComboBox.Items.Add(Constants.TextBased.All);

            var dt = DbController.LoadData("SELECT guardhouse FROM guardhouse ORDER BY id");
            foreach (DataRow row in dt.Rows)
            {
                GuardhouseComboBox.Items.Add(row[0]);
            }

            GuardhouseComboBox.Text = Constants.TextBased.All;
            label31.Visible = true;
            GuardhouseComboBox.Visible = true;
        }

        private void LoadMemberSpecificData()
        {
            if (Configs.UseSettingNewMember)
            {
                ConfigureNewMemberSettings();
            }
            else if (Configs.UseMemberGroupPriceMonth || Configs.UseGroupPromotion && Configs.ShowConditionMemberGroupPriceMonth)
            {
                ConfigureMemberGroupPriceSettings();
            }

            if (Configs.NoPanelUp2U == "2")
            {
                ConfigureUp2UPanelSettings();
            }
        }

        private void ConfigureNewMemberSettings()
        {
            MemberGroupMonthComboBox.Visible = true;
            label19.Text = "บริษัทผู้ถือบัตร";
            label19.Visible = true;
            PaymentStatusComboBox.Visible = true;
            label20.Text = "ประเภทบัตร";
            label20.Visible = true;
            AdditionalMemberInfoPanel.Visible = true;

            //CRUDManager.LoadComboBoxDataFromQuery(
            //    MemberGroupMonthComboBox,
            //    "SELECT store_name, store_id FROM m_store WHERE store_name IS NOT NULL AND LENGTH(TRIM(store_name)) > 0 ORDER BY store_id",
            //    AppGlobalVariables.MemberGroupMonthsToId);

            PaymentStatusComboBox.Items.Clear();
            PaymentStatusComboBox.Text = Constants.TextBased.All;

            //CRUDManager.LoadComboBoxDataFromQuery(
            //    PaymentStatusComboBox,
            //    "SELECT name, id FROM cardtype WHERE name IS NOT NULL AND LENGTH(TRIM(name)) > 0 ORDER BY id",
            //    AppGlobalVariables.MemberGroupsToId);

            ConfigsManager.LoadComboBoxDataFromQuery(
                MemberRenewalTypeComboBox,
                "SELECT name, id FROM renew_mem WHERE name IS NOT NULL AND LENGTH(TRIM(name)) > 0 ORDER BY id",
                AppGlobalVariables.RenewMemberGroupsToId);
        }

        private void ConfigureMemberGroupPriceSettings()
        {
            MemberGroupMonthComboBox.Visible = true;
            PaymentStatusComboBox.Visible = true;
            label19.Visible = true;
            label20.Visible = true;
            MemberNameComboBox.Visible = true;
            label21.Visible = true;

            ConfigsManager.LoadComboBoxDataFromQuery(
                MemberGroupMonthComboBox,
                "SELECT groupname,id FROM membergroupprice_month ORDER BY groupname",
                AppGlobalVariables.MemberGroupMonthsToId);

            ConfigsManager.LoadComboBoxDataFromQuery(
                MemberNameComboBox,
                "SELECT name FROM member ORDER BY name",
                null);
        }

        private void ConfigureUp2UPanelSettings()
        {
            UserUp2UPanel.Visible = false;
            StickerUp2UPanel.Visible = false;
            CarTypeUp2UPanel.Visible = false;
            MemberParkingUp2UPanel.Visible = true;

            ConfigsManager.LoadComboBoxDataFromQuery(
                MemberGroupComboBox,
                "SELECT memgroup FROM member_up2u GROUP BY memgroup ORDER BY memgroup",
                null);

            AppGlobalVariables.MemberStatusesLookup.Clear(); // Optional: ensures a clean state
            AppGlobalVariables.MemberStatusesLookup.Add(Constants.TextBased.All, Constants.TextBased.All);
            AppGlobalVariables.MemberStatusesLookup.Add(Constants.TextBased.MemberStatusActive, "Y");
            AppGlobalVariables.MemberStatusesLookup.Add(Constants.TextBased.MemberStatusCanceled, "C");
            AppGlobalVariables.MemberStatusesLookup.Add(Constants.TextBased.MemberStatusLossCard, "L");

            // Populate ComboBox from dictionary keys
            MemberStatusComboBox.Items.Clear();
            foreach (var statusKey in AppGlobalVariables.MemberStatusesLookup.Keys)
            {
                MemberStatusComboBox.Items.Add(statusKey);
            }

            MemberStatusComboBox.Text = Constants.TextBased.All;

            IgnoreExpirationDateCheckBox.Checked = true;
        }

        private void SetInitialControlStates()
        {
            if (Configs.UseMifare)
            {
                MifareCheckTimer.Enabled = true;
            }

            ExcelExportButton.Enabled = false;
            PdfExportButton.Enabled = false;
            UpdateReportButton.Visible = false;

            dgvX = ResultGridView.Location.X;
            dgvY = ResultGridView.Location.Y;
            dgvH = ResultGridView.Height;

            groupBox3.Visible = false;
            SetReportConditionButton.Visible = false;

            if (Configs.UsePDFOnly)
            {
                ExcelExportButton.Visible = false;
            }
        }

        private void InitializeTabControl()
        {
            tabUser = tabPage3;
            tabUser.Name = "tabUser";
            PrimaryTabControl.TabPages.Remove(tabPage3);
        }

        private void InitializeTimeControls()
        {
            StartTimePicker.Value = DateTime.Parse("00:00:00");
            EndTimePicker.Value = DateTime.Parse("23:59:59");
        }
        #endregion INIT_SETTINGS_END


        #region HARDWARES
        private void InitializeHardwares()
        {
            #region Mifare reader
            mfReader = new MifareReader(false);
            string strError = mfReader.InitializeMfReader();
            if (strError != "")
                MessageBox.Show(strError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            #endregion

        }
        #endregion HARDWARES_END


        #region PROCESS
        private void Display(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                return;

            DataTable dataTableFromQuery = DbController.LoadData(sql);

            ReportHeaderLabel.Text = AppGlobalVariables.Printings.Header = SetReportHeader().Replace("รายงานรายงาน", "รายงาน");

            ResultGridView.Location = new Point(dgvX, dgvY);
            ResultGridView.Height = dgvH;
            groupBox3.Visible = false;

            try
            {
                if (dataTableFromQuery.Rows.Count > 0)
                {
                    string startDateMonthYearWithFullMonthName = TextFormatters.ExtractDateMonthYearWithFullMonthName(StartDatePicker.Value);
                    string endDateMonthYearWithFullMonthName = TextFormatters.ExtractDateMonthYearWithFullMonthName(EndDatePicker.Value);
                    string reportRange = $@"'ตั้งแต่วันที่ {startDateMonthYearWithFullMonthName} ถึงวันที่ {endDateMonthYearWithFullMonthName}'";

                    if (selectedReportId == 11)
                    {
                        ResultGridView.DataSource = DataTableManager.ConvertTableType(dataTableFromQuery);
                        CaseReportGroupPrice();
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        ReportDocument rpt = new ReportDocument();
                        PrimaryCrystalReportViewer.ReportSource = null;
                        PrimaryCrystalReportViewer.Refresh();

                        foreach (DataGridViewColumn col in ResultGridView.Columns)
                        {
                            dataTableFromQuery.Columns.Add(col.HeaderText);
                        }
                        for (int i = 0; i < ResultGridView.Rows.Count - 1; i++)
                        {
                            DataRow dRow = dataTableFromQuery.NewRow();
                            for (int j = 0; j < ResultGridView.Columns.Count; j++)
                            {
                                dRow[j] = ResultGridView.Rows[i].Cells[j].Value;
                            }
                            dataTableFromQuery.Rows.Add(dRow);
                        }

                        string p0, p1, p2, p3, p4, p5, p6;
                        p0 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[4].Value.ToString();
                        p1 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[6].Value.ToString();
                        p2 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[7].Value.ToString();
                        p3 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[8].Value.ToString();
                        p4 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[9].Value.ToString();
                        p5 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[10].Value.ToString();
                        p6 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[11].Value.ToString();

                        rpt.Load(path + "\\CrystalReports\\Report12.rpt");
                        rpt.SetDataSource(dataTableFromQuery);
                        rpt.DataDefinition.FormulaFields["ReportName"].Text = $"'{ReportHeaderLabel.Text}'";
                        rpt.DataDefinition.FormulaFields["CompanyName"].Text = $"'{AppGlobalVariables.Printings.Company1.Trim()}'";
                        rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + p0 + "'";
                        rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + p1 + "'";
                        rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + p2 + "'";
                        rpt.DataDefinition.FormulaFields["Pa3"].Text = "'" + p3 + "'";
                        rpt.DataDefinition.FormulaFields["Pa4"].Text = "'" + p4 + "'";
                        rpt.DataDefinition.FormulaFields["Pa5"].Text = "'" + p5 + "'";
                        rpt.DataDefinition.FormulaFields["Pa6"].Text = "'" + p6 + "'";

                        PrimaryCrystalReportViewer.ReportSource = rpt;
                        PrimaryCrystalReportViewer.Refresh();

                        ////

                        PdfExportButton.Enabled = true;
                        ExcelExportButton.Enabled = true;
                        return;
                    }

                    if (selectedReportId == 12 || selectedReportId == 13 || selectedReportId == 14)
                    {
                        ResultGridView.DataSource = ConvertTableType(dataTableFromQuery);
                        if (selectedReportId == 13 && Configs.Reports.UseReport13_12) //Mac 2023/08/09
                            CaseReportPricePromotion13_12();
                        else
                            CaseReportPricePromotion();

                        //////////////
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        ReportDocument rpt = new ReportDocument();
                        DataTable dtMap = DbController.LoadData("Select value FROM param Where name = 'com1' or name = 'add1' or name = 'add2' or name = 'tax'");
                        PrimaryCrystalReportViewer.ReportSource = null;
                        PrimaryCrystalReportViewer.Refresh();

                        dt = new DataTable();
                        foreach (DataGridViewColumn col in ResultGridView.Columns)
                        {
                            dataTableFromQuery.Columns.Add(col.HeaderText);
                        }

                        for (int i = 0; i < ResultGridView.Rows.Count - 1; i++)
                        {
                            DataRow dRow = dataTableFromQuery.NewRow();
                            for (int j = 0; j < ResultGridView.Columns.Count; j++)
                            {
                                dRow[j] = ResultGridView.Rows[i].Cells[j].Value;
                            }
                            dataTableFromQuery.Rows.Add(dRow);
                        }

                        string p0, p1, p2, p3, p4;
                        if (selectedReportId == 13 && Configs.Reports.UseReport13_12) //Mac 2023/08/09
                        {
                            p0 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[7].Value.ToString();
                            p1 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[12].Value.ToString();
                            p2 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[13].Value.ToString();
                            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) p3 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[16].Value.ToString();
                            else p3 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[14].Value.ToString();
                            p4 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[15].Value.ToString();
                        }
                        else
                        {
                            p0 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[6].Value.ToString();
                            p1 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[11].Value.ToString();
                            p2 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[12].Value.ToString();
                            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) p3 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[15].Value.ToString();
                            else p3 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[13].Value.ToString();
                            p4 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[14].Value.ToString();
                        }


                        if (Configs.Reports.ReportNoRunning)
                        {
                            if (selectedReportId == 12)
                            {
                                if (Configs.UseMemo)
                                    rpt.Load(path + "\\CrystalReports\\Report13_2NoRunning.rpt");
                                else
                                {
                                    if (Configs.Reports.UseReport13_13)
                                        rpt.Load(path + "\\CrystalReports\\Report13_13NoRunning.rpt");
                                    else if (Configs.Reports.UseReport13_11)
                                        rpt.Load(path + "\\CrystalReports\\Report13_11NoRunning.rpt");
                                    else if (Configs.Reports.UseReport13_10)
                                        rpt.Load(path + "\\CrystalReports\\Report13_10NoRunning.rpt");
                                    else if (Configs.Reports.UseReport13logo)
                                        rpt.Load(path + "\\CrystalReports\\Report13logoNoRunning.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report13NoRunning.rpt");
                                }
                            }
                            else if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                            {
                                if (Configs.Reports.UseReport13_12)
                                {
                                    if (Configs.IsSwitch)
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report13_12_sw.rpt");
                                        rpt.DataDefinition.FormulaFields["Condition"].Text = "'ประจำวันที่ " + StartDatePicker.Value.ToString("dd/MM/yyyy") + " " + StartTimePicker.Value.ToLongTimeString() + " ถึงวันที่ " + EndDatePicker.Value.ToString("dd/MM/yyyy") + " " + EndTimePicker.Value.ToLongTimeString() + "'";
                                        rpt.DataDefinition.FormulaFields["ReportCon"].Text = "'เดือนภาษี" + StartDatePicker.Value.ToString(" MMMM ") + "ปี " + (StartDatePicker.Value.Year + 543) + "'";
                                        rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                                        Configs.IsSwitch = false;
                                    }
                                    else
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report13_12NoRunning.rpt");
                                        Configs.IsSwitch = true;
                                    }
                                }
                                else if (Configs.Reports.UseReport13_3)
                                    rpt.Load(path + "\\CrystalReports\\Report13_3NoRunning.rpt");
                                else if (Configs.Reports.UseReport13_1logo)
                                    rpt.Load(path + "\\CrystalReports\\Report13_1logoNoRunning.rpt");
                                else if (Configs.Reports.UseReport13_7)
                                    rpt.Load(path + "\\CrystalReports\\Report13_7NoRunning.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report13_1NoRunning.rpt");
                            }
                            else rpt.Load(path + "\\CrystalReports\\Report13NoRunning.rpt");
                        }
                        else
                        {
                            if (selectedReportId == 12)
                            {
                                if (Configs.UseMemo)
                                    rpt.Load(path + "\\CrystalReports\\Report13_2.rpt");
                                else
                                {
                                    if (Configs.Reports.UseReport13_13)
                                        rpt.Load(path + "\\CrystalReports\\Report13_13.rpt");
                                    else if (Configs.Reports.UseReport13_11)
                                        rpt.Load(path + "\\CrystalReports\\Report13_11.rpt");
                                    else if (Configs.Reports.UseReport13_10)
                                        rpt.Load(path + "\\CrystalReports\\Report13_10.rpt");
                                    else if (Configs.Reports.UseReport13logo)
                                        rpt.Load(path + "\\CrystalReports\\Report13logo.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report13.rpt");
                                }
                            }
                            else if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                            {
                                if (Configs.Reports.UseReport13_12)
                                {
                                    if (Configs.IsSwitch)
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report13_12_sw.rpt");
                                        rpt.DataDefinition.FormulaFields["Condition"].Text = "'ประจำวันที่ " + StartDatePicker.Value.ToString("dd/MM/yyyy") + " " + StartTimePicker.Value.ToLongTimeString() + " ถึงวันที่ " + EndDatePicker.Value.ToString("dd/MM/yyyy") + " " + EndTimePicker.Value.ToLongTimeString() + "'";
                                        rpt.DataDefinition.FormulaFields["ReportCon"].Text = "'เดือนภาษี" + StartDatePicker.Value.ToString(" MMMM ") + "ปี " + (StartDatePicker.Value.Year + 543) + "'";
                                        rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";

                                        Configs.IsSwitch = false;
                                    }
                                    else
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report13_12.rpt");
                                        Configs.IsSwitch = true;
                                    }
                                }
                                else if (Configs.Reports.UseReport13_3)
                                    rpt.Load(path + "\\CrystalReports\\Report13_3.rpt");
                                else if (Configs.Reports.UseReport13_1logo)
                                    rpt.Load(path + "\\CrystalReports\\Report13_1logo.rpt");
                                else if (Configs.Reports.UseReport13_7)
                                    rpt.Load(path + "\\CrystalReports\\Report13_7.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report13_1.rpt");
                            }
                            else rpt.Load(path + "\\CrystalReports\\Report13.rpt");
                        }

                        rpt.SetDataSource(dataTableFromQuery);

                        if (selectedReportId == 12)
                            ReportHeaderLabel.Text = ReportHeaderLabel.Text.Replace("แสดงโปรโมชั่น", "");

                        rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                        if (dtMap.Rows.Count > 0)
                        {
                            rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                            {
                                try
                                {
                                    rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                catch (Exception)
                                {
                                    rpt.DataDefinition.FormulaFields["Address1"].Text = "''";
                                    rpt.DataDefinition.FormulaFields["Address2"].Text = "''";
                                    rpt.DataDefinition.FormulaFields["TaxID"].Text = "''";
                                }
                            }

                            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                            {
                                if (Configs.Reports.UseReport13_12)
                                {
                                    rpt.DataDefinition.FormulaFields["Pa4"].Text = "'" + ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[14].Value.ToString() + "'";
                                    rpt.DataDefinition.FormulaFields["Pa5"].Text = "'" + ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[15].Value.ToString() + "'";
                                    if (Configs.Reports.UseReport13_3)
                                        rpt.DataDefinition.FormulaFields["Sender"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "'";
                                }
                                else
                                {
                                    rpt.DataDefinition.FormulaFields["Pa4"].Text = "'" + ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[13].Value.ToString() + "'";
                                    rpt.DataDefinition.FormulaFields["Pa5"].Text = "'" + ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[14].Value.ToString() + "'";
                                    if (Configs.Reports.UseReport13_3)
                                        rpt.DataDefinition.FormulaFields["Sender"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "'";
                                }
                            }

                            if (Configs.Reports.UseReport13logo)
                            {
                                rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                            }

                        }
                        PrimaryCrystalReportViewer.ReportSource = rpt;
                        PrimaryCrystalReportViewer.Refresh();

                        ResultGridView.Columns[5].Visible = false;

                        PdfExportButton.Enabled = true;
                        ExcelExportButton.Enabled = true;
                        return;
                    }

                    if (selectedReportId == 15)
                    {
                        PdfExportButton.Enabled = true;
                        ExcelExportButton.Enabled = true;
                        ResultGridView.DataSource = ConvertTableType(dataTableFromQuery);
                        ResultGridView.Columns[0].HeaderText = "E-Stamp";
                        ResultGridView.Columns[1].HeaderText = "ยอดรวม";
                        ResultGridView.Columns[0].Width = 160;
                        ResultGridView.Columns[1].Width = 100;
                        int intNo = ResultGridView.Rows.Count - 1;
                        int intSumEStamp = 0;

                        for (int i = 0; i < intNo; i++)
                        {
                            int intID = Convert.ToInt32(ResultGridView[0, i].Value);
                            try
                            {
                                if (intID > 0)
                                {
                                    ResultGridView[0, i].Value = AppGlobalVariables.PromotionNamesById[intID];
                                    intSumEStamp += Convert.ToInt32(ResultGridView[1, i].Value);
                                }
                                else
                                    ResultGridView[0, i].Value = "";
                            }
                            catch (Exception)
                            {
                                ResultGridView[0, i].Value = "E-Stamp เลิกใช้";
                            }
                        }
                        ResultGridView[0, intNo].Value = "E-Stamp ทั้งหมด";
                        ResultGridView[1, intNo].Value = intSumEStamp.ToString();

                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        ReportDocument rpt = new ReportDocument();
                        DataTable dtMap = DbController.LoadData("Select value FROM param Where name = 'com1' or name = 'add1' or name = 'add2' or name = 'tax'");
                        PrimaryCrystalReportViewer.ReportSource = null;
                        PrimaryCrystalReportViewer.Refresh();

                        dt = new DataTable();
                        foreach (DataGridViewColumn col in ResultGridView.Columns)
                        {
                            dataTableFromQuery.Columns.Add(col.HeaderText);
                        }
                        if (PromotionComboBox.SelectedIndex > 0)
                        {
                            DataRow dRow = dataTableFromQuery.NewRow();
                            for (int j = 0; j < ResultGridView.Columns.Count; j++)
                            {
                                dRow[j] = ResultGridView.Rows[0].Cells[j].Value;
                            }
                            dataTableFromQuery.Rows.Add(dRow);
                        }
                        else
                        {
                            for (int i = 1; i < ResultGridView.Rows.Count - 1; i++)
                            {
                                DataRow dRow = dataTableFromQuery.NewRow();
                                for (int j = 0; j < ResultGridView.Columns.Count; j++)
                                {
                                    dRow[j] = ResultGridView.Rows[i].Cells[j].Value;
                                }
                                dataTableFromQuery.Rows.Add(dRow);
                            }
                        }
                        if (Configs.Reports.UseReport16logo)
                            rpt.Load(path + "\\CrystalReports\\Report16logo.rpt");
                        else
                            rpt.Load(path + "\\CrystalReports\\Report16.rpt");
                        rpt.SetDataSource(dataTableFromQuery);
                        rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                        if (dtMap.Rows.Count > 0)
                        {
                            rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                            rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[1].Value.ToString() + "'";
                            if (PromotionComboBox.SelectedIndex > 0)
                                rpt.DataDefinition.FormulaFields["Pa1"].Text = "'0'";
                            else
                                rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + ResultGridView.Rows[0].Cells[1].Value.ToString() + "'";

                            if (Configs.Reports.UseReport16logo)
                            {
                                rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                            }
                        }
                        PrimaryCrystalReportViewer.ReportSource = rpt;
                        PrimaryCrystalReportViewer.Refresh();

                        return;
                    }

                    PdfExportButton.Enabled = true;
                    ExcelExportButton.Enabled = true;

                    ResultGridView.DataSource = dataTableFromQuery;

                    ResultGridView.AutoResizeColumns();

                    for (int i = 0; i < ResultGridView.Columns.Count; i++)
                    {
                        ResultGridView.Columns[i].Width = ResultGridView.Columns[i].Width + 30;
                    }

                    if (Configs.VisitorFillDetail && (selectedReportId == 0 || selectedReportId == 8 || selectedReportId == 90))
                        ResultGridView.Columns[9].HeaderText = Constants.TextBased.Generic.ContactHeaderName;

                    if (selectedReportId == 1 || selectedReportId == 91)
                    {
                        int iVil = 0;
                        if (Configs.Use2Camera)
                        {
                            if (Configs.IsVillage && Configs.Use2Camera) iVil = 5;
                            if (Configs.NoPanelUp2U == "2") //Mac 2017/03/13
                                iVil += 4;
                            //if (Configs.Reports.UseReport1_6) //Mac 2018/11/29
                            if (Configs.Reports.UseReport1_6 || Configs.Reports.UseReport1_8) //Mac 2024/07/25
                                iVil = 1;
                            ResultGridView.Columns[9 + iVil].Visible = false;
                            ResultGridView.Columns[10 + iVil].Visible = false;
                            ResultGridView.Columns[11 + iVil].Visible = false;
                            ResultGridView.Columns[12 + iVil].Visible = false;
                            if (Configs.IsVillage && Configs.Use2Camera)
                                ResultGridView.Columns[13 + iVil].Visible = false;
                            else if (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0) //Mac 2015/02/04
                                ResultGridView.Columns[13 + iVil].Visible = false;
                            ResultGridView.Location = new Point(ResultGridView.Location.X, ResultGridView.Location.Y + 150);
                            ResultGridView.Height = ResultGridView.Height - 150;
                            groupBox3.Visible = true;
                            lbPic1.Text = "รูปคนขับขาเข้า";
                            lbPic2.Text = "รูปทะเบียนขาเข้า";
                            lbPic3.Visible = true;
                            lbPic4.Visible = true;
                            pictureBox3.Visible = true;
                            pictureBox4.Visible = true;
                            if ((Configs.IsVillage && Configs.Use2Camera) || (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0)) //Mac 2015/02/04
                            {
                                pictureBox5.Visible = true;
                                lbPic5.Visible = true;
                            }
                        }
                        else
                        {
                            ResultGridView.Location = new Point(ResultGridView.Location.X, ResultGridView.Location.Y + 150);
                            ResultGridView.Height = ResultGridView.Height - 150;
                            groupBox3.Visible = true;
                            lbPic1.Text = "รูปขาเข้า";
                            lbPic2.Text = "รูปขาออก";
                            pictureBox1.Visible = true;
                            pictureBox2.Visible = true;
                            pictureBox3.Visible = false;
                            pictureBox4.Visible = false;
                            pictureBox5.Visible = false;
                            ResultGridView.Columns[9].Visible = false;
                            ResultGridView.Columns[10].Visible = false;
                        }
                    }

                    if (selectedReportId == 7)
                    {
                        ResultGridView.Columns[4].Visible = false;
                        try
                        {
                            ResultGridView.Columns[5].Visible = false;
                        }
                        catch (Exception) { }

                        ResultGridView.Location = new Point(ResultGridView.Location.X, ResultGridView.Location.Y + 150);
                        ResultGridView.Height = ResultGridView.Height - 150;
                        groupBox3.Visible = true;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;
                        if (Configs.IsVillage && Configs.Use2Camera)
                        {
                            pictureBox5.Visible = false;
                            lbPic5.Visible = false;
                        }
                        lbPic1.Text = "รูปคนขับ";
                        lbPic2.Text = "รูปทะเบียน";
                        lbPic3.Visible = false;
                        lbPic4.Visible = false;
                    }

                    if (selectedReportId == 31 || selectedReportId == 93) //Mac 2020/10/26
                    {
                        if (Configs.NoPanelUp2U == "2") //Mac 2017/03/13
                        {
                            ResultGridView.Columns[9].Visible = false;
                            try
                            {
                                ResultGridView.Columns[10].Visible = false;
                            }
                            catch (Exception) { }
                        }
                        else
                        {
                            ResultGridView.Columns[5].Visible = false;
                            try
                            {
                                ResultGridView.Columns[6].Visible = false;
                            }
                            catch (Exception) { }
                        }

                        ResultGridView.Location = new Point(ResultGridView.Location.X, ResultGridView.Location.Y + 150);
                        ResultGridView.Height = ResultGridView.Height - 150;
                        groupBox3.Visible = true;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;
                        if (Configs.IsVillage && Configs.Use2Camera)
                        {
                            pictureBox5.Visible = false;
                            lbPic5.Visible = false;
                        }
                        lbPic1.Text = "รูปคนขับ";
                        lbPic2.Text = "รูปทะเบียน";
                        lbPic3.Visible = false;
                        lbPic4.Visible = false;
                    }

                    try
                    {
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        string fullReportPath;
                        ReportDocument rpt = new ReportDocument();
                        PrimaryCrystalReportViewer.ReportSource = null;
                        PrimaryCrystalReportViewer.Refresh();

                        switch (selectedReportId)
                        {
                            case 1:
                                fullReportPath = CrystalReportHelper.GetFullReportFilePath(selectedReportId);
                                rpt.Load(fullReportPath);
                                rpt.SetDataSource(dataTableFromQuery);

                                if (Configs.VisitorFillDetail && (selectedReportId == 0 || selectedReportId == 90))
                                {
                                    TextObject txtReportCol;
                                    txtReportCol = rpt.ReportDefinition.ReportObjects["text21"] as TextObject;
                                    txtReportCol.Text = Constants.TextBased.Generic.ContactHeaderName;
                                }

                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 2:
                                DataTable dataTable2 = new DataTable("dataTable2");
                                dataTable2.Columns.Add(new DataColumn("ลำดับ", typeof(string)));
                                dataTable2.Columns.Add(new DataColumn("ประเภท", typeof(string)));
                                dataTable2.Columns.Add(new DataColumn("ทะเบียน", typeof(string)));
                                if (Configs.IsVillage && Configs.Use2Camera)
                                {
                                    dataTable2.Columns.Add(new DataColumn("ชื่อผู้มาติดต่อ", typeof(string)));
                                    dataTable2.Columns.Add(new DataColumn("ประเภทบัตร", typeof(string)));
                                    dataTable2.Columns.Add(new DataColumn("เบอร์โทรศัพท์", typeof(string)));
                                    dataTable2.Columns.Add(new DataColumn("ติดต่อ", typeof(string)));
                                    dataTable2.Columns.Add(new DataColumn("ที่อยู่", typeof(string)));
                                }
                                dataTable2.Columns.Add(new DataColumn("เวลาเข้า", typeof(string)));
                                dataTable2.Columns.Add(new DataColumn("เจ้าหน้าที่ขาเข้า", typeof(string)));
                                dataTable2.Columns.Add(new DataColumn("เวลาออก", typeof(string)));
                                dataTable2.Columns.Add(new DataColumn("รายได้", typeof(string)));
                                dataTable2.Columns.Add(new DataColumn("ส่วนลด", typeof(string)));
                                dataTable2.Columns.Add(new DataColumn("เจ้าหน้าที่ขาออก", typeof(string)));
                                dataTable2.Columns.Add(new DataColumn("il", typeof(System.Byte[]))); // เข้า-ทะเบียน
                                dataTable2.Columns.Add(new DataColumn("ol", typeof(System.Byte[]))); // ออก-ทะเบียน
                                dataTable2.Columns.Add(new DataColumn("iv", typeof(System.Byte[]))); // เข้า-หน้าคน
                                dataTable2.Columns.Add(new DataColumn("ov", typeof(System.Byte[]))); // ออก-หน้าคน
                                if (Configs.Reports.UseReport1_6)
                                    dataTable2.Columns.Add(new DataColumn("เลขที่บัตร", typeof(string)));
                                else if (Configs.Reports.UseReport1_8 || Configs.Reports.UseReport2_4)
                                    dataTable2.Columns.Add(new DataColumn("ผู้ถือบัตร", typeof(string)));
                                if (Configs.Reports.UseReport2_4)
                                    dataTable2.Columns.Add(new DataColumn("ชม.จอด", typeof(string)));

                                if (Configs.IsVillage && Configs.Use2Camera)
                                    dataTable2.Columns.Add(new DataColumn("vi", typeof(System.Byte[])));
                                else if (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0)
                                    dataTable2.Columns.Add(new DataColumn("io", typeof(System.Byte[])));
                                int i = 0;
                                for (i = 0; i < dataTableFromQuery.Rows.Count; i++)
                                {
                                    try
                                    {
                                        DataRow dataRow = dataTable2.NewRow();
                                        dataRow["ลำดับ"] = dataTableFromQuery.Rows[i]["ลำดับ"];
                                        dataRow["ประเภท"] = dataTableFromQuery.Rows[i]["ประเภท"];
                                        dataRow["ทะเบียน"] = dataTableFromQuery.Rows[i]["ทะเบียน"];
                                        if (Configs.Reports.UseReport1_6)
                                            dataRow["เลขที่บัตร"] = dataTableFromQuery.Rows[i]["เลขที่บัตร"];
                                        else if (Configs.Reports.UseReport1_8 || Configs.Reports.UseReport2_4)
                                            dataRow["ผู้ถือบัตร"] = dataTableFromQuery.Rows[i]["ผู้ถือบัตร"];
                                        if (Configs.Reports.UseReport2_4)
                                            dataRow["ชม.จอด"] = dataTableFromQuery.Rows[i]["ชม.จอด"];
                                        dataRow["เวลาเข้า"] = dataTableFromQuery.Rows[i]["เวลาเข้า"];
                                        dataRow["เจ้าหน้าที่ขาเข้า"] = dataTableFromQuery.Rows[i]["เจ้าหน้าที่ขาเข้า"];
                                        dataRow["เวลาออก"] = dataTableFromQuery.Rows[i]["เวลาออก"];
                                        dataRow["รายได้"] = dataTableFromQuery.Rows[i]["รายได้"];
                                        dataRow["ส่วนลด"] = dataTableFromQuery.Rows[i]["ส่วนลด"];
                                        dataRow["เจ้าหน้าที่ขาออก"] = dataTableFromQuery.Rows[i]["เจ้าหน้าที่ขาออก"];
                                        if (Configs.IsVillage && Configs.Use2Camera)
                                        {
                                            dataRow["ชื่อผู้มาติดต่อ"] = dataTableFromQuery.Rows[i]["ชื่อผู้มาติดต่อ"];
                                            dataRow["ประเภทบัตร"] = dataTableFromQuery.Rows[i]["ประเภทบัตร"];
                                            dataRow["เบอร์โทรศัพท์"] = dataTableFromQuery.Rows[i]["เบอร์โทรศัพท์"];
                                            dataRow["ติดต่อ"] = dataTableFromQuery.Rows[i]["ติดต่อ"];
                                            dataRow["ที่อยู่"] = dataTableFromQuery.Rows[i]["ที่อยู่"];
                                        }

                                        ImagesManager.AssignFileBytesToDataRow(dataRow: dataRow, columnName: "il", filePath: dataTableFromQuery?.Rows[i]?["il"].ToString());
                                        ImagesManager.AssignFileBytesToDataRow(dataRow: dataRow, columnName: "ol", filePath: dataTableFromQuery?.Rows[i]?["ol"].ToString());
                                        ImagesManager.AssignFileBytesToDataRow(dataRow: dataRow, columnName: "iv", filePath: dataTableFromQuery?.Rows[i]?["iv"].ToString());
                                        ImagesManager.AssignFileBytesToDataRow(dataRow: dataRow, columnName: "ov", filePath: dataTableFromQuery?.Rows[i]?["ov"].ToString());

                                        if (Configs.IsVillage && Configs.Use2Camera) 
                                        { 
                                            ImagesManager.AssignFileBytesToDataRow(dataRow: dataRow, columnName: "vi", filePath: dataTableFromQuery?.Rows[i]?["vi"].ToString()); 
                                        }
                                        if (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0) 
                                        { 
                                            ImagesManager.AssignFileBytesToDataRow(dataRow: dataRow, columnName: "io", filePath: dataTableFromQuery?.Rows[i]?["io"].ToString()); 
                                        }
                                        dataTable2.Rows.Add(dataRow);
                                    }
                                    catch { }
                                }

                                if (Configs.Reports.ReportNoRunning)
                                {
                                    if (Configs.Reports.UseReport2_4)
                                        rpt.Load(path + "\\CrystalReports\\Report2_4NoRunning.rpt");
                                    else
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report2NoRunning.rpt");
                                        if (Configs.IsVillage && Configs.Use2Camera) rpt.Load(path + "\\CrystalReports\\Report2_2NoRunning.rpt");
                                        else if (!Configs.Use2Camera) rpt.Load(path + "\\CrystalReports\\Report2_1NoRunning.rpt");
                                        else if (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0) rpt.Load(path + "\\CrystalReports\\Report2_3NoRunning.rpt");
                                        else if (Configs.Reports.UseReport2logo)
                                            rpt.Load(path + "\\CrystalReports\\Report2logoNoRunning.rpt");
                                    }
                                }
                                else
                                {
                                    if (Configs.Reports.UseReport2_4)
                                        rpt.Load(path + "\\CrystalReports\\Report2_4.rpt");
                                    else
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report2.rpt");
                                        if (Configs.IsVillage && Configs.Use2Camera) rpt.Load(path + "\\CrystalReports\\Report2_2.rpt");
                                        else if (!Configs.Use2Camera) rpt.Load(path + "\\CrystalReports\\Report2_1.rpt");
                                        else if (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0) rpt.Load(path + "\\CrystalReports\\Report2_3.rpt");
                                        else if (Configs.Reports.UseReport2logo)
                                            rpt.Load(path + "\\CrystalReports\\Report2logo.rpt");
                                    }
                                }
                                rpt.SetDataSource(dataTable2);
                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 3:
                                if (Configs.Reports.UseReport3_1)
                                    rpt.Load(path + "\\CrystalReports\\Report3_1.rpt");
                                else if (Configs.Reports.UseReport3logo)
                                    rpt.Load(path + "\\CrystalReports\\Report3logo.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report3.rpt");

                                rpt.SetDataSource(dataTableFromQuery);
                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 4:
                                if (Configs.Reports.ReportNoRunning)
                                {
                                    if (Configs.Reports.UseReport4logo)
                                        rpt.Load(path + "\\CrystalReports\\Report4logoNoRunning.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report4NoRunning.rpt");
                                }
                                else
                                {
                                    if (Configs.Reports.UseReport4logo) 
                                        rpt.Load(path + "\\CrystalReports\\Report4logo.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report4.rpt");
                                }
                                rpt.SetDataSource(dataTableFromQuery);
                                
                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 5:
                                if (Configs.Reports.ReportNoRunning)
                                {
                                    if (Configs.UseNameOnCard)
                                        rpt.Load(path + "\\CrystalReports\\Report5_5NoRunning.rpt");
                                    else if (Configs.Reports.UseReport5_1 || Configs.IsVillage)
                                        rpt.Load(path + "\\CrystalReports\\Report5_1NoRunning.rpt");
                                    else if (Configs.Reports.UseReport5_2)
                                        rpt.Load(path + "\\CrystalReports\\Report5_2NoRunning.rpt");
                                    else if (Configs.Reports.UseReport5_3)
                                        rpt.Load(path + "\\CrystalReports\\Report5_3NoRunning.rpt");
                                    else if (Configs.Reports.UseReport5logo)
                                        rpt.Load(path + "\\CrystalReports\\Report5logoNoRunning.rpt");
                                    else if (Configs.Reports.UseReport5_4)
                                        rpt.Load(path + "\\CrystalReports\\Report5_4NoRunning.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report5NoRunning.rpt");
                                }
                                else
                                {
                                    if (Configs.UseNameOnCard)
                                        rpt.Load(path + "\\CrystalReports\\Report5_5.rpt");
                                    else if (Configs.Reports.UseReport5_1 || Configs.IsVillage)
                                        rpt.Load(path + "\\CrystalReports\\Report5_1.rpt");
                                    else if (Configs.Reports.UseReport5_2)
                                        rpt.Load(path + "\\CrystalReports\\Report5_2.rpt");
                                    else if (Configs.Reports.UseReport5_3)
                                        rpt.Load(path + "\\CrystalReports\\Report5_3.rpt");
                                    else if (Configs.Reports.UseReport5logo)
                                        rpt.Load(path + "\\CrystalReports\\Report5logo.rpt");
                                    else if (Configs.Reports.UseReport5_4)
                                        rpt.Load(path + "\\CrystalReports\\Report5_4.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report5.rpt");
                                }
                                rpt.SetDataSource(dataTableFromQuery);

                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;


                            case 7:
                                CreateReportLicense(dataTableFromQuery);
                                PdfExportButton.Enabled = true;
                                ExcelExportButton.Enabled = true;
                                break;

                            case 8:
                                DataTable Map8 = new DataTable("myMember");
                                Map8.Columns.Add(new DataColumn("เวลายก", typeof(string)));
                                Map8.Columns.Add(new DataColumn("พนักงาน", typeof(string)));
                                Map8.Columns.Add(new DataColumn("ประตู", typeof(string)));
                                Map8.Columns.Add(new DataColumn("บันทึก", typeof(string)));
                                Map8.Columns.Add(new DataColumn("picdiv", typeof(System.Byte[])));
                                Map8.Columns.Add(new DataColumn("piclic", typeof(System.Byte[])));

                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    DataRow dr8 = Map8.NewRow();
                                    dr8["เวลายก"] = dataTableFromQuery.Rows[j]["เวลายก"];
                                    dr8["พนักงาน"] = dataTableFromQuery.Rows[j]["พนักงาน"];
                                    dr8["ประตู"] = dataTableFromQuery.Rows[j]["ประตู"];
                                    try
                                    {
                                        dr8["บันทึก"] = dataTableFromQuery.Rows[j]["บันทึก"];
                                    }
                                    catch { }

                                    ImagesManager.AssignFileBytesToDataRow(dr8, "picdiv", dataTableFromQuery.Rows[j]["picdiv"].ToString());
                                  
                                    ImagesManager.AssignFileBytesToDataRow(dr8, "piclic", dataTableFromQuery.Rows[j]["piclic"].ToString());

                                    Map8.Rows.Add(dr8);
                                }
                                if (Configs.Reports.UseReport8logo) //Mac 2018/05/08
                                    rpt.Load(path + "\\CrystalReports\\Report8logo.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report8.rpt");
                                rpt.SetDataSource(Map8);
                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 9:
                                if (Configs.Reports.ReportNoRunning)
                                {
                                    if (Configs.IsVillage && Configs.Use2Camera) 
                                        rpt.Load(path + "\\CrystalReports\\Report1_1NoRunning.rpt");
                                    else if (Configs.IsVillage || Configs.VisitorFillDetail) 
                                        rpt.Load(path + "\\CrystalReports\\Report1_2NoRunning.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report1NoRunning.rpt");
                                }
                                else
                                {
                                    if (Configs.IsVillage && Configs.Use2Camera) 
                                        rpt.Load(path + "\\CrystalReports\\Report1_1.rpt");
                                    else if (Configs.IsVillage || Configs.VisitorFillDetail) 
                                        rpt.Load(path + "\\CrystalReports\\Report1_2.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report1.rpt");
                                }

                                rpt.SetDataSource(dataTableFromQuery);

                                if (Configs.VisitorFillDetail)
                                {
                                    TextObject txtReportCol;
                                    txtReportCol = rpt.ReportDefinition.ReportObjects["text21"] as TextObject;
                                    txtReportCol.Text = Constants.TextBased.Generic.ContactHeaderName;
                                }

                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);
                                rpt.DataDefinition.FormulaFields["PaCar"].Text = $@"'{ResultGridView.Rows.Count - 1}'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 10:
                                ResultGridView.DataSource = DataTableManager.ConvertTableType(dataTableFromQuery);
                                CaseReportTax();
                                PdfExportButton.Enabled = true;
                                ExcelExportButton.Enabled = true;

                                path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                                path = path.Replace("\\bin\\Debug", "");
                                
                                PrimaryCrystalReportViewer.ReportSource = null;
                                PrimaryCrystalReportViewer.Refresh();
                                try
                                {
                                    DataTable dt = new DataTable();
                                    foreach (DataGridViewColumn col in ResultGridView.Columns)
                                    {
                                        dataTableFromQuery.Columns.Add(col.HeaderText);
                                    }
                                    for ( i = 0; i < ResultGridView.Rows.Count - 1; i++)
                                    {
                                        DataRow dRow = dataTableFromQuery.NewRow();
                                        for (int j = 0; j < ResultGridView.Columns.Count; j++)
                                        {
                                            dRow[j] = ResultGridView.Rows[i].Cells[j].Value;
                                        }
                                        dataTableFromQuery.Rows.Add(dRow);
                                    }
                                    string p0, p1, p2, p3, p4, p5;
                                    p0 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[4].Value.ToString();
                                    p1 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[7].Value.ToString();
                                    p2 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[8].Value.ToString();
                                    p3 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[9].Value.ToString();
                                    p4 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[10].Value.ToString();
                                    p5 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[11].Value.ToString();

                                    rpt.Load(path + "\\CrystalReports\\Report10.rpt");
                                    rpt.SetDataSource(dataTableFromQuery);
                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                    if (dataTableFromQuery.Rows.Count > 0)
                                    {
                                        rpt.DataDefinition.FormulaFields["CompanyName"].Text = $"'{AppGlobalVariables.Printings.Company1.Trim()}'";
                                        rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + p0 + "'";
                                        rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + p1 + "'";
                                        rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + p2 + "'";
                                        rpt.DataDefinition.FormulaFields["Pa3"].Text = "'" + p3 + "'";
                                        rpt.DataDefinition.FormulaFields["Pa4"].Text = "'" + p4 + "'";
                                        rpt.DataDefinition.FormulaFields["Pa5"].Text = "'" + p5 + "'";
                                    }
                                    PrimaryCrystalReportViewer.ReportSource = rpt;
                                    PrimaryCrystalReportViewer.Refresh();
                                }
                                catch { }
                               
                                break;
                            case 11:
                                rpt.Load(path + "\\CrystalReports\\Report11.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 23:
                                if (Configs.Reports.UseReport23_1)
                                    rpt.Load(path + "\\CrystalReports\\Report23_1.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report23.rpt");

                                rpt.SetDataSource(dataTableFromQuery);

                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 24:
                                if (Configs.Reports.UseReport24_1) 
                                    rpt.Load(path + "\\CrystalReports\\Report24_1.rpt");
                                else if (Configs.Reports.UseReport24_2) 
                                    rpt.Load(path + "\\CrystalReports\\Report24_2.rpt");
                                else if (Configs.Reports.UseReport24_3) 
                                    rpt.Load(path + "\\CrystalReports\\Report24_3.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report24.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 25:
                                rpt.Load(path + "\\CrystalReports\\Report25.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                CrystalReportHelper.SetGenericReportFormulaFields(rpt, useCombinedAddress:true);
                                string reportName = $@"'ตั้งแต่ {startDateMonthYearWithFullMonthName} 00:00:00 ถึง {endDateMonthYearWithFullMonthName} 23:59:59'";
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = reportName;
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 26:
                                rpt.Load(path + "\\CrystalReports\\Report26.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["head"].Text = reportRange;
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 27:
                                rpt.Load(path + "\\CrystalReports\\Report27.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);
                                rpt.DataDefinition.FormulaFields["head"].Text = reportRange;
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 28:
                                rpt.Load(path + "\\CrystalReports\\Report28.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["head"].Text = reportRange;
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 29:
                                rpt.Load(path + "\\CrystalReports\\Report29.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["head"].Text = reportRange;
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 31:
                                if (Configs.Reports.ReportNoRunning) 
                                    rpt.Load(path + "\\CrystalReports\\Report31NoRunning.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report31.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                CrystalReportHelper.SetGenericReportFormulaFields(rpt);
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 32:
                                DataTable Map3 = new DataTable("myMember");  

                                DataRow dr3 = null;
                                Map3.Columns.Add(new DataColumn("ลำดับ", typeof(string)));
                                Map3.Columns.Add(new DataColumn("ประเภท", typeof(string)));
                                Map3.Columns.Add(new DataColumn("ทะเบียน", typeof(string)));
                                Map3.Columns.Add(new DataColumn("เวลาเข้า", typeof(string)));
                                Map3.Columns.Add(new DataColumn("เจ้าหน้าที่ขาเข้า", typeof(string)));
                                Map3.Columns.Add(new DataColumn("picdiv", typeof(System.Byte[])));
                                Map3.Columns.Add(new DataColumn("piclic", typeof(System.Byte[])));

                                if (Configs.UseNameOnCard) 
                                    Map3.Columns.Add(new DataColumn("ชื่อบัตร", typeof(string)));

                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    dr3 = Map3.NewRow();
                                    dr3["ลำดับ"] = dataTableFromQuery.Rows[j]["ลำดับ"];
                                    dr3["ประเภท"] = dataTableFromQuery.Rows[j]["ประเภท"];
                                    dr3["เวลาเข้า"] = dataTableFromQuery.Rows[j]["เวลาเข้า"];
                                    dr3["เจ้าหน้าที่ขาเข้า"] = dataTableFromQuery.Rows[j]["เจ้าหน้าที่ขาเข้า"];
                                    try
                                    {
                                        dr3["ทะเบียน"] = dataTableFromQuery.Rows[j]["ทะเบียน"];
                                    }
                                    catch (Exception) { }
                                    FileStream fiStream;
                                    BinaryReader binReader;
                                    byte[] pic = { };

                                    try
                                    {
                                        fiStream = new FileStream(dataTableFromQuery.Rows[j]["picdiv"].ToString(), FileMode.Open);
                                        binReader = new BinaryReader(fiStream);
                                        pic = binReader.ReadBytes((int)fiStream.Length);
                                        dr3["picdiv"] = pic;
                                        fiStream.Close();
                                        binReader.Close();
                                    }
                                    catch (Exception)
                                    {
                                        dr3["picdiv"] = null;
                                    }


                                    try
                                    {
                                        fiStream = new FileStream(dataTableFromQuery.Rows[j]["piclic"].ToString(), FileMode.Open);
                                        binReader = new BinaryReader(fiStream);
                                        pic = binReader.ReadBytes((int)fiStream.Length);
                                        dr3["piclic"] = pic;
                                        fiStream.Close();
                                        binReader.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        dr3["piclic"] = null;
                                    }

                                    if (Configs.UseNameOnCard)
                                        dr3["ชื่อบัตร"] = dataTableFromQuery.Rows[j]["ชื่อบัตร"];

                                    Map3.Rows.Add(dr3);
                                }

                                if (Configs.Reports.ReportNoRunning)
                                {
                                    if (Configs.UseNameOnCard) 
                                        rpt.Load(path + "\\CrystalReports\\Report32_1NoRunning.rpt");
                                    else if (Configs.Reports.UseReport32logo) 
                                        rpt.Load(path + "\\CrystalReports\\Report32logoNoRunning.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report32NoRunning.rpt");
                                }
                                else
                                {
                                    if (Configs.UseNameOnCard) 
                                        rpt.Load(path + "\\CrystalReports\\Report32_1.rpt");
                                    else if (Configs.Reports.UseReport32logo) 
                                        rpt.Load(path + "\\CrystalReports\\Report32logo.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report32.rpt");
                                }
                                rpt.SetDataSource(Map3);
                                dtMap = DbController.LoadData("Select value FROM param Where name = 'com1' or name = 'add1' or name = 'add2' or name = 'tax'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    if (Configs.Reports.UseReport32logo) //Mac 2018/05/08
                                    {
                                        rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                    }
                                }
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 32:
                                rpt.Load(path + "\\CrystalReports\\Report33.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานยกเลิกใบกำกับภาษีอย่างย่อประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.ToString("yyyy") + " ถึงวันที่ " + EndDatePicker.Value.ToString("d MMMM ") + EndDatePicker.Value.ToString("yyyy") + "'"; //Mac 2019/01/03

                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    try
                                    {
                                        rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                        try
                                        {
                                            rpt.DataDefinition.FormulaFields["FooterName1"].Text = "'" + dtMap.Rows[4][0].ToString().Trim() + "'";
                                        }
                                        catch { }
                                        try
                                        {
                                            rpt.DataDefinition.FormulaFields["FooterName2"].Text = "'" + dtMap.Rows[5][0].ToString().Trim() + "'";
                                        }
                                        catch { }
                                        try
                                        {
                                            rpt.DataDefinition.FormulaFields["FooterName3"].Text = "'" + dtMap.Rows[6][0].ToString().Trim() + "'";
                                        }
                                        catch { }
                                        try
                                        {
                                            rpt.DataDefinition.FormulaFields["FooterName4"].Text = "'" + AppGlobalVariables.Printings.ReportFooter4 + "'";
                                        }
                                        catch { }
                                        try
                                        {
                                            rpt.DataDefinition.FormulaFields["FooterName5"].Text = "'" + AppGlobalVariables.Printings.ReportFooter5 + "'";
                                        }
                                        catch { }
                                    }
                                    catch (Exception)
                                    {
                                        rpt.DataDefinition.FormulaFields["Address1"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["Address2"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["TaxID"].Text = "''";
                                    }
                                }


                                double sumTotalV = 0;

                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    sumTotalV += Convert.ToDouble(dataTableFromQuery.Rows[j]["จำนวนเงิน"]);
                                }

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();

                                break;

                            case 33:

                                rpt.Load(path + "\\CrystalReports\\Report34.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขายค่าบริการที่จอดรถประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.AddYears(543).ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    try
                                    {
                                        rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.AddYears(543).ToString("yyyy") + "'";
                                        rpt.DataDefinition.FormulaFields["FooterName1"].Text = "'" + dtMap.Rows[4][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["FooterName2"].Text = "'" + dtMap.Rows[5][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["FooterName3"].Text = "'" + dtMap.Rows[6][0].ToString().Trim() + "'";
                                    }
                                    catch (Exception)
                                    {
                                        rpt.DataDefinition.FormulaFields["Address1"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["Address2"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["TaxID"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["DatePrint"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["FooterName1"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["FooterName2"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["FooterName3"].Text = "''";
                                    }
                                }

                                double sumVat = 0;
                                double sumBefore = 0;
                                double sumTotal = 0;
                                double sumCntSlip = 0;

                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    sumVat += Convert.ToDouble(dataTableFromQuery.Rows[j]["VAT"]);
                                    sumBefore += Convert.ToDouble(dataTableFromQuery.Rows[j]["ค่าบริการ"]);
                                    sumTotal += Convert.ToDouble(dataTableFromQuery.Rows[j]["รวมเงิน"]);
                                    sumCntSlip += Convert.ToDouble(dataTableFromQuery.Rows[j]["จำนวนใบ"]);
                                }

                                if (Configs.UseCalVatFromTotal) //Mac 2022/09/30
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + (sumTotal - (sumTotal * 7 / 107)).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + (sumTotal * 7 / 107).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotal.ToString("#,###,##0.00") + "'";
                                }
                                else
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + sumBefore.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + sumVat.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotal.ToString("#,###,##0.00") + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Pa3"].Text = "'" + sumCntSlip.ToString("#,###,##0") + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();

                                break;

                            case 34:
                                if (Configs.Reports.UseReport35_1) //Mac 2021/10/15
                                    rpt.Load(path + "\\CrystalReports\\Report35_1.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report35.rpt");

                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขายค่าบริการที่จอดรถประจำเดือน " + StartDatePicker.Value.ToString("MMMM") + " " + StartDatePicker.Value.AddYears(543).ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    try
                                    {
                                        rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.AddYears(543).ToString("yyyy") + "'";
                                        rpt.DataDefinition.FormulaFields["FooterName1"].Text = "'" + dtMap.Rows[4][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["FooterName2"].Text = "'" + dtMap.Rows[5][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["FooterName3"].Text = "'" + dtMap.Rows[6][0].ToString().Trim() + "'";
                                    }
                                    catch (Exception)
                                    {
                                        rpt.DataDefinition.FormulaFields["Address1"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["Address2"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["TaxID"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["DatePrint"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["FooterName1"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["FooterName2"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["FooterName3"].Text = "''";
                                    }
                                }

                                double sumVatM = 0;
                                double sumBeforeM = 0;
                                double sumTotalM = 0;
                                double sumCountSlip = 0;

                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    sumVatM += Convert.ToDouble(dataTableFromQuery.Rows[j]["VAT"]);
                                    sumBeforeM += Convert.ToDouble(dataTableFromQuery.Rows[j]["ค่าบริการ"]);
                                    sumTotalM += Convert.ToDouble(dataTableFromQuery.Rows[j]["รวมเงิน"]);
                                    sumCountSlip += Convert.ToDouble(dataTableFromQuery.Rows[j]["จำนวนใบ"]);
                                }

                                if (Configs.UseCalVatFromTotal) //Mac 2022/09/30
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + (sumTotalM - (sumTotalM * 7 / 107)).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + (sumTotalM * 7 / 107).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotalM.ToString("#,###,##0.00") + "'";
                                }
                                else
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + sumBeforeM.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + sumVatM.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotalM.ToString("#,###,##0.00") + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Pa3"].Text = "'" + sumCountSlip.ToString("#,###,##0") + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();

                                break;

                            case 35:
                                if (Configs.Reports.UseReport36_1) //Mac 2021/10/15
                                    rpt.Load(path + "\\CrystalReports\\Report36_1.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report36.rpt");

                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสรุปรายได้ประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.AddYears(543).ToString("yyyy") + " ถึงวันที่ " + EndDatePicker.Value.ToString("d MMMM ") + EndDatePicker.Value.AddYears(543).ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    try
                                    {
                                        rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.AddYears(543).ToString("yyyy") + "'";
                                        rpt.DataDefinition.FormulaFields["FooterName1"].Text = "'" + dtMap.Rows[4][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["FooterName2"].Text = "'" + dtMap.Rows[5][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["FooterName3"].Text = "'" + dtMap.Rows[6][0].ToString().Trim() + "'";
                                    }
                                    catch (Exception)
                                    {
                                        rpt.DataDefinition.FormulaFields["Address1"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["Address2"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["TaxID"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["DatePrint"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["FooterName1"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["FooterName2"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["FooterName3"].Text = "''";
                                    }
                                }

                                double sumVatT = 0;
                                double sumBeforeT = 0;
                                double sumTotalT = 0;
                                double sumPriceT = 0;
                                double sumLossCardT = 0;
                                double sumOverdateT = 0;

                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    sumVatT += Convert.ToDouble(dataTableFromQuery.Rows[j]["VAT"]);
                                    sumBeforeT += Convert.ToDouble(dataTableFromQuery.Rows[j]["ค่าบริการ"]);
                                    sumTotalT += Convert.ToDouble(dataTableFromQuery.Rows[j]["รวมเงิน"]);
                                    sumPriceT += Convert.ToDouble(dataTableFromQuery.Rows[j]["ค่าจอดรถ"]);
                                    sumLossCardT += Convert.ToDouble(dataTableFromQuery.Rows[j]["ค่าปรับบัตรหาย"]);
                                    sumOverdateT += Convert.ToDouble(dataTableFromQuery.Rows[j]["ค่าปรับค้างคืน"]);
                                }

                                if (Configs.UseCalVatFromTotal) //Mac 2022/09/30
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + (sumTotalT - (sumTotalT * 7 / 107)).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + (sumTotalT * 7 / 107).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotalT.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa3"].Text = "'" + (sumTotalT - (sumTotalT * 7 / 107)).ToString("#,###,##0.00") + "'";
                                }
                                else
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + sumBeforeT.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + sumVatT.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotalT.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa3"].Text = "'" + sumPriceT.ToString("#,###,##0.00") + "'";
                                }

                                rpt.DataDefinition.FormulaFields["Pa4"].Text = "'" + sumLossCardataTableFromQuery.ToString("#,###,##0.00") + "'";
                                rpt.DataDefinition.FormulaFields["Pa5"].Text = "'" + sumOverdateT.ToString("#,###,##0.00") + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();

                                break;

                            case 36:
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                                rpt.Load(path + "\\CrystalReports\\Report37.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 37:

                                rpt.Load(path + "\\CrystalReports\\Report38.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    try
                                    {
                                        rpt.DataDefinition.FormulaFields["Branch"].Text = "'Branch: " + dtMap.Rows[7][0].ToString().Trim() + "'";
                                        rpt.DataDefinition.FormulaFields["Date"].Text = "'Date:  " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.ToString("yyyy") + "'";
                                    }
                                    catch (Exception)
                                    {
                                        rpt.DataDefinition.FormulaFields["Branch"].Text = "''";
                                        rpt.DataDefinition.FormulaFields["Date"].Text = "''";
                                    }
                                }

                                int sumCoupon = 0;
                                int sumPrice = 0;

                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    sumCoupon += Convert.ToInt32(dataTableFromQuery.Rows[j]["No of Coupon"]);
                                    sumPrice += Convert.ToInt32(dataTableFromQuery.Rows[j]["Actual Payment"]);
                                }

                                rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + sumCoupon + "'";
                                rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + sumPrice + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();

                                break;

                            case 38:


                                break;

                            case 39:

                                /*for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    sumCoupon += Convert.ToInt32(dataTableFromQuery.Rows[j]["No of Coupon"]);
                                    sumPrice += Convert.ToInt32(dataTableFromQuery.Rows[j]["Actual Payment"]);
                                }*/

                                break;

                            case 40:
                                rpt.Load(path + "\\CrystalReports\\Report41.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["SumCar"].Text = "'" + (ResultGridView.Rows.Count - 1).ToString("#,###,##0") + "'";
                                }

                                ResultGridView.Columns[3].Visible = false;
                                ResultGridView.Columns[4].Visible = false;

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 41:
                                DataTable Map4 = new DataTable("myMember");  //*** DataTable Map DataSet.xsd ***//

                                DataRow dr4 = null;
                                Map4.Columns.Add(new DataColumn("ลำดับ", typeof(string)));
                                Map4.Columns.Add(new DataColumn("ชื่อ", typeof(string)));
                                Map4.Columns.Add(new DataColumn("ทะเบียน", typeof(string)));
                                Map4.Columns.Add(new DataColumn("วันที่", typeof(string)));
                                Map4.Columns.Add(new DataColumn("ประตู", typeof(string)));
                                Map4.Columns.Add(new DataColumn("picdiv", typeof(System.Byte[])));
                                Map4.Columns.Add(new DataColumn("piclic", typeof(System.Byte[])));

                                ///////////////////////////////////////////////////
                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    dr4 = Map4.NewRow();
                                    try
                                    {
                                        dr4["ลำดับ"] = dataTableFromQuery.Rows[j]["ลำดับ"];
                                        dr4["ชื่อ"] = dataTableFromQuery.Rows[j]["ชื่อ"];
                                        dr4["ทะเบียน"] = dataTableFromQuery.Rows[j]["ทะเบียน"];
                                        dr4["วันที่"] = dataTableFromQuery.Rows[j]["วันที่"];
                                        dr4["ประตู"] = dataTableFromQuery.Rows[j]["ประตู"];
                                    }
                                    catch (Exception) { }
                                    FileStream fiStream;
                                    BinaryReader binReader;
                                    byte[] pic = { };

                                    try
                                    {
                                        fiStream = new FileStream(dataTableFromQuery.Rows[j]["picdiv"].ToString(), FileMode.Open);
                                        binReader = new BinaryReader(fiStream);
                                        pic = binReader.ReadBytes((int)fiStream.Length);
                                        dr4["picdiv"] = pic;
                                        fiStream.Close();
                                        binReader.Close();
                                    }
                                    catch (Exception)
                                    {
                                        dr4["picdiv"] = null;
                                    }


                                    try
                                    {
                                        fiStream = new FileStream(dataTableFromQuery.Rows[j]["piclic"].ToString(), FileMode.Open);
                                        binReader = new BinaryReader(fiStream);
                                        pic = binReader.ReadBytes((int)fiStream.Length);
                                        dr4["piclic"] = pic;
                                        fiStream.Close();
                                        binReader.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        dr4["piclic"] = null;
                                    }

                                    Map4.Rows.Add(dr4);
                                }

                                rpt.Load(path + "\\CrystalReports\\Report42.rpt");
                                rpt.SetDataSource(Map4);
                                dtMap = DbController.LoadData("Select value FROM param Where name = 'com1' or name = 'add1' or name = 'add2' or name = 'tax'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                ResultGridView.Columns[3].Visible = false;
                                ResultGridView.Columns[4].Visible = false;

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();

                                break;
                            case 46:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report47.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสมาชิก'";
                                rpt.DataDefinition.FormulaFields["VehicleType"].Text = $"'{AppGlobalVariables.Database.VehicleTypeTh}'";
                                rpt.DataDefinition.FormulaFields["Address"].Text = "'อาคารธนภูมิ'";
                                rpt.DataDefinition.FormulaFields["ReportMonth"].Text = $"'ประจำเดือน {TextFormatters.ExtractThaiMonthFromDate(end_date)}'";
                                rpt.DataDefinition.FormulaFields["PrintedByUser"].Text = $"'{AppGlobalVariables.OperatingUser.Name}'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 47:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report48.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 48:
                                PrimaryTabControl.SelectTab(1);

                                double sumVat48 = 0;
                                double sumBefore48 = 0;
                                double sumTotal48 = 0;

                                if (Configs.Reports.Report49_LossCard_NoVat) //Mac 2021/05/28
                                {
                                    if (!Configs.IsSwitch)
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report49_losscard_novat.rpt");
                                        rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขายค่าปรับประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.ToString("yyyy") + "'";
                                    }
                                    else
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report49.rpt");
                                        rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขายค่าบริการที่จอดรถยนต์ประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.ToString("yyyy") + "'";
                                    }
                                }
                                else
                                {

                                    if (Configs.Reports.UseReport49_1) //Mac 2021/10/14
                                        rpt.Load(path + "\\CrystalReports\\Report49_1.rpt");
                                    else
                                        rpt.Load(path + "\\CrystalReports\\Report49.rpt");

                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = $"'รายงานภาษีขายค่าบริการที่จอด{AppGlobalVariables.Database.VehicleTypeTh}ประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.ToString("yyyy") + "'";
                                }
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + AppGlobalVariables.Printings.Address1.Trim() + " " + AppGlobalVariables.Printings.Address2.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + AppGlobalVariables.Printings.Tax1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";

                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    sumVat48 += Convert.ToDouble(dataTableFromQuery.Rows[j]["VAT"]);
                                    sumBefore48 += Convert.ToDouble(dataTableFromQuery.Rows[j]["ค่าบริการ"]);
                                    sumTotal48 += Convert.ToDouble(dataTableFromQuery.Rows[j]["จำนวนเงิน"]);
                                }

                                if (Configs.UseCalVatFromTotal) //Mac 2022/09/30
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + (sumTotal48 - (sumTotal48 * 7 / 107)).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + (sumTotal48 * 7 / 107).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotal48.ToString("#,###,##0.00") + "'";
                                }
                                else if (Configs.Reports.UseReport49_1) //Mac 2021/10/14
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + (sumTotal48 - (sumTotal48 * 7 / 107)).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + (sumTotal48 * 7 / 107).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotal48.ToString("#,###,##0.00") + "'";
                                }
                                else
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + sumBefore48.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + sumVat48.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotal48.ToString("#,###,##0.00") + "'";
                                }

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 49:
                                PrimaryTabControl.SelectTab(1);
                                if (Configs.Reports.UseReport50logo) //Mac 2018/05/23
                                    rpt.Load(path + "\\CrystalReports\\Report50logo.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report50.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (Configs.Reports.UseReport50logo) //Mac 2018/05/08
                                {
                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงาน" + ReportComboBox.Text + "ประจำเดือน " + StartDatePicker.Value.ToString("MMMM") + " " + StartDatePicker.Value.ToString("yyyy") + "'";
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                }
                                else
                                {
                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขายค่าบริการที่จอดรถประจำเดือน " + StartDatePicker.Value.ToString("MMMM") + " " + StartDatePicker.Value.ToString("yyyy") + "'";
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + AppGlobalVariables.Printings.Address1.Trim() + " " + AppGlobalVariables.Printings.Address2.Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + AppGlobalVariables.Printings.Tax1.Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                }

                                double sumVat49 = 0;
                                double sumBefore49 = 0;
                                double sumTotal49 = 0;

                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    sumVat49 += Convert.ToDouble(dataTableFromQuery.Rows[j]["VAT"]);
                                    sumBefore49 += Convert.ToDouble(dataTableFromQuery.Rows[j]["ค่าบริการ"]);
                                    sumTotal49 += Convert.ToDouble(dataTableFromQuery.Rows[j]["รวมเงิน"]);
                                }

                                if (Configs.UseCalVatFromTotal) //Mac 2022/09/30
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + (sumTotal49 - (sumTotal49 * 7 / 107)).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + (sumTotal49 * 7 / 107).ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotal49.ToString("#,###,##0.00") + "'";
                                }
                                else
                                {
                                    rpt.DataDefinition.FormulaFields["Pa0"].Text = "'" + sumBefore49.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa1"].Text = "'" + sumVat49.ToString("#,###,##0.00") + "'";
                                    rpt.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotal49.ToString("#,###,##0.00") + "'";
                                }

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 51:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report52.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขาย'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address"].Text = "'ที่อยู่ : " + AppGlobalVariables.Printings.Address1.Trim() + " " + AppGlobalVariables.Printings.Address2.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + AppGlobalVariables.Printings.Tax1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานภาษีขาย " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 52:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report53.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสรุปรายรับ (รายวัน)'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานสรุปรายรับ (รายวัน) " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 53:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report54.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสรุปรายรับ (รายเดือน)'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานสรุปรายรับ (รายเดือน) " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 54:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report55.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานรถคงค้าง'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานรถคงค้าง " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 55:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report56.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานบัตรหาย'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานบัตรหาย " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 56:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report57.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสถิติชั่วโมงการจอด'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานสถิติชั่วโมงการจอด " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 57:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report58.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานข้อมูลบัตรสมาชิก'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานข้อมูลบัตรสมาชิก " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 58:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report58.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานประวัติการบันทึกบัตร'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานประวัติการบันทึกบัตร " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 59:
                                PrimaryTabControl.SelectTab(1);
                                if (Configs.IsSwitch)
                                    rpt.Load(path + "\\CrystalReports\\Report60_1.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report60.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานการใช้บริการลานจอด/วัน'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานการใช้บริการลานจอด/วัน " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();

                                if (Configs.IsSwitch)
                                    Configs.IsSwitch = false;
                                else
                                    Configs.IsSwitch = true;

                                break;
                            case 60:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report61.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานการใช้บริการสรุปรายเดือน'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["Month"].Text = "'ประจำเดือน" + StartDatePicker.Value.ToString("MMMM") + "'";
                                ReportHeaderLabel.Text = "รายงานการใช้บริการสรุปรายเดือน " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 61:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report62.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานการใช้บริการลานจอดของกลุ่มสมาชิก (เพศ)'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานการใช้บริการลานจอดของกลุ่มสมาชิก (เพศ) " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 62:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report63.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานการใช้บริการลานจอดของกลุ่มสมาชิก (วันหยุด)'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานการใช้บริการลานจอดของกลุ่มสมาชิก (วันหยุด) " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 63:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report64.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสมาชิกที่ไม่เคยใช้บริการ'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานสมาชิกที่ไม่เคยใช้บริการ " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 64:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report65.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสรุปสมาชิกที่ไม่เคยใช้บริการ'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานสรุปสมาชิกที่ไม่เคยใช้บริการ " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 65:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report66.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานการใช้บริการลานจอดแยกตามช่วงเวลา'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานการใช้บริการลานจอดแยกตามช่วงเวลา " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 66:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report67.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสรุปการขายประจำวัน/สำเนาใบกำกับภาษีอย่างย่อ'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address"].Text = "'" + AppGlobalVariables.Printings.Address1.Trim() + " " + AppGlobalVariables.Printings.Address2.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + AppGlobalVariables.Printings.Telephone.Trim() + " " + AppGlobalVariables.Printings.Tax1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานสรุปการขายประจำวัน/สำเนาใบกำกับภาษีอย่างย่อ" + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 67:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report68.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานการใช้บริการที่จอดรถ เฉพาะรายการแจ้งหนี้'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address"].Text = "'" + AppGlobalVariables.Printings.Address1.Trim() + " " + AppGlobalVariables.Printings.Address2.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + AppGlobalVariables.Printings.Telephone.Trim() + " " + AppGlobalVariables.Printings.Tax1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                if (MemberGroupMonthComboBox.SelectedIndex > 0)
                                    rpt.DataDefinition.FormulaFields["Grouppro"].Text = "'ชื่อลูกค้า : " + MemberGroupMonthComboBox.Text + "'";
                                else
                                    rpt.DataDefinition.FormulaFields["Grouppro"].Text = "'โปรโมชั่น : " + PromotionComboBox.Text + "'";

                                ReportHeaderLabel.Text = "รายงานการใช้บริการที่จอดรถ เฉพาะรายการแจ้งหนี้" + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 68:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report69.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานรถเข้าออกประจำวัน'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address"].Text = "'" + AppGlobalVariables.Printings.Address1.Trim() + " " + AppGlobalVariables.Printings.Address2.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + AppGlobalVariables.Printings.Telephone.Trim() + " " + AppGlobalVariables.Printings.Tax1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานรถเข้าออกประจำวัน" + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 69:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report70.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานรถค้างคืน'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address"].Text = "'" + AppGlobalVariables.Printings.Address1.Trim() + " " + AppGlobalVariables.Printings.Address2.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + AppGlobalVariables.Printings.Telephone.Trim() + " " + AppGlobalVariables.Printings.Tax1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานรถค้างคืน" + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 70:
                                PrimaryTabControl.SelectTab(1);
                                if (Configs.Reports.UseReport71_1)
                                    rpt.Load(path + "\\CrystalReports\\Report71_1.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report71.rpt");

                                rpt.SetDataSource(dataTableFromQuery);
                                if (Configs.Reports.UseReport71_1)
                                {
                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานค่าบริการที่จอดรถประจำวัน'";
                                }
                                else
                                {
                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขาย'";
                                    rpt.DataDefinition.FormulaFields["ReportCon"].Text = "'เดือนภาษี" + StartDatePicker.Value.ToString(" MMMM ") + (StartDatePicker.Value.Year + 543) + "'";
                                }
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address"].Text = "'" + AppGlobalVariables.Printings.Address1.Trim() + " " + AppGlobalVariables.Printings.Address2.Trim() + "'";
                                if (Configs.Reports.UseReport71_1)
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + AppGlobalVariables.Printings.Tax1.Trim() + "'";
                                else
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + AppGlobalVariables.Printings.Tax1.Split(' ')[1].Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานภาษีขาย" + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 71:
                                PrimaryTabControl.SelectTab(1);
                                if (Configs.Reports.UseReport72_1)
                                    rpt.Load(path + "\\CrystalReports\\Report72_1.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report72.rpt");

                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขาย(แบบสรุป)'";
                                rpt.DataDefinition.FormulaFields["ReportCon"].Text = "'เดือนภาษี" + StartDatePicker.Value.ToString(" MMMM ") + (StartDatePicker.Value.Year + 543) + "'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Address"].Text = "'" + AppGlobalVariables.Printings.Address1.Trim() + " " + AppGlobalVariables.Printings.Address2.Trim() + "'";
                                if (Configs.Reports.UseReport72_1)
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + AppGlobalVariables.Printings.Tax1.Trim() + "'";
                                else
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + AppGlobalVariables.Printings.Tax1.Split(' ')[1].Trim() + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานภาษีขาย" + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 72:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report73.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 73:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report74.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 74:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report75.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 75:
                                rpt.Load(path + "\\CrystalReports\\Report76.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงาน" + ReportComboBox.Text + "ประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.AddYears(543).ToString("yyyy") + " ถึงวันที่ " + EndDatePicker.Value.ToString("d MMMM ") + EndDatePicker.Value.AddYears(543).ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Sender"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "'";
                                }

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 76:
                                rpt.Load(path + "\\CrystalReports\\Report77.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["SumCar"].Text = "'" + (ResultGridView.Rows.Count - 1).ToString("#,###,##0") + "'";
                                }

                                ResultGridView.Columns[2].Visible = false;
                                ResultGridView.Columns[3].Visible = false;

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 77:
                                DataTable Map77 = new DataTable("myMember");  //*** DataTable Map DataSet.xsd ***//

                                DataRow dr77 = null;
                                Map77.Columns.Add(new DataColumn("ลำดับ", typeof(string)));
                                Map77.Columns.Add(new DataColumn("ทะเบียน", typeof(string)));
                                Map77.Columns.Add(new DataColumn("วันที่", typeof(string)));
                                Map77.Columns.Add(new DataColumn("ประตู", typeof(string)));
                                Map77.Columns.Add(new DataColumn("picdiv", typeof(System.Byte[])));
                                Map77.Columns.Add(new DataColumn("piclic", typeof(System.Byte[])));
                                Map77.Columns.Add(new DataColumn("ชื่อจุดผ่าน", typeof(string)));
                                Map77.Columns.Add(new DataColumn("ลำดับทางเข้าหลัก", typeof(string)));

                                ///////////////////////////////////////////////////
                                for (int j = 0; j < dataTableFromQuery.Rows.Count; j++)
                                {
                                    dr77 = Map77.NewRow();
                                    try
                                    {
                                        dr77["ลำดับ"] = dataTableFromQuery.Rows[j]["ลำดับ"];
                                        dr77["ทะเบียน"] = dataTableFromQuery.Rows[j]["ทะเบียน"];
                                        dr77["วันที่"] = dataTableFromQuery.Rows[j]["วันที่"];
                                        dr77["ประตู"] = dataTableFromQuery.Rows[j]["ประตู"];
                                        dr77["ชื่อจุดผ่าน"] = dataTableFromQuery.Rows[j]["ชื่อจุดผ่าน"];
                                        dr77["ลำดับทางเข้าหลัก"] = dataTableFromQuery.Rows[j]["ลำดับทางเข้าหลัก"];
                                    }
                                    catch (Exception) { }
                                    FileStream fiStream;
                                    BinaryReader binReader;
                                    byte[] pic = { };

                                    try
                                    {
                                        fiStream = new FileStream(dataTableFromQuery.Rows[j]["picdiv"].ToString(), FileMode.Open);
                                        binReader = new BinaryReader(fiStream);
                                        pic = binReader.ReadBytes((int)fiStream.Length);
                                        dr77["picdiv"] = pic;
                                        fiStream.Close();
                                        binReader.Close();
                                    }
                                    catch (Exception)
                                    {
                                        dr77["picdiv"] = null;
                                    }


                                    try
                                    {
                                        fiStream = new FileStream(dataTableFromQuery.Rows[j]["piclic"].ToString(), FileMode.Open);
                                        binReader = new BinaryReader(fiStream);
                                        pic = binReader.ReadBytes((int)fiStream.Length);
                                        dr77["piclic"] = pic;
                                        fiStream.Close();
                                        binReader.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        dr77["piclic"] = null;
                                    }

                                    Map77.Rows.Add(dr77);
                                }

                                rpt.Load(path + "\\CrystalReports\\Report78.rpt");
                                rpt.SetDataSource(Map77);
                                dtMap = DbController.LoadData("Select value FROM param Where name = 'com1' or name = 'add1' or name = 'add2' or name = 'tax'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                ResultGridView.Columns[2].Visible = false;
                                ResultGridView.Columns[3].Visible = false;

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();

                                break;
                            case 78:
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                                rpt.Load(path + "\\CrystalReports\\Report79.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 79: //Mac 2018/05/13
                                if (Configs.Reports.ReportNoRunning)
                                    rpt.Load(path + "\\CrystalReports\\Report80NoRunning.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report80.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address1"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address2"].Text = "'" + dtMap.Rows[2][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["TaxID"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }

                                ResultGridView.Columns[2].Visible = false;
                                ResultGridView.Columns[3].Visible = false;

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 80:
                                rpt.Load(path + "\\CrystalReports\\Report81.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 81:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report82.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 82:
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                                rpt.Load(path + "\\CrystalReports\\Report83.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 83:
                                PrimaryTabControl.SelectTab(1);
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString() + " " + StartTimePicker.Value.ToLongTimeString();
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString() + " " + EndTimePicker.Value.ToLongTimeString();

                                rpt.Load(path + "\\CrystalReports\\Report84.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'ประจำวันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 84:
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                                rpt.Load(path + "\\CrystalReports\\Report85.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (GuardhouseComboBox.SelectedIndex > 0)
                                    rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "        ป้อม : " + GuardhouseComboBox.Text + "'";
                                else
                                    rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 85:
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                                rpt.Load(path + "\\CrystalReports\\Report86.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (GuardhouseComboBox.SelectedIndex > 0)
                                    rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "        ป้อม : " + GuardhouseComboBox.Text + "'";
                                else
                                    rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 86:
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                                rpt.Load(path + "\\CrystalReports\\Report87.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (GuardhouseComboBox.SelectedIndex > 0)
                                    rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "        ป้อม : " + GuardhouseComboBox.Text + "'";
                                else
                                    rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 87:
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                                rpt.Load(path + "\\CrystalReports\\Report88.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (GuardhouseComboBox.SelectedIndex > 0)
                                    rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "        ป้อม : " + GuardhouseComboBox.Text + "'";
                                else
                                    rpt.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 88:
                                rpt.Load(path + "\\CrystalReports\\Report89.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 89:
                                ResultGridView.Columns[0].Visible = false;

                                if (intCase == 1)
                                {
                                    rpt.Load(path + "\\CrystalReports\\Report90.rpt");
                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = "'บัตรสมาชิก - ช่องจอด'";
                                }
                                else if (intCase == 2)
                                {
                                    rpt.Load(path + "\\CrystalReports\\Report90_1.rpt");
                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = "'บัตรสมาชิก - เลขที่บัตร'";
                                }
                                else if (intCase == 3)
                                {
                                    rpt.Load(path + "\\CrystalReports\\Report90_2.rpt");
                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = "'บัตรสมาชิก - เลขห้องชุด'";
                                }
                                else if (intCase == 4)
                                {
                                    rpt.Load(path + "\\CrystalReports\\Report90_3.rpt");
                                    rpt.DataDefinition.FormulaFields["ReportName"].Text = "'บัตรสมาชิก - อายัดบัตร'";
                                }
                                rpt.SetDataSource(dataTableFromQuery);

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 90: //Mac 2020/10/26
                                goto case 0;
                            case 91: //Mac 2020/10/26
                                goto case 1;
                            case 92: //Mac 2020/10/26
                                goto case 4;
                            case 93: //Mac 2020/10/26
                                goto case 31;
                            case 94: //Mac 2021/02/05
                                if (Configs.Reports.ReportNoRunning)
                                    rpt.Load(path + "\\CrystalReports\\Report95NoRunning.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report95.rpt");

                                rpt.SetDataSource(dataTableFromQuery);
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 95: //Mac 2022/03/17
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report96.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานการจอดรถแบบมีเงื่อนไข'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานการจอดรถแบบมีเงื่อนไข " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 96: //Mac 2022/03/29
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report97.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานการเก็บ Log'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                ReportHeaderLabel.Text = "รายงานการเก็บ Log " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 97: //Mac 2020/06/26
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report100.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }

                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 98: //Mac 2020/06/26
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report100.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 99: //Mac 2020/06/26
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report100.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 100: //Mac 2019/12/24
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report101.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 101: //Mac 2020/01/20
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report102.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 102: //Mac 2020/01/23
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report103.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 103: //Mac 2020/01/23
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report104.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 104: //Mac 2020/01/24
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report105.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 105: //Mac 2020/03/09
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report106.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 106: //Mac 2020/03/12
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report107.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 107: //Mac 2020/06/04
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                if (Configs.Reports.UseReportHourUse) //Mac 2023/02/22
                                    rpt.Load(path + "\\CrystalReports\\Report108_houruse.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report108.rpt");

                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                if (MemberGroupMonthComboBox.Text.Trim() == Constants.TextBased.All)
                                    rpt.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : ทั้งหมด'";
                                else
                                    rpt.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : " + AppGlobalVariables.MemberGroupMonthsToId[MemberGroupMonthComboBox.Text] + " " + MemberGroupMonthComboBox.Text + "'";

                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 108: //Mac 2020/06/09
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report109.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                if (MemberGroupMonthComboBox.Text.Trim() == Constants.TextBased.All)
                                    rpt.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : ทั้งหมด'";
                                else
                                    rpt.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : " + AppGlobalVariables.MemberGroupMonthsToId[MemberGroupMonthComboBox.Text] + " " + MemberGroupMonthComboBox.Text + "'";

                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 109: //Mac 2020/06/09
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report110.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                if (MemberGroupMonthComboBox.Text.Trim() == Constants.TextBased.All)
                                    rpt.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : ทั้งหมด'";
                                else
                                    rpt.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : " + AppGlobalVariables.MemberGroupMonthsToId[MemberGroupMonthComboBox.Text] + " " + MemberGroupMonthComboBox.Text + "'";

                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                rpt.DataDefinition.FormulaFields["SumCalQuota"].Text = "'" + SumCalQuota109.ToString("#,###,##0") + "'";
                                rpt.DataDefinition.FormulaFields["SumCalPriceQuota"].Text = "'" + (SumCalQuota109 * 10).ToString("#,###,##0") + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();

                                break;
                            case 110: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report200.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 111: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report201.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 112: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report202.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 113: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report203.rpt");
                                dataTableFromQuery.Columns.Add(new DataColumn("เวลาจอด", typeof(string)));

                                for (int x = 0; x < dataTableFromQuery.Rows.Count; x++)
                                {
                                    string dateinStr = dataTableFromQuery.Rows[x]["วันที่-เวลาเข้า"].ToString();
                                    string dateoutStr = dataTableFromQuery.Rows[x]["วันที่-เวลาออก"].ToString();
                                    DateTime datein = DateTime.ParseExact(dateinStr, "dd/MM/yyyy HH:mm:ss", null);
                                    DateTime dateout = DateTime.ParseExact(dateoutStr, "dd/MM/yyyy HH:mm:ss", null);
                                    TimeSpan dateall = dateout - datein;
                                    int hours = dateall.Hours;
                                    int mins = dateall.Minutes;
                                    dataTableFromQuery.Rows[x]["เวลาจอด"] = hours + "." + mins;
                                }
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 114: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report204.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 115: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report205.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 116: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report206.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 117: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report207.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 118: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report208.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 119: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report209.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 120: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report210.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                //rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                //rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                //rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 121: //ouan 2020/02/11
                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("dd/MM/yyyy");
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("dd/MM/yyyy");

                                rpt.Load(path + "\\CrystalReports\\Report211.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'วันที่พิมพ์ :   " + DateTime.Now.ToString("dd/MM/yyyy") + " เวลา " + DateTime.Now.ToString("HH:mm:ss") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                //rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                //rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                //rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 122:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report212.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Guardhouse"].Text = "'" + GuardhouseComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 123:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report213.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Guardhouse"].Text = "'" + GuardhouseComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 124:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report214.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Guardhouse"].Text = "'" + GuardhouseComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 125:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report215.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 126:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report216.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 127:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report217.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 128:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report217.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 129:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report219.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 130:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report220.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 131:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report221.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 132:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report222.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 133:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report223.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }

                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 134:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report224.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 135:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report225.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 136:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report226.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 137:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report227.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 138:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report228.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 139:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report229.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 140:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report230.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";

                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 141:
                                PrimaryTabControl.SelectTab(1);

                                if (Configs.Reports.UseReportHourUse) //Mac 2023/03/16
                                    rpt.Load(path + "\\CrystalReports\\Report231_houruse.rpt");
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report231.rpt");

                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 142:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report232.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 143:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report233.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 144:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report234.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                //rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Guardhouse"].Text = "'" + GuardhouseComboBox.Text + "'";

                                rpt.DataDefinition.FormulaFields["Building"].Text = "'" + AppGlobalVariables.Printings.Building + "'";
                                rpt.DataDefinition.FormulaFields["Office"].Text = "'" + AppGlobalVariables.Printings.Office + "'";
                                rpt.DataDefinition.FormulaFields["TaxMonth"].Text = "'" + StartDatePicker.Value.ToString("MMMM ปี ") + StartDatePicker.Value.Year + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 145:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report235.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                //dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                //rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                //rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 146:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report236.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                ////dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                //rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                //rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                ////rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 147:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report237.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                ////dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                //rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                //rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                ////rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                ////rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 148:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report238.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                ////dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                //rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                //rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                ////rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 149:

                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report239.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                ////dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                //rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 150:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report240.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                ////dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                //rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                //rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                ////rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 151:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report241.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                ////dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                //rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                //rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                ////rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 152:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report242.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                ////dtMap = DbController.LoadData("Select value FROM param Where name = 'com1'");
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                //rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'" + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy") + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                //rpt.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";
                                ////rpt.DataDefinition.FormulaFields["User"].Text = "'" + UserComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 153:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report243.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                }
                                rpt.DataDefinition.FormulaFields["Typemem"].Text = "'" + CarTypeComboBox.Text + "'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 154: //Mac 2021/11/23
                                PrimaryTabControl.SelectTab(1);

                                dst = StartDatePicker.Value;
                                startDateTime = dst.ToString("d MMMM yyyy") + " " + StartTimePicker.Value.ToLongTimeString();
                                dfn = EndDatePicker.Value;
                                endDateTime = dfn.ToString("d MMMM yyyy") + " " + EndTimePicker.Value.ToLongTimeString();
                                AppGlobalVariables.ConditionText = "'วันที่ :   " + startDateTime + "  ถึง  " + endDateTime + "'";

                                rpt.Load(path + "\\CrystalReports\\Report155.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("d/MM/yyyy HH:mm:ss") + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = AppGlobalVariables.ConditionText;
                                ReportHeaderLabel.Text = ReportComboBox.Text + " " + AppGlobalVariables.ConditionText;
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 155: //Mac 2021/11/26
                                PrimaryTabControl.SelectTab(1);

                                rpt.Load(path + "\\CrystalReports\\Report156.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + AppGlobalVariables.Printings.Company1.Trim() + "'";
                                rpt.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("d/MM/yyyy HH:mm:ss") + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;
                            case 156:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report131.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["CreatedBy"].Text = "' '";
                                }
                                rpt.DataDefinition.FormulaFields["VehicleType"].Text = $"'{AppGlobalVariables.Database.VehicleTypeTh}'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 157:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report131.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["CreatedBy"].Text = "' '";
                                }
                                rpt.DataDefinition.FormulaFields["VehicleType"].Text = $"'{AppGlobalVariables.Database.VehicleTypeTh}'";

                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 158:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report132.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["CreatedBy"].Text = "''";
                                }
                                rpt.DataDefinition.FormulaFields["VehicleType"].Text = $"'{AppGlobalVariables.Database.VehicleTypeTh}'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 159:
                                PrimaryTabControl.SelectTab(1);
                                rpt.Load(path + "\\CrystalReports\\Report132.rpt");
                                rpt.SetDataSource(dataTableFromQuery);

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'";
                                rpt.DataDefinition.FormulaFields["Condition"].Text = "'" + AppGlobalVariables.ConditionText + "'";
                                if (dtMap.Rows.Count > 0)
                                {
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Address"].Text = "'" + dtMap.Rows[1][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["Tax"].Text = "'" + dtMap.Rows[3][0].ToString().Trim() + "'";
                                    rpt.DataDefinition.FormulaFields["CreatedBy"].Text = "''";
                                }
                                rpt.DataDefinition.FormulaFields["VehicleType"].Text = $"'{AppGlobalVariables.Database.VehicleTypeTh}'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;

                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 160:
                                rpt.Load(path + "\\CrystalReports\\Report161.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 161:
                                rpt.Load(path + "\\CrystalReports\\Report162.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;

                            case 162:
                                rpt.Load(path + "\\CrystalReports\\Report163.rpt");
                                rpt.SetDataSource(dataTableFromQuery);
                                if (dtMap.Rows.Count > 0)
                                    rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";

                                rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
                                PrimaryCrystalReportViewer.ReportSource = rpt;
                                PrimaryCrystalReportViewer.Refresh();
                                break;
                        }
                    }
                    catch (Exception) { }

                    CalculationsManager.AddTotalToGridView(selectedReportId, ResultGridView);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(TextFormatters.ErrorStacktraceFromException(ex));
            }
        }



        private void ApplyConditionalUIChanges()
        {
            if (AppGlobalVariables.OperatingUser.Level > 2 &&
                (selectedReportId == 12 || selectedReportId == 37))
                UpdateReportButton.Visible = true;

            if (Configs.UseAsciiMember && CardIdTextBox.Text.Trim().Length > 0 && CardIdTextBox.Text.Trim().Length < 10)
            {
                var firstCharAscii = ((int)Convert.ToChar(CardIdTextBox.Text.Substring(0, 1))).ToString();
                CardIdTextBox.Text = Convert.ToInt32(firstCharAscii + CardIdTextBox.Text.Substring(1)).ToString();
            }


            if (ResultGridView.DataSource != null)
                ResultGridView.DataSource = null;
            else
            {
                ResultGridView.Rows.Clear();
                ResultGridView.Columns.Clear();
            }
        }

        private void ResetInitialUI()
        {
            PrimaryCrystalReportViewer.ReportSource = null;
            PrimaryCrystalReportViewer.Refresh();
            PrimaryTabControl.TabPages.Remove(tabUser);
            RemoveTab("tabPer");
            PrimaryTabControl.SelectTab(tabPage1);
            ExcelExportButton.Enabled = false;
            PdfExportButton.Enabled = false;
        }

        private void CreateReportLicense(DataTable dt)
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
                    dtTmp.Rows.Add(dr);//this will add the row at the end of the datatable

                }
                catch { }
            }
            dtTmp.DefaultView.Sort = dtTmp.Columns[5].ColumnName + " DESC";
            ResultGridView.DataSource = dtTmp;

            ReportDocument rpt = new ReportDocument();
            DataTable dtMap = DbController.LoadData("Select value FROM param Where name = 'com1' or name = 'add1' or name = 'add2' or name = 'tax'");
            PrimaryCrystalReportViewer.ReportSource = null;
            PrimaryCrystalReportViewer.Refresh();
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = path.Replace("\\bin\\Debug", "");
            rpt.Load(path + "\\CrystalReports\\Report7.rpt");
            rpt.SetDataSource(dtTmp);
            rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportHeaderLabel.Text + "'";
            if (dtMap.Rows.Count > 0)
            {
                rpt.DataDefinition.FormulaFields["CompanyName"].Text = "'" + dtMap.Rows[0][0].ToString().Trim() + "'";
            }
            PrimaryCrystalReportViewer.ReportSource = rpt;
            PrimaryCrystalReportViewer.Refresh();
        }

        public void ExportToExcel(DataGridView Tbl, string ExcelFilePath = null)
        {
            try
            {
                if (Tbl == null || Tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook
                Excel.Application excelApp = new Excel.Application();
                excelApp.Workbooks.Add();

                // single worksheet
                Excel._Worksheet workSheet = excelApp.ActiveSheet;

                // heading report
                workSheet.Cells[1, 1] = AppGlobalVariables.Printings.Header;

                // column headings
                for (int i = 0; i < Tbl.Columns.Count; i++)
                {
                    workSheet.Cells[2, (i + 1)] = Tbl.Columns[i].HeaderText;
                }

                // rows
                for (int i = 0; i < Tbl.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < Tbl.Columns.Count; j++)
                    {
                        //workSheet.Cells[(i + 3), (j + 1)] = "'" + Tbl.Rows[i].Cells[j].Value;
                        if ((Tbl.Columns[j].HeaderText.Length > 2) && (Tbl.Columns[j].HeaderText.Substring(0, 3) == "วัน") && (Configs.Reports.UseReportDateString)) //Mac 2018/07/05
                        {
                            workSheet.Cells[(i + 3), (j + 1)].NumberFormat = "@";
                        }
                        else if ((Tbl.Columns[j].HeaderText.Length > 3) && (Tbl.Columns[j].HeaderText.Substring(0, 4) == "เวลา") && (Configs.Reports.UseReportDateString)) //Mac 2017/11/25
                        {
                            //workSheet.Cells[(i + 3), (j + 1)].NumberFormat = "dd/mm/yyyy hh:mm:ss;@";
                            workSheet.Cells[(i + 3), (j + 1)].NumberFormat = "@";
                        }
                        else if ((Tbl.Columns[j].HeaderText.Length > 3) && (Tbl.Columns[j].HeaderText.Substring(0, 4) == "ว.ด.") && (Configs.Reports.UseReportDateString)) //Mac 2018/12/22
                        {
                            workSheet.Cells[(i + 3), (j + 1)].NumberFormat = "@";
                        }


                        workSheet.Cells[(i + 3), (j + 1)] = Tbl.Rows[i].Cells[j].Value;
                    }
                }

                // check fielpath
                if (ExcelFilePath != null && ExcelFilePath != "")
                {
                    try
                    {
                        workSheet.SaveAs(ExcelFilePath);
                        excelApp.Quit();
                        MessageBox.Show("Excel file saved!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    excelApp.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        private void SavePermission(object sender, System.EventArgs e)
        {
            string sql;
            Cursor = Cursors.WaitCursor;
            DataGridView dgv = this.Controls.Find("dgvPer", true).FirstOrDefault() as DataGridView;
            Button savePer = this.Controls.Find("savePer", true).FirstOrDefault() as Button;
            savePer.Enabled = false;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                sql = "delete from usereport where user_id = " + dgv.Rows[i].Cells[0].Value.ToString();
                DbController.SaveData(sql);

                for (int j = 2; j < dgv.Columns.Count; j++)
                {
                    if ((bool)dgv[j, i].Value)
                    {
                        sql = "insert into usereport (user_id,reports_id) VALUES (" + dgv.Rows[i].Cells[0].Value.ToString() + "," + (j - 1) + ")";
                        DbController.SaveData(sql);
                    }
                }
            }
            LoadPermission();
            LoadReportType();
            savePer.Enabled = true;
        }

        private void LoadReportType()
        {
            ReportComboBox.Items.Clear();
            Dictionary<int, string> dicReport = new Dictionary<int, string>();
            string sql = "Select reports.id,reports.name,reports.active From reports "; // By userid
            int u = 0;
            DataTable dttemp = DbController.LoadData(sql);
            sql = "select reports_id from usereport where user_id = " + AppGlobalVariables.OperatingUser.Id + " order by reports_id";
            Console.WriteLine(sql);
            DataTable useReport = DbController.LoadData(sql);
            try
            {
                label16.Visible = false;

                int intReportNo = dttemp.Rows.Count;
                for (int i = 0; i < intReportNo; i++)
                {
                    bool active = Convert.ToBoolean(dttemp.Rows[i].ItemArray[2]);
                    int j = i + 1;
                    if (active)
                    {
                        for (int a = 0; a < useReport.Rows.Count; a++)
                        {
                            if (Int32.Parse(useReport.Rows[a].ItemArray[0].ToString()) == j)
                            {
                                ReportComboBox.Items.Add(dttemp.Rows[i].ItemArray[1].ToString());
                                AppGlobalVariables.ReportsById.Add(i, dttemp.Rows[i].ItemArray[1].ToString());
                                /*if (dttemp.Rows[i].ItemArray[1].ToString() == "รายชื่อสมาชิก") //Mac 2015/02/03
                                {
                                    PaymentStatusComboBox.Visible = true;
                                    label16.Visible = true;
                                }
                                AppGlobalVariables.IntReportIndex[u] = i;
                                */
                                u++;
                                break;
                            }
                        }
                    }
                }


            }
            catch (Exception) { }
        }

        private void LoadPermission()
        {
            Cursor = Cursors.WaitCursor;
            RemoveTab("tabPer");
            RemoveTab("tabManage");
            string sql = "select * from reports ";
            DataTable dt = DbController.LoadData(sql);
            int col = dt.Rows.Count;
            sql = "Select id,name from user";
            DataTable dtUser = DbController.LoadData(sql);


            TabPage page = new TabPage("จัดการสิทธ์ดูรายงาน");
            page.Name = "tabPer";
            DataGridView dgv = new DataGridView();
            dgv.Name = "dgvPer";
            dgv.Width = 1305;
            dgv.Height = dgvH - 50;
            dgv.AllowUserToAddRows = false;
            dgv.Columns.Add("id", "รหัส");
            dgv.Columns.Add("name", "ชื่อ");
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dgv.Columns.Add("name"+i, dt.Rows[i][1].ToString());
                    DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
                    column.Name = "Column" + i;
                    column.HeaderText = dt.Rows[i][1].ToString();
                    column.ReadOnly = false;
                    column.Visible = Convert.ToBoolean(dt.Rows[i][2]);

                    dgv.Columns.Add(column);
                }

                dgv.Columns[0].Width = 55;
                dgv.Columns[1].Width = 200;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersHeight = 60;

                for (int i = 0; i < dtUser.Rows.Count; i++)
                {
                    var row = new DataGridViewRow();
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = dtUser.Rows[i][0].ToString() });
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = dtUser.Rows[i][1].ToString() });
                    sql = "select reports_id from usereport where user_id = " + dtUser.Rows[i][0].ToString();
                    Console.WriteLine(sql);
                    DataTable dtR = DbController.LoadData(sql);

                    int n = 0;
                    for (int j = 1; j <= col; j++)
                    {
                        if (dtR.Rows.Count > 0)
                        {
                            Console.WriteLine(j.ToString() + " " + n.ToString());
                            try
                            {
                                if (Int32.Parse(dtR.Rows[n][0].ToString()) == j)
                                {
                                    row.Cells.Add(new DataGridViewCheckBoxCell { Value = true });
                                    n++;
                                }
                                else
                                    row.Cells.Add(new DataGridViewCheckBoxCell { Value = false });
                            }
                            catch (Exception) { row.Cells.Add(new DataGridViewCheckBoxCell { Value = false }); }

                        }
                        else
                        {
                            row.Cells.Add(new DataGridViewCheckBoxCell { Value = false });
                        }


                    }
                    dgv.Rows.Add(row);
                }
            }
            catch { }

            Button savePer = new Button();
            savePer.Name = "savePer";
            savePer.Text = "Save";
            savePer.Click += new EventHandler(this.SavePermission);
            savePer.Location = new Point(1150, dgvH - 20);
            savePer.Width = 80;
            savePer.Height = 45;
            page.Controls.Add(savePer);
            page.Controls.Add(dgv);

            PrimaryTabControl.TabPages.Add(page);
            PrimaryTabControl.SelectedTab = page;

            Cursor = Cursors.Default;
        }

        private void LoadManageUser()
        {
            string sql = "select id,cardname as การ์ด,level as เลเวล,username,password,name as ชื่อ_นามสกุล ,address as ที่อยูา,tel as เบอร์โทร,grouprpt as กลุ่มรายงาน from user";
            DataTable dt = DbController.LoadData(sql);
            PrimaryTabControl.TabPages.Add(tabUser);
            PrimaryTabControl.SelectTab(tabUser);
            UserGridView.DataSource = dt;
        }

        private void RemoveTab(string name)
        {
            for (int i = 0; i < PrimaryTabControl.TabPages.Count; i++)
            {
                if (PrimaryTabControl.TabPages[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    PrimaryTabControl.TabPages.RemoveAt(i);
                    break;
                }
            }
        }

        private void SaveUser()
        {
            string sql;
            if (AppGlobalVariables.IdTextUser != "")
            {
                sql = "update user SET cardname = '" + UserCardNumberTextBox.Text + "',"
                    + " level = '" + UserLevelTextBox.Text + "',"
                    + " username = '" + UsernameTextBox.Text + "',"
                    + " password = '" + UserPasswordTextBox.Text + "',"
                    + " name = '" + UserFullNameTextBox.Text + "',"
                    + " address = '" + UserAddressTextBox.Text + "',"
                    + " tel = '" + UserTelTextBox.Text + "',";
                if (UserGroupTextBox.Text.Trim() != "")
                    sql += " grouprpt =" + UserGroupTextBox.Text;
                else sql += " grouprpt = NULL";
                sql += "  where id = " + AppGlobalVariables.IdTextUser;
                Console.WriteLine(sql);
                if (DbController.SaveData(sql) == "")
                {
                    MessageBox.Show("บันทึกการแก้ไขข้อมูลเรียบร้อย");
                }
                else MessageBox.Show("บันทึกไม่สำเร็จ");
            }
            else
            {
                sql = "INSERT INTO user (cardname,level,username,password,name,address,tel,grouprpt)";
                sql += "VALUES ('" + UserCardNumberTextBox.Text + "',"
                    + "'" + UserLevelTextBox.Text + "',"
                    + "'" + UsernameTextBox.Text + "',"
                    + "'" + UserPasswordTextBox.Text + "',"
                    + "'" + UserFullNameTextBox.Text + "',"
                    + "'" + UserAddressTextBox.Text + "',"
                    + "'" + UserTelTextBox.Text + "',";
                if (UserGroupTextBox.Text.Trim() != "")
                    sql += " grouprpt =" + UserGroupTextBox.Text + ")";
                else sql += " grouprpt = NULL)";
                if (DbController.SaveData(sql) == "")
                {
                    MessageBox.Show("บันทึกการเพิ่มข้อมูลเรียบร้อย");
                }
                else MessageBox.Show("บันทึกไม่สำเร็จ");
            }
            UserGridView.DataSource = null;
            sql = "select * from user";
            DataTable dt = DbController.LoadData(sql);
            UserGridView.DataSource = dt;
            ClearUser();
        }

        private void ClearUser()
        {
            AppGlobalVariables.IdTextUser = "";
            UserCardNumberTextBox.Text = "";
            UserLevelTextBox.Text = "";
            UsernameTextBox.Text = "";
            UserPasswordTextBox.Text = "";
            UserFullNameTextBox.Text = "";
            UserAddressTextBox.Text = "";
            UserTelTextBox.Text = "";
            UserGroupTextBox.Text = "";
        }

        private void ResultGridViewAtRunning()
        {
            if (selectedReportId == 0 || selectedReportId == 1 || selectedReportId == 2 || selectedReportId == 3 || selectedReportId == 4 || selectedReportId == 5 || selectedReportId == 12 || selectedReportId == 13 || selectedReportId == 30 || selectedReportId == 31 || selectedReportId == 8 || selectedReportId == 79 || selectedReportId == 90 || selectedReportId == 91 || selectedReportId == 92 || selectedReportId == 93 || selectedReportId == 94 || selectedReportId == 10) //Mac 2020/10/26
            {
                if (this.ResultGridView.RowCount > 0)
                {
                    foreach (DataGridViewRow r in this.ResultGridView.Rows)
                    {
                        if (r.Index < ResultGridView.RowCount - 1)
                            this.ResultGridView.Rows[r.Index].HeaderCell.Value = (r.Index + 1).ToString();
                    }
                    this.ResultGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                    this.ResultGridView.TopLeftHeaderCell.Value = "ลำดับ";
                    if (selectedReportId == 12 || selectedReportId == 13)
                        this.ResultGridView.Columns[1].Visible = false;
                    else
                        this.ResultGridView.Columns[0].Visible = false;
                }
            }
            else
            {
                this.ResultGridView.TopLeftHeaderCell.Value = "";
            }
        }
        private string SetReportHeader()
        {
            string startDateLong = StartDatePicker.Value.ToLongDateString();
            string startTimeLong = StartTimePicker.Value.ToLongTimeString();
            string endDateLong = EndDatePicker.Value.ToLongDateString();
            string endTimeLong = EndTimePicker.Value.ToLongTimeString();
            string reportName = ReportComboBox.Text;

            switch (selectedReportId)
            {
                case 13:
                    return $"รายงาน{reportName} จากวันที่ {startDateLong} เวลา {startTimeLong} ถึงวันที่ {endDateLong} เวลา {endTimeLong}";

                case 24:
                    return $"รายงาน{reportName} จากวันที่ {startDateLong} เวลา 0:00:00 ถึงวันที่ {startDateLong} เวลา 23:59:59";

                case 33:
                case 38:
                    return $"รายงาน{reportName} {StartDatePicker.Value.ToString("d MMMM yyyy")}";

                case 34:
                    return $"รายงาน{reportName} {StartDatePicker.Value.ToString("MMMM yyyy")}";

                case 42:
                case 46:
                case 47:
                    return $"รายงาน{reportName}";

                case 48 when !Configs.IsSwitch:  // Note: 'when' clause is available in C# 7.0+
                    return $"รายงานภาษีขายค่าปรับประจำวัน จากวันที่ {startDateLong} เวลา {startTimeLong} ถึงวันที่ {endDateLong} เวลา {endTimeLong}";

                case 162:
                    string paymentChannelText = PaymentChannelComboBox.Text == Constants.TextBased.All ? "ทั้งหมด" : PaymentChannelComboBox.Text;
                    return $"{reportName}: {paymentChannelText} จากวันที่ {startDateLong} เวลา {startTimeLong} ถึงวันที่ {endDateLong} เวลา {endTimeLong}";

                default:
                    return BuildDefaultHeader(reportName, startDateLong, startTimeLong, endDateLong, endTimeLong);
            }
        }

        private string BuildDefaultHeader(string reportName, string startDate, string startTime, string endDate, string endTime)
        {
            if (Configs.NoshowSelectTime == null || Configs.NoshowSelectTime.Length == 0)
            {
                return $"รายงาน{reportName} จากวันที่ {startDate} เวลา {startTime} ถึงวันที่ {endDate} เวลา {endTime}";
            }

            bool shouldHideTime = Array.IndexOf(Configs.NoshowSelectTime, (selectedReportId + 1).ToString()) > -1;
            return shouldHideTime
                ? $"รายงาน{reportName} จากวันที่ {startDate} ถึงวันที่ {endDate}"
                : $"รายงาน{reportName} จากวันที่ {startDate} เวลา {startTime} ถึงวันที่ {endDate} เวลา {endTime}";
        }
        #endregion PROCESS_END


        #region UI_EVENT_HANDLER
        private void ResultGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CalculationsManager.AddTotalToGridView(selectedReportId, ResultGridView);
        }

        private void ResultGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Clear all picture boxes
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;

            int rowIndex = e.RowIndex;
            bool isValidRowIndex = rowIndex > -1 && rowIndex < ResultGridView.Rows.Count;

            if (!isValidRowIndex) return;

            switch (selectedReportId)
            {
                case 1:
                case 91:
                    int columnOffset = CalculationsManager.CalculateColumnOffset();
                    if (Configs.Use2Camera)
                    {
                        HandleTwoCameraReport(rowIndex, columnOffset);
                    }
                    else
                    {
                        HandleSingleCameraReport(rowIndex, columnOffset);
                    }
                    break;
                case 7:
                    try
                    {
                        string pic1 = ResultGridView.Rows[rowIndex].Cells[4].Value.ToString();
                        ImagesManager.SetImageSourceToPictureBox(pic1, pictureBox1);

                        string pic2 = ResultGridView.Rows[rowIndex].Cells[5].Value.ToString();
                        ImagesManager.SetImageSourceToPictureBox(pic2, pictureBox2);
                    }
                    catch { }
                    break;
                case 31:
                case 93:
                    try
                    {
                        string pic1 = GetReport31ImagePath(rowIndex, isFirstImage: true);
                        ImagesManager.SetImageSourceToPictureBox(pic1, pictureBox1);

                        string pic2 = GetReport31ImagePath(rowIndex, isFirstImage: false);
                        ImagesManager.SetImageSourceToPictureBox(pic2, pictureBox2);
                    }
                    catch { }
                    break;
            }
        }

        private void UserGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UserListBox.Visible = false;
            if (e.RowIndex > -1)
            {
                AppGlobalVariables.IdTextUser = UserGridView[0, e.RowIndex].Value.ToString();
                UserCardNumberTextBox.Text = UserGridView[1, e.RowIndex].Value.ToString();
                UserLevelTextBox.Text = UserGridView[2, e.RowIndex].Value.ToString();
                UsernameTextBox.Text = UserGridView[3, e.RowIndex].Value.ToString();
                UserPasswordTextBox.Text = UserGridView[4, e.RowIndex].Value.ToString();
                UserFullNameTextBox.Text = UserGridView[5, e.RowIndex].Value.ToString();
                UserAddressTextBox.Text = UserGridView[6, e.RowIndex].Value.ToString();
                UserTelTextBox.Text = UserGridView[7, e.RowIndex].Value.ToString();
                UserGroupTextBox.Text = UserGridView[8, e.RowIndex].Value.ToString();
            }
        }

        private void ResultGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            CalculationsManager.AddTotalToGridView(selectedReportId, ResultGridView);
        }

        private void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (UsernameTextBox.Text.Length > 0)
            {
                if (e.KeyCode == Keys.Down)
                {
                    UserListBox.Focus();
                    if (dtUser.Rows.Count > 0)
                    {
                        UserListBox.SelectedIndex = 0;
                    }
                }
            }
        }

        private void UsernameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            dtUser = DbController.LoadData("SELECT username from user where username like '%" + UsernameTextBox.Text + "%'");
            if (UsernameTextBox.Text.Length > 0)
            {
                UserListBox.Items.Clear();
                if (dtUser.Rows.Count > 0)
                {
                    UserListBox.Visible = true;
                    for (int i = 0; i < dtUser.Rows.Count; i++)
                    {
                        UserListBox.Items.Add(dtUser.Rows[i][0].ToString());
                    }
                }
                else UserListBox.Visible = false;
            }
            else
                UserListBox.Visible = false;

        }

        private void UserListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                UsernameTextBox.Focus();
            }
            if (e.KeyCode == Keys.Return)
            {
                string sql = "select * from user where username = '" + UserListBox.Text + "'";
                DataTable dt = DbController.LoadData(sql);
                AppGlobalVariables.IdTextUser = dt.Rows[0][0].ToString();
                UserCardNumberTextBox.Text = dt.Rows[0][1].ToString();
                UserLevelTextBox.Text = dt.Rows[0][2].ToString();
                UsernameTextBox.Text = dt.Rows[0][3].ToString();
                UserPasswordTextBox.Text = dt.Rows[0][4].ToString();
                UserFullNameTextBox.Text = dt.Rows[0][5].ToString();
                UserAddressTextBox.Text = dt.Rows[0][6].ToString();
                UserTelTextBox.Text = dt.Rows[0][7].ToString();
                UserGroupTextBox.Text = dt.Rows[0][8].ToString();


                UserListBox.Items.Clear();
                UserListBox.Visible = false;
                UserNameListBox.Items.Clear();
                UserNameListBox.Visible = false;
                // saveUser();
            }
        }

        private void UserFullNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (UserFullNameTextBox.Text.Length > 0)
            {
                if (e.KeyCode == Keys.Down)
                {
                    UserNameListBox.Focus();
                    if (dtName.Rows.Count > 0)
                    {
                        UserNameListBox.SelectedIndex = 0;
                    }
                }
            }
        }

        private void UserFullNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            dtName = DbController.LoadData("SELECT name from user where name like '%" + UserFullNameTextBox.Text + "%'");
            if (UserFullNameTextBox.Text.Length > 0)
            {
                UserNameListBox.Items.Clear();
                if (dtName.Rows.Count > 0)
                {
                    UserNameListBox.Visible = true;
                    for (int i = 0; i < dtName.Rows.Count; i++)
                    {
                        UserNameListBox.Items.Add(dtName.Rows[i][0].ToString());
                    }
                }
                else UserNameListBox.Visible = false;
            }
            else
                UserNameListBox.Visible = false;
        }

        private void UserNameListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                UserFullNameTextBox.Focus();
            }
            if (e.KeyCode == Keys.Return)
            {
                string sql = "select * from user where name = '" + UserNameListBox.Text + "'";
                DataTable dt = DbController.LoadData(sql);
                AppGlobalVariables.IdTextUser = dt.Rows[0][0].ToString();
                UserCardNumberTextBox.Text = dt.Rows[0][1].ToString();
                UserLevelTextBox.Text = dt.Rows[0][2].ToString();
                UsernameTextBox.Text = dt.Rows[0][3].ToString();
                UserPasswordTextBox.Text = dt.Rows[0][4].ToString();
                UserFullNameTextBox.Text = dt.Rows[0][5].ToString();
                UserAddressTextBox.Text = dt.Rows[0][6].ToString();
                UserTelTextBox.Text = dt.Rows[0][7].ToString();
                UserGroupTextBox.Text = dt.Rows[0][8].ToString();

                UserListBox.Items.Clear();
                UserListBox.Visible = false;
                UserNameListBox.Items.Clear();
                UserNameListBox.Visible = false;
            }
        }

        private void SetReportConditionButton_Click(object sender, EventArgs e)
        {
            FormSetReport frm = new FormSetReport();
            frm.ShowDialog();
        }

        private void ReportComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedReportId = AppGlobalVariables.ReportsById.First(kvp => kvp.Value == ReportComboBox.Text).Key;

            if (selectedReportId == 16 || selectedReportId == 17 || selectedReportId == 18
                || selectedReportId == 19 || selectedReportId == 20 || selectedReportId == 21)
                SetReportConditionButton.Visible = true;
            else SetReportConditionButton.Visible = false;

            if (selectedReportId == 19)
            {

            }
            else if (selectedReportId == 21)
            {
                if (PromotionComboBox.SelectedIndex == 0)
                    PromotionComboBox.SelectedIndex = 1;
            }
            else
                PromotionComboBox.SelectedIndex = 0;

            if (selectedReportId == 40 || selectedReportId == 41 || selectedReportId == 76 || selectedReportId == 77)
            {
                RecordNumberTextBox.Text = "";
                label30.Visible = true;
                RecordNumberTextBox.Visible = true;
            }
            else
            {
                RecordNumberTextBox.Text = "";
                label30.Visible = false;
                RecordNumberTextBox.Visible = false;
            }

            if (selectedReportId == 89 || selectedReportId == 155)
                AddressPanel.Visible = true;
            else
                AddressPanel.Visible = false;

            if (selectedReportId == 22)
                PaymentChannelPanel.Visible = true;
            else
                PaymentChannelPanel.Visible = false;


            if (selectedReportId == 95)
                ParkingTimeComparisonPanel.Visible = true;
            else
                ParkingTimeComparisonPanel.Visible = false;

            if (selectedReportId == 162 || selectedReportId == 48 || selectedReportId == 49)
            {
                label42.Visible = true;
                PaymentChannelComboBox.Visible = true;
                PaymentChannelComboBox.Text = Constants.TextBased.All;
            }
            else
            {
                label42.Visible = false;
                PaymentChannelComboBox.Visible = false;
            }

            if (selectedReportId == 20 || selectedReportId == 21 || selectedReportId == 161)
            {
                PromotionIdFrom.Clear();
                PromotionIdTo.Clear();
                PromotionComboBox.SelectedIndex = 0;

                PromotionIdRangePanel.Visible = true;
                PromotionIdRangePanel.Location = new Point(347, 85);
            }
            else
                PromotionIdRangePanel.Visible = false;
        }

        private void UpdateReportButton_Click(object sender, EventArgs e)
        {
            string sql = "";
            double num;
            string datein = "";
            string dateout = "";

            for (int i = 0; i < ResultGridView.Rows.Count - 1; i++)
            {
                try
                {
                    if (selectedReportId == 12) //Mac 2016/04/26
                    {
                        if (double.TryParse(ResultGridView.Rows[i].Cells[15].Value.ToString(), out num))
                        {
                            sql = "update recordout set proid = " + ResultGridView.Rows[i].Cells[15].Value;
                            sql += " where no = " + ResultGridView.Rows[i].Cells[1].Value;
                            DbController.SaveData(sql);
                        }

                    }
                    else
                    {
                        datein = ResultGridView.Rows[i].Cells[2].Value + ":00";
                        dateout = ResultGridView.Rows[i].Cells[3].Value + ":00";
                        sql = "UPDATE recordin t1 LEFT JOIN recordout t2 ON t1.no = t2.no ";
                        sql += " SET t1.datein = concat(left(t1.datein, 11),'" + datein + "')";
                        sql += " , t2.dateout = concat(left(t2.dateout, 11),'" + dateout + "')";
                        sql += " , t1.license = " + ResultGridView.Rows[i].Cells[4].Value;
                        sql += " , t2.proid = " + ResultGridView.Rows[i].Cells[5].Value;
                        sql += " , t2.price = " + ResultGridView.Rows[i].Cells[6].Value;
                        sql += " WHERE t2.no = " + ResultGridView.Rows[i].Cells[0].Value;

                        DbController.SaveData(sql);
                    }
                }
                catch { }
            }
            MessageBox.Show("Update complete", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResultGridView.DataSource = null;
        }

        private void MifareCheckTimer_Tick(object sender, EventArgs e)
        {
            if (Configs.UseMifare)
            {
                if (mfReader.CheckCard())
                {
                    MifareCheckTimer.Enabled = false;
                    AppGlobalVariables.IdText = mfReader.Init1();
                    if (AppGlobalVariables.IdText != "")
                    {
                        mfReader.SetLED(1);
                        if (Configs.Hardwares.IsMFPassiveInProx)
                        {
                            AppGlobalVariables.IdText = AppGlobalVariables.IdText.Substring(4, 2) + AppGlobalVariables.IdText.Substring(2, 2) + AppGlobalVariables.IdText.Substring(0, 2);
                        }
                        uint intID = Convert.ToUInt32(AppGlobalVariables.IdText, 16);
                        AppGlobalVariables.IdText = "";
                        CardIdTextBox.Text = intID.ToString();
                        mfReader.SetSound(8);
                        while (mfReader.CheckCard())
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(10);
                        }
                        mfReader.SetLED(2);
                        MifareCheckTimer.Enabled = true;
                    }
                    else
                    {
                        MifareCheckTimer.Enabled = true;
                    }
                }
            }
        }

        private void IgnoreExpirationDateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (IgnoreExpirationDateCheckBox.Checked)
            {
                MemberExpirationStartDatePicker.Enabled = false;
                MemberExpirationEndDatePicker.Enabled = false;
            }
            else
            {
                MemberExpirationStartDatePicker.Enabled = true;
                MemberExpirationEndDatePicker.Enabled = true;
            }
        }

        private void RegistrationDateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RegistrationDateCheckBox.Checked)
                ExpirationDateCheckBox.Checked = false;
        }

        private void ExpirationDateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ExpirationDateCheckBox.Checked)
                RegistrationDateCheckBox.Checked = false;
        }

        private void ParkingGreaterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ParkingGreaterCheckBox.Checked)
            {
                ParkingLesserCheckBox.Checked = false;
                ParkingBetweenCheckBox.Checked = false;
            }
        }

        private void ParkingLesserCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ParkingLesserCheckBox.Checked)
            {
                ParkingGreaterCheckBox.Checked = false;
                ParkingBetweenCheckBox.Checked = false;
            }
        }

        private void ParkingBetweenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ParkingBetweenCheckBox.Checked)
            {
                ParkingGreaterCheckBox.Checked = false;
                ParkingLesserCheckBox.Checked = false;
            }
        }

        private void ManageUserButton_Click(object sender, EventArgs e)
        {
            PrimaryTabControl.TabPages.Remove(tabUser);
            RemoveTab("tabPer");
            LoadManageUser();
        }

        private void ManageUserClearButton_Click(object sender, EventArgs e)
        {
            //btnClear
            ClearUser();
        }

        private void ManageUserSaveButton_Click(object sender, EventArgs e)
        {
            // ManageUserButtonSave 
            SaveUser();
        }

        private void ExcelExportButton_Click(object sender, EventArgs e)
        {
            if (ResultGridView.RowCount < 1)
                return;

            DataTable data = (DataTable)ResultGridView.DataSource;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx|Excel 97-2003 Workbook (*.xls)|*.xls";
            sfd.FileName = "";

            if (sfd.ShowDialog() == DialogResult.OK)
                ExportToExcel(ResultGridView, sfd.FileName);
        }

        private void PdfExportButton_Click(object sender, EventArgs e)
        {
            // btnPdf
            PrimaryTabControl.SelectTab(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrimaryTabControl.TabPages.Remove(tabUser);
            LoadPermission();
        }

        private void UpdateReportSearchCondition()
        {

        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (ReportComboBox.Items.Count < 1) return;

            Cursor = Cursors.WaitCursor;

            ResetInitialUI();

            if (ReportComboBox.Text == "" || UserComboBox.Text == "")
            {
                MessageBox.Show("กรุณาเลือกรายงาน");
                return;
            }

            int selectedReportId = this.selectedReportId;

            ApplyConditionalUIChanges();

            string sql = new ReportQueryService().BuildReportQuery(
                selectedReportId,
                PaymentChannelComboBox.Text,
                RecordNumberTextBox.Text,
                UserComboBox.Text,
                CarTypeComboBox.Text,
                LicensePlateTextBox.Text,
                PromotionComboBox.Text,
                CardIdTextBox.Text,
                NameOnCardTextBox.Text,
                MemberTypeComboBox.Text,
                MemberGroupMonthComboBox.Text,
                MemberNameComboBox.Text,
                MemberRenewalTypeComboBox.Text,
                MemberProcessStateComboBox.Text,
                MemberCardTypeComboBox.Text,
                GuardhouseComboBox.Text,
                PaymentStatusComboBox.Text,
                AddressTextBox.Text,
                Up2UNameTextBox.Text,
                Up2UStaffIdTextBox.Text,
                Up2UStickerNumberTextBox.Text,
                Up2UCarTypeTextBox.Text,
                MemberGroupComboBox.Text,
                MemberIdTextBox.Text,
                MemberStatusComboBox.Text,
                MemberParkingCountStartTextBox.Text,
                MemberParkingCountEndTextBox.Text,
                MemberTypeComboBox.SelectedIndex,
                ParkingGreaterTextBox.Text,
                ParkingLesserTextBox.Text,
                ParkingBetweenFromTextBox.Text,
                ParkingBetweenToTextBox.Text,
                PromotionIdFrom.Text,
                PromotionIdTo.Text,
                RegistrationDateCheckBox.Checked,
                ExpirationDateCheckBox.Checked,
                ParkingGreaterCheckBox.Checked,
                ParkingLesserCheckBox.Checked,
                ParkingBetweenCheckBox.Checked,
                StartDatePicker.Value,
                EndDatePicker.Value,
                StartTimePicker.Value,
                EndTimePicker.Value,

                MemberExpirationStartDatePicker.Value,
                MemberExpirationEndDatePicker.Value
            );

            if (selectedReportId == 16 
                || selectedReportId == 17 
                || selectedReportId == 18 
                || selectedReportId == 19 
                || selectedReportId == 20 
                || selectedReportId == 21
                || selectedReportId == 163)
                FuckingShit(selectedReportId, sql);
            else
            {
                Display(sql);
            }

            if (!Configs.Reports.ReportNoRunning)
                ResultGridViewAtRunning();

            ResultGridView.Visible = true;

            /* FOR TEST
            if (PrimaryCrystalReportViewer.ReportSource is ReportDocument reportDoc)
            {
                string reportPath = reportDoc.FileName;
                MessageBox.Show(reportPath);
            }
            else
            {
                MessageBox.Show("ReportSource is not a ReportDocument.");
            }*/

            Cursor = Cursors.Default;
        }
        #endregion UI_EVENT_HANDLER_END


        #region HELPERS
        private void AddMemberGroups(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                string groupName = row["groupname"].ToString();
                int groupId = Convert.ToInt32(row["id"]);

                AddToDictionaryIfNotExists(AppGlobalVariables.MemberGroupsToId, groupName, groupId);
                AddToComboBoxIfNotExists(MemberTypeComboBox, groupName);
            }
        }

        private static DataTable GetReport19()
        {
            throw new NotImplementedException();
        }

        private void AddCarTypes(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                string typeName = row["typename"].ToString();
                int typeId = Convert.ToInt32(row["typeid"]);

                AddToDictionaryIfNotExists(AppGlobalVariables.CarTypesById, typeId, typeName);
                AddToComboBoxIfNotExists(CarTypeComboBox, typeName);
            }
        }

        private void AddToDictionaryIfNotExists<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, value);
        }

        private void AddToComboBoxIfNotExists(ComboBox comboBox, string item)
        {
            if (!comboBox.Items.Contains(item))
                comboBox.Items.Add(item);
        }

        private void LoadCarTypes()
        {
            try
            {
                AppGlobalVariables.CarTypesById.Add(0, Constants.TextBased.All);
                AppGlobalVariables.CarTypesById.Add(199, Constants.TextBased.Visitor);
                AppGlobalVariables.CarTypesById.Add(200, Constants.TextBased.Member);

                CarTypeComboBox.Items.Add(Constants.TextBased.All);
                CarTypeComboBox.Items.Add(Constants.TextBased.Visitor);
                CarTypeComboBox.Items.Add(Constants.TextBased.Member);

                DataTable carTypes = DbController.LoadData("SELECT typeid, typename FROM cartype ORDER BY typeid");
                if (carTypes?.Rows.Count > 0)
                {
                    for (int i = 0; i < carTypes.Rows.Count; i++)
                    {
                        int carTypeId = Convert.ToInt16(carTypes.Rows[i].ItemArray[0]);
                        string carTypeName = carTypes.Rows[i].ItemArray[1].ToString();

                        if (!AppGlobalVariables.CarTypesById.ContainsValue(carTypeName))
                            if (carTypeName != Constants.TextBased.Member)
                                AppGlobalVariables.CarTypesById.Add(carTypeId, carTypeName);

                        if (!CarTypeComboBox.Items.Contains(carTypeName))
                            if (carTypeName != Constants.TextBased.Member)
                                CarTypeComboBox.Items.Add(carTypeName);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(TextFormatters.ErrorStacktraceFromException(ex), "LoadCarTypes");
            }
        }

        private void LoadUsers()
        {
            if (!AppGlobalVariables.UsersById.ContainsKey(0))
                AppGlobalVariables.UsersById.Add(0, Constants.TextBased.All);

            if (!UserComboBox.Items.Contains(Constants.TextBased.All))
                UserComboBox.Items.Add(Constants.TextBased.All);

            UserComboBox.Text = Constants.TextBased.All;

            try
            {
                DataTable dt = DbController.LoadData("SELECT id, name FROM user ORDER BY id");
                if (dt?.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dt.Rows[i];
                        int userId = Convert.ToInt32(row["id"]);
                        string userName = row["name"].ToString();

                        if (!AppGlobalVariables.UsersById.ContainsKey(userId))
                            AppGlobalVariables.UsersById.Add(userId, userName);

                        if (!UserComboBox.Items.Contains(userName))
                            UserComboBox.Items.Add(userName);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(TextFormatters.ErrorStacktraceFromException(ex), "LoadUsers");
            }
        }

        private void LoadPromotions()
        {
            AppGlobalVariables.PromotionNamesById.Add(0, Constants.TextBased.All);
            PromotionComboBox.Items.Add(Constants.TextBased.All);

            try
            {
                string sql = "SELECT id, name, minute FROM promotion ORDER BY id";
                DataTable dt = DbController.LoadData(sql);

                if (dt?.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int promotionId = Convert.ToInt32(row["id"]);
                        string promotionName = row["name"].ToString();
                        int minutes = Convert.ToInt32(row["minute"]);

                        //AppGlobalVariables.PromotionNamesToId.Add(promotionName, promotionId);
                        AppGlobalVariables.PromotionNamesById.Add(promotionId, promotionName);
                        AppGlobalVariables.PromotionNamesMinuteMap.Add(promotionId, minutes);

                        if (promotionId != 9999)
                        {
                            PromotionComboBox.Items.Add(promotionName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(TextFormatters.ErrorStacktraceFromException(ex), "LoadPromotions");
            }
        }

        private void LoadReports()
        {
            try
            {
                label16.Visible = false;

                DataTable allReports = DbController.LoadData("SELECT id, name, active FROM reports");
                DataTable userReports = DbController.LoadData($"SELECT reports_id FROM usereport WHERE user_id = {AppGlobalVariables.OperatingUser.Id} ORDER BY reports_id");

                if (allReports?.Rows.Count > 0 && userReports?.Rows.Count > 0)
                {
                    int[] specialReports = { 19, 20, 21, 46, 47, 67 }; // Special reports that affect visibility

                    foreach (DataRow reportRow in allReports.Rows)
                    {
                        bool isActive = Convert.ToBoolean(reportRow["active"]);
                        int reportId = Convert.ToInt32(reportRow["id"]);
                        string reportName = reportRow["name"].ToString();

                        if (isActive && UserHasAccessToReport(userReports, reportId))
                        {
                            if (specialReports.Contains(reportId - 1)) // Adjust for 0-based index
                            {
                                Configs.ShowConditionMemberGroupPriceMonth = true;
                            }

                            AppGlobalVariables.ReportsById.Add(reportId, reportName);
                            ReportComboBox.Items.Add(reportName);
                        }
                    }

                    if (ReportComboBox.Items.Count > 0)
                        ReportComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(TextFormatters.ErrorStacktraceFromException(ex), "LoadReports");
            }
        }

        private void ConfigureCameraVisibility()
        {
            bool showSecondCamera = (Configs.IsVillage && Configs.Use2Camera) ||
                                  (Configs.Use2Camera && !string.IsNullOrWhiteSpace(Configs.IPIn3));

            pictureBox5.Visible = showSecondCamera;
            lbPic5.Visible = showSecondCamera;
        }

        private bool UserHasAccessToReport(DataTable userReports, int reportId)
        {
            foreach (DataRow row in userReports.Rows)
            {
                if (Convert.ToInt32(row["reports_id"]) == reportId)
                {
                    return true;
                }
            }
            return false;
        }

        private void LoadMemberGroupReport()
        {
            MemberTypeComboBox.Items.Clear();
            CarTypeComboBox.Items.Clear();

            AddToDictionaryIfNotExists(AppGlobalVariables.CarTypesById, 0, Constants.TextBased.All);
            AddToDictionaryIfNotExists(AppGlobalVariables.CarTypesById, 199, Constants.TextBased.Visitor);
            AddToDictionaryIfNotExists(AppGlobalVariables.CarTypesById, 200, Constants.TextBased.Member);
            AddToComboBoxIfNotExists(CarTypeComboBox, Constants.TextBased.All);
            AddToComboBoxIfNotExists(CarTypeComboBox, Constants.TextBased.Visitor);
            AddToComboBoxIfNotExists(CarTypeComboBox, Constants.TextBased.Member);

            AddToDictionaryIfNotExists(AppGlobalVariables.MemberGroupsToId, Constants.TextBased.All, 0);
            AddToComboBoxIfNotExists(MemberTypeComboBox, Constants.TextBased.All);


            if (Configs.Reports.ReportSearchMemberGroup || Configs.Reports.UseReport24_2)
            {
                label17.Text = Constants.TextBased.Generic.MemberGroup;
                MemberTypeComboBox.Visible = label17.Visible = true;

                DataTable dt = DbController.LoadData("SELECT groupname, id FROM membergroup ORDER BY id");
                AddMemberGroups(dataTableFromQuery);
            }
            else
            {
                string query = $"SELECT column_name FROM information_schema.columns WHERE table_schema = '{AppGlobalVariables.Database.Name}' AND table_name = 'member' AND column_name = 'typeid'";
                DataTable dt = DbController.LoadData(query);

                if (dt.Rows.Count == 0) return;

                MemberTypeComboBox.Visible = label17.Visible = true;

                if (Configs.Member2Cartype)
                {
                    CarTypeComboBox.Text = Constants.TextBased.All;
                    dt = DbController.LoadData("SELECT typename, typeid FROM cartype ORDER BY typeid");
                    AddCarTypes(dataTableFromQuery);
                }
                else
                {
                    CarTypeComboBox.Text = Constants.TextBased.All;
                    dt = DbController.LoadData("SELECT t1.typename, t1.typeid FROM cartype t1 LEFT JOIN member t2 ON t1.typeid = t2.typeid WHERE t2.typeid IS NULL AND t1.typeid != 200 ORDER BY t1.typeid");
                    AddCarTypes(dataTableFromQuery);
                }

                if (Configs.Member2Cartype)
                {
                    AddToDictionaryIfNotExists(AppGlobalVariables.MemberGroupsToId, Constants.TextBased.Member, 200);

                    AddToComboBoxIfNotExists(MemberTypeComboBox, Constants.TextBased.All);
                    AddToComboBoxIfNotExists(MemberTypeComboBox, Constants.TextBased.Member);

                    dt = DbController.LoadData("SELECT groupname, id FROM membergroup ORDER BY id");
                    AddMemberGroups(dataTableFromQuery);
                }
                else
                {
                    MemberTypeComboBox.Text = Constants.TextBased.All;
                    dt = DbController.LoadData("SELECT DISTINCT typeid FROM member WHERE typeid != 200 ORDER BY typeid");

                    foreach (DataRow row in dt.Rows)
                    {
                        string key = row["typeid"].ToString();
                        int value = Convert.ToInt32(row["typeid"]);

                        AddToDictionaryIfNotExists(AppGlobalVariables.MemberGroupsToId, key, value);
                        AddToComboBoxIfNotExists(MemberTypeComboBox, key);
                    }
                }
            }
        }

        private void HandleTwoCameraReport(int rowIndex, int columnOffset)
        {
            string pic1 = ResultGridView.Rows[rowIndex].Cells["iv"].Value.ToString();
            string pic2 = ResultGridView.Rows[rowIndex].Cells["il"].Value.ToString();
            string pic3 = ResultGridView.Rows[rowIndex].Cells["ov"].Value.ToString();
            string pic4 = ResultGridView.Rows[rowIndex].Cells["ol"].Value.ToString();

            ImagesManager.SetImageSourceToPictureBox(pic1, pictureBox1);
            ImagesManager.SetImageSourceToPictureBox(pic2, pictureBox2);
            ImagesManager.SetImageSourceToPictureBox(pic3, pictureBox3);
            ImagesManager.SetImageSourceToPictureBox(pic4, pictureBox4);

            HandleAdditionalFifthImage(rowIndex, columnOffset);
        }

        private void HandleSingleCameraReport(int rowIndex, int columnOffset)
        {
            string pic1 = ResultGridView.Rows[rowIndex].Cells[9 + columnOffset].Value.ToString();
            string pic2 = ResultGridView.Rows[rowIndex].Cells[10 + columnOffset].Value.ToString();

            ImagesManager.SetImageSourceToPictureBox(pic1, pictureBox1);
            ImagesManager.SetImageSourceToPictureBox(pic2, pictureBox2);
        }

        private void HandleAdditionalFifthImage(int rowIndex, int columnOffset)
        {
            bool shouldShowFifthImage = (Configs.IsVillage && Configs.Use2Camera) ||
                                       (Configs.Use2Camera && !string.IsNullOrEmpty(Configs.IPIn3.Trim()));

            if (shouldShowFifthImage)
            {
                string pic5 = ResultGridView.Rows[rowIndex].Cells[13 + columnOffset].Value.ToString();
                ImagesManager.SetImageSourceToPictureBox(pic5, pictureBox5);
            }
        }

        private string GetReport31ImagePath(int rowIndex, bool isFirstImage)
        {
            if (Configs.NoPanelUp2U == "2")
            {
                return ResultGridView.Rows[rowIndex].Cells[isFirstImage ? 9 : 10].Value.ToString();
            }
            else
            {
                return ResultGridView.Rows[rowIndex].Cells[isFirstImage ? 5 : 6].Value.ToString();
            }
        }

        private void CaseReportTax()
        {
            ResultGridView.Columns[0].HeaderText = "ลำดับ";
            ResultGridView.Columns[1].HeaderText = "ประเภท";
            ResultGridView.Columns[2].HeaderText = "ทะเบียน";
            ResultGridView.Columns[3].HeaderText = "เวลาเข้า";
            ResultGridView.Columns[4].HeaderText = "เจ้าหน้าที่ขาเข้า";
            ResultGridView.Columns[5].HeaderText = "เวลาออก";
            ResultGridView.Columns[6].HeaderText = "เวลาจอดรวม";
            ResultGridView.Columns[7].HeaderText = "ยอดรับ";
            ResultGridView.Columns[8].HeaderText = "ส่วนลด";
            ResultGridView.Columns[9].HeaderText = "ยอดรับสุทธิ";
            ResultGridView.Columns[10].HeaderText = "ยอดก่อนภาษี";
            ResultGridView.Columns[11].HeaderText = "ภาษี 7%";
            ResultGridView.Columns[12].HeaderText = "เจ้าหน้าที่ขาออก";
            ResultGridView.Columns[0].Width = 50;
            ResultGridView.Columns[4].Width = 160;
            ResultGridView.Columns[12].Width = 160;

            int intNo = ResultGridView.Rows.Count - 1;

            int intSumR = 0;
            int intSumD = 0;
            int intSumT = 0;
            double intSumBV = 0;
            double intSumV = 0;

            for (int i = 0; i < intNo; i++)
            {
                try
                {
                    int intT = Convert.ToInt32(ResultGridView[6, i].Value);
                    int intD = Convert.ToInt32(ResultGridView[7, i].Value);
                    int intR = intT + intD;
                    int intID = Convert.ToInt32(ResultGridView[1, i].Value);
                    ResultGridView[1, i].Value = AppGlobalVariables.CarTypesById[intID];
                    intID = Convert.ToInt32(ResultGridView[4, i].Value);
                    if (intID == 0)
                        ResultGridView[4, i].Value = "";
                    else
                        ResultGridView[4, i].Value = AppGlobalVariables.UsersById[intID];
                    intID = Convert.ToInt32(ResultGridView[8, i].Value);
                    if (intID == 0)
                        ResultGridView[12, i].Value = "";
                    else
                        ResultGridView[12, i].Value = AppGlobalVariables.UsersById[intID];
                    DateTime dti = DateTime.Parse(ResultGridView[3, i].Value.ToString());
                    DateTime dto = DateTime.Parse(ResultGridView[5, i].Value.ToString());
                    TimeSpan diffTime = dto - dti;
                    int intHour = diffTime.Hours;
                    if (diffTime.Days > 0)
                        intHour += diffTime.Days * 24;
                    ResultGridView[6, i].Value = intHour.ToString() + "." + diffTime.Minutes.ToString();
                    //txtH(i3).Text = Format(cFH(i2), "#0.00000")
                    ResultGridView[7, i].Value = intR.ToString("#0.00");
                    ResultGridView[8, i].Value = intD.ToString("#0.00");
                    ResultGridView[9, i].Value = intT.ToString("#0.00");
                    float floT = (float)intT;
                    //double floV = floT * 0.07;                    
                    double floBV = (floT * 100) / 107;
                    floBV = Math.Round(floBV, 2);
                    double floV = floT - floBV;
                    ResultGridView[10, i].Value = floBV.ToString("#0.00");
                    ResultGridView[11, i].Value = floV.ToString("#0.00");
                    intSumR += intR;
                    intSumD += intD;
                    intSumT += intT;
                    intSumBV += floBV;
                    intSumV += floV;
                }
                catch (Exception) { }
            }
            ResultGridView[3, intNo].Value = "จำนวนรถ";
            ResultGridView[4, intNo].Value = intNo.ToString() + " คัน";
            ResultGridView[6, intNo].Value = "ยอดรวม";
            ResultGridView[7, intNo].Value = intSumR.ToString("#0.00");
            ResultGridView[8, intNo].Value = intSumD.ToString("#0.00");
            ResultGridView[9, intNo].Value = intSumT.ToString("#0.00");
            ResultGridView[10, intNo].Value = intSumBV.ToString("#0.00");
            ResultGridView[11, intNo].Value = intSumV.ToString("#0.00");
            totalReceived = intSumR;
            totalDiscount = intSumD;
            totalAmount = intSumT;
            totalBeforeVat = intSumBV;
            totalVat = intSumV;
        }

        private void CaseReportGroupPrice()
        {
            ResultGridView.Columns[0].HeaderText = "เลขที่ใบเสร็จรับเงิน";
            ResultGridView.Columns[1].HeaderText = "ทะเบียน";
            ResultGridView.Columns[2].HeaderText = "เวลาเข้า";
            ResultGridView.Columns[3].HeaderText = "เจ้าหน้าที่ขาเข้า";
            ResultGridView.Columns[4].HeaderText = "เวลาออก";
            ResultGridView.Columns[5].HeaderText = "เจ้าหน้าที่ขาออก";
            ResultGridView.Columns[6].HeaderText = "ไม่ได้ E-stamp";
            ResultGridView.Columns[7].HeaderText = "ได้ E-stamp แบบที่1 ฟรี 2 ชั่วโมง";
            ResultGridView.Columns[8].HeaderText = "ได้ E-stamp แบบที่2 ฟรี 3 ชั่วโมง";
            ResultGridView.Columns[9].HeaderText = "ค่าบริการจอด";
            ResultGridView.Columns[10].HeaderText = "ยอดภาษี";
            ResultGridView.Columns[11].HeaderText = "รายได้รวม";
            ResultGridView.Columns[12].HeaderText = "ชื่อโปรโมชั่น";
            ResultGridView.Columns[0].Width = 100;
            ResultGridView.Columns[3].Width = 160;
            ResultGridView.Columns[5].Width = 160;
            ResultGridView.Columns[7].Width = 110;
            ResultGridView.Columns[8].Width = 110;
            ResultGridView.Columns[12].Width = 170;

            int intNo = ResultGridView.Rows.Count - 1;

            int intSumR = 0;

            int intSumE0 = 0;
            int intSumE1 = 0;
            int intSumE2 = 0;

            double intSumBV = 0;
            double intSumV = 0;

            for (int i = 0; i < intNo; i++)
            {

                try
                {
                    int intGtype = Convert.ToInt32(ResultGridView[11, i].Value);
                    int intT = Convert.ToInt32(ResultGridView[6, i].Value);
                    int intD = Convert.ToInt32(ResultGridView[7, i].Value);
                    int intR = intT + intD;
                    DateTime dto = DateTime.Parse(ResultGridView[4, i].Value.ToString());
                    dto = dto.AddYears(-543);
                    string strdto = dto.ToString("yyyyMM") + ResultGridView[0, i].Value;
                    ResultGridView[0, i].Value = strdto;

                    int intID = Convert.ToInt32(ResultGridView[3, i].Value);
                    if (intID == 0)
                        ResultGridView[3, i].Value = "";
                    else if (intID > 100 && intID < 104)
                        ResultGridView[3, i].Value = AppGlobalVariables.DispensersById[intID];
                    else
                        ResultGridView[3, i].Value = AppGlobalVariables.UsersById[intID];

                    intID = Convert.ToInt32(ResultGridView[5, i].Value);
                    if (intID == 0)
                        ResultGridView[5, i].Value = "";
                    else if (intID > 100 && intID < 104)
                        ResultGridView[3, i].Value = AppGlobalVariables.DispensersById[intID];
                    else
                        ResultGridView[5, i].Value = AppGlobalVariables.UsersById[intID];

                    if (intGtype == 0)
                    {
                        ResultGridView[6, i].Value = intR.ToString("#0.00");
                        ResultGridView[7, i].Value = "";
                        ResultGridView[8, i].Value = "";
                        intSumE0 += intR;
                    }
                    else if (intGtype == 1)
                    {
                        ResultGridView[6, i].Value = "";
                        ResultGridView[7, i].Value = intR.ToString("#0.00");
                        ResultGridView[8, i].Value = "";
                        intSumE1 += intR;
                    }
                    else if (intGtype == 2)
                    {
                        ResultGridView[6, i].Value = "";
                        ResultGridView[7, i].Value = "";
                        ResultGridView[8, i].Value = intR.ToString("#0.00");
                        intSumE2 += intR;
                    }
                    else if (intGtype == 200)
                    {
                        ResultGridView[6, i].Value = "";
                        ResultGridView[7, i].Value = "";
                        ResultGridView[8, i].Value = "";
                        intSumE2 += intR;
                    }
                    intSumR += intR;

                    float floT = (float)intT;
                    double floBV = (floT * 100) / 107;
                    floBV = Math.Round(floBV, 2);
                    double floV = floT - floBV;
                    ResultGridView[9, i].Value = floBV.ToString("#0.00");
                    ResultGridView[10, i].Value = floV.ToString("#0.00");
                    ResultGridView[11, i].Value = intR.ToString("#0.00");
                    intSumBV += floBV;
                    intSumV += floV;
                    string strProname = "";
                    intGtype = Convert.ToInt32(ResultGridView[12, i].Value);

                    if (intGtype > 0)
                        strProname = AppGlobalVariables.PromotionNamesById[intGtype];
                    ResultGridView[12, i].Value = strProname;
                }
                catch (Exception)
                {
                }
            }

            ResultGridView[3, intNo].Value = "จำนวนรถ";
            ResultGridView[4, intNo].Value = intNo.ToString() + " คัน";
            ResultGridView[5, intNo].Value = "ยอดรวม";
            ResultGridView[6, intNo].Value = intSumE0.ToString("#0.00");
            ResultGridView[7, intNo].Value = intSumE1.ToString("#0.00");
            ResultGridView[8, intNo].Value = intSumE2.ToString("#0.00");
            ResultGridView[9, intNo].Value = intSumBV.ToString("#0.00");
            ResultGridView[10, intNo].Value = intSumV.ToString("#0.00");
            ResultGridView[11, intNo].Value = intSumR.ToString("#0.00");
        }

        private void CaseReportPricePromotion()
        {
            ResultGridView.Columns[0].HeaderText = "เลขที่ใบเสร็จ/ใบกำกับภาษี";
            ResultGridView.Columns[1].HeaderText = "ลำดับ";
            ResultGridView.Columns[2].HeaderText = "ประเภท";
            ResultGridView.Columns[3].HeaderText = "ทะเบียน";
            ResultGridView.Columns[4].HeaderText = "เวลาเข้า";
            ResultGridView.Columns[5].HeaderText = "เจ้าหน้าที่ขาออก";
            ResultGridView.Columns[6].HeaderText = "เวลาออก";
            ResultGridView.Columns[7].HeaderText = "ชม.จอด";
            ResultGridView.Columns[8].HeaderText = "ชม.ส่วนลดผู้มาติดต่อ";
            ResultGridView.Columns[9].HeaderText = "ชม.ลด";
            ResultGridView.Columns[10].HeaderText = "ชม.จ่าย";
            ResultGridView.Columns[11].HeaderText = "ค่าปรับบัตรหาย";

            if (selectedReportId == 12 && Configs.Reports.UseReport13_11) //Mac 2017/11/30
            {
                ResultGridView.Columns[12].HeaderText = "เหตุผล";
            }
            else
                ResultGridView.Columns[12].HeaderText = "ค่าปรับข้ามวัน";
            ResultGridView.Columns[13].HeaderText = "รายได้";
            ResultGridView.Columns[14].HeaderText = "ส่วนลด"; //Mac 2016/03/05
            ResultGridView.Columns[15].HeaderText = "E-Stamp";
            ResultGridView.Columns[0].Width = 110;
            ResultGridView.Columns[1].Width = 50;
            ResultGridView.Columns[4].Width = 120;
            ResultGridView.Columns[5].Width = 160;
            ResultGridView.Columns[6].Width = 120;

            if (Configs.UseMemo)
            {
                ResultGridView.Columns[16].HeaderText = "บันทึกเพิ่มเติม"; //Mac 2016/03/05
                ResultGridView.Columns[16].Width = 160;
            }

            //if (selectedReportId == 13)
            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) //Mac 2018/02/23
            {
                ResultGridView.Columns[13].HeaderText = "รายได้ก่อนภาษี";
                ResultGridView.Columns[14].HeaderText = "ภาษี 7%";
                ResultGridView.Columns[15].HeaderText = "รายได้";
                ResultGridView.Columns[16].HeaderText = "E-Stamp";
                if (Configs.Reports.UseReport13_3)
                    ResultGridView.Columns[17].HeaderText = "เก็บจริง";
            }

            int intNo = ResultGridView.Rows.Count - 1;
            ResultGridView.Columns[11].Width = 105;
            ResultGridView.Columns[15].Width = 160;
            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) ResultGridView.Columns[16].Width = 160;
            int intSumPrice = 0;
            int intSumPriceLoss = 0;
            int intSumPriceOver = 0;
            int intSumDiscount = 0; //Mac 2016/03/05
            double doubleSumBeforeVat = 0;
            double doubleSumVat = 0;


            for (int i = 0; i < intNo; i++)
            {
                int intID = Convert.ToInt32(ResultGridView[0, i].Value);
                DateTime dto = DateTime.Parse(ResultGridView[6, i].Value.ToString());
                if (intID > 0)
                {
                    //Mac 2018/05/13
                    string fontSlip13 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip13 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName)
                            fontSlip13 = "IV";
                    }

                    if (Configs.UseReceiptFor1Out) //Mac 2018/11/14
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2016/04/27
                            ResultGridView[0, i].Value = ResultGridView[ResultGridView.ColumnCount - 1, i].Value.ToString() + dto.ToString("yyMM") + intID.ToString("00000#");
                        else
                            ResultGridView[0, i].Value = ResultGridView[ResultGridView.ColumnCount - 1, i].Value.ToString() + dto.ToString("yy") + intID.ToString("00000#");
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth) //Mac 2016/04/27
                        {
                            ResultGridView[0, i].Value = fontSlip13 + dto.ToString("yyMM") + intID.ToString("00000#"); //Mac 2022/04/26
                        }
                        else
                        {
                            ResultGridView[0, i].Value = fontSlip13 + dto.ToString("yy") + intID.ToString("00000#"); //Mac 2018/05/13
                        }
                    }
                }

                try
                {
                    string x = ResultGridView[2, i].Value.ToString();
                    int value;
                    if (int.TryParse(x, out value))
                    {
                        intID = value;
                        ResultGridView[2, i].Value = AppGlobalVariables.CarTypesById[intID];
                    }
                    else
                        ResultGridView[2, i].Value = x;

                }
                catch
                {
                    ResultGridView[2, i].Value = "";
                }


                try
                {
                    intID = Convert.ToInt32(ResultGridView[5, i].Value);
                    if (intID == 0)
                        ResultGridView[5, i].Value = "";
                    else
                        ResultGridView[5, i].Value = AppGlobalVariables.UsersById[intID];
                }
                catch
                {
                    ResultGridView[5, i].Value = "";
                }

                DateTime dti = DateTime.Parse(ResultGridView[4, i].Value.ToString());
                TimeSpan diffTime = dto - dti;
                int intHour = diffTime.Hours;
                if (diffTime.Days > 0)
                    intHour += diffTime.Days * 24;
                if (diffTime.Minutes > 0) //Mac 2022/11/03
                    intHour++;
                ResultGridView[7, i].Value = intHour.ToString();

                //Mac 2017/06/07
                string totalInOut = "";
                if (diffTime.Days == 0 && diffTime.Hours == 0 && diffTime.Minutes == 0)
                    totalInOut = "0";
                else
                    totalInOut = (diffTime.Days * 24) + diffTime.Hours + "." + diffTime.Minutes.ToString("00");

                ResultGridView[7, i].Value = totalInOut;

                try //Mac 2018/01/16
                {
                    if (Configs.UseProIDAll) //Mac 2015/10/16
                    {
                        string[] ProIDAll;
                        int intHourPro = 0;

                        //if (selectedReportId == 13)
                        if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) //Mac 2018/02/23
                        {
                            ProIDAll = ResultGridView[16, i].Value.ToString().Split(',');
                            ResultGridView[16, i].Value = "";
                        }
                        else
                        {
                            /*ProIDAll = ResultGridView[14, i].Value.ToString().Split(',');
                            ResultGridView[14, i].Value = "";*/
                            ProIDAll = ResultGridView[15, i].Value.ToString().Split(',');
                            ResultGridView[15, i].Value = ""; //Mac 2016/03/05
                        }

                        for (int n = 0; n < ProIDAll.Length; n++)
                        {
                            if (ProIDAll[n].Length > 0)
                            {
                                intHourPro += AppGlobalVariables.PromotionNamesMinuteMap[Convert.ToInt16(ProIDAll[n])];

                                //if (selectedReportId == 13)
                                if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) //Mac 2018/02/23
                                    ResultGridView[16, i].Value += AppGlobalVariables.PromotionNamesById[Convert.ToInt16(ProIDAll[n])];
                                else
                                    ResultGridView[15, i].Value += AppGlobalVariables.PromotionNamesById[Convert.ToInt16(ProIDAll[n])]; //Mac 2016/03/05

                                if (n < (ProIDAll.Length - 2))
                                {
                                    if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) //Mac 2018/02/23
                                        ResultGridView[16, i].Value += "|"; //Mac 2016/03/05
                                    else
                                        ResultGridView[15, i].Value += "|"; //Mac 2016/03/05
                                }

                            }
                        }

                        intHourPro = intHourPro / 60;

                        ResultGridView[8, i].Value = intHourPro.ToString();
                        if (intHourPro < intHour)
                        {
                            ResultGridView[9, i].Value = intHourPro.ToString();//ลด
                            ResultGridView[10, i].Value = (intHour - intHourPro).ToString();//จ่าย
                        }
                        else
                        {
                            ResultGridView[9, i].Value = intHour.ToString();//ลด
                            ResultGridView[10, i].Value = "0";//จ่าย
                        }
                    }
                    else
                    {
                        //if (selectedReportId == 13) intID = Convert.ToInt32(ResultGridView[16, i].Value);
                        //Mac 2018/02/23
                        if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) intID = Convert.ToInt32(ResultGridView[16, i].Value); //Mac 2018/02/23
                        else intID = Convert.ToInt32(ResultGridView[15, i].Value); //Mac 2016/03/05
                        //else intID = Convert.ToInt32(ResultGridView[14, i].Value);

                        int intHourPro = 0;
                        if (intID > 0)
                            intHourPro = AppGlobalVariables.PromotionNamesMinuteMap[intID] / 60;
                        ResultGridView[8, i].Value = intHourPro.ToString();
                        if (intHourPro < intHour)
                        {
                            ResultGridView[9, i].Value = intHourPro.ToString();//ลด
                            ResultGridView[10, i].Value = (intHour - intHourPro).ToString();//จ่าย
                        }
                        else
                        {
                            ResultGridView[9, i].Value = intHour.ToString();//ลด
                            ResultGridView[10, i].Value = "0";//จ่าย
                        }

                        if (intID > 0)
                        {
                            //if (selectedReportId == 13) ResultGridView[16, i].Value = AppGlobalVariables.PromotionNamesById[intID];
                            //Mac 2018/02/24
                            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) ResultGridView[16, i].Value = AppGlobalVariables.PromotionNamesById[intID];
                            else ResultGridView[15, i].Value = AppGlobalVariables.PromotionNamesById[intID]; //Mac 2016/03/05
                            //else ResultGridView[14, i].Value = AppGlobalVariables.PromotionNamesById[intID];
                        }
                        else
                        {
                            //if (selectedReportId == 13) ResultGridView[16, i].Value = "";
                            //Mac 2018/02/24
                            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) ResultGridView[16, i].Value = "";
                            else ResultGridView[15, i].Value = ""; //Mac 2016/03/05
                            //else ResultGridView[14, i].Value = "";
                        }
                    }
                }
                catch
                {
                    ResultGridView[8, i].Value = "0";
                    ResultGridView[9, i].Value = "0";
                    ResultGridView[10, i].Value = "0";
                    //if (selectedReportId == 13)
                    if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) //Mac 2018/02/24
                        ResultGridView[16, i].Value = "";
                    else
                        ResultGridView[15, i].Value = "";
                }

                //Golf2014/10/08
                // (selectedReportId == 13)
                if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) //Mac 2018/02/24
                {
                    try
                    {
                        /*double beforeVat = double.Parse(ResultGridView[15, i].Value.ToString()) * 100 / 107;
                        beforeVat = Math.Round(beforeVat, 2);
                        double vat = double.Parse(ResultGridView[15, i].Value.ToString()) - beforeVat;*/

                        double vat = (double.Parse(ResultGridView[15, i].Value.ToString()) * 7) / 107;

                        if (Configs.Reports.Report3Decimal) //Mac 2016/10/06
                            vat = Math.Round(vat, 3);
                        else
                            vat = Math.Round(vat, 2);

                        double beforeVat = double.Parse(ResultGridView[15, i].Value.ToString()) - vat;

                        if (Configs.Reports.Report3Decimal) //Mac 2016/10/06
                        {
                            ResultGridView[13, i].Value = beforeVat.ToString("#,###,##0.000");
                            ResultGridView[14, i].Value = vat.ToString("#,###,##0.000");
                        }
                        else
                        {
                            ResultGridView[13, i].Value = beforeVat.ToString("#,###,##0.00");
                            ResultGridView[14, i].Value = vat.ToString("#,###,##0.00");
                        }

                        /*ResultGridView[13, i].Value = beforeVat.ToString("#0.0000");
                        ResultGridView[14, i].Value = vat.ToString("#0.0000");*/
                        doubleSumBeforeVat += beforeVat;
                        doubleSumVat += vat;
                    }
                    catch (Exception) { }
                }


                intSumPriceLoss += Convert.ToInt32(ResultGridView[11, i].Value);
                if (selectedReportId == 12 && Configs.Reports.UseReport13_11) //Mac 2017/11/30
                {

                }
                else
                    intSumPriceOver += Convert.ToInt32(ResultGridView[12, i].Value);
                //if (selectedReportId == 13)
                if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) //Mac 2018/02/24
                {
                    intSumPrice += Convert.ToInt32(ResultGridView[15, i].Value);

                    if (Convert.ToInt32(ResultGridView[15, i].Value) == 0) //Mac 2017/07/12
                        ResultGridView[10, i].Value = "0";

                    if (Configs.Reports.UseReport13_3) //Mac 2018/04/18
                    {
                        if (ResultGridView[17, i].Value.ToString().Trim() == "")
                            ResultGridView[17, i].Value = ResultGridView[15, i].Value;
                    }
                }
                else
                {
                    intSumPrice += Convert.ToInt32(ResultGridView[13, i].Value);
                    intSumDiscount += Convert.ToInt32(ResultGridView[14, i].Value); //Mac 2016/03/05

                    if (Convert.ToInt32(ResultGridView[13, i].Value) == 0) //Mac 2017/07/12
                        ResultGridView[10, i].Value = "0";
                }
            }
            ResultGridView[5, intNo].Value = "จำนวนรถ";
            ResultGridView[6, intNo].Value = intNo.ToString("#,###,##0") + " คัน";
            ResultGridView[10, intNo].Value = "รายได้รวม";
            ResultGridView[11, intNo].Value = intSumPriceLoss.ToString("#,###,##0");
            ResultGridView[12, intNo].Value = intSumPriceOver.ToString("#,###,##0");

            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
            {
                if (Configs.UseCalVatFromTotal)
                {
                    ResultGridView[13, intNo].Value = (Convert.ToDouble(intSumPrice) - (Convert.ToDouble(intSumPrice) * 7 / 107)).ToString("#,###,##0.00");
                    ResultGridView[14, intNo].Value = (Convert.ToDouble(intSumPrice) * 7 / 107).ToString("#,###,##0.00");
                    ResultGridView[15, intNo].Value = intSumPrice.ToString("#,###,##0");
                }
                else
                {
                    if (Configs.Reports.Report3Decimal)
                    {
                        ResultGridView[13, intNo].Value = doubleSumBeforeVat.ToString("#,###,##0.000");
                        ResultGridView[14, intNo].Value = doubleSumVat.ToString("#,###,##0.000");
                    }
                    else
                    {
                        ResultGridView[13, intNo].Value = doubleSumBeforeVat.ToString("#,###,##0.00");
                        ResultGridView[14, intNo].Value = doubleSumVat.ToString("#,###,##0.00");
                    }
                    ResultGridView[15, intNo].Value = intSumPrice.ToString("#,###,##0");
                }
            }
            else
            {
                ResultGridView[13, intNo].Value = intSumPrice.ToString("#,###,##0");
                ResultGridView[14, intNo].Value = intSumDiscount.ToString("#,###,##0");
            }
            totalLoss = intSumPriceLoss;
            totalOver = intSumPriceOver;
            totalPrice = intSumPrice;
            totalDiscount = intSumDiscount;
            totalBeforeVat = doubleSumBeforeVat;
            totalVat = doubleSumVat;

            if (Configs.UseReceiptFor1Out)
                ResultGridView.Columns[ResultGridView.ColumnCount - 1].Visible = false;
        }

        private void CaseReportPricePromotion13_12()
        {
            ResultGridView.Columns[0].HeaderText = "เครื่องรับเงิน online";
            ResultGridView.Columns[1].HeaderText = "เลขที่ใบเสร็จ/ใบกำกับภาษี";
            ResultGridView.Columns[2].HeaderText = "ลำดับ";
            ResultGridView.Columns[3].HeaderText = "ประเภท";
            ResultGridView.Columns[4].HeaderText = "ทะเบียน";
            ResultGridView.Columns[5].HeaderText = "เวลาเข้า";
            ResultGridView.Columns[6].HeaderText = "เจ้าหน้าที่ขาออก";
            ResultGridView.Columns[7].HeaderText = "เวลาออก";
            ResultGridView.Columns[8].HeaderText = "ชม.จอด";
            ResultGridView.Columns[9].HeaderText = "ชม.โปรโมชั่น";
            ResultGridView.Columns[10].HeaderText = "ชม.ลด";
            ResultGridView.Columns[11].HeaderText = "ชม.จ่าย";
            ResultGridView.Columns[12].HeaderText = "ค่าปรับบัตรหาย";
            if (selectedReportId == 12 && Configs.Reports.UseReport13_11)
            {
                ResultGridView.Columns[13].HeaderText = "เหตุผล";
            }
            else
                ResultGridView.Columns[13].HeaderText = "ค่าปรับข้ามวัน";
            ResultGridView.Columns[14].HeaderText = "รายได้";
            ResultGridView.Columns[15].HeaderText = "ส่วนลด";
            ResultGridView.Columns[16].HeaderText = "E-Stamp";

            ResultGridView.Columns[0].Width = 110;
            ResultGridView.Columns[1].Width = 110;
            ResultGridView.Columns[2].Width = 50;
            ResultGridView.Columns[5].Width = 120;
            ResultGridView.Columns[6].Width = 160;
            ResultGridView.Columns[7].Width = 120;

            if (Configs.UseMemo)
            {
                ResultGridView.Columns[17].HeaderText = "บันทึกเพิ่มเติม";
                ResultGridView.Columns[17].Width = 160;
            }

            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
            {
                ResultGridView.Columns[14].HeaderText = "รายได้ก่อนภาษี";
                ResultGridView.Columns[15].HeaderText = "ภาษี 7%";
                ResultGridView.Columns[16].HeaderText = "รายได้";
                ResultGridView.Columns[17].HeaderText = "E-Stamp";
                if (Configs.Reports.UseReport13_3)
                    ResultGridView.Columns[18].HeaderText = "เก็บจริง";
            }

            int intNo = ResultGridView.Rows.Count - 1;
            ResultGridView.Columns[12].Width = 105;
            ResultGridView.Columns[16].Width = 160;

            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) ResultGridView.Columns[17].Width = 160;
            int intSumPrice = 0;
            int intSumPriceLoss = 0;
            int intSumPriceOver = 0;
            int intSumDiscount = 0;
            double doubleSumBeforeVat = 0;
            double doubleSumVat = 0;

            for (int i = 0; i < intNo; i++)
            {
                int intID = Convert.ToInt32(ResultGridView[1, i].Value);
                DateTime dto = DateTime.Parse(ResultGridView[7, i].Value.ToString());
                if (intID > 0)
                {
                    string fontSlip13 = "";
                    if (AppGlobalVariables.Printings.ReceiptName.Length > 0)
                        fontSlip13 = AppGlobalVariables.Printings.ReceiptName;
                    else
                    {
                        if (!Configs.UseReceiptName)
                            fontSlip13 = "IV";
                    }

                    if (Configs.UseReceiptFor1Out)
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            ResultGridView[1, i].Value = ResultGridView[ResultGridView.ColumnCount - 1, i].Value.ToString() + dto.ToString("yyMM") + intID.ToString("00000#"); //Mac 2022/04/26
                        }
                        else
                        {
                            ResultGridView[1, i].Value = ResultGridView[ResultGridView.ColumnCount - 1, i].Value.ToString() + dto.ToString("yy") + intID.ToString("00000#");
                        }
                    }
                    else
                    {
                        if (Configs.OutReceiptNameMonth)
                        {
                            ResultGridView[1, i].Value = fontSlip13 + dto.ToString("yyMM") + intID.ToString("00000#");
                        }
                        else
                        {
                            ResultGridView[1, i].Value = fontSlip13 + dto.ToString("yy") + intID.ToString("00000#");
                        }
                    }
                }

                try
                {
                    string x = ResultGridView[3, i].Value.ToString();
                    int value;
                    if (int.TryParse(x, out value))
                    {
                        intID = value;
                        ResultGridView[3, i].Value = AppGlobalVariables.CarTypesById[intID];
                    }
                    else
                        ResultGridView[3, i].Value = x;
                }
                catch
                {
                    ResultGridView[3, i].Value = "";
                }


                try
                {
                    intID = Convert.ToInt32(ResultGridView[6, i].Value);
                    if (intID == 0)
                        ResultGridView[6, i].Value = "";
                    else
                        ResultGridView[6, i].Value = AppGlobalVariables.UsersById[intID];
                }
                catch
                {
                    ResultGridView[6, i].Value = "";
                }

                DateTime dti = DateTime.Parse(ResultGridView[5, i].Value.ToString());
                TimeSpan diffTime = dto - dti;
                int intHour = diffTime.Hours;
                if (diffTime.Days > 0)
                    intHour += diffTime.Days * 24;
                if (diffTime.Minutes > 0)
                    intHour++;
                ResultGridView[8, i].Value = intHour.ToString();

                string totalInOut = "";
                if (diffTime.Days == 0 && diffTime.Hours == 0 && diffTime.Minutes == 0)
                    totalInOut = "0";
                else
                    totalInOut = (diffTime.Days * 24) + diffTime.Hours + "." + diffTime.Minutes.ToString("00");

                ResultGridView[8, i].Value = totalInOut;

                try
                {
                    if (Configs.UseProIDAll)
                    {
                        string[] ProIDAll;
                        int intHourPro = 0;

                        if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                        {
                            ProIDAll = ResultGridView[17, i].Value.ToString().Split(',');
                            ResultGridView[17, i].Value = "";
                        }
                        else
                        {
                            ProIDAll = ResultGridView[16, i].Value.ToString().Split(',');
                            ResultGridView[16, i].Value = "";
                        }

                        for (int n = 0; n < ProIDAll.Length; n++)
                        {
                            if (ProIDAll[n].Length > 0)
                            {
                                intHourPro += AppGlobalVariables.PromotionNamesMinuteMap[Convert.ToInt16(ProIDAll[n])];

                                if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                                    ResultGridView[17, i].Value += AppGlobalVariables.PromotionNamesById[Convert.ToInt16(ProIDAll[n])];
                                else
                                    ResultGridView[16, i].Value += AppGlobalVariables.PromotionNamesById[Convert.ToInt16(ProIDAll[n])];

                                if (n < (ProIDAll.Length - 2))
                                {
                                    if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                                        ResultGridView[17, i].Value += "|";
                                    else
                                        ResultGridView[16, i].Value += "|";
                                }

                            }
                        }

                        intHourPro = intHourPro / 60;

                        ResultGridView[9, i].Value = intHourPro.ToString();
                        if (intHourPro < intHour)
                        {
                            ResultGridView[10, i].Value = intHourPro.ToString();
                            ResultGridView[11, i].Value = (intHour - intHourPro).ToString();
                        }
                        else
                        {
                            ResultGridView[10, i].Value = intHour.ToString();
                            ResultGridView[11, i].Value = "0";
                        }
                    }
                    else
                    {
                        if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) intID = Convert.ToInt32(ResultGridView[17, i].Value);
                        else intID = Convert.ToInt32(ResultGridView[16, i].Value);

                        int intHourPro = 0;
                        if (intID > 0)
                            intHourPro = AppGlobalVariables.PromotionNamesMinuteMap[intID] / 60;
                        ResultGridView[9, i].Value = intHourPro.ToString();
                        if (intHourPro < intHour)
                        {
                            ResultGridView[10, i].Value = intHourPro.ToString();
                            ResultGridView[11, i].Value = (intHour - intHourPro).ToString();
                        }
                        else
                        {
                            ResultGridView[10, i].Value = intHour.ToString();
                            ResultGridView[11, i].Value = "0";
                        }

                        if (intID > 0)
                        {

                            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) ResultGridView[17, i].Value = AppGlobalVariables.PromotionNamesById[intID];
                            else ResultGridView[16, i].Value = AppGlobalVariables.PromotionNamesById[intID];
                        }
                        else
                        {
                            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13) ResultGridView[17, i].Value = "";
                            else ResultGridView[16, i].Value = "";
                        }
                    }
                }
                catch
                {
                    ResultGridView[9, i].Value = "0";
                    ResultGridView[10, i].Value = "0";
                    ResultGridView[11, i].Value = "0";

                    if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                        ResultGridView[17, i].Value = "";
                    else
                        ResultGridView[16, i].Value = "";
                }

                if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                {
                    try
                    {
                        double vat = (double.Parse(ResultGridView[16, i].Value.ToString()) * 7) / 107;

                        if (Configs.Reports.Report3Decimal)
                            vat = Math.Round(vat, 3);
                        else
                            vat = Math.Round(vat, 2);

                        double beforeVat = double.Parse(ResultGridView[16, i].Value.ToString()) - vat;

                        if (Configs.Reports.Report3Decimal)
                        {
                            ResultGridView[14, i].Value = beforeVat.ToString("#,###,##0.000");
                            ResultGridView[15, i].Value = vat.ToString("#,###,##0.000");
                        }
                        else
                        {
                            ResultGridView[14, i].Value = beforeVat.ToString("#,###,##0.00");
                            ResultGridView[15, i].Value = vat.ToString("#,###,##0.00");
                        }

                        doubleSumBeforeVat += beforeVat;
                        doubleSumVat += vat;
                    }
                    catch (Exception) { }
                }


                intSumPriceLoss += Convert.ToInt32(ResultGridView[12, i].Value);
                if (selectedReportId == 12 && Configs.Reports.UseReport13_11)
                {

                }
                else
                    intSumPriceOver += Convert.ToInt32(ResultGridView[13, i].Value);

                if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
                {
                    intSumPrice += Convert.ToInt32(ResultGridView[16, i].Value);

                    if (Convert.ToInt32(ResultGridView[16, i].Value) == 0)
                        ResultGridView[11, i].Value = "0";

                    if (Configs.Reports.UseReport13_3)
                    {
                        if (ResultGridView[18, i].Value.ToString().Trim() == "")
                            ResultGridView[18, i].Value = ResultGridView[16, i].Value;
                    }
                }
                else
                {
                    intSumPrice += Convert.ToInt32(ResultGridView[14, i].Value);
                    intSumDiscount += Convert.ToInt32(ResultGridView[15, i].Value);

                    if (Convert.ToInt32(ResultGridView[14, i].Value) == 0)
                        ResultGridView[11, i].Value = "0";
                }
            }
            ResultGridView[6, intNo].Value = "จำนวนรถ";
            ResultGridView[7, intNo].Value = intNo.ToString("#,###,##0") + " คัน";
            ResultGridView[11, intNo].Value = "รายได้รวม";
            ResultGridView[12, intNo].Value = intSumPriceLoss.ToString("#,###,##0");
            ResultGridView[13, intNo].Value = intSumPriceOver.ToString("#,###,##0");
            if (selectedReportId == 13 && !Configs.Reports.UseReport14like13)
            {
                if (Configs.UseCalVatFromTotal)
                {
                    ResultGridView[14, intNo].Value = (Convert.ToDouble(intSumPrice) - (Convert.ToDouble(intSumPrice) * 7 / 107)).ToString("#,###,##0.00");
                    ResultGridView[15, intNo].Value = (Convert.ToDouble(intSumPrice) * 7 / 107).ToString("#,###,##0.00");
                    ResultGridView[16, intNo].Value = intSumPrice.ToString("#,###,##0");
                }
                else
                {
                    if (Configs.Reports.Report3Decimal)
                    {
                        ResultGridView[14, intNo].Value = doubleSumBeforeVat.ToString("#,###,##0.000");
                        ResultGridView[15, intNo].Value = doubleSumVat.ToString("#,###,##0.000");
                    }
                    else
                    {
                        ResultGridView[14, intNo].Value = doubleSumBeforeVat.ToString("#,###,##0.00");
                        ResultGridView[15, intNo].Value = doubleSumVat.ToString("#,###,##0.00");
                    }
                    ResultGridView[16, intNo].Value = intSumPrice.ToString("#,###,##0");
                }
            }
            else
            {
                ResultGridView[14, intNo].Value = intSumPrice.ToString("#,###,##0");
                ResultGridView[15, intNo].Value = intSumDiscount.ToString("#,###,##0");
            }

            totalLoss = intSumPriceLoss;
            totalOver = intSumPriceOver;
            totalPrice = intSumPrice;
            totalDiscount = intSumDiscount;
            totalBeforeVat = doubleSumBeforeVat;
            totalVat = doubleSumVat;

            if (Configs.UseReceiptFor1Out)
                ResultGridView.Columns[ResultGridView.ColumnCount - 1].Visible = false;
        }
        #endregion

        private void FuckingShit(int selectedReportId, string sql)
        {
            string startDate = StartDatePicker.Value.ToString("yyyy-MM-dd");
            string endDate = EndDatePicker.Value.ToString("yyyy-MM-dd");
            string startTime = StartTimePicker.Value.ToLongTimeString();
            string endTime = EndTimePicker.Value.ToLongTimeString();
            string startDateTime = startDate + " " + startTime;
            string endDateTime = endDate + " " + endTime;

            ReportQueryService rqs = new ReportQueryService();

            switch (selectedReportId)
            {
                case 16:
                    try
                    {
                        PrimaryTabControl.SelectTab(1);
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        ReportDocument rpt = new ReportDocument();
                        rpt.Load(path + "\\CrystalReports\\Report17.rpt");

                        string nTel = "";
                        string nFax = "";
                        string nTax = "";
                        try
                        {
                            string tel = AppGlobalVariables.Printings.Telephone;
                            int t = tel.IndexOf(" แฟ") - tel.IndexOf("L: ");
                            nTel = tel.Substring(tel.IndexOf("L: ") + 2, t - 2);
                            t = tel.IndexOf("AX:");
                            nFax = tel.Substring(t + 3);
                            tel = AppGlobalVariables.Printings.Tax1;
                            t = tel.IndexOf("D. ");
                            nTax = tel.Substring(t + 3);
                        }
                        catch
                        {
                            try
                            {
                                nTel = AppGlobalVariables.Printings.Telephone.Split(':')[1].Trim().Replace("fax", "").Replace("FAX", "").Replace("Fax", "");
                            }
                            catch
                            {
                                nTel = "";
                            }
                            try
                            {
                                nFax = AppGlobalVariables.Printings.Telephone.Split(':')[2].Trim();
                            }
                            catch
                            {
                                nFax = "";
                            }
                            try
                            {
                                nTax = AppGlobalVariables.Printings.Tax1.Split(':')[1].Trim();
                            }
                            catch
                            {
                                nTax = "";
                            }
                            if (nTax.Trim().Length == 0) //Mac 2022/08/31
                            {
                                try
                                {
                                    nTax = AppGlobalVariables.Printings.Tax1.Split(' ')[1].Trim();
                                }
                                catch
                                {
                                    nTax = "";
                                }
                            }
                        }
                        rpt.SetParameterValue("compName", AppGlobalVariables.Printings.Company1.Trim());
                        rpt.SetParameterValue("ComAddress1", AppGlobalVariables.Printings.Address1.Trim() + "\r\n" + AppGlobalVariables.Printings.Address2.Trim());
                        rpt.SetParameterValue("ComTel", nTel);
                        rpt.SetParameterValue("comFax", nFax);
                        rpt.SetParameterValue("compTax", nTax);
                        rpt.SetParameterValue("DateSearch", startDateTime);
                        rpt.SetParameterValue("DateSearch2", endDateTime);

                        sql = "SELECT COUNT(recordin.no) "
                        + " FROM recordout JOIN recordin ON  recordin.no = recordout.no "
                        + " WHERE recordout.proid = 0 AND  recordin.cartype != 200 AND  recordout.price = 0 AND recordout.losscard = 0 "
                        + " AND recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'";
                        DataTable dt = DbController.LoadData(sql);
                        if (dt != null && dt.Rows.Count > 0)
                            rpt.SetParameterValue("01", dt.Rows[0].ItemArray[0].ToString());
                        else rpt.SetParameterValue("01", "'0'");

                        sql = "SELECT COUNT(recordin.no) "
                        + " FROM recordout JOIN recordin ON  recordin.no = recordout.no "
                        + " WHERE recordout.proid = 0 AND  recordin.cartype != 200 AND  recordout.price > 0 AND recordout.losscard = 0 "
                        + " AND recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "' ";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("02", dt.Rows[0].ItemArray[0].ToString());


                        sql = "SELECT SUM(recordout.price) "
                        + " FROM recordout JOIN recordin ON  recordin.no = recordout.no "
                        + " WHERE recordout.proid = 0 AND  recordin.cartype != 200 AND  recordout.price > 0 AND recordout.losscard = 0 "
                        + " AND recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "' ";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("04", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT COUNT(recordin.no) "
                        + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
                        + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
                        + " recordout.price = 0 AND recordout.losscard = 0 AND recordout.proid NOT IN  "
                        + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
                        + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("05", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT COUNT(recordin.no) "
                        + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
                        + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
                        + " recordout.price = 0 AND recordout.losscard = 0 AND recordout.proid  IN  "
                        + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
                        + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("06", dt.Rows[0].ItemArray[0].ToString());

                        ///////////////////////////////////////////////
                        int total = 0;
                        try
                        {

                            sql = "SELECT value FROM param WHERE name = 'not_day'";
                            dt = DbController.LoadData(sql);
                            Boolean notDay = Convert.ToBoolean(dt.Rows[0].ItemArray[0].ToString());
                            sql = "SELECT truncate(TIMESTAMPDIFF(minute,recordin.datein,recordout.dateout),0), " //Mac 2017/02/07
                            + " recordout.proid "
                            + " , recordin.datein, recordout.dateout" //Mac 2017/12/06
                            + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no   "
                            + " WHERE recordout.proid > 0 AND recordin.cartype != 200 "
                            + " AND   recordout.price = 0 AND recordout.losscard = 0 "
                            + " AND recordout.proid  IN   (SELECT PromotionId FROM prosetprice GROUP BY PromotionId)  "
                            + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "';";
                            dt = DbController.LoadData(sql);

                            if (dt.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataTable dt2 = DbController.LoadData("select * from prosetprice where PromotionID = " + dt.Rows[j].ItemArray[1].ToString() + " order by no");
                                    if (dt2 != null && dt2.Rows.Count > 0)
                                    {
                                        AppGlobalVariables.IntTime = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntPriceMin = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntPriceHour = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntHourRound = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntExpense = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntOver = new int[dt2.Rows.Count];
                                        for (int i = 0; i < dt2.Rows.Count; i++)
                                        {
                                            //AppGlobalVariables.IntTime[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[3].ToString());
                                            if (i == 0) //Mac 2016/04/05
                                            {
                                                AppGlobalVariables.IntTime[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[3].ToString());
                                            }
                                            else
                                            {
                                                AppGlobalVariables.IntTime[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[3].ToString()) - Convert.ToInt32(dt2.Rows[i - 1].ItemArray[3].ToString());
                                            }
                                            AppGlobalVariables.IntPriceMin[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[4].ToString());
                                            AppGlobalVariables.IntPriceHour[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[5].ToString());
                                            AppGlobalVariables.IntHourRound[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[6].ToString());
                                            AppGlobalVariables.IntExpense[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[7].ToString());
                                            AppGlobalVariables.IntOver[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[8].ToString());
                                        }
                                        int intHour;
                                        int intMin;
                                        intHour = 0;
                                        intMin = Convert.ToInt32(dt.Rows[j].ItemArray[0].ToString());
                                        //--------------------------------- //Mac 2017/12/06
                                        int ZoneMin = 0;
                                        DataTable dt3 = DbController.LoadData("select * from prosetprice_zone where PromotionID = " + dt.Rows[j].ItemArray[1].ToString() + " order by no");
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

                                            var CalPriceZone = (dynamic)null;
                                            DateTime dti = DateTime.Parse(dt.Rows[j]["datein"].ToString());
                                            DateTime dto = DateTime.Parse(dt.Rows[j]["dateout"].ToString());
                                            DateTime dtInOne;
                                            DateTime dtOutOne;
                                            TimeSpan diffInOut = DateTime.Parse(dto.ToShortDateString()) - DateTime.Parse(dti.ToShortDateString());

                                            bool booNoRound = false; //Mac 2018/01/08
                                            booNoRound = false; //Mac 2018/01/08
                                            for (int x = 0; x < diffInOut.Days + 1; x++)
                                            {
                                                if (diffInOut.Days == 0)
                                                {
                                                    booNoRound = true; //Mac 2018/01/08
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

                                                CalPriceZone = CalculationsManager.CalPriceZoneOneDay(0, dtInOne.ToString(), dtOutOne.ToString(), ZoneStart, ZoneStop, 0, 0, 0, booNoRound);
                                                ZoneMin += CalPriceZone.Key;
                                            }
                                        }
                                        if (ZoneMin > 0)
                                        {
                                            intMin -= ZoneMin;
                                            total += CalculationsManager.CalPrice2(0, ZoneMin, notDay);
                                        }
                                        //--------------------------------- //Mac 2017/12/06
                                        total += CalculationsManager.CalPrice(intHour, intMin, notDay);
                                    }
                                }
                            }
                        }
                        catch (Exception) { }
                        rpt.SetParameterValue("07", total.ToString());
                        ///////////////////////////////////////////////

                        ////////////////////////////////////////////////////

                        int total2 = 0;
                        total = 0;
                        try
                        {
                            sql = "SELECT value FROM param WHERE name = 'not_day'";
                            dt = DbController.LoadData(sql);
                            Boolean notDay = Convert.ToBoolean(dt.Rows[0].ItemArray[0].ToString());
                            /*sql = "SELECT truncate(time_to_sec(timediff(recordout.dateout,recordin.datein))/60,0),"*/
                            sql = "SELECT truncate(TIMESTAMPDIFF(minute,recordin.datein,recordout.dateout),0)," //Mac 2017/02/07
                              + " recordout.proid, recordout.price "
                              + " , recordin.datein, recordout.dateout " //Mac 2017/12/06
                              + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no "
                              + " WHERE recordout.proid > 0 AND recordin.cartype != 200 "
                              + " AND   recordout.price > 0 AND recordout.losscard = 0 "
                              + " AND recordout.proid IN   (SELECT PromotionId FROM prosetprice GROUP BY PromotionId)  "
                              + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "' ";
                            dt = DbController.LoadData(sql);
                            if (dt.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataTable dt2 = DbController.LoadData("select * from prosetprice where PromotionID = " + dt.Rows[j].ItemArray[1].ToString() + " order by no");
                                    if (dt2 != null && dt2.Rows.Count > 0)
                                    {
                                        AppGlobalVariables.IntTime = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntPriceMin = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntPriceHour = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntHourRound = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntExpense = new int[dt2.Rows.Count];
                                        AppGlobalVariables.IntOver = new int[dt2.Rows.Count];
                                        for (int i = 0; i < dt2.Rows.Count; i++)
                                        {
                                            //AppGlobalVariables.IntTime[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[3].ToString());
                                            if (i == 0) //Mac 2016/04/05
                                            {
                                                AppGlobalVariables.IntTime[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[3].ToString());
                                            }
                                            else
                                            {
                                                AppGlobalVariables.IntTime[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[3].ToString()) - Convert.ToInt32(dt2.Rows[i - 1].ItemArray[3].ToString());
                                            }
                                            AppGlobalVariables.IntPriceMin[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[4].ToString());
                                            AppGlobalVariables.IntPriceHour[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[5].ToString());
                                            AppGlobalVariables.IntHourRound[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[6].ToString());
                                            AppGlobalVariables.IntExpense[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[7].ToString());
                                            AppGlobalVariables.IntOver[i] = Convert.ToInt32(dt2.Rows[i].ItemArray[8].ToString());
                                        }
                                        int intHour;
                                        int intMin;
                                        intHour = 0;
                                        intMin = Convert.ToInt32(dt.Rows[j].ItemArray[0].ToString());
                                        //--------------------------------- //Mac 2017/12/06
                                        int ZoneMin = 0;
                                        DataTable dt3 = DbController.LoadData("select * from prosetprice_zone where PromotionID = " + dt.Rows[j].ItemArray[1].ToString() + " order by no");
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

                                            var CalPriceZone = (dynamic)null;
                                            DateTime dti = DateTime.Parse(dt.Rows[j]["datein"].ToString());
                                            DateTime dto = DateTime.Parse(dt.Rows[j]["dateout"].ToString());
                                            DateTime dtInOne;
                                            DateTime dtOutOne;
                                            TimeSpan diffInOut = DateTime.Parse(dto.ToShortDateString()) - DateTime.Parse(dti.ToShortDateString());

                                            bool booNoRound = false; //Mac 2018/01/08
                                            booNoRound = false; //Mac 2018/01/08
                                            for (int x = 0; x < diffInOut.Days + 1; x++)
                                            {
                                                if (diffInOut.Days == 0)
                                                {
                                                    booNoRound = true; //Mac 2018/01/08
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

                                                CalPriceZone = CalculationsManager.CalPriceZoneOneDay(0, dtInOne.ToString(), dtOutOne.ToString(), ZoneStart, ZoneStop, 0, 0, 0, booNoRound);
                                                ZoneMin += CalPriceZone.Key;
                                            }
                                        }
                                        if (ZoneMin > 0)
                                        {
                                            intMin -= ZoneMin;
                                            total += CalculationsManager.CalPrice2(0, ZoneMin, notDay);
                                        }
                                        //--------------------------------- //Mac 2017/12/06
                                        total += CalculationsManager.CalPrice(intHour, intMin, notDay);
                                        total2 += Convert.ToInt32(dt.Rows[j].ItemArray[2].ToString());
                                    }
                                }
                            }
                        }
                        catch (Exception) { }
                        rpt.SetParameterValue("13", total.ToString());
                        rpt.SetParameterValue("14", total2.ToString());
                        ////////////////////////////////////////////////////

                        sql = "SELECT COUNT(recordin.no) "
                        + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
                        + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
                        + " recordout.price > 0 AND recordout.losscard = 0 AND recordout.proid NOT IN  "
                        + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
                        + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("09", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT SUM(recordout.price) "
                        + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
                        + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
                        + " recordout.price > 0 AND recordout.losscard = 0 AND recordout.proid NOT IN  "
                        + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
                        + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("11", dt.Rows[0].ItemArray[0].ToString());


                        sql = "SELECT COUNT(recordin.no) "
                        + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
                        + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
                        + " recordout.price > 0 AND recordout.losscard = 0 AND recordout.proid IN  "
                        + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
                        + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("12", dt.Rows[0].ItemArray[0].ToString());
                        if (dt.Rows[0].ItemArray[0].ToString().Trim() == "0")
                            rpt.SetParameterValue("12", "0");

                        sql = "SELECT COUNT(*) FROM cardmf";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("15", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT COUNT(*) FROM cardmf WHERE level > 1";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("16", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT COUNT(*) FROM cardmf WHERE level = 1";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("33", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT COUNT(*) FROM cardmf WHERE level = 0";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("17", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT COUNT(*) FROM liftrecord WHERE datelift BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("24", dt.Rows[0].ItemArray[0].ToString());

                        /*sql = "SELECT COUNT(recordin.no) "
                        + " FROM recordout JOIN recordin ON  recordin.no = recordout.no   "
                        + " WHERE  recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                        + " AND timediff(recordout.dateout, recordin.datein) > '12:00:00';";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("20", dt.Rows[0].ItemArray[0].ToString());*/

                        //Mac 2016/01/06
                        sql = "SELECT COUNT(t1.no)"
                        + " FROM recordin t1 LEFT JOIN recordout t2 ON t1.no = t2.no"
                        //+ " WHERE t1.datein BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                        + " WHERE t1.datein <= '" + endDateTime + "'" //Mac 2016/02/01
                        + " AND t2.no IS null"
                        + " AND timediff(NOW(), t1.datein) > '12:00:00';";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("20", dt.Rows[0].ItemArray[0].ToString());

                        /*sql = "SELECT COUNT(recordin.no) "
                        + " FROM recordout JOIN recordin ON  recordin.no = recordout.no   "
                        + " WHERE  recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                        + " AND timediff(recordout.dateout, recordin.datein) > '12:00:00'"
                        + " AND recordin.cartype != 200;";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("23", dt.Rows[0].ItemArray[0].ToString());*/

                        //Mac 2016/01/06
                        sql = "SELECT COUNT(t1.no)"
                        + " FROM recordin t1 LEFT JOIN recordout t2 ON t1.no = t2.no"
                        //+ " WHERE t1.datein BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                        + " WHERE t1.datein <= '" + endDateTime + "'" //Mac 2016/02/01
                        + " AND t2.no IS null"
                        + " AND timediff(NOW(), t1.datein) > '12:00:00'"
                        + " AND t1.cartype != 200;";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("23", dt.Rows[0].ItemArray[0].ToString());

                        /*sql = "SELECT COUNT(recordin.no) "
                        + " FROM recordout JOIN recordin ON  recordin.no = recordout.no   "
                        + " WHERE  recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                        + " AND timediff(recordout.dateout, recordin.datein) > '12:00:00'"
                        + " AND recordin.cartype = 200;";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("34", dt.Rows[0].ItemArray[0].ToString());*/

                        //Mac 2016/01/06
                        sql = "SELECT COUNT(t1.no)"
                        + " FROM recordin t1 LEFT JOIN recordout t2 ON t1.no = t2.no"
                        //+ " WHERE t1.datein BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                        + " WHERE t1.datein <= '" + endDateTime + "'" //Mac 2016/02/01
                        + " AND t2.no IS null"
                        + " AND timediff(NOW(), t1.datein) > '12:00:00'"
                        + " AND t1.cartype = 200;";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("34", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT SUM(recordout.price) FROM recordout "
                        + " WHERE losscard > 0 AND recordout.dateout BETWEEN '"
                        + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        string losscard = dt.Rows[0].ItemArray[0].ToString();
                        Console.WriteLine(losscard);
                        rpt.SetParameterValue("19", "0");
                        if (losscard.Trim() != "")
                            rpt.SetParameterValue("19", losscard);


                        //Golf2014/10/09
                        sql = "SELECT COUNT(recordout.price) FROM recordout "
                        + " WHERE losscard > 0 AND recordout.dateout BETWEEN '"
                        + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        losscard = dt.Rows[0].ItemArray[0].ToString();
                        Console.WriteLine(losscard);
                        rpt.SetParameterValue("18", "0");
                        if (losscard.Trim() != "")
                            rpt.SetParameterValue("18", losscard);



                        sql = "SELECT COUNT(printno) FROM recordout WHERE printno > 0 AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("25", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT MIN(printno) FROM recordout WHERE printno > 0 AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("26", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT MAX(printno) FROM recordout WHERE printno > 0 AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("27", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT  COUNT(recordin.id) "
                        + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
                        + " WHERE recordin.cartype != 200 AND recordout.losscard = 0 AND "
                        + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("31", dt.Rows[0].ItemArray[0].ToString());
                        rpt.SetParameterValue("32", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT  COUNT(recordin.id)  "
                        + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
                        + " WHERE recordin.cartype = 200 AND "
                        + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("28", dt.Rows[0].ItemArray[0].ToString());
                        rpt.SetParameterValue("29", dt.Rows[0].ItemArray[0].ToString());

                        //Mac 2016/05/30
                        sql = "SELECT if(SUM(recordout.price) is null,0,SUM(recordout.price)) "
                        + " FROM recordout JOIN recordin ON recordin.no = recordout.no "
                        + " WHERE recordin.cartype = 200 AND  recordout.price > 0 AND recordout.losscard = 0 "
                        + " AND recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "' ";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("35", dt.Rows[0].ItemArray[0].ToString());

                        //Mac 2016/02/01
                        try
                        {
                            sql = "SELECT COUNT(*)"
                            + " FROM liftrecord"
                            + " WHERE datelift BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                            + " AND confirm1 IS null;";
                            dt = DbController.LoadData(sql);
                            if (dt.Rows[0].ItemArray[0].ToString() == "0")
                                rpt.SetParameterValue("Verified1", "Verified 1");
                        }
                        catch
                        {
                            rpt.SetParameterValue("Verified1", "");
                        }

                        //Mac 2016/02/01
                        try
                        {
                            sql = "SELECT COUNT(*)"
                            + " FROM liftrecord"
                            + " WHERE datelift BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                            + " AND confirm2 IS null;";
                            dt = DbController.LoadData(sql);
                            if (dt.Rows[0].ItemArray[0].ToString() == "0")
                                rpt.SetParameterValue("Verified2", "Verified 2");
                        }
                        catch
                        {
                            rpt.SetParameterValue("Verified2", "");
                        }
                        PrimaryCrystalReportViewer.ReportSource = rpt;
                        PrimaryCrystalReportViewer.Refresh();
                        Cursor = Cursors.Default;
                    }
                    catch (Exception)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("ไม่พบข้อมูล");
                    }
                    return;

                case 17:
                    sql = "select recordin.no as no, recordin.license as license,  "
                    + " CASE  "
                    + "      WHEN recordin.cartype = 200  "
                    + "   THEN CASE  "
                    + "         WHEN (SELECT level FROM cardmf WHERE name =  recordin.id) = 0 THEN 'VIP' "
                    + "      WHEN (SELECT level FROM cardmf WHERE name =  recordin.id) = 2 THEN 'VIP' "
                    + "    WHEN (SELECT level FROM cardmf WHERE name =  recordin.id) = 3 THEN 'Member' "
                    + "   END "
                    + "   WHEN recordin.cartype != 200  "
                    + "      THEN  "
                    + "     CASE WHEN recordout.losscard > 0 THEN 'Lost' "
                    + "          ELSE 'Visitor' "
                    + "    END  "
                    + " END as Membername,  "
                    + " recordin.datein as datein, recordout.dateout as dateout,  "
                    + " TRUNCATE(TIMESTAMPDIFF(minute, recordin.datein,recordout.dateout),0) as tdf,  " //Mac 2017/02/07
                    + " recordout.proid as proid, promotion.name as proname,  recordout.price as price,  "
                    + "  recordout.printno as printno, recordin.userin as userin, (select name from user where id =  recordout.userout) as userout   "
                    + " from recordin join recordout on recordin.no = recordout.no "
                    + " left join promotion ON promotion.id = recordout.proid "
                    + "  WHERE recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "' ";
                    if (UserComboBox.SelectedIndex > 0)
                    {

                        sql += " AND recordout.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == UserComboBox.Text).Key;// +" OR recordout.userout =" + dicUserInt[cobUserName.Text];
                    }
                    if (LicensePlateTextBox.Text != "")
                        sql += " AND recordin.license LIKE '%" + LicensePlateTextBox.Text + "%'";
                    if (CardIdTextBox.Text != "")
                        sql += " AND recordin.id = " + CardIdTextBox.Text;

                    if (PromotionComboBox.SelectedIndex > 0)
                    {
                        sql += " AND recordout.proid =" + AppGlobalVariables.PromotionNamesById.First(kvp => kvp.Value == PromotionComboBox.Text).Key;// +" OR recordout.userout =" + dicUserInt[cobUserName.Text];
                    }
                    if (CarTypeComboBox.SelectedIndex > 1)
                    {
                        sql += " AND recordin.cartype =" + AppGlobalVariables.CarTypesById.First(kvp => kvp.Value == CarTypeComboBox.Text).Key;
                    }
                    if (CarTypeComboBox.SelectedIndex == 1)
                    {
                        sql += " AND recordin.cartype != 200";
                    }
                    try
                    {
                        DataTable dtLoad = DbController.LoadData(sql);
                        sql = "SELECT value FROM param WHERE name = 'not_day'";
                        DataTable dtB = DbController.LoadData(sql);
                        Boolean notDay = Convert.ToBoolean(dtB.Rows[0].ItemArray[0].ToString());
                        DataTable dtMap = new DataTable("dtall");  //*** DataTable Map DataSet.xsd ***//
                        DataRow dr = null;
                        dtMap.Columns.Add(new DataColumn("no", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("datein", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("dateout", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("ParkTime", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("proid", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("Proname", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("PayPrice", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("printno", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("userin", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("userout", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("Membername", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("id", typeof(string)));
                        dtMap.Columns.Add(new DataColumn("PriceList", typeof(string)));
                        if (dtLoad.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtLoad.Rows.Count; i++)
                            {
                                dr = dtMap.NewRow();
                                dr["no"] = dtLoad.Rows[i]["no"];
                                dr["id"] = dtLoad.Rows[i]["license"];
                                dr["Membername"] = dtLoad.Rows[i]["Membername"];
                                dr["datein"] = dtLoad.Rows[i]["datein"];
                                dr["dateout"] = dtLoad.Rows[i]["dateout"];
                                dr["ParkTime"] = dtLoad.Rows[i]["tdf"];
                                dr["proid"] = dtLoad.Rows[i]["proid"];
                                dr["Proname"] = dtLoad.Rows[i]["proname"];


                                //Cal from minute
                                dr["PriceList"] = "0";
                                DataTable dt2 = DbController.LoadData("select * from prosetprice where PromotionID = " + dtLoad.Rows[i]["proid"].ToString() + " order by no");
                                if (dt2 != null && dt2.Rows.Count > 0)
                                {
                                    AppGlobalVariables.IntTime = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntPriceMin = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntPriceHour = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntHourRound = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntExpense = new int[dt2.Rows.Count];
                                    AppGlobalVariables.IntOver = new int[dt2.Rows.Count];
                                    for (int j = 0; j < dt2.Rows.Count; j++)
                                    {
                                        if (j == 0) //Mac 2016/04/05
                                        {
                                            AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[3].ToString());
                                        }
                                        else
                                        {
                                            AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[3].ToString()) - Convert.ToInt32(dt2.Rows[j - 1].ItemArray[3].ToString());
                                        }
                                        AppGlobalVariables.IntPriceMin[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[4].ToString());
                                        AppGlobalVariables.IntPriceHour[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[5].ToString());
                                        AppGlobalVariables.IntHourRound[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[6].ToString());
                                        AppGlobalVariables.IntExpense[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[7].ToString());
                                        AppGlobalVariables.IntOver[j] = Convert.ToInt32(dt2.Rows[j].ItemArray[8].ToString());
                                    }
                                    int intHour;
                                    int intMin;
                                    intHour = 0;
                                    intMin = Convert.ToInt32(dtLoad.Rows[i]["tdf"].ToString());
                                    //--------------------------------- //Mac 2017/12/06
                                    int ZoneMin = 0;
                                    int intTotal = 0;
                                    DataTable dt3 = DbController.LoadData("select * from prosetprice_zone where PromotionID = " + dtLoad.Rows[i]["proid"].ToString() + " order by no");
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

                                        var CalPriceZone = (dynamic)null;
                                        DateTime dti = DateTime.Parse(dtLoad.Rows[i]["datein"].ToString());
                                        DateTime dto = DateTime.Parse(dtLoad.Rows[i]["dateout"].ToString());
                                        DateTime dtInOne;
                                        DateTime dtOutOne;
                                        TimeSpan diffInOut = DateTime.Parse(dto.ToShortDateString()) - DateTime.Parse(dti.ToShortDateString());

                                        bool booNoRound = false; //Mac 2018/01/08
                                        booNoRound = false; //Mac 2018/01/08
                                        for (int x = 0; x < diffInOut.Days + 1; x++)
                                        {
                                            if (diffInOut.Days == 0)
                                            {
                                                booNoRound = true; //Mac 2018/01/08
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

                                            CalPriceZone = CalculationsManager.CalPriceZoneOneDay(0, dtInOne.ToString(), dtOutOne.ToString(), ZoneStart, ZoneStop, 0, 0, 0, booNoRound);
                                            ZoneMin += CalPriceZone.Key;
                                        }
                                    }
                                    if (ZoneMin > 0)
                                    {
                                        intMin -= ZoneMin;
                                        intTotal = CalculationsManager.CalPrice2(0, ZoneMin, notDay);
                                        dr["PriceList"] = (CalculationsManager.CalPrice(intHour, intMin, notDay) + intTotal).ToString();
                                    }
                                    else
                                        dr["PriceList"] = CalculationsManager.CalPrice(intHour, intMin, notDay).ToString();
                                    //--------------------------------- //Mac 2017/12/06
                                }


                                dr["PayPrice"] = dtLoad.Rows[i]["price"];
                                dr["Printno"] = dtLoad.Rows[i]["printno"];
                                dr["userin"] = dtLoad.Rows[i]["userin"];
                                dr["userout"] = dtLoad.Rows[i]["userout"];
                                dtMap.Rows.Add(dr);
                            }
                        }
                        PrimaryTabControl.SelectTab(1);
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        ReportDocument rpt = new ReportDocument();
                        rpt.Load(path + "\\CrystalReports\\Report18.rpt");
                        rpt.SetDataSource(dtMap);
                        string nTel = "";
                        string nFax = "";
                        string nTax = "";
                        try
                        {
                            string tel = AppGlobalVariables.Printings.Telephone;
                            int t = tel.IndexOf(" แฟ") - tel.IndexOf("L: ");
                            nTel = tel.Substring(tel.IndexOf("L: ") + 2, t - 2);
                            t = tel.IndexOf("AX:");
                            nFax = tel.Substring(t + 3);
                            tel = AppGlobalVariables.Printings.Tax1;
                            t = tel.IndexOf("D. ");
                            nTax = tel.Substring(t + 3);
                        }
                        catch
                        {
                            try
                            {
                                nTel = AppGlobalVariables.Printings.Telephone.Split(':')[1].Trim().Replace("fax", "").Replace("FAX", "").Replace("Fax", "");
                            }
                            catch
                            {
                                nTel = "";
                            }
                            try
                            {
                                nFax = AppGlobalVariables.Printings.Telephone.Split(':')[2].Trim();
                            }
                            catch
                            {
                                nFax = "";
                            }
                            try
                            {
                                nTax = AppGlobalVariables.Printings.Tax1.Split(':')[1].Trim();
                            }
                            catch
                            {
                                nTax = "";
                            }
                            if (nTax.Trim().Length == 0) //Mac 2022/08/31
                            {
                                try
                                {
                                    nTax = AppGlobalVariables.Printings.Tax1.Split(' ')[1].Trim();
                                }
                                catch
                                {
                                    nTax = "";
                                }
                            }
                        }
                        /////////////////////////////////////////////////////
                        rpt.SetParameterValue("compName", AppGlobalVariables.Printings.Company1.Trim());
                        rpt.SetParameterValue("ComAddress1", AppGlobalVariables.Printings.Address1.Trim() + "\r\n" + AppGlobalVariables.Printings.Address2.Trim());
                        rpt.SetParameterValue("ComTel", nTel);
                        rpt.SetParameterValue("comFax", nFax);
                        rpt.SetParameterValue("compTax", nTax);
                        rpt.SetParameterValue("compTelext", "");
                        rpt.SetParameterValue("DateSearch", startDateTime);
                        rpt.SetParameterValue("DateSearch2", endDateTime);

                        PrimaryCrystalReportViewer.ReportSource = rpt;
                        PrimaryCrystalReportViewer.Refresh();



                    }
                    catch (Exception) { }
                    Cursor = Cursors.Default;
                    return;

                case 18:
                    try
                    {
                        PrimaryTabControl.SelectTab(1);
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        ReportDocument rpt = new ReportDocument();
                        rpt.Load(path + "\\CrystalReports\\Report19.rpt");
                        string nTel = "";
                        string nFax = "";
                        string nTax = "";
                        try
                        {
                            string tel = AppGlobalVariables.Printings.Telephone;
                            int t = tel.IndexOf(" แฟ") - tel.IndexOf("L: ");
                            nTel = tel.Substring(tel.IndexOf("L: ") + 2, t - 2);
                            t = tel.IndexOf("AX:");
                            nFax = tel.Substring(t + 3);
                            tel = AppGlobalVariables.Printings.Tax1;
                            t = tel.IndexOf("D. ");
                            nTax = tel.Substring(t + 3);
                        }
                        catch
                        {
                            try
                            {
                                nTel = AppGlobalVariables.Printings.Telephone.Split(':')[1].Trim().Replace("fax", "").Replace("FAX", "").Replace("Fax", "");
                            }
                            catch
                            {
                                nTel = "";
                            }
                            try
                            {
                                nFax = AppGlobalVariables.Printings.Telephone.Split(':')[2].Trim();
                            }
                            catch
                            {
                                nFax = "";
                            }
                            try
                            {
                                nTax = AppGlobalVariables.Printings.Tax1.Split(':')[1].Trim();
                            }
                            catch
                            {
                                nTax = "";
                            }
                            if (nTax.Trim().Length == 0) //Mac 2022/08/31
                            {
                                try
                                {
                                    nTax = AppGlobalVariables.Printings.Tax1.Split(' ')[1].Trim();
                                }
                                catch
                                {
                                    nTax = "";
                                }
                            }
                        }
                        rpt.SetParameterValue("compName", AppGlobalVariables.Printings.Company1.Trim());
                        rpt.SetParameterValue("ComAddress1", AppGlobalVariables.Printings.Address1.Trim() + "\r\n" + AppGlobalVariables.Printings.Address2.Trim());
                        rpt.SetParameterValue("ComTel", nTel);
                        rpt.SetParameterValue("comFax", nFax);
                        rpt.SetParameterValue("compTax", nTax);
                        rpt.SetParameterValue("compTelext", "");
                        rpt.SetParameterValue("DateSearch", startDateTime);
                        rpt.SetParameterValue("DateSearch2", endDateTime);
                        /////////////////////////////////////////////////////

                        sql = "SELECT  COUNT(recordin.id) "
                        + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
                        + " WHERE recordin.cartype != 200 AND recordout.losscard = 0 AND"
                        + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "' ";

                        DataTable dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("visiterin", dt.Rows[0].ItemArray[0].ToString());
                        rpt.SetParameterValue("visiterout", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT  COUNT(recordin.id)  "
                        + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
                        + " WHERE recordin.cartype = 200 AND  (SELECT level FROM cardmf WHERE name = recordin.id) = 2 AND"
                        + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("vipin", dt.Rows[0].ItemArray[0].ToString());
                        rpt.SetParameterValue("vipout", dt.Rows[0].ItemArray[0].ToString());

                        sql = "SELECT  COUNT(recordin.id)  "
                        + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
                        + " WHERE recordin.cartype = 200 AND  (SELECT level FROM cardmf WHERE name = recordin.id) = 3 AND"
                        + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
                        dt = DbController.LoadData(sql);
                        rpt.SetParameterValue("memberin", dt.Rows[0].ItemArray[0].ToString());
                        rpt.SetParameterValue("memberout", dt.Rows[0].ItemArray[0].ToString());


                        PrimaryCrystalReportViewer.ReportSource = rpt;
                        PrimaryCrystalReportViewer.Refresh();

                    }
                    catch (Exception) { }
                    Cursor = Cursors.Default;
                    return;

                case 19:
                    sql = "select id,name";

                    if (Configs.UseActivePromotion)
                        sql += ",active";
                    sql += " from promotion"; //Mac 2016/01/05
                    if (MemberGroupMonthComboBox.SelectedIndex > 0) //Mac 2016/04/02
                        sql += " where groupro = " + AppGlobalVariables.MemberGroupMonthsToId[MemberGroupMonthComboBox.Text];
                    else if (PromotionComboBox.Text != "ALL") //Mac 2017/12/21
                        sql += " where id = " + AppGlobalVariables.PromotionNamesById.First(kvp => kvp.Value == PromotionComboBox.Text).Key;

                    PrimaryTabControl.SelectTab(1);
                    try
                    {

                        DataTable dt = DbController.LoadData(sql);
                        DataTable dt2;
                        DataTable estampSumMap = new DataTable("myMember");  //*** DataTable Map DataSet.xsd ***//
                        DataRow dr = null;
                        estampSumMap.Columns.Add(new DataColumn("proid", typeof(string)));
                        estampSumMap.Columns.Add(new DataColumn("proName", typeof(string)));
                        if (Configs.UseActivePromotion)
                            estampSumMap.Columns.Add(new DataColumn("active", typeof(string))); //Mac 2016/01/05
                        estampSumMap.Columns.Add(new DataColumn("Data0", typeof(string)));
                        estampSumMap.Columns.Add(new DataColumn("Data1", typeof(string)));
                        estampSumMap.Columns.Add(new DataColumn("SumData0", typeof(string)));
                        estampSumMap.Columns.Add(new DataColumn("SumData1", typeof(string)));
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dr = estampSumMap.NewRow();
                                dr["proid"] = dt.Rows[i]["id"];
                                dr["proName"] = dt.Rows[i]["name"];
                                if (Configs.UseActivePromotion)
                                    dr["active"] = dt.Rows[i]["active"]; //Mac 2016/01/05
                                dr["Data0"] = "0";
                                ////////////////////////////////////////////////////////////////////////////
                                sql = "select count(no) from recordout where dateout "
                                + " BETWEEN '" + startDateTime + "' AND '" + endDateTime + "' AND proid =" + dt.Rows[i]["id"];
                                dt2 = DbController.LoadData(sql);
                                dr["Data1"] = dt2.Rows[0].ItemArray[0].ToString();
                                ////////////////////////////////////////////////////////////////////////////
                                sql = "select * from prosetprice where PromotionId = " + dt.Rows[i]["id"];
                                dt2 = DbController.LoadData(sql);
                                if (dt2 != null && dt2.Rows.Count > 0)
                                {
                                    int totalCreditDayWeek = 0; //Mac 2019/05/28
                                    sql = "SELECT value FROM param WHERE name = 'not_day'";
                                    DataTable dt3 = DbController.LoadData(sql);
                                    Boolean notDay = Convert.ToBoolean(dt3.Rows[0].ItemArray[0].ToString());
                                    int SumData0 = 0;
                                    sql = "select recordout.price, "
                                    /*+ " truncate(time_to_sec(timediff(recordout.dateout,recordin.datein))/60,0) "*/
                                    + " truncate(TIMESTAMPDIFF(minute,recordin.datein,recordout.dateout),0) " //Mac 2017/02/07
                                    + " , recordin.datein, recordout.dateout" //Mac 2017/12/06
                                    + " , recordin.cartype " //Mac 2019/05/28
                                    + " from recordout join recordin on recordout.no = recordin.no where  dateout  BETWEEN '"
                                    + startDateTime + "' AND '" + endDateTime + "' AND recordout.proid = " + dt.Rows[i]["id"];
                                    dt3 = DbController.LoadData(sql);
                                    if (dt3 != null && dt3.Rows.Count > 0)
                                    {
                                        for (int k = 0; k < dt3.Rows.Count; k++)
                                        {
                                            //Mac 2019/05/28
                                            AppGlobalVariables.IntTime = new int[0];
                                            AppGlobalVariables.IntPriceMin = new int[0];
                                            AppGlobalVariables.IntPriceHour = new int[0];
                                            AppGlobalVariables.IntHourRound = new int[0];
                                            AppGlobalVariables.IntExpense = new int[0];
                                            AppGlobalVariables.IntOver = new int[0];
                                            AppGlobalVariables.IntTime2 = new int[0];
                                            AppGlobalVariables.IntPriceMin2 = new int[0];
                                            AppGlobalVariables.IntPriceHour2 = new int[0];
                                            AppGlobalVariables.IntHourRound2 = new int[0];
                                            AppGlobalVariables.IntExpense2 = new int[0];
                                            AppGlobalVariables.IntOver2 = new int[0];

                                            if (Configs.Reports.ReportProsetPriceDayWeek) //Mac 2019/05/27
                                            {
                                                SumData0 = 0;
                                                var CalPriceZone = (dynamic)null;
                                                DateTime dti = DateTime.Parse(dt3.Rows[k]["datein"].ToString());
                                                DateTime dto = DateTime.Parse(dt3.Rows[k]["dateout"].ToString());
                                                DateTime dtInOne;
                                                DateTime dtOutOne;
                                                TimeSpan diffInOut = DateTime.Parse(dto.ToShortDateString()) - DateTime.Parse(dti.ToShortDateString());
                                                int intFM = 0;
                                                int intLM = 0;

                                                bool booNoRound = false;
                                                booNoRound = false;
                                                for (int x = 0; x < diffInOut.Days + 1; x++)
                                                {
                                                    int intMin = 0;
                                                    int ZoneMin = 0;
                                                    TimeSpan diffIO;
                                                    string ZoneStart = "";
                                                    string ZoneStop = "";

                                                    if (diffInOut.Days == 0)
                                                    {
                                                        booNoRound = true;
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

                                                    diffIO = dtOutOne - dtInOne;
                                                    if (diffIO.Days > 0)
                                                        intMin += diffIO.Days * 24 * 60;
                                                    intMin += diffIO.Hours * 60;
                                                    intMin += diffIO.Minutes;

                                                    string stringDW = dtInOne.DayOfWeek.ToString().ToLower().Substring(0, 2);

                                                    if ((diffInOut.Days == 0) || (x == 0))
                                                    {
                                                        DataTable dtFM = DbController.LoadData("select freemin, pricelimit from cartype_freemin_prosetprice_dayweek where typeid = " + dt3.Rows[k]["cartype"] + " and dayweek like '%" + stringDW + "%'");
                                                        if (dtFM != null && dtFM.Rows.Count > 0)
                                                        {
                                                            intFM = Convert.ToInt32(dtFM.Rows[0][0]);
                                                            intLM = Convert.ToInt32(dtFM.Rows[0][1]);
                                                        }
                                                    }

                                                    AppGlobalVariables.IntTime = new int[0];
                                                    AppGlobalVariables.IntPriceMin = new int[0];
                                                    AppGlobalVariables.IntPriceHour = new int[0];
                                                    AppGlobalVariables.IntHourRound = new int[0];
                                                    AppGlobalVariables.IntExpense = new int[0];
                                                    AppGlobalVariables.IntOver = new int[0];

                                                    DataTable dt4 = DbController.LoadData("select * from prosetprice where PromotionID = " + dt.Rows[i]["id"] + " and dayweek like '%" + stringDW + "%' order by no");
                                                    if (dt4 != null && dt4.Rows.Count > 0)
                                                    {
                                                        AppGlobalVariables.IntTime = new int[dt4.Rows.Count];
                                                        AppGlobalVariables.IntPriceMin = new int[dt4.Rows.Count];
                                                        AppGlobalVariables.IntPriceHour = new int[dt4.Rows.Count];
                                                        AppGlobalVariables.IntHourRound = new int[dt4.Rows.Count];
                                                        AppGlobalVariables.IntExpense = new int[dt4.Rows.Count];
                                                        AppGlobalVariables.IntOver = new int[dt4.Rows.Count];

                                                        for (int j = 0; j < dt4.Rows.Count; j++)
                                                        {
                                                            if (j == 0)
                                                            {
                                                                AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[3].ToString());
                                                            }
                                                            else
                                                            {
                                                                AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[3].ToString()) - Convert.ToInt32(dt4.Rows[j - 1].ItemArray[3].ToString());
                                                            }
                                                            AppGlobalVariables.IntPriceMin[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[4].ToString());
                                                            AppGlobalVariables.IntPriceHour[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[5].ToString());
                                                            AppGlobalVariables.IntHourRound[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[6].ToString());
                                                            AppGlobalVariables.IntExpense[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[7].ToString());
                                                            AppGlobalVariables.IntOver[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[8].ToString());
                                                        }
                                                    }

                                                    AppGlobalVariables.IntTime2 = new int[0];
                                                    AppGlobalVariables.IntPriceMin2 = new int[0];
                                                    AppGlobalVariables.IntPriceHour2 = new int[0];
                                                    AppGlobalVariables.IntHourRound2 = new int[0];
                                                    AppGlobalVariables.IntExpense2 = new int[0];
                                                    AppGlobalVariables.IntOver2 = new int[0];
                                                    DataTable dt5 = DbController.LoadData("select * from prosetprice_zone where PromotionID = " + dt.Rows[i]["id"] + " and dayweek like '%" + stringDW + "%' order by no");
                                                    if (dt5 != null && dt5.Rows.Count > 0)
                                                    {
                                                        AppGlobalVariables.IntTime2 = new int[dt5.Rows.Count];
                                                        AppGlobalVariables.IntPriceMin2 = new int[dt5.Rows.Count];
                                                        AppGlobalVariables.IntPriceHour2 = new int[dt5.Rows.Count];
                                                        AppGlobalVariables.IntHourRound2 = new int[dt5.Rows.Count];
                                                        AppGlobalVariables.IntExpense2 = new int[dt5.Rows.Count];
                                                        AppGlobalVariables.IntOver2 = new int[dt5.Rows.Count];
                                                        for (int y = 0; y < dt5.Rows.Count; y++)
                                                        {
                                                            if (y == 0)
                                                            {
                                                                AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[3].ToString());
                                                            }
                                                            else
                                                            {
                                                                AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[3].ToString()) - Convert.ToInt32(dt5.Rows[y - 1].ItemArray[3].ToString());
                                                            }
                                                            AppGlobalVariables.IntPriceMin2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[4].ToString());
                                                            AppGlobalVariables.IntPriceHour2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[5].ToString());
                                                            AppGlobalVariables.IntHourRound2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[6].ToString());
                                                            AppGlobalVariables.IntExpense2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[7].ToString());
                                                            AppGlobalVariables.IntOver2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[8].ToString());
                                                        }

                                                        ZoneStart = dt5.Rows[0]["zone_start"].ToString();
                                                        ZoneStop = dt5.Rows[0]["zone_stop"].ToString();
                                                    }

                                                    CalPriceZone = CalculationsManager.CalPriceZoneOneDay(0, dtInOne.ToString(), dtOutOne.ToString(), ZoneStart, ZoneStop, 0, 0, 0, booNoRound);
                                                    ZoneMin = CalPriceZone.Key;

                                                    if (ZoneMin > 0)
                                                    {
                                                        intMin -= ZoneMin;

                                                        ZoneMin -= intFM;
                                                        if (ZoneMin > 0)
                                                        {
                                                            intFM = 0;
                                                        }
                                                        else
                                                        {
                                                            ZoneMin = 0;
                                                            intFM -= CalPriceZone.Key;
                                                        }

                                                        SumData0 += CalculationsManager.CalPrice2(0, ZoneMin, notDay);
                                                    }

                                                    int tmpintMin = intMin;
                                                    intMin -= intFM;
                                                    if (intMin > 0)
                                                    {
                                                        intFM = 0;
                                                    }
                                                    else
                                                    {
                                                        intMin = 0;
                                                        intFM -= tmpintMin;
                                                    }
                                                    SumData0 += CalculationsManager.CalPrice(0, intMin, notDay);

                                                    if ((SumData0 > intLM) && (intLM > 0))
                                                        SumData0 = intLM;

                                                }

                                                totalCreditDayWeek += SumData0;
                                            }
                                            else
                                            {
                                                //Mac 2022/07/26----------------
                                                string sql19;
                                                string stringDW = "";
                                                if (Configs.UseDayWeek == "I")
                                                {
                                                    stringDW = DateTime.Parse(dt3.Rows[k]["datein"].ToString()).DayOfWeek.ToString().ToLower().Substring(0, 2);
                                                }
                                                else if (Configs.UseDayWeek == "O")
                                                {
                                                    stringDW = DateTime.Parse(dt3.Rows[k]["dateout"].ToString()).DayOfWeek.ToString().ToLower().Substring(0, 2);
                                                }

                                                if (Configs.UseHoliday) //Mac 2020/08/04
                                                {
                                                    string sqlHD = "SELECT * FROM holiday WHERE date(date) = '" + DateTime.Parse(dt3.Rows[k]["datein"].ToString()).Year + "-" + DateTime.Parse(dt3.Rows[k]["datein"].ToString()).Month + "-" + DateTime.Parse(dt3.Rows[k]["datein"].ToString()).Day + "'";
                                                    DataTable dtHD = DbController.LoadData(sqlHD);
                                                    if (dtHD.Rows.Count > 0)
                                                    {
                                                        stringDW = "hd";
                                                    }
                                                }
                                                //------------------------------

                                                int intMin = 0;
                                                intMin = Int32.Parse(dt3.Rows[k][1].ToString());
                                                //////////////////////////////////////////
                                                // Set before sent to function
                                                AppGlobalVariables.IntTime = new int[0];
                                                AppGlobalVariables.IntPriceMin = new int[0];
                                                AppGlobalVariables.IntPriceHour = new int[0];
                                                AppGlobalVariables.IntHourRound = new int[0];
                                                AppGlobalVariables.IntExpense = new int[0];
                                                AppGlobalVariables.IntOver = new int[0];
                                                //DataTable dt4 = DbController.LoadData("select * from prosetprice where PromotionID = " + dt.Rows[i]["id"] + " order by no");
                                                //Mac 2022/07/26 ---------------
                                                sql19 = "select * from prosetprice where PromotionID = " + dt.Rows[i]["id"] + " ";

                                                if (stringDW.Length > 1)
                                                    sql19 += " and dayweek like '%" + stringDW + "%'";

                                                sql19 += " order by no";

                                                DataTable dt4 = DbController.LoadData(sql19);
                                                //------------------------------

                                                if (dt4 != null && dt4.Rows.Count > 0)
                                                {
                                                    AppGlobalVariables.IntTime = new int[dt4.Rows.Count];
                                                    AppGlobalVariables.IntPriceMin = new int[dt4.Rows.Count];
                                                    AppGlobalVariables.IntPriceHour = new int[dt4.Rows.Count];
                                                    AppGlobalVariables.IntHourRound = new int[dt4.Rows.Count];
                                                    AppGlobalVariables.IntExpense = new int[dt4.Rows.Count];
                                                    AppGlobalVariables.IntOver = new int[dt4.Rows.Count];
                                                    for (int j = 0; j < dt4.Rows.Count; j++)
                                                    {
                                                        //AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[3].ToString());
                                                        if (j == 0) //Mac 2016/04/05
                                                        {
                                                            AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[3].ToString());
                                                        }
                                                        else
                                                        {
                                                            AppGlobalVariables.IntTime[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[3].ToString()) - Convert.ToInt32(dt4.Rows[j - 1].ItemArray[3].ToString());
                                                        }
                                                        AppGlobalVariables.IntPriceMin[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[4].ToString());
                                                        AppGlobalVariables.IntPriceHour[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[5].ToString());
                                                        AppGlobalVariables.IntHourRound[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[6].ToString());
                                                        AppGlobalVariables.IntExpense[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[7].ToString());
                                                        AppGlobalVariables.IntOver[j] = Convert.ToInt32(dt4.Rows[j].ItemArray[8].ToString());
                                                    }
                                                }

                                                //Mac 2022/07/29
                                                FlatRateM = 0;
                                                FlatRateP = 0;
                                                FlatRateX = 0;

                                                if (Configs.UseFlatRateProSetPrice)
                                                {
                                                    try
                                                    {
                                                        FlatRateM = Convert.ToInt32(dt4.Rows[0]["flat_rate"].ToString().Split('|')[0]);
                                                        FlatRateP = Convert.ToInt32(dt4.Rows[0]["flat_rate"].ToString().Split('|')[1]);
                                                        FlatRateX = Convert.ToInt32(dt4.Rows[0]["flat_rate"].ToString().Split('|')[2]);
                                                    }
                                                    catch
                                                    { }
                                                }

                                                //////////////////////////////////////////
                                                //--------------------------------- //Mac 2017/12/06
                                                int ZoneMin = 0;
                                                AppGlobalVariables.IntTime2 = new int[0];
                                                AppGlobalVariables.IntPriceMin2 = new int[0];
                                                AppGlobalVariables.IntPriceHour2 = new int[0];
                                                AppGlobalVariables.IntHourRound2 = new int[0];
                                                AppGlobalVariables.IntExpense2 = new int[0];
                                                AppGlobalVariables.IntOver2 = new int[0];
                                                //DataTable dt5 = DbController.LoadData("select * from prosetprice_zone where PromotionID = " + dt.Rows[i]["id"] + " order by no");
                                                //Mac 2022/07/26 ---------------
                                                sql19 = "select * from prosetprice_zone where PromotionID = " + dt.Rows[i]["id"] + " ";

                                                if (stringDW.Length > 1)
                                                    sql19 += " and dayweek like '%" + stringDW + "%'";

                                                sql19 += " order by no";

                                                DataTable dt5 = DbController.LoadData(sql19);
                                                //------------------------------
                                                if (dt5 != null && dt5.Rows.Count > 0)
                                                {
                                                    AppGlobalVariables.IntTime2 = new int[dt5.Rows.Count];
                                                    AppGlobalVariables.IntPriceMin2 = new int[dt5.Rows.Count];
                                                    AppGlobalVariables.IntPriceHour2 = new int[dt5.Rows.Count];
                                                    AppGlobalVariables.IntHourRound2 = new int[dt5.Rows.Count];
                                                    AppGlobalVariables.IntExpense2 = new int[dt5.Rows.Count];
                                                    AppGlobalVariables.IntOver2 = new int[dt5.Rows.Count];
                                                    for (int y = 0; y < dt5.Rows.Count; y++)
                                                    {
                                                        if (y == 0)
                                                        {
                                                            AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[3].ToString());
                                                        }
                                                        else
                                                        {
                                                            AppGlobalVariables.IntTime2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[3].ToString()) - Convert.ToInt32(dt5.Rows[y - 1].ItemArray[3].ToString());
                                                        }
                                                        AppGlobalVariables.IntPriceMin2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[4].ToString());
                                                        AppGlobalVariables.IntPriceHour2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[5].ToString());
                                                        AppGlobalVariables.IntHourRound2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[6].ToString());
                                                        AppGlobalVariables.IntExpense2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[7].ToString());
                                                        AppGlobalVariables.IntOver2[y] = Convert.ToInt32(dt5.Rows[y].ItemArray[8].ToString());
                                                    }
                                                    string ZoneStart = dt5.Rows[0]["zone_start"].ToString();
                                                    string ZoneStop = dt5.Rows[0]["zone_stop"].ToString();

                                                    var CalPriceZone = (dynamic)null;
                                                    DateTime dti = DateTime.Parse(dt3.Rows[k]["datein"].ToString());
                                                    DateTime dto = DateTime.Parse(dt3.Rows[k]["dateout"].ToString());
                                                    DateTime dtInOne;
                                                    DateTime dtOutOne;
                                                    TimeSpan diffInOut = DateTime.Parse(dto.ToShortDateString()) - DateTime.Parse(dti.ToShortDateString());

                                                    bool booNoRound = false; //Mac 2018/01/08
                                                    booNoRound = false; //Mac 2018/01/08
                                                    for (int x = 0; x < diffInOut.Days + 1; x++)
                                                    {
                                                        if (diffInOut.Days == 0)
                                                        {
                                                            booNoRound = true; //Mac 2018/01/08
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

                                                        CalPriceZone = CalculationsManager.CalPriceZoneOneDay(0, dtInOne.ToString(), dtOutOne.ToString(), ZoneStart, ZoneStop, 0, 0, 0, booNoRound);
                                                        ZoneMin += CalPriceZone.Key;
                                                    }
                                                }
                                                if (ZoneMin > 0)
                                                {
                                                    intMin -= ZoneMin;
                                                    SumData0 += CalculationsManager.CalPrice2(0, ZoneMin, notDay);
                                                }
                                                //--------------------------------- //Mac 2017/12/06
                                                SumData0 += CalculationsManager.CalPrice(0, intMin, notDay);

                                                if (Configs.UseFlatRateProSetPrice) //Mac 2022/07/29
                                                {
                                                    SumData0 += CalculationsManager.CalFlatRate(DateTime.Parse(dt3.Rows[k]["datein"].ToString()), DateTime.Parse(dt3.Rows[k]["dateout"].ToString()), FlatRateM, FlatRateP, FlatRateX);
                                                }
                                            }
                                        }
                                    }
                                    //dr["SumData0"] = SumData0.ToString();

                                    if (Configs.Reports.ReportProsetPriceDayWeek) //Mac 2019/05/28
                                        dr["SumData0"] = totalCreditDayWeek.ToString();
                                    else
                                        dr["SumData0"] = SumData0.ToString();

                                }
                                else dr["SumData0"] = "0";
                                /////////////////////////////////////////////////////////////////////////////
                                sql = "select SUM(recordout.price) from recordout join recordin on recordout.no = recordin.no "
                                    + " where  dateout  BETWEEN '" + startDateTime + "' AND '" + endDateTime + "' AND recordout.proid = " + dt.Rows[i]["id"];
                                DataTable tmp = DbController.LoadData(sql);

                                if (tmp.Rows[0].ItemArray[0].ToString() != null && tmp.Rows[0].ItemArray[0].ToString().Trim() != "")
                                {
                                    dr["SumData1"] = tmp.Rows[0].ItemArray[0].ToString();
                                }
                                else
                                {
                                    dr["SumData1"] = "0";
                                }
                                estampSumMap.Rows.Add(dr);

                            }
                        }

                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        ReportDocument rpt = new ReportDocument();
                        if (Configs.UseActivePromotion)
                            rpt.Load(path + "\\CrystalReports\\Report20_active.rpt");
                        else
                            rpt.Load(path + "\\CrystalReports\\Report20.rpt");
                        rpt.SetDataSource(estampSumMap);
                        string nTel = "";
                        string nFax = "";
                        string nTax = "";
                        try
                        {
                            string tel = AppGlobalVariables.Printings.Telephone;
                            int t = tel.IndexOf(" แฟ") - tel.IndexOf("L: ");
                            nTel = tel.Substring(tel.IndexOf("L: ") + 2, t - 2);
                            t = tel.IndexOf("AX:");
                            nFax = tel.Substring(t + 3);
                            tel = AppGlobalVariables.Printings.Tax1;
                            t = tel.IndexOf("D. ");
                            nTax = tel.Substring(t + 3);
                        }
                        catch
                        {
                            try
                            {
                                nTel = AppGlobalVariables.Printings.Telephone.Split(':')[1].Trim().Replace("fax", "").Replace("FAX", "").Replace("Fax", "");
                            }
                            catch
                            {
                                nTel = "";
                            }
                            try
                            {
                                nFax = AppGlobalVariables.Printings.Telephone.Split(':')[2].Trim();
                            }
                            catch
                            {
                                nFax = "";
                            }
                            try
                            {
                                nTax = AppGlobalVariables.Printings.Tax1.Split(':')[1].Trim();
                            }
                            catch
                            {
                                nTax = "";
                            }
                            if (nTax.Trim().Length == 0) //Mac 2022/08/31
                            {
                                try
                                {
                                    nTax = AppGlobalVariables.Printings.Tax1.Split(' ')[1].Trim();
                                }
                                catch
                                {
                                    nTax = "";
                                }
                            }
                        }
                        if (MemberGroupMonthComboBox.SelectedIndex > 0) //Mac 2016/04/02
                        {
                            CrystalDecisions.CrystalReports.Engine.TextObject txtReportHeader;
                            txtReportHeader = rpt.ReportDefinition.ReportObjects["text7"] as TextObject;
                            txtReportHeader.Text = MemberGroupMonthComboBox.Text;
                        }
                        rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'"; //Mac 2020/08/27
                        rpt.SetParameterValue("compName", AppGlobalVariables.Printings.Company1.Trim());
                        rpt.SetParameterValue("ComAddress1", AppGlobalVariables.Printings.Address1.Trim() + "\r\n" + AppGlobalVariables.Printings.Address2.Trim());
                        rpt.SetParameterValue("ComTel", nTel);
                        rpt.SetParameterValue("compTax", nTax);
                        rpt.SetParameterValue("DateSearch", startDateTime);
                        rpt.SetParameterValue("DateSearch2", endDateTime);

                        PrimaryCrystalReportViewer.ReportSource = rpt;
                        PrimaryCrystalReportViewer.Refresh();

                    }
                    catch (Exception) { }

                    Cursor = Cursors.Default;
                    return;

                case 20:
                    {
                        DataTable itemizedMap = CRUDManager.GetItemizedPromotionUsage(sql, PaymentStatusComboBox.Text);

                        try
                        {
                            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                            path = path.Replace("\\bin\\Debug", "");
                            ReportDocument rpt = new ReportDocument();

                            if (Configs.Reports.UseReport21_3)
                            {
                                rpt.Load(path + "\\CrystalReports\\Report21_3.rpt");
                            }
                            else if (Configs.Reports.UseReport21_2)
                            {
                                rpt.Load(path + "\\CrystalReports\\Report21_2.rpt");
                            }
                            else if (Configs.Reports.UseReport21_1)
                            {
                                if (Configs.Reports.Report21_1_Switch)
                                {
                                    if (Configs.IsSwitch)
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report21_1_sw.rpt");
                                        Configs.IsSwitch = false;
                                    }
                                    else
                                    {
                                        rpt.Load(path + "\\CrystalReports\\Report21_1.rpt");
                                        Configs.IsSwitch = true;
                                    }
                                }
                                else
                                    rpt.Load(path + "\\CrystalReports\\Report21_1.rpt");
                            }

                            else
                                rpt.Load(path + "\\CrystalReports\\Report21.rpt");

                            rpt.SetDataSource(itemizedMap);

                            string nTel = "";
                            string nFax = "";
                            string nTax = "";
                            try
                            {
                                string tel = AppGlobalVariables.ParamsLookup["tel"];
                                int t = tel.IndexOf(" แฟ") - tel.IndexOf("L: ");
                                nTel = tel.Substring(tel.IndexOf("L: ") + 2, t - 2);
                                t = tel.IndexOf("AX:");
                                nFax = tel.Substring(t + 3);
                                tel = AppGlobalVariables.ParamsLookup["tax"];
                                t = tel.IndexOf("D. ");
                                nTax = tel.Substring(t + 3);
                            }
                            catch
                            {
                                try
                                {
                                    nTel = AppGlobalVariables.ParamsLookup["tel"].Split(':')[1].Trim().Replace("fax", "").Replace("FAX", "").Replace("Fax", "");
                                }
                                catch
                                {
                                    nTel = "";
                                }
                                try
                                {
                                    nFax = AppGlobalVariables.ParamsLookup["tel"].Split(':')[2].Trim();
                                }
                                catch
                                {
                                    nFax = "";
                                }
                                try
                                {
                                    nTax = AppGlobalVariables.ParamsLookup["tax"].Split(':')[1].Trim();
                                }
                                catch
                                {
                                    nTax = "";
                                }
                                if (nTax.Trim().Length == 0) //Mac 2022/08/31
                                {
                                    try
                                    {
                                        nTax = AppGlobalVariables.ParamsLookup["tax"].Split(' ')[1].Trim();
                                    }
                                    catch
                                    {
                                        nTax = "";
                                    }
                                }
                            }
                            if (MemberGroupMonthComboBox.SelectedIndex > 0) //Mac 2016/04/18
                            {
                                TextObject txtReportHeader = rpt.ReportDefinition.ReportObjects["text7"] as TextObject;
                                txtReportHeader.Text = "บัตรจอดรถ";
                            }
                            rpt.DataDefinition.FormulaFields["ReportName"].Text = "'" + ReportComboBox.Text + "'"; //Mac 2020/08/27
                            rpt.SetParameterValue("DateSearch", StartDatePicker.Text + " " + StartTimePicker.Text);
                            rpt.SetParameterValue("DateSearch2", EndDatePicker.Text + " " + EndTimePicker.Text);

                            PrimaryTabControl.SelectTab(1);
                            PrimaryCrystalReportViewer.ReportSource = rpt;
                            PrimaryCrystalReportViewer.Refresh();
                        }
                        catch (Exception) { }
                        Cursor = Cursors.Default;
                        return;
                    }

                case 21:
                    DataTable itemizedTable = CRUDManager.GetItemizedPromotionUsage(sql, Constants.TextBased.All);
                    DataTable mappedTable = CRUDManager.GetItemizedDailyPromotionUsage(
                        itemizedTable,
                        PromotionComboBox.Text,
                        PaymentStatusComboBox.Text,
                        StartDatePicker.Value,
                        EndDatePicker.Value);

                    try
                    {
                        PrimaryTabControl.SelectTab(1);
                        //////////////////////////////////////////////////////
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        ReportDocument rpt = new ReportDocument();
                        rpt.Load(path + "\\CrystalReports\\Report22.rpt");
                        //////////////////////////////////////////////////////
                        rpt.SetDataSource(mappedTable);

                        string start_date = StartDatePicker.Value.ToString("yyyy-MM-dd");
                        string end_date = EndDatePicker.Value.ToString("yyyy-MM-dd");

                        rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสรุปการใช้ตราประทับ'";
                        rpt.DataDefinition.FormulaFields["ReportMonth"].Text = $"'ประจำเดือน {TextFormatters.ExtractThaiMonthFromDate(end_date)}'";
                        rpt.DataDefinition.FormulaFields["ReportSearchDate"].Text = $"'{start_date} ถึง {end_date}'";
                        rpt.DataDefinition.FormulaFields["VehicleType"].Text = $"'{AppGlobalVariables.Database.VehicleTypeTh}'";
                        rpt.DataDefinition.FormulaFields["Address"].Text = "'อาคารธนภูมิ'";
                        rpt.DataDefinition.FormulaFields["PrintedByUser"].Text = $"'Printed By: {AppGlobalVariables.OperatingUser.Name}'";
                        //rpt.DataDefinition.FormulaFields["PromotionName"].Text = $"'{PromotionComboBox?.Text}'";
                        PrimaryCrystalReportViewer.ReportSource = rpt;
                        PrimaryCrystalReportViewer.Refresh();
                    }
                    catch (Exception) { }
                    Cursor = Cursors.Default;
                    return;

                case 163:
                    try
                    {
                        string start_date = StartDatePicker.Value.ToString("yyyy-MM-dd");
                        string end_date = EndDatePicker.Value.ToString("yyyy-MM-dd");

                        DataTable dataTable = CRUDManager.GetVehicleEarningSummary(sql, StartDatePicker.Value, EndDatePicker.Value);
                        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = path.Replace("\\bin\\Debug", "");
                        ReportDocument rpt = new ReportDocument();
                        rpt.Load(path + "\\CrystalReports\\TnptVehicleEarningSummary.rpt");
                        rpt.SetDataSource(dataTable);

                        rpt.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสรุปจำนวนรถและรายได้'";
                        rpt.DataDefinition.FormulaFields["CompanyName"].Text = $"'{AppGlobalVariables.Printings.Company2}'";
                        rpt.DataDefinition.FormulaFields["PrintedPersonnel"].Text = $"'Printed By: {AppGlobalVariables.OperatingUser.Name}'";
                        rpt.DataDefinition.FormulaFields["ReportMonth"].Text = $"'ประจำเดือน {TextFormatters.ExtractThaiMonthFromDate(EndDatePicker.Value.ToString("yyyy-MM-dd"))}'";
                        rpt.DataDefinition.FormulaFields["ReportSearchDate"].Text = $"'{start_date} ถึง {end_date}'";
                        PrimaryCrystalReportViewer.ReportSource = rpt;
                        PrimaryCrystalReportViewer.Refresh();

                        PrimaryTabControl.SelectTab(1);
                    }
                    catch { }
                    break;
            }
        }
    }
}