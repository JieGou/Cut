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
    public class CmdHandler
    {
        //--------------------------------------------------------------------------
        //
        // 功能：注册图形集的事件监控
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("RegHandlers")]
        public void CmdRegisterHandlers()
        {
            MapApplication mapApi = HostMapApplicationServices.Application;
            ProjectModel proj = mapApi.ActiveProject;
            DrawingSet drawingSet = proj.DrawingSet;

                // MapDrawingSetEventHandler
                drawingSet.DrawingActivated += new DrawingActivatedEventHandler(DrawingSetHandler.DrawingActivatedHandler);
                drawingSet.DrawingToBeAttached += new DrawingToBeAttachedEventHandler(DrawingSetHandler.DrawingToBeAttachedHandler);
                drawingSet.DrawingAttachCancelled += new DrawingAttachCancelledEventHandler(DrawingSetHandler.DrawingAttachCancelledHandler);
                drawingSet.DrawingAttached += new DrawingAttachedEventHandler(DrawingSetHandler.DrawingAttachedHandler);
                drawingSet.DrawingDetached += new DrawingDetachedEventHandler(DrawingSetHandler.DrawingDetachedHandler);
                drawingSet.DrawingToBeActivated += new DrawingToBeActivatedEventHandler(DrawingSetHandler.DrawingToBeActivatedHandler);
                drawingSet.DrawingActivationCancelled += new DrawingActivationCancelledEventHandler(DrawingSetHandler.DrawingActivationCancelledHandler);
                drawingSet.DrawingDeactivated += new DrawingDeactivatedEventHandler(DrawingSetHandler.DrawingDeactivatedHandler);
                drawingSet.DrawingSettingsModified += new DrawingSettingsModifiedEventHandler(DrawingSetHandler.DrawingSettingsModifiedHandler);

                Utility.AcadEditor.WriteMessage("\n 对图形集的事件监控已经已经注册成功 。");
                Utility.AcadEditor.WriteMessage("\n 可以执行 'UnregisterHandlers' 取消事件的监控。 ");
        }

        //--------------------------------------------------------------------------
        //
        // 功能：取消图形集的事件监控
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------

        [CommandMethod("UnregHandlers")]
        public void UnregisterHandlers()
        {
                MapApplication mapApi = HostMapApplicationServices.Application;

                ProjectModel proj = mapApi.ActiveProject;
                DrawingSet drawingSet = proj.DrawingSet;

                // MapDrawingSetEventHandler
                drawingSet.DrawingActivated -= new DrawingActivatedEventHandler(DrawingSetHandler.DrawingActivatedHandler);
                drawingSet.DrawingToBeAttached -= new DrawingToBeAttachedEventHandler(DrawingSetHandler.DrawingToBeAttachedHandler);
                drawingSet.DrawingAttachCancelled -= new DrawingAttachCancelledEventHandler(DrawingSetHandler.DrawingAttachCancelledHandler);
                drawingSet.DrawingAttached -= new DrawingAttachedEventHandler(DrawingSetHandler.DrawingAttachedHandler);
                drawingSet.DrawingDetached -= new DrawingDetachedEventHandler(DrawingSetHandler.DrawingDetachedHandler);
                drawingSet.DrawingToBeActivated -= new DrawingToBeActivatedEventHandler(DrawingSetHandler.DrawingToBeActivatedHandler);
                drawingSet.DrawingActivationCancelled -= new DrawingActivationCancelledEventHandler(DrawingSetHandler.DrawingActivationCancelledHandler);
                drawingSet.DrawingDeactivated -= new DrawingDeactivatedEventHandler(DrawingSetHandler.DrawingDeactivatedHandler);
                drawingSet.DrawingSettingsModified -= new DrawingSettingsModifiedEventHandler(DrawingSetHandler.DrawingSettingsModifiedHandler);

                Utility.AcadEditor.WriteMessage("\n 已经取消对图形集的事件监控。 ");

            
        }
    }
}
