using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Symbols;

namespace RevitDevelop
{
    public class NewFloor : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;
            Document curDoc = revit.ActiveDocument;

            //配置几何曲线
            CurveArray curves = new CurveArray();
            if (null == curves)
            {
                message = "Create the curves failed.";
                return IExternalCommand.Result.Failed;
            }
            XYZ first = new XYZ(0, 0, 0);
            XYZ second = new XYZ(10, 0, 0);
            XYZ third = new XYZ(10, 10, 0);
            XYZ fourth = new XYZ(0, 10, 0);
            curves.Append(revit.Create.NewLine(ref first, ref second, true));
            curves.Append(revit.Create.NewLine(ref second, ref third, true));
            curves.Append(revit.Create.NewLine(ref third, ref fourth, true));
            curves.Append(revit.Create.NewLine(ref fourth, ref first, true));
            // 利用几何曲线，类型，标高等创建地板对象
            Floor createdFloor = curDoc.Create.NewFloor(curves, true);
            if (null == createdFloor)
            {
                message = "Create floor failed.!";
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
