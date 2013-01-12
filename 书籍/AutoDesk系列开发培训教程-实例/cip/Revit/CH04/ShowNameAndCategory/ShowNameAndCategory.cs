using System;
using Autodesk.Revit;
using System.Windows.Forms;

namespace RevitDevelop
{
    public class ShowNameAndCategory : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ElementSetIterator it = commandData.Application.ActiveDocument.Selection.Elements.ForwardIterator();
            while (it.MoveNext())
            {
                Element e = it.Current as Element;
                if (e == null) continue;
                string name;
                name = "Name: " + e.Name + "; ";
                if (e.Category == null)
                    name += "Category: null";
                else
                    name += "Category: " + e.Category.Name;
                MessageBox.Show(name);
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
