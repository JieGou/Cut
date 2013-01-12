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
    //--------------------------------------------------------------------------
    //
    // 功能：图形集的事件处理类
    //
    //  作者： 
    //
    //  日期：200708
    //
    //   历史：
    //--------------------------------------------------------------------------
    public sealed class DrawingSetHandler
    {
        internal static void DrawingToBeAttachedHandler(Object sender, DrawingToBeAttachedEventArgs args)
        {
            Utility.AcadEditor.WriteMessage(String.Format("\nDrawingSetEventHandler.DrawingToBeAttachedHandler({0});", args.AliasPath));
        }

        internal static void DrawingAttachCancelledHandler(Object sender, DrawingAttachCancelledEventArgs args)
        {
            Utility.AcadEditor.WriteMessage("\nDrawingSetEventHandler.DrawingAttachCancelledHandler();");
        }

        internal static void DrawingAttachedHandler(Object sender, DrawingAttachedEventArgs args)
        {
            Utility.AcadEditor.WriteMessage("\nDrawingSetEventHandler.DrawingAttachedHandler();");
        }

        internal static void DrawingDetachedHandler(Object sender, DrawingDetachedEventArgs args)
        {
            Utility.AcadEditor.WriteMessage("\nDrawingSetEventHandler.DrawingDetachedHandler();");
        }

        internal static void DrawingToBeActivatedHandler(Object sender, DrawingToBeActivatedEventArgs args)
        {
            Utility.AcadEditor.WriteMessage("\nDrawingSetEventHandler.DrawingToBeActivatedHandler();");
        }

        internal static void DrawingActivationCancelledHandler(Object sender, DrawingActivationCancelledEventArgs args)
        {
            Utility.AcadEditor.WriteMessage("\nDrawingSetEventHandler.DrawingActivationCancelledHandler();");
        }

        internal static void DrawingActivatedHandler(Object sender, DrawingActivatedEventArgs args)
        {
            Utility.AcadEditor.WriteMessage("\nDrawingSetEventHandler.DrawingActivatedHandler();");
        }

        internal static void DrawingDeactivatedHandler(Object sender, DrawingDeactivatedEventArgs args)
        {
            Utility.AcadEditor.WriteMessage("\nDrawingSetEventHandler.DrawingDeactivatedHandler();");
        }

        internal static void DrawingSettingsModifiedHandler(Object sender, DrawingSettingsModifiedEventArgs args)
        {
            Utility.AcadEditor.WriteMessage("\nDrawingSetEventHandler.DrawingSettingsModifiedHandler();");
        }

        private DrawingSetHandler()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
