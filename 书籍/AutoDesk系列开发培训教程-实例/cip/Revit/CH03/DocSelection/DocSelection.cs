using System;
using Autodesk.Revit;
using System.Windows.Forms;

namespace RevitDevelop
{
    public class DocSelection : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.Application revit = commandData.Application;
            SelElementSet selectionSet = revit.ActiveDocument.Selection.Elements;
            MessageBox.Show(selectionSet.Size.ToString());

            return IExternalCommand.Result.Succeeded;
        }
    }
}
