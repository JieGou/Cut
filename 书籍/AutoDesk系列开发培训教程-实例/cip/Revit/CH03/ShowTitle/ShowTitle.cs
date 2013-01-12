using System;
using Autodesk.Revit;
using System.Windows.Forms;

namespace RevitDevelop
{
    public class ShowTitle : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.Application revit = commandData.Application;
            Document curDoc = revit.ActiveDocument;
            MessageBox.Show(curDoc.Title);

            return IExternalCommand.Result.Succeeded;
        }
    }
}
