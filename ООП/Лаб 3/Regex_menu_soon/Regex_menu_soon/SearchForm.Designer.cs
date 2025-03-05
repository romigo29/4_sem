namespace Bank
{
	partial class SearchForm
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
			this.ChooseCriteria = new System.Windows.Forms.Label();
			this.CriteriaBox = new System.Windows.Forms.ComboBox();
			this.QueryInput = new System.Windows.Forms.RichTextBox();
			this.SearchButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ChooseCriteria
			// 
			this.ChooseCriteria.AutoSize = true;
			this.ChooseCriteria.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ChooseCriteria.Location = new System.Drawing.Point(12, 61);
			this.ChooseCriteria.Name = "ChooseCriteria";
			this.ChooseCriteria.Size = new System.Drawing.Size(198, 25);
			this.ChooseCriteria.TabIndex = 0;
			this.ChooseCriteria.Text = "Выберите критерий";
			// 
			// CriteriaBox
			// 
			this.CriteriaBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.CriteriaBox.FormattingEnabled = true;
			this.CriteriaBox.Items.AddRange(new object[] {
            "Номер счета",
            "ФИО",
            "Паспортные данные"});
			this.CriteriaBox.Location = new System.Drawing.Point(256, 58);
			this.CriteriaBox.Name = "CriteriaBox";
			this.CriteriaBox.Size = new System.Drawing.Size(121, 33);
			this.CriteriaBox.TabIndex = 2;
			// 
			// QueryInput
			// 
			this.QueryInput.Location = new System.Drawing.Point(17, 134);
			this.QueryInput.Name = "QueryInput";
			this.QueryInput.Size = new System.Drawing.Size(360, 96);
			this.QueryInput.TabIndex = 3;
			this.QueryInput.Text = "Введите запрос";
			// 
			// SearchButton
			// 
			this.SearchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.SearchButton.Location = new System.Drawing.Point(440, 162);
			this.SearchButton.Name = "SearchButton";
			this.SearchButton.Size = new System.Drawing.Size(89, 39);
			this.SearchButton.TabIndex = 4;
			this.SearchButton.Text = "Поиск";
			this.SearchButton.UseVisualStyleBackColor = true;
			this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
			// 
			// SearchForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.SearchButton);
			this.Controls.Add(this.QueryInput);
			this.Controls.Add(this.CriteriaBox);
			this.Controls.Add(this.ChooseCriteria);
			this.Name = "SearchForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label ChooseCriteria;
		private System.Windows.Forms.ComboBox CriteriaBox;
		private System.Windows.Forms.RichTextBox QueryInput;
		private System.Windows.Forms.Button SearchButton;
	}
}