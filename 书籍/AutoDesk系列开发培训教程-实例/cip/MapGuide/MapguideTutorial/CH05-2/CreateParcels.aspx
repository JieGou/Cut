<%@ Page Language="C#" %>
<%@ Import Namespace="OSGeo.MapGuide" %>

<%
    String sessionId = Request.Form.Get("SESSION");
    String coordinates = Request.Form.Get("coordinates");
    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer(sessionId);

    MgPolygon geo = utility.parseString(coordinates);

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

    ParcelProperty prop = new ParcelProperty();
    prop.Acreage = acreage;
    prop.BillingAddr = billaddr;
    prop.Description1 = desc1;
    prop.Description2 = desc2;
    prop.Description3 = desc3;
    prop.Description4 = desc4;
    prop.LotDimension = lotdim;
    prop.LotSize = (lotsize == "" ? 0 : Convert.ToInt32(lotsize));
    prop.Owner = owner;
    prop.Zoning = zoning;


    utility.createNewParcel(geo, prop);
        
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
    <h1 class="AppHeading">地块创建成功</h1>
    
    
</body>
</html>
