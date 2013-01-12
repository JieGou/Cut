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
    public class CmdDrawingSet
    {

        //--------------------------------------------------------------------------
        //
        // 功能：输出图形集信息
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("PrintDrawSet")]
        public void PrintDrawSet()
        {

            // 获取Map 3D 程序对象和图形集
            MapApplication mapApi = HostMapApplicationServices.Application;
            ProjectModel proj = mapApi.ActiveProject;
            DrawingSet drawingSet = proj.DrawingSet;

            //输出图形集信息
            if (drawingSet.DirectDrawingsCount > 0)
                Utility.AcadEditor.WriteMessage("\n\t图形集:");

            for (int i = 0; i < drawingSet.DirectDrawingsCount; i++)
            {
                try
                {
                    AttachedDrawing drawingEntry = drawingSet.DirectAttachedDrawings[i];
                    PrintAttachedDrawingStatus(drawingEntry, 0);
                }
                catch (MapException e )
                {
                    Utility.AcadEditor.WriteMessage(e.Message);
                }
            }   

        }


        //--------------------------------------------------------------------------
        //
        // 功能：附着图形
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("AttachDraw")]
        public void AttachDraw()
        {

            // 获取Map 3D 程序对象和图形集
            MapApplication mapApi = HostMapApplicationServices.Application;
            ProjectModel proj = mapApi.ActiveProject;
            DrawingSet drawingSet = proj.DrawingSet;

            // 获取用户输入
            string strName = "";
            PromptResult promptResName = null;
            promptResName = Utility.AcadEditor.GetString("\n输入附着图形名称： ");
            if (promptResName.Status == PromptStatus.OK)
            {
                strName = promptResName.StringResult;
                try
                {
                    AttachedDrawing dwgDrawing = drawingSet.AttachDrawing(strName);
                    return;
                }
                catch (MapException e)
                {
                    Utility.AcadEditor.WriteMessage(e.Message);
                }
            }
            else
            {
                Utility.AcadEditor.WriteMessage("\n输入别名名称错误，退出。");
            }
            
        }

        //--------------------------------------------------------------------------
        //
        // 功能：附着图形
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("DetachDraw")]
        public void DetachDraw()
        {

            // 获取Map 3D 程序对象和图形集
            MapApplication mapApi = HostMapApplicationServices.Application;
            ProjectModel proj = mapApi.ActiveProject;
            DrawingSet drawingSet = proj.DrawingSet;

            // 获取用户输入
            string strName = "";
            PromptResult promptResName = null;
            promptResName = Utility.AcadEditor.GetString("\n输入附着图形名称： ");
            if (promptResName.Status == PromptStatus.OK)
            {
                strName = promptResName.StringResult;
                try
                {
                    drawingSet.DetachDrawing(strName);
                    return;
                }
                catch (MapException e)
                {
                    Utility.AcadEditor.WriteMessage(e.Message);
                }
            }
            else
            {
                Utility.AcadEditor.WriteMessage("\n输入别名名称错误，退出。");
                return;
            }

        }


        //--------------------------------------------------------------------------
        //
        // 功能：附着图形
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("ActivateDraw")]
        public void ActivateDraw()
        {

            // 获取Map 3D 程序对象和图形集
            MapApplication mapApi = HostMapApplicationServices.Application;
            ProjectModel proj = mapApi.ActiveProject;
            DrawingSet drawingSet = proj.DrawingSet;

            // 获取用户输入
            string strName = "";
            PromptResult promptResName = null;
            promptResName = Utility.AcadEditor.GetString("\n输入要激活的附着图形名称： ");
            if (promptResName.Status == PromptStatus.OK)
            {
                strName = promptResName.StringResult;
                try
                {
                    AttachedDrawing dwgDrawing = drawingSet.DirectAttachedDrawings[strName];
                    dwgDrawing.Activate();
                    return;
                }
                catch (MapException e)
                {
                    Utility.AcadEditor.WriteMessage(e.Message);
                }
            }
            else
            {
                Utility.AcadEditor.WriteMessage("\n输入别名名称错误，退出。");
                return;
            }

        }


        //--------------------------------------------------------------------------
        //
        // 功能：附着图形
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("DeactivateDraw")]
        public void DeactivateDraw()
        {

            // 获取Map 3D 程序对象和图形集
            MapApplication mapApi = HostMapApplicationServices.Application;
            ProjectModel proj = mapApi.ActiveProject;
            DrawingSet drawingSet = proj.DrawingSet;

            // 获取用户输入
            string strName = "";
            PromptResult promptResName = null;
            promptResName = Utility.AcadEditor.GetString("\n输入要取消激活的附着图形名称： ");
            if (promptResName.Status == PromptStatus.OK)
            {
                strName = promptResName.StringResult;
                try
                {
                    AttachedDrawing dwgDrawing = drawingSet.DirectAttachedDrawings[strName];
                    dwgDrawing.Deactivate();
                    return;
                }
                catch (MapException e)
                {
                    Utility.AcadEditor.WriteMessage(e.Message);
                }
            }
            else
            {
                Utility.AcadEditor.WriteMessage("\n输入别名名称错误，退出。");
                return;
            }

        }
        //--------------------------------------------------------------------------
        //
        // 功能：输出附着图形信息
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        private void PrintAttachedDrawingStatus(AttachedDrawing drawing, int nestedLevel)
        {
            if (null == drawing)
                return;

            Utility.AcadEditor.WriteMessage("\n  ");
            for (int i = 0; i < nestedLevel; i++)
            {
                Utility.AcadEditor.WriteMessage("－");
            }
                    Utility.AcadEditor.WriteMessage("路径：描述");

            switch (drawing.ActiveStatus)
            {
                case AdeDwgStatus.DwgInactive:
                    Utility.AcadEditor.WriteMessage(string.Format("未激活 {0} ({1})", drawing.AliasPath, drawing.Description));
                    break;
                case AdeDwgStatus.DwgActive:
                    Utility.AcadEditor.WriteMessage(string.Format("激活 {0} ({1})", drawing.AliasPath, drawing.Description));
                    break;
                case AdeDwgStatus.DwgLocked:
                    Utility.AcadEditor.WriteMessage(string.Format("锁定 {0} ({1})", drawing.AliasPath, drawing.Description));
                    break;
            }

            for (int i = 0; i < drawing.DirectNestedDrawingsCount; i++)
            {
                AttachedDrawing nestedDrawing = drawing.DirectNestedDrawings[i];
                if (nestedDrawing != null)
                {
                    // 
                    PrintAttachedDrawingStatus(nestedDrawing, nestedDrawing.NestedLevel);
                }
            }
        }

        // end of class
    }
}
