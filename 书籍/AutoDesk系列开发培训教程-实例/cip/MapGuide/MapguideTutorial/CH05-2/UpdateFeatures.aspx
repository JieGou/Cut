<%@ Page Language="C#" %>
<%@ Import Namespace="OSGeo.MapGuide" %>

<%
    String sessionId = Request.Form.Get("SESSION");
    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer(sessionId);

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
    prop.ID = Convert.ToInt32(id);
    prop.LotDimension = lotdim;
    prop.LotSize = (lotsize == "" ? 0 : Convert.ToInt32(lotsize));
    prop.Owner = owner;
    prop.Zoning = zoning;    
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Update Feature</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
    <link href="styles/globalstyles.css" rel="stylesheet" type="text/css" />
    <link href="styles/alphastyles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function RefreshMap() {
            var token;
            token = document.getElementById("successToken").value;
            if (token == "yes"){
                parent.parent.Refresh();
            }
        }
    </script>

</head>
<body class="AppFrame" onload="javascript:RefreshMap()">

<h1 class="AppHeading">

<% if (utility.UpdateFeature(prop))
   {
%>

The feature <%= prop.ID %> has been successfully updated. Please re-select the parcel to examine its new feature values.
<input type="hidden" value="yes" id="successToken"/> 

<%
    }
%>

</h1>

</body>
</html>
