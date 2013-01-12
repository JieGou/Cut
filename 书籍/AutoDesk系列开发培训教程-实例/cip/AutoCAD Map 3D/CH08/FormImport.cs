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
// FormImport.cs

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Windows.Forms.Design;

namespace CH08
{
	/// <summary>
	/// Summary description for FormImport.
	/// </summary>
	public class ImportForm : System.Windows.Forms.Form
	{
		private bool m_importFile;        // true, import a file false, import a directory
		private string m_impFileName;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ImportForm()
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
		private System.Windows.Forms.ComboBox comboBoxFileType;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonImpFile;
		private System.Windows.Forms.TextBox textBoxImpFile;
		private System.Windows.Forms.CheckBox checkBoxMLayer;
		private System.Windows.Forms.Button buttonImpDir;
		private System.Windows.Forms.TextBox textBoxImpDir;
		private System.Windows.Forms.CheckBox checkBoxImpOD;
		private System.Windows.Forms.ComboBox comboBoxColor;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.buttonImpFile = new System.Windows.Forms.Button();
            this.textBoxImpFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxFileType = new System.Windows.Forms.ComboBox();
            this.checkBoxMLayer = new System.Windows.Forms.CheckBox();
            this.buttonImpDir = new System.Windows.Forms.Button();
            this.textBoxImpDir = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxImpOD = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxColor = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonImpFile
            // 
            this.buttonImpFile.Location = new System.Drawing.Point(280, 48);
            this.buttonImpFile.Name = "buttonImpFile";
            this.buttonImpFile.Size = new System.Drawing.Size(72, 24);
            this.buttonImpFile.TabIndex = 0;
            this.buttonImpFile.Text = "浏览...";
            this.buttonImpFile.Click += new System.EventHandler(this.buttonImpFile_Click);
            // 
            // textBoxImpFile
            // 
            this.textBoxImpFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxImpFile.Location = new System.Drawing.Point(24, 50);
            this.textBoxImpFile.Name = "textBoxImpFile";
            this.textBoxImpFile.ReadOnly = true;
            this.textBoxImpFile.Size = new System.Drawing.Size(248, 20);
            this.textBoxImpFile.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "或者";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "导入文件类型";
            // 
            // comboBoxFileType
            // 
            this.comboBoxFileType.Items.AddRange(new object[] {
            "MAPINFO",
            "MIF"});
            this.comboBoxFileType.Location = new System.Drawing.Point(120, 166);
            this.comboBoxFileType.Name = "comboBoxFileType";
            this.comboBoxFileType.Size = new System.Drawing.Size(80, 21);
            this.comboBoxFileType.TabIndex = 4;
            this.comboBoxFileType.Text = "MAPINFO";
            // 
            // checkBoxMLayer
            // 
            this.checkBoxMLayer.Location = new System.Drawing.Point(216, 164);
            this.checkBoxMLayer.Name = "checkBoxMLayer";
            this.checkBoxMLayer.Size = new System.Drawing.Size(136, 24);
            this.checkBoxMLayer.TabIndex = 5;
            this.checkBoxMLayer.Text = "每个文件一个层";
            // 
            // buttonImpDir
            // 
            this.buttonImpDir.Location = new System.Drawing.Point(280, 200);
            this.buttonImpDir.Name = "buttonImpDir";
            this.buttonImpDir.Size = new System.Drawing.Size(72, 24);
            this.buttonImpDir.TabIndex = 6;
            this.buttonImpDir.Text = "浏览...";
            this.buttonImpDir.Click += new System.EventHandler(this.buttonImpDir_Click);
            // 
            // textBoxImpDir
            // 
            this.textBoxImpDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxImpDir.Location = new System.Drawing.Point(24, 202);
            this.textBoxImpDir.Name = "textBoxImpDir";
            this.textBoxImpDir.ReadOnly = true;
            this.textBoxImpDir.Size = new System.Drawing.Size(248, 20);
            this.textBoxImpDir.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(8, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 80);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择文件...";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(8, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 112);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择目录...";
            // 
            // checkBoxImpOD
            // 
            this.checkBoxImpOD.Location = new System.Drawing.Point(8, 264);
            this.checkBoxImpOD.Name = "checkBoxImpOD";
            this.checkBoxImpOD.Size = new System.Drawing.Size(208, 16);
            this.checkBoxImpOD.TabIndex = 11;
            this.checkBoxImpOD.Text = "导入属性为 OD表";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 296);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(232, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "用颜色高亮显示导入的实体";
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColor.Location = new System.Drawing.Point(280, 294);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(72, 21);
            this.comboBoxColor.TabIndex = 13;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(195, 336);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 24);
            this.buttonCancel.TabIndex = 14;
            this.buttonCancel.Text = "取消l";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(82, 336);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(96, 24);
            this.buttonOK.TabIndex = 15;
            this.buttonOK.Text = "确定";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // ImportForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(469, 445);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.comboBoxColor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxImpOD);
            this.Controls.Add(this.textBoxImpDir);
            this.Controls.Add(this.buttonImpDir);
            this.Controls.Add(this.checkBoxMLayer);
            this.Controls.Add(this.comboBoxFileType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxImpFile);
            this.Controls.Add(this.buttonImpFile);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "导入文件对话框";
            this.Load += new System.EventHandler(this.FormImport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		// nested class to select folder, the syntax is like OpenFileDialog
		private class OpenFolderDialog : System.Windows.Forms.Design.FolderNameEditor
		{
			System.Windows.Forms.Design.FolderNameEditor.FolderBrowser fDialog = new
				System.Windows.Forms.Design.FolderNameEditor.FolderBrowser();
			public OpenFolderDialog()
			{
			}
			public DialogResult ShowDialog()
			{
				return ShowDialog("Slect a folder");
			}

			public DialogResult ShowDialog(string description)
			{
				fDialog.Description = description;
				return fDialog.ShowDialog();
			}
			public string SelectedPath
			{
				get
				{
					return fDialog.DirectoryPath;
				}
			}
			~OpenFolderDialog()
			{
				fDialog.Dispose();
			}
		}

		private void buttonImpFile_Click(System.Object sender, System.EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.InitialDirectory = @"c:\";
			openFileDialog1.Filter = "MIF files (*.mif)|*.mif|MAPINFO files (*.tab)|*.tab";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = false;
			openFileDialog1.Title = "Import File";

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				this.textBoxImpFile.Text = openFileDialog1.FileName;
				this.textBoxImpDir.Text = "";
				this.m_impFileName = openFileDialog1.FileName;
				this.m_importFile = true;
			}
		}

		private void buttonImpDir_Click(System.Object sender, System.EventArgs e)
		{
			OpenFolderDialog openFolderDialog = new OpenFolderDialog();
			if (openFolderDialog.ShowDialog() == DialogResult.OK)
			{
				this.textBoxImpDir.Text = openFolderDialog.SelectedPath;
				this.textBoxImpFile.Text = "";
				this.m_impFileName = openFolderDialog.SelectedPath;
				this.m_importFile = false;
			}
		}

		private void buttonCancel_Click(System.Object sender, System.EventArgs e)
		{
			Close();
		}

		private void buttonOK_Click(System.Object sender, System.EventArgs e)
		{
			SampleCommands sc = SampleCommands.Instance;			

			// Check the file names
			if (m_impFileName == null)
			{
				MessageBox.Show( "Please specify the file name or choose a directory." );
				return;
			}

			if (this.m_importFile)
			{
				if (File.Exists(this.m_impFileName) == false)
				{
					MessageBox.Show("\nInput file not found!");
					return;
				}

				// If file extension is ".mif" then the import format is "MIF"
				if ((m_impFileName[m_impFileName.Length - 1] == 'f') ||
					(m_impFileName[m_impFileName.Length - 1] == 'F'))
				{
					sc.ImportDwg("MIF", m_impFileName, false, this.checkBoxImpOD.Checked);
				}
				// If file extension is ".tab" then the import format is "MAPINFO"
				else if ((m_impFileName[m_impFileName.Length - 1] == 'b') ||
					(m_impFileName[m_impFileName.Length - 1] == 'B'))
				{
					sc.ImportDwg("MAPINFO", this.m_impFileName, false, this.checkBoxImpOD.Checked);
				}
				else
				{
					MessageBox.Show("\nInvalid file name");
					return;
				}
			}
			else
			{
				bool isToLayer0 = !this.checkBoxMLayer.Checked;
				string fileExtension = null;
				string format = null;

				if (this.comboBoxFileType.SelectedIndex == 1)
				{
					fileExtension = ".mif";
					format = "MIF";
				}
				else
				{
					fileExtension = ".tab";
					format = "MAPINFO";
				}

				DirectoryInfo di = new DirectoryInfo(this.m_impFileName);
				// Create an array representing the files in the current directory.
				FileInfo []fi = di.GetFiles();
				// Print out the names of the files in the current directory.
				foreach (FileInfo fiTemp in fi)
				{
					string fileName = string.Concat(String.Concat(this.m_impFileName, @"\"), fiTemp.Name);
					fileName = fileName.ToLower();
					if (fileName.EndsWith(fileExtension))
					{
						sc.ImportDwg(format, fileName, isToLayer0, this.checkBoxImpOD.Checked);
					}
				}
			}

			// Convert the color of the entities if necessary
			if (this.comboBoxColor.SelectedIndex > 0)
			{
				int colorIndex = this.comboBoxColor.SelectedIndex;
				long lockedEntityCount = 0;
				sc.HighlightEntities(sc.ImportEventManager.ImportedEntities,
					colorIndex, ref lockedEntityCount);

				if (lockedEntityCount > 0)
					Utility.AcadEditor.WriteMessage(string.Format("\n There are {0} locked entities.", lockedEntityCount));
			}
			sc.ImportEventManager.ImportedEntities.Clear();

			Close();
		}

		private void FormImport_Load(object sender, System.EventArgs e)
		{
			comboBoxColor.Items.Add("-NO-");
			comboBoxColor.Items.Add("Red");
			comboBoxColor.Items.Add("Yellow");
			comboBoxColor.Items.Add("Green");
			comboBoxColor.Items.Add("Cyan");
			comboBoxColor.Items.Add("Blue");
			comboBoxColor.Items.Add("Magenta");
			comboBoxColor.Items.Add("White");
		}
	}
}
