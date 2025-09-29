using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using ParkingManagementReport.Common;
using ParkingManagementReport.Utilities.Database;
using ParkingManagementReport.Utilities.Formatters;

namespace ParkingManagementReport.Utilities
{
    internal class ConfigsManager
    {
        #region Fields
        public static readonly string ConfigPath = "C:\\Windows\\carpark";
        public static readonly string ConfigXmlFileName = "Setting.xml";
        public static readonly string ConfigXmlFilePath = $"{ConfigPath}\\{ConfigXmlFileName}";

        static DataTable dt;
        static string sql;
        #endregion

        #region Database
        public static void LoadConfigsFromDb()
        {
            dt = new DataTable();
            AppGlobalVariables.PromotionNamesMinuteMap = new Dictionary<int, int>();
            AppGlobalVariables.CarTypesById = new Dictionary<int, string>();
            AppGlobalVariables.DispensersById = new Dictionary<int, string>();
            AppGlobalVariables.PromotionNamesById = new Dictionary<int, string>();
            AppGlobalVariables.VendorGroupMonthsById = new Dictionary<int, string>();
            AppGlobalVariables.UsersById = new Dictionary<int, string>();
            AppGlobalVariables.ReportsById = new Dictionary<int, string>();
            AppGlobalVariables.ParamsLookup = new Dictionary<string, string>();
            AppGlobalVariables.MemberStatusesLookup = new Dictionary<string, string>();
            AppGlobalVariables.MemberGroupsToId = new Dictionary<string, int>();
            AppGlobalVariables.MemberGroupMonthsToId = new Dictionary<string, int>();
            AppGlobalVariables.RenewMemberGroupsToId = new Dictionary<string, int>();

            sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'promotion' AND COLUMN_NAME = 'active'"; //Mac 2016/11/10
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
                Configs.UseActivePromotion = true;

            sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = '" + AppGlobalVariables.Database.Name + "' and table_name = 'recordoutvoidslip'";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
                Configs.UseFormVoidSlip = true;

            sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'cardpx' AND COLUMN_NAME = 'name_on_card'";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'cardmf' AND COLUMN_NAME = 'name_on_card'";
                dt = DbController.LoadData(sql);
                if (dt.Rows.Count > 0)
                    Configs.UseNameOnCard = true;
            }

            sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'member' AND COLUMN_NAME = 'hour_balance'";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
                Configs.UseMemberHourBalance = true;

            sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = '" + AppGlobalVariables.Database.Name + "' and table_name = 'holiday'";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
                Configs.UseHoliday = true;

            sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'prosetprice' AND COLUMN_NAME = 'flat_rate'";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
                Configs.UseFlatRateProSetPrice = true;

            LoadDispensers();

            SetSlipOutFormat();

            SetSlipVoidPayFormat();

            SetUseVoidSlip();

            SetUseMemberType();
           
            SetOnlinePaymentType();

            SetParkingFreeMinutes();

        }

        public static void SaveConfigsToDb()
        {

        }
        #endregion

        #region XML
        public static void LoadConfigsFromXml()
        {
            try
            {
                if (!Directory.Exists(ConfigPath))
                {
                    Directory.CreateDirectory(ConfigPath);
                }
                if (!File.Exists(ConfigXmlFilePath))
                {
                    File.Copy(ConfigXmlFileName, ConfigXmlFilePath);
                }
            }
            catch (Exception)
            {

            }

            XmlTextReader reader = new XmlTextReader(ConfigXmlFilePath);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Mode")
                {
                    Configs.Mode = reader.ReadElementString();
                    if (Configs.Mode == "DIn")
                    {
                        Configs.Hardwares.ModeDispenser = true;
                        Configs.Mode = "In";
                    }
                    if (Configs.Mode == "DOut")
                    {
                        Configs.Hardwares.ModeDispenser = true;
                        Configs.Mode = "Out";
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "UseCarType")
                {
                    Configs.UseCarType = Convert.ToBoolean(reader.ReadElementString());
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Use2Camera")
                {
                    Configs.Use2Camera = Convert.ToBoolean(reader.ReadElementString());
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "ServerIP")
                {
                    Configs.ServerIP = reader.ReadElementString();

                    try
                    {
                        AppGlobalVariables.Database.Name = Configs.ServerIP.Split('|')[1];
                        if (AppGlobalVariables.Database.Name.Trim().Length == 0)
                            AppGlobalVariables.Database.Name = "carpark2";
                    }
                    catch { AppGlobalVariables.Database.Name = "carpark2"; }

                    Configs.ServerIP = Configs.ServerIP.Split('|')[0];  //Mac 2016/11/12
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "ServerDirectory")
                {
                    Configs.Paths.ServerDirectory = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "BackupDirectory")
                {
                    Configs.Paths.BackupDirectory = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPIn1")
                {
                    Configs.IPIn1 = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPIn2")
                {
                    Configs.IPIn2 = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPIn3") //Mac 2015/02/04
                {
                    Configs.IPIn3 = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPOut1")
                {
                    Configs.IPOut1 = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "IPOut2")
                {
                    Configs.IPOut2 = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortControl")
                {
                    Configs.Hardwares.PortControl = reader.ReadElementString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortProxIn")
                {
                    Configs.Hardwares.PortProxIn = reader.ReadElementString();
                    if (Configs.Hardwares.PortProxIn.IndexOf("B") >= 0)
                    {
                        Configs.Hardwares.UseBluetoothReaderIn = true;
                        Configs.Hardwares.PortProxIn = Configs.Hardwares.PortProxIn.Replace("B", "");
                    }
                    else if (Configs.Hardwares.PortProxIn.Substring(0, 2) == "MF")
                    {
                        Configs.Hardwares.IsMFPassiveInProx = true;
                        Configs.Hardwares.UseMFReaderIn = true;
                        Configs.Hardwares.PortProxIn = Configs.Hardwares.PortProxIn.Replace("MF", "");
                    }
                    else if (Configs.Hardwares.PortProxIn.Substring(0, 2) == "PX")
                    {
                        Configs.Hardwares.UsePXReaderIn = true;
                        Configs.Hardwares.PortProxIn = Configs.Hardwares.PortProxIn.Replace("PX", "");
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortProxOut")
                {
                    Configs.Hardwares.PortProxOut = reader.ReadElementString();
                    if (Configs.Hardwares.PortProxOut.IndexOf("B") >= 0)
                    {
                        Configs.Hardwares.UseBluetoothReaderOut = true;
                        Configs.Hardwares.PortProxOut = Configs.Hardwares.PortProxOut.Replace("B", "");
                    }
                    else if (Configs.Hardwares.PortProxOut.Substring(0, 2) == "MF")
                    {
                        Configs.Hardwares.IsMFPassiveInProx = true;
                        Configs.Hardwares.UsePXReaderOut = true;
                        Configs.Hardwares.PortProxOut = Configs.Hardwares.PortProxOut.Replace("MF", "");
                    }
                    else if (Configs.Hardwares.PortProxOut.Substring(0, 2) == "PX")
                    {
                        Configs.Hardwares.UsePXReaderOut = true;
                        Configs.Hardwares.PortProxOut = Configs.Hardwares.PortProxOut.Replace("PX", "");
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortMifare")
                {
                    Configs.Hardwares.PortMifare = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortMifareIn")
                {
                    Configs.Hardwares.PortMifareIn = reader.ReadElementString();
                    if (Configs.Hardwares.PortMifareIn.IndexOf("F") >= 0)
                    {
                        Configs.Hardwares.UseMFReaderIn = true;
                        Configs.Hardwares.PortMifareIn = Configs.Hardwares.PortMifareIn.Replace("F", "");
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "PortMifareOut")
                {
                    Configs.Hardwares.PortMifareOut = reader.ReadElementString();
                    if (Configs.Hardwares.PortMifareOut.IndexOf("F") >= 0)
                    {
                        Configs.Hardwares.UseMFReaderOut = true;
                        Configs.Hardwares.PortMifareOut = Configs.Hardwares.PortMifareOut.Replace("F", "");
                    }
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "UseOfflineMode")
                {
                    Configs.UseOfflineMode = Convert.ToBoolean(reader.ReadElementString());
                    Configs.IsOffline = Configs.UseOfflineMode;
                }

            }
            reader.Close();
        }

        public static void SaveConfigsToXml()
        {
            XmlWriter xmlWriter = XmlWriter.Create(ConfigXmlFilePath);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("ParkingManager");
            xmlWriter.WriteStartElement("Mode");
            xmlWriter.WriteString(Configs.Mode);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("UseCarType");
            xmlWriter.WriteString(Configs.UseCarType.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Use2Camera");
            xmlWriter.WriteString(Configs.Use2Camera.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("ServerIP");
            xmlWriter.WriteString(Configs.ServerIP);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("ServerDirectory");
            xmlWriter.WriteString(Configs.Paths.ServerDirectory);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("BackupDirectory");
            xmlWriter.WriteString(Configs.Paths.BackupDirectory);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPIn1");
            xmlWriter.WriteString(Configs.IPIn1);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPIn2");
            xmlWriter.WriteString(Configs.IPIn2);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPIn3"); //Mac 2015/02/04
            xmlWriter.WriteString(Configs.IPIn3);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPOut1");
            xmlWriter.WriteString(Configs.IPOut1);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("IPOut2");
            xmlWriter.WriteString(Configs.IPOut2);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("PortControl");
            xmlWriter.WriteString(Configs.Hardwares.PortControl);
            xmlWriter.WriteEndElement();

            if (Configs.Hardwares.UseBluetoothReaderIn)
                Configs.Hardwares.PortProxIn = "B" + Configs.Hardwares.PortProxIn;
            else if (Configs.Hardwares.UseMFReaderIn)
                Configs.Hardwares.PortProxIn = "MF" + Configs.Hardwares.PortProxIn;
            else if (Configs.Hardwares.UsePXReaderIn)
                Configs.Hardwares.PortProxIn = "PX" + Configs.Hardwares.PortProxIn;
            xmlWriter.WriteStartElement("PortProxIn");
            xmlWriter.WriteString(Configs.Hardwares.PortProxIn);
            xmlWriter.WriteEndElement();
            if (Configs.Hardwares.UseBluetoothReaderOut)
                Configs.Hardwares.PortProxOut = "B" + Configs.Hardwares.PortProxOut;
            else if (Configs.Hardwares.UseMFReaderOut)
                Configs.Hardwares.PortProxOut = "MF" + Configs.Hardwares.PortProxOut;
            else if (Configs.Hardwares.UsePXReaderOut)
                Configs.Hardwares.PortProxOut = "PX" + Configs.Hardwares.PortProxOut;
            xmlWriter.WriteStartElement("PortProxOut");
            xmlWriter.WriteString(Configs.Hardwares.PortProxOut);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("PortMifare");
            xmlWriter.WriteString(Configs.Hardwares.PortMifare);
            xmlWriter.WriteEndElement();
            if (Configs.Hardwares.UseMFReaderIn)
                Configs.Hardwares.PortMifareIn = "F" + Configs.Hardwares.PortMifareIn;
            xmlWriter.WriteStartElement("PortMifareIn");
            xmlWriter.WriteString(Configs.Hardwares.PortMifareIn);
            xmlWriter.WriteEndElement();
            if (Configs.Hardwares.UseMFReaderOut)
                Configs.Hardwares.PortMifareOut = "F" + Configs.Hardwares.PortMifareOut;
            xmlWriter.WriteStartElement("PortMifareOut");
            xmlWriter.WriteString(Configs.Hardwares.PortMifareOut);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("UseOfflineMode");
            xmlWriter.WriteString(Configs.UseOfflineMode.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }
        #endregion

        public static void LoadComboBoxDataFromQuery(ComboBox comboBox, string query, Dictionary<string, int> dictionary)
        {
            if (dictionary != null && !dictionary.ContainsKey(Constants.TextBased.All))
                dictionary.Add(Constants.TextBased.All, 0);

            if (comboBox != null)
            {
                if (!comboBox.Items.Contains(Constants.TextBased.All))
                    comboBox.Items.Add(Constants.TextBased.All);

                comboBox.Text = Constants.TextBased.All;
            }

            var dt = DbController.LoadData(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string displayText = row[0].ToString();

                    if (dictionary != null && !dictionary.ContainsKey(displayText))
                    {
                        int id = Convert.ToInt32(row[1]);
                        dictionary.Add(displayText, id);
                    }

                    if (comboBox != null && !comboBox.Items.Contains(displayText))
                    {
                        comboBox.Items.Add(displayText);
                    }
                }
            }

        }

        public static void LoadDataToIntStringDictionary(string query, Dictionary<int, string> dictionary)
        {
            DataTable dt = DbController.LoadData(query);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int currentKey = Convert.ToInt16(row[0]);

                    if (dictionary != null && !dictionary.ContainsKey(currentKey))
                    {
                        string value = row[1]?.ToString();
                        dictionary.Add(currentKey, value);
                    }
                }
            }
        }

        public static void LoadDispensers()
        {
            if (Configs.UseGroupPrice)
            {
                try
                {
                    DataTable dt = DbController.LoadData("SELECT * FROM dispenser");
                    if (dt?.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            int dispenserId = Convert.ToInt32(row[0]);
                            string dispenserName = row[1].ToString();
                            AppGlobalVariables.DispensersById.Add(dispenserId, dispenserName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(TextFormatters.ErrorStacktraceFromException(ex), "LoadDispensers");
                }
            }
        }

        private static void SetOnlinePaymentType()
        {
            int paymentMethodCount = 0;

            sql = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'rabbit_post'";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                Configs.UsePaymentRabbit = true;
                paymentMethodCount++;
            }

            sql = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'ksherpay_post'";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                Configs.UsePaymentKsher = true;
                paymentMethodCount++;
            }

            sql = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'beam_post'";
            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                Configs.UsePaymentBeam = true;
                paymentMethodCount++;
            }

            if (paymentMethodCount > 1)
            {
                string caption = "Online Payment Conflict";

                MessageBox.Show(
                    "มีการเปิดใช้งานวิธีชำระเงินมากกว่า 1 รายการ กรุณาตรวจสอบ table:\r\n" +
                    "-beam_post\r\n" +
                    "-ksherpay_post\r\n" +
                    "-rabbit_post",
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        public static void SetUseMemberType()
        {
            sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'member' AND COLUMN_NAME = 'typeid'";
            DataTable dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
                Configs.UseMemberType = true;
        }

        public static void SetParkingFreeMinutes()
        {
            sql = "SELECT `round` FROM `pricerate0` ORDER BY `no` ASC LIMIT 1";
            DataTable dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                if (int.TryParse(dt.Rows[0]["round"].ToString(), out int freeMinutes))
                    Configs.ParkingFreeMinutes = freeMinutes;
                else
                    Configs.ParkingFreeMinutes = 0;
            }
            else
            {
                Configs.ParkingFreeMinutes = 0;
            }
        }

        public static void SetUseVoidSlip()
        {
            sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + AppGlobalVariables.Database.Name + "' AND TABLE_NAME = 'recordout' AND COLUMN_NAME = 'status'";
            dt = DbController.LoadData(sql);

            if (dt.Rows.Count > 0)
                Configs.UseVoidSlip = true;
            else
                Configs.UseVoidSlip = false;
        }

        public static void SetSlipVoidPayFormat()
        {
            sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = '" + AppGlobalVariables.Database.Name + "' and table_name = 'slipvoidpayformat'";

            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "SELECT value FROM slipvoidpayformat WHERE name = 'receiptname' AND used = 'True'";
                dt = DbController.LoadData(sql);
                if (dt.Rows.Count > 0)
                {
                    AppGlobalVariables.Printings.ReceiptNameVoidPay = dt.Rows[0][0].ToString();
                }
            }
        }

        public static void SetSlipOutFormat()
        {
            sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = '" + AppGlobalVariables.Database.Name + "' and table_name = 'slipoutformat'";

            dt = DbController.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "SELECT value FROM slipoutformat WHERE name = 'receiptname' AND used = 'True'";
                dt = DbController.LoadData(sql);
                if (dt.Rows.Count > 0)
                {
                    AppGlobalVariables.Printings.ReceiptName = dt.Rows[0][0].ToString();
                    Configs.UseReceiptName = true;
                }
            }
        }


        public static Dictionary<string, string> LoadParametersFromDatabase()
        {
            var kvpLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                DataTable dt = DbController.LoadData("SELECT name, value FROM param");
                foreach (DataRow row in dt.Rows)
                {
                    kvpLookup[row[0].ToString()] = row[1].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed loading parameters: {ex.Message}");
            }

            return kvpLookup;
        }

        public static void SetConfigsParamsFromLookupData(Dictionary<string, string> paramsLookup)
        {
            // Database schema checks
            CheckDatabaseSchemaFeatures();

            // System configuration
            SetBoolConfig(paramsLookup, "print_officer", value => Configs.UsePrintOfficer = !value);
            SetBoolConfig(paramsLookup, "print_barcode", value => Configs.UsePrintBarcode = !value);
            SetBoolConfig(paramsLookup, "print_qrcode", value => Configs.UsePrintQRCode = !value);
            SetBoolConfig(paramsLookup, "use_lastpro", value => Configs.UseProIDAll = !value);
            SetBoolConfig(paramsLookup, "member_2cartype", value => Configs.Member2Cartype = value);
            SetBoolConfig(paramsLookup, "use_lastpro", value => Configs.UseLastPromotion = value);
            SetBoolConfig(paramsLookup, "use_memgrouppricemonth", value => Configs.UseMemberGroupPriceMonth = value);
            SetBoolConfig(paramsLookup, "use_grouppromotion", value => Configs.UseGroupPromotion = value);
            SetBoolConfig(paramsLookup, "use_ascii_mem", value => Configs.UseAsciiMember = value);
            SetBoolConfig(paramsLookup, "visitor_filldetail", value => Configs.VisitorFillDetail = value);
            SetBoolConfig(paramsLookup, "outreceiptname_month", value => Configs.OutReceiptNameMonth = value);
            SetBoolConfig(paramsLookup, "not_day", value => Configs.UseNotDay = value);
            SetBoolConfig(paramsLookup, "not_slipprice", value => Configs.UseSlipRecord = value);
            SetBoolConfig(paramsLookup, "asiatriq_price", value => Configs.UseAsiaTriqPrice = value);
            SetBoolConfig(paramsLookup, "group_price", value => Configs.UseGroupPrice = value);
            SetBoolConfig(paramsLookup, "sum_price", value => Configs.UseSumPrice = value);
            SetBoolConfig(paramsLookup, "printin", value => Configs.IsPrintCarIn = value);
            SetBoolConfig(paramsLookup, "cardloss_price", value => Configs.UseCardLossPrice = value);
            SetBoolConfig(paramsLookup, "use_calvat_from_total", value => Configs.UseCalVatFromTotal = value);
            SetBoolConfig(paramsLookup, "use_memlicenseplate", value => Configs.UseMemberLicensePlate = value);
            SetBoolConfig(paramsLookup, "use_receiptfor1out", value => Configs.UseReceiptFor1Out = value); 
            SetBoolConfig(paramsLookup, "use_receiptfor1mem", value => Configs.UseReceiptFor1Mem = value); 
            SetBoolConfig(paramsLookup, "use_setting_newmem", value => Configs.UseSettingNewMember = value);
            SetBoolConfig(paramsLookup, "use_qrcodenew", value => Configs.UseQRCodeNew = value);
            if (Configs.UseQRCodeNew)
                Configs.UseSettingNewMember = true;

            SetIntConfig(paramsLookup, "no_car", value => Configs.NoCar = value);
            SetIntConfig(paramsLookup, "cardloss", value => Configs.PriceCardLoss = value);
            SetIntConfig(paramsLookup, "carin_repeat", value => Configs.CarInRepeat = value);
            SetStringConfig(paramsLookup, "day_of_week", value => Configs.UseDayWeek = value);
            SetStringConfig(paramsLookup, "discount", value => Configs.Discount = value);
            SetStringConfig(paramsLookup, "no_panelUp2U", value => Configs.NoPanelUp2U = value);
            SetStringConfig(paramsLookup, "not_showno", value => Configs.NotShowNoString = value);

            // Report configurations (grouped by category)
            ConfigureReportSettings(paramsLookup);

            // Printing settings
            SetStringConfig(paramsLookup, "com1", value => AppGlobalVariables.Printings.Company1 = value);
            SetStringConfig(paramsLookup, "com2", value => AppGlobalVariables.Printings.Company2 = value);
            SetStringConfig(paramsLookup, "add1", value => AppGlobalVariables.Printings.Address1 = value);
            SetStringConfig(paramsLookup, "add2", value => AppGlobalVariables.Printings.Address2 = value);
            SetStringConfig(paramsLookup, "tax", value => AppGlobalVariables.Printings.Tax1 = value);
            SetStringConfig(paramsLookup, "tel", value => AppGlobalVariables.Printings.Telephone = value);
            SetStringConfig(paramsLookup, "building", value => AppGlobalVariables.Printings.Building = value);
            SetStringConfig(paramsLookup, "office", value => AppGlobalVariables.Printings.Office = value);
            SetStringConfig(paramsLookup, "footerreport1", value => AppGlobalVariables.Printings.ReportFooter1 = value);
            SetStringConfig(paramsLookup, "footerreport2", value => AppGlobalVariables.Printings.ReportFooter2 = value);
            SetStringConfig(paramsLookup, "footerreport3", value => AppGlobalVariables.Printings.ReportFooter3 = value);
            SetStringConfig(paramsLookup, "footerreport4", value => AppGlobalVariables.Printings.ReportFooter4 = value);
            SetStringConfig(paramsLookup, "footerreport5", value => AppGlobalVariables.Printings.ReportFooter5 = value);

            // Initialize printing format
            InitializePrintingFormat();
        }


        private static void SetBoolConfig(Dictionary<string, string> lookup, string key, Action<bool> setter, bool defaultValue = false)
        {
            if (lookup.TryGetValue(key, out string value))
            {
                try
                {
                    setter(Convert.ToBoolean(value));
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Invalid boolean value for {key}: {value}");
                    setter(defaultValue);
                }
            }
        }

        private static void SetStringConfig(Dictionary<string, string> lookup, string key, Action<string> setter, string defaultValue = "")
        {
            setter(lookup.TryGetValue(key, out string value) ? value : defaultValue);
        }

        private static void SetIntConfig(Dictionary<string, string> lookup, string key, Action<int> setter, int defaultValue = 0)
        {
            if (lookup.TryGetValue(key, out string value))
            {
                try
                {
                    setter(Convert.ToInt32(value));
                }
                catch
                {
                    Logger.Warn($"Invalid integer value for {key}: {value}");
                    setter(defaultValue);
                }
            }
        }

        private static void ConfigureReportSettings(Dictionary<string, string> lookup)
        {
            // Report boolean flags configuration
            var reportSettings = new Dictionary<string, Action<bool>>
            {
                // Report usage flags
                ["report_search_memgroup"] = v => Configs.Reports.ReportSearchMemberGroup = v,
                ["use_report5_1"] = v => Configs.Reports.UseReport5_1 = v,
                ["use_report1_3"] = v => Configs.Reports.UseReport1_3 = v,
                ["use_report13_3"] = v => Configs.Reports.UseReport13_3 = v,
                ["use_report13_10"] = v => Configs.Reports.UseReport13_10 = v,
                ["use_report3_1"] = v => Configs.Reports.UseReport3_1 = v,
                ["use_report13_11"] = v => Configs.Reports.UseReport13_11 = v,
                ["use_report1_4"] = v => Configs.Reports.UseReport1_4 = v,
                ["use_report5_2"] = v => Configs.Reports.UseReport5_2 = v,
                ["use_report14like13"] = v => Configs.Reports.UseReport14like13 = v,
                ["use_report1_5"] = v => Configs.Reports.UseReport1_5 = v,
                ["use_report5_3"] = v => Configs.Reports.UseReport5_3 = v,
                ["use_report21_1"] = v => Configs.Reports.UseReport21_1 = v,
                ["use_report72_1"] = v => Configs.Reports.UseReport72_1 = v,
                ["use_report71_1"] = v => Configs.Reports.UseReport71_1 = v,
                ["use_report5_4"] = v => Configs.Reports.UseReport5_4 = v,
                ["use_report1_6"] = v => Configs.Reports.UseReport1_6 = v,
                ["use_report13_7"] = v => Configs.Reports.UseReport13_7 = v,
                ["use_report6"] = v => Configs.Reports.UseReport6 = v,
                ["use_report23_1"] = v => Configs.Reports.UseReport23_1 = v,
                ["use_report1_7"] = v => Configs.Reports.UseReport1_7 = v,
                ["use_report24_1"] = v => Configs.Reports.UseReport24_1 = v,
                ["use_report108_110_1"] = v => Configs.Reports.UseReport108_110_1 = v,
                ["use_report24_2"] = v => Configs.Reports.UseReport24_2 = v,
                ["use_report49_1"] = v => Configs.Reports.UseReport49_1 = v,
                ["use_report35_1"] = v => Configs.Reports.UseReport35_1 = v,
                ["use_report36_1"] = v => Configs.Reports.UseReport36_1 = v,
                ["use_report24_3"] = v => Configs.Reports.UseReport24_3 = v,
                ["use_report1_8"] = v => Configs.Reports.UseReport1_8 = v,
                ["use_report21_2"] = v => Configs.Reports.UseReport21_2 = v,
                ["use_report21_3"] = v => Configs.Reports.UseReport21_3 = v,
                ["use_report13_12"] = v => Configs.Reports.UseReport13_12 = v,
                ["use_report13_13"] = v => Configs.Reports.UseReport13_13 = v,
                ["use_report2_4"] = v => Configs.Reports.UseReport2_4 = v,

                // Report behavior flags
                ["report3decimal"] = v => Configs.Reports.Report3Decimal = v,
                ["report_norunning"] = v => Configs.Reports.ReportNoRunning = v,
                ["report_cartypefree15min"] = v => Configs.Reports.ReportCartypeFree15Min = v,
                ["report_pricesplit_losscard"] = v => Configs.Reports.ReportPriceSplitLosscard = v,
                ["report21_22_nocreditnoshow"] = v => Configs.Reports.Report21_22_NoCreditNoShow = v,
                ["report_prosetprice_dayweek"] = v => Configs.Reports.ReportProsetPriceDayWeek = v,
                ["report21_1_switch"] = v => Configs.Reports.Report21_1_Switch = v,
                ["report13pro_switchprice_not0"] = v => Configs.Reports.Report13Pro_SwitchPriceNot0 = v,
                ["report49_losscard_novat"] = v => Configs.Reports.Report49_LossCard_NoVat = v,
                ["report_pdfonly"] = v => Configs.UsePDFOnly = v,
                ["report_datestring"] = v => Configs.Reports.UseReportDateString = v,
                ["use_report_hour_use"] = v => Configs.Reports.UseReportHourUse = v,
                
                // Report logo flags
                ["use_report1logo"] = v => Configs.Reports.UseReport1logo = v,
                ["use_report2logo"] = v => Configs.Reports.UseReport2logo = v,
                ["use_report3logo"] = v => Configs.Reports.UseReport3logo = v,
                ["use_report4logo"] = v => Configs.Reports.UseReport4logo = v,
                ["use_report5logo"] = v => Configs.Reports.UseReport5logo = v,
                ["use_report8logo"] = v => Configs.Reports.UseReport8logo = v,
                ["use_report13logo"] = v => Configs.Reports.UseReport13logo = v,
                ["use_report13_1logo"] = v => Configs.Reports.UseReport13_1logo = v,
                ["use_report16logo"] = v => Configs.Reports.UseReport16logo = v,
                ["use_report32logo"] = v => Configs.Reports.UseReport32logo = v,
                ["use_report50logo"] = v => Configs.Reports.UseReport50logo = v
            };

            // Apply all report settings
            foreach (var setting in reportSettings)
            {
                SetBoolConfig(lookup, setting.Key, setting.Value);
            }

            // Special case: noshow_selecttime (array)
            if (lookup.TryGetValue("report_noshow_selecttime", out string noshowTimes))
            {
                try
                {
                    Configs.NoshowSelectTime = noshowTimes.Split(',');
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Failed to parse report_noshow_selecttime: {ex.Message}");
                    Configs.NoshowSelectTime = Array.Empty<string>();
                }
            }
        }

        private static void CheckDatabaseSchemaFeatures()
        {
            try
            {
                // Check for proid_all column
                var dt = DbController.LoadData($@"
                    SELECT COLUMN_NAME 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = '{AppGlobalVariables.Database.Name}' 
                    AND TABLE_NAME = 'recordout' 
                    AND COLUMN_NAME = 'proid_all'"
                );

                Configs.UseProIDAll = dt.Rows.Count > 0 && !Configs.UseLastPromotion;

                // Check for status column
                dt = DbController.LoadData($@"
                    SELECT COLUMN_NAME 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'carpark2' 
                    AND TABLE_NAME = 'recordout' 
                    AND COLUMN_NAME = 'status'"
                );

                Configs.UseVoidSlip = dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Logger.Error($"Schema check failed: {ex.Message}");
            }
        }

        private static void InitializePrintingFormat()
        {
            AppGlobalVariables.Printings.PrintingFixedFormat = string.Join(Environment.NewLine,
                TextCenter(AppGlobalVariables.Printings.Company1),
                TextCenter(AppGlobalVariables.Printings.Address1),
                TextCenter(AppGlobalVariables.Printings.Address2),
                TextCenter(AppGlobalVariables.Printings.Telephone),
                TextCenter(AppGlobalVariables.Printings.Tax1),
                "",
                Configs.UseSlipRecord
                    ? TextCenter("                               ใบบันทึกเวลา")
                    : TextCenter("               OFFICIAL RECEIPT / TAX INVOICE") +
                      Environment.NewLine +
                      TextCenter("               ใบเสร็จรับเงิน / ใบกำกับภาษีอย่างย่อ")
            );
        }
        #region Helpers

        private static void LoadParam(bool Offline)
        {

        }

        private static void UpdateParam()
        {
            UpdateParamToDb("printin", Configs.IsPrintCarIn.ToString());
            UpdateParamToDb("com1", AppGlobalVariables.Printings.Company1.ToString());
            UpdateParamToDb("add1", AppGlobalVariables.Printings.Address1.ToString());
            UpdateParamToDb("add2", AppGlobalVariables.Printings.Address1.ToString());
            UpdateParamToDb("tax", AppGlobalVariables.Printings.Tax1.ToString());
            UpdateParamToDb("tel", AppGlobalVariables.Printings.Telephone.ToString());
            UpdateParamToDb("print_barcode", Configs.UsePrintBarcode.ToString());
            UpdateParamToDb("print_officer", Configs.UsePrintOfficer.ToString());
        }

        private static void UpdateParamToDb(string strpname, string strpvalue)
        {
            string sql = "UPDATE param SET value ='" + strpvalue;
            sql += "' WHERE name='" + strpname + "'";
            DbController.SaveData(sql);
        }
        #endregion

        private static string TextCenter(String s)
        {
            string txt = s + "\r\n";

            return txt;
        }
    }
}

/*public static Dictionary<string, string> LoadParametersFromDatabase()
{
    Dictionary<string, string> kvpLookup = new Dictionary<string, string>();

    try
    {
        DataTable dt = DbController.LoadData("SELECT * FROM param");
        if (dt?.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string paramName = row[0].ToString();
                string paramValue = row[1].ToString();

                kvpLookup.Add(paramName, paramValue);
            }
        }

    }
    catch (Exception ex)
    {
        Logger.Error(TextFormatters.ErrorStacktraceFromException(ex), "LoadParametersFromDatabase");
    }
    return kvpLookup;
}


private static string ReadParam(string strParam)
{
    string pareamRead;
    string sql = "SELECT * FROM param WHERE name='" + strParam + "'";
    DataTable dt = new DataTable();
    if (Configs.IsOffline)
        dt = DbController.LoadData(sql, true);
    else
        dt = DbController.LoadData(sql);

    pareamRead = dt.Rows[0]?.ItemArray[1]?.ToString();

    return pareamRead;
}
*/
