using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ParkingManagementReport.Common;
using ParkingManagementReport.Utilities.Database;
using ParkingManagementReport.Utilities;
using ParkingManagementReport.Utilities.Hardwares;

namespace ParkingManagementReport
{
    public partial class FormLogin : Form
    {
        MifareReader mfReader;

        public FormLogin()
        {
            InitializeComponent();

            mfReader = new MifareReader(false);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region WINDOW_HANDLER
        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            if (Configs.UseMifare)
            {
                MifareCheckTimer.Enabled = true;
            }

            UsernameTextBox.Focus();
            this.KeyPreview = true;
            if (!File.Exists(@"C:\Windows\carpark\conDatabase.txt"))
            {
                this.Height = 211;
            }
            else
            {
                AppGlobalVariables.Database.LookupList = new System.Collections.Generic.Dictionary<string, string>();
                string strFile = @"C:\Windows\carpark\conDatabase.txt";
                FileStream MyFileStream = new FileStream(strFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader streamReader = new StreamReader(MyFileStream, Encoding.UTF8, true);
                String line = "";
                string[] str;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Trim().Length > 0)
                    {
                        str = line.Split(',');
                        AppGlobalVariables.Database.LookupList.Add(str[1], str[0]);
                        cobDatabase.Items.Add(str[1]);
                    }
                }
                streamReader.Close();
                MyFileStream.Close();
                if (cobDatabase.Items.Count > 0)
                    cobDatabase.SelectedIndex = 0;
            }
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AppGlobalVariables.OperatingUser.LoginReady)
            {
                if (MessageBox.Show("ต้องการออกจากโปรแกรม", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    AppGlobalVariables.OperatingUser.LoginReady = true;
                    Environment.Exit(0);
                }
            }
        }
        #endregion WINDOW_HANDLER_END



        #region UI_EVENT_HANDLER
        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (UsernameTextBox.Text.Length >= 25)
                {
                    AccessManager.Login("", "", UsernameTextBox.Text, false);
                    if (AppGlobalVariables.OperatingUser.LoginReady)
                    {
                        Close();
                    }
                    UsernameTextBox.Text = "";
                }
                else
                    HandleLogin();
            }
            else
            {
                String strKey = e.KeyCode.ToString();
                if (strKey.IndexOf("D") >= 0)
                {
                    try
                    {
                        UsernameTextBox.Text += strKey.Substring(1, 1);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            HandleLogin();
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                UserPasswordTextBox.Focus();
        }

        private void UserPasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (cobDatabase.Text.Length > 0)
                {
                    Configs.ServerIP = AppGlobalVariables.Database.LookupList[cobDatabase.Text].Split('|')[0];
                    try
                    {
                        AppGlobalVariables.Database.Name = AppGlobalVariables.Database.LookupList[cobDatabase.Text].Split('|')[1];
                        if (AppGlobalVariables.Database.Name.Trim().Length == 0)
                            AppGlobalVariables.Database.Name = "carpark2";
                    }
                    catch { AppGlobalVariables.Database.Name = "carpark2"; }
                }
                if (!DbController.Connect(Configs.ServerIP, AppGlobalVariables.Database.Name))
                {
                    MessageBox.Show("Can not connect database IP : " + Configs.ServerIP + " | database name : " + AppGlobalVariables.Database.Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                HandleLogin();
            }
        }

        private void btnAddDatabase_Click(object sender, EventArgs e)
        {
            FormDatabase frm = new FormDatabase();
            frm.ShowDialog();
            cobDatabase.Items.Clear();
            foreach (var kvp in AppGlobalVariables.Database.LookupList.ToArray())
            {
                cobDatabase.Items.Add(kvp.Key);
            }
            if (cobDatabase.Items.Count > 0)
                cobDatabase.SelectedIndex = 0;
        }

        private void MifareCheckTimer_Tick(object sender, EventArgs e)
        {
            if (AppGlobalVariables.OperatingUser.LoginReady)
                return;
            if (mfReader.CheckCard())
            {


                MifareCheckTimer.Enabled = false;
                UsernameTextBox.Text = mfReader.Init1();
                if (UsernameTextBox.Text != "")
                {
                    mfReader.SetLED(1);
                    if (Configs.Hardwares.IsMFPassiveInProx)
                    {
                        UsernameTextBox.Text = UsernameTextBox.Text.Substring(4, 2) + UsernameTextBox.Text.Substring(2, 2) + UsernameTextBox.Text.Substring(0, 2);
                    }
                    uint intID = Convert.ToUInt32(UsernameTextBox.Text, 16);
                    UsernameTextBox.Text = "";
                    AccessManager.Login("", "", intID.ToString(), false);
                    mfReader.SetSound(8);
                    mfReader.SetLED(2);
                    if (AppGlobalVariables.OperatingUser.LoginReady)
                    {
                        Close();
                    }
                    else
                    {
                        MifareCheckTimer.Enabled = true;
                    }
                }
                else
                {
                    MifareCheckTimer.Enabled = true;
                }
            }
        }

        #endregion UI_EVENT_HANDLER_END

        #region PROCESS
        private void HandleLogin()
        {
            if (cobDatabase.Text.Length > 0)
            {
                Configs.ServerIP = AppGlobalVariables.Database.LookupList[cobDatabase.Text].Split('|')[0];
                try
                {
                    AppGlobalVariables.Database.Name = AppGlobalVariables.Database.LookupList[cobDatabase.Text].Split('|')[1];

                    if (AppGlobalVariables.Database.Name.Trim().Length == 0)
                        AppGlobalVariables.Database.Name = "carpark2";

                    if (AppGlobalVariables.Database.Name.Trim().Contains("_m") || AppGlobalVariables.Database.Name.Trim().ToLower().Contains("moto"))
                    {
                        AppGlobalVariables.Database.VehicleTypeTh = "รถจักรยานยนต์";
                        AppGlobalVariables.Database.VehicleTypeEn = "Motorcycle";
                    }
                    else
                    {
                        AppGlobalVariables.Database.VehicleTypeTh = "รถยนต์";
                        AppGlobalVariables.Database.VehicleTypeEn = "Car";
                    }
                }
                catch
                {
                    AppGlobalVariables.Database.Name = "carpark2";
                    AppGlobalVariables.Database.VehicleTypeTh = "รถยนต์";
                    AppGlobalVariables.Database.VehicleTypeEn = "Car";
                }
            }
            if (!DbController.Connect(Configs.ServerIP, AppGlobalVariables.Database.Name))
            {
                MessageBox.Show("Can not connect database IP : " + Configs.ServerIP + " | database name : " + AppGlobalVariables.Database.Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DoLogin();
        }

        private void DoLogin()
        {
            if (UsernameTextBox.Text == "" || UserPasswordTextBox.Text == "")
                return;
            else
                AccessManager.Login(UsernameTextBox.Text, UserPasswordTextBox.Text, "", false);

            if (AppGlobalVariables.OperatingUser.LoginReady)
            {
                Close();
            }
            else
            {
                UserPasswordTextBox.Clear();
                UsernameTextBox.Text = "";
                UsernameTextBox.Focus();
            }
        }
        #endregion PROCESS_END
    }
}