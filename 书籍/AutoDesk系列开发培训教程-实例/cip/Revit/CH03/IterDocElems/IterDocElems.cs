using System;
using Autodesk.Revit;
using System.Windows.Forms;

namespace RevitDevelop
{
    public class IterDocElems : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            bool bFound = false;
            Autodesk.Revit.Application revit = commandData.Application;
            ElementIterator it = revit.ActiveDocument.Elements;
            while (it.MoveNext())
            {
                Element e = it.Current as Element;
                if (e == null || e.Category == null) continue;
                if (e.Category.Name.Equals("房间") == true)
                {
                    bFound = true;
                    break;
                }
            }
            if (bFound == true)
                MessageBox.Show("Found Room");
            else
                MessageBox.Show("Not found Room");

            return IExternalCommand.Result.Succeeded;
        }
    }
}
