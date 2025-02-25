namespace Bank
{
	partial class Bank
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
			this.label1 = new System.Windows.Forms.Label();
			this.FullName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.PassportData = new System.Windows.Forms.MaskedTextBox();
			this.Passport = new System.Windows.Forms.Label();
			this.Opening = new System.Windows.Forms.DateTimePicker();
			this.OpeningDate = new System.Windows.Forms.Label();
			this.BirthDate = new System.Windows.Forms.DateTimePicker();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.AccountType = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.Balance = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.InternetBanking = new System.Windows.Forms.CheckBox();
			this.SMS = new System.Windows.Forms.CheckBox();
			this.Confirm = new System.Windows.Forms.Button();
			this.ReadFile = new System.Windows.Forms.Button();
			this.WriteFile = new System.Windows.Forms.Button();
			this.Budget = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.Account = new System.Windows.Forms.MaskedTextBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.FIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PassportInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Birth = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AccountNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Account_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CurrrentBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Дата_открытия = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SMSON = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Banking = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.OutputBudget = new System.Windows.Forms.RichTextBox();
			((System.ComponentModel.ISupportInitialize)(this.Balance)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(256, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(327, 29);
			this.label1.TabIndex = 0;
			this.label1.Text = "Регистрация клиента банка";
			// 
			// FullName
			// 
			this.FullName.Location = new System.Drawing.Point(456, 54);
			this.FullName.Name = "FullName";
			this.FullName.Size = new System.Drawing.Size(242, 22);
			this.FullName.TabIndex = 1;
			this.FullName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FullName_KeyPress);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(374, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 25);
			this.label2.TabIndex = 2;
			this.label2.Text = "ФИО";
			// 
			// PassportData
			// 
			this.PassportData.Location = new System.Drawing.Point(456, 87);
			this.PassportData.Mask = "000-00-0000";
			this.PassportData.Name = "PassportData";
			this.PassportData.Size = new System.Drawing.Size(242, 22);
			this.PassportData.TabIndex = 3;
			this.PassportData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PassportData_KeyPress);
			// 
			// Passport
			// 
			this.Passport.AutoSize = true;
			this.Passport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Passport.Location = new System.Drawing.Point(244, 105);
			this.Passport.Name = "Passport";
			this.Passport.Size = new System.Drawing.Size(0, 25);
			this.Passport.TabIndex = 4;
			// 
			// Opening
			// 
			this.Opening.Location = new System.Drawing.Point(456, 296);
			this.Opening.Name = "Opening";
			this.Opening.Size = new System.Drawing.Size(242, 22);
			this.Opening.TabIndex = 5;
			// 
			// OpeningDate
			// 
			this.OpeningDate.AutoSize = true;
			this.OpeningDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.OpeningDate.Location = new System.Drawing.Point(234, 296);
			this.OpeningDate.Name = "OpeningDate";
			this.OpeningDate.Size = new System.Drawing.Size(200, 24);
			this.OpeningDate.TabIndex = 6;
			this.OpeningDate.Text = "Дата открытия счета";
			// 
			// BirthDate
			// 
			this.BirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.BirthDate.Location = new System.Drawing.Point(456, 130);
			this.BirthDate.Name = "BirthDate";
			this.BirthDate.Size = new System.Drawing.Size(242, 22);
			this.BirthDate.TabIndex = 7;
			this.BirthDate.Value = new System.DateTime(2025, 2, 21, 17, 43, 38, 0);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(273, 130);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(161, 25);
			this.label3.TabIndex = 8;
			this.label3.Text = "Дата рождения";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.label4.Location = new System.Drawing.Point(309, 173);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(125, 24);
			this.label4.TabIndex = 10;
			this.label4.Text = "Номер счета";
			// 
			// AccountType
			// 
			this.AccountType.FormattingEnabled = true;
			this.AccountType.Items.AddRange(new object[] {
            "Срочный",
            "Пополняемый",
            "До востребования"});
			this.AccountType.Location = new System.Drawing.Point(456, 215);
			this.AccountType.Name = "AccountType";
			this.AccountType.Size = new System.Drawing.Size(242, 24);
			this.AccountType.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.label5.Location = new System.Drawing.Point(334, 215);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 24);
			this.label5.TabIndex = 12;
			this.label5.Text = "Тип счета";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.label6.Location = new System.Drawing.Point(360, 258);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(74, 24);
			this.label6.TabIndex = 13;
			this.label6.Text = "Баланс";
			// 
			// Balance
			// 
			this.Balance.DecimalPlaces = 2;
			this.Balance.Location = new System.Drawing.Point(456, 259);
			this.Balance.Name = "Balance";
			this.Balance.Size = new System.Drawing.Size(242, 22);
			this.Balance.TabIndex = 14;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label7.Location = new System.Drawing.Point(233, 87);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(201, 25);
			this.label7.TabIndex = 15;
			this.label7.Text = "Паспортные данные";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.InternetBanking);
			this.groupBox1.Controls.Add(this.SMS);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.groupBox1.Location = new System.Drawing.Point(238, 338);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(398, 105);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Выберитие услуги для подключения";
			// 
			// InternetBanking
			// 
			this.InternetBanking.AutoSize = true;
			this.InternetBanking.Location = new System.Drawing.Point(27, 60);
			this.InternetBanking.Name = "InternetBanking";
			this.InternetBanking.Size = new System.Drawing.Size(185, 26);
			this.InternetBanking.TabIndex = 19;
			this.InternetBanking.Text = "Интернет-Банкинг";
			this.InternetBanking.UseVisualStyleBackColor = true;
			// 
			// SMS
			// 
			this.SMS.AutoSize = true;
			this.SMS.Location = new System.Drawing.Point(27, 27);
			this.SMS.Name = "SMS";
			this.SMS.Size = new System.Drawing.Size(183, 26);
			this.SMS.TabIndex = 18;
			this.SMS.Text = "СМС-оповещения";
			this.SMS.UseVisualStyleBackColor = true;
			// 
			// Confirm
			// 
			this.Confirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Confirm.Location = new System.Drawing.Point(349, 449);
			this.Confirm.Name = "Confirm";
			this.Confirm.Size = new System.Drawing.Size(330, 38);
			this.Confirm.TabIndex = 18;
			this.Confirm.Text = "Подтвердить данные";
			this.Confirm.UseVisualStyleBackColor = true;
			this.Confirm.Click += new System.EventHandler(this.ButtonFinish_Click);
			// 
			// ReadFile
			// 
			this.ReadFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.ReadFile.Location = new System.Drawing.Point(472, 693);
			this.ReadFile.Name = "ReadFile";
			this.ReadFile.Size = new System.Drawing.Size(398, 38);
			this.ReadFile.TabIndex = 19;
			this.ReadFile.Text = "Получить данные из файла";
			this.ReadFile.UseVisualStyleBackColor = true;
			this.ReadFile.Click += new System.EventHandler(this.FileRead_Click);
			// 
			// WriteFile
			// 
			this.WriteFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.WriteFile.Location = new System.Drawing.Point(119, 693);
			this.WriteFile.Name = "WriteFile";
			this.WriteFile.Size = new System.Drawing.Size(331, 38);
			this.WriteFile.TabIndex = 20;
			this.WriteFile.Text = "Записать данные в файл";
			this.WriteFile.UseVisualStyleBackColor = true;
			this.WriteFile.Click += new System.EventHandler(this.WriteFile_Click);
			// 
			// Budget
			// 
			this.Budget.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.Budget.Location = new System.Drawing.Point(393, 781);
			this.Budget.Name = "Budget";
			this.Budget.Size = new System.Drawing.Size(286, 38);
			this.Budget.TabIndex = 21;
			this.Budget.Text = "Рассчитать бюджет";
			this.Budget.UseVisualStyleBackColor = true;
			this.Budget.Click += new System.EventHandler(this.Budget_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label8.Location = new System.Drawing.Point(38, 489);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(179, 25);
			this.label8.TabIndex = 22;
			this.label8.Text = "Исходные данные";
			this.label8.Click += new System.EventHandler(this.label8_Click);
			// 
			// Account
			// 
			this.Account.Location = new System.Drawing.Point(456, 173);
			this.Account.Mask = "00000000";
			this.Account.Name = "Account";
			this.Account.Size = new System.Drawing.Size(242, 22);
			this.Account.TabIndex = 9;
			this.Account.ValidatingType = typeof(int);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FIO,
            this.PassportInfo,
            this.Birth,
            this.AccountNum,
            this.Account_Type,
            this.CurrrentBalance,
            this.Дата_открытия,
            this.SMSON,
            this.Banking});
			this.dataGridView1.Location = new System.Drawing.Point(14, 528);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersWidth = 51;
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.Size = new System.Drawing.Size(856, 150);
			this.dataGridView1.TabIndex = 23;
			// 
			// FIO
			// 
			this.FIO.HeaderText = "ФИО";
			this.FIO.MinimumWidth = 6;
			this.FIO.Name = "FIO";
			this.FIO.Width = 67;
			// 
			// PassportInfo
			// 
			this.PassportInfo.HeaderText = "Паспорт";
			this.PassportInfo.MinimumWidth = 6;
			this.PassportInfo.Name = "PassportInfo";
			this.PassportInfo.Width = 92;
			// 
			// Birth
			// 
			this.Birth.HeaderText = "Дата рождения";
			this.Birth.MinimumWidth = 6;
			this.Birth.Name = "Birth";
			this.Birth.Width = 124;
			// 
			// AccountNum
			// 
			this.AccountNum.HeaderText = "Номер счета";
			this.AccountNum.MinimumWidth = 6;
			this.AccountNum.Name = "AccountNum";
			this.AccountNum.Width = 110;
			// 
			// Account_Type
			// 
			this.Account_Type.HeaderText = "Тип счета";
			this.Account_Type.MinimumWidth = 6;
			this.Account_Type.Name = "Account_Type";
			this.Account_Type.Width = 94;
			// 
			// CurrrentBalance
			// 
			this.CurrrentBalance.HeaderText = "Баланс";
			this.CurrrentBalance.MinimumWidth = 6;
			this.CurrrentBalance.Name = "CurrrentBalance";
			this.CurrrentBalance.Width = 84;
			// 
			// Дата_открытия
			// 
			this.Дата_открытия.HeaderText = "Дата_открытия";
			this.Дата_открытия.MinimumWidth = 6;
			this.Дата_открытия.Name = "Дата_открытия";
			this.Дата_открытия.Width = 136;
			// 
			// SMSON
			// 
			this.SMSON.HeaderText = "Подключены СМС";
			this.SMSON.MinimumWidth = 6;
			this.SMSON.Name = "SMSON";
			this.SMSON.Width = 139;
			// 
			// Banking
			// 
			this.Banking.HeaderText = "Подключен Банкинг";
			this.Banking.MinimumWidth = 6;
			this.Banking.Name = "Banking";
			this.Banking.Width = 154;
			// 
			// OutputBudget
			// 
			this.OutputBudget.Location = new System.Drawing.Point(14, 749);
			this.OutputBudget.Name = "OutputBudget";
			this.OutputBudget.Size = new System.Drawing.Size(358, 106);
			this.OutputBudget.TabIndex = 25;
			this.OutputBudget.Text = "";
			// 
			// Bank
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(908, 880);
			this.Controls.Add(this.OutputBudget);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.Budget);
			this.Controls.Add(this.WriteFile);
			this.Controls.Add(this.ReadFile);
			this.Controls.Add(this.Confirm);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.Balance);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.AccountType);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Account);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.BirthDate);
			this.Controls.Add(this.OpeningDate);
			this.Controls.Add(this.Opening);
			this.Controls.Add(this.Passport);
			this.Controls.Add(this.PassportData);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.FullName);
			this.Controls.Add(this.label1);
			this.Name = "Bank";
			this.Text = "Bank";
			this.Load += new System.EventHandler(this.Bank_Load);
			((System.ComponentModel.ISupportInitialize)(this.Balance)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox FullName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.MaskedTextBox PassportData;
		private System.Windows.Forms.Label Passport;
		private System.Windows.Forms.DateTimePicker Opening;
		private System.Windows.Forms.Label OpeningDate;
		private System.Windows.Forms.DateTimePicker BirthDate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox AccountType;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown Balance;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox InternetBanking;
		private System.Windows.Forms.CheckBox SMS;
		private System.Windows.Forms.Button Confirm;
		private System.Windows.Forms.Button ReadFile;
		private System.Windows.Forms.Button WriteFile;
		private System.Windows.Forms.Button Budget;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.MaskedTextBox Account;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn FIO;
		private System.Windows.Forms.DataGridViewTextBoxColumn PassportInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn Birth;
		private System.Windows.Forms.DataGridViewTextBoxColumn AccountNum;
		private System.Windows.Forms.DataGridViewTextBoxColumn Account_Type;
		private System.Windows.Forms.DataGridViewTextBoxColumn CurrrentBalance;
		private System.Windows.Forms.DataGridViewTextBoxColumn Дата_открытия;
		private System.Windows.Forms.DataGridViewTextBoxColumn SMSON;
		private System.Windows.Forms.DataGridViewTextBoxColumn Banking;
		private System.Windows.Forms.RichTextBox OutputBudget;
	}
}