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

// AnnotationCS.cs

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;

namespace CH04
{
	public sealed class AnnotationSampleApp : IExtensionApplication
	{
		public void Initialize()
		{
			Utility.ShowMsg("\n注释示例程序初始化。");
			SampleCommand.CmdList();
		}
		public void Terminate()
		{
            Utility.ShowMsg("\n注释示例程序终止。");
		}
	}

	public sealed class SampleCommand
	{
		private AnnotationDlg m_form = null;
        //--------------------------------------------------------------------------
        //
        // 功能：定义命令
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		[Autodesk.AutoCAD.Runtime.CommandMethodAttribute("RunAnnoForm")]
		public void RunAnnoForm()
		{
    	    if (null == m_form)
			{
				m_form = new AnnotationDlg();
			}
			Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(
				Autodesk.AutoCAD.ApplicationServices.Application.MainWindow, m_form);
		}

		/// <summary>
		/// Lists the Commands this sample provided.
		/// </summary>
		[Autodesk.AutoCAD.Runtime.CommandMethodAttribute("CmdList")]
		public static void CmdList()
		{
            Utility.ShowMsg("\n 注释示例程序定义的命令 :\n");
			Utility.ShowMsg("** Cmd : RunAnnoForm\n");
		}
	}

	public sealed class Utility
	{
        //--------------------------------------------------------------------------
        //
        // 功能：输出消息
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		public static void ShowMsg(string msg)
		{
			AcadEditor.WriteMessage(msg);
		}
        //--------------------------------------------------------------------------
        //
        // 功能：调用AutoCAD Editor对象
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		public static Autodesk.AutoCAD.EditorInput.Editor AcadEditor
		{
			get 
			{
				return Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
			}
		}

        //--------------------------------------------------------------------------
        //
        // 功能：调用AutoCAD Map 3D 内部命令
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
		public static bool SendCommand(string cmd)
		{
			try
			{
				Autodesk.AutoCAD.ApplicationServices.Document doc = 
					Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
				doc.SendStringToExecute(cmd, true, false, true);
				return true;
			}
			catch (System.Exception e)
			{
				ShowMsg(e.Message);
				return false;
			}
		}

		private Utility()
		{
		}
	}
}
