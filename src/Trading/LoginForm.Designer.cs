namespace Trading
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            chkSaveKeys = new CheckBox();
            btnExit = new Button();
            btnLogin = new Button();
            txtSecretKey = new TextBox();
            txtAccessKey = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(chkSaveKeys);
            panel1.Controls.Add(btnExit);
            panel1.Controls.Add(btnLogin);
            panel1.Controls.Add(txtSecretKey);
            panel1.Controls.Add(txtAccessKey);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(469, 116);
            panel1.TabIndex = 0;
            // 
            // chkSaveKeys
            // 
            chkSaveKeys.AutoSize = true;
            chkSaveKeys.Checked = true;
            chkSaveKeys.CheckState = CheckState.Checked;
            chkSaveKeys.Location = new Point(12, 80);
            chkSaveKeys.Name = "chkSaveKeys";
            chkSaveKeys.Size = new Size(79, 19);
            chkSaveKeys.TabIndex = 13;
            chkSaveKeys.Text = "Save Keys";
            chkSaveKeys.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(299, 68);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(158, 40);
            btnExit.TabIndex = 12;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(135, 68);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(158, 40);
            btnLogin.TabIndex = 11;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtSecretKey
            // 
            txtSecretKey.Location = new Point(84, 39);
            txtSecretKey.Name = "txtSecretKey";
            txtSecretKey.PasswordChar = '*';
            txtSecretKey.Size = new Size(373, 23);
            txtSecretKey.TabIndex = 10;
            // 
            // txtAccessKey
            // 
            txtAccessKey.Location = new Point(84, 9);
            txtAccessKey.Name = "txtAccessKey";
            txtAccessKey.PasswordChar = '*';
            txtAccessKey.Size = new Size(373, 23);
            txtAccessKey.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 42);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 8;
            label2.Text = "Secret Key";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 7;
            label1.Text = "Access Key";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(469, 116);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private CheckBox chkSaveKeys;
        private Button btnExit;
        private Button btnLogin;
        private TextBox txtSecretKey;
        private TextBox txtAccessKey;
        private Label label2;
        private Label label1;
    }
}
