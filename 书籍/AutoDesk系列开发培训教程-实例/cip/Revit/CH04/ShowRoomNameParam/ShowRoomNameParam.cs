using System;
using Autodesk.Revit;
using System.Windows.Forms;
using Autodesk.Revit.Elements;

namespace RevitDevelop
{
    public class ShowRoomNameParam : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SelElementSet selSet = commandData.Application.ActiveDocument.Selection.Elements;
            if (selSet.Size != 1)	//仅当只选一个Room时才显示
            {
                return IExternalCommand.Result.Cancelled;
            }
            ElementSetIterator it = selSet.ForwardIterator();
            if (it.MoveNext())
            {
                Room room = it.Current as Room;
                if (room != null)
                {
                    Parameter para = room.get_Parameter(Autodesk.Revit.Parameters.BuiltInParameter.ROOM_NAME);
                    if (para != null)
                    {
                        MessageBox.Show(para.AsString());
                    }
                }
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
