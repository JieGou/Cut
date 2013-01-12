using System;
using Autodesk.Revit;
using System.Windows.Forms;

namespace RevitDevelop
{
    public class ShowVersion : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.Application revit = commandData.Application;
            string version = revit.VersionBuild;
            version += "\n" + revit.VersionName;
            version += "\n" + revit.VersionNumber;
            MessageBox.Show(version);

            return IExternalCommand.Result.Succeeded;
        }
    }
}
