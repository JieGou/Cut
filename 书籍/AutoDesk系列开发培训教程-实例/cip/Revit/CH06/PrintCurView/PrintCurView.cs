using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace RevitDevelop
{
    public class PrintCurView : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document curDoc = commandData.Application.ActiveDocument;
            View curView = curDoc.ActiveView;

            if (curView.CanBePrinted)
                curView.Print();

            return IExternalCommand.Result.Succeeded;
        }
    }
}
