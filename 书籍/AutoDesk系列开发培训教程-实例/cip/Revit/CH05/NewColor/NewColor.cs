using System;
using Autodesk.Revit;

namespace RevitDevelop
{
    public class NewColor : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application revit = commandData.Application;

            //创建红色的颜色对象
            Color createdColor = revit.Create.NewColor();
            if (null == createdColor)
            {
                message = "Create the color failed.";
                return IExternalCommand.Result.Failed;
            }
            createdColor.Red = 255;
            createdColor.Green = 0;
            createdColor.Blue = 0;

            return IExternalCommand.Result.Succeeded;
        }
    }
}
