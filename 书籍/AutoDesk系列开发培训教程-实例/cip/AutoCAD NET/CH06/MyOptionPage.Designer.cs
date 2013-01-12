namespace CH06
{
    partial class MyOptionPage
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
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.tb_Company = new System.Windows.Forms.TextBox();
            this.tb_Author = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(16, 70);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(136, 16);
            this.Label2.TabIndex = 19;
            this.Label2.Text = "公司：";
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(16, 22);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(136, 16);
            this.Label1.TabIndex = 18;
            this.Label1.Text = "设计者：";
            // 
            // tb_Company
            // 
            this.tb_Company.Location = new System.Drawing.Point(16, 86);
            this.tb_Company.Name = "tb_Company";
            this.tb_Company.Size = new System.Drawing.Size(184, 22);
            this.tb_Company.TabIndex = 17;
            this.tb_Company.Text = Class1.sCompanyDefault;// "ADSK";
            // 
            // tb_Author
            // 
            this.tb_Author.Location = new System.Drawing.Point(16, 38);
            this.tb_Author.Name = "tb_Author";
            this.tb_Author.Size = new System.Drawing.Size(184, 22);
            this.tb_Author.TabIndex = 16;
            this.tb_Author.Text = Class1.sAuthorDefault;// " ";
            // 
            // MyOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.tb_Company);
            this.Controls.Add(this.tb_Author);
            this.Name = "MyOptionPage";
            this.Size = new System.Drawing.Size(230, 143);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tb_Company;
        internal System.Windows.Forms.TextBox tb_Author;
    }
}
