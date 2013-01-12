using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using OSGeo.MapGuide;
using System.Text;
using System.Collections;

public class UtilityClass
{
    MgSiteConnection siteConnection;

  //----------------------------------------------------------------------------------------
    // 功 能： 初始化网络层
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public void InitializeWebTier(HttpRequest Request)
    {
        string realPath = Request.ServerVariables["APPL_PHYSICAL_PATH"];
        String configPath = realPath + "webconfig.ini";
        MapGuideApi.MgInitializeWebTier(configPath);
    }

   //----------------------------------------------------------------------------------------
    // 功 能： 初始化网络层
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public void InitializeWebTier(String path)
    {
        MapGuideApi.MgInitializeWebTier(path);
    }
  
    public void ConnectToServer(String sessionID)
    {
        MgUserInformation userInfo = new MgUserInformation(sessionID);
        siteConnection = new MgSiteConnection();
        siteConnection.Open(userInfo);
    }

    //----------------------------------------------------------------------------------------
    // 功 能： 链接站点服务器
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public void ConnectToServer(String userName, String password)
    {
        MgUserInformation userInfo = new MgUserInformation(userName, password);
        siteConnection = new MgSiteConnection();
        siteConnection.Open(userInfo);
    }
    //----------------------------------------------------------------------------------------
    // 功 能： 将Mapguide返回的数据转化为KML
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public String createMuniPolygons()
    {
        MgGeometryFactory geoFactory = new MgGeometryFactory();
        StringBuilder outString = new StringBuilder();
        outString.Append("<?xml version=\"1.0\"?>");
        //通过调用MapGuide API获取特定的要素
        MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);
        MgResourceIdentifier resId = new MgResourceIdentifier("Library://Exercise/Data/WheatonMunicipalities.FeatureSource");
        MgFeatureReader featureReader = featureService.SelectFeatures(resId, "WheatonMunicipalities", null);
        //将结果保存为Google Map能够识别的 XML.
        outString.Append("<overlays>");
        MgAgfReaderWriter geoWriter = new MgAgfReaderWriter();
        while (featureReader.ReadNext())
        {
            String muniName = featureReader.GetString("MUNINAME");
            MgByteReader byteReader = featureReader.GetGeometry("Geometry");
            MgPolygon polygon = (MgPolygon)geoWriter.Read(byteReader);
            MgPoint point = polygon.GetCentroid();
            double x = point.GetCoordinate().GetX();
            double y = point.GetCoordinate().GetY();

            outString.Append("<polygon info=\"" + muniName + "\" cent_x=\"" + x + "\" cent_y=\"" + y + "\">");
            MgLinearRing linearRing = polygon.GetExteriorRing();
            MgCoordinateIterator coordIterator = linearRing.GetCoordinates();
            outString.Append("<points>");
            while (coordIterator.MoveNext())
            {
                MgCoordinate coord = coordIterator.GetCurrent();
                if (coord != null)
                {
                    outString.Append("<point lng=\"" + coord.GetX() + "\" lat=\"" + coord.GetY() + "\" />");
                }
            }
            outString.Append("</points>");
            outString.Append("</polygon>");
        }
        featureReader.Close();
        outString.Append("</overlays>");
        return outString.ToString();
    }
    //----------------------------------------------------------------------------------------
    // 功 能： 将Mapguide返回的数据转化为KML
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public String createQueryPolygon(String queryString)
    {
        StringBuilder outString = new StringBuilder();

        outString.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        MgFeatureService featureservice = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);
        MgFeatureQueryOptions queryOptions = new MgFeatureQueryOptions();
        queryOptions.SetFilter(queryString);

        MgResourceIdentifier resId = new MgResourceIdentifier("Library://Exercise/Data/SheboyganParcels.FeatureSource");
        MgFeatureReader featureReader = featureservice.SelectFeatures(resId, "SheboyganParcels", queryOptions);
        outString.Append("<overlays>");

        MgAgfReaderWriter geoReader = new MgAgfReaderWriter();
        while (featureReader.ReadNext())
        {
            String keyVal = featureReader.GetString("RTYPE");
            MgByteReader byteReader = featureReader.GetGeometry("Geometry");
            MgPolygon polygon = (MgPolygon)geoReader.Read(byteReader);
            MgPoint point = polygon.GetCentroid();
            double x = point.GetCoordinate().GetX();
            double y = point.GetCoordinate().GetY();

            outString.Append("<polygon info=\"" + keyVal + "\" cent_x=\"" + x + "\" cent_y=\"" + y + "\">");

            MgLinearRing linearRing = polygon.GetExteriorRing();
            MgCoordinateIterator coordIter = linearRing.GetCoordinates();

            outString.Append("<points>");
            while (coordIter.MoveNext())
            {
                MgCoordinate coord = coordIter.GetCurrent();
                if (coord != null)
                {
                    outString.Append("<point lng=\"" + coord.GetX() + "\" lat=\"" + coord.GetY() + "\"/>");
                }
            }

            outString.Append("</points>");
            outString.Append("</polygon>");
        }
        featureReader.Close();
        outString.Append("</overlays>");


        return outString.ToString();
    }
    //----------------------------------------------------------------------------------------
    // 功 能： 将Mapguide返回的数据转化为KML
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public String createMuniMarker()
    {
        StringBuilder outString = new StringBuilder();
        MgGeometryFactory geoFactory = new MgGeometryFactory();
        outString.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);
        MgResourceIdentifier resId = new MgResourceIdentifier("Library://MgTutorial/Data/WheatonMunicipalities.FeatureSource");
        MgFeatureReader featureReader = featureService.SelectFeatures(resId, "WheatonMunicipalities", null);

        outString.Append("<markers>");
        MgAgfReaderWriter geoReader = new MgAgfReaderWriter();
        while (featureReader.ReadNext())
        {
            String muniName = featureReader.GetString("MUNINAME");
            MgByteReader byteReader = featureReader.GetGeometry("Geometry");
            MgGeometry geo = geoReader.Read(byteReader);
            MgPoint pt = geo.GetCentroid();
            double x = pt.GetCoordinate().GetX();
            double y = pt.GetCoordinate().GetY();
            outString.Append("<marker lat=\"" + y + "\" lng=\"" + x + "\" info=\"" + muniName + "\" />");
        }
        featureReader.Close();
        outString.Append("</markers>");

        return outString.ToString();
    }
    //----------------------------------------------------------------------------------------
    // 功 能： 将Mapguide返回的数据转化为KML
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public String createSheboygon3D()
    {
        StringBuilder outString = new StringBuilder();

        outString.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        outString.Append("<kml xmlns=\"http://earth.google.com/kml/2.0\">");
        outString.Append("<Document>");
        outString.Append("<Style id=\"muniStyle\">");
        outString.Append("<PolyStyle>");
        outString.Append("<outline>1</outline>");
        outString.Append("<fill>1</fill>");
        outString.Append("<color>70000000</color>");
        outString.Append("</PolyStyle>");
        outString.Append("<LineStyle>");
        outString.Append("<color>ff0000ff</color>");
        outString.Append("</LineStyle>");
        outString.Append("</Style>");
        outString.Append("<Folder>");
        outString.Append("<description>Sheboygan Building Footprints 3D</description>");
        outString.Append("<name>Sheboygan Bulding Model</name>");
        outString.Append("<visibility>0</visibility>");
        outString.Append("<open>0</open>");

        MgGeometryFactory geoFactory = new MgGeometryFactory();
        MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);
        MgResourceIdentifier resId = new MgResourceIdentifier("Library://MgTutorial/Data/Sheboygan3D.FeatureSource");
        MgFeatureReader featureReader = featureService.SelectFeatures(resId, "Sheboygan3D", null);
        MgAgfReaderWriter geoReader = new MgAgfReaderWriter();

        outString.Append("<Placemark>");
        outString.Append("<name>3D Model</name>");
        outString.Append("<description>Sheboygan Building Model Area</description>");
        outString.Append("<styleUrl>#muniStyle</styleUrl>");

        PolygonWriter polygonWriter = new PolygonWriter();

        polygonWriter.StartEmitConsolidatedGeometry();

        while (featureReader.ReadNext())
        {
            MgByteReader byteReader = featureReader.GetGeometry("Geometry");
            double ht = featureReader.GetDouble("HEIGHT");
            MgGeometry geo = geoReader.Read(byteReader);
            polygonWriter.WriteConsolidatedGeometryRing(geo, 1, ht * 0.1);

        }

        featureReader.Close();
        polygonWriter.EndEmitConsolidatedGeometry();

        outString.Append(polygonWriter.getOutputString());
        outString.Append("</Placemark>");
        outString.Append("</Folder>");
        outString.Append("</Document>");
        outString.Append("</kml>");

        return outString.ToString();

    }
    //----------------------------------------------------------------------------------------
    // 功 能： 将Mapguide返回的数据转化为KML
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public String CreateWheatonMuni()
    {
        StringBuilder outString = new StringBuilder(1024000);
        outString.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        outString.Append("<kml xmlns=\"http://earth.google.com/kml/2.0\">");
        outString.Append("<Document>");
        outString.Append("<Style id=\"muniStyle\">");
        outString.Append("<PolyStyle>");
        outString.Append("<outline>1</outline>");
        outString.Append("<fill>0</fill>");
        outString.Append("<color>ff00ff00</color>");
        outString.Append("</PolyStyle>");
        outString.Append("<geomColor>ff00ff00</geomColor>");
        outString.Append("<geomScale>2</geomScale>");
        outString.Append("</Style>");

        outString.Append("<Folder>");
        outString.Append("<description>Tux - Wheaton Municipal Districts</description>");
        outString.Append("<name>Wheaton Municipal Districts</name>");
        outString.Append("<visibility>0</visibility>");
        outString.Append("<open>0</open>");

        MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);
        MgResourceIdentifier resId = new MgResourceIdentifier("Library://MgTutorial/Data/WheatonMunicipalities.FeatureSource");
        MgFeatureReader featureReader = featureService.SelectFeatures(resId, "WheatonMunicipalities", null);
        MgAgfReaderWriter geoReader = new MgAgfReaderWriter();
        while (featureReader.ReadNext())
        {
            String muniName = featureReader.GetString("MUNINAME");
            MgByteReader byteReader = featureReader.GetGeometry("Geometry");
            MgGeometry geo = geoReader.Read(byteReader);

            outString.Append("<Placemark>");
            outString.Append("<name>" + muniName + "</name>");
            outString.Append("<description>" + muniName + "</description>");
            outString.Append("<styleUrl>#muniStyle</styleUrl>");

            PolygonWriter polygonWriter = new PolygonWriter();
            polygonWriter.EmitGeometry(geo, 0, 0);

            outString.Append(polygonWriter.getOutputString());
            outString.Append("</Placemark>");
        }
        featureReader.Close();

        outString.Append("</Folder>");
        outString.Append("</Document>");
        outString.Append("</kml>");

        return outString.ToString();

    }


}
