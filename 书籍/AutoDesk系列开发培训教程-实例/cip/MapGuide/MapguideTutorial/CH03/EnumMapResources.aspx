<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="OSGeo.MapGuide" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%
    
    //---------------------------------------------------------------------------------------
    //
    //        功能：遍历资源
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
    String MapName;
    try
    {
        String configPath = @"C:\Program Files\Autodesk\MapGuideEnterprise2007\WebServerExtensions\www\webconfig.ini";

        if (!File.Exists(configPath))
        {
            Response.Write("文件没有找到 webconfig.ini ,终止");
            Response.End();
        }
        // 初始化网络成
        MapGuideApi.MgInitializeWebTier(configPath);
        //建立站点连接
        MgUserInformation userInfo = new MgUserInformation(UserID, Password);

        MgSiteConnection siteConn = new MgSiteConnection();
        siteConn.Open(userInfo);
        MgSite site = new MgSite();
        site.Open(userInfo);
        String sessionId = site.CreateSession();
   
        // 创建资源服务
        MgResourceService resService = (MgResourceService)siteConn.CreateService(MgServiceType.ResourceService);
        //指定要遍历的资源
        MgResourceIdentifier resourceID = new MgResourceIdentifier("Library://");

        // 获取当前库中的所有地图定义      
        MgByteReader byteRdr = resService.EnumerateResources(resourceID, -1, "MapDefinition");
         //处理返回结果
        String MapResStr = byteRdr.ToString();
        //载入XML文档，从中解析出所有的地图名称
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(MapResStr);
        //提取地图名称        
        XmlNodeList MapResIdNodeList;
        XmlElement root = doc.DocumentElement;
        MapResIdNodeList = root.SelectNodes("//ResourceId");
        int nRes = MapResIdNodeList.Count;

        Response.Write("<form>");
        Response.Write("<strong><span style=\"font-size: 16pt\">Site Maps</span></strong>");
        Response.Write("<br /><br />");
        Response.Write("<select Name=\"lstMapRes\" Style=\"z-index: 101; Width=\"115px\"; left: 51px; position: absolute; top: 223px\">");
        for (int i = 0; i < nRes; i++)
        {
            XmlNode MapResIdNode = MapResIdNodeList.Item(i);
            String MapRes = MapResIdNode.InnerText;
            int index1 = MapRes.LastIndexOf('/') + 1;
            int index2 = MapRes.IndexOf("MapDefinition") - 2;
            int length = index2 - index1 + 1;
            MapName = MapRes.Substring(index1, length);
            Response.Write(" <option value=\"" + MapRes + "\"\"Value\">" + MapName + "</option>");
        }

        Response.Write("</select>");
        Response.Write("<br /><br />");
        Response.Write("</form>");
    }
     
    catch (MgException Mge)
    {
        Response.Write(Mge.GetMessage());
        Response.Write("<br>");
        Response.Write(Mge.GetDetails());
    }

%>
