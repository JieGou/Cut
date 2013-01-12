<%@ Page Language="C#" %>

<%--
Copyright (C) 2004-2007  Autodesk, Inc.
This library is free software; you can redistribute it and/or
modify it under the terms of version 2.1 of the GNU Lesser
General Public License as published by the Free Software Foundation.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
--%>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections.Specialized" %>
<%@ Import Namespace="OSGeo.MapGuide" %>

<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：向地图中添加层
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.08.23
    //          
    //         修改历史：无 
    //          
    //---------------------------------------------------------------------------------------   
    Response.Charset = "utf-8";
    String sessionId = Request.Form.Get("SESSION");
    try
    {
        try
        {         
            // 建立到站点的链接（通过封装类实现） 
            UtilityClass utility = new UtilityClass();
            utility.InitializeWebTier(Request);
            utility.ConnectToServer(sessionId);
            MgSiteConnection siteConnection = utility.GetSiteConnection();
            if (siteConnection == null)
            {
                Response.Write("建立到站点的链接失败，退出");
                 return;
            }
            // 创建资源服务
            MgResourceService resourceService = siteConnection.CreateService(MgServiceType.ResourceService) as MgResourceService;

            String layerDefinition = @"Library://MgTutorial/Layers/Hydrography.LayerDefinition";
            MgResourceIdentifier layerDefId = new MgResourceIdentifier(layerDefinition);
            //l打开地图对象
            MgMap map = new MgMap();
            map.Open(resourceService, "Sheboygan");
            // 获取层集合对象
            MgLayerCollection layers = map.GetLayers();
            // 创建新层，如果所创建的层已经存在，则抛出异常
            MgLayer layer  = new MgLayer(layerDefId, resourceService);
            layer.SetName("Hydrography2");
            layer.SetDisplayInLegend(true);
            layer.SetLegendLabel("Hydrography2");
            layer.SetVisible(true);
            layers.Add(layer);
            Response.Write("向地图中添加了层Hydrography2<br>");
            map.Save(resourceService);
        }
        catch (MgException exc)
        {
            Response.Write("层Hydrography2已经在地图中<br>");
            Response.Write(exc.GetMessage());
            return;
        }

    }
    finally
    {
    }
%>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Create Points Task</title>
    <script type="text/javascript">
        function RefreshMap() {
            parent.parent.GetMapFrame().Refresh();
        }
    </script>
</head>
<body onload="RefreshMap()">

</body>
</html>