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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

// Utility.cs

namespace CH06
{
    //--------------------------------------------------------------------------
    //
    // 功能：工具类，提供用户交互函数
    //
    //  作者： 
    //
    //  日期：200708
    //
    //   历史：
    //--------------------------------------------------------------------------
    internal sealed class Utility
    {  
        private Utility()
        {
        }

        public static Autodesk.AutoCAD.EditorInput.Editor Editor
        {
            get
            {
                return Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            }
        }

        public static void ShowMessage(string message)
        {
            Editor.WriteMessage(message);
        }
  
        public static Autodesk.AutoCAD.DatabaseServices.TransactionManager TransactionManager
        {
            get
            {
                return Autodesk.Gis.Map.HostMapApplicationServices.Application.ActiveProject.Database.TransactionManager;
            }
        }
    }
}
