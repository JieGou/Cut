using System;
using Autodesk.Revit;
using Autodesk.Revit.Geometry;

namespace RevitDevelop
{
    public class NewArc : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;

            //配置几何曲线
            XYZ end0 = new XYZ(0, 0, 0);
            XYZ end1 = new XYZ(3000, 0, 0);
            XYZ pointOnCurve = new XYZ(1500, 1500, 0);
            // 通过两端点和弧线上一点创建弧线
            Arc createdArc = revit.Create.NewArc(ref end0, ref end1, ref pointOnCurve);
            if (null == createdArc)
            {
                message = "Create the Arc failed.";
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
