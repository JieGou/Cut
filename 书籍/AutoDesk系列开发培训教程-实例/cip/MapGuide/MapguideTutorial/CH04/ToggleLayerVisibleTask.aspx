<%@ Page Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections.Specialized" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="OSGeo.MapGuide" %>


<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：控制层在地图中的可见性
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.08.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
    String sessionId = Request.Form.Get("SESSION");
    try
    {
        UtilityClass utility = new UtilityClass();
        utility.InitializeWebTier(Request);
        utility.ConnectToServer(sessionId);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        if (siteConnection == null)
        {
            Response.Write("建立到Mapguide站点链接时候失败，退出");
            return;
        }

        MgResourceService resService = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
        MgLayer tmpLayer = null;

        MgMap map = new MgMap();
        map.Open(resService, "Sheboygan");
        tmpLayer = utility.GetLayerByName(map, "Hydrography");
        tmpLayer.SetVisible(!tmpLayer.IsVisible());
        tmpLayer.ForceRefresh();
        map.Save(resService);
        
        if (tmpLayer.IsVisible())
            Response.Write("<p><b> 打开Hydrography层 </b></p>");
        else
            Response.Write("<p><b>关闭Hydrography层 </b></p>");
           
    }
    catch (MgException ex)
    {
        Response.Write(ex.GetMessage ());
        Response.Write(ex.GetDetails());
    }
    

%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Change Layer Visibility</title>

<script type="text/javascript">
    function RefreshMap() {
       parent.parent.Refresh();
    }
</script>

   
</head>
<body onload="javascript:RefreshMap()">

</body>
</html>
