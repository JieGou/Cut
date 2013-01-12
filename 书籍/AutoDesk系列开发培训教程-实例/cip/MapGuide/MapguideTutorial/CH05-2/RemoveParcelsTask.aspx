<%@ Page Language="C#" %>

<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：删除指定的要素对象
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

    bool hasSelection = utility.ListSelections();
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Remove Parcels</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
    <link href="styles/globalstyles.css" rel="stylesheet" type="text/css" />
    <link href="styles/alphastyles.css" rel="stylesheet" type="text/css" />
</head>
<body class="AppFrame">

<%
    if (hasSelection)
    {
        string selectString = utility.SelectString;
        
%>
<h1 class="AppHeading">确认要删除地块？</h1>
<%= utility.SelectionResult%>
    <form action="RemoveParcels.aspx" method="post">
        <input type="checkbox" name="delete" />是,我要删除地块。<br />
        <br />
        <input type="submit" value="删除" />
        <input type="hidden" value="<%= sessionId %>" name="SESSION" /> 
        <input type="hidden" value="<%= selectString %>" name="selectstring" />
    </form>
<%
    }
    else
    {
%>

<h1 class="AppHeading">请先从地图中选择要删除的地块。</h1>
<form action="removeparcelstask.aspx" method="post">
    <input type="hidden" name="SESSION" value="<%= sessionId %>" />
    <input type="submit" value="更新" />
</form>

<%
    }
%>
</body>
</html>
