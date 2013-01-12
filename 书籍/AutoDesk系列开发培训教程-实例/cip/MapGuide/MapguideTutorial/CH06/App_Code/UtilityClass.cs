using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
// the above are the defautl reference
// Mapguide reference library
using OSGeo.MapGuide;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;

public class UtilityClass
{
    // 站点链接对象 
    MgSiteConnection siteConnection;

    public UtilityClass()
    {
    }
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
    // 功 能： 将地图输出为DWF文件
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public bool PlotMap()
    {
        // 获取资源服务和地图服务对象
        MgResourceService resourceService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgMappingService mappingService = (MgMappingService)siteConnection.CreateService(MgServiceType.MappingService);
        //  打开地图
        MgMap map = new MgMap();
        map.Open(resourceService, "Sheboygan");
        // 设置DWF的设置信息
        MgDwfVersion dwfVer = new MgDwfVersion("6.01", "1.2");
        MgPlotSpecification plotSpec = new MgPlotSpecification(8.5f, 11.0f, MgPageUnitsType.Inches, 0f, 0f, 0f, 0f);
        plotSpec.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);

         // 输出DWF文件
        MgResourceIdentifier layoutRes = new MgResourceIdentifier("Library://MgTutorial/Layouts/SheboyganMap.PrintLayout");
        MgLayout layout = new MgLayout(layoutRes, " DWF文件创建示例", MgPageUnitsType.Inches);
        MgByteReader byteReader = mappingService.GeneratePlot(map, plotSpec, layout, dwfVer);
         //保存文件
        MgByteSink byteSink = new MgByteSink(byteReader);
        string filePath = "C:\\Temp\\Map.DWF";
        byteSink.ToFile(filePath);
        
        return true;
    }

    //----------------------------------------------------------------------------------------
    // 功 能： 将地图输出为图象文件
    //
    // 作 者： 
    //
    //
    // 日 期：2007.05.#
    //
    //-----------------------------------------------------------------------------------------
    public bool GenerateMapImage()
    {
        // 获取资源服务和渲染服务对象
        MgResourceService resourceService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgRenderingService renderingService = (MgRenderingService)siteConnection.CreateService(MgServiceType.RenderingService);
        //  打开地图
        MgMap map = new MgMap();
        map.Open(resourceService, "Sheboygan");
      
        //  构造选择集
        MgSelection selection = new MgSelection(map);
        selection.Open(resourceService, "Sheboygan");
        // 指定范围
        MgEnvelope extent = map.GetMapExtent();
        double x = 0.5*extent.GetLowerLeftCoordinate().GetX();
        double y = 0.5*extent.GetLowerLeftCoordinate().GetY() ;

        MgColor color = new MgColor("FFFFBF20");
        // 生成并保存图象文件
        MgByteReader byteReader = renderingService.RenderMap(map, selection, extent, 450, 613, color, "PNG");
        MgByteSink byteSink = new MgByteSink(byteReader);
        string filePath = "C:\\Temp\\Map.PNG";
        byteSink.ToFile(filePath);
        return true;
    }

}
