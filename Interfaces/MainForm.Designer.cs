using System.Windows.Forms;

namespace ClassMonitor3.Interfaces
{
    partial class MainForm : Form
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
            this.lblLogout = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.menuCB = new System.Windows.Forms.ComboBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnStartCourse = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.flRightPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLogout
            // 
            this.lblLogout.BackColor = System.Drawing.Color.Teal;
            this.lblLogout.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblLogout.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblLogout.Location = new System.Drawing.Point(1188, 0);
            this.lblLogout.Name = "lblLogout";
            this.lblLogout.Size = new System.Drawing.Size(82, 43);
            this.lblLogout.TabIndex = 9;
            this.lblLogout.Text = "Logout";
            this.lblLogout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLogout.Click += new System.EventHandler(this.label3_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Teal;
            this.panel4.Controls.Add(this.menuCB);
            this.panel4.Controls.Add(this.lblLogout);
            this.panel4.Controls.Add(this.button9);
            this.panel4.Controls.Add(this.button8);
            this.panel4.Controls.Add(this.button5);
            this.panel4.Controls.Add(this.button10);
            this.panel4.Controls.Add(this.button7);
            this.panel4.Controls.Add(this.button6);
            this.panel4.Controls.Add(this.button4);
            this.panel4.Controls.Add(this.button11);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.btnStartCourse);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1270, 43);
            this.panel4.TabIndex = 13;
            // 
            // menuCB
            // 
            this.menuCB.FormattingEnabled = true;
            this.menuCB.Location = new System.Drawing.Point(1061, 11);
            this.menuCB.Name = "menuCB";
            this.menuCB.Size = new System.Drawing.Size(100, 23);
            this.menuCB.TabIndex = 10;
            this.menuCB.SelectedIndexChanged += new System.EventHandler(this.menuCB_SelectedIndexChangedAsync);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(983, 9);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(72, 27);
            this.button9.TabIndex = 1;
            this.button9.Text = "Reboot IPC";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(901, 9);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(76, 27);
            this.button8.TabIndex = 1;
            this.button8.Text = "Reboot PPC";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(808, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(87, 27);
            this.button5.TabIndex = 1;
            this.button5.Text = "Push Config";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(707, 9);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(95, 27);
            this.button10.TabIndex = 1;
            this.button10.Text = "List Local Data";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.Location = new System.Drawing.Point(606, 9);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(95, 27);
            this.button7.TabIndex = 1;
            this.button7.Text = "Group Schedule";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(509, 9);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(89, 27);
            this.button6.TabIndex = 1;
            this.button6.Text = "Check Schedule";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(414, 9);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(87, 27);
            this.button4.TabIndex = 1;
            this.button4.Text = "Push Schedule";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Location = new System.Drawing.Point(310, 9);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(95, 27);
            this.button11.TabIndex = 0;
            this.button11.Text = "Classroom Info";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(209, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 27);
            this.button3.TabIndex = 0;
            this.button3.Text = "Start Test Course";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(112, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 27);
            this.button2.TabIndex = 0;
            this.button2.Text = "Abort Course";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnStartCourse
            // 
            this.btnStartCourse.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCourse.Location = new System.Drawing.Point(17, 9);
            this.btnStartCourse.Name = "btnStartCourse";
            this.btnStartCourse.Size = new System.Drawing.Size(87, 27);
            this.btnStartCourse.TabIndex = 0;
            this.btnStartCourse.Text = "Start Course";
            this.btnStartCourse.UseVisualStyleBackColor = true;
            this.btnStartCourse.Click += new System.EventHandler(this.btnStartCourse_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.dataGridView);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(675, 716);
            this.panel1.TabIndex = 14;
            // 
            // dataGridView
            // 
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 26);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(675, 690);
            this.dataGridView.TabIndex = 11;
            this.dataGridView.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView_RowPrePaint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel2.Controls.Add(this.comboBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(675, 26);
            this.panel2.TabIndex = 10;
            // 
            // comboBox
            // 
            this.comboBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.comboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox.FormattingEnabled = true;
            this.comboBox.ItemHeight = 20;
            this.comboBox.Location = new System.Drawing.Point(539, 0);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(136, 28);
            this.comboBox.TabIndex = 0;
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // flRightPanel
            // 
            this.flRightPanel.AutoScroll = true;
            this.flRightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.flRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flRightPanel.Location = new System.Drawing.Point(675, 43);
            this.flRightPanel.Name = "flRightPanel";
            this.flRightPanel.Size = new System.Drawing.Size(595, 716);
            this.flRightPanel.TabIndex = 15;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1270, 759);
            this.Controls.Add(this.flRightPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblLogout;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnStartCourse;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flRightPanel;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.ComboBox menuCB;
        private System.Windows.Forms.Timer timer1;
    }
}