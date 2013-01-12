using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;

namespace RevitDevelop
{
    public class New3DView : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;
            Document curDoc = revit.ActiveDocument;
            XYZ viewDirection = new XYZ(10, 10, -10);
            // 利用区域范围创建一个剖面视图
            View3D view3d = curDoc.Create.NewView3D(ref viewDirection);
            if (null == view3d)
            {
                message = "Create the View3D failed.";
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
