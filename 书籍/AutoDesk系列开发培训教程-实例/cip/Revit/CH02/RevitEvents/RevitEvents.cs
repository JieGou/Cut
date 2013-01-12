using System;
using Autodesk.Revit;
using Autodesk.Revit.Events;
using System.Windows.Forms;

namespace RevitDevelop
{
    public class RevitEvents : IExternalApplication
    {
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            application.OnDocumentOpened += new Autodesk.Revit.Events.DocumentOpenedEventHandler(onDocOpened);

            return IExternalApplication.Result.Succeeded;
        }

        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {
            return IExternalApplication.Result.Succeeded;
        }

        public void onDocOpened(Document document)
        {
            MessageBox.Show(document.PathName);
        }
    }
}
