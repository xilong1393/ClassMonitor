namespace ClassMonitor3.Interfaces
{
    partial class LoginForm
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
            System.Windows.Forms.Label lblName;
            System.Windows.Forms.Button btnLogin;
            System.Windows.Forms.Button btnCancel;
            System.Windows.Forms.Label lblPwd;
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            lblName = new System.Windows.Forms.Label();
            btnLogin = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            lblPwd = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            lblName.Anchor = System.Windows.Forms.AnchorStyles.None;
            lblName.AutoSize = true;
            lblName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblName.Location = new System.Drawing.Point(256, 189);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(46, 19);
            lblName.TabIndex = 5;
            lblName.Text = "Name";
            // 
            // btnLogin
            // 
            btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            btnLogin.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnLogin.Location = new System.Drawing.Point(437, 337);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(87, 35);
            btnLogin.TabIndex = 10;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += new System.EventHandler(this.btnLogin_ClickAsync);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            btnCancel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnCancel.Location = new System.Drawing.Point(325, 337);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(87, 35);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblPwd
            // 
            lblPwd.Anchor = System.Windows.Forms.AnchorStyles.None;
            lblPwd.AutoSize = true;
            lblPwd.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblPwd.Location = new System.Drawing.Point(233, 251);
            lblPwd.Name = "lblPwd";
            lblPwd.Size = new System.Drawing.Size(69, 19);
            lblPwd.TabIndex = 7;
            lblPwd.Text = "Password";
            // 
            // txtName
            // 
            this.txtName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtName.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(343, 185);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(181, 26);
            this.txtName.TabIndex = 6;
            // 
            // txtPwd
            // 
            this.txtPwd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPwd.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtPwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPwd.Location = new System.Drawing.Point(343, 247);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(181, 26);
            this.txtPwd.TabIndex = 8;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ClassMonitor3.Properties.Resources.depaul;
            this.pictureBox1.Location = new System.Drawing.Point(87, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 101);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(800, 532);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(lblName);
            this.Controls.Add(btnLogin);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(btnCancel);
            this.Controls.Add(lblPwd);
            this.DoubleBuffered = true;
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LoginForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}