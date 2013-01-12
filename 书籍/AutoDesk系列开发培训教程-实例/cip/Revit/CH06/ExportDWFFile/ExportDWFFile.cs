using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace RevitDevelop
{
    public class ExportDWFFile : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document curDoc = commandData.Application.ActiveDocument;
            DWF2DExportOptions options = new DWF2DExportOptions();
            ViewSet viewSet = new ViewSet();

            viewSet.Insert(curDoc.ActiveView);
            if (curDoc.Export("C:\\", "abc.dwf", viewSet, options) == false)
            {
                message = "Export current view to DWF file failed.";
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
