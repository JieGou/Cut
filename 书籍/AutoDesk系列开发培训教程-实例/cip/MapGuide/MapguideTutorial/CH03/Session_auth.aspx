<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="OSGeo.MapGuide" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：完成创建用户会话
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.08.23
    //          
    //         修改历史：无 
    //          
    //---------------------------------------------------------------------------------------   
    
    String UserID = Request.Form["UserName"];
	String Password = Request.Form["Password"];	
    try
    {

        // 获取网络配置文件
        // 网络配置文件需要从安装目录复制到站点所在的目录.
        string realPath = Request.ServerVariables["APPL_PHYSICAL_PATH"];
        String configPath = realPath + "webconfig.ini";
        if (!File.Exists(configPath))
        {
            Response.Write(configPath);
            Response.Write("文件没有找到 webconfig.ini ,终止");
            Response.End();
        }
        //初始化网络层
        MapGuideApi.MgInitializeWebTier(configPath);
        //创建用户信息对象
        MgUserInformation userInfo = new MgUserInformation(UserID, Password);
        // 通过用户信息对象创建站点连接       
        MgSiteConnection siteConn = new MgSiteConnection();
        siteConn.Open(userInfo);
        //获取站点对象
        MgSite site = new MgSite();
        site.Open(userInfo);
        //通过站点对象创建用户会话
        String sessionId = site.CreateSession();
        Response.Write("创建的用户会话 ID为: ");
        Response.Write(sessionId);
    }
    catch (MgException Mge)
    {
        Response.Write(Mge.GetMessage());
        Response.Write("<br>");
        Response.Write(Mge.GetDetails());
    }
%>



    


