using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace RevitDevelop
{
    public class PrintAllViews : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document curDoc = commandData.Application.ActiveDocument;
            ElementFilterIterator it = curDoc.get_Elements(typeof(View));
            while (it.MoveNext())
            {
                View view = it.Current as View;
                if (view == null) continue;
                if (view.CanBePrinted)
                    view.Print();
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
