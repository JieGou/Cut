namespace CH06
{
    partial  class MyModalForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_layer = new System.Windows.Forms.TextBox();
            this.tb_center = new System.Windows.Forms.TextBox();
            this.tb_radius = new System.Windows.Forms.TextBox();
            this.Button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "选择圆";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "所在层：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "圆心：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "半径：";
            // 
            // tb_layer
            // 
            this.tb_layer.Location = new System.Drawing.Point(101, 20);
            this.tb_layer.Name = "tb_layer";
            this.tb_layer.Size = new System.Drawing.Size(100, 22);
            this.tb_layer.TabIndex = 2;
            // 
            // tb_center
            // 
            this.tb_center.Location = new System.Drawing.Point(101, 56);
            this.tb_center.Name = "tb_center";
            this.tb_center.Size = new System.Drawing.Size(100, 22);
            this.tb_center.TabIndex = 2;
            // 
            // tb_radius
            // 
            this.tb_radius.Location = new System.Drawing.Point(101, 105);
            this.tb_radius.Name = "tb_radius";
            this.tb_radius.Size = new System.Drawing.Size(100, 22);
            this.tb_radius.TabIndex = 2;
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(137, 149);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(96, 39);
            this.Button2.TabIndex = 31;
            this.Button2.Text = "退出";
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // MyModalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 202);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.tb_radius);
            this.Controls.Add(this.tb_center);
            this.Controls.Add(this.tb_layer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "MyModalForm";
            this.Text = "MyModalForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_layer;
        private System.Windows.Forms.TextBox tb_center;
        private System.Windows.Forms.TextBox tb_radius;
        internal System.Windows.Forms.Button Button2;
    }
}