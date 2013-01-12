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

// AnnotationDlg.cs

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Gis.Map.Annotation;
using Autodesk.Gis.Map.ObjectData;
using Autodesk.Gis.Map.Constants;
using Autodesk.Gis.Map.Project;
using Autodesk.Gis.Map;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

namespace CH04
{
	public class AnnotationDlg : System.Windows.Forms.Form
	{
		private Label lblColumnName;
		private Label lblODTable;
		private Label lblTemplateDefined;
		private Label lblTemplateName;
		private Button btnClose;
		private Button btnInsertOverride;
		private Button btnInsertAnnotation;
		private Button btnRemove;
		private Button btnShowInfo;
		private Button btnCreateNew;
		private ListBox listbTemplates;
		private ListBox listbColumnName;
		private ListBox listbODTable;
		private TextBox textbTemplateName;
		private CheckBox checkbFieldName;
		private GroupBox groupbODData;
		private GroupBox groupbDefine;
		private GroupBox groupbInsert;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private Autodesk.Gis.Map.MapApplication m_mapApp;
		private Autodesk.Gis.Map.Annotation.Annotations m_annotations;
		private ObjectId m_idTemplate;
		private string m_templateName;
		private System.Collections.Specialized.StringCollection m_colODtblList;
		private PromptSelectionResult m_selSetResult;
		private string m_ODTableName;
		private Autodesk.Gis.Map.ObjectData.Table m_ODTable;
		private Autodesk.Gis.Map.ObjectData.Tables m_ODTables;
		private string m_selectedTemplateName;
		private int	m_fieldNameCheckBoxStatus;
		private AnnotationOverrides m_annoOverrides;

		public AnnotationDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		private void AnnotationDlg_Load(object sender, System.EventArgs e)
		{
			try
			{
				m_mapApp = Autodesk.Gis.Map.HostMapApplicationServices.Application;
				m_annotations = m_mapApp.ActiveProject.Annotations;
				m_idTemplate = ObjectId.Null;
				m_templateName = "";
				m_colODtblList = null;
				m_ODTableName = "";
				m_ODTables = null;
				m_ODTable = null;
				m_selectedTemplateName = "";
				m_fieldNameCheckBoxStatus = 0;
				m_annoOverrides = new AnnotationOverrides();
				m_annoOverrides.Clear();

				this.textbTemplateName.Text = "";
				this.listbColumnName.Items.Clear();
				this.listbODTable.SelectedIndex = -1;

				AnnoTemplateNameListBoxUpdate();
				InitODTableList();
				btnCreateNew.Enabled = false;
				InitButtons();
				checkbFieldName.Checked = false;
			}
			catch(System.Exception err)
			{
				MessageBox.Show(err.Message);
			}
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

		#region " Windows API "
		[DllImport("User32.dll")]
		private static extern Int32 SetFocus(Int32 hWnd);
		[DllImport("User32.dll")]
		private static extern bool EnableWindow(Int32 hWnd, bool enable);
		[DllImport("User32.dll")]
		private static extern bool SetWindowPos(Int32 hWnd, Int32 hWndInsertAfter, Int32 X, Int32 Y, Int32 cx, Int32 cy, Int32 flags);

		private const Int32 LB_ERR = -1;
		private const Int32 SWP_NOSIZE = 0x0001;
		private const Int32 SWP_NOMOVE = 0x0002;
		private const Int32 SWP_SHOWWINDOW = 0x0040;
		private const Int32 HWND_TOPMOST = -1;
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnClose = new System.Windows.Forms.Button();
            this.btnInsertOverride = new System.Windows.Forms.Button();
            this.btnInsertAnnotation = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnShowInfo = new System.Windows.Forms.Button();
            this.listbTemplates = new System.Windows.Forms.ListBox();
            this.lblTemplateDefined = new System.Windows.Forms.Label();
            this.btnCreateNew = new System.Windows.Forms.Button();
            this.checkbFieldName = new System.Windows.Forms.CheckBox();
            this.listbColumnName = new System.Windows.Forms.ListBox();
            this.listbODTable = new System.Windows.Forms.ListBox();
            this.lblColumnName = new System.Windows.Forms.Label();
            this.lblODTable = new System.Windows.Forms.Label();
            this.textbTemplateName = new System.Windows.Forms.TextBox();
            this.lblTemplateName = new System.Windows.Forms.Label();
            this.groupbODData = new System.Windows.Forms.GroupBox();
            this.groupbDefine = new System.Windows.Forms.GroupBox();
            this.groupbInsert = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(164, 493);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(125, 27);
            this.btnClose.TabIndex = 38;
            this.btnClose.Text = "退出";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnInsertOverride
            // 
            this.btnInsertOverride.Location = new System.Drawing.Point(266, 437);
            this.btnInsertOverride.Name = "btnInsertOverride";
            this.btnInsertOverride.Size = new System.Drawing.Size(154, 28);
            this.btnInsertOverride.TabIndex = 36;
            this.btnInsertOverride.Text = "注释重载";
            this.btnInsertOverride.Click += new System.EventHandler(this.btnInsertOverride_Click);
            // 
            // btnInsertAnnotation
            // 
            this.btnInsertAnnotation.Location = new System.Drawing.Point(266, 400);
            this.btnInsertAnnotation.Name = "btnInsertAnnotation";
            this.btnInsertAnnotation.Size = new System.Drawing.Size(154, 28);
            this.btnInsertAnnotation.TabIndex = 35;
            this.btnInsertAnnotation.Text = "插入注释";
            this.btnInsertAnnotation.Click += new System.EventHandler(this.btnInsertAnnotation_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(266, 327);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(154, 27);
            this.btnRemove.TabIndex = 34;
            this.btnRemove.Text = "删除";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnShowInfo
            // 
            this.btnShowInfo.Location = new System.Drawing.Point(266, 363);
            this.btnShowInfo.Name = "btnShowInfo";
            this.btnShowInfo.Size = new System.Drawing.Size(154, 28);
            this.btnShowInfo.TabIndex = 33;
            this.btnShowInfo.Text = "显示内容";
            this.btnShowInfo.Click += new System.EventHandler(this.btnShowInfo_Click);
            // 
            // listbTemplates
            // 
            this.listbTemplates.ItemHeight = 16;
            this.listbTemplates.Location = new System.Drawing.Point(49, 354);
            this.listbTemplates.Name = "listbTemplates";
            this.listbTemplates.Size = new System.Drawing.Size(192, 84);
            this.listbTemplates.TabIndex = 32;
            this.listbTemplates.SelectedIndexChanged += new System.EventHandler(this.listbTemplates_SelectedIndexChanged);
            // 
            // lblTemplateDefined
            // 
            this.lblTemplateDefined.Location = new System.Drawing.Point(40, 327);
            this.lblTemplateDefined.Name = "lblTemplateDefined";
            this.lblTemplateDefined.Size = new System.Drawing.Size(182, 18);
            this.lblTemplateDefined.TabIndex = 31;
            this.lblTemplateDefined.Text = "当前定义的注释模板:";
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.Location = new System.Drawing.Point(164, 243);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(125, 28);
            this.btnCreateNew.TabIndex = 29;
            this.btnCreateNew.Text = "创建";
            this.btnCreateNew.Click += new System.EventHandler(this.btnCreateNew_Click);
            // 
            // checkbFieldName
            // 
            this.checkbFieldName.Location = new System.Drawing.Point(59, 203);
            this.checkbFieldName.Name = "checkbFieldName";
            this.checkbFieldName.Size = new System.Drawing.Size(153, 22);
            this.checkbFieldName.TabIndex = 27;
            this.checkbFieldName.Text = "是否显示字段名称";
            this.checkbFieldName.CheckedChanged += new System.EventHandler(this.checkbFieldName_CheckedChanged);
            // 
            // listbColumnName
            // 
            this.listbColumnName.ItemHeight = 16;
            this.listbColumnName.Location = new System.Drawing.Point(260, 102);
            this.listbColumnName.Name = "listbColumnName";
            this.listbColumnName.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listbColumnName.Size = new System.Drawing.Size(144, 68);
            this.listbColumnName.TabIndex = 26;
            this.listbColumnName.SelectedIndexChanged += new System.EventHandler(this.listbColumnName_SelectedIndexChanged);
            // 
            // listbODTable
            // 
            this.listbODTable.ItemHeight = 16;
            this.listbODTable.Location = new System.Drawing.Point(59, 102);
            this.listbODTable.Name = "listbODTable";
            this.listbODTable.Size = new System.Drawing.Size(144, 68);
            this.listbODTable.TabIndex = 25;
            this.listbODTable.SelectedIndexChanged += new System.EventHandler(this.listbODTable_SelectedIndexChanged);
            // 
            // lblColumnName
            // 
            this.lblColumnName.Location = new System.Drawing.Point(251, 83);
            this.lblColumnName.Name = "lblColumnName";
            this.lblColumnName.Size = new System.Drawing.Size(144, 19);
            this.lblColumnName.TabIndex = 24;
            this.lblColumnName.Text = "选择列名:";
            // 
            // lblODTable
            // 
            this.lblODTable.Location = new System.Drawing.Point(49, 83);
            this.lblODTable.Name = "lblODTable";
            this.lblODTable.Size = new System.Drawing.Size(120, 19);
            this.lblODTable.TabIndex = 23;
            this.lblODTable.Text = "选择对象数据表:";
            // 
            // textbTemplateName
            // 
            this.textbTemplateName.Location = new System.Drawing.Point(184, 40);
            this.textbTemplateName.Name = "textbTemplateName";
            this.textbTemplateName.Size = new System.Drawing.Size(240, 22);
            this.textbTemplateName.TabIndex = 22;
            this.textbTemplateName.TextChanged += new System.EventHandler(this.textbTemplateName_TextChanged);
            // 
            // lblTemplateName
            // 
            this.lblTemplateName.Location = new System.Drawing.Point(40, 40);
            this.lblTemplateName.Name = "lblTemplateName";
            this.lblTemplateName.Size = new System.Drawing.Size(153, 19);
            this.lblTemplateName.TabIndex = 21;
            this.lblTemplateName.Text = "注释样板名称:";
            // 
            // groupbODData
            // 
            this.groupbODData.Location = new System.Drawing.Point(40, 65);
            this.groupbODData.Name = "groupbODData";
            this.groupbODData.Size = new System.Drawing.Size(384, 166);
            this.groupbODData.TabIndex = 28;
            this.groupbODData.TabStop = false;
            this.groupbODData.Text = "对象数据";
            // 
            // groupbDefine
            // 
            this.groupbDefine.Location = new System.Drawing.Point(30, 13);
            this.groupbDefine.Name = "groupbDefine";
            this.groupbDefine.Size = new System.Drawing.Size(403, 267);
            this.groupbDefine.TabIndex = 30;
            this.groupbDefine.TabStop = false;
            this.groupbDefine.Text = "创建注释样板";
            // 
            // groupbInsert
            // 
            this.groupbInsert.Location = new System.Drawing.Point(30, 299);
            this.groupbInsert.Name = "groupbInsert";
            this.groupbInsert.Size = new System.Drawing.Size(403, 184);
            this.groupbInsert.TabIndex = 37;
            this.groupbInsert.TabStop = false;
            this.groupbInsert.Text = "注释操作";
            // 
            // AnnotationDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(462, 528);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnInsertOverride);
            this.Controls.Add(this.btnInsertAnnotation);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnShowInfo);
            this.Controls.Add(this.listbTemplates);
            this.Controls.Add(this.lblTemplateDefined);
            this.Controls.Add(this.btnCreateNew);
            this.Controls.Add(this.checkbFieldName);
            this.Controls.Add(this.listbColumnName);
            this.Controls.Add(this.listbODTable);
            this.Controls.Add(this.lblColumnName);
            this.Controls.Add(this.lblODTable);
            this.Controls.Add(this.textbTemplateName);
            this.Controls.Add(this.lblTemplateName);
            this.Controls.Add(this.groupbODData);
            this.Controls.Add(this.groupbDefine);
            this.Controls.Add(this.groupbInsert);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnnotationDlg";
            this.ShowInTaskbar = false;
            this.Text = "注释示例程序";
            this.Load += new System.EventHandler(this.AnnotationDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        //--------------------------------------------------------------------------
        //
        // 功能：创建注释模板
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void btnCreateNew_Click(System.Object sender, System.EventArgs e)
		{
			try
			{
                ////用户输入模板名称
				string newTemplateName = textbTemplateName.Text; 
				if (newTemplateName.Length == 0)
				{
                    MessageBox.Show("输入模板名称。'");
					textbTemplateName.Focus();
					return;
				}

                // 建成模板名称是否已经存在
				bool templExists = m_annotations.AnnotationTemplateExists(newTemplateName);  
				if (templExists)
				{
					MessageBox.Show(string.Format("Annotation template name {0} already exists. Creation failed.", newTemplateName));
					textbTemplateName.Focus();
					return;
				}

                //创建模板
				m_idTemplate = m_annotations.CreateAnnotationTemplate(newTemplateName);   
				m_templateName = newTemplateName;                                        
				if (!m_idTemplate.IsNull)
				{
                    //  设置表达式
					SetExpressionFromDialogFields();  
					 // 更新列表
                    AnnoTemplateNameListBoxUpdate();  // Update the list box
				}
				else
				{
					MessageBox.Show("创建注释失败。");
				}
				textbTemplateName.Text = "";
			}
			catch(System.Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}


        //--------------------------------------------------------------------------
        //
        // 功能：设置注释表达式
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void SetExpressionFromDialogFields()
		{
			int columnsSelected = listbColumnName.SelectedIndices.Count;
			for (int index = 0; index < columnsSelected; index++)
			{
				string fieldNameSelected = listbColumnName.SelectedItems[index].ToString();

				Autodesk.AutoCAD.Colors.Color blockColor = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.None, 0);

				string expressionString = null;
                //是否显示字段名称		
				if (m_fieldNameCheckBoxStatus == 1)	
				{
                    // 最后组成表达式
					// (strcat FEATURE_NAME = :FEATURE_NAME@FEATURE_TABLE)
					expressionString = string.Concat("(strcat ", fieldNameSelected, " = :", fieldNameSelected);
					expressionString = string.Concat(expressionString, "@", m_ODTableName, ")");
				}
				else if (m_fieldNameCheckBoxStatus == 0)
				{
					// 最后组成表达式   :FEATURE_NAME@FEATURE_TABLE
					expressionString = string.Concat(":", fieldNameSelected, "@", m_ODTableName);
				}
               //完成属性定义的设置 
				SetAttributeDefinition(                
					fieldNameSelected,
					new Point3d(0.0, 0.0, 0.0), // setPosition
					0.5, // Height
					blockColor, // Color
					TextVerticalMode.TextVerticalMid,
					TextHorizontalMode.TextCenter,
					expressionString,
					new Point3d(0.0, index + 0.5, 0.0));
			}
		}

        //--------------------------------------------------------------------------
        //
        // 功能：完成属性定义的设置
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void SetAttributeDefinition(string tag,
			Point3d position,
			double height,
			Autodesk.AutoCAD.Colors.Color color,
			TextVerticalMode vertMode,
			TextHorizontalMode horzMode,
			string expressionString,
			Point3d alignmentPoint)
		{
			using (Transaction trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
			{
                //创建注记文字
				ObjectId idTagText;
				idTagText = m_annotations[m_templateName].CreateAnnotationText();        

				// 
				AttributeDefinition tagText = null; 
				tagText = trans.GetObject(idTagText, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForWrite) as AttributeDefinition;
				if (null == tagText)
				{
                    MessageBox.Show("创建注记文字失败。");
					return;
				}

                // 检查是否是注记文字
				if (!m_annotations.IsAnnotationText(tagText))
				{
                    MessageBox.Show(string.Format("不能判断{0} 为注记文字", tag));
					return;
				}
				tagText.Position = position;
				tagText.Tag = tag;
				tagText.Height = height;
				tagText.Color = color;
				tagText.VerticalMode = vertMode;
				tagText.HorizontalMode = horzMode;
				tagText.AlignmentPoint = alignmentPoint;

				m_annotations.SetExpressionString(tagText,
					AnnotationExpressionFields.AttributeDefinitionAnnotationString,
					expressionString); // For OD ":Column_Name@Table_Name"
				
				// 使用 Annotation.TextLayer设置层.


				m_annotations.SetExpressionString(tagText,
					AnnotationExpressionFields.AttributeDefinitionTextLayer,
					"Layer1");

				string newTemplateName = null;
				newTemplateName = textbTemplateName.Text;

				// 设置块定义的颜色
				Autodesk.AutoCAD.Colors.Color redClr = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.None, 1);
				m_annotations[newTemplateName].Color = redClr;
				//创建注记完成
				trans.Commit();         
			}
		}

        //--------------------------------------------------------------------------
        //
        // 功能：显示字段名称
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void checkbFieldName_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			//
			if (checkbFieldName.Checked)
			{
				m_fieldNameCheckBoxStatus = 1;
			}
			else
			{
				m_fieldNameCheckBoxStatus = 0;
			}
		}

       //--------------------------------------------------------------------------
        //
        // 功能：删除注释模板
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void btnRemove_Click(System.Object sender, System.EventArgs e)
		{
			try
			{
				int curSel = listbTemplates.SelectedIndex;
				if (LB_ERR == curSel)
				{
					MessageBox.Show("Select one entry from the list box and click on 'Remove'.");
					return;
				}
				m_selectedTemplateName = listbTemplates.SelectedItem.ToString();
			
				//检查注释模板是否有引用，有则不能删除
				if (!m_annotations[m_selectedTemplateName].Referenced)
				{
					try
					{
						// 删除注释模板m_selectedTemplateName
						m_annotations.DeleteAnnotationTemplate(m_selectedTemplateName);
						AnnoTemplateNameListBoxUpdate();
					}
					catch (Autodesk.AutoCAD.Runtime.Exception err)
					{
						MessageBox.Show(err.Message);
					}
				}
				else
				{
					MessageBox.Show("所选的注释模板不能删除，由于被引用");
					return;
				}
			}
			catch(System.Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

       //--------------------------------------------------------------------------
        //
        // 功能：显示信息
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void btnShowInfo_Click(System.Object sender, System.EventArgs e)
		{
			try
			{
				int curSel = listbTemplates.SelectedIndex;

				if (LB_ERR == curSel)
				{
					MessageBox.Show("从列表框中选择一项，然后在点击该按钮");
					return;
				}
			
				m_selectedTemplateName = listbTemplates.SelectedItem.ToString();
				m_idTemplate = m_annotations[m_selectedTemplateName].BlockDefinitionId;
				m_templateName = m_selectedTemplateName;
				
				// Hide the dialog and give control to the editor
				BeginCommand();

				Utility.ShowMsg(string.Format("\n\n模板名称 = {0}", m_selectedTemplateName));
				try
				{
					string layerMsg = null;
					layerMsg = m_annotations[m_selectedTemplateName].Layer;
					Utility.ShowMsg(string.Format("\n层名 = {0}", layerMsg));

					string lineTypeMsg = null;
					lineTypeMsg = m_annotations[m_selectedTemplateName].Linetype;
					Utility.ShowMsg(string.Format("\n线型 = {0}", lineTypeMsg));

					LineWeight lineWeight;
					lineWeight = m_annotations[m_selectedTemplateName].LineWeight;
					Utility.ShowMsg(string.Format("\n线宽 = {0}", lineWeight));

					Autodesk.AutoCAD.Colors.Color color = null;
					color = m_annotations[m_selectedTemplateName].Color;
					Utility.ShowMsg(string.Format("\n颜色索引= {0}", color.ColorIndex));

					using (Transaction trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
					{
						BlockTableRecord blkTblRcd = (BlockTableRecord)(trans.GetObject(m_idTemplate, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead));
						string blockPositionMsg = null;
						blockPositionMsg = m_annotations.GetExpressionString(blkTblRcd, AnnotationExpressionFields.BlockPosition);
						Utility.ShowMsg(string.Format("\n插入点= {0}", blockPositionMsg));
						trans.Commit();
					}

					double scaleFactor = 0.0;
					scaleFactor = m_annotations[m_selectedTemplateName].ScaleFactor;
					Utility.ShowMsg(string.Format("\n缩放比例 = {0}", scaleFactor));

					double rotation = 0.0;
					rotation = m_annotations[m_selectedTemplateName].Rotation;
					Utility.ShowMsg(string.Format("\n需安装角度 = {0}", rotation));

					// 读取属性信息
					ReadAttributeDefinitions();	
					Utility.ShowMsg("\n回车，返回到对话框:\n\n");
				}
				catch (Autodesk.AutoCAD.Runtime.Exception err)
				{
					MessageBox.Show(err.Message);
				}
				//
				DialogResult = DialogResult.OK;
			}
			catch(System.Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

         //--------------------------------------------------------------------------
        //
        // 功能：插入注释
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void btnInsertAnnotation_Click(System.Object sender, System.EventArgs e)
		{
			try
			{
				int curSel = listbTemplates.SelectedIndex;
				if (LB_ERR == curSel)
				{
					MessageBox.Show("请先选择注释模板");
					return;
				}

				m_selectedTemplateName = listbTemplates.SelectedItem.ToString();
				m_idTemplate = m_annotations[m_selectedTemplateName].BlockDefinitionId;
				m_templateName = m_selectedTemplateName;

			
				using (Transaction trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
				{
					BlockTableRecord blkTblRcd = (BlockTableRecord)trans.GetObject(m_idTemplate, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead);

					// 
					string templateName = null;
					if (!m_annotations.IsAnnotationTemplate(ref templateName, blkTblRcd))
					{
						MessageBox.Show("检查注释模板失败");
						return;
					}

					// 设置表达字符
					try
					{
						m_annotations.SetExpressionString(blkTblRcd,      //对注记内容进行旋转
							AnnotationExpressionFields.BlockRotation,
							".ANGLE");
						trans.Commit();
					}
					catch (Autodesk.AutoCAD.Runtime.Exception err)
					{
						MessageBox.Show(string.Format("对注记内容进行旋转失败\n{0}", err.Message));
					}
				} // end of using transaction

				// 选择实体，插入注释
				try
				{
					m_selSetResult = Utility.AcadEditor.GetSelection();
					if ((m_selSetResult.Status == PromptStatus.OK) && (m_selSetResult.Value.Count > 0))
					{
						foreach (Autodesk.AutoCAD.EditorInput.SelectedObject selectedObject in m_selSetResult.Value)
						{
							m_annotations[m_selectedTemplateName].InsertReference(selectedObject.ObjectId);　　　　　　　　　//sunj-完成注记的插入
						}
						Regen();
						// 
						DialogResult = DialogResult.OK;
					}
					else
					{
						MessageBox.Show("没有选中实体，命令取消.");
					}
				}
				catch (Autodesk.AutoCAD.Runtime.Exception err)
				{
					MessageBox.Show(string.Format("插入注释失败\n{0}", err.Message));
				}
			}
			catch (System.Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}


         //--------------------------------------------------------------------------
        //
        // 功能：删除注释重载
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void btnInsertOverride_Click(System.Object sender, System.EventArgs e)
		{
			try
			{
				// 
				m_annoOverrides.Clear();

				int curSel = listbTemplates.SelectedIndex;
				if (LB_ERR == curSel)
				{
                    MessageBox.Show("请先选择注释模板");
					return;
				}	

				m_selectedTemplateName = listbTemplates.SelectedItem.ToString();
				m_idTemplate = m_annotations[m_selectedTemplateName].BlockDefinitionId;
				m_templateName = m_selectedTemplateName;
				
				using (Transaction trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
				{
					BlockTableRecord blkTblRcd = (BlockTableRecord)(trans.GetObject(m_idTemplate, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead));

					//
					string templateName = null;
					if (!m_annotations.IsAnnotationTemplate(ref templateName, blkTblRcd))
					{
                        MessageBox.Show("检查注释模板失败.");
						return;
					}
					trans.Commit();
				}

				try
				{
					m_selSetResult = Utility.AcadEditor.GetSelection();

					if ((m_selSetResult.Status == PromptStatus.OK) && (m_selSetResult.Value.Count > 0))
					{
						// Initialize the m_annoOverrides with values which we want to override
						m_annoOverrides.RotationOverride = 0.0;
						m_annoOverrides.ScaleFactorOverride = 0.0;

						Autodesk.AutoCAD.Colors.Color greenClr = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.None, 3);
						m_annoOverrides.ColorOverride = greenClr;
						// Note: Above m_annoOverrides.ColorOverride will override the value
						// which we set in the annotation template using the 
						// tagText.set_Color(color); if color is other than byblock
									
						m_annoOverrides.LayerOverride = null;
						m_annoOverrides.LinetypeOverride = null;
						m_annoOverrides.LineweightOverride = null;
						m_annoOverrides.RotationExpressionOverride = "(- .ANGLE 0.785398163)";   //旋转的角度
						m_annoOverrides.ScaleFactorExpressionOverride = null;

						m_annoOverrides.ColorExpressionOverride = null;
						m_annoOverrides.LayerExpressionOverride = null;
						m_annoOverrides.LinetypeExpressionOverride = null;
						m_annoOverrides.LineweightExpressionOverride = null;
						m_annoOverrides.PositionExpressionOverride = null;

						foreach (Autodesk.AutoCAD.EditorInput.SelectedObject selectedObject in m_selSetResult.Value)
						{
							ObjectId idInserted;
							idInserted = m_annotations[m_selectedTemplateName].InsertReference(selectedObject.ObjectId, m_annoOverrides);
						}
						Regen();
						// Close the dialog
						DialogResult = DialogResult.OK;
					}
					else
					{
                        MessageBox.Show("没有选中实体，命令取消");
					}
				}
				catch (Autodesk.AutoCAD.Runtime.Exception err)
				{
                    MessageBox.Show(string.Format("插入重载失败 .\n{0}", err.Message));
				}
			}
			catch (System.Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}


		private void btnClose_Click(System.Object sender, System.EventArgs e)
		{
			Close();
		}
        //--------------------------------------------------------------------------
        //
        // 功能：初始化按钮
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void InitButtons()
		{
			bool isSetVal = false;
			if (listbTemplates.SelectedIndex != LB_ERR)
			{
				isSetVal = true;
			}

			btnRemove.Enabled = isSetVal;
			btnShowInfo.Enabled = isSetVal;
			btnInsertAnnotation.Enabled = isSetVal;
			btnInsertOverride.Enabled = isSetVal;
		}


       //--------------------------------------------------------------------------
        //
        // 功能：获取当前可用的对象数据
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void InitODTableList()
		{
			listbODTable.Items.Clear();
			Autodesk.Gis.Map.MapApplication mapApi = m_mapApp;
			Autodesk.Gis.Map.Project.ProjectModel proj = mapApi.ActiveProject;
			m_ODTables = proj.ODTables;
			if ((null != m_ODTables) && (m_ODTables.TablesCount != 0))
			{
				// Add ObjectData table names into list box control
				m_colODtblList = m_ODTables.GetTableNames();
				listbODTable.BeginUpdate();
				foreach (string tableName in m_colODtblList)
				{
					if (listbODTable.FindStringExact(tableName) == -1)
					{
						listbODTable.Items.Add(tableName);
					}
				}
				listbODTable.EndUpdate();
			}
		}

        //--------------------------------------------------------------------------
        //
        // 功能：获取当前的注释样板
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void AnnoTemplateNameListBoxUpdate()
		{
			System.Collections.Specialized.StringCollection colTemplateName = new System.Collections.Specialized.StringCollection();
			listbTemplates.Items.Clear();

			// Create an array of template names defined in the current drawing.
			try
			{
				colTemplateName = m_annotations.GetTemplateNames();
				listbTemplates.BeginUpdate();
				foreach (string templateName in colTemplateName)
				{
					if (listbTemplates.FindStringExact(templateName) == -1)
					{
						listbTemplates.Items.Add(templateName);
					}	
				}
				listbTemplates.EndUpdate();
				this.InitButtons();
			}
			catch (Autodesk.AutoCAD.Runtime.Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

        //--------------------------------------------------------------------------
        //
        // 功能：获取对象数据的字段
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void listbODTable_SelectedIndexChanged(Object sender, EventArgs e)
		{
			try
			{
				m_ODTableName = "";
				listbColumnName.Items.Clear();

				int curSel = listbODTable.SelectedIndex;
				if (LB_ERR != curSel)
				{
					m_ODTableName = listbODTable.SelectedItem.ToString();
					m_ODTable = m_ODTables[m_ODTableName];

					FieldDefinitions fieldDefs = m_ODTable.FieldDefinitions;

					listbColumnName.BeginUpdate();
					for (int index = 0; index < fieldDefs.Count; index++)
					{	
						FieldDefinition fieldDef = fieldDefs[index];
						string colName = fieldDef.Name;
						if (listbColumnName.FindStringExact(colName) == -1)
						{
							listbColumnName.Items.Add(colName);
						}
					}
					listbColumnName.EndUpdate();
				}
			}
			catch (System.Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}


		private void listbTemplates_SelectedIndexChanged(Object sender, EventArgs e)
		{
			InitButtons();
		}

        //--------------------------------------------------------------------------
        //
        // 功能：按钮控制
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void EnableCreateBtn()
		{
			string newTemplateName = null;	
			newTemplateName = textbTemplateName.Text;
			if ((listbODTable.SelectedIndex != LB_ERR) && (listbColumnName.SelectedIndices.Count != 0) && (newTemplateName.Length != 0))
			{
				btnCreateNew.Enabled = true;
			}
			else
			{
				btnCreateNew.Enabled = false;
			}
		}


         //--------------------------------------------------------------------------
        //
        // 功能：用户列表中选择了列的相应函数
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void listbColumnName_SelectedIndexChanged(Object sender, EventArgs e)
		{
			EnableCreateBtn();
		}


       //--------------------------------------------------------------------------
        //
        // 功能：用户在数据模板名称控件相应函数
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void textbTemplateName_TextChanged(System.Object sender, System.EventArgs e)
		{
			EnableCreateBtn();
		}


        //--------------------------------------------------------------------------
        //
        // 功能：输出文本信息
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void DumpText(Entity entity)
		{
			DBText text = (DBText)(entity);   
			ObjectId blockId = text.TextStyle;
			Point3d position = text.Position;
			Point3d align = text.AlignmentPoint;
			bool isDefAlign = text.IsDefaultAlignment;
			Vector3d normal = text.Normal;
			double thickness = text.Thickness;
			double oblique = text.Oblique;
			double rotation = text.Rotation;
			double height = text.Height;
			double widthFactor = text.WidthFactor;
			string textMsg = text.TextString;
			bool isMirrorX = text.IsMirroredInX;
			bool isMirrorY = text.IsMirroredInY;
			TextHorizontalMode horzMode = text.HorizontalMode;
			TextVerticalMode vertMode = text.VerticalMode;

			TextStyleTableRecord record = null;

			using (Transaction trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
			{
				record = trans.GetObject(blockId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead) as TextStyleTableRecord;
				if (null != record)
				{
					string rcdName = null;
					try
					{
						rcdName = record.Name;
						Utility.ShowMsg(string.Format("\t\t文本样式e: {0}\n", rcdName));
					}
					catch (Autodesk.AutoCAD.Runtime.Exception err)
					{
						MessageBox.Show(err.Message);
					}
				}
				trans.Commit();
			}

			Utility.ShowMsg(string.Format("\t\t文本: {0}\n", textMsg));
			Utility.ShowMsg(string.Format("\t\t位置 {0},{1},{2}\n", position.X, position.Y, position.Z));
			Utility.ShowMsg(string.Format("\t\t对齐点{0},{1},{2}\n", align.X, align.Y, align.Z));
			Utility.ShowMsg(string.Format("\t\t向量 {0},{1},{2}\n", normal.X, normal.Y, normal.Z));
			Utility.ShowMsg(string.Format("\t\t厚度{0}\n", thickness));
            Utility.ShowMsg(string.Format("\t\t倾斜角{0}\n", oblique));
			Utility.ShowMsg(string.Format("\t\t旋转角 {0}\n", rotation));
			Utility.ShowMsg(string.Format("\t\t高度 {0}\n", height));
			Utility.ShowMsg(string.Format("\t\t宽度比例因子 {0}\n", widthFactor));
			Utility.ShowMsg(string.Format("\t\t镜像 X {0} Y {1}\n", isMirrorX, isMirrorY));
			Utility.ShowMsg(string.Format("\t\t水平模式 {0} 垂直{1}\n", horzMode, vertMode));
		}

        //--------------------------------------------------------------------------
        //
        // 功能：输出属性信息
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void DumpAttributeDefinition(Entity entity)
		{
			AttributeDefinition attributeDefinition = (AttributeDefinition)(entity);
			DumpText(attributeDefinition);

			try
			{
				string prompt = attributeDefinition.Prompt;
				string tag = attributeDefinition.Tag;
				bool isInvisible = attributeDefinition.Invisible;
				bool isConstant = attributeDefinition.Constant;
				bool isVerifiable = attributeDefinition.Verifiable;
				bool isPreset = attributeDefinition.Preset;
				UInt16 fieldLength = (UInt16)attributeDefinition.FieldLength;

				Utility.ShowMsg(string.Format("\t\t提示信息 : {0}\n", prompt));
				Utility.ShowMsg(string.Format("\t\tt标识 : {0}\n", tag));
				Utility.ShowMsg(string.Format("\t\t是否可见 : {0} 是否可变 {1}\n", isInvisible, isConstant));
				Utility.ShowMsg(string.Format("\t\ti是否垂直: {0}  是否预设 {1}\n", isVerifiable, isPreset));
				Utility.ShowMsg(string.Format("\t\t字段宽度 : {0}\n", fieldLength));
			}
			catch (Autodesk.AutoCAD.Runtime.Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}


        //--------------------------------------------------------------------------
        //
        // 功能：输出属性文本信息
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void DumpAttributeTextDialogSettings(Entity entity)
		{
			AttributeDefinition attDef = (AttributeDefinition)(entity);
			try
			{
				string expression = null;
				expression = m_annotations.GetExpressionString(attDef, AnnotationExpressionFields.AttributeDefinitionAnnotationString);
				Utility.ShowMsg(string.Format("\t\tTag = {0}. 	Expression string = {1}\n", attDef.Tag, expression));
			}
			catch (Autodesk.AutoCAD.Runtime.Exception err)
			{
				Utility.ShowMsg(err.Message);
			}

			try
			{
				string layerName = null;
				layerName = m_annotations.GetExpressionString(attDef, AnnotationExpressionFields.AttributeDefinitionTextLayer);
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Layer = {1}\n", attDef.Tag, layerName));
			}
			catch (Autodesk.AutoCAD.Runtime.Exception)
			{
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Layer (default/static) = {1}\n", attDef.Tag, attDef.Layer));
			}

			try
			{
				string colorMsg = null;
				colorMsg = m_annotations.GetExpressionString(attDef, AnnotationExpressionFields.AttributeDefinitionTextColor);
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Color = {1}\n", attDef.Tag, colorMsg));
			}
			catch (Autodesk.AutoCAD.Runtime.Exception)
			{
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Color (default/static) = {1}\n", attDef.Tag, attDef.Color.ColorIndex));
			}

			try
			{
				string lineweightMsg = null;
				lineweightMsg = m_annotations.GetExpressionString(attDef, AnnotationExpressionFields.AttributeDefinitionTextLineWeight);
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Line Weight = {1}\n", attDef.Tag, lineweightMsg));
			}
			catch (Autodesk.AutoCAD.Runtime.Exception)
			{
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Line Weight (default/static) = {1}\n", attDef.Tag, attDef.LineWeight));
			}

			try
			{
				string textstyleMsg = null;
				textstyleMsg = m_annotations.GetExpressionString(attDef, AnnotationExpressionFields.AttributeDefinitionTextStyle);
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Style = {1}\n", attDef.Tag, textstyleMsg));
			}
			catch (Autodesk.AutoCAD.Runtime.Exception)
			{
			}

			try
			{
				string textheightMsg = null;
				textheightMsg = m_annotations.GetExpressionString(attDef, AnnotationExpressionFields.AttributeDefinitionTextHeight);
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Height = {1}\n", attDef.Tag, textheightMsg));
			}
			catch (Autodesk.AutoCAD.Runtime.Exception)
			{
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Height (default/static) = {1}\n", attDef.Tag, attDef.Height));
			}

			try
			{
				string textrotationMsg = null;
				textrotationMsg = m_annotations.GetExpressionString(attDef, AnnotationExpressionFields.AttributeDefinitionTextRotation);
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Rotation = {1}\n", attDef.Tag, textrotationMsg));
			}
			catch (Autodesk.AutoCAD.Runtime.Exception)
			{
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Rotation (default/static) = {1}\n", attDef.Tag, attDef.Rotation));
			}

			try
			{
				string textjustificationMsg = null;
				textjustificationMsg = m_annotations.GetExpressionString(attDef, AnnotationExpressionFields.AttributeDefinitionTextJustification);
				Utility.ShowMsg(string.Format("\t\tTag = {0}.  Text Justification = {1}\n", attDef.Tag, textjustificationMsg));
			}
			catch (Autodesk.AutoCAD.Runtime.Exception)
			{
			}
		}

       //--------------------------------------------------------------------------
        //
        // 功能：获取属性定义
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void ReadAttributeDefinitions()
		{
			if (LB_ERR != listbTemplates.SelectedIndex)
			{
				// Get an object to the model_space block table record
				BlockTableRecord blkTblRcd = null;

				using (Transaction trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
				{
					// Get the block table	
					blkTblRcd = trans.GetObject(m_idTemplate, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead) as BlockTableRecord;

					if (null == blkTblRcd)
					{
						return;
					}

					// Check for all attributes
					if (!blkTblRcd.HasAttributeDefinitions)
					{
						return;
					}			
	
					foreach (ObjectId id in blkTblRcd)
					{
						Entity entity = null;
						
						try
						{
							entity = (Entity)(trans.GetObject(id, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead));

							AttributeDefinition attDef = null;
							attDef = (AttributeDefinition)(entity);				

							if (null != attDef)
							{
								if (!attDef.Constant)
								{
									Utility.ShowMsg(string.Format("\n********** 开始定义AttributeDefinition. Tag = {0} ********\n\n", attDef.Tag));
									DumpAttributeDefinition(attDef);
									DumpAttributeTextDialogSettings(attDef);
									Utility.ShowMsg(string.Format("\n********** 结束定义AttributeDefinition.   Tag = {0} ********\n\n", attDef.Tag));
								}
							}
						}
						catch (System.Exception e)
						{
							Utility.ShowMsg("\n Exception : " + e.Message);
						}
					}

					trans.Commit();
				}
			}
		}

        //--------------------------------------------------------------------------
        //
        // 功能：激活AutoCAD Map 3D的主窗口
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void BeginCommand()
		{
			Visible = false;
			EnableWindow((Int32)(Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Handle), true);
			SetWindowPos((Int32)(Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Handle), HWND_TOPMOST, 0, 0, 0, 0,
				SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
			SetFocus((Int32)(Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Handle));
		}


        //--------------------------------------------------------------------------
        //
        // 功能：激活对话框
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void EndCommand()
		{
			Visible = true;
			Enabled = true;
			EnableWindow((Int32)(Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Handle), false);
			SetWindowPos((Int32)(Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Handle), (Int32)(this.Handle), 0, 0, 0, 0,
				SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
			Focus();
		}


        //--------------------------------------------------------------------------
        //
        // 功能：重绘制当前视图
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		private void Regen()
		{
			Utility.SendCommand("_regen\n_runannoform\n");
		}
	}
}
