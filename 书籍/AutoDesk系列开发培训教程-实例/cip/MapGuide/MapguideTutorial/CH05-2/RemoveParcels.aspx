<%@ Page Language="C#" %>
<%
    
    String sessionId = Request.Form.Get("SESSION");
    string selectString = Request.Form.Get("selectstring");
    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer(sessionId);
    string delete = Request.Form.Get("delete");

    bool result = false;
    if (delete == "on")
    {
        result = utility.RemoveSelectedParcels(selectString);
    }
    
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>删除地块</title>
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
<%
    if (result)
    {
%>

<h1 class="AppHeading">已经成功删除地块</h1>
<input type="hidden" value="yes" id="successToken"/> 
<%
    }
    else
    {       
%>
<h1 class="AppHeading">确认要删除所选中的地块</h1>
<input type="hidden" value="no" id="successToken"/>
<form action="removeparcelstask.aspx" method="post">
    <input type="hidden" name="SESSION" value="<%= sessionId %>" />
    <input type="submit" value="回退" />
</form>

<%
    }
%>
</body>
</html>
