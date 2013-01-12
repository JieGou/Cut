<%@ Page Language="C#" ContentType="text/xml" %>
<%@ Import Namespace="OSGeo.MapGuide" %>

<%

    //---------------------------------------------------------------------------------------
    //
    //        功能：获取系统支持的FDO provider 列表
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
    // 初始化网络层   
    MapGuideApi.MgInitializeWebTier(Request.ServerVariables["APPL_PHYSICAL_PATH"] + "webconfig.ini");
    //建立站点链接
    MgSiteConnection siteConnection = new MgSiteConnection();
    siteConnection.Open(new MgUserInformation("Anonymous", ""));
    // 创建要素服务
    MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);
     
    // 获取系统支持的FDO provider 列表,并将结果输出 
        
     MgByteReader reader = featureService.GetFeatureProviders();
    String providers = reader.ToString();
    Response.Write(providers);
%>



