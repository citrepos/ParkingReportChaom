using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using ParkingManagementReport.Common;
using ParkingManagementReport.Utilities;
using ParkingManagementReport.Utilities.Database;
using ParkingManagementReport.Utilities.Formatters;
using ParkingManagementReport.Utilities.Hardwares;
using Excel = Microsoft.Office.Interop.Excel;
using static ParkingManagementReport.Common.Constants;

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
        int FlatRateM = 0;
        int FlatRateP = 0;
        int FlatRateX = 0;
        //int totalReceived, totalDiscount, totalAmount, totalLoss, totalOver, totalPrice;
        //double totalBeforeVat, totalVat;
        int dgvX, dgvY, dgvH;
        #endregion

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            ConfigsManager.LoadConfigsFromXml();

            FormLogin frmLogin = new FormLogin();
            frmLogin.ShowDialog();

            if (AppGlobalVariables.OperatingUser.Level > 2)
                optionBox.Visible = true;

            ConfigsManager.LoadConfigsFromDb();

            InitializeHardwares();

            InitializeUIElements();

            /*FOR TEST 
            StartDatePicker.Value = new DateTime(day: 01, month: 10, year: 2025);
            EndDatePicker.Value = new DateTime(day: 10, month: 10, year: 2025);
            */
        }

        #region INIT
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
            if (Configs.Reports.ReportSearchMemberGroup)
            {
                label17.Text = "กลุ่มสมาชิก";
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

        private void SetDefaultComboBoxValues(params ComboBox[] comboBoxes)
        {
            foreach (var comboBox in comboBoxes)
            {
                comboBox.Text = Constants.TextBased.All;
                if (!comboBox.Items.Contains(Constants.TextBased.All))
                {
                    comboBox.Items.Add(Constants.TextBased.All);
                }
            }
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

            ConfigsManager.LoadDataToIntStringDictionary(
                "SELECT id, vendor_name FROM vendor_group ORDER BY id",
                AppGlobalVariables.VendorGroupMonthsById);
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

            AddToDictionaryIfNotExists(AppGlobalVariables.MemberGroupsToId, Constants.TextBased.All, 0);
            AddToComboBoxIfNotExists(MemberTypeComboBox, Constants.TextBased.All);

            if (Configs.Reports.ReportSearchMemberGroup)
            {
                label17.Text = "กลุ่มสมาชิก";
                MemberTypeComboBox.Visible = label17.Visible = true;

                DataTable dt = DbController.LoadData("SELECT groupname, id FROM membergroup ORDER BY id");
                AddMemberGroups(dt);
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
                    AddCarTypes(dt);
                }
                else
                {
                    CarTypeComboBox.Text = Constants.TextBased.All;
                    dt = DbController.LoadData("SELECT t1.typename, t1.typeid FROM cartype t1 LEFT JOIN member t2 ON t1.typeid = t2.typeid WHERE t2.typeid IS NULL AND t1.typeid != 200 ORDER BY t1.typeid");
                    AddCarTypes(dt);
                }

                if (Configs.Member2Cartype)
                {
                    AddToDictionaryIfNotExists(AppGlobalVariables.MemberGroupsToId, Constants.TextBased.Member, 200);

                    AddToComboBoxIfNotExists(MemberTypeComboBox, Constants.TextBased.All);
                    AddToComboBoxIfNotExists(MemberTypeComboBox, Constants.TextBased.Member);

                    dt = DbController.LoadData("SELECT groupname, id FROM membergroup ORDER BY id");
                    AddMemberGroups(dt);
                }
                else
                {
                    MemberTypeComboBox.Text = Constants.TextBased.All;
                    dt = DbController.LoadData("SELECT DISTINCT member.typeid, cartype.typename FROM member JOIN cartype ON member.typeid = cartype.typeid ORDER BY member.typeid");
                    //dt = DbController.LoadData("SELECT DISTINCT typeid FROM member WHERE typeid != 200 ORDER BY typeid");


                    foreach (DataRow row in dt.Rows)
                    {
                        string name = row["typename"].ToString();
                        int id = Convert.ToInt32(row["typeid"]);

                        AddToDictionaryIfNotExists(AppGlobalVariables.MemberGroupsToId, name, id);
                        AddToComboBoxIfNotExists(MemberTypeComboBox, name);
                    }
                }
            }
        }

        private void LoadCarTypes()
        {
            try
            {
                AddToDictionaryIfNotExists(AppGlobalVariables.CarTypesById, -1, Constants.TextBased.All);
                AddToComboBoxIfNotExists(CarTypeComboBox, Constants.TextBased.All);

                //AppGlobalVariables.CarTypesById.Add(0, Constants.TextBased.All);
                //AppGlobalVariables.CarTypesById.Add(199, Constants.TextBased.Visitor);
                //AppGlobalVariables.CarTypesById.Add(200, Constants.TextBased.Member);

                //CarTypeComboBox.Items.Add(Constants.TextBased.All);
                //CarTypeComboBox.Items.Add(Constants.TextBased.Visitor);
                //CarTypeComboBox.Items.Add(Constants.TextBased.Member);

                DataTable carTypes = DbController.LoadData("SELECT typeid, typename FROM cartype ORDER BY typeid");
                if (carTypes?.Rows.Count > 0)
                {
                    for (int i = 0; i < carTypes.Rows.Count; i++)
                    {
                        int carTypeId = Convert.ToInt16(carTypes.Rows[i].ItemArray[0]);
                        string carTypeName = carTypes.Rows[i].ItemArray[1].ToString();

                        AddToDictionaryIfNotExists(AppGlobalVariables.CarTypesById, carTypeId, carTypeName);
                        AddToComboBoxIfNotExists(CarTypeComboBox, carTypeName);
                    }
                }

                if (!AppGlobalVariables.CarTypesById.ContainsKey(199))
                {
                    AddToDictionaryIfNotExists(AppGlobalVariables.CarTypesById, 199, Constants.TextBased.Visitor);
                    AddToComboBoxIfNotExists(CarTypeComboBox, Constants.TextBased.Visitor);
                }
                if (!AppGlobalVariables.CarTypesById.ContainsKey(200))
                {
                    AddToDictionaryIfNotExists(AppGlobalVariables.CarTypesById, 200, Constants.TextBased.Member);
                    AddToComboBoxIfNotExists(CarTypeComboBox, Constants.TextBased.Member);
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

                        AddToDictionaryIfNotExists(AppGlobalVariables.UsersById, userId, userName);
                        AddToComboBoxIfNotExists(UserComboBox, userName);
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
        #endregion


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

            DataTable dataFromQuery = DbController.LoadData(sql);
            DataTable dtMap = new DataTable();
            ReportDocument reportDocument = new ReportDocument();

            ResultGridView.Location = new Point(dgvX, dgvY);
            ResultGridView.Height = dgvH;
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            groupBox3.Visible = false;
            string reportName = ReportComboBox.Text;
            string startTime = StartTimePicker.Value.ToLongTimeString();
            string endTime = EndTimePicker.Value.ToLongTimeString();
            if (dataFromQuery.Rows.Count > 0)
            {
                PrimaryCrystalReportViewer.ReportSource = null;
                PrimaryCrystalReportViewer.Refresh();

                switch (selectedReportId)
                {
                    //การเข้าออก
                    case 1:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report1_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report1.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    //การเข้าออก แสดงรูปภาพ
                    case 2:
                        DataTable dataTable2 = DataTableManager.การเข้าออกแสดงรูปภาพ(dataFromQuery);

                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report2_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report2.rpt");

                        TrySetReportData(reportDocument, dataTable2);

                        SetDisplayImageUI();
                        break;

                    //การทำงานของเจ้าหน้าที่
                    case 3:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report3_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report3.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    //การยกไม้
                    case 4:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report4_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report4.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    //รถคงค้าง
                    case 5:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report5_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report5.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    //บัตรหาย
                    case 6:
                        if (Configs.Reports.ReportPriceSplitLosscard)
                        {
                            if (Configs.Reports.ReportNoRunning)
                                reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report6_NoRunning.rpt");
                            else
                                reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report6.rpt");
                        }
                        else //ถ้าไม่แยกเงินค่าปรับ กับค่าบริการจอดรถ → ไปใช้ Report1
                        {
                            if (Configs.Reports.ReportNoRunning)
                                reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report1_NoRunning.rpt");
                            else
                                reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report1.rpt");
                        }

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    //สถิติการเข้าออก
                    case 7:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report7.rpt");

                        DataTable dataTable7 = DataTableManager.สถิติการเข้าออก(dataFromQuery, ResultGridView);

                        TrySetReportData(reportDocument, dataTable7);
                        break;

                    //การยกไม้ แสดงรูปภาพ
                    case 8:
                        DataTable dataTable8 = DataTableManager.การยกไม้แสดงรูปภาพ(dataFromQuery);

                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report8.rpt");

                        TrySetReportData(reportDocument, dataTable8);

                        SetDisplayImageUI();
                        break;

                    //โปรโมชั่น
                    case 9:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report9_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report9.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    //รายได้แยกภาษี
                    case 10:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report10.rpt");

                        DataTable dataTable10 = DataTableManager.ConvertedTableType(dataFromQuery);
                        DataTableManager.CaseReportTax(ResultGridView);

                        #region cal sum 10
                        try
                        {
                            string p0, p1, p2, p3, p4, p5;
                            p0 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[4].Value.ToString();
                            p1 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[7].Value.ToString();
                            p2 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[8].Value.ToString();
                            p3 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[9].Value.ToString();
                            p4 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[10].Value.ToString();
                            p5 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[11].Value.ToString();

                            if (dtMap.Rows.Count > 0)
                            {
                                reportDocument.DataDefinition.FormulaFields["Pa0"].Text = "'" + p0 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa1"].Text = "'" + p1 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa2"].Text = "'" + p2 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa3"].Text = "'" + p3 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa4"].Text = "'" + p4 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa5"].Text = "'" + p5 + "'";
                            }
                        }
                        catch { }
                        #endregion

                        TrySetReportData(reportDocument, dataTable10);
                        break;

                    //รายได้ของ Member
                    case 11:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report11_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report11.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    //รายได้แบบแยกกลุ่ม
                    case 12:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report12.rpt");
                        ResultGridView.DataSource = DataTableManager.ConvertedTableType(dataFromQuery);
                        CaseReportGroupPrice();

                        #region cal sum 12
                        try
                        {
                            string p0, p1, p2, p3, p4, p5, p6;
                            p0 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[4].Value.ToString();
                            p1 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[6].Value.ToString();
                            p2 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[7].Value.ToString();
                            p3 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[8].Value.ToString();
                            p4 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[9].Value.ToString();
                            p5 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[10].Value.ToString();
                            p6 = ResultGridView.Rows[ResultGridView.Rows.Count - 1].Cells[11].Value.ToString();

                            if (dtMap.Rows.Count > 0)
                            {
                                reportDocument.DataDefinition.FormulaFields["Pa0"].Text = "'" + p0 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa1"].Text = "'" + p1 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa2"].Text = "'" + p2 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa3"].Text = "'" + p3 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa4"].Text = "'" + p4 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa5"].Text = "'" + p5 + "'";
                                reportDocument.DataDefinition.FormulaFields["Pa6"].Text = "'" + p6 + "'";
                            }
                        }
                        catch { }
                        #endregion

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    //การเข้าออกแสดงโปรโมชั่น
                    case 13:
                    //รายได้ค่าจอดตามเลขที่ใบเสร็จ/ใบกำกับภาษี
                    case 14:
                    //การเข้าออกของรถแสดงโปรโมชั่นตามการทำงานของเจ้าหน้าที่
                    case 15:
                        Handle131415(reportDocument, dataFromQuery);
                        break;

                    //ยอดรวม E-Stamp
                    case 16:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report16.rpt");

                        ResultGridView.DataSource = DataTableManager.ConvertedTableType(dataFromQuery);

                        ResultGridView.Columns[0].HeaderText = ResultGridView.Columns[0].Name = "E-Stamp";
                        ResultGridView.Columns[1].HeaderText = ResultGridView.Columns[1].Name = "ยอดรวม";

                        int intNo = ResultGridView.Rows.Count - 1;
                        int sumPromotion = 0;
                        int sumNonPromotion = 0;

                        for (int i = 0; i < intNo; i++)
                        {
                            int intID = Convert.ToInt32(ResultGridView[0, i].Value);
                            try
                            {
                                if (intID > 0)
                                {
                                    ResultGridView[0, i].Value = AppGlobalVariables.PromotionNamesById[intID];
                                    sumPromotion += Convert.ToInt32(ResultGridView[1, i].Value);
                                }
                                else
                                {
                                    ResultGridView[0, i].Value = "(ไม่มีโปรโมชั่น-ส่วนลด)";
                                    sumNonPromotion = Convert.ToInt32(ResultGridView[1, i].Value);
                                }
                            }
                            catch (Exception)
                            {
                                ResultGridView[0, i].Value = "E-Stamp เลิกใช้";
                            }
                        }

                        dataFromQuery = DataTableManager.ConvertedDataGridView(ResultGridView);

                        reportDocument.DataDefinition.FormulaFields["SumPromotion"].Text = $"'{sumPromotion}'";
                        reportDocument.DataDefinition.FormulaFields["SumNonPromotion"].Text = $"'{sumNonPromotion}'";
                        reportDocument.DataDefinition.FormulaFields["SumAll"].Text = $"'{sumPromotion + sumNonPromotion}'";

                        TrySetReportData(reportDocument, dataFromQuery);

                        ResultGridView[0, intNo].Value = "รวม E-Stamp ทั้งหมด";
                        ResultGridView[1, intNo].Value = sumPromotion.ToString();
                        break;

                    case 17: //รายงานสรุปการเข้า-ออก
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report17.rpt");

                        รายงานสรุปการเข้าออก(reportDocument);
                        return;

                    case 18: //รายงานการเข้า-ออกประจำวัน
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report18.rpt");

                        รายงานการเข้าออกประจำวัน(reportDocument);
                        return;

                    case 19: //รายงานสถิติการเข้าออกที่จอดรถ
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report19.rpt");

                        รายงานสถิติการเข้าออกที่จอดรถ(reportDocument);
                        return;

                    case 20: //รายงานสรุปจำนวนตราประทับ
                        if (Configs.UseActivePromotion)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report20_Active.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report20.rpt");

                        รายงานสรุปจำนวนตราประทับ(reportDocument);
                        return;

                    case 21: //รายงานจำนวนตราประทับรถยนต์แบบแจกแจง
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report21.rpt");

                        รายงานจำนวนตราประทับรถยนต์แบบแจกแจง(reportDocument, sql);
                        return;

                    case 22: //รายงานรายวันจำนวนตราประทับรถยนต์แบบแจกแจง
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report22.rpt");

                        รายงานรายวันจำนวนตราประทับรถยนต์แบบแจกแจง(reportDocument, sql);
                        return;

                    case 23: //รายชื่อสมาชิก
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report23.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 24: //รายได้จากสมาชิก
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report24_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report24.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 25: //สรุปรถเข้า-ออกตามช่วงเวลา เฉพาะวัน
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report25.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);

                        DateTime dst = StartDatePicker.Value;
                        string startDateTime = dst.ToString("dd MMMM ") + dst.Year.ToString();
                        reportDocument.DataDefinition.FormulaFields["ReportName"].Text = "'ตั้งแต่วันที่ " + startDateTime + " 00:00:00 ถึงวันที่ " + startDateTime + " 23:59:59'";
                        break;

                    case 26: //รายงานรถยนต์เข้าออก ตามช่วงเวลา
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report26.rpt");

                        DataTable dataTable26 = DataTableManager.สรุปรถยนต์เข้าออกตามชั่วโมง(dataFromQuery);

                        TrySetReportData(reportDocument, dataTable26);

                        CalculationsManager.AddTotalToGridView(selectedReportId, ResultGridView);

                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่ " + startDateTime + " 00:00:00 ถึงวันที่ " + startDateTime + " 23:59:59'";
                        break;

                    case 27: //รายงานรถยนต์เข้าออก ตามวันที่
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report27.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);

                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        DateTime dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();
                        reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                        break;

                    case 28: //รายงานตราประทับ
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report28.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);

                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();
                        reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                        break;

                    case 29: //รายงานรถเข้าออก แสดงช่องทาง
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report29.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);

                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();
                        reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                        break;

                    case 30:
                        DataTable dataTable30 = DataTableManager.สรุปรถยนต์เข้าออกตามวันที่(dataFromQuery);

                        ResultGridView.DataSource = dataTable30;

                        SetColumnWidthIfExists("วันที่", 180);
                        SetColumnWidthIfExists("ไม่ได้ประทับตรา", 120);

                        CalculationsManager.AddTotalToGridView(selectedReportId, ResultGridView);
                        return;

                    case 31:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report31_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report31.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);

                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                        try
                        {
                            reportDocument.DataDefinition.FormulaFields["Header"].Text =
                            $"'รายงาน{reportName} จากวันที่ {startDateTime} เวลา {startTime} ถึงวันที่ {endDateTime} เวลา {endTime}'";
                        }
                        catch { }

                        break;

                    case 32:
                        DataTable dataTable32 = DataTableManager.คงค้างแสดงรูปภาพ(dataFromQuery);

                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report32_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report32.rpt");

                        TrySetReportData(reportDocument, dataTable32);

                        SetDisplayImageUI();
                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();
                        reportDocument.DataDefinition.FormulaFields["ReportName"].Text =
                        $"'รายงาน{reportName} จากวันที่ {startDateTime} เวลา {startTime} ถึงวันที่ {endDateTime} เวลา {endTime}'";
                        break;

                    case 33: //รายงานยกเลิกใบกำกับภาษีอย่างย่อ
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report33.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        reportDocument.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานยกเลิกใบกำกับภาษีอย่างย่อประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.ToString("yyyy") + " ถึงวันที่ " + EndDatePicker.Value.ToString("d MMMM ") + EndDatePicker.Value.ToString("yyyy") + "'";
                        break;

                    case 34: //รายงานภาษีขายค่าบริการที่จอดรถประจำวัน
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report34.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        reportDocument.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขายค่าบริการที่จอดรถประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.AddYears(543).ToString("yyyy") + "'";

                        #region cal sum 34
                        double sumVat = 0;
                        double sumBefore = 0;
                        double sumTotal = 0;
                        double sumCntSlip = 0;

                        for (int j = 0; j < dataFromQuery.Rows.Count; j++)
                        {
                            sumVat += Convert.ToDouble(dataFromQuery.Rows[j]["VAT"]);
                            sumBefore += Convert.ToDouble(dataFromQuery.Rows[j]["ค่าบริการ"]);
                            sumTotal += Convert.ToDouble(dataFromQuery.Rows[j]["รวมเงิน"]);
                            sumCntSlip += Convert.ToDouble(dataFromQuery.Rows[j]["จำนวนใบ"]);
                        }

                        if (Configs.UseCalVatFromTotal)
                        {
                            reportDocument.DataDefinition.FormulaFields["Pa0"].Text = "'" + (sumTotal - (sumTotal * 7 / 107)).ToString("#,###,##0.00") + "'";
                            reportDocument.DataDefinition.FormulaFields["Pa1"].Text = "'" + (sumTotal * 7 / 107).ToString("#,###,##0.00") + "'";
                            reportDocument.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotal.ToString("#,###,##0.00") + "'";
                        }
                        else
                        {
                            reportDocument.DataDefinition.FormulaFields["Pa0"].Text = "'" + sumBefore.ToString("#,###,##0.00") + "'";
                            reportDocument.DataDefinition.FormulaFields["Pa1"].Text = "'" + sumVat.ToString("#,###,##0.00") + "'";
                            reportDocument.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotal.ToString("#,###,##0.00") + "'";
                        }
                        reportDocument.DataDefinition.FormulaFields["Pa3"].Text = "'" + sumCntSlip.ToString("#,###,##0") + "'";
                        #endregion

                        break;

                    case 35: //รายงานภาษีขายค่าบริการที่จอดรถประจำเดือน
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report35.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        reportDocument.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานภาษีขายค่าบริการที่จอดรถประจำเดือน " + StartDatePicker.Value.ToString("MMMM") + " " + StartDatePicker.Value.AddYears(543).ToString("yyyy") + "'";

                        #region cal sum 35
                        double sumVatM = 0;
                        double sumBeforeM = 0;
                        double sumTotalM = 0;
                        double sumCountSlip = 0;

                        for (int j = 0; j < dataFromQuery.Rows.Count; j++)
                        {
                            sumVatM += Convert.ToDouble(dataFromQuery.Rows[j]["VAT"]);
                            sumBeforeM += Convert.ToDouble(dataFromQuery.Rows[j]["ค่าบริการ"]);
                            sumTotalM += Convert.ToDouble(dataFromQuery.Rows[j]["รวมเงิน"]);
                            sumCountSlip += Convert.ToDouble(dataFromQuery.Rows[j]["จำนวนใบ"]);
                        }

                        if (Configs.UseCalVatFromTotal)
                        {
                            reportDocument.DataDefinition.FormulaFields["Pa0"].Text = "'" + (sumTotalM - (sumTotalM * 7 / 107)).ToString("#,###,##0.00") + "'";
                            reportDocument.DataDefinition.FormulaFields["Pa1"].Text = "'" + (sumTotalM * 7 / 107).ToString("#,###,##0.00") + "'";
                            reportDocument.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotalM.ToString("#,###,##0.00") + "'";
                        }
                        else
                        {
                            reportDocument.DataDefinition.FormulaFields["Pa0"].Text = "'" + sumBeforeM.ToString("#,###,##0.00") + "'";
                            reportDocument.DataDefinition.FormulaFields["Pa1"].Text = "'" + sumVatM.ToString("#,###,##0.00") + "'";
                            reportDocument.DataDefinition.FormulaFields["Pa2"].Text = "'" + sumTotalM.ToString("#,###,##0.00") + "'";
                        }
                        reportDocument.DataDefinition.FormulaFields["Pa3"].Text = "'" + sumCountSlip.ToString("#,###,##0") + "'";
                        #endregion

                        break;

                    case 36: //รายงานสรุปรายได้
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report36.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        reportDocument.DataDefinition.FormulaFields["ReportName"].Text = "'รายงานสรุปรายได้ประจำวันที่ " + StartDatePicker.Value.ToString("d MMMM ") + StartDatePicker.Value.AddYears(543).ToString("yyyy") + " ถึงวันที่ " + EndDatePicker.Value.ToString("d MMMM ") + EndDatePicker.Value.AddYears(543).ToString("yyyy") + "'";

                        #region cal sum 36
                        var columnToFormulaMap = new (string Column, string Formula)[]
                        {
                           ("ค่าจอดรถ", "Pa0"),
                           ("ค่าปรับบัตรหาย", "Pa1"),
                           ("ค่าปรับค้างคืน", "Pa2"),
                           ("ค่าบริการ PromptPay", "Pa3"),
                           ("ค่าบริการเงินสด", "Pa4"),
                           ("รวมค่าบริการ", "Pa5"),
                           ("รวม VAT", "Pa6"),
                           ("รวมสุทธิ", "Pa7")
                        };
                        int rowCount = ResultGridView.Rows.Count - 1;

                        var sums = columnToFormulaMap.ToDictionary(x => x.Column, x => 0.0);

                        foreach (DataRow row in dataFromQuery.Rows)
                        {
                            foreach (var col in columnToFormulaMap)
                            {
                                if (row[col.Column] != DBNull.Value)
                                    sums[col.Column] += Convert.ToDouble(row[col.Column]);
                            }
                        }

                        foreach (var map in columnToFormulaMap)
                        {
                            string formatted = sums[map.Column].ToString("#,###,##0.00");

                            ResultGridView.Rows[rowCount].Cells[map.Column].Value = formatted;

                            reportDocument.DataDefinition.FormulaFields[map.Formula].Text = $"'{formatted}'";
                        }
                        #endregion
                        break;

                    case 37:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report37.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);

                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();
                        reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                        break;

                    case 38:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report38.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);

                        #region cal sum 38
                        int sumCoupon = 0;
                        int sumPrice = 0;

                        for (int j = 0; j < dataFromQuery.Rows.Count; j++)
                        {
                            sumCoupon += Convert.ToInt32(dataFromQuery.Rows[j]["No of Coupon"]);
                            sumPrice += Convert.ToInt32(dataFromQuery.Rows[j]["Actual Payment"]);
                        }

                        reportDocument.DataDefinition.FormulaFields["Pa0"].Text = "'" + sumCoupon + "'";
                        reportDocument.DataDefinition.FormulaFields["Pa1"].Text = "'" + sumPrice + "'";
                        #endregion

                        break;

                    case 41:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report41_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report41.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);

                        ResultGridView.Columns[3].Visible = false;
                        ResultGridView.Columns[4].Visible = false;

                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();
                        reportDocument.DataDefinition.FormulaFields["ReportName"].Text =
                        $"'รายงาน{reportName} จากวันที่ {startDateTime} เวลา {startTime} ถึงวันที่ {endDateTime} เวลา {endTime}'";
                        break;

                    case 42: //การเข้าออกMember แสดงรูปภาพ
                        DataTable dataTable42 = DataTableManager.การเข้าออกMemberแสดงรูปภาพ(dataFromQuery);

                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report42_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report42.rpt");

                        TrySetReportData(reportDocument, dataTable42);

                        SetDisplayImageUI();
                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();
                        reportDocument.DataDefinition.FormulaFields["ReportName"].Text =
                        $"'รายงาน{reportName} จากวันที่ {startDateTime} เวลา {startTime} ถึงวันที่ {endDateTime} เวลา {endTime}'";
                        break;

                    case 47:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report47.rpt");
                        TrySetReportData(reportDocument, dataFromQuery, true);

                        break;

                    case 48:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report48.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        SetColumnWidthIfExists("บริษัท", 450);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 49:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report49.rpt");
                        TrySetReportData(reportDocument, dataFromQuery, true);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 50:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report50_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report50.rpt");

                        TrySetReportData(reportDocument, dataFromQuery, true);
                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 51:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report51.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        if (selectedReportId == 51) //Mac 2020/10/26
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
                                this.ResultGridView.Columns[0].Visible = false;
                            }
                        }
                        else
                        {
                            this.ResultGridView.TopLeftHeaderCell.Value = "";
                        }
                        break;
                    case 52:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report52.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 53:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report53.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 54:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report54.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 55:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report55.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 56:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report56.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 57:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report57.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 58:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report58.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 59:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report59.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 60:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report60.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 61:
                        PrimaryTabControl.SelectTab(1);
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report61.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 62:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report62.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 63:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report63.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 64:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report64.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);


                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 65:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report65.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 66:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report66.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 67:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report67.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 68:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report68.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 69:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report69.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 70:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report70.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 71:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report71.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 72:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report72.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 73:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report73.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 74:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report74.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 75:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report75.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 76:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report76.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 77:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report77.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        reportDocument.DataDefinition.FormulaFields["SumCar"].Text = "'" + (ResultGridView.Rows.Count - 1).ToString("#,###,##0") + "'";

                        ResultGridView.Columns[2].Visible = false;
                        ResultGridView.Columns[3].Visible = false;

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 79:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report79.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                        reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";
                        break;

                    case 80:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report80_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report80.rpt");

                        ResultGridView.Columns[2].Visible = false;
                        ResultGridView.Columns[3].Visible = false;
                        break;

                    case 81:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report81.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 82:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report82.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 83:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report83.rpt");

                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                        reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;
                    case 84:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report84.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 85:
                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();
                        if (GuardhouseComboBox.SelectedIndex > 0)
                            reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "        ป้อม : " + GuardhouseComboBox.Text + "'";
                        else
                            reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";

                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report85.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 86:
                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                        if (GuardhouseComboBox.SelectedIndex > 0)
                            reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "        ป้อม : " + GuardhouseComboBox.Text + "'";
                        else
                            reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";

                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report86.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 87:
                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                        if (GuardhouseComboBox.SelectedIndex > 0)
                            reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "        ป้อม : " + GuardhouseComboBox.Text + "'";
                        else
                            reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";

                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report87.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 88:
                        dst = StartDatePicker.Value;
                        startDateTime = dst.ToString("dd MMMM") + " " + dst.Year.ToString();
                        dfn = EndDatePicker.Value;
                        endDateTime = dfn.ToString("dd MMMM") + " " + dfn.Year.ToString();

                        if (GuardhouseComboBox.SelectedIndex > 0)
                            reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "        ป้อม : " + GuardhouseComboBox.Text + "'";
                        else
                            reportDocument.DataDefinition.FormulaFields["head"].Text = "'ตั้งแต่วันที่  " + startDateTime + "  ถึงวันที่  " + endDateTime + "'";

                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report88.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 89:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report89.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 94:
                        if (Configs.Reports.ReportNoRunning)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report95_NoRunning.rpt");
                        else
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report95.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 96:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report96.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 97:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report97.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 100:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report100.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);

                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 101:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report101.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 102:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report102.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 103:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report103.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 104:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report104.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 105: //Mac 2020/03/09
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report105.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 106:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report106.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 107:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report107.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 108:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report108.rpt");

                        if (MemberGroupMonthComboBox.Text.Trim() == Constants.TextBased.All)
                            reportDocument.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : ทั้งหมด'";
                        else
                            reportDocument.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : " + AppGlobalVariables.MemberGroupMonthsToId[MemberGroupMonthComboBox.Text] + " " + MemberGroupMonthComboBox.Text + "'";

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 109:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report109.rpt");

                        if (MemberGroupMonthComboBox.Text.Trim() == Constants.TextBased.All)
                            reportDocument.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : ทั้งหมด'";
                        else
                            reportDocument.DataDefinition.FormulaFields["Condition2"].Text = "'รหัส/บริษัท : " + AppGlobalVariables.MemberGroupMonthsToId[MemberGroupMonthComboBox.Text] + " " + MemberGroupMonthComboBox.Text + "'";

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 161:
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report161.rpt");
                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 164: // การเข้าออกของรถยนต์แสดงช่องทางการชำระเงิน
                        if (Configs.UsePaymentBeam)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report164_Beam.rpt");
                        else if (Configs.UsePaymentKsher)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report164_Ksher.rpt");
                        else if (Configs.UsePaymentRabbit)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report164_Rabbit.rpt");
                        else if (Configs.UsePaymentScb)
                            reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report164_SCB.rpt");

                        TrySetReportData(reportDocument, dataFromQuery);
                        break;

                    case 165: // รายงานสรุปจำนวนรถและรายได้
                        DataTable dataTable = CRUDManager.GetVehicleEarningSummary(sql, StartDatePicker.Value, EndDatePicker.Value);
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\VehicleEarningSummary.rpt");
                        reportDocument.SetDataSource(dataTable);

                        reportDocument.DataDefinition.FormulaFields["CompanyName"].Text = $"'{AppGlobalVariables.Printings.Company2}'";
                        PrimaryTabControl.SelectTab(1);
                        break;

                    case 166: // สรุปจำนวนบัตรทั้งหมดตามบริษัท
                        ResultGridView.DataSource = null;

                        DataTable dataTable166 = CRUDManager.GetCardSortByCompanySummary(sql);
                        reportDocument.Load($"{FolderDirectories.CrystalReport}\\CardSortedCompanySummary.rpt");
                        reportDocument.SetDataSource(dataTable166);
                        TrySetCrystalReportHeaders(reportDocument);
                        PrimaryCrystalReportViewer.ReportSource = reportDocument;
                        PrimaryCrystalReportViewer.Refresh();

                        PrimaryTabControl.SelectTab(1);

                        break;
                }

                CalculationsManager.AddTotalToGridView(selectedReportId, ResultGridView);
            }
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

        private void SavePermission(object sender, EventArgs e)
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

            PrimaryTabControl.TabPages.Remove(tabUser);

            RemoveTab("tabPer");

            PrimaryTabControl.SelectTab(0);


            MessageBox.Show("กรุณาออกโปรแกรม แล้วเข้าใหม่ เพื่อดูรายงาน");
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

            DataGridView dgv = new DataGridView
            {
                Name = "dgvPer",
                Width = 1305,
                Height = dgvH - 50,
                AllowUserToAddRows = false
            };

            dgv.Columns.Add("id", "รหัส");
            dgv.Columns.Add("name", "ชื่อ");

            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
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

            Button savePer = new Button
            {
                Name = "savePer",
                Text = "Save"
            };
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
            string sql = "select id,cardname as การ์ด,level as เลเวล,username,password,name as ชื่อ_นามสกุล ,address as ที่อยูา,tel as เบอร์โทร,groupreportDocument as กลุ่มรายงาน from user";
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
                    sql += " groupreportDocument =" + UserGroupTextBox.Text;
                else sql += " groupreportDocument = NULL";
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
                    sql += " groupreportDocument =" + UserGroupTextBox.Text + ")";
                else sql += " groupreportDocument = NULL)";
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
            if (selectedReportId == 1 || selectedReportId == 2 || selectedReportId == 3 || selectedReportId == 4 || selectedReportId == 5 || selectedReportId == 6 || selectedReportId == 9 || selectedReportId == 11 || selectedReportId == 13 || selectedReportId == 14 || selectedReportId == 31 || selectedReportId == 32 || selectedReportId == 51 || selectedReportId == 80 || selectedReportId == 91 || selectedReportId == 92 || selectedReportId == 93 || selectedReportId == 94 || selectedReportId == 95) //Mac 2020/10/26
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
                    if (selectedReportId == 13 || selectedReportId == 14)
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

                case 163:
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
        #endregion PROCESS_END


        #region UI_EVENT_HANDLER
        private void ResultGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CalculationsManager.AddTotalToGridView(selectedReportId, ResultGridView);
        }

        private void LoadImageToPictureBox(
            DataGridView grid,
            int rowIndex,
            string columnName,
            PictureBox targetPictureBox)
        {
            try
            {
                targetPictureBox.Image?.Dispose();
                targetPictureBox.Image = null;

                var cellValue = grid.Rows[rowIndex].Cells[columnName].Value;

                if (cellValue != null && cellValue is byte[] bytes && bytes.Length > 0)
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        targetPictureBox.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    targetPictureBox.Image = null;
                }
            }
            catch
            {
                targetPictureBox.Image = null;
            }
        }

        private void ResultGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;

            if (e.RowIndex < ResultGridView.Rows.Count && e.RowIndex > -1)
            {

                if (selectedReportId == 2 || selectedReportId == 92) //Mac 2020/10/26
                {
                    int iVil = 0;
                    if (Configs.IsVillage && Configs.Use2Camera) iVil = 5;
                    if (Configs.NoPanelUp2U == "2") //Mac 2017/03/13
                        iVil += 4;

                    string pic1, pic2, pic3, pic4, pic5;
                    if (Configs.Use2Camera)
                    {
                        // รูปคนขับเข้า
                        LoadImageToPictureBox(ResultGridView, e.RowIndex, "iv", pictureBox1);

                        // รูปทะเบียนเข้า
                        LoadImageToPictureBox(ResultGridView, e.RowIndex, "il", pictureBox2);


                        //pic1 = ResultGridView.Rows[e.RowIndex].Cells["iv"].Value.ToString();
                        //pic2 = ResultGridView.Rows[e.RowIndex].Cells["il"].Value.ToString();

                        /* Old
                        pic1 = ResultGridView.Rows[e.RowIndex].Cells[11 + iVil].Value.ToString();
                        pic2 = ResultGridView.Rows[e.RowIndex].Cells[9 + iVil].Value.ToString();
                        */
                    }
                    else
                    {
                        pic1 = ResultGridView.Rows[e.RowIndex].Cells[9 + iVil].Value.ToString();
                        pic2 = ResultGridView.Rows[e.RowIndex].Cells[10 + iVil].Value.ToString();
                    }

                    if (Configs.IsVillage && Configs.Use2Camera)
                    {
                        pic5 = ResultGridView.Rows[e.RowIndex].Cells[13 + iVil].Value.ToString();
                        if (pic5.Trim() != "" || pic5 != null)
                        {
                            Image im = ImagesManager.GetCopyImage(pic5);
                            pictureBox5.Image = im;
                        }
                    }
                    else if (Configs.Use2Camera && Configs.IPIn3.Trim().Length > 0) //Mac 2015/02/04
                    {
                        pic5 = ResultGridView.Rows[e.RowIndex].Cells[13 + iVil].Value.ToString();
                        if (pic5.Trim() != "" || pic5 != null)
                        {
                            Image im = ImagesManager.GetCopyImage(pic5);
                            pictureBox5.Image = im;
                        }
                    }

                    if (Configs.Use2Camera)
                    {
                        /* Old
                        pic3 = ResultGridView.Rows[e.RowIndex].Cells[12 + iVil].Value.ToString();
                        pic4 = ResultGridView.Rows[e.RowIndex].Cells[10 + iVil].Value.ToString();
                        */
                        // รูปคนขับออก
                        LoadImageToPictureBox(ResultGridView, e.RowIndex, "ov", pictureBox3);

                        // รูปทะเบียนออก
                        LoadImageToPictureBox(ResultGridView, e.RowIndex, "ol", pictureBox4);

                        //pic3 = ResultGridView.Rows[e.RowIndex].Cells["ov"].Value.ToString();
                        //pic4 = ResultGridView.Rows[e.RowIndex].Cells["ol"].Value.ToString();

                        //if (pic3.Trim() != "" || pic3 != null)
                        //{
                        //    Image im = GetCopyImage(pic3);
                        //    pictureBox3.Image = im;
                        //}

                        //if (pic4.Trim() != "" || pic4 != null)
                        //{
                        //    Image im = GetCopyImage(pic4);
                        //    pictureBox4.Image = im;
                        //}
                    }

                }
                if (selectedReportId == 8 || selectedReportId == 32 || selectedReportId == 94) //Mac 2020/10/26
                {
                    // รูปคนขับเข้า
                    LoadImageToPictureBox(ResultGridView, e.RowIndex, "picdiv", pictureBox1);

                    // รูปทะเบียนเข้า
                    LoadImageToPictureBox(ResultGridView, e.RowIndex, "piclic", pictureBox2);
                }
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

            PromotionComboBox.SelectedIndex = 0;

            // Reset common UI
            RecordNumberTextBox.Text = "";
            ViewBlockerPanel.Visible = false;

            // ---------- SetReportConditionButton ----------
            switch (selectedReportId)
            {
                case 13:
                case 14:
                case 15:
                case 16:
                case 20:
                case 21:
                case 12:
                    SetReportConditionButton.Visible = true;
                    break;
                default:
                    SetReportConditionButton.Visible = false;
                    break;
            }

            // ---------- Record number ----------
            switch (selectedReportId)
            {
                case 41:
                case 42:
                case 77:
                case 78:
                    label30.Visible = true;
                    RecordNumberTextBox.Visible = true;
                    break;
                default:
                    label30.Visible = false;
                    RecordNumberTextBox.Visible = false;
                    break;
            }

            // ---------- Address panel ----------
            AddressPanel.Visible = selectedReportId == 90;

            // ---------- Payment channel panel ----------
            PaymentChannelPanel.Visible = selectedReportId == 23;

            // ---------- Parking time comparison ----------
            ParkingTimeComparisonPanel.Visible = selectedReportId == 96;

            // ---------- Payment status enable ----------
            bool disablePaymentStatus = selectedReportId == 162;
            label20.Enabled = !disablePaymentStatus;
            PaymentStatusComboBox.Enabled = !disablePaymentStatus;

            // ---------- Payment channel combo ----------
            switch (selectedReportId)
            {
                case 164:
                case 49:
                case 50:
                    label42.Visible = true;
                    PaymentChannelComboBox.Visible = true;
                    PaymentChannelComboBox.Text = Constants.TextBased.All;
                    break;
                default:
                    label42.Visible = false;
                    PaymentChannelComboBox.Visible = false;
                    break;
            }

            // ---------- Promotion ID range ----------
            switch (selectedReportId)
            {
                case 21:
                case 22:
                case 47:
                case 162:
                case 166:
                    PromotionIdFrom.Clear();
                    PromotionIdTo.Clear();
                    PromotionComboBox.SelectedIndex = 0;

                    PromotionIdRangePanel.Visible = true;
                    PromotionIdRangePanel.Location = new Point(347, 85);
                    break;
                default:
                    PromotionIdRangePanel.Visible = false;
                    break;
            }

            switch (selectedReportId)
            {
                case 36:
                    ViewBlockerPanel.Bounds = new Rectangle(9, 48, 1095, 135);
                    ViewBlockerPanel.Visible = true;
                    break;
                default:
                    break;
            }
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
            ClearUser();
        }

        private void ManageUserSaveButton_Click(object sender, EventArgs e)
        {
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

        private void ManagePermissionButton_Click(object sender, EventArgs e)
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

            ApplyConditionalUIChanges();

            string sql = new ReportQueryService().BuildReportQuery(
                this.selectedReportId,
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

            ReportHeaderLabel.Text = AppGlobalVariables.Printings.Header = SetReportHeader().Replace("รายงานรายงาน", "รายงาน");
            Display(sql);

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

        private void Handle131415(ReportDocument reportDocument, DataTable dataTable)
        {
            if (dataTable?.Rows.Count <= 0)
                return;

            if (Configs.Reports.ReportNoRunning)
            {
                if (selectedReportId == 13)
                    reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report13_NoRunning.rpt");
                else if (selectedReportId == 14)
                    reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report14_NoRunning.rpt");
            }
            else
            {
                if (selectedReportId == 13)
                {
                    reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report13.rpt");
                }
                else if (selectedReportId == 14)
                {
                    reportDocument.Load($"{FolderDirectories.CrystalReport}\\Report14.rpt");
                }
            }

            #region Set data to ResultGridView and PrimaryCrystalReportViewer
            ResultGridView.DataSource = DataTableManager.ConvertedTableType(dataTable);

            DataTableManager.CaseReportPricePromotion(selectedReportId, ResultGridView);

            DataTable dtNew = DataTableManager.ConvertedDataGridView(ResultGridView);

            if (dtNew.Rows.Count > 0)
                reportDocument.SetDataSource(dtNew);

            if (!Configs.Reports.ReportNoRunning)
                ResultGridViewAtRunning();

            PrimaryCrystalReportViewer.ReportSource = reportDocument;
            PrimaryCrystalReportViewer.Refresh();

            SetReportColumnsWidth();

            TrySetCrystalReportHeaders(reportDocument);
            #endregion

            ResultGridView.Columns["เจ้าหน้าที่ขาออก"].Visible = false;
            ResultGridView.Columns["ชม.ส่วนลดผู้มาติดต่อ"].Visible = false;

            PdfExportButton.Enabled = true;
            ExcelExportButton.Enabled = true;
        }

        private void TrySetReportData(ReportDocument reportDocument, DataTable dataTable, bool hideDataGridView = false)
        {
            if (dataTable.Rows.Count > 0)
            {
                if (!hideDataGridView)
                {
                    ResultGridView.DataSource = dataTable;

                    if (!Configs.Reports.ReportNoRunning)
                        ResultGridViewAtRunning();

                    SetReportColumnsWidth();
                }

                try
                {
                    reportDocument.SetDataSource(dataTable);
                }
                catch { }

                //ResultGridView.AutoResizeColumns();

                PrimaryCrystalReportViewer.ReportSource = reportDocument;
                PrimaryCrystalReportViewer.Refresh();
            }

            TrySetCrystalReportHeaders(reportDocument);

            if (hideDataGridView)
                PrimaryTabControl.SelectTab(1);
            PdfExportButton.Enabled = true;
            ExcelExportButton.Enabled = true;
        }

        private void TrySetCrystalReportHeaders(ReportDocument reportDocument)
        {
            string startDate = StartDatePicker.Value.ToString("yyyy-MM-dd");
            string endDate = EndDatePicker.Value.ToString("yyyy-MM-dd");
            string startTime = StartTimePicker.Value.ToLongTimeString();
            string endTime = EndTimePicker.Value.ToLongTimeString();
            string startDateTime = startDate + " " + startTime;
            string endDateTime = endDate + " " + endTime;
            string reportName = ReportComboBox.Text;
            string conditionText = AppGlobalVariables.ConditionText;

            ReportHeaderLabel.Text = $"{reportName} {conditionText}";

            string nTel = "";
            string nFax = "";
            string nTax = "";
            #region get fax/tax/tel
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
            #endregion

            #region set headers
            try
            {
                reportDocument.DataDefinition.FormulaFields["CompanyName"].Text = $"'{AppGlobalVariables.Printings.Company1}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["CompanyName"].Text = $"'{AppGlobalVariables.Printings.Company1}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Condition"].Text = $"'{conditionText}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["ReportName"].Text = $"'{reportName}'";
            }
            catch { }
            try
            {
                var monthText = StartDatePicker.Value.ToString(" MMMM ");
                var thaiYear = StartDatePicker.Value.Year + 543;

                reportDocument.DataDefinition.FormulaFields["ReportCon"].Text =
                    $"'เดือนภาษี{monthText}{thaiYear}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["VehicleType"].Text = $"'{AppGlobalVariables.Database.VehicleTypeTh}'";
            }
            catch { }
            try
            {
                if (MemberGroupMonthComboBox.SelectedIndex > 0)
                    reportDocument.DataDefinition.FormulaFields["Grouppro"].Text = $"'ชื่อลูกค้า : {MemberGroupMonthComboBox.Text}'";
                else
                    reportDocument.DataDefinition.FormulaFields["Grouppro"].Text = $"'โปรโมชั่น : {PromotionComboBox.Text}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["LongAddress"].Text = $"'{AppGlobalVariables.Printings.Address1.Trim()} {AppGlobalVariables.Printings.Address2.Trim()}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Address"].Text = $"'{AppGlobalVariables.Printings.Company2}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Address1"].Text = $"'{AppGlobalVariables.Printings.Address1}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Address2"].Text = $"'{AppGlobalVariables.Printings.Address2}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["TaxID"].Text = $"'{AppGlobalVariables.Printings.Tax1}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["TaxMonth"].Text = $"'{StartDatePicker.Value.ToString("MMMM ปี ")}{StartDatePicker.Value.Year}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Branch"].Text = $"'{AppGlobalVariables.Printings.Branch}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Building"].Text = $"'{AppGlobalVariables.Printings.Building}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Office"].Text = $"'{AppGlobalVariables.Printings.Office}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Date"].Text = $"'Date: {StartDatePicker.Value.ToString("d MMMM ")} {StartDatePicker.Value.ToString("yyyy")}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["DatePrint"].Text = "'พิมพ์วันที่ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["DatetimePrint"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "  " + DateTime.Now.ToString("d/M/") + DateTime.Now.ToString("yyyy HH:mm:ss") + "'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["ReportMonth"].Text = $"'ประจำเดือน {TextFormatters.ExtractThaiMonthFromDate(endDate)}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["PrintedPersonnel"].Text = $"'Printed By: {AppGlobalVariables.OperatingUser.Name}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["Sender"].Text = "'" + AppGlobalVariables.OperatingUser.Name + "'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["PrintedByUser"].Text = $"'{AppGlobalVariables.OperatingUser.Name}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["ReportSearchDate"].Text = $"'{startDate} ถึง {endDate}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["FooterName1"].Text = $"'{AppGlobalVariables.Printings.ReportFooter1}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["FooterName2"].Text = $"'{AppGlobalVariables.Printings.ReportFooter2}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["FooterName3"].Text = $"'{AppGlobalVariables.Printings.ReportFooter3}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["FooterName4"].Text = $"'{AppGlobalVariables.Printings.ReportFooter4}'";
            }
            catch { }
            try
            {
                reportDocument.DataDefinition.FormulaFields["FooterName5"].Text = $"'{AppGlobalVariables.Printings.ReportFooter5}'";
            }
            catch { }
            #endregion

            try
            {
                reportDocument.SetParameterValue("compName", AppGlobalVariables.Printings.Company1.Trim());
            }
            catch { }
            try
            {
                reportDocument.SetParameterValue("ComAddress1", AppGlobalVariables.Printings.Address1.Trim() + "\r\n" + AppGlobalVariables.Printings.Address2.Trim());
            }
            catch { }
            try
            {
                reportDocument.SetParameterValue("ComAddress1", AppGlobalVariables.Printings.Address1.Trim() + "\r\n" + AppGlobalVariables.Printings.Address2.Trim());
            }
            catch { }
            try
            {
                reportDocument.SetParameterValue("ComTel", nTel);
            }
            catch { }
            try
            {
                reportDocument.SetParameterValue("comFax", nFax);
            }
            catch { }
            try
            {
                reportDocument.SetParameterValue("DateSearch", startDateTime);
            }
            catch { }
            try
            {
                reportDocument.SetParameterValue("DateSearch2", endDateTime);
            }
            catch { }
        }

        private void SetReportColumnsWidth()
        {
            SetColumnWidthIfExists("ยอดรวม", 100);
            SetColumnWidthIfExists("เวลาเข้า", 120);
            SetColumnWidthIfExists("เวลาออก", 120);
            SetColumnWidthIfExists("เวลาเคลียร์บัตร", 120);
            SetColumnWidthIfExists("วัน-เวลาเข้า", 120);
            SetColumnWidthIfExists("วัน-เวลาออก", 120);
            SetColumnWidthIfExists("เวลายก", 120);
            SetColumnWidthIfExists("วันที่สมัคร", 120);
            SetColumnWidthIfExists("วันที่หมดอายุ", 120);
            SetColumnWidthIfExists("วันหมดอายุ", 120);
            SetColumnWidthIfExists("วันที่ชำระ", 120);
            SetColumnWidthIfExists("เจ้าหน้าที่ขาออก", 140);
            SetColumnWidthIfExists("E-Stamp", 160);
            SetColumnWidthIfExists("โปรโมชัน", 160);
            SetColumnWidthIfExists("โปรโมชั่น", 160);
            SetColumnWidthIfExists("บันทึก", 260);
            SetColumnWidthIfExists("เหตุผล", 260);
            SetColumnWidthIfExists("ชื่อ - นามสกุล", 260);
            SetColumnWidthIfExists("เลขที่ใบกำกับภาษี", 150);
            SetColumnWidthIfExists("E-Stamp", 350);
            SetColumnWidthIfExists("เจ้าหน้าที่", 250);
            /* switch (selectedReportId) { case: } */
        }

        private void SetColumnWidthIfExists(string columnName, int width)
        {
            if (ResultGridView.Columns.Contains(columnName))
            {
                ResultGridView.Columns[columnName].Width = width;
            }
        }

        private void SetDisplayImageUI()
        {
            ResultGridView.Location = new Point(ResultGridView.Location.X, ResultGridView.Location.Y + 150);
            ResultGridView.Height = ResultGridView.Height - 150;
            groupBox3.Visible = true;

            HideColumnIfExists("picdiv");
            HideColumnIfExists("piclic");

            HideColumnIfExists("il");
            HideColumnIfExists("ol");
            HideColumnIfExists("iv");
            HideColumnIfExists("ov");

            if (Configs.IsVillage && Configs.Use2Camera)
            {
                pictureBox5.Visible = false;
                lbPic5.Visible = false;
            }

            // Set image boxes visibility
            if (selectedReportId != 2 && selectedReportId != 42)
            {
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                lbPic3.Visible = false;
                lbPic4.Visible = false;

                lbPic1.Text = "รูปคนขับ";
                lbPic2.Text = "รูปทะเบียน";
            }
            else
            {
                pictureBox3.Visible = true;
                pictureBox4.Visible = true;
                lbPic3.Visible = true;
                lbPic4.Visible = true;
                lbPic1.Text = "รูปคนขับขาเข้า";
                lbPic2.Text = "รูปทะเบียนขาเข้า";
                lbPic3.Text = "รูปคนขับขาออก";
                lbPic4.Text = "รูปทะเบียนขาออก";
            }
        }

        private void HideColumnIfExists(string columnName)
        {
            if (ResultGridView.Columns.Contains(columnName))
            {
                ResultGridView.Columns[columnName].Visible = false;
            }
        }

        private void ApplyConditionalUIChanges()
        {
            if (AppGlobalVariables.OperatingUser.Level > 2 &&
                (selectedReportId == 13 || selectedReportId == 38))
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
                catch (Exception) { }
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

        private void รายงานรายวันจำนวนตราประทับรถยนต์แบบแจกแจง(ReportDocument reportDocument, string sql)
        {
            DataTable itemizedTable = CRUDManager.GetItemizedPromotionUsage(sql, Constants.TextBased.All);
            DataTable mappedTable = CRUDManager.GetItemizedDailyPromotionUsage(
                itemizedTable,
                PromotionComboBox.Text,
                PaymentStatusComboBox.Text,
                StartDatePicker.Value,
                EndDatePicker.Value);

            if (mappedTable.Rows.Count > 0)
            {
                reportDocument.SetDataSource(mappedTable);

                TrySetCrystalReportHeaders(reportDocument);

                PrimaryCrystalReportViewer.ReportSource = reportDocument;
                PrimaryCrystalReportViewer.Refresh();

                PrimaryTabControl.SelectTab(1);
            }

            return;
        }

        private void รายงานจำนวนตราประทับรถยนต์แบบแจกแจง(ReportDocument reportDocument, string sql)
        {
            DataTable itemizedMap = CRUDManager.GetItemizedPromotionUsage(sql, PaymentStatusComboBox.Text);

            if (itemizedMap.Rows.Count > 0)
            {
                reportDocument.SetDataSource(itemizedMap);

                TrySetCrystalReportHeaders(reportDocument);

                PrimaryTabControl.SelectTab(1);

                PrimaryCrystalReportViewer.ReportSource = reportDocument;
                PrimaryCrystalReportViewer.Refresh();
            }
        }

        private void รายงานสรุปจำนวนตราประทับ(ReportDocument reportDocument)
        {
            string sql = "select id,name";

            if (Configs.UseActivePromotion)
                sql += ",active";
            sql += " from promotion";
            if (MemberGroupMonthComboBox.SelectedIndex > 0)
                sql += " where groupro = " + AppGlobalVariables.MemberGroupMonthsToId[MemberGroupMonthComboBox.Text];
            else if (PromotionComboBox.Text != "ALL")
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
                    estampSumMap.Columns.Add(new DataColumn("active", typeof(string)));
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
                        sql = "select count(no) from recordout where dateout "
                        + " BETWEEN '" + startDateTime + "' AND '" + endDateTime + "' AND proid =" + dt.Rows[i]["id"];
                        dt2 = DbController.LoadData(sql);
                        dr["Data1"] = dt2.Rows[0].ItemArray[0].ToString();
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
                                        // Set before sent to function
                                        AppGlobalVariables.IntTime = new int[0];
                                        AppGlobalVariables.IntPriceMin = new int[0];
                                        AppGlobalVariables.IntPriceHour = new int[0];
                                        AppGlobalVariables.IntHourRound = new int[0];
                                        AppGlobalVariables.IntExpense = new int[0];
                                        AppGlobalVariables.IntOver = new int[0];
                                        sql19 = "select * from prosetprice where PromotionID = " + dt.Rows[i]["id"] + " ";

                                        if (stringDW.Length > 1)
                                            sql19 += " and dayweek like '%" + stringDW + "%'";

                                        sql19 += " order by no";

                                        DataTable dt4 = DbController.LoadData(sql19);

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

                                        int ZoneMin = 0;
                                        AppGlobalVariables.IntTime2 = new int[0];
                                        AppGlobalVariables.IntPriceMin2 = new int[0];
                                        AppGlobalVariables.IntPriceHour2 = new int[0];
                                        AppGlobalVariables.IntHourRound2 = new int[0];
                                        AppGlobalVariables.IntExpense2 = new int[0];
                                        AppGlobalVariables.IntOver2 = new int[0];

                                        sql19 = "select * from prosetprice_zone where PromotionID = " + dt.Rows[i]["id"] + " ";

                                        if (stringDW.Length > 1)
                                            sql19 += " and dayweek like '%" + stringDW + "%'";

                                        sql19 += " order by no";

                                        DataTable dt5 = DbController.LoadData(sql19);
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
                                        SumData0 += CalculationsManager.CalPrice(0, intMin, notDay);

                                        if (Configs.UseFlatRateProSetPrice)
                                        {
                                            SumData0 += CalculationsManager.CalFlatRate(DateTime.Parse(dt3.Rows[k]["datein"].ToString()), DateTime.Parse(dt3.Rows[k]["dateout"].ToString()), FlatRateM, FlatRateP, FlatRateX);
                                        }
                                    }
                                }
                            }

                            if (Configs.Reports.ReportProsetPriceDayWeek)
                                dr["SumData0"] = totalCreditDayWeek.ToString();
                            else
                                dr["SumData0"] = SumData0.ToString();
                        }
                        else dr["SumData0"] = "0";
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

                TrySetCrystalReportHeaders(reportDocument);

                if (MemberGroupMonthComboBox.SelectedIndex > 0)
                {
                    TextObject txtReportHeader = reportDocument.ReportDefinition.ReportObjects["text7"] as TextObject;
                    txtReportHeader.Text = MemberGroupMonthComboBox.Text;
                }

                reportDocument.SetDataSource(estampSumMap);

                PrimaryCrystalReportViewer.ReportSource = reportDocument;
                PrimaryCrystalReportViewer.Refresh();
            }
            catch (Exception) { }
        }

        private void รายงานสถิติการเข้าออกที่จอดรถ(ReportDocument reportDocument)
        {
            TrySetCrystalReportHeaders(reportDocument);

            string sql = "SELECT  COUNT(recordin.id) "
                  + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
                  + " WHERE recordin.cartype != 200 AND recordout.losscard = 0 AND"
                  + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "' ";

            DataTable dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("visiterin", dt.Rows[0].ItemArray[0].ToString());
            reportDocument.SetParameterValue("visiterout", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT  COUNT(recordin.id)  "
            + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
            + " WHERE recordin.cartype = 200 AND  (SELECT level FROM cardmf WHERE name = recordin.id) = 2 AND"
            + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("vipin", dt.Rows[0].ItemArray[0].ToString());
            reportDocument.SetParameterValue("vipout", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT  COUNT(recordin.id)  "
            + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
            + " WHERE recordin.cartype = 200 AND  (SELECT level FROM cardmf WHERE name = recordin.id) = 3 AND"
            + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("memberin", dt.Rows[0].ItemArray[0].ToString());
            reportDocument.SetParameterValue("memberout", dt.Rows[0].ItemArray[0].ToString());

            PrimaryCrystalReportViewer.ReportSource = reportDocument;
            PrimaryCrystalReportViewer.Refresh();
        }

        private void รายงานการเข้าออกประจำวัน(ReportDocument reportDocument)
        {
            string sql = "select recordin.no as no, recordin.license as license,  "
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
                 + " TRUNCATE(TIMESTAMPDIFF(minute, recordin.datein,recordout.dateout),0) as tdf,  "
                 + " recordout.proid as proid, promotion.name as proname,  recordout.price as price,  "
                 + "  recordout.printno as printno, recordin.userin as userin, (select name from user where id =  recordout.userout) as userout   "
                 + " from recordin join recordout on recordin.no = recordout.no "
                 + " left join promotion ON promotion.id = recordout.proid "
                 + "  WHERE recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "' ";
            if (UserComboBox.SelectedIndex > 0)
            {
                sql += " AND recordout.userout =" + AppGlobalVariables.UsersById.First(kvp => kvp.Value == UserComboBox.Text).Key;
            }
            if (LicensePlateTextBox.Text != "")
                sql += " AND recordin.license LIKE '%" + LicensePlateTextBox.Text + "%'";
            if (CardIdTextBox.Text != "")
                sql += " AND recordin.id = " + CardIdTextBox.Text;

            if (PromotionComboBox.SelectedIndex > 0)
            {
                sql += " AND recordout.proid =" + AppGlobalVariables.PromotionNamesById.First(kvp => kvp.Value == PromotionComboBox.Text).Key;
            }
            if (CarTypeComboBox.SelectedIndex > 1)
            {
                sql += " AND recordin.cartype =" + AppGlobalVariables.CarTypesById.First(kvp => kvp.Value == CarTypeComboBox.Text).Key;
            }
            if (CarTypeComboBox.SelectedIndex == 1)
            {
                sql += " AND recordin.cartype != 200";
            }

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

                    /* Cal from minute */
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
                            if (j == 0)
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

                            bool booNoRound = false;
                            for (int x = 0; x < diffInOut.Days + 1; x++)
                            {
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
                    }

                    dr["PayPrice"] = dtLoad.Rows[i]["price"];
                    dr["Printno"] = dtLoad.Rows[i]["printno"];
                    dr["userin"] = dtLoad.Rows[i]["userin"];
                    dr["userout"] = dtLoad.Rows[i]["userout"];
                    dtMap.Rows.Add(dr);
                }
            }

            TrySetCrystalReportHeaders(reportDocument);

            PrimaryCrystalReportViewer.ReportSource = reportDocument;
            PrimaryCrystalReportViewer.Refresh();

            PrimaryTabControl.SelectTab(1);
        }

        private void รายงานสรุปการเข้าออก(ReportDocument reportDocument)
        {
            string sql = "SELECT COUNT(recordin.no) "
                    + " FROM recordout JOIN recordin ON  recordin.no = recordout.no "
                    + " WHERE recordout.proid = 0 AND  recordin.cartype != 200 AND  recordout.price = 0 AND recordout.losscard = 0 "
                    + " AND recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'";
            DataTable dt = DbController.LoadData(sql);
            if (dt != null && dt.Rows.Count > 0)
                reportDocument.SetParameterValue("01", dt.Rows[0].ItemArray[0].ToString());
            else reportDocument.SetParameterValue("01", "'0'");

            sql = "SELECT COUNT(recordin.no) "
            + " FROM recordout JOIN recordin ON  recordin.no = recordout.no "
            + " WHERE recordout.proid = 0 AND  recordin.cartype != 200 AND  recordout.price > 0 AND recordout.losscard = 0 "
            + " AND recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "' ";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("02", dt.Rows[0].ItemArray[0].ToString());


            sql = "SELECT SUM(recordout.price) "
            + " FROM recordout JOIN recordin ON  recordin.no = recordout.no "
            + " WHERE recordout.proid = 0 AND  recordin.cartype != 200 AND  recordout.price > 0 AND recordout.losscard = 0 "
            + " AND recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "' ";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("04", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT COUNT(recordin.no) "
            + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
            + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
            + " recordout.price = 0 AND recordout.losscard = 0 AND recordout.proid NOT IN  "
            + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
            + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("05", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT COUNT(recordin.no) "
            + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
            + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
            + " recordout.price = 0 AND recordout.losscard = 0 AND recordout.proid  IN  "
            + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
            + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("06", dt.Rows[0].ItemArray[0].ToString());

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
            reportDocument.SetParameterValue("07", total.ToString());

            int total2 = 0;
            total = 0;
            try
            {
                sql = "SELECT value FROM param WHERE name = 'not_day'";
                dt = DbController.LoadData(sql);
                Boolean notDay = Convert.ToBoolean(dt.Rows[0].ItemArray[0].ToString());
                sql = "SELECT truncate(TIMESTAMPDIFF(minute,recordin.datein,recordout.dateout),0),"
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
                                if (i == 0)
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

                            total += CalculationsManager.CalPrice(intHour, intMin, notDay);
                            total2 += Convert.ToInt32(dt.Rows[j].ItemArray[2].ToString());
                        }
                    }
                }
            }
            catch (Exception) { }
            reportDocument.SetParameterValue("13", total.ToString());
            reportDocument.SetParameterValue("14", total2.ToString());

            sql = "SELECT COUNT(recordin.no) "
            + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
            + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
            + " recordout.price > 0 AND recordout.losscard = 0 AND recordout.proid NOT IN  "
            + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
            + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("09", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT SUM(recordout.price) "
            + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
            + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
            + " recordout.price > 0 AND recordout.losscard = 0 AND recordout.proid NOT IN  "
            + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
            + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("11", dt.Rows[0].ItemArray[0].ToString());


            sql = "SELECT COUNT(recordin.no) "
            + " FROM recordout JOIN recordin  ON  recordin.no = recordout.no  "
            + " WHERE recordout.proid > 0 AND recordin.cartype != 200 AND  "
            + " recordout.price > 0 AND recordout.losscard = 0 AND recordout.proid IN  "
            + " (SELECT PromotionId FROM prosetprice GROUP BY PromotionId) "
            + " AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("12", dt.Rows[0].ItemArray[0].ToString());
            if (dt.Rows[0].ItemArray[0].ToString().Trim() == "0")
                reportDocument.SetParameterValue("12", "0");

            sql = "SELECT COUNT(*) FROM cardmf";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("15", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT COUNT(*) FROM cardmf WHERE level > 1";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("16", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT COUNT(*) FROM cardmf WHERE level = 1";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("33", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT COUNT(*) FROM cardmf WHERE level = 0";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("17", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT COUNT(*) FROM liftrecord WHERE datelift BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("24", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT COUNT(t1.no)"
            + " FROM recordin t1 LEFT JOIN recordout t2 ON t1.no = t2.no"
            + " WHERE t1.datein <= '" + endDateTime + "'" //Mac 2016/02/01
            + " AND t2.no IS null"
            + " AND timediff(NOW(), t1.datein) > '12:00:00';";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("20", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT COUNT(t1.no)"
            + " FROM recordin t1 LEFT JOIN recordout t2 ON t1.no = t2.no"
            + " WHERE t1.datein <= '" + endDateTime + "'" //Mac 2016/02/01
            + " AND t2.no IS null"
            + " AND timediff(NOW(), t1.datein) > '12:00:00'"
            + " AND t1.cartype != 200;";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("23", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT COUNT(t1.no)"
            + " FROM recordin t1 LEFT JOIN recordout t2 ON t1.no = t2.no"
            + " WHERE t1.datein <= '" + endDateTime + "'" //Mac 2016/02/01
            + " AND t2.no IS null"
            + " AND timediff(NOW(), t1.datein) > '12:00:00'"
            + " AND t1.cartype = 200;";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("34", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT SUM(recordout.price) FROM recordout "
            + " WHERE losscard > 0 AND recordout.dateout BETWEEN '"
            + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            string losscard = dt.Rows[0].ItemArray[0].ToString();
            Console.WriteLine(losscard);
            reportDocument.SetParameterValue("19", "0");
            if (losscard.Trim() != "")
                reportDocument.SetParameterValue("19", losscard);

            sql = "SELECT COUNT(recordout.price) FROM recordout "
            + " WHERE losscard > 0 AND recordout.dateout BETWEEN '"
            + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            losscard = dt.Rows[0].ItemArray[0].ToString();
            Console.WriteLine(losscard);
            reportDocument.SetParameterValue("18", "0");
            if (losscard.Trim() != "")
                reportDocument.SetParameterValue("18", losscard);

            sql = "SELECT COUNT(printno) FROM recordout WHERE printno > 0 AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("25", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT MIN(printno) FROM recordout WHERE printno > 0 AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("26", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT MAX(printno) FROM recordout WHERE printno > 0 AND recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("27", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT  COUNT(recordin.id) "
            + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
            + " WHERE recordin.cartype != 200 AND recordout.losscard = 0 AND "
            + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("31", dt.Rows[0].ItemArray[0].ToString());
            reportDocument.SetParameterValue("32", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT  COUNT(recordin.id)  "
            + " FROM recordin JOIN recordout ON recordin.no = recordout.no "
            + " WHERE recordin.cartype = 200 AND "
            + " recordout.dateout BETWEEN '" + startDateTime + "' AND '" + endDateTime + "'";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("28", dt.Rows[0].ItemArray[0].ToString());
            reportDocument.SetParameterValue("29", dt.Rows[0].ItemArray[0].ToString());

            sql = "SELECT if(SUM(recordout.price) is null,0,SUM(recordout.price)) "
            + " FROM recordout JOIN recordin ON recordin.no = recordout.no "
            + " WHERE recordin.cartype = 200 AND  recordout.price > 0 AND recordout.losscard = 0 "
            + " AND recordout.dateout BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "' ";
            dt = DbController.LoadData(sql);
            reportDocument.SetParameterValue("35", dt.Rows[0].ItemArray[0].ToString());

            try
            {
                sql = "SELECT COUNT(*)"
                + " FROM liftrecord"
                + " WHERE datelift BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                + " AND confirm1 IS null;";
                dt = DbController.LoadData(sql);
                if (dt.Rows[0].ItemArray[0].ToString() == "0")
                    reportDocument.SetParameterValue("Verified1", "Verified 1");
            }
            catch
            {
                reportDocument.SetParameterValue("Verified1", "");
            }

            try
            {
                sql = "SELECT COUNT(*)"
                + " FROM liftrecord"
                + " WHERE datelift BETWEEN '" + startDateTime + "'  AND '" + endDateTime + "'  "
                + " AND confirm2 IS null;";
                dt = DbController.LoadData(sql);
                if (dt.Rows[0].ItemArray[0].ToString() == "0")
                    reportDocument.SetParameterValue("Verified2", "Verified 2");
            }
            catch
            {
                reportDocument.SetParameterValue("Verified2", "");
            }

            TrySetCrystalReportHeaders(reportDocument);

            PrimaryCrystalReportViewer.ReportSource = reportDocument;
            PrimaryCrystalReportViewer.Refresh();

            PrimaryTabControl.SelectTab(1);
        }
        #endregion
    }
}