using System;
using System.Collections;
using OSGeo.MapGuide;

public class StringParser
{
    public MgPolygon parseString(string coordinates)
    {
        MgPolygon polygon = null;
        
        //Please type in the code between the comments
MgGeometryFactory geoFactory = new MgGeometryFactory();
MgCoordinateCollection coordCol = new MgCoordinateCollection();

string[] num_coords = coordinates.Split('~');
int num = Convert.ToInt16(num_coords[0]);
string[] coords = num_coords[1].Split('_');

for (int i = 0; i < num; i++)
{
    string[] xyString = coords[i].Split('!');
    double x = Convert.ToDouble(xyString[0]);
    double y = Convert.ToDouble(xyString[1]);

    MgCoordinate coord = geoFactory.CreateCoordinateXY(x, y);
    coordCol.Add(coord);
}

MgLinearRing outRing = geoFactory.CreateLinearRing(coordCol);
polygon = geoFactory.CreatePolygon(outRing, null);

        ///////////////////////////////////////

        return polygon;
    }
}
