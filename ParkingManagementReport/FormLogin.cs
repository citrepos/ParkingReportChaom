using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ParkingManagementReport
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void Login()
        {
            if (txtUserName.Text == "" || txtPassword.Text == "")
                return;
            else
            {
                FormMain.pm.Login(txtUserName.Text, txtPassword.Text, "", false);
                FormMain.pm.LoadOnlinePaymentType();
            }

            if (FormMain.pm.user.LoginReady)
            {
                Close();
            }
            else
            {
                txtPassword.Clear();
                txtUserName.Text = "";
                txtUserName.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            if (FormMain.pm.UseMifare)
            {
                tmMFCheck.Enabled = true;
            }

            txtUserName.Focus();
            this.KeyPreview = true;
            //Mac 2015/07/29 -----------------------------
            if (!File.Exists(@"C:\Windows\carpark\conDatabase.txt"))
            {
                this.Height = 211;
            }
            else
            {
                string strFile = @"C:\Windows\carpark\conDatabase.txt";
                FileStream MyFileStream = new FileStream(strFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(MyFileStream, System.Text.Encoding.UTF8, true);
                String line = "";
                string[] str;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Trim().Length > 0)
                    {
                        //line = sr.ReadLine();
                        str = line.Split(',');
                        FormMain.pm.DicDatabase.Add(str[1], str[0]);
                        //DicPromotionIns.Add(Int32.Parse(str[0]), str[1]);
                        cobDatabase.Items.Add(str[1]);
                    }
                }
                sr.Close();
                MyFileStream.Close();
                if (cobDatabase.Items.Count > 0)
                    cobDatabase.SelectedIndex = 0;
            }
            //--------------------------------------------
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!FormMain.pm.user.LoginReady)
            {
                if (MessageBox.Show("ต้องการออกจากโปรแกรม", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    FormMain.pm.user.LoginReady = true;
                    Environment.Exit(0); //Mac 2015/07/29
                    //Application.Exit();
                }
            }
        }

        String strID = "";

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (strID.Length >= 10)
                {
                    //FormMain.pm.UseLogOut = false;
                    FormMain.pm.Login("", "", strID, false);
                    if (FormMain.pm.user.LoginReady)
                    {
                        Close();
                    }
                    strID = "";
                }
            }
            else
            {
                String strKey = e.KeyCode.ToString();
                if (strKey.IndexOf("D") >= 0)
                {
                    try
                    {
                        strID += strKey.Substring(1, 1);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void tmMFCheck_Tick(object sender, EventArgs e)
        {
            if (FormMain.pm.user.LoginReady)
                return;
            if (FormMain.pm.mifaV.chkCard())
            {
                tmMFCheck.Enabled = false;
                strID = FormMain.pm.mifaV.Init1();
                if (strID != "")
                {
                    FormMain.pm.mifaV.setLED(1);
                    if (FormMain.pm.print.MFPassiveInProx) //Mac 2015/06/13
                    {
                        strID = strID.Substring(4, 2) + strID.Substring(2, 2) + strID.Substring(0, 2);
                    }
                    uint intID = Convert.ToUInt32(strID, 16);
                    strID = "";
                    //FormMain.pm.UseLogOut = true;
                    FormMain.pm.Login("", "", intID.ToString(), false);
                    FormMain.pm.mifaV.setSound(8);
                    //FormMain.pm.mifa.WaitCardOut();
                    //System.Threading.Thread.Sleep(50);
                    FormMain.pm.mifaV.setLED(2);
                    if (FormMain.pm.user.LoginReady)
                    {
                        Close();
                    }
                    else
                    {
                        tmMFCheck.Enabled = true;
                    }
                }
                else
                {
                    tmMFCheck.Enabled = true;
                }
            }
            //else
            //{
            //    FormMain.pm.mifa.setLED(2);
            //}
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Mac 2015/07/29 -----------
            if (cobDatabase.Text.Length > 0)
            {
                //FormMain.pm.ServerIP = FormMain.pm.DicDatabase[cobDatabase.Text];
                FormMain.pm.ServerIP = FormMain.pm.DicDatabase[cobDatabase.Text].Split('|')[0]; //Mac 2016/11/10
                try
                {
                    FormMain.pm.DatabaseName = FormMain.pm.DicDatabase[cobDatabase.Text].Split('|')[1]; //Mac 2016/11/10
                    if (FormMain.pm.DatabaseName.Trim().Length == 0)
                        FormMain.pm.DatabaseName = "carpark2";
                }
                catch { FormMain.pm.DatabaseName = "carpark2"; }
            }
            /*if (!FormMain.pm.DBConnect(FormMain.pm.ServerIP))
            {
                MessageBox.Show("Can not connect database IP : " + FormMain.pm.ServerIP, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/
            //Mac 2016/11/10
            if (!FormMain.pm.DBConnect(FormMain.pm.ServerIP, FormMain.pm.DatabaseName))
            {
                MessageBox.Show("Can not connect database IP : " + FormMain.pm.ServerIP + " | database name : " + FormMain.pm.DatabaseName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //--------------------------

            Login();
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                txtPassword.Focus();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                //Mac 2015/07/29 -----------
                if (cobDatabase.Text.Length > 0)
                {
                    //FormMain.pm.ServerIP = FormMain.pm.DicDatabase[cobDatabase.Text];
                    FormMain.pm.ServerIP = FormMain.pm.DicDatabase[cobDatabase.Text].Split('|')[0]; //Mac 2016/11/10
                    try
                    {
                        FormMain.pm.DatabaseName = FormMain.pm.DicDatabase[cobDatabase.Text].Split('|')[1]; //Mac 2016/11/10
                        if (FormMain.pm.DatabaseName.Trim().Length == 0)
                            FormMain.pm.DatabaseName = "carpark2";
                    }
                    catch { FormMain.pm.DatabaseName = "carpark2"; }
                }
                /*if (!FormMain.pm.DBConnect(FormMain.pm.ServerIP))
                {
                    MessageBox.Show("Can not connect database IP : " + FormMain.pm.ServerIP, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }*/
                //Mac 2016/11/10
                if (!FormMain.pm.DBConnect(FormMain.pm.ServerIP, FormMain.pm.DatabaseName))
                {
                    MessageBox.Show("Can not connect database IP : " + FormMain.pm.ServerIP + " | database name : " + FormMain.pm.DatabaseName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //--------------------------

                Login();
            }
        }

        private void btnAddDatabase_Click(object sender, EventArgs e)
        {
            FormDatabase frm = new FormDatabase();
            frm.ShowDialog();
            cobDatabase.Items.Clear();
            foreach (var kvp in FormMain.pm.DicDatabase.ToArray())
            {
                cobDatabase.Items.Add(kvp.Key);
            }
            if (cobDatabase.Items.Count > 0)
                cobDatabase.SelectedIndex = 0;
        }
    }
}
