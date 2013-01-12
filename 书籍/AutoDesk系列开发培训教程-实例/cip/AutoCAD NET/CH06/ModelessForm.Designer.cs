namespace CH06
{
    partial class ModelessForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Label5 = new System.Windows.Forms.Label();
            this.lb_drop = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.tb_Salary = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.tb_Division = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.tb_Name = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Label5
            // 
            this.Label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(15, 42);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(160, 23);
            this.Label5.TabIndex = 39;
            this.Label5.Text = "员工信息";
            // 
            // lb_drop
            // 
            this.lb_drop.Location = new System.Drawing.Point(39, 300);
            this.lb_drop.Name = "lb_drop";
            this.lb_drop.Size = new System.Drawing.Size(74, 19);
            this.lb_drop.TabIndex = 38;
            this.lb_drop.Text = "拖拽创建";
            // 
            // Label3
            // 
            this.Label3.Location = new System.Drawing.Point(15, 225);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(128, 16);
            this.Label3.TabIndex = 37;
            this.Label3.Text = "薪水：";
            // 
            // tb_Salary
            // 
            this.tb_Salary.Location = new System.Drawing.Point(15, 242);
            this.tb_Salary.Name = "tb_Salary";
            this.tb_Salary.Size = new System.Drawing.Size(160, 22);
            this.tb_Salary.TabIndex = 36;
            this.tb_Salary.Text = "1200";
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(15, 154);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(128, 16);
            this.Label2.TabIndex = 35;
            this.Label2.Text = "部门：";
            // 
            // tb_Division
            // 
            this.tb_Division.Location = new System.Drawing.Point(15, 170);
            this.tb_Division.Name = "tb_Division";
            this.tb_Division.Size = new System.Drawing.Size(160, 22);
            this.tb_Division.TabIndex = 34;
            this.tb_Division.Text = "技术部";
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(15, 82);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(128, 16);
            this.Label1.TabIndex = 33;
            this.Label1.Text = "姓名：";
            // 
            // tb_Name
            // 
            this.tb_Name.Location = new System.Drawing.Point(15, 98);
            this.tb_Name.Name = "tb_Name";
            this.tb_Name.Size = new System.Drawing.Size(160, 22);
            this.tb_Name.TabIndex = 32;
            this.tb_Name.Text = "张三";
            // 
            // ModelessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.lb_drop);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.tb_Salary);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.tb_Division);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.tb_Name);
            this.Name = "ModelessForm";
            this.Size = new System.Drawing.Size(195, 328);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label lb_drop;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox tb_Salary;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox tb_Division;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tb_Name;

    }
}
