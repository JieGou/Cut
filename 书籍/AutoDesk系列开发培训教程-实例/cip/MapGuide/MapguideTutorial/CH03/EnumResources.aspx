<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="OSGeo.MapGuide" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：获取库中的资源
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.08.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
	String UserID = "Administrator";
	String Password = "admin";
    try
    {
        //配置文件
        String configPath = @"C:\Program Files\Autodesk\MapGuideEnterprise2007\WebServerExtensions\www\webconfig.ini";

        if (!File.Exists(configPath))
        {
            Response.Write("文件没有找到 webconfig.ini ,终止");
            Response.End();
        }
        string realPath = Request.ServerVariables["APPL_PHYSICAL_PATH"];
        
        //指定存储文件
        String AllResFilePath = realPath + @"Support\AllResources.xml";
        // 初始化网络层
        MapGuideApi.MgInitializeWebTier(configPath);

        //建立站点链接
        MgUserInformation userInfo = new MgUserInformation(UserID, Password);
        MgSiteConnection siteConn = new MgSiteConnection();
        siteConn.Open(userInfo);
        //创建会话
        MgSite site = new MgSite();
        site.Open(userInfo);
        String sessionId = site.CreateSession();


        // 创建资源服务
        MgResourceService resService = (MgResourceService)siteConn.CreateService(MgServiceType.ResourceService);
        MgResourceIdentifier resourceID = new MgResourceIdentifier("Library://");

        ///获取库中的资源
        MgByteReader byteReader = resService.EnumerateResources(resourceID, -1, "");
        //  保存文档
        MgByteSink byteSink = new MgByteSink(byteReader);
        byteSink.ToFile(AllResFilePath);
         
        Response.Write("<br><br><br><br><br><br><br>");
        Response.Write("<b> 资源文件 " + AllResFilePath + " 创建完毕.");
    
    }
     
    catch (MgException Mge)
    {
        Response.Write(Mge.GetMessage());
        Response.Write("<br>");
        Response.Write(Mge.GetDetails());
    }


%>