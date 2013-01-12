//
// (C) Copyright 2004-2007 by Autodesk, Inc.
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


using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

using Autodesk.Gis.Map;
using Autodesk.Gis.Map.Utilities;
using Autodesk.Gis.Map.Project;
using Autodesk.Gis.Map.Query;
using Autodesk.Gis.Map.Constants;

namespace CH02
{
    public class CmdAliase
    {
        //--------------------------------------------------------------------------
        //
        // 功能：增加驱动器别名
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("AddAliases")]
        public void AddAliases()
        {
            //获取Map3D程序对象
            MapApplication mapApi = HostMapApplicationServices.Application;
            Aliases aliases = mapApi.Aliases;
            // 获取用户输入
            string strName = "";
            string strPath = "";
            PromptResult promptResName = null;
            PromptResult promptResPath = null;

            promptResName = Utility.AcadEditor.GetString("\n输入驱动器别名名称： ");
            if (promptResName.Status == PromptStatus.OK)
            {
                strName = promptResName.StringResult;
            }
            else
            {
                Utility.AcadEditor.WriteMessage("\n输入驱动器别名名称错误，退出。");
                return;
            }
             
            promptResPath = Utility.AcadEditor.GetString("\n输入关联路径: ");
            if (promptResPath.Status == PromptStatus.OK)
            {
                strPath = promptResPath.StringResult;
            }
            else
            {
                Utility.AcadEditor.WriteMessage("\n输入关联路径错误，退出。");
                return;
            }
            //  增加别名
            try
            {
                aliases.AddAlias(strName, strPath);
            }
            catch (MapException e)
            {
                Utility.AcadEditor.WriteMessage(e.Message);
            }

        }


        //--------------------------------------------------------------------------
        //
        // 功能：输出所有别名
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("PrintAliases")]
        public void PrintAliases()
        {
            //获取Map3D程序对象
            MapApplication mapApi = HostMapApplicationServices.Application;

            Aliases aliases = mapApi.Aliases;
            if (aliases.AliasesCount > 0)
                Utility.AcadEditor.WriteMessage("\n所有别名:");
            Utility.AcadEditor.WriteMessage("\n名称:路径");
            //输出所有别名
            for (int i = 0; i < aliases.AliasesCount; i++)
            {
                try
                {
                    DriveAlias alias = aliases[i];
                    Utility.AcadEditor.WriteMessage(string.Format("\n{0}:\t{1}\t= {2}", i, alias.Name, alias.Path));
                }
                catch (MapException e)
                {
                    Utility.AcadEditor.WriteMessage(e.Message);
                }
            }
        }

        //--------------------------------------------------------------------------
        //
        // 功能：删除别名
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("RemoveAliase")]
        public void RemoveAliase()
        {
            //获取Map3D程序对象
            MapApplication mapApi = HostMapApplicationServices.Application;

            Aliases aliases = mapApi.Aliases;
            // 获取用户输入
            string strName = "";
            PromptResult promptResName = null;
            promptResName = Utility.AcadEditor.GetString("\n指定要删除的驱动器别名: ");
            strName = promptResName.StringResult;
            try
            {
                aliases.RemoveAlias(strName);
            }
            catch (MapException)
            {
                Utility.AcadEditor.WriteMessage("\n删除驱动器别名失败。");
            }

   
        }

    }

    //--------------------------------------------------------------------------
    //
    // 功能：功能函数类，用于获取Editor
    //
    //  作者： 
    //
    //  日期：200708
    //
    //   历史：
    //--------------------------------------------------------------------------
    public  class Utility
    {
        private Utility()
        {
        }

        public static Autodesk.AutoCAD.EditorInput.Editor AcadEditor
        {
            get
            {
                return Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            }
        }
    }
}
