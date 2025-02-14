namespace WinForms
{
	partial class ProductCalculator
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductCalculator));
			this.Header = new System.Windows.Forms.Label();
			this.Output = new System.Windows.Forms.RichTextBox();
			this.ValuePerWeight = new System.Windows.Forms.Button();
			this.CostPrice = new System.Windows.Forms.Button();
			this.MonthUsages = new System.Windows.Forms.Button();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Goods = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DailyUsage = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Add = new System.Windows.Forms.Button();
			this.Clear = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// Header
			// 
			resources.ApplyResources(this.Header, "Header");
			this.Header.Name = "Header";
			// 
			// Output
			// 
			resources.ApplyResources(this.Output, "Output");
			this.Output.Name = "Output";
			// 
			// ValuePerWeight
			// 
			resources.ApplyResources(this.ValuePerWeight, "ValuePerWeight");
			this.ValuePerWeight.Name = "ValuePerWeight";
			this.ValuePerWeight.UseVisualStyleBackColor = true;
			this.ValuePerWeight.Click += new System.EventHandler(this.buttonValuePerWeight_Click);
			// 
			// CostPrice
			// 
			resources.ApplyResources(this.CostPrice, "CostPrice");
			this.CostPrice.Name = "CostPrice";
			this.CostPrice.UseVisualStyleBackColor = true;
			this.CostPrice.Click += new System.EventHandler(this.buttonCostPrice_Click);
			// 
			// MonthUsages
			// 
			resources.ApplyResources(this.MonthUsages, "MonthUsages");
			this.MonthUsages.Name = "MonthUsages";
			this.MonthUsages.UseVisualStyleBackColor = true;
			this.MonthUsages.Click += new System.EventHandler(this.ButtonMonthUsages_Click);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Goods,
            this.Amount,
            this.Value,
            this.DailyUsage});
			resources.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
			// 
			// Goods
			// 
			resources.ApplyResources(this.Goods, "Goods");
			this.Goods.Name = "Goods";
			// 
			// Amount
			// 
			resources.ApplyResources(this.Amount, "Amount");
			this.Amount.Name = "Amount";
			// 
			// Value
			// 
			resources.ApplyResources(this.Value, "Value");
			this.Value.Name = "Value";
			// 
			// DailyUsage
			// 
			resources.ApplyResources(this.DailyUsage, "DailyUsage");
			this.DailyUsage.Name = "DailyUsage";
			// 
			// Add
			// 
			resources.ApplyResources(this.Add, "Add");
			this.Add.Name = "Add";
			this.Add.UseVisualStyleBackColor = true;
			this.Add.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// Clear
			// 
			resources.ApplyResources(this.Clear, "Clear");
			this.Clear.Name = "Clear";
			this.Clear.UseVisualStyleBackColor = true;
			this.Clear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// ProductCalculator
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.Clear);
			this.Controls.Add(this.Add);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.MonthUsages);
			this.Controls.Add(this.CostPrice);
			this.Controls.Add(this.ValuePerWeight);
			this.Controls.Add(this.Output);
			this.Controls.Add(this.Header);
			this.Name = "ProductCalculator";
			this.Load += new System.EventHandler(this.ProductCalculator_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label Header;
		private System.Windows.Forms.RichTextBox Output;
		private System.Windows.Forms.Button ValuePerWeight;
		private System.Windows.Forms.Button CostPrice;
		private System.Windows.Forms.Button MonthUsages;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button Add;
		private System.Windows.Forms.Button Clear;
		private System.Windows.Forms.DataGridViewTextBoxColumn Goods;
		private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
		private System.Windows.Forms.DataGridViewTextBoxColumn Value;
		private System.Windows.Forms.DataGridViewTextBoxColumn DailyUsage;
	}
}