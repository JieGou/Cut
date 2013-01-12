using System;
using Autodesk.Revit;
using System.Windows.Forms;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;

namespace RevitDevelop
{
    public class ShowRoomParams : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SelElementSet selSet = commandData.Application.ActiveDocument.Selection.Elements;
            if (selSet.Size != 1)
            {
                message = "Please only select one room!";
                return IExternalCommand.Result.Cancelled;
            }
            ElementSetIterator it = selSet.ForwardIterator();
            if (it.MoveNext())
            {
                Room room = it.Current as Room;
                if (room != null)
                {
                    string showMsg = "Name: " + room.Name + "\nNumber: " + room.Number + "\nArea: ";
                    Parameter para = room.get_Parameter(BuiltInParameter.ROOM_AREA);
                    if (para.StorageType == StorageType.Double)
                    {
                        showMsg += para.AsDouble().ToString();
                        switch (para.DisplayUnitType)
                        {
                            case Autodesk.Revit.Enums.DisplayUnitType.DUT_SQUARE_METERS:
                                showMsg += " square meters";
                                break;
                            case Autodesk.Revit.Enums.DisplayUnitType.DUT_SQUARE_FEET:
                                showMsg += " square feet";
                                break;
                        }
                    }
                    MessageBox.Show(showMsg);
                }
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
