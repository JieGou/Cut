using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Symbols;

namespace RevitDevelop
{
    public class NewRoom : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;
            Document curDoc = revit.ActiveDocument;

            Level createlevel = curDoc.ActiveView.GenLevel;
            UV point = new UV(0, 0);
            // 利用标高，坐标创建房间对象
            Room createdRoom = curDoc.Create.NewRoom(createlevel, ref point);
            if (null == createdRoom)
            {
                message = "Create the wall failed.";
                return IExternalCommand.Result.Failed;
            }
            curDoc.Create.NewRoomTag(createdRoom, ref point);

            return IExternalCommand.Result.Succeeded;
        }
    }
}
