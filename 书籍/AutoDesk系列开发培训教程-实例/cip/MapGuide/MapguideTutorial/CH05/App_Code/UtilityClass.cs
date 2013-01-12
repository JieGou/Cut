using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using OSGeo.MapGuide;
using System.Collections;

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
    MgSiteConnection siteConnection;
    String selectionResult;
    String selectionXML;

    public String SelectionResult
    {
        get { return selectionResult; }
        set { selectionResult = value; }
    }

    public String SelectionXML
    {
        get { return selectionXML; }
        set { selectionXML = value; }
    }

    public NameValueCollection GetParameters(HttpRequest Request)
    {
        return Request.HttpMethod == "GET" ? Request.QueryString : Request.Form;
    }

    public void InitializeWebTier(HttpRequest Request)
    {
        string realPath = Request.ServerVariables["APPL_PHYSICAL_PATH"];
        String configPath = realPath + "webconfig.ini";
        MapGuideApi.MgInitializeWebTier(configPath);
    }

    public void ConnectToServer(String sessionID)
    {
        MgUserInformation userInfo = new MgUserInformation(sessionID);
        siteConnection = new MgSiteConnection();
        siteConnection.Open(userInfo);
    }

    //---------------------------------------------------------------------------------------
    //
    //        功能：将查询结果转化为XML格式的字符
    //
    //         作者： 
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
    public void CreateSelectionXML(String queryString)
    {
        //创建资源和要素服务
        MgResourceService resService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);
        //获取要查询的要素类别所在的层
        MgMap map = new MgMap();
        map.Open(resService, "Sheboygan");
        MgLayer layer = map.GetLayers().GetItem("Parcels");
        //执行要素查询
        MgResourceIdentifier resId = new MgResourceIdentifier(layer.GetFeatureSourceId());
        MgFeatureQueryOptions queryOption = new MgFeatureQueryOptions();
        queryOption.SetFilter(queryString);
        MgFeatureReader featureReader = featureService.SelectFeatures(resId, "Parcels", queryOption);
          //创建选择集
        MgSelection selection = new MgSelection(map);
        selection.AddFeatures(layer, featureReader, 0);
        //处理选择集，并转化为XML
        OutputSelectionInHTML(selection, featureService);
        selectionXML = selection.ToXml();
    }
    //---------------------------------------------------------------------------------------
    //
    //        功能：输出查询结果
    //
    //         作者： 
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
    public void OutputSelectionInHTML(MgSelection selection, MgFeatureService featureService)
    {
        //返回被选中的层
        MgReadOnlyLayerCollection layers = selection.GetLayers();
        String outString = null;
        MgFeatureReader featReader = null;
        if (layers != null)
        {
            //对每一个层处理
            for (int i = 0; i < layers.GetCount(); i++)
            {
                MgLayer layer = layers.GetItem(i);
                if ((layer != null) && (layer.GetName() == "Parcels"))
                {
                    //返回选中的要素类别
                    String layerClassName = layer.GetFeatureClassName();
                    // 创建选择集过滤条件
                    String selectString = selection.GenerateFilter(layer, layerClassName);
                    String layerFeatureIdString = layer.GetFeatureSourceId();
                    MgResourceIdentifier layerResId = new MgResourceIdentifier(layerFeatureIdString);
                    MgFeatureQueryOptions queryOptions = new MgFeatureQueryOptions();
                    queryOptions.SetFilter(selectString);
                    // 为选中的要素创建MgFeatureReader对象
                    featReader = featureService.SelectFeatures(layerResId, layerClassName, queryOptions);

                    ///处理MgFeatureReader 对象, 获取每个选中的要素
                   outString = "\n";
                    outString = outString + "<table border=\"1\">\n";

                    double acre = 0;
                    // 
                    outString = outString + "<tr>\n";

                    outString = outString + "<td>RNAME</td>\n";
                    outString = outString + "<td>RPROPAD</td>\n";
                    outString = outString + "<td>RACRE</td>\n";
                    outString = outString + "</tr>\n";
                    while (featReader.ReadNext())
                    {
                        outString = outString + "<tr>\n";
                        outString = outString + "<td>";
                        outString = outString + featReader.GetString("RNAME");
                        outString = outString + "</td>\n";
                        outString = outString + "<td>";
                        outString = outString + featReader.GetString("RPROPAD");
                        outString = outString + "</td>\n";
                        outString = outString + "<td>";
                        String acreString = featReader.GetString("RACRE");
                        acre = acre + (acreString == "" ? 0 : Convert.ToDouble(acreString));
                        outString = outString + acreString;
                        outString = outString + "</tr>\n";
                    }
                    outString = outString + "</table>\n";
                }
            }

        }
        selectionResult = outString;
    }
    //---------------------------------------------------------------------------------------
    //
    //        功能：列出当前选择的要素数据
    //
    //         作者： 
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
    public int ListSelections()
    {
        // 创建资源服务和要素服务
        MgResourceService resService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgFeatureService featService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        //打开地图对象并创建选择集对象
        MgMap map = new MgMap();
        map.Open(resService, "Sheboygan");
        MgSelection mapSelection = null;
        mapSelection = new MgSelection(map);
        mapSelection.Open(resService, "Sheboygan");
        // 处理选择集
        if (mapSelection.GetLayers() != null)
        {
            OutputSelectionInHTML(mapSelection, featService);
            return 1;
        }
        else
        {
            return 0;
        }
    }
    //---------------------------------------------------------------------------------------
    //
    //        功能：查询要素
    //
    //         作者： 
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
    public ArrayList MakeQuery(int districtNum, String name)
    {
        ArrayList list = null;
        //获取资源服务和要素服务
        MgResourceService resService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);

        MgMap map = new MgMap();
        map.Open(resService, "Sheboygan");
        // 指定查询的过滤条件
        MgFeatureQueryOptions districtQuery = new MgFeatureQueryOptions();
        districtQuery.SetFilter("Autogenerated_SDF_ID = " + districtNum);
        ////指定查询的要素源的资源标识符（投票选区VotingDistricts）
        MgResourceIdentifier districtId = new MgResourceIdentifier("Library://MgTutorial/Data/VotingDistricts.FeatureSource");
        MgFeatureReader featureReader = featureService.SelectFeatures(districtId, "VotingDistricts", districtQuery);
        // 处理查询结果（投票选区VotingDistricts）
        featureReader.ReadNext();
        MgByteReader geometryBytes = featureReader.GetGeometry("Data");
        MgAgfReaderWriter agfReader = new MgAgfReaderWriter();
        MgGeometry districtGeometry = agfReader.Read(geometryBytes);

        // 指定查询的过滤条件：在指定投票选区VotingDistricts范围内查询
        MgFeatureQueryOptions nameQuery = new MgFeatureQueryOptions();
        nameQuery.SetFilter("RNAME LIKE '" + name + "%'");
        nameQuery.SetSpatialFilter("SHPGEOM", districtGeometry, MgFeatureSpatialOperations.Inside);

        ////指定查询的要素源的资源标识符（地块Parcels）
        MgResourceIdentifier parcelId =new MgResourceIdentifier("Library://MgTutorial/Data/Parcels.FeatureSource");
        featureReader = featureService.SelectFeatures(parcelId, "Parcels", nameQuery);
        // 处理查询结果
        list = new ArrayList();
        while (featureReader.ReadNext())
        {
            // 获取RPROPAD属性段对应的数据（String类型）
            list.Add(featureReader.GetString("RPROPAD"));
        }

        String schema = featureService.DescribeSchemaAsXml(parcelId, "");

        return list;
    }


      //---------------------------------------------------------------------------------------
    //
    //        功能：GetPropertyType()函数和GetPropertyName()函数的使用
    //
    //         作者： 
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
       public void OutputReader(MgFeatureReader featureReader)
       {
           while (featureReader.ReadNext())
           {
               int propCount = featureReader.GetPropertyCount();
               for (int j = 0; j < propCount; j++)
               {
                   string propertyName = featureReader.GetPropertyName(j);
                   bool boolVal = featureReader.IsNull(propertyName);
                   if (boolVal)
                   {
                       continue;
                   }
                   //MgPropertyType propertyType = (MgPropertyType)featureReader.GetPropertyType(propertyName);
                   int proType = featureReader.GetPropertyType(propertyName);
                   GetPropertyValue(featureReader, proType, propertyName);
               }
           }

       }

     
       //---------------------------------------------------------------------------------------
       //
       //        功能：未知查询结果中的属性数据的类型的处理方法
       //
       //
       //         作者： 
       //
       //         日期： 2007.5.23
       //          
       //         修改历史：无 
       //          
       //---------------------------------------------------------------------------------------   
    
    public void GetPropertyValue(MgFeatureReader featureReader, int propertyType, string propertyName)
       {
           switch (propertyType )
           {
               case MgPropertyType.Null:
                   break;
               case MgPropertyType.Boolean:
                   bool val1 = featureReader.GetBoolean(propertyName);
                   break;
               case MgPropertyType.Byte:
                   Byte val2 = featureReader.GetByte(propertyName);
                   break;
               case MgPropertyType.DateTime:
                   MgDateTime val3 = featureReader.GetDateTime(propertyName);
                   break;
               case MgPropertyType.Single:
                   float val4 = featureReader.GetSingle(propertyName);
                   break;
               case MgPropertyType.Double:
                   double val5 = featureReader.GetDouble(propertyName);
                   break;
               case MgPropertyType.Int16:
                   Int16 val6 = featureReader.GetInt16(propertyName);
                   break;
               case MgPropertyType.Int32:
                   Int32 val7 = featureReader.GetInt32(propertyName);
                   break;
               case MgPropertyType.Int64:
                   Int64 val8 = featureReader.GetInt64(propertyName);
                   break;
               case MgPropertyType.String:
                   string val9 = featureReader.GetString(propertyName);
                   break;
               case MgPropertyType.Blob:
                   string val10 = propertyName + " 是Blob类型";
                   break;
               case MgPropertyType.Clob:
                   string val11 = propertyName + " 是Clob类型";
                   break;
               case MgPropertyType.Feature:
                   MgFeatureReader val12 = featureReader.GetFeatureObject(propertyName);
                   break;
               case MgPropertyType.Geometry:
                   MgByteReader va13l = featureReader.GetGeometry(propertyName);
                   MgAgfReaderWriter agfReader = new MgAgfReaderWriter();
                   MgGeometry districtGeometry = agfReader.Read(va13l);
                   break;
               case MgPropertyType.Raster:
                   MgRaster val14 = featureReader.GetRaster(propertyName) ;
                   break;
               default:
                   string val13 =  "未知类型";
                   break;
           }
       }



}
