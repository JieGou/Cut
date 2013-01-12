<%@ Page Language="C#" %>

<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：更新要素对象
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
    String sessionId = Request.Form.Get("SESSION");
    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer(sessionId);


    bool noSelection = false;
    ParcelProperty prop = utility.GetFirstSelectedFeature();
    if (prop == null)
    {
        noSelection = true;
        prop = new ParcelProperty();
    }    
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Update Feature</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
    <link href="styles/globalstyles.css" rel="stylesheet" type="text/css" />
    <link href="styles/alphastyles.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/mapguide/viewerfiles/viewer.css" type="text/css" />
</head>
<body class="AppFrame">
    <%
        if (noSelection)
        {
    %>
    
    <h1 class="AppHeading">请选择要更新的地块然后执行下面的操作。</h1>

    <form action="updatefeaturestask.aspx" method="post" id="refreshform">
        <input type="hidden" name="SESSION" value="<%= sessionId %>" />
        <input type="submit" value="Refresh" />
        
    </form>
    
    <%
        }
    %>
    <form id="form1" action="UpdateFeatures.aspx" method="post">
        <strong>面积:</strong><br />
        <input type="text" value="<%= prop.Acreage  %>" name="acreage"/><br />
        <strong>地址:</strong><br />
        <input type="text" value="<%= prop.BillingAddr  %>" name="billaddr"/><br />
        <strong>描述1:</strong><br />
        <input type="text" value="<%= prop.Description1  %>" name="desc1"/><br />
        <strong>描述2:</strong><br />
        <input type="text" value="<%= prop.Description2  %>" name="desc2"/><br />
        <strong>描述3:</strong><br />
        <input type="text" value="<%= prop.Description3  %>" name="desc3"/><br />
        <strong>描述4:<br />
        </strong>
        <input type="text" value="<%= prop.Description4 %>" name="desc4" /><br />
        <strong>规格:</strong><br />
        <input type="text" value="<%= prop.LotDimension  %>" name="lotdim"/><br />
        <strong>大小:</strong><br />
        <input type="text" value="<%= prop.LotSize %>" name="lotsize" /><br />
        <strong>户主:</strong><br />
        <input type="text" value="<%= prop.Owner  %>" name="owner"/><br />
        <strong>区域:<br />
        </strong>
        <input type="text" value="<%= prop.Zoning  %>" name="zoning"/><br /><br />
        <input type="hidden" value="<%= prop.ID %>" name="id"/>
        <input type="hidden" value="<%=sessionId %>" name="SESSION" />
        <input type="submit" value="Update" name="update"  <%= noSelection? "disabled" : "" %>/>
        
    </form>
</body>
</html>
