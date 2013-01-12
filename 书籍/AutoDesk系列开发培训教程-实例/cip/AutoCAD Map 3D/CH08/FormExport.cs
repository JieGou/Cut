//
// (C) Copyright 2004-2007 by Autodesk, Inc.
//
//
//
// By using this code, you are agreeing to the terms
// and conditions of the License Agreement that appeared
// and was accepted upon download or installation
// (or in connection with the download or installation)
// of the Autodesk software in which this code is included.
// All permissions on use of this code are as set forth
// in such License Agreement provided that the above copyright
// notice appears in all authorized copies and that both that
// copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

// FormExport.cs

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace CH08
{
	/// <summary>
	/// Summary description for FormExport.
	/// </summary>
	public class FormExport : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormExport()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonExpFile;
		private System.Windows.Forms.Button buttonLogFile;
		private System.Windows.Forms.CheckBox checkBoxExpOD;
		private System.Windows.Forms.RadioButton radioButtonAllToOne;
		private System.Windows.Forms.RadioButton radioButtonDiffLayers;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.TextBox textBoxExpFile;
		private System.Windows.Forms.TextBox textBoxLogFile;


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxExpFile = new System.Windows.Forms.TextBox();
            this.textBoxLogFile = new System.Windows.Forms.TextBox();
            this.buttonExpFile = new System.Windows.Forms.Button();
            this.buttonLogFile = new System.Windows.Forms.Button();
            this.checkBoxExpOD = new System.Windows.Forms.CheckBox();
            this.radioButtonAllToOne = new System.Windows.Forms.RadioButton();
            this.radioButtonDiffLayers = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "导出文件名称";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Log文件名";
            // 
            // textBoxExpFile
            // 
            this.textBoxExpFile.Location = new System.Drawing.Point(32, 32);
            this.textBoxExpFile.Name = "textBoxExpFile";
            this.textBoxExpFile.ReadOnly = true;
            this.textBoxExpFile.Size = new System.Drawing.Size(216, 20);
            this.textBoxExpFile.TabIndex = 2;
            // 
            // textBoxLogFile
            // 
            this.textBoxLogFile.Location = new System.Drawing.Point(32, 88);
            this.textBoxLogFile.Name = "textBoxLogFile";
            this.textBoxLogFile.ReadOnly = true;
            this.textBoxLogFile.Size = new System.Drawing.Size(216, 20);
            this.textBoxLogFile.TabIndex = 3;
            // 
            // buttonExpFile
            // 
            this.buttonExpFile.Location = new System.Drawing.Point(264, 32);
            this.buttonExpFile.Name = "buttonExpFile";
            this.buttonExpFile.Size = new System.Drawing.Size(80, 20);
            this.buttonExpFile.TabIndex = 4;
            this.buttonExpFile.Text = "浏览...";
            this.buttonExpFile.Click += new System.EventHandler(this.buttonExpFile_Click);
            // 
            // buttonLogFile
            // 
            this.buttonLogFile.Location = new System.Drawing.Point(264, 88);
            this.buttonLogFile.Name = "buttonLogFile";
            this.buttonLogFile.Size = new System.Drawing.Size(80, 20);
            this.buttonLogFile.TabIndex = 5;
            this.buttonLogFile.Text = "浏览...";
            this.buttonLogFile.Click += new System.EventHandler(this.buttonLogFile_Click);
            // 
            // checkBoxExpOD
            // 
            this.checkBoxExpOD.Location = new System.Drawing.Point(32, 128);
            this.checkBoxExpOD.Name = "checkBoxExpOD";
            this.checkBoxExpOD.Size = new System.Drawing.Size(184, 20);
            this.checkBoxExpOD.TabIndex = 6;
            this.checkBoxExpOD.Text = "输出对象数据表";
            // 
            // radioButtonAllToOne
            // 
            this.radioButtonAllToOne.Checked = true;
            this.radioButtonAllToOne.Location = new System.Drawing.Point(48, 200);
            this.radioButtonAllToOne.Name = "radioButtonAllToOne";
            this.radioButtonAllToOne.Size = new System.Drawing.Size(208, 24);
            this.radioButtonAllToOne.TabIndex = 8;
            this.radioButtonAllToOne.TabStop = true;
            this.radioButtonAllToOne.Text = "所以的实体在一个文件";
            // 
            // radioButtonDiffLayers
            // 
            this.radioButtonDiffLayers.Location = new System.Drawing.Point(48, 232);
            this.radioButtonDiffLayers.Name = "radioButtonDiffLayers";
            this.radioButtonDiffLayers.Size = new System.Drawing.Size(216, 24);
            this.radioButtonDiffLayers.TabIndex = 9;
            this.radioButtonDiffLayers.Text = "根据层来到处文件（每个层一个文件）";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(24, 168);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 104);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择导出方法";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(80, 304);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(96, 24);
            this.buttonOK.TabIndex = 12;
            this.buttonOK.Text = "确定";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(208, 304);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 24);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FormExport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(376, 382);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.radioButtonDiffLayers);
            this.Controls.Add(this.radioButtonAllToOne);
            this.Controls.Add(this.checkBoxExpOD);
            this.Controls.Add(this.buttonLogFile);
            this.Controls.Add(this.buttonExpFile);
            this.Controls.Add(this.textBoxLogFile);
            this.Controls.Add(this.textBoxExpFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormExport";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "导出文件对话框";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void buttonExpFile_Click(System.Object sender, System.EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();

			saveFileDialog1.InitialDirectory = @"c:\" ;
			saveFileDialog1.Filter = "MIF files (*.mif)|*.mif|MAPINFO files (*.tab)|*.tab";
			saveFileDialog1.FilterIndex = 1;
			saveFileDialog1.RestoreDirectory = false;
			saveFileDialog1.CheckFileExists = false;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				textBoxExpFile.Text = saveFileDialog1.FileName;
			}
		}

		private void buttonLogFile_Click(System.Object sender, System.EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();

			saveFileDialog1.InitialDirectory = @"c:\" ;
			saveFileDialog1.Filter = "log files (*.log)|*.log|text files (*.txt)|*.txt";
			saveFileDialog1.FilterIndex = 1;
			saveFileDialog1.RestoreDirectory = false;
			saveFileDialog1.CheckFileExists = false;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				textBoxLogFile.Text = saveFileDialog1.FileName;
			}
		}

		private void buttonOK_Click(System.Object sender, System.EventArgs e)
		{
			SampleCommands sc = SampleCommands.Instance;
			// Check if user want to export OD table
			bool isODTable = this.checkBoxExpOD.Checked;
			String expFileName = this.textBoxExpFile.Text;
			String logFileName = this.textBoxLogFile.Text;
			String format = null;

			// Check the file names
			if (expFileName.Length == 0)
			{
				MessageBox.Show( "Please specify the export file name." );
				return;
			}
			if (logFileName.Length == 0)
			{
				MessageBox.Show( "Please specify the log file name." );
				return;
			}


			// If file extension is ".mif" then the import format is "MIF"
			if ((expFileName[expFileName.Length-1] == 'f') ||
				(expFileName[expFileName.Length-1] == 'F'))
			{
				format = "MIF";
			}
			// If file extension is ".tab" then the import format is "MAPINFO"
			else if ((expFileName[expFileName.Length-1] == 'b') ||
				(expFileName[expFileName.Length-1] == 'B'))
			{
				format = "MAPINFO";
			}
			else
			{
				Utility.ShowMsg("\nInvalid file name");
				Close();
				Utility.ShowMsg("\nExporting failed.");
				return;
			}

			if (this.radioButtonAllToOne.Checked)
			{
				sc.DoExport(format, expFileName, null, logFileName, isODTable, false);
			}
			else
			{
				StringCollection layerNames = new StringCollection();
				if (sc.GetLayers(layerNames))
				{
					foreach (string layerName in layerNames)
					{
						string expLayerFileName = string.Copy(expFileName);
						expLayerFileName = expLayerFileName.Insert(expLayerFileName.Length - 4, "_");
						expLayerFileName = expLayerFileName.Insert(expLayerFileName.Length - 4, layerName);
						sc.DoExport(format, expLayerFileName, layerName, 
							logFileName, isODTable, false);
					}
				} // End of if
			} // End of if
			
			Close();
		}

		private void buttonCancel_Click(System.Object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
