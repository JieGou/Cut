using System;
using Autodesk.Revit;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;

namespace RevitDevelop
{
    public class NewFamilyInstance : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;
            Document curDoc = revit.ActiveDocument;
            FamilySymbol symbol = null;
            ElementSetIterator selIt = curDoc.Selection.Elements.ForwardIterator();
            if (selIt.MoveNext())
            {
                FamilyInstance fi = selIt.Current as FamilyInstance;
                symbol = fi.Symbol;
            }
            if (symbol == null)
            {
                message = "Please select a family instance!";
                return IExternalCommand.Result.Cancelled;
            }

            XYZ startPoint = new XYZ(0, 30, 0);
            FamilyInstance createdFamily = curDoc.Create.NewFamilyInstance(ref startPoint, symbol, Autodesk.Revit.Structural.Enums.StructuralType.UnknownFraming);
            if (null == createdFamily)
            {
                message = "Create the family failed.";
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
