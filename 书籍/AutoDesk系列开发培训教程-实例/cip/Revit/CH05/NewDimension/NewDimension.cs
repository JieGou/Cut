using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;

namespace RevitDevelop
{
    public class NewDimension : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;
            Document curDoc = revit.ActiveDocument;
            SelElementSet selSet = curDoc.Selection.Elements;
            if (selSet.Size != 1)
            {
                message = "Please select a wall!";
                return IExternalCommand.Result.Cancelled;
            }
            Wall wall = null;
            ElementSetIterator it = selSet.ForwardIterator();
            if (it.MoveNext())
            {
                wall = it.Current as Wall;
            }
            if (wall == null)
            {
                message = "Please select a wall!";
                return IExternalCommand.Result.Cancelled;
            }
            LocationCurve cur = wall.Location as LocationCurve;
            if (cur == null)
                return IExternalCommand.Result.Failed;
            Line line = cur.Curve as Line;
            Options options = revit.Create.NewGeometryOptions();
            options.ComputeReferences = true;
            options.View = curDoc.ActiveView;
            Autodesk.Revit.Geometry.Element element = wall.get_Geometry(options);
            ReferenceArray referenceArray = new ReferenceArray();
            GeometryObjectArray geoObjectArray = element.Objects;
            //enum the geometry element
            for (int j = 0; j < geoObjectArray.Size; j++)
            {
                GeometryObject geoObject = geoObjectArray.get_Item(j);
                Line l0 = geoObject as Line;
                if (null != l0)
                {
                    //检查该线是否与线line正交
                    double d = (l0.get_EndPoint(1).X - l0.get_EndPoint(0).X) * (line.get_EndPoint(1).X - line.get_EndPoint(0).X);
                    d += (l0.get_EndPoint(1).Y - l0.get_EndPoint(0).Y) * (line.get_EndPoint(1).Y - line.get_EndPoint(0).Y);
                    d += (l0.get_EndPoint(1).Z - l0.get_EndPoint(0).Z) * (line.get_EndPoint(1).Z - line.get_EndPoint(0).Z);
                    if (d < 0.0000001 && d > -0.0000001)
                        referenceArray.Append(l0.Reference);
                    if (2 == referenceArray.Size)
                    {
                        break;
                    }
                }
            }
            Dimension createdDimension = curDoc.Create.NewDimension(curDoc.ActiveView, line, referenceArray);
            if (null == createdDimension)
            {
                message = "Create the Dimension failed.";
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
