using System;
using Autodesk.Revit;
using System.Windows.Forms;
using Autodesk.Revit.Symbols;

namespace RevitDevelop
{
    public class ShowAllFurnitures : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string symbols = "";
            ElementIterator it = commandData.Application.ActiveDocument.Elements;
            while (it.MoveNext())
            {
                FamilySymbol famSym = it.Current as FamilySymbol;
                if (famSym == null || famSym.Category == null) continue;
                if (famSym.Category.Name.Equals("家具"))
                {
                    if (famSym.Family != null)
                        symbols += famSym.Family.Name;
                    symbols += " : " + famSym.Name + "\n";
                }
            }
            if (symbols.Length > 0)
                MessageBox.Show(symbols);

            return IExternalCommand.Result.Succeeded;
        }
    }
}
