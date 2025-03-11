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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bank));
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
			this.AccountNumber = new System.Windows.Forms.MaskedTextBox();
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
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.SaveAsJsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.сохранитьРезультатыПоискToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.сохранитьРезлуьтатыСортировкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.статусToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.количествоКлиентовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.текущееВремяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.текущаяДатаToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.информацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.последнееДействиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.отобразитьПанельToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DownButton = new System.Windows.Forms.ToolStripButton();
			this.BackButton = new System.Windows.Forms.ToolStripButton();
			this.BottomToolPannel = new System.Windows.Forms.ToolStrip();
			this.SearchButton = new System.Windows.Forms.ToolStripLabel();
			this.SortByButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.датаРожденияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.фИОToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.номеруСчетаToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.ClearButton = new System.Windows.Forms.ToolStripLabel();
			this.DeleteButton = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.HidePanel = new System.Windows.Forms.ToolStripButton();
			this.PinPannel = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this.Balance)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.BottomToolPannel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(343, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(327, 29);
			this.label1.TabIndex = 0;
			this.label1.Text = "Регистрация клиента банка";
			// 
			// FullName
			// 
			this.FullName.Location = new System.Drawing.Point(252, 83);
			this.FullName.Name = "FullName";
			this.FullName.Size = new System.Drawing.Size(242, 22);
			this.FullName.TabIndex = 1;
			this.FullName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FullName_KeyPress);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(170, 83);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 25);
			this.label2.TabIndex = 2;
			this.label2.Text = "ФИО";
			// 
			// PassportData
			// 
			this.PassportData.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.PassportData.Location = new System.Drawing.Point(252, 116);
			this.PassportData.Mask = "LL0000000";
			this.PassportData.Name = "PassportData";
			this.PassportData.Size = new System.Drawing.Size(242, 22);
			this.PassportData.TabIndex = 3;
			// 
			// Passport
			// 
			this.Passport.AutoSize = true;
			this.Passport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Passport.Location = new System.Drawing.Point(40, 134);
			this.Passport.Name = "Passport";
			this.Passport.Size = new System.Drawing.Size(0, 25);
			this.Passport.TabIndex = 4;
			// 
			// Opening
			// 
			this.Opening.Location = new System.Drawing.Point(252, 325);
			this.Opening.Name = "Opening";
			this.Opening.Size = new System.Drawing.Size(242, 22);
			this.Opening.TabIndex = 5;
			// 
			// OpeningDate
			// 
			this.OpeningDate.AutoSize = true;
			this.OpeningDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.OpeningDate.Location = new System.Drawing.Point(30, 325);
			this.OpeningDate.Name = "OpeningDate";
			this.OpeningDate.Size = new System.Drawing.Size(200, 24);
			this.OpeningDate.TabIndex = 6;
			this.OpeningDate.Text = "Дата открытия счета";
			// 
			// BirthDate
			// 
			this.BirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.BirthDate.Location = new System.Drawing.Point(252, 159);
			this.BirthDate.Name = "BirthDate";
			this.BirthDate.Size = new System.Drawing.Size(242, 22);
			this.BirthDate.TabIndex = 7;
			this.BirthDate.Value = new System.DateTime(2025, 2, 21, 17, 43, 38, 0);
			this.BirthDate.Validating += new System.ComponentModel.CancelEventHandler(this.BirthDate_Validating);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(69, 159);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(161, 25);
			this.label3.TabIndex = 8;
			this.label3.Text = "Дата рождения";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.label4.Location = new System.Drawing.Point(105, 202);
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
			this.AccountType.Location = new System.Drawing.Point(252, 244);
			this.AccountType.Name = "AccountType";
			this.AccountType.Size = new System.Drawing.Size(242, 24);
			this.AccountType.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.label5.Location = new System.Drawing.Point(130, 244);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 24);
			this.label5.TabIndex = 12;
			this.label5.Text = "Тип счета";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.label6.Location = new System.Drawing.Point(156, 287);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(74, 24);
			this.label6.TabIndex = 13;
			this.label6.Text = "Баланс";
			// 
			// Balance
			// 
			this.Balance.DecimalPlaces = 2;
			this.Balance.Location = new System.Drawing.Point(252, 288);
			this.Balance.Name = "Balance";
			this.Balance.Size = new System.Drawing.Size(242, 22);
			this.Balance.TabIndex = 14;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label7.Location = new System.Drawing.Point(29, 116);
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
			this.groupBox1.Location = new System.Drawing.Point(507, 83);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(382, 105);
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
			this.Confirm.Location = new System.Drawing.Point(534, 202);
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
			this.ReadFile.Location = new System.Drawing.Point(455, 607);
			this.ReadFile.Name = "ReadFile";
			this.ReadFile.Size = new System.Drawing.Size(398, 38);
			this.ReadFile.TabIndex = 19;
			this.ReadFile.Text = "Прочитать из JSON-файла";
			this.ReadFile.UseVisualStyleBackColor = true;
			this.ReadFile.Click += new System.EventHandler(this.FileRead_Click);
			// 
			// WriteFile
			// 
			this.WriteFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.WriteFile.Location = new System.Drawing.Point(74, 607);
			this.WriteFile.Name = "WriteFile";
			this.WriteFile.Size = new System.Drawing.Size(352, 38);
			this.WriteFile.TabIndex = 20;
			this.WriteFile.Text = "Записать в JSON-файл";
			this.WriteFile.UseVisualStyleBackColor = true;
			this.WriteFile.Click += new System.EventHandler(this.WriteFile_Click);
			// 
			// Budget
			// 
			this.Budget.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			this.Budget.Location = new System.Drawing.Point(545, 334);
			this.Budget.Name = "Budget";
			this.Budget.Size = new System.Drawing.Size(308, 38);
			this.Budget.TabIndex = 21;
			this.Budget.Text = "Рассчитать бюджет";
			this.Budget.UseVisualStyleBackColor = true;
			this.Budget.Click += new System.EventHandler(this.Budget_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label8.Location = new System.Drawing.Point(40, 382);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(179, 25);
			this.label8.TabIndex = 22;
			this.label8.Text = "Исходные данные";
			// 
			// AccountNumber
			// 
			this.AccountNumber.Location = new System.Drawing.Point(252, 202);
			this.AccountNumber.Mask = "00000000";
			this.AccountNumber.Name = "AccountNumber";
			this.AccountNumber.Size = new System.Drawing.Size(242, 22);
			this.AccountNumber.TabIndex = 9;
			this.AccountNumber.ValidatingType = typeof(int);
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
			this.dataGridView1.Location = new System.Drawing.Point(34, 421);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersWidth = 51;
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.Size = new System.Drawing.Size(819, 168);
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
			this.OutputBudget.Location = new System.Drawing.Point(548, 271);
			this.OutputBudget.Name = "OutputBudget";
			this.OutputBudget.Size = new System.Drawing.Size(308, 57);
			this.OutputBudget.TabIndex = 25;
			this.OutputBudget.Text = "";
			// 
			// toolTip1
			// 
			this.toolTip1.ShowAlways = true;
			this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveAsJsonToolStripMenuItem,
            this.статусToolStripMenuItem,
            this.справкаToolStripMenuItem,
            this.отобразитьПанельToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(953, 30);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// SaveAsJsonToolStripMenuItem
			// 
			this.SaveAsJsonToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьРезультатыПоискToolStripMenuItem,
            this.сохранитьРезлуьтатыСортировкиToolStripMenuItem});
			this.SaveAsJsonToolStripMenuItem.Name = "SaveAsJsonToolStripMenuItem";
			this.SaveAsJsonToolStripMenuItem.Size = new System.Drawing.Size(97, 26);
			this.SaveAsJsonToolStripMenuItem.Text = "&Сохранить";
			// 
			// сохранитьРезультатыПоискToolStripMenuItem
			// 
			this.сохранитьРезультатыПоискToolStripMenuItem.Name = "сохранитьРезультатыПоискToolStripMenuItem";
			this.сохранитьРезультатыПоискToolStripMenuItem.Size = new System.Drawing.Size(334, 26);
			this.сохранитьРезультатыПоискToolStripMenuItem.Text = "Сохранить результаты поиска";
			this.сохранитьРезультатыПоискToolStripMenuItem.Click += new System.EventHandler(this.сохранитьРезультатыПоискToolStripMenuItem_Click);
			// 
			// сохранитьРезлуьтатыСортировкиToolStripMenuItem
			// 
			this.сохранитьРезлуьтатыСортировкиToolStripMenuItem.Name = "сохранитьРезлуьтатыСортировкиToolStripMenuItem";
			this.сохранитьРезлуьтатыСортировкиToolStripMenuItem.Size = new System.Drawing.Size(334, 26);
			this.сохранитьРезлуьтатыСортировкиToolStripMenuItem.Text = "Сохранить резлуьтаты сортировки";
			this.сохранитьРезлуьтатыСортировкиToolStripMenuItem.Click += new System.EventHandler(this.сохранитьРезультатыСортировкиToolStripMenuItem_Click);
			// 
			// статусToolStripMenuItem
			// 
			this.статусToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.количествоКлиентовToolStripMenuItem,
            this.текущееВремяToolStripMenuItem,
            this.текущаяДатаToolStripMenuItem1});
			this.статусToolStripMenuItem.Name = "статусToolStripMenuItem";
			this.статусToolStripMenuItem.Size = new System.Drawing.Size(66, 26);
			this.статусToolStripMenuItem.Text = "Статус";
			// 
			// количествоКлиентовToolStripMenuItem
			// 
			this.количествоКлиентовToolStripMenuItem.Name = "количествоКлиентовToolStripMenuItem";
			this.количествоКлиентовToolStripMenuItem.Size = new System.Drawing.Size(241, 26);
			this.количествоКлиентовToolStripMenuItem.Text = "Количество клиентов";
			this.количествоКлиентовToolStripMenuItem.Click += new System.EventHandler(this.количествоКлиентовToolStripMenuItem_Click);
			// 
			// текущееВремяToolStripMenuItem
			// 
			this.текущееВремяToolStripMenuItem.Name = "текущееВремяToolStripMenuItem";
			this.текущееВремяToolStripMenuItem.Size = new System.Drawing.Size(241, 26);
			this.текущееВремяToolStripMenuItem.Text = "Текущее время";
			this.текущееВремяToolStripMenuItem.Click += new System.EventHandler(this.текущееВремяToolStripMenuItem_Click);
			// 
			// текущаяДатаToolStripMenuItem1
			// 
			this.текущаяДатаToolStripMenuItem1.Name = "текущаяДатаToolStripMenuItem1";
			this.текущаяДатаToolStripMenuItem1.Size = new System.Drawing.Size(241, 26);
			this.текущаяДатаToolStripMenuItem1.Text = "Текущая дата";
			this.текущаяДатаToolStripMenuItem1.Click += new System.EventHandler(this.текущаяДатаToolStripMenuItem1_Click);
			// 
			// справкаToolStripMenuItem
			// 
			this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.информацияToolStripMenuItem,
            this.toolStripSeparator2,
            this.последнееДействиеToolStripMenuItem});
			this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
			this.справкаToolStripMenuItem.Size = new System.Drawing.Size(118, 26);
			this.справкаToolStripMenuItem.Text = "О &программе";
			// 
			// информацияToolStripMenuItem
			// 
			this.информацияToolStripMenuItem.Name = "информацияToolStripMenuItem";
			this.информацияToolStripMenuItem.Size = new System.Drawing.Size(235, 26);
			this.информацияToolStripMenuItem.Text = "Информация";
			this.информацияToolStripMenuItem.Click += new System.EventHandler(this.информацияToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(232, 6);
			// 
			// последнееДействиеToolStripMenuItem
			// 
			this.последнееДействиеToolStripMenuItem.Name = "последнееДействиеToolStripMenuItem";
			this.последнееДействиеToolStripMenuItem.Size = new System.Drawing.Size(235, 26);
			this.последнееДействиеToolStripMenuItem.Text = "Последнее действие";
			this.последнееДействиеToolStripMenuItem.Click += new System.EventHandler(this.последнееДействиеToolStripMenuItem_Click);
			// 
			// отобразитьПанельToolStripMenuItem
			// 
			this.отобразитьПанельToolStripMenuItem.Name = "отобразитьПанельToolStripMenuItem";
			this.отобразитьПанельToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
			this.отобразитьПанельToolStripMenuItem.Text = "Отобразить панель";
			this.отобразитьПанельToolStripMenuItem.Visible = false;
			this.отобразитьПанельToolStripMenuItem.Click += new System.EventHandler(this.отобразитьПанельToolStripMenuItem_Click);
			// 
			// DownButton
			// 
			this.DownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.DownButton.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.DownButton.Image = ((System.Drawing.Image)(resources.GetObject("DownButton.Image")));
			this.DownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DownButton.Name = "DownButton";
			this.DownButton.Size = new System.Drawing.Size(29, 24);
			this.DownButton.Text = "NextButton";
			this.DownButton.Click += new System.EventHandler(this.DownButtonButton_Click);
			// 
			// BackButton
			// 
			this.BackButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.BackButton.Image = ((System.Drawing.Image)(resources.GetObject("BackButton.Image")));
			this.BackButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.BackButton.Name = "BackButton";
			this.BackButton.Size = new System.Drawing.Size(29, 24);
			this.BackButton.Text = "UpButton";
			this.BackButton.Click += new System.EventHandler(this.UpButton_Click);
			// 
			// BottomToolPannel
			// 
			this.BottomToolPannel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.BottomToolPannel.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.BottomToolPannel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SearchButton,
            this.SortByButton,
            this.ClearButton,
            this.DeleteButton,
            this.BackButton,
            this.DownButton,
            this.toolStripSeparator3,
            this.HidePanel,
            this.PinPannel});
			this.BottomToolPannel.Location = new System.Drawing.Point(0, 665);
			this.BottomToolPannel.Name = "BottomToolPannel";
			this.BottomToolPannel.Padding = new System.Windows.Forms.Padding(10, 5, 0, 10);
			this.BottomToolPannel.Size = new System.Drawing.Size(953, 42);
			this.BottomToolPannel.TabIndex = 26;
			this.BottomToolPannel.Text = "toolStrip1";
			// 
			// SearchButton
			// 
			this.SearchButton.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.SearchButton.Image = ((System.Drawing.Image)(resources.GetObject("SearchButton.Image")));
			this.SearchButton.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
			this.SearchButton.Name = "SearchButton";
			this.SearchButton.Size = new System.Drawing.Size(72, 24);
			this.SearchButton.Text = "Поиск";
			this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
			// 
			// SortByButton
			// 
			this.SortByButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.датаРожденияToolStripMenuItem,
            this.фИОToolStripMenuItem1,
            this.номеруСчетаToolStripMenuItem1});
			this.SortByButton.Image = ((System.Drawing.Image)(resources.GetObject("SortByButton.Image")));
			this.SortByButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.SortByButton.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
			this.SortByButton.Name = "SortByButton";
			this.SortByButton.Size = new System.Drawing.Size(126, 24);
			this.SortByButton.Text = "Сортировка";
			// 
			// датаРожденияToolStripMenuItem
			// 
			this.датаРожденияToolStripMenuItem.Name = "датаРожденияToolStripMenuItem";
			this.датаРожденияToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
			this.датаРожденияToolStripMenuItem.Text = "дата рождения";
			this.датаРожденияToolStripMenuItem.Click += new System.EventHandler(this.датаРожденияToolStripMenuItem_Click);
			// 
			// фИОToolStripMenuItem1
			// 
			this.фИОToolStripMenuItem1.Name = "фИОToolStripMenuItem1";
			this.фИОToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
			this.фИОToolStripMenuItem1.Text = "ФИО";
			this.фИОToolStripMenuItem1.Click += new System.EventHandler(this.фИОToolStripMenuItem1_Click);
			// 
			// номеруСчетаToolStripMenuItem1
			// 
			this.номеруСчетаToolStripMenuItem1.Name = "номеруСчетаToolStripMenuItem1";
			this.номеруСчетаToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
			this.номеруСчетаToolStripMenuItem1.Text = "номер счета";
			this.номеруСчетаToolStripMenuItem1.Click += new System.EventHandler(this.номеруСчетаToolStripMenuItem1_Click);
			// 
			// ClearButton
			// 
			this.ClearButton.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.ClearButton.Image = ((System.Drawing.Image)(resources.GetObject("ClearButton.Image")));
			this.ClearButton.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
			this.ClearButton.Name = "ClearButton";
			this.ClearButton.Size = new System.Drawing.Size(93, 24);
			this.ClearButton.Text = "Очистить";
			this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
			// 
			// DeleteButton
			// 
			this.DeleteButton.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
			this.DeleteButton.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(85, 24);
			this.DeleteButton.Text = "Удалить";
			this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
			// 
			// HidePanel
			// 
			this.HidePanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.HidePanel.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.HidePanel.Image = ((System.Drawing.Image)(resources.GetObject("HidePanel.Image")));
			this.HidePanel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.HidePanel.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
			this.HidePanel.Name = "HidePanel";
			this.HidePanel.Size = new System.Drawing.Size(117, 24);
			this.HidePanel.Text = "Скрыть панель";
			this.HidePanel.Click += new System.EventHandler(this.HidePanel_Click);
			// 
			// PinPannel
			// 
			this.PinPannel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.PinPannel.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.PinPannel.Image = ((System.Drawing.Image)(resources.GetObject("PinPannel.Image")));
			this.PinPannel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.PinPannel.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
			this.PinPannel.Name = "PinPannel";
			this.PinPannel.Size = new System.Drawing.Size(141, 24);
			this.PinPannel.Text = "Закрепить_панель";
			this.PinPannel.Click += new System.EventHandler(this.PinPannel_Click);
			// 
			// Bank
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Info;
			this.ClientSize = new System.Drawing.Size(953, 707);
			this.Controls.Add(this.BottomToolPannel);
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
			this.Controls.Add(this.AccountNumber);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.BirthDate);
			this.Controls.Add(this.OpeningDate);
			this.Controls.Add(this.Opening);
			this.Controls.Add(this.Passport);
			this.Controls.Add(this.PassportData);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.FullName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Bank";
			this.Text = "Bank";
			this.Load += new System.EventHandler(this.Bank_Load);
			((System.ComponentModel.ISupportInitialize)(this.Balance)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.BottomToolPannel.ResumeLayout(false);
			this.BottomToolPannel.PerformLayout();
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
		private System.Windows.Forms.MaskedTextBox AccountNumber;
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
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem информацияToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton DownButton;
		private System.Windows.Forms.ToolStripButton BackButton;
		private System.Windows.Forms.ToolStrip BottomToolPannel;
		private System.Windows.Forms.ToolStripLabel ClearButton;
		private System.Windows.Forms.ToolStripLabel DeleteButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton HidePanel;
		private System.Windows.Forms.ToolStripButton PinPannel;
		private System.Windows.Forms.ToolStripMenuItem отобразитьПанельToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SaveAsJsonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem сохранитьРезультатыПоискToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem сохранитьРезлуьтатыСортировкиToolStripMenuItem;
		private System.Windows.Forms.ToolStripLabel SearchButton;
		private System.Windows.Forms.ToolStripDropDownButton SortByButton;
		private System.Windows.Forms.ToolStripMenuItem датаРожденияToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem фИОToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem номеруСчетаToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem статусToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem количествоКлиентовToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem текущееВремяToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem последнееДействиеToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem текущаяДатаToolStripMenuItem1;
	}
}