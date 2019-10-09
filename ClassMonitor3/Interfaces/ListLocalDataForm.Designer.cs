namespace ClassMonitor3.Interfaces
{
    partial class ListLocalDataForm
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SortName = new System.Windows.Forms.Button();
            this.SortSelect = new System.Windows.Forms.Button();
            this.SortDate = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(25, 55);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(521, 319);
            this.checkedListBox1.TabIndex = 0;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.checkedListBox1);
            this.panel1.Location = new System.Drawing.Point(113, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(575, 413);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.SortName);
            this.panel2.Controls.Add(this.SortSelect);
            this.panel2.Controls.Add(this.SortDate);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Location = new System.Drawing.Point(25, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(521, 38);
            this.panel2.TabIndex = 1;
            // 
            // SortName
            // 
            this.SortName.Location = new System.Drawing.Point(418, 12);
            this.SortName.Name = "SortName";
            this.SortName.Size = new System.Drawing.Size(84, 23);
            this.SortName.TabIndex = 0;
            this.SortName.Text = "Sort by Name";
            this.SortName.UseVisualStyleBackColor = true;
            this.SortName.Click += new System.EventHandler(this.SortName_Click);
            // 
            // SortSelect
            // 
            this.SortSelect.Location = new System.Drawing.Point(327, 12);
            this.SortSelect.Name = "SortSelect";
            this.SortSelect.Size = new System.Drawing.Size(85, 23);
            this.SortSelect.TabIndex = 0;
            this.SortSelect.Text = "Sort by Select";
            this.SortSelect.UseVisualStyleBackColor = true;
            this.SortSelect.Click += new System.EventHandler(this.SortSelect_Click);
            // 
            // SortDate
            // 
            this.SortDate.Location = new System.Drawing.Point(246, 12);
            this.SortDate.Name = "SortDate";
            this.SortDate.Size = new System.Drawing.Size(75, 23);
            this.SortDate.TabIndex = 0;
            this.SortDate.Text = "Sort by Date";
            this.SortDate.UseVisualStyleBackColor = true;
            this.SortDate.Click += new System.EventHandler(this.SortDate_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(165, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "Select All";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(84, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_ClickAsync);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Upload";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_ClickAsync);
            // 
            // ListLocalDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "ListLocalDataForm";
            this.Text = "ListLocalDataForm";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button SortName;
        private System.Windows.Forms.Button SortSelect;
        private System.Windows.Forms.Button SortDate;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}