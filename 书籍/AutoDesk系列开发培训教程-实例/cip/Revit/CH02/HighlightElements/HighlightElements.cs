using System;
using Autodesk.Revit;

namespace RevitDevelop
{
    public class HighlightElements : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            int ii = 0;
            message = "Error elements with highlight"; //这行不能少，必须给message赋值
            ElementSetIterator it = commandData.Application.ActiveDocument.Selection.Elements.ForwardIterator();
            while (it.MoveNext())
            {
                Element e = it.Current as Element;
                if (e == null) continue;
                elements.Insert(e);
                if (++ii > 2) break;
            }

            return IExternalCommand.Result.Failed;
        }
    }
}
