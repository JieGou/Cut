using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using OSGeo.MapGuide;

//----------------------------------------------------------------------------------------
// 功 能： MgGeometry转化为KML格式工具类
//
// 作 者： 
//
//
// 日 期：2007.05.#
//
//-----------------------------------------------------------------------------------------
public class PolygonWriter
{
    StringBuilder outString = new StringBuilder(2048000);

    public String getOutputString() {
        return outString.ToString();
    }

    public void EmitGeometry(MgGeometry geo, int extrude, double ht) 
    {
        if (geo != null)
        {
            if (geo.GetGeometryType() == MgGeometryType.Polygon)
            {
                EmitPolygon(geo, extrude, ht);
            }
            else if (geo.GetGeometryType() == MgGeometryType.MultiPolygon)
            {
                EmitMultiPolygon(geo, extrude, ht);
            }
        }
    }

    public void StartEmitConsolidatedGeometry()
    {
        outString.Append("<GeometryCollection>");
    }

    public void EndEmitConsolidatedGeometry()
    {
        outString.Append("</GeometryCollection>");
    }

    public void WriteConsolidatedGeometryRing(MgGeometry geo, int extrude, double ht) {
        EmitPolygon(geo, extrude, ht);
    }

    private void EmitMultiPolygon(MgGeometry geo, int extrude, double ht)
    {
        if (geo != null)
        {
            outString.Append("<GeometryCollection>");
            MgMultiPolygon multiPolygon = (MgMultiPolygon)geo;
            for (int i = 0; i < multiPolygon.GetCount(); i++)
            {
                EmitPolygon(multiPolygon.GetPolygon(i), extrude, ht);
            }
            outString.Append("</GeometryCollection>");
        }
    }


    private void EmitPolygon(MgGeometry geo, int extrude, double ht)
    {
        if (geo != null)
        {
            outString.Append("<Polygon>");
            outString.Append("<extrude>" + extrude + "</extrude>");
            outString.Append("<altitudeMode>relativeToGround</altitudeMode>");
            outString.Append("<outerBoundaryIs>");

            MgPolygon polygon = (MgPolygon)geo;
            EmitLinearRing(polygon.GetExteriorRing(), ht);
            outString.Append("</outerBoundaryIs>");
            outString.Append("</Polygon>");
        }
    }

    private void EmitLinearRing(MgLinearRing linearRing, double ht)
    {
        if (linearRing != null)
        {
            outString.Append("<LinearRing>");
            outString.Append("<coordinates>");

            MgCoordinateIterator coordIter = linearRing.GetCoordinates();
            while (coordIter.MoveNext())
            {
                MgCoordinate coord = coordIter.GetCurrent();
                if (coord != null)
                {
                    outString.Append(coord.GetX() + ", " + coord.GetY() + ", " + ht + " ");
                }
            }
            outString.Append("</coordinates>");
            outString.Append("</LinearRing>");
        }
    }
}
