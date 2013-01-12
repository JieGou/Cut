<%@ Page Language="C#" %>
<%@ Import Namespace="OSGeo.MapGuide" %>
<%
    
    String sessionId = Request.Form.Get("SESSION");
    String district = Request.Form.Get("DISTRICT");
    String name = Request.Form.Get("NAME");

    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer(sessionId);    
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>执行要素查询</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
    <link href="styles/globalstyles.css" rel="stylesheet" type="text/css"/>
    <link href="styles/alphastyles.css" rel="stylesheet" type="text/css"/>
</head>
<body class="AppFrame">
<strong>查询结果: </strong>
<ul>
<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：执行空间查询要素
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
ArrayList list = utility.MakeQuery(Convert.ToInt32(district), name);
for (int i = 0; i < list.Count; i++)
{
    String address = list[i].ToString();
    if (address != "")
    {
        Response.Write("<li>" + list[i].ToString() + "</li>\n");
    } 
}
%>

</ul>  
    

</body>
</html>

<% //MapGuideApi.TerminateSockets(); %>
