using System;
using Autodesk.Revit;
using System.Collections.Generic;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;

namespace RevitDevelop
{
    public class GetFace : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;
            Document curDoc = revit.ActiveDocument;

            FaceArray faces = null;
            List<double> areas = new List<double>();

            Options geoOptions = revit.Create.NewGeometryOptions();
            geoOptions.ComputeReferences = true;

            ElementSetIterator it = curDoc.Selection.Elements.ForwardIterator();
            while (it.MoveNext())
            {
                Autodesk.Revit.Element e = it.Current as Autodesk.Revit.Element;
                if (e == null) continue;
                Autodesk.Revit.Geometry.Element geoElem = e.get_Geometry(geoOptions);
                GeometryObjectArray geoElems = geoElem.Objects;

                foreach (object o in geoElems)
                {
                    Solid geoSolid = o as Solid;
                    if (null == geoSolid)
                    {
                        continue;
                    }

                    faces = geoSolid.Faces;
                }
                if (faces != null)
                    foreach (Face face in faces)
                    {
                        areas.Add(face.Area);
                    }
            }
            return IExternalCommand.Result.Succeeded;
        }
    }
}
