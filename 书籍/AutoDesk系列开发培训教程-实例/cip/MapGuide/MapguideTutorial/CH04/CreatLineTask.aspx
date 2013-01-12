<%@ Page Language="C#" %>

<%--
Copyright (C) 2004-2007  Autodesk, Inc.
This library is free software; you can redistribute it and/or
modify it under the terms of version 2.1 of the GNU Lesser
General Public License as published by the Free Software Foundation.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
--%>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections.Specialized" %>
<%@ Import Namespace="OSGeo.MapGuide" %>


<script runat="server">
//bool clear = false;

String srs = "";
int us = 0;
double distance = 0;
String legendName = "Line";
String dataSource = "";
</script>

<%
    
    Response.Charset = "utf-8";
    String sessionId = Request.Form.Get("SESSION");
    String featureName = "Line";

    try
    {
        //GetRequestParameters();
        double x1 = -87.726065;
        double y1 = 43.791720;
        double x2 = -87.718451;
        double y2 = 43.722526;
 
        double x3 = -87.715651;
        double y3 = 43.701126;
        
        dataSource = "Session:" + sessionId + "//Line.FeatureSource";
        String layerDefinition = "Session:" + sessionId + "//Line.LayerDefinition";

        try
        {         
            UtilityClass utility = new UtilityClass();
            utility.InitializeWebTier(Request);
            utility.ConnectToServer(sessionId);
            //Response.Write(sessionId);
            MgSiteConnection siteConnection = utility.GetSiteConnection();
            if (siteConnection == null)
            {
                Response.Write("fail to get site connection, exit");
                 return;
            }

            MgFeatureService featureService = siteConnection.CreateService(MgServiceType.FeatureService) as MgFeatureService;
            MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

            MgResourceIdentifier dataSourceId = new MgResourceIdentifier(dataSource);
            MgResourceIdentifier layerDefId = new MgResourceIdentifier(layerDefinition);

            //load the map runtime state and locate the measure layer
            //
            MgMap map = new MgMap();
            map.Open(resourceService, "Sheboygan");
             
            // for book
            //MgMap map = new MgMap();
            //map.Open(resourceService, "Sheboygan");
            //string srs = map.GetMapSRS();                  
            //MgCoordinateSystemFactory srsFactory = new MgCoordinateSystemFactory();
            //MgCoordinateSystem srsMap = srsFactory.Create(srs);            
            
            // end for book
            MgLayerCollection layers = map.GetLayers();
            srs = utility.GetMapSrs(map);

            MgLayer layer = utility.GetLayerByName (map, layerDefinition);

            MgCoordinateSystemFactory srsFactory = new MgCoordinateSystemFactory();
            MgCoordinateSystem srsMap = srsFactory.Create(srs);
            
            int srsType = srsMap.GetType();
            if (srsType == MgCoordinateSystemType.Geographic)
            {
                distance = srsMap.MeasureGreatCircleDistance(x1, y1, x2, y2);
                distance += srsMap.MeasureGreatCircleDistance(x2, y2, x3, y3);
            }
            else
            {
                distance = srsMap.MeasureEuclideanDistance(x1, y1, x2, y2);
                distance += srsMap.MeasureGreatCircleDistance(x2, y2, x3, y3);
            }

           distance = srsMap.ConvertCoordinateSystemUnitsToMeters(distance);

           if (0 == us)
                distance *= 0.001;//get kilometers
           else
                distance *= 0.000621371192;     //get miles

 
           //create the line string geometry representing this segment
  
          MgGeometryFactory geomFactory = new MgGeometryFactory();
          //用于创建几何对对象
          MgCoordinateCollection coordinates = new MgCoordinateCollection();
          coordinates.Add(geomFactory.CreateCoordinateXY(x1, y1));
          coordinates.Add(geomFactory.CreateCoordinateXY(x2, y2));
          coordinates.Add(geomFactory.CreateCoordinateXY(x3, y3));
          
          MgLineString geom = geomFactory.CreateLineString(coordinates);
  
          //data source already exist. clear its content
          if(utility.IsDataSourceExists (resourceService, dataSourceId))
          {
             ClearDataSource(featureService, dataSourceId, featureName);
           }
          //create feature source
          MgClassDefinition classDef = new MgClassDefinition();

          classDef.SetName(featureName);
          classDef.SetDescription("the line layer definition");
          classDef.SetDefaultGeometryPropertyName("GEOM");

          //Set KEY property
          MgDataPropertyDefinition prop = new MgDataPropertyDefinition("KEY");
          prop.SetDataType(MgPropertyType.Int32);
          prop.SetAutoGeneration(true);
          prop.SetReadOnly(true);
          classDef.GetIdentityProperties().Add(prop);
          classDef.GetProperties().Add(prop);

          //Set LENGTH property. Hold the distance for this segment
          prop = new MgDataPropertyDefinition("LENGTH");
          prop.SetDataType(MgPropertyType.Double);
          classDef.GetProperties().Add(prop);


          //Set geometry property
          MgGeometricPropertyDefinition geomProp = new MgGeometricPropertyDefinition("GEOM");
          geomProp.SetGeometryTypes(4);
          classDef.GetProperties().Add(geomProp);

          //Create the schema
          MgFeatureSchema schema = new MgFeatureSchema("LineSchema", "the line schema");
          schema.GetClasses().Add(classDef);

          //finally, creation of the feature source
          MgCreateSdfParams parameters = new MgCreateSdfParams("LatLong", srs, schema);
          featureService.CreateFeatureSource(dataSourceId, parameters);


          //build map tip
          String tip = "build map tip?";
            
             
          //Create the layer definition
          //dataSource = "Session:" + sessionId + "//Line.FeatureSource";
          //String layerDefinition = "Session:" + sessionId + "//Line.LayerDefinition";

          MgByteReader layerDefContent = BuildLayerDefinitionContent(dataSource, featureName, tip);
            
          resourceService.SetResource(layerDefId, layerDefContent, null);
          //Add the layer to the map, if it's not already in it
          if (layer == null)
          {
            layer = new MgLayer(layerDefId, resourceService);
            layer.SetName("Line1");
            layer.SetDisplayInLegend(true);
            layer.SetLegendLabel(legendName);
            layer.SetVisible(false);
            layers.Insert(0, layer);
          }
          // create a feature representing this segment and insert it into the data source
          //
          MgPropertyCollection LineProps = new MgPropertyCollection();

          MgDoubleProperty partialProp = new MgDoubleProperty("LENGTH", distance);
          LineProps.Add(partialProp);


          MgAgfReaderWriter agf = new MgAgfReaderWriter();
          MgByteReader geomReader = agf.Write(geom);
          MgGeometryProperty geometryProp = new MgGeometryProperty("GEOM", geomReader);
          LineProps.Add(geometryProp);
           
          MgInsertFeatures cmd = new MgInsertFeatures(featureName, LineProps);
          MgFeatureCommandCollection commands = new MgFeatureCommandCollection();
          commands.Add(cmd);
          //Insert the distance feature in the temporary data source
          //
          if (layer != null)
          {
              layer.SetVisible(true);
              utility.AddLayerToGroup(layer, "Line", "layer lengend", map);
              layer.SetDisplayInLegend(true);
              layer.ForceRefresh();
          } 
          ReleaseReader(featureService.UpdateFeatures(dataSourceId, commands, false));
          map.Save(resourceService);
            
          Response.Write (" 创建临时层成功<br>");
          Response.Write("距离  :" + distance + "公里<br>");
            
        }
            
        catch (MgException exc)
        {
            Response.Write(exc.GetMessage());
            return;
        }
        catch (Exception ne)
        {
            Response.Write(ne.Message );
            return;
        }
    }
    finally
    {
        //MapGuideApi.TerminateSockets();
    }
%>

<script runat="server">




MgByteReader BuildLayerDefinitionContent(String dataSource, String featureName, String tip)
{
    String layerTempl = LoadTemplate(Request, "/viewerfiles/linelayerdef.templ");
    String[] vals = {
      dataSource,
      featureName,
      "GEOM",
      tip,
      "1",
      "ff0000"
      };
    layerTempl = Substitute(layerTempl, vals);
    MgByteSource src = new MgByteSource(Encoding.UTF8.GetBytes(layerTempl), layerTempl.Length);

    return src.GetReader();
}

void ClearDataSource(MgFeatureService featureService, MgResourceIdentifier dataSourceId, String featureName)
{
    MgDeleteFeatures deleteCmd = new MgDeleteFeatures(featureName, "KEY >= 0");
    MgFeatureCommandCollection commands = new MgFeatureCommandCollection();
    commands.Add(deleteCmd);
    featureService.UpdateFeatures(dataSourceId, commands, false);
}



void ReleaseReader(MgPropertyCollection res)
{
    if(res == null)
        return;
    MgProperty prop = res.GetItem(0);
    if (prop == null)
        return;
    if (prop is MgStringProperty)
        throw new Exception(((MgStringProperty)prop).GetValue());
    MgFeatureProperty resProp = (MgFeatureProperty)prop;
    MgFeatureReader reader = (MgFeatureReader)resProp.GetValue();
    if(reader == null)
        return;
    reader.Close();
}

</script>

<script runat="server">


String Substitute(String templ, String[] vals)
{
    StringBuilder res = new StringBuilder();
    int index = 0, val = 0;
    bool found;
    do
    {
        found = false;
        int i = templ.IndexOf('%', index);
        if(i != -1)
        {
            found = true;
            res.Append(templ.Substring(index, i - index));
            if(i < templ.Length - 1)
            {
  if(templ[i+1] == '%')
      res.Append('%');
  else if(templ[i+1] == 's')
      res.Append(vals[val ++]);
  else
      res.Append('@');    //add a character illegal in jscript so we know the template was incorrect
  index = i + 2;
            }
        }
    } while(found);
    res.Append(templ.Substring(index));
    return res.ToString();
}

String LoadTemplate(HttpRequest request, String filePath)
{
    StreamReader sr = File.OpenText(request.ServerVariables["APPL_PHYSICAL_PATH"]+filePath);
    String template = sr.ReadToEnd();
    return template;

}


</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>创建临时层</title>
    <script type="text/javascript">
        function RefreshMap() {
            parent.parent.GetMapFrame().Refresh();
        }
    </script>
</head>
<body onload="RefreshMap()">

</body>
</html>