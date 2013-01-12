using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Symbols;

namespace RevitDevelop
{
    public class NewWall : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;
            Document curDoc = revit.ActiveDocument;
            XYZ startPoint = new XYZ(0, 0, 0);
            XYZ endPoint = new XYZ(10, 0, 0);
            WallType wallType = null;
            WallTypeSetIterator it = curDoc.WallTypes.ForwardIterator();
            if (it.MoveNext())
            {
                wallType = it.Current as WallType;
            }
            if (wallType == null)
            {
                message = "No any wall type in current document!";
                return IExternalCommand.Result.Failed;
            }
            Level createlevel = curDoc.ActiveView.GenLevel;
            //创建几何曲线
            Line geometryLine = revit.Create.NewLine(ref startPoint, ref endPoint, true);
            if (null == geometryLine)
            {
                message = "Create the geometry line failed.";
                return IExternalCommand.Result.Failed;
            }
            // 利用几何曲线，墙类型，标高创建墙对象
            Wall createdWall = curDoc.Create.NewWall(geometryLine, wallType, createlevel, 10, startPoint.Z + createlevel.Elevation, true, true);
            if (null == createdWall)
            {
                message = "Create the wall failed.";
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
