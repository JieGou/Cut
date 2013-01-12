using System;
using Autodesk.Revit;
using System.Windows.Forms;
using Autodesk.Revit.Elements;

namespace RevitDevelop
{
    public class FindRoomIndex : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.Application revit = commandData.Application;
            SelElementSet selectionSet = revit.ActiveDocument.Selection.Elements;
            ElementSetIterator it = selectionSet.ForwardIterator();
            while (it.MoveNext())
            {
                FamilyInstance fi = it.Current as FamilyInstance;
                if (fi == null) continue;
                if (fi.Room == null)
                    MessageBox.Show("Doesn't belong to any room!");
                else
                    MessageBox.Show("Belongs to " + fi.Room.Number);
                break;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
