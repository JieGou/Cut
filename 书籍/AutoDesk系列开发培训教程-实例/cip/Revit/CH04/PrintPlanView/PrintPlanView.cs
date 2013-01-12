using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace RevitDevelop
{
    public class PrintPlanView : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document curDoc = commandData.Application.ActiveDocument;
            View curView = curDoc.ActiveView;
            if (curView.ViewType != Autodesk.Revit.Enums.ViewType.FloorPlan)
            {
                message = "Please swith to a Floor plan view";
                return IExternalCommand.Result.Cancelled;
            }
            SelElementSet selSet = curDoc.Selection.Elements;
            if (selSet.Size != 1)
            {
                message = "Please only select one room!";
                return IExternalCommand.Result.Cancelled;
            }
            ElementSetIterator it = selSet.ForwardIterator();
            if (it.MoveNext())
            {
                Element e = it.Current as Element;
                if (e != null && e.Category != null)
                {
                    curView.setVisibility(e.Category, false);
                    if (curView.CanBePrinted == true)
                        curView.Print();
                    curView.setVisibility(e.Category, true);
                }
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
