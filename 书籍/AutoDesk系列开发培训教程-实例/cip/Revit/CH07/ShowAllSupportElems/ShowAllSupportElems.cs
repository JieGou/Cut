using System;
using Autodesk.Revit;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Elements;
using System.Windows.Forms;

namespace RevitDevelop
{
    public class ShowAllSupportElems : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document curDoc = commandData.Application.ActiveDocument;
            string showMsg = "";
            AnalyticalSupportData supportData = null;
            ElementSetIterator it = curDoc.Selection.Elements.ForwardIterator();
            if (it.MoveNext())
            {
                Element e = it.Current as Element;
                if (e == null)
                {
                    message = "Please select a wall/Floor/Beam!";
                    return IExternalCommand.Result.Cancelled;
                }
                if (e is Wall)
                {
                    Wall w = e as Wall;
                    AnalyticalModelWall wallModel = w.AnalyticalModel as AnalyticalModelWall;
                    supportData = wallModel.SupportData;
                }
                else if (e is Floor)
                {
                    Floor f = e as Floor;
                    AnalyticalModelFloor floorModel = f.AnalyticalModel as AnalyticalModelFloor;
                    supportData = floorModel.SupportData;
                }
                else if (e is FamilyInstance)
                {
                    FamilyInstance fi = e as FamilyInstance;
                    AnalyticalModelFrame frameModel = fi.AnalyticalModel as AnalyticalModelFrame;
                    if (frameModel != null)
                        supportData = frameModel.SupportData;
                }
            }
            if (supportData == null)
            {
                showMsg = "Analytical model is empty!";
            }
            else if (supportData.Supported == false)
            {
                showMsg = "Element is not supported!";
            }
            else
            {
                AnalyticalSupportInfoArrayIterator infoIt = supportData.InfoArray.ForwardIterator();
                while (infoIt.MoveNext())
                {
                    AnalyticalSupportInfo info = infoIt.Current as AnalyticalSupportInfo;
                    showMsg += info.Element.Name + "\n";
                }
            }
            MessageBox.Show(showMsg);

            return IExternalCommand.Result.Succeeded;
        }
    }
}
