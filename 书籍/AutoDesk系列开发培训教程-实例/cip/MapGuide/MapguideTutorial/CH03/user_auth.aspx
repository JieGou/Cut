<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_auth.aspx.cs" Inherits="user_auth" %>
<%@ Import Namespace = "System" %>
<%@ Import Namespace = "System.IO" %>
<%@ import Namespace = "OSGeo.MapGuide" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：通过用户名和密码完成用户认证
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
        // 获取网络配置文件，如果采用开源Mapguide，修改为相应的文件路径
        String configPath = @"C:\Program Files\Autodesk\MapGuideEnterprise2007\WebServerExtensions\www\webconfig.ini";
        if (!File.Exists(configPath))
        {
            Response.Write("文件没有找到 webconfig.ini ,终止");
            Response.End();
        }
        // 初始化网络层
        MapGuideApi.MgInitializeWebTier(configPath);
        //创建MgUserInformation对象
        MgUserInformation userInfo = new MgUserInformation(UserID, Password);

        //创建站点连接对象并打开站点对象
        MgSiteConnection siteConn = new MgSiteConnection();
        siteConn.Open(userInfo);
        Response.Write("<b>欢迎登陆 MapGuide 站点 <br>当前登陆的用户名为："); 
        Response.Write(UserID);
    }
    catch (MgException Mge)
    {
        Response.Write(Mge.GetMessage());
        Response.Write("<br>");
        Response.Write(Mge.GetDetails());
    }

%>
