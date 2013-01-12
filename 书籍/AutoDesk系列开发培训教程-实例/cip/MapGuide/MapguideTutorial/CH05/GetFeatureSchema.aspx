<%@ Page Language="C#" ContentType="text/xml" %>
<%@ Import Namespace="OSGeo.MapGuide" %>
<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：获取系要素模式
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
    String source = Request.QueryString.Get("source");
    String featureSource = "";
    switch (source)
    {
        case "city":
            featureSource = "Library://MgTutorial/Data/CityLimits.FeatureSource";
            break;
        case "parcel":
            featureSource = "Library://MgTutorial/Data/Parcels.FeatureSource";
            break;
    }
    
    
    MapGuideApi.MgInitializeWebTier(Request.ServerVariables["APPL_PHYSICAL_PATH"] + "webconfig.ini");

    MgSiteConnection siteConnection = new MgSiteConnection();
    siteConnection.Open(new MgUserInformation("Anonymous", ""));

    MgFeatureService featureService = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);
    MgResourceIdentifier resId = new MgResourceIdentifier(featureSource);

 
    String schema = featureService.DescribeSchemaAsXml(resId, "");
    Response.Write(schema);

    
%>

