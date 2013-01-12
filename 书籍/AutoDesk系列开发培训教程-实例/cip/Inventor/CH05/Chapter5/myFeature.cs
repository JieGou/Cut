using System;
using System.Collections.Generic;
using System.Text;
using Inventor;

namespace Chapter5
{
    class myFeature
    {
        public void CreateExtrudeFeature(Inventor.Application ThisApplication)
        {
            PartDocument partDoc;
            partDoc = (PartDocument)ThisApplication.Documents.Add(DocumentTypeEnum.kPartDocumentObject,
                ThisApplication.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject, SystemOfMeasureEnum.kDefaultSystemOfMeasure, DraftingStandardEnum.kGB_DraftingStandard, null), true);

            PlanarSketch sketch;
            sketch = partDoc.ComponentDefinition.Sketches.Add(partDoc.ComponentDefinition.WorkPlanes[3], false);

            TransientGeometry transGeom;
            transGeom = ThisApplication.TransientGeometry;

            SketchPoints skPnts;
            skPnts = sketch.SketchPoints;
            skPnts.Add(transGeom.CreatePoint2d(0, 0), false);
            skPnts.Add(transGeom.CreatePoint2d(1, 0), false);
            skPnts.Add(transGeom.CreatePoint2d(1, 1), false);

            SketchLines lines;
            lines = sketch.SketchLines;
            System.Array Line = System.Array.CreateInstance(typeof(SketchLine), 3);
            SketchLine[] line = Line as SketchLine[];
            line[0] = lines.AddByTwoPoints(skPnts[1], skPnts[2]);
            line[1] = lines.AddByTwoPoints(skPnts[2], skPnts[3]);
            line[2] = lines.AddByTwoPoints(skPnts[3], skPnts[1]);

            Profile profile;
            profile = sketch.Profiles.AddForSolid(true, null, null);

            ExtrudeFeature extFeature;
            extFeature = partDoc.ComponentDefinition.Features.ExtrudeFeatures.AddByDistanceExtent(
                profile,1.0, PartFeatureExtentDirectionEnum.kPositiveExtentDirection,PartFeatureOperationEnum.kJoinOperation,null);
            ThisApplication.ActiveView.Fit(true);
        }

        public void CreateRevolveFeature(Inventor.Application ThisApplication)
        {
            PartDocument partDoc;
            partDoc = (PartDocument)ThisApplication.Documents.Add(DocumentTypeEnum.kPartDocumentObject,
                ThisApplication.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject, SystemOfMeasureEnum.kDefaultSystemOfMeasure, DraftingStandardEnum.kGB_DraftingStandard, null), true);

            PlanarSketch sketch;
            sketch = partDoc.ComponentDefinition.Sketches.Add(partDoc.ComponentDefinition.WorkPlanes[3], false);

            TransientGeometry transGeom;
            transGeom = ThisApplication.TransientGeometry;

            SketchPoints skPnts;
            skPnts = sketch.SketchPoints;
            skPnts.Add(transGeom.CreatePoint2d(0, 0), false);
            skPnts.Add(transGeom.CreatePoint2d(1, 0), false);
            skPnts.Add(transGeom.CreatePoint2d(1, 1), false);

            SketchLines lines;
            lines = sketch.SketchLines;
            SketchLine line;
            line = lines.AddByTwoPoints(skPnts[1], skPnts[2]);

            SketchCircles circs;
            circs = sketch.SketchCircles;
            SketchCircle circ;
            circ = circs.AddByCenterRadius(skPnts[3], 0.5);
 
            Profile profile;
            profile = sketch.Profiles.AddForSolid(true, null, null);

            RevolveFeature revFeature;
            revFeature = partDoc.ComponentDefinition.Features.RevolveFeatures.AddFull(profile, line, PartFeatureOperationEnum.kJoinOperation);
            ThisApplication.ActiveView.Fit(true);
        }

    }
}
