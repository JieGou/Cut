using System;
using Autodesk.Revit;
using Autodesk.Revit.Geometry;

namespace RevitDevelop
{
    public class NewLine : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;
            XYZ m_startPoint = new XYZ(0, 0, 0);
            XYZ m_endPoint = new XYZ(20, 0, 0);
            //创建过两点的直线
            Line geometryLine = revit.Create.NewLine(ref m_startPoint, ref m_endPoint, true);
            if (null == geometryLine)
            {
                message = "Create the geometry line failed.";
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
