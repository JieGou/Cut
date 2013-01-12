<%@ Page Language="C#" AutoEventWireup="true"  %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections.Specialized" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="OSGeo.MapGuide" %>


<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：获取地图数据
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

        MgMap map = new MgMap();
        map.Open(resService, "Sheboygan");

        //输出地图属性
        Response.Write("<b>  地图名称 : </b>  " + map.GetName() + "</p>");
        Response.Write("<b>  地图的会话ID: </b>" + map.GetSessionId() + "</p>");
        Response.Write("<b>  对象的 ID :    </b> " + map.GetObjectId() + "</p>");
        Response.Write("<b>  显示比例 :  </b> " + map.GetViewScale() + "</p>");

        MgEnvelope envelope = map.GetMapExtent();
        MgCoordinate lowerLeft = envelope.GetLowerLeftCoordinate();
        MgCoordinate upperRight = envelope.GetUpperRightCoordinate();
        Response.Write("<b>  左下角坐标:  </b>");
        Response.Write("(" + lowerLeft.GetX() + "," + lowerLeft.GetY() + "," + lowerLeft.GetZ() + ")");
        Response.Write("<BR>");
        Response.Write("<b> 右上角坐标: </b>");
        Response.Write("(" + upperRight.GetX() + "," + upperRight.GetY() + "," + upperRight.GetZ() + ")");

        Response.Write("<p><b>当前地图包含以下层组:</b></p>");
        MgLayerGroupCollection LayerGrpCollection = map.GetLayerGroups();
        for (int j = 0; j < LayerGrpCollection.GetCount(); j++)
        {
            Response.Write("<p>     " + LayerGrpCollection.GetItem(j).GetName() + "</p>");
        }
        MgLayerCollection layerCollection;
        layerCollection = map.GetLayers();
        MgLayer layer = null;
        Response.Write("<p><b>包含以下层:</b></p>");
        for (int i = 0; i < layerCollection.GetCount(); i++)
        {
            layer = layerCollection.GetItem(i);
            Response.Write("<p>     " + layer.GetName() + "</p>");
        }
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
    <title>获取地图数据 </title>
</head>
<body>
<form id="form1" runat="server">

</form>
</body>
</html>

