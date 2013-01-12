<%@ Page Language="C#" %>

<%@ Import Namespace="OSGeo.MapGuide" %>
<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：定制界面布局
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.08.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
    //初始化网络层
    MapGuideApi.MgInitializeWebTier(@"C:\Program Files\Autodesk\MapGuideEnterprise2007\WebServerExtensions\www\webconfig.ini");
    // 建立到站点的链接
    MgUserInformation userInfo = new MgUserInformation("Anonymous", "");
    MgSite site = new MgSite();
    site.Open(userInfo);
    String sessionId = site.CreateSession();
    // 指定多引用的网络布局 （WebLayout）
    String webLayout = @"Library://Sample_World/Layouts/Sample_World _CH02.WebLayout";
    
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>CH02-Mapguide定制界面布局</title>
</head>
<frameset rows="*,80">
    <frameset cols="*, 250">
        <frame src="/mapguide/mapviewernet/ajaxviewer.aspx?SESSION=<%= sessionId %>&WEBLAYOUT=<%= webLayout %>"
            name="ViewerFrame" />
        <frameset rows="*,*">
            <frame src="InfoPage.htm" name="InfoFrame" />
            <frame src="" name="DynaFrame" />
        </frameset>
    </frameset>
    <frame src="Toolbar.htm" name="ControlFrame" />

</frameset>
</html>
