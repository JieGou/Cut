using System;
using System.Collections.Generic;
using System.Text;
using Inventor;

namespace Chapter5
{
    class MyConstraint
    {
        public void CreateSketchWithCons(Inventor.Application ThisApplication)
        {
            PartDocument partDoc;
            partDoc = (PartDocument)ThisApplication.Documents.Add(DocumentTypeEnum.kPartDocumentObject,
                ThisApplication.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject, SystemOfMeasureEnum.kDefaultSystemOfMeasure, DraftingStandardEnum.kGB_DraftingStandard, null),true);

            PlanarSketch sketch;
            sketch = partDoc.ComponentDefinition.Sketches.Add(partDoc.ComponentDefinition.WorkPlanes[3], false);

            TransientGeometry transGeom;
            transGeom = ThisApplication.TransientGeometry;

            SketchPoints skPnts;
            skPnts = sketch.SketchPoints;
            skPnts.Add(transGeom.CreatePoint2d(0, 0), false);
            skPnts.Add(transGeom.CreatePoint2d(1.0, 0), false);
            skPnts.Add(transGeom.CreatePoint2d(1.0, 0.5), false);
            skPnts.Add(transGeom.CreatePoint2d(2.2, 0.5), false);
            skPnts.Add(transGeom.CreatePoint2d(0.5, 1.5), false);
            skPnts.Add(transGeom.CreatePoint2d(0, 1.0), false);
            skPnts.Add(transGeom.CreatePoint2d(2.7, 1.5), false);
            skPnts.Add(transGeom.CreatePoint2d(0.5, 1.0), false);

            SketchLines lines;
            lines = sketch.SketchLines;

            System.Array Line = System.Array.CreateInstance(typeof(SketchLine), 6);
            SketchLine[] line = Line as SketchLine[];

            line[0] = lines.AddByTwoPoints(skPnts[1], skPnts[2]);
            line[1] = lines.AddByTwoPoints(skPnts[2], skPnts[3]);
            line[2] = lines.AddByTwoPoints(skPnts[3], skPnts[4]);
            line[3] = lines.AddByTwoPoints(skPnts[4], skPnts[7]);
            line[4] = lines.AddByTwoPoints(skPnts[7], skPnts[5]);
            line[5] = lines.AddByTwoPoints(skPnts[6], skPnts[1]);

            SketchArcs arcs;
            arcs = sketch.SketchArcs;
            SketchArc arc;
            arc = arcs.AddByCenterStartEndPoint(skPnts[8], skPnts[5], skPnts[6], true);

            sketch.GeometricConstraints.AddPerpendicular((SketchEntity)line[3], (SketchEntity)line[4], true, true);
            ThisApplication.ActiveView.Update();

            sketch.GeometricConstraints.AddTangent((SketchEntity)line[4], (SketchEntity)arc, null);
            sketch.GeometricConstraints.AddTangent((SketchEntity)line[5], (SketchEntity)arc, null);
            ThisApplication.ActiveView.Update();

            sketch.GeometricConstraints.AddParallel((SketchEntity)line[2], (SketchEntity)line[4], true, true);
            ThisApplication.ActiveView.Update();

            sketch.GeometricConstraints.AddHorizontal((SketchEntity)line[4], true);
            ThisApplication.ActiveView.Update();
        }
    }
}
