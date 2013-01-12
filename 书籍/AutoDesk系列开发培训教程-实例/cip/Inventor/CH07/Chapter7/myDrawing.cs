using System;
using System.Collections.Generic;
using System.Text;
using Inventor;

namespace Chapter7
{
    class myDrawing
    {
        public void CreateDrawing(Inventor.Application ThisApplication)
        {
            DrawingDocument drawingDoc;
            drawingDoc = (DrawingDocument)ThisApplication.ActiveDocument;

            Sheet sheet;
            sheet = drawingDoc.Sheets[1];

            Point2d point1;
            point1 = ThisApplication.TransientGeometry.CreatePoint2d(5, 5);

            _Document partDoc;
            partDoc = ThisApplication.Documents.Open("c:\\testpart.ipt", false);

            DrawingView view1;
            view1 = sheet.DrawingViews.AddBaseView(partDoc, point1, 1, ViewOrientationTypeEnum.kBottomViewOrientation, DrawingViewStyleEnum.kHiddenLineDrawingViewStyle, "", null, null);

            partDoc.Close(true);

            Point2d point2;
            point2 = ThisApplication.TransientGeometry.CreatePoint2d(15, 40);

            DrawingSketch drawingSketch;
            drawingSketch = sheet.Sketches.Add();

            drawingSketch.Edit();
            SketchLine sketchLine;
            sketchLine = drawingSketch.SketchLines.AddByTwoPoints(point1, point2);
            drawingSketch.ExitEdit();

            SectionDrawingView view2;
            view2 = sheet.DrawingViews.AddSectionView(view1, drawingSketch, point2, DrawingViewStyleEnum.kHiddenLineDrawingViewStyle,
                1, true, "", false, true, null);
            drawingSketch.Visible = false;
        }
    }
}
