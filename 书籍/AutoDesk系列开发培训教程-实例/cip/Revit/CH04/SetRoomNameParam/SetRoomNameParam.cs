using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace RevitDevelop
{
    public class SetRoomNameParam : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SelElementSet selSet = commandData.Application.ActiveDocument.Selection.Elements;
            if (selSet.Size != 1)
            {
                return IExternalCommand.Result.Cancelled;
            }
            ElementSetIterator it = selSet.ForwardIterator();
            if (it.MoveNext())
            {
                Room room = it.Current as Room;
                if (room != null)
                {
                    ParameterSetIterator paraIt = room.Parameters.ForwardIterator();
                    while (paraIt.MoveNext())
                    {
                        Parameter para = paraIt.Current as Parameter;
                        if (para == null) continue;
                        if (para.Definition.Name.Equals("名称"))
                        {
                            para.Set("My Room");
                            break;
                        }
                    }
                }
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
