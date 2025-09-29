namespace ParkingManagementReport
{
    partial class FormDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabase));
            this.cobDB = new System.Windows.Forms.ComboBox();
            this.txtNameDB = new System.Windows.Forms.TextBox();
            this.txtIPDB = new System.Windows.Forms.TextBox();
            this.btnAddDB = new System.Windows.Forms.Button();
            this.btnDeleteDB = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNewDB = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cobDB
            // 
            this.cobDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobDB.FormattingEnabled = true;
            this.cobDB.Location = new System.Drawing.Point(116, 20);
            this.cobDB.Margin = new System.Windows.Forms.Padding(4);
            this.cobDB.Name = "cobDB";
            this.cobDB.Size = new System.Drawing.Size(221, 24);
            this.cobDB.TabIndex = 0;
            this.cobDB.DropDownClosed += new System.EventHandler(this.cobDB_DropDownClosed);
            // 
            // txtNameDB
            // 
            this.txtNameDB.Location = new System.Drawing.Point(103, 84);
            this.txtNameDB.Margin = new System.Windows.Forms.Padding(4);
            this.txtNameDB.Name = "txtNameDB";
            this.txtNameDB.Size = new System.Drawing.Size(221, 22);
            this.txtNameDB.TabIndex = 1;
            // 
            // txtIPDB
            // 
            this.txtIPDB.Location = new System.Drawing.Point(103, 142);
            this.txtIPDB.Margin = new System.Windows.Forms.Padding(4);
            this.txtIPDB.Name = "txtIPDB";
            this.txtIPDB.Size = new System.Drawing.Size(221, 22);
            this.txtIPDB.TabIndex = 2;
            // 
            // btnAddDB
            // 
            this.btnAddDB.Location = new System.Drawing.Point(139, 247);
            this.btnAddDB.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddDB.Name = "btnAddDB";
            this.btnAddDB.Size = new System.Drawing.Size(100, 28);
            this.btnAddDB.TabIndex = 3;
            this.btnAddDB.Text = "Add/Update";
            this.btnAddDB.UseVisualStyleBackColor = true;
            this.btnAddDB.Click += new System.EventHandler(this.btnAddDB_Click);
            // 
            // btnDeleteDB
            // 
            this.btnDeleteDB.Enabled = false;
            this.btnDeleteDB.Location = new System.Drawing.Point(263, 247);
            this.btnDeleteDB.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteDB.Name = "btnDeleteDB";
            this.btnDeleteDB.Size = new System.Drawing.Size(100, 28);
            this.btnDeleteDB.TabIndex = 4;
            this.btnDeleteDB.Text = "Delete";
            this.btnDeleteDB.UseVisualStyleBackColor = true;
            this.btnDeleteDB.Click += new System.EventHandler(this.btnDeleteDB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Database";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 145);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 87);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Name";
            // 
            // btnNewDB
            // 
            this.btnNewDB.Location = new System.Drawing.Point(15, 247);
            this.btnNewDB.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewDB.Name = "btnNewDB";
            this.btnNewDB.Size = new System.Drawing.Size(100, 28);
            this.btnNewDB.TabIndex = 8;
            this.btnNewDB.Text = "New";
            this.btnNewDB.UseVisualStyleBackColor = true;
            this.btnNewDB.Click += new System.EventHandler(this.btnNewDB_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 201);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "DBName";
            // 
            // txtDB
            // 
            this.txtDB.Location = new System.Drawing.Point(103, 198);
            this.txtDB.Margin = new System.Windows.Forms.Padding(4);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(221, 22);
            this.txtDB.TabIndex = 9;
            // 
            // FormDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 322);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDB);
            this.Controls.Add(this.btnNewDB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDeleteDB);
            this.Controls.Add(this.btnAddDB);
            this.Controls.Add(this.txtIPDB);
            this.Controls.Add(this.txtNameDB);
            this.Controls.Add(this.cobDB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add/Change Database";
            this.Load += new System.EventHandler(this.FormDatabase_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cobDB;
        private System.Windows.Forms.TextBox txtNameDB;
        private System.Windows.Forms.TextBox txtIPDB;
        private System.Windows.Forms.Button btnAddDB;
        private System.Windows.Forms.Button btnDeleteDB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnNewDB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDB;
    }
}