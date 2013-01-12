namespace Client
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ownerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description4DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotDimensionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zoningDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description3DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acreageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billingAddrDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parcelPropertyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parcelPropertyBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 161);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "查询地块";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Address:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(129, 27);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(324, 22);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "601 North 5th Street";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(463, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sheboygan, WI";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 78);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Parcel Type:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(129, 74);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(324, 22);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "EXM";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 121);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Buffer Distance:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(129, 117);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(324, 22);
            this.textBox3.TabIndex = 8;
            this.textBox3.Text = "0.1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ownerDataGridViewTextBoxColumn,
            this.description2DataGridViewTextBoxColumn,
            this.description4DataGridViewTextBoxColumn,
            this.lotSizeDataGridViewTextBoxColumn,
            this.lotDimensionDataGridViewTextBoxColumn,
            this.zoningDataGridViewTextBoxColumn,
            this.description1DataGridViewTextBoxColumn,
            this.iDDataGridViewTextBoxColumn,
            this.description3DataGridViewTextBoxColumn,
            this.acreageDataGridViewTextBoxColumn,
            this.billingAddrDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.parcelPropertyBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(-39, 254);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1484, 468);
            this.dataGridView1.TabIndex = 9;
            // 
            // ownerDataGridViewTextBoxColumn
            // 
            this.ownerDataGridViewTextBoxColumn.DataPropertyName = "Owner";
            this.ownerDataGridViewTextBoxColumn.HeaderText = "Owner";
            this.ownerDataGridViewTextBoxColumn.Name = "ownerDataGridViewTextBoxColumn";
            // 
            // description2DataGridViewTextBoxColumn
            // 
            this.description2DataGridViewTextBoxColumn.DataPropertyName = "Description2";
            this.description2DataGridViewTextBoxColumn.HeaderText = "Description2";
            this.description2DataGridViewTextBoxColumn.Name = "description2DataGridViewTextBoxColumn";
            // 
            // description4DataGridViewTextBoxColumn
            // 
            this.description4DataGridViewTextBoxColumn.DataPropertyName = "Description4";
            this.description4DataGridViewTextBoxColumn.HeaderText = "Description4";
            this.description4DataGridViewTextBoxColumn.Name = "description4DataGridViewTextBoxColumn";
            // 
            // lotSizeDataGridViewTextBoxColumn
            // 
            this.lotSizeDataGridViewTextBoxColumn.DataPropertyName = "LotSize";
            this.lotSizeDataGridViewTextBoxColumn.HeaderText = "LotSize";
            this.lotSizeDataGridViewTextBoxColumn.Name = "lotSizeDataGridViewTextBoxColumn";
            // 
            // lotDimensionDataGridViewTextBoxColumn
            // 
            this.lotDimensionDataGridViewTextBoxColumn.DataPropertyName = "LotDimension";
            this.lotDimensionDataGridViewTextBoxColumn.HeaderText = "LotDimension";
            this.lotDimensionDataGridViewTextBoxColumn.Name = "lotDimensionDataGridViewTextBoxColumn";
            // 
            // zoningDataGridViewTextBoxColumn
            // 
            this.zoningDataGridViewTextBoxColumn.DataPropertyName = "Zoning";
            this.zoningDataGridViewTextBoxColumn.HeaderText = "Zoning";
            this.zoningDataGridViewTextBoxColumn.Name = "zoningDataGridViewTextBoxColumn";
            // 
            // description1DataGridViewTextBoxColumn
            // 
            this.description1DataGridViewTextBoxColumn.DataPropertyName = "Description1";
            this.description1DataGridViewTextBoxColumn.HeaderText = "Description1";
            this.description1DataGridViewTextBoxColumn.Name = "description1DataGridViewTextBoxColumn";
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            // 
            // description3DataGridViewTextBoxColumn
            // 
            this.description3DataGridViewTextBoxColumn.DataPropertyName = "Description3";
            this.description3DataGridViewTextBoxColumn.HeaderText = "Description3";
            this.description3DataGridViewTextBoxColumn.Name = "description3DataGridViewTextBoxColumn";
            // 
            // acreageDataGridViewTextBoxColumn
            // 
            this.acreageDataGridViewTextBoxColumn.DataPropertyName = "Acreage";
            this.acreageDataGridViewTextBoxColumn.HeaderText = "Acreage";
            this.acreageDataGridViewTextBoxColumn.Name = "acreageDataGridViewTextBoxColumn";
            // 
            // billingAddrDataGridViewTextBoxColumn
            // 
            this.billingAddrDataGridViewTextBoxColumn.DataPropertyName = "BillingAddr";
            this.billingAddrDataGridViewTextBoxColumn.HeaderText = "BillingAddr";
            this.billingAddrDataGridViewTextBoxColumn.Name = "billingAddrDataGridViewTextBoxColumn";
            // 
            // parcelPropertyBindingSource
            // 
            this.parcelPropertyBindingSource.DataSource = typeof(Client.localhost.ParcelProperty);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1404, 778);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "MapGuide网络服务客户端";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parcelPropertyBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ownerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn description2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn description4DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotDimensionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn zoningDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn description1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn description3DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn acreageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billingAddrDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource parcelPropertyBindingSource;




    }
}

