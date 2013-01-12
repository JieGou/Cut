namespace Chapter2
{
    partial class BrowserForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserForm));
            this.axCalendar1 = new AxMSACAL.AxCalendar();
            ((System.ComponentModel.ISupportInitialize)(this.axCalendar1)).BeginInit();
            this.SuspendLayout();
            // 
            // axCalendar1
            // 
            this.axCalendar1.Enabled = true;
            this.axCalendar1.Location = new System.Drawing.Point(-2, -1);
            this.axCalendar1.Name = "axCalendar1";
            this.axCalendar1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCalendar1.OcxState")));
            this.axCalendar1.Size = new System.Drawing.Size(296, 268);
            this.axCalendar1.TabIndex = 0;
            // 
            // BrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.axCalendar1);
            this.Name = "BrowserForm";
            this.Text = "BrowserForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BrowserForm_FormClosed);
            this.Load += new System.EventHandler(this.BrowserForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axCalendar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMSACAL.AxCalendar axCalendar1;
    }
}