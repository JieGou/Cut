using System;
using System.Windows.Forms;
using Autodesk.Revit;

namespace RevitDevelop
{
    public class HelloRevit : Autodesk.Revit.IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                MessageBox.Show("Hello Revit!");
            }
            catch (Exception e)
            {
                message = e.Message;
                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
