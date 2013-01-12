using System;
using Autodesk.Revit;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using System.Windows.Forms;

namespace RevitDevelop
{
    public class ShowWallModelCurvesNum : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document curDoc = commandData.Application.ActiveDocument;
            ElementSetIterator it = curDoc.Selection.Elements.ForwardIterator();
            if (it.MoveNext())
            {
                Wall w = it.Current as Wall;
                if (w == null)
                {
                    message = "Please select a wall!";
                    return IExternalCommand.Result.Cancelled;
                }
                AnalyticalModelWall wallModel = w.AnalyticalModel as AnalyticalModelWall;
                MessageBox.Show("Wall model curves size: " + wallModel.Curves.Size.ToString());
            }

            return IExternalCommand.Result.Succeeded;
        }
    }
}
