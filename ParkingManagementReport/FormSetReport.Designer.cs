namespace ParkingManagementReport
{
    partial class FormSetReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetReport));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtStartTime = new System.Windows.Forms.TextBox();
            this.txtFinishTime = new System.Windows.Forms.TextBox();
            this.txtPayHour = new System.Windows.Forms.TextBox();
            this.txtPayMinute = new System.Windows.Forms.TextBox();
            this.txtLossCard = new System.Windows.Forms.TextBox();
            this.txtMoreOne = new System.Windows.Forms.TextBox();
            this.txtFatPay = new System.Windows.Forms.TextBox();
            this.txtMinuteToHour = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDayWeek = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer1.Location = new System.Drawing.Point(8, 123);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView2);
            this.splitContainer1.Size = new System.Drawing.Size(1184, 560);
            this.splitContainer1.SplitterDistance = 229;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1180, 225);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.Size = new System.Drawing.Size(1180, 322);
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 15);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 28);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "เพิ่ม";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(921, 15);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "บันทึกข้อมูล";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1051, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "ยกเลิก";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtStartTime
            // 
            this.txtStartTime.Location = new System.Drawing.Point(98, 91);
            this.txtStartTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Size = new System.Drawing.Size(97, 22);
            this.txtStartTime.TabIndex = 4;
            this.txtStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtFinishTime
            // 
            this.txtFinishTime.Location = new System.Drawing.Point(219, 91);
            this.txtFinishTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtFinishTime.Name = "txtFinishTime";
            this.txtFinishTime.Size = new System.Drawing.Size(95, 22);
            this.txtFinishTime.TabIndex = 5;
            this.txtFinishTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPayHour
            // 
            this.txtPayHour.Location = new System.Drawing.Point(459, 91);
            this.txtPayHour.Margin = new System.Windows.Forms.Padding(4);
            this.txtPayHour.Name = "txtPayHour";
            this.txtPayHour.Size = new System.Drawing.Size(92, 22);
            this.txtPayHour.TabIndex = 7;
            this.txtPayHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPayMinute
            // 
            this.txtPayMinute.Location = new System.Drawing.Point(338, 91);
            this.txtPayMinute.Margin = new System.Windows.Forms.Padding(4);
            this.txtPayMinute.Name = "txtPayMinute";
            this.txtPayMinute.Size = new System.Drawing.Size(96, 22);
            this.txtPayMinute.TabIndex = 6;
            this.txtPayMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLossCard
            // 
            this.txtLossCard.Location = new System.Drawing.Point(973, 91);
            this.txtLossCard.Margin = new System.Windows.Forms.Padding(4);
            this.txtLossCard.Name = "txtLossCard";
            this.txtLossCard.Size = new System.Drawing.Size(99, 22);
            this.txtLossCard.TabIndex = 11;
            this.txtLossCard.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtMoreOne
            // 
            this.txtMoreOne.Location = new System.Drawing.Point(843, 91);
            this.txtMoreOne.Margin = new System.Windows.Forms.Padding(4);
            this.txtMoreOne.Name = "txtMoreOne";
            this.txtMoreOne.Size = new System.Drawing.Size(99, 22);
            this.txtMoreOne.TabIndex = 10;
            this.txtMoreOne.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtFatPay
            // 
            this.txtFatPay.Location = new System.Drawing.Point(707, 91);
            this.txtFatPay.Margin = new System.Windows.Forms.Padding(4);
            this.txtFatPay.Name = "txtFatPay";
            this.txtFatPay.Size = new System.Drawing.Size(103, 22);
            this.txtFatPay.TabIndex = 9;
            this.txtFatPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtMinuteToHour
            // 
            this.txtMinuteToHour.Location = new System.Drawing.Point(579, 91);
            this.txtMinuteToHour.Margin = new System.Windows.Forms.Padding(4);
            this.txtMinuteToHour.Name = "txtMinuteToHour";
            this.txtMinuteToHour.Size = new System.Drawing.Size(101, 22);
            this.txtMinuteToHour.TabIndex = 8;
            this.txtMinuteToHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 71);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "นาทีเริ่ม";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(215, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 17);
            this.label3.TabIndex = 14;
            this.label3.Text = "นาทีสิ้นสุด";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(455, 71);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "ชั่วโมงละ(บาท)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(334, 71);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 17);
            this.label5.TabIndex = 15;
            this.label5.Text = "นาทีละ(บาท)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(575, 71);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 17);
            this.label6.TabIndex = 20;
            this.label6.Text = "ปัดเศษ(นาที)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(703, 71);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 17);
            this.label7.TabIndex = 19;
            this.label7.Text = "เหมาจ่าย(บาท)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(821, 71);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 17);
            this.label8.TabIndex = 18;
            this.label8.Text = "จอดเกินกำหนด(บาท)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(969, 71);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 17);
            this.label9.TabIndex = 17;
            this.label9.Text = "บัตรหาย(บาท)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1089, 71);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 17);
            this.label10.TabIndex = 22;
            this.label10.Text = "วันในสัปดาห์";
            // 
            // txtDayWeek
            // 
            this.txtDayWeek.Location = new System.Drawing.Point(1093, 91);
            this.txtDayWeek.Margin = new System.Windows.Forms.Padding(4);
            this.txtDayWeek.Name = "txtDayWeek";
            this.txtDayWeek.Size = new System.Drawing.Size(99, 22);
            this.txtDayWeek.TabIndex = 21;
            // 
            // FormSetReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 688);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtDayWeek);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLossCard);
            this.Controls.Add(this.txtMoreOne);
            this.Controls.Add(this.txtFatPay);
            this.Controls.Add(this.txtMinuteToHour);
            this.Controls.Add(this.txtPayHour);
            this.Controls.Add(this.txtPayMinute);
            this.Controls.Add(this.txtFinishTime);
            this.Controls.Add(this.txtStartTime);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormSetReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "เงื่อนไขการคำนวณ";
            this.Load += new System.EventHandler(this.FormSetReport_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtStartTime;
        private System.Windows.Forms.TextBox txtFinishTime;
        private System.Windows.Forms.TextBox txtPayHour;
        private System.Windows.Forms.TextBox txtPayMinute;
        private System.Windows.Forms.TextBox txtLossCard;
        private System.Windows.Forms.TextBox txtMoreOne;
        private System.Windows.Forms.TextBox txtFatPay;
        private System.Windows.Forms.TextBox txtMinuteToHour;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDayWeek;


    }
}