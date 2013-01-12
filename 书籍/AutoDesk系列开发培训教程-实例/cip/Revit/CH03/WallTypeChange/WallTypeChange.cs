using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;

namespace RevitDevelop
{
    public class WallTypeChange : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;
            Document curDoc = revit.ActiveDocument;
            SelElementSet selectionSet = curDoc.Selection.Elements;
            ElementSetIterator it = selectionSet.ForwardIterator();
            while (it.MoveNext())
            {
                Wall w = it.Current as Wall;
                if (w == null) continue;
                WallTypeSetIterator typeIt = curDoc.WallTypes.ForwardIterator();
                while (typeIt.MoveNext())
                {
                    WallType wallType = typeIt.Current as WallType;
                    if (wallType.Id.Equals(w.WallType.Id) == false)
                    {
                        w.WallType = wallType;
                        break;
                    }
                }
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
