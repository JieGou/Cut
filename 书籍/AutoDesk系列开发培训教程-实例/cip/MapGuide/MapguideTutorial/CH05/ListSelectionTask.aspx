<%@ Page Language="C#" %>
<%@ Import Namespace="OSGeo.MapGuide" %>

<%
    String sessionID = Request.Form.Get("SESSION");
    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer(sessionID);
    int hasResult = utility.ListSelections();
    
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>List Selection</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
    <link href="styles/globalstyles.css" rel="stylesheet" type="text/css"/>
    <link href="styles/alphastyles.css" rel="stylesheet" type="text/css"/>
</head>
<body class="AppFrame">

<%
    if (hasResult != 0)
    {
        Response.Write("<div class=\"bold\" style=\"font-size: 15pt;\">\n");
        Response.Write(" 当前选择集中的数据为：\n");
        Response.Write("</div><br>\n");
        Response.Write(utility.SelectionResult);
    }
    else
    {
%>

<div class="bold" style="font-size: 15pt;">
   先借助选取工具选择对象。
</div>

<%  
    }
    %>

</body>
</html>
