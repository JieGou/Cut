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
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
//---------------------------------------------------------------------------------------
//
//        功能：工具类，完成站点的连接，封装功能函数
//
//         作者： 
//
//         日期： 2007.5.23
//          
//         修改历史：无 
//          
//--------------------------------------------------------------------------------------- 
public class UtilityClass
{
    // 站点siteConnection对象实例 
    MgSiteConnection siteConnection;

    //----------------------------------------------------------------------------------------
    // 功 能： 获取站点链接对象
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public MgSiteConnection GetSiteConnection()
    {
        return siteConnection;
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
    public void InitializeWebTier(HttpRequest Request)
    {
        string realPath = Request.ServerVariables["APPL_PHYSICAL_PATH"];
        String configPath = realPath + "webconfig.ini";
        MapGuideApi.MgInitializeWebTier(configPath);
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
    public void ConnectToServer()
    {
        MgResourceService resourceService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgRenderingService renderingService = (MgRenderingService)siteConnection.CreateService(MgServiceType.RenderingService);
    }

    //----------------------------------------------------------------------------------------
    // 功 能： 通过层名获取MgLayer层对象
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public MgLayer GetLayerByName(MgMap map, String layerName)
    {
        MgLayer layer = null;
        for (int i = 0; i < map.GetLayers().GetCount(); i++)
        {
            MgLayer nextLayer = map.GetLayers().GetItem(i);
            if (nextLayer.GetName() == layerName)
            {
                layer = nextLayer;
                break;
            }

        }
        return layer;
    }

    //----------------------------------------------------------------------------------------
    // 功 能： 判断资源是否存在
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public bool IsDataSourceExists(MgResourceService resourceService, MgResourceIdentifier dataSourceId)
    {
        try
        {
            MgByteReader cnt = resourceService.GetResourceContent(dataSourceId);
            return true;
        }
        catch (MgResourceNotFoundException)
        {
            return false;
        }
    }

    //----------------------------------------------------------------------------------------
    // 功 能： 创建要素源
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------

    public void CreateFeatureSource(String sessionId)
    {
        MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        // 为要素源创建临时要素类

        MgClassDefinition classDefinition = new MgClassDefinition();
        classDefinition.SetName("Points");
        classDefinition.SetDescription("临时要素类（点）");
        classDefinition.SetDefaultGeometryPropertyName("GEOM");

        // 创建要素类标识属性
        MgDataPropertyDefinition identityProperty = new MgDataPropertyDefinition("KEY");
        identityProperty.SetDataType(MgPropertyType.Int32);
        identityProperty.SetAutoGeneration(true);
        identityProperty.SetReadOnly(true);

        classDefinition.GetIdentityProperties().Add(identityProperty);
        classDefinition.GetProperties().Add(identityProperty);

        // 创建普通属性（NAME)
        MgDataPropertyDefinition nameProperty = new MgDataPropertyDefinition("NAME");
        nameProperty.SetDataType(MgPropertyType.String);
        classDefinition.GetProperties().Add(nameProperty);

        // 创建几何属性（GEOM)
        MgGeometricPropertyDefinition geometryProperty = new MgGeometricPropertyDefinition("GEOM");
        geometryProperty.SetGeometryTypes(MgFeatureGeometricType.Point);
        classDefinition.GetProperties().Add(geometryProperty);

        // 创建要素模式
        MgFeatureSchema featureSchema = new MgFeatureSchema("PointSchema", "this is Point schema");
        featureSchema.GetClasses().Add(classDefinition);


        // 创建要素源
        String featureSourceName = "Session:" + sessionId + "//Points.FeatureSource";
        MgResourceIdentifier resourceIdentifier = new MgResourceIdentifier(featureSourceName);

        String wkt = "LOCALCS[\"*XY-MT*\",LOCAL_DATUM[\"*X-Y*\",10000],UNIT[\"Meter\", 1],AXIS[\"X\",EAST],AXIS[\"Y\",NORTH]]";
        MgCreateSdfParams sdfParams = new MgCreateSdfParams("ArbitraryXY", wkt, featureSchema);

        featureService.CreateFeatureSource(resourceIdentifier, sdfParams);

    }

    //----------------------------------------------------------------------------------------
    // 功 能： 释放数据
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    void ReleaseReader(MgPropertyCollection res, MgFeatureCommandCollection commands)
    {
        if (res == null)
            return;

        for (int i = 0; i < res.GetCount(); i++)
        {
            MgFeatureCommand cmd = commands.GetItem(i);
            if (cmd is MgInsertFeatures)
            {
                MgFeatureProperty resProp = res.GetItem(i) as MgFeatureProperty;
                if (resProp != null)
                {
                    MgFeatureReader reader = resProp.GetValue() as MgFeatureReader;
                    if (reader == null)
                        return;
                    reader.Close();
                }
            }
        }
    }

    //----------------------------------------------------------------------------------------
    // 功 能： 基于传入的数据（x，y）创建空间点要素属性
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public MgPropertyCollection MakePoint(string name, double x, double y)
    {
        MgPropertyCollection propertyCollection = new MgPropertyCollection();
        MgStringProperty nameProperty = new MgStringProperty("NAME", name);
        propertyCollection.Add(nameProperty);

        MgWktReaderWriter wktReaderWriter = new MgWktReaderWriter();
        MgAgfReaderWriter agfReaderWriter = new MgAgfReaderWriter();

        MgGeometry geometry = wktReaderWriter.Read("POINT XY (" + x.ToString() + " " + y.ToString() + ")");
        MgByteReader geometryByteReader = agfReaderWriter.Write(geometry);
        MgGeometryProperty geometryProperty = new MgGeometryProperty("GEOM", geometryByteReader);
        propertyCollection.Add(geometryProperty);

        return propertyCollection;
    }


    //----------------------------------------------------------------------------------------
    // 功 能： 添加层到层组，如果层组不存在，则创建
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public MgLayer CreateLayerResource(MgResourceIdentifier layerResourceID, MgResourceService resourceService, string layerName, string layerLegendLabel, MgMap map)
    {
        MgLayer newLayer = new MgLayer(layerResourceID, resourceService);
        //  创建层并添加家到地图中
        newLayer.SetName(layerName);
        newLayer.SetVisible(true);				
        newLayer.SetLegendLabel(layerLegendLabel);
        newLayer.SetDisplayInLegend(true);
        MgLayerCollection layerCollection = map.GetLayers();
        if (!layerCollection.Contains(layerName))
        {
            // 用Insert方法插入层，新创建的层位于绘制次序的最顶部
            layerCollection.Insert(0, newLayer);
        }
        newLayer.SetDisplayInLegend(true);
        return newLayer;
    }



    
    //----------------------------------------------------------------------------------------
    // 功 能： 添加层到层组，如果层组不存在，则创建
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public void AddLayerToGroup(MgLayer layer, string layerGroupName, string layerGroupLegendLabel, MgMap map)
    {

        // 获取层组
        MgLayerGroupCollection layerGroupCollection = map.GetLayerGroups();
        MgLayerGroup layerGroup = null;
        if (layerGroupCollection.Contains(layerGroupName))
        {
            layerGroup = layerGroupCollection.GetItem(layerGroupName);
        }
        else
        {
            // 如果没有存在，则创建层组
            layerGroup = new MgLayerGroup(layerGroupName);
            layerGroup.SetVisible(true);
            layerGroup.SetDisplayInLegend(true);
            layerGroup.SetLegendLabel(layerGroupLegendLabel);
            layerGroupCollection.Add(layerGroup);
        }
        layerGroup.SetDisplayInLegend(true);
        // 添加层到层组
        layer.SetGroup(layerGroup);

    }

    //----------------------------------------------------------------------------------------
    // 功 能： 获取地图的坐标系统
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public String GetMapSrs(MgMap map)
    {
        String srs = map.GetMapSRS();
        if (srs != "")
            return srs;

        return "LOCALCS[\"*XY-MT*\",LOCAL_DATUM[\"*X-Y*\",10000],UNIT[\"Meter\", 1],AXIS[\"X\",EAST],AXIS[\"Y\",NORTH]]";
    }

    public UtilityClass()
    {
    }
    //----------------------------------------------------------------------------------------
    // 功 能： 创建临时层
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public bool CreatePointsLayer(String rootPath, String sessionId)
    {
        // 获取要素服务和资源服务
        MgResourceService resourceService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        //  打开地图
        MgMap map = new MgMap();
        map.Open(resourceService, "Sheboygan");
        // ---要素类和要素操作（用户可以跳过，后面还要介绍）--开始
        // 创建点数据的要素源
        CreateFeatureSource(sessionId);
        String featureSourceName = "Session:" + sessionId + "//Points.FeatureSource";
        MgResourceIdentifier resourceIdentifier = new MgResourceIdentifier(featureSourceName);

        MgBatchPropertyCollection batchPropertyCollection = new MgBatchPropertyCollection();
        MgWktReaderWriter wktReaderWriter = new MgWktReaderWriter();
        MgAgfReaderWriter agfReaderWriter = new MgAgfReaderWriter();
        MgGeometryFactory geometryFactory = new MgGeometryFactory();

        // 创建点记录
        MgPropertyCollection propertyCollection = MakePoint("Point A", -87.727, 43.748);
        batchPropertyCollection.Add(propertyCollection);

        propertyCollection = MakePoint("Point B", -87.728, 43.730);
        batchPropertyCollection.Add(propertyCollection);

        propertyCollection = MakePoint("Point C", -87.726, 43.750);
        batchPropertyCollection.Add(propertyCollection);

        propertyCollection = MakePoint("Point D", -87.728, 43.750);
        batchPropertyCollection.Add(propertyCollection);

        // 将创建的要素数据添加到要素源
        MgInsertFeatures Insertcmd = new MgInsertFeatures("Points", batchPropertyCollection);
        MgFeatureCommandCollection featureCommandCollection = new MgFeatureCommandCollection();
        featureCommandCollection.Add(Insertcmd);
        featureService.UpdateFeatures(resourceIdentifier, featureCommandCollection, false);
        MgResourceIdentifier resourceID = null;
        //--------------要素类和要素操作 结束


        // 创建层，通过层工厂LayerDefinitionFactory
        LayerDefinitionFactory factory = new LayerDefinitionFactory();
        factory.RootDirectoryPath = rootPath;

        //-------------------创建点样式------------------------//
        // 创建标记符号l
        string resourceSymbel = "Library://MgTutorial/Symbols/BasicSymbols.SymbolLibrary";
        string symbolName = "PushPin";
        string width = "24";  // unit = points
        string height = "24"; // unit = points
        string color = "FFFF0000";
        string markSymbol = factory.CreateMarkSymbol(resourceSymbel, symbolName, width, height, color);

        // 创建文本
        string text = "ID";
        string fontHeight = "12";
        string foregroundColor = "FF000000";
        string textSymbol = factory.CreateTextSymbol(text, fontHeight, foregroundColor);

        // 创建点规则
        string legendLabel = "trees";
        string filter = "";
        string pointRule = factory.CreatePointRule(legendLabel, filter, textSymbol, markSymbol);

        // 创建点样式
        string pointTypeStyle = factory.CreatePointTypeStyle(pointRule);

        // 创建缩放范围
        string minScale = "0";
        string maxScale = "1000000000000";
        string pointScaleRange = factory.CreateScaleRange(minScale, maxScale, pointTypeStyle);
        
        // 创建层定义
        string featureName = "PointSchema:Points";
        string geometry = "GEOM";
        string layerDefinition = factory.CreateLayerDefinition(featureSourceName, featureName, geometry, pointScaleRange);

        MgByteSource byteSource = new MgByteSource(Encoding.UTF8.GetBytes(layerDefinition), layerDefinition.Length);
        MgByteReader byteReader = byteSource.GetReader();


        resourceID = new MgResourceIdentifier("Session:" + sessionId + "//Points.LayerDefinition");
        resourceService.SetResource(resourceID, byteReader, null);
        //输出创建的层定义内容（测试用）
        MgByteSink byteSink = new MgByteSink(resourceService.GetResourceContent(resourceID));
        string filePath = "C:\\Temp\\LayerDefinition.xml";
        byteSink.ToFile(filePath);

        // 创建层并添加到层组和地图中
        MgLayer newLayer = CreateLayerResource(resourceID, resourceService, "Points", "临时层", map);

        AddLayerToGroup(newLayer, "Analysis", "分析", map);

        MgLayerCollection layerCollection = map.GetLayers();
        if (layerCollection.Contains("Points"))
        {
            MgLayer pointsLayer = layerCollection.GetItem("Points");
            pointsLayer.SetVisible(true);
        }
        // 保存地图
        map.Save(resourceService);

        return true;
    }




}
