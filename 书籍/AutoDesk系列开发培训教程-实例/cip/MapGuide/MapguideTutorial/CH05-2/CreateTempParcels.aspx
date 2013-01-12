<%@ Page Language="C#" %>
<%@ Import Namespace="OSGeo.MapGuide" %>

<%
    String sessionId = Request.Form.Get("SESSION");
    String coordinates = Request.Form.Get("coordinates");
    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer(sessionId);

    // 创建几何对象
    MgPolygon geo = utility.parseString(coordinates);
    MgAgfReaderWriter agfWriter = new MgAgfReaderWriter();
    MgWktReaderWriter wktWriter = new MgWktReaderWriter();
    //转化为WKT 文本 
    string polygon = wktWriter.Write(geo);
    //获取其他属性 
    string acreage = Request.Form.Get("acreage");
    string billaddr = Request.Form.Get("billaddr");
    string desc1 = Request.Form.Get("desc1");
    string desc2 = Request.Form.Get("desc2");
    string desc3 = Request.Form.Get("desc3");
    string desc4 = Request.Form.Get("desc4");
    string lotdim = Request.Form.Get("lotdim");
    string lotsize = Request.Form.Get("lotsize");
    string owner = Request.Form.Get("owner");
    string zoning = Request.Form.Get("zoning");
    string id = Request.Form.Get("id");

    ParcelProperty newParcel = new ParcelProperty();
    newParcel.Acreage = acreage;
    newParcel.BillingAddr = billaddr;
    newParcel.Description1 = desc1;
    newParcel.Description2 = desc2;
    newParcel.Description3 = desc3;
    newParcel.Description4 = desc4;
    newParcel.LotDimension = lotdim;
    newParcel.LotSize = (lotsize == "" ? 0 : Convert.ToInt32(lotsize));
    newParcel.Owner = owner;
    newParcel.Zoning = zoning;

    utility.createTempParcels(geo, newParcel, sessionId);
        
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Create Parcels</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
    <link href="styles/globalstyles.css" rel="stylesheet" type="text/css" />
    <link href="styles/alphastyles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function RefreshMap() {
            parent.parent.Refresh();
        }
    </script>
</head>
<body onload="javascript:RefreshMap()">
    <h1 class="AppHeading">新地块创建完成</h1>
    
    
</body>
</html>
