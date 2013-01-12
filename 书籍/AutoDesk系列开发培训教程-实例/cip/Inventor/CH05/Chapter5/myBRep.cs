using System;
using System.Collections.Generic;
using System.Text;
using Inventor;

namespace Chapter5
{
    class myBRep
    {
        public void TraverseBRep(Inventor.Application ThisApplication)
        {
            PartComponentDefinition partDef;
            partDef = ((PartDocument)ThisApplication.ActiveDocument).ComponentDefinition;

            foreach (SurfaceBody surfaceBody in partDef.SurfaceBodies)
            {
                foreach (Face face in surfaceBody.Faces)
                {
                    if (face.SurfaceType == SurfaceTypeEnum.kPlaneSurface)
                    {
                        SurfaceEvaluator eval;
                        eval = face.Evaluator;

                        Box2d range;
                        range = eval.ParamRangeRect;

                        System.Array Params = System.Array.CreateInstance(typeof(double), 2);
                        double[] paras = Params as double[];
                        paras[0] = range.MinPoint.X + (range.MaxPoint.X - range.MinPoint.X) * 0.5;
                        paras[1] = range.MinPoint.Y + (range.MaxPoint.Y - range.MinPoint.Y) * 0.5;
 
                        System.Array cenPt = System.Array.CreateInstance(typeof(double),20);
                        eval.GetPointAtParam(ref Params, ref cenPt);

                        Point point;
                        point = ThisApplication.TransientGeometry.CreatePoint(
                            (double)cenPt.GetValue(0), (double)cenPt.GetValue(1), (double)cenPt.GetValue(2));
                        
                        PlanarSketch sketch;
                        sketch = partDef.Sketches.Add(face,true);

                        Point2d point2d;
                        point2d = sketch.ModelToSketchSpace(point);

                        SketchPoint skPnt;
                        skPnt = sketch.SketchPoints.Add(point2d,false);
                    }
                }
            }
        }
    }
}
