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
    public partial class FormDatabase : Form
    {
        string tmpNameDB = "";
        public FormDatabase()
        {
            InitializeComponent();
        }

        private void FormDatabase_Load(object sender, EventArgs e)
        {
            try
            {
                this.TopMost = true;
                cobDB.Items.Clear();
                foreach (var kvp in FormMain.pm.DicDatabase.ToArray())
                {
                    cobDB.Items.Add(kvp.Key);
                }
                if (cobDB.Items.Count > 0)
                    cobDB.SelectedIndex = 0;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void btnDeleteDB_Click(object sender, EventArgs e)
        {
            try
            {
                string strText = "";
                btnDeleteDB.Enabled = true;
                FormMain.pm.DicDatabase.Remove(txtNameDB.Text);
                cobDB.Items.Clear();
                foreach (var kvp in FormMain.pm.DicDatabase.ToArray())
                {
                    cobDB.Items.Add(kvp.Key);
                    strText += kvp.Value + "," + kvp.Key + Environment.NewLine;
                }
                if (cobDB.Items.Count > 0)
                    cobDB.SelectedIndex = 0;

                if (!File.Exists(@"C:\Windows\carpark\conDatabase.txt"))
                {
                    StreamWriter sw = File.CreateText(@"C:\Windows\carpark\conDatabase.txt");
                    sw.WriteLine(strText);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    File.Delete(@"C:\Windows\carpark\conDatabase.txt");
                    FileStream MyFileStream = new FileStream(@"C:\Windows\carpark\conDatabase.txt", FileMode.Append, FileAccess.Write, FileShare.Read);
                    StreamWriter sw = new StreamWriter(MyFileStream);
                    sw.WriteLine(strText);
                    sw.Close();
                    MyFileStream.Close();
                }

                tmpNameDB = "";
                txtNameDB.Text = "";
                txtIPDB.Text = "";
                txtDB.Text = ""; //Mac 2016/11/09
                MessageBox.Show("Delete Complete", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void btnAddDB_Click(object sender, EventArgs e)
        {
            try
            {
                string strText = "";
                if ((txtIPDB.Text.Trim().Length < 1) && (txtNameDB.Text.Trim().Length < 1))
                {
                    MessageBox.Show("กรุณาใส่ IP และ Name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtIPDB.Focus();
                    return;
                }

                if (tmpNameDB.Length > 0)
                {
                    FormMain.pm.DicDatabase.Remove(tmpNameDB);
                    //FormMain.pm.DicDatabase.Add(txtNameDB.Text, txtIPDB.Text);
                    FormMain.pm.DicDatabase.Add(txtNameDB.Text, txtIPDB.Text + "|" + txtDB.Text); //Mac 2016/11/09
                    tmpNameDB = "";
                    txtNameDB.Text = "";
                    txtIPDB.Text = "";
                    txtDB.Text = ""; //Mac 2016/11/09
                    MessageBox.Show("Update Complete", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (var kvp in FormMain.pm.DicDatabase.ToArray())
                    {
                        if (kvp.Key == txtNameDB.Text)
                        {
                            MessageBox.Show("Name ซ้ำกัน", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tmpNameDB = "";
                            txtNameDB.Focus();
                            return;
                        }
                    }

                    //FormMain.pm.DicDatabase.Add(txtNameDB.Text, txtIPDB.Text);
                    FormMain.pm.DicDatabase.Add(txtNameDB.Text, txtIPDB.Text + "|" + txtDB.Text); //Mac 2016/11/09
                    tmpNameDB = "";
                    txtNameDB.Text = "";
                    txtIPDB.Text = "";
                    txtDB.Text = ""; //Mac 2016/11/09
                    MessageBox.Show("Add Complete", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                cobDB.Items.Clear();
                foreach (var kvp in FormMain.pm.DicDatabase.ToArray())
                {
                    cobDB.Items.Add(kvp.Key);
                    strText += kvp.Value + "," + kvp.Key + Environment.NewLine;
                }
                if (cobDB.Items.Count > 0)
                    cobDB.SelectedIndex = 0;

                if (!File.Exists(@"C:\Windows\carpark\conDatabase.txt"))
                {
                    StreamWriter sw = File.CreateText(@"C:\Windows\carpark\conDatabase.txt");
                    sw.WriteLine(strText);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    File.Delete(@"C:\Windows\carpark\conDatabase.txt");
                    FileStream MyFileStream = new FileStream(@"C:\Windows\carpark\conDatabase.txt", FileMode.Append, FileAccess.Write, FileShare.Read);
                    StreamWriter sw = new StreamWriter(MyFileStream);
                    sw.WriteLine(strText);
                    sw.Close();
                    MyFileStream.Close();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void cobDB_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                tmpNameDB = "";
                tmpNameDB = cobDB.Text;
                txtNameDB.Text = cobDB.Text;
                //txtIPDB.Text = FormMain.pm.DicDatabase[cobDB.Text];
                txtIPDB.Text = FormMain.pm.DicDatabase[cobDB.Text].Split('|')[0]; //Mac 2016/11/10
                try
                {
                    txtDB.Text = FormMain.pm.DicDatabase[cobDB.Text].Split('|')[1]; //Mac 2016/11/10
                }
                catch { txtDB.Text = ""; }

                btnDeleteDB.Enabled = true;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void btnNewDB_Click(object sender, EventArgs e)
        {
            try
            {
                tmpNameDB = "";
                txtNameDB.Text = "";
                txtIPDB.Text = "";
                txtDB.Text = ""; //Mac 2016/11/09
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }
    }
}
