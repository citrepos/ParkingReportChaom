namespace ParkingManagementReport
{
    partial class FormLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbUsername = new System.Windows.Forms.Label();
            this.lbPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tmMFCheck = new System.Windows.Forms.Timer(this.components);
            this.btnAddDatabase = new System.Windows.Forms.Button();
            this.cobDatabase = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("LogoPictureBox.Image")));
            this.LogoPictureBox.Location = new System.Drawing.Point(0, 1);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(133, 174);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LogoPictureBox.TabIndex = 0;
            this.LogoPictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(155, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "กรุณาล็อกอินด้วยบัตรหรือใส่รหัสผ่าน";
            // 
            // lbUsername
            // 
            this.lbUsername.Location = new System.Drawing.Point(158, 40);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(67, 23);
            this.lbUsername.TabIndex = 2;
            this.lbUsername.Text = "&User name";
            this.lbUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbPassword
            // 
            this.lbPassword.Location = new System.Drawing.Point(158, 77);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(61, 23);
            this.lbPassword.TabIndex = 3;
            this.lbPassword.Text = "&Password";
            this.lbPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUserName
            // 
            this.txtUserName.AcceptsReturn = true;
            this.txtUserName.Location = new System.Drawing.Point(231, 43);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(127, 20);
            this.txtUserName.TabIndex = 4;
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserName_KeyDown);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(231, 80);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(127, 20);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(161, 123);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(78, 33);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(280, 123);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(78, 33);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tmMFCheck
            // 
            this.tmMFCheck.Interval = 1000;
            this.tmMFCheck.Tick += new System.EventHandler(this.tmMFCheck_Tick);
            // 
            // btnAddDatabase
            // 
            this.btnAddDatabase.Location = new System.Drawing.Point(263, 182);
            this.btnAddDatabase.Name = "btnAddDatabase";
            this.btnAddDatabase.Size = new System.Drawing.Size(95, 23);
            this.btnAddDatabase.TabIndex = 8;
            this.btnAddDatabase.Text = "เพิ่ม/แก้ไข DB.";
            this.btnAddDatabase.UseVisualStyleBackColor = true;
            this.btnAddDatabase.Click += new System.EventHandler(this.btnAddDatabase_Click);
            // 
            // cobDatabase
            // 
            this.cobDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobDatabase.FormattingEnabled = true;
            this.cobDatabase.Location = new System.Drawing.Point(117, 184);
            this.cobDatabase.Name = "cobDatabase";
            this.cobDatabase.Size = new System.Drawing.Size(132, 21);
            this.cobDatabase.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(37, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Database :";
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(382, 212);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cobDatabase);
            this.Controls.Add(this.btnAddDatabase);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.lbUsername);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogoPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogin_FormClosing);
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormLogin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.PictureBox LogoPictureBox;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label lbUsername;
        internal System.Windows.Forms.Label lbPassword;
        internal System.Windows.Forms.TextBox txtUserName;
        internal System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.Button btnLogin;
        internal System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Timer tmMFCheck;
        private System.Windows.Forms.Button btnAddDatabase;
        private System.Windows.Forms.ComboBox cobDatabase;
        private System.Windows.Forms.Label label2;
    }
}