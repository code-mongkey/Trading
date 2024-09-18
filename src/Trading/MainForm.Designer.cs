namespace Trading
{
    partial class MainForm
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            dgvAccounts = new DataGridView();
            tabPage2 = new TabPage();
            dgvMarketInfo = new DataGridView();
            tabPage3 = new TabPage();
            dgvOrderHistory = new DataGridView();
            lblStatus = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            rdo_Sell = new RadioButton();
            rdoBuy = new RadioButton();
            btnPlaceOrder = new Button();
            cmbOrderType = new ComboBox();
            txtVolume = new TextBox();
            txtPrice = new TextBox();
            cmbMarket = new ComboBox();
            tabPage4 = new TabPage();
            lblOpenAIResponse = new Label();
            btnOpenAI = new Button();
            label5 = new Label();
            txtOpenAIAPIKey = new TextBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMarketInfo).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrderHistory).BeginInit();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvAccounts);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(792, 422);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "My Page";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvAccounts
            // 
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAccounts.Dock = DockStyle.Fill;
            dgvAccounts.Location = new Point(3, 3);
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.ReadOnly = true;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.Size = new Size(786, 416);
            dgvAccounts.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(dgvMarketInfo);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 422);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Market Info";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvMarketInfo
            // 
            dgvMarketInfo.AllowUserToAddRows = false;
            dgvMarketInfo.AllowUserToDeleteRows = false;
            dgvMarketInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMarketInfo.Dock = DockStyle.Fill;
            dgvMarketInfo.Location = new Point(3, 3);
            dgvMarketInfo.Name = "dgvMarketInfo";
            dgvMarketInfo.ReadOnly = true;
            dgvMarketInfo.RowHeadersVisible = false;
            dgvMarketInfo.Size = new Size(786, 416);
            dgvMarketInfo.TabIndex = 1;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dgvOrderHistory);
            tabPage3.Controls.Add(lblStatus);
            tabPage3.Controls.Add(label4);
            tabPage3.Controls.Add(label3);
            tabPage3.Controls.Add(label2);
            tabPage3.Controls.Add(label1);
            tabPage3.Controls.Add(rdo_Sell);
            tabPage3.Controls.Add(rdoBuy);
            tabPage3.Controls.Add(btnPlaceOrder);
            tabPage3.Controls.Add(cmbOrderType);
            tabPage3.Controls.Add(txtVolume);
            tabPage3.Controls.Add(txtPrice);
            tabPage3.Controls.Add(cmbMarket);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(792, 422);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Order";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvOrderHistory
            // 
            dgvOrderHistory.AllowUserToAddRows = false;
            dgvOrderHistory.AllowUserToDeleteRows = false;
            dgvOrderHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrderHistory.Location = new Point(330, 15);
            dgvOrderHistory.Name = "dgvOrderHistory";
            dgvOrderHistory.ReadOnly = true;
            dgvOrderHistory.RowHeadersVisible = false;
            dgvOrderHistory.Size = new Size(454, 319);
            dgvOrderHistory.TabIndex = 25;
            dgvOrderHistory.CellContentClick += dgvOrderHistory_CellContentClick;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(48, 265);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(12, 15);
            lblStatus.TabIndex = 24;
            lblStatus.Text = "-";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(42, 156);
            label4.Name = "label4";
            label4.Size = new Size(49, 15);
            label4.TabIndex = 23;
            label4.Text = "Volume";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(38, 117);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 22;
            label3.Text = "Price";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(31, 77);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 21;
            label2.Text = "Order Type";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(31, 48);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 20;
            label1.Text = "Market";
            // 
            // rdo_Sell
            // 
            rdo_Sell.AutoSize = true;
            rdo_Sell.Location = new Point(139, 15);
            rdo_Sell.Name = "rdo_Sell";
            rdo_Sell.Size = new Size(49, 19);
            rdo_Sell.TabIndex = 19;
            rdo_Sell.Text = "매도";
            rdo_Sell.UseVisualStyleBackColor = true;
            // 
            // rdoBuy
            // 
            rdoBuy.AutoSize = true;
            rdoBuy.Checked = true;
            rdoBuy.Location = new Point(38, 15);
            rdoBuy.Name = "rdoBuy";
            rdoBuy.Size = new Size(49, 19);
            rdoBuy.TabIndex = 18;
            rdoBuy.TabStop = true;
            rdoBuy.Text = "매수";
            rdoBuy.UseVisualStyleBackColor = true;
            // 
            // btnPlaceOrder
            // 
            btnPlaceOrder.Location = new Point(30, 201);
            btnPlaceOrder.Name = "btnPlaceOrder";
            btnPlaceOrder.Size = new Size(231, 40);
            btnPlaceOrder.TabIndex = 17;
            btnPlaceOrder.Text = "Order";
            btnPlaceOrder.UseVisualStyleBackColor = true;
            btnPlaceOrder.Click += btnPlaceOrder_Click;
            // 
            // cmbOrderType
            // 
            cmbOrderType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrderType.FormattingEnabled = true;
            cmbOrderType.Items.AddRange(new object[] { "limit", "price", "market" });
            cmbOrderType.Location = new Point(101, 74);
            cmbOrderType.Name = "cmbOrderType";
            cmbOrderType.Size = new Size(160, 23);
            cmbOrderType.TabIndex = 16;
            // 
            // txtVolume
            // 
            txtVolume.Location = new Point(101, 153);
            txtVolume.Name = "txtVolume";
            txtVolume.Size = new Size(160, 23);
            txtVolume.TabIndex = 15;
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(101, 114);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(160, 23);
            txtPrice.TabIndex = 14;
            // 
            // cmbMarket
            // 
            cmbMarket.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMarket.FormattingEnabled = true;
            cmbMarket.Location = new Point(101, 45);
            cmbMarket.Name = "cmbMarket";
            cmbMarket.Size = new Size(160, 23);
            cmbMarket.TabIndex = 13;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(txtOpenAIAPIKey);
            tabPage4.Controls.Add(label5);
            tabPage4.Controls.Add(lblOpenAIResponse);
            tabPage4.Controls.Add(btnOpenAI);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(792, 422);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "OpenAI";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // lblOpenAIResponse
            // 
            lblOpenAIResponse.AutoSize = true;
            lblOpenAIResponse.Location = new Point(8, 165);
            lblOpenAIResponse.Name = "lblOpenAIResponse";
            lblOpenAIResponse.Size = new Size(39, 15);
            lblOpenAIResponse.TabIndex = 1;
            lblOpenAIResponse.Text = "label5";
            // 
            // btnOpenAI
            // 
            btnOpenAI.Location = new Point(8, 68);
            btnOpenAI.Name = "btnOpenAI";
            btnOpenAI.Size = new Size(194, 71);
            btnOpenAI.TabIndex = 0;
            btnOpenAI.Text = "button1";
            btnOpenAI.UseVisualStyleBackColor = true;
            btnOpenAI.Click += btnOpenAI_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 16);
            label5.Name = "label5";
            label5.Size = new Size(26, 15);
            label5.TabIndex = 2;
            label5.Text = "Key";
            // 
            // txtOpenAIAPIKey
            // 
            txtOpenAIAPIKey.Location = new Point(40, 13);
            txtOpenAIAPIKey.Name = "txtOpenAIAPIKey";
            txtOpenAIAPIKey.PasswordChar = '*';
            txtOpenAIAPIKey.Size = new Size(724, 23);
            txtOpenAIAPIKey.TabIndex = 3;
            txtOpenAIAPIKey.TextChanged += txtOpenAIAPIKey_TextChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "MainForm";
            Text = "Main";
            Load += MainForm_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMarketInfo).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrderHistory).EndInit();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private DataGridView dgvAccounts;
        private TabPage tabPage2;
        private DataGridView dgvMarketInfo;
        private TabPage tabPage3;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private RadioButton rdo_Sell;
        private RadioButton rdoBuy;
        private Button btnPlaceOrder;
        private ComboBox cmbOrderType;
        private TextBox txtVolume;
        private TextBox txtPrice;
        private ComboBox cmbMarket;
        private Label lblStatus;
        private DataGridView dgvOrderHistory;
        private TabPage tabPage4;
        private Label lblOpenAIResponse;
        private Button btnOpenAI;
        private TextBox txtOpenAIAPIKey;
        private Label label5;
    }
}