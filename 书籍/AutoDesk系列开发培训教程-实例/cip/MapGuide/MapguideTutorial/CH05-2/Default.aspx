<%@ Page Language="C#" %>
<%@ Import Namespace="OSGeo.MapGuide" %>


<%
    UtilityClass utility = new UtilityClass();
    String webLayout = "Library://MgTutorial/Layouts/Sheboygan_CH05-2.WebLayout";
    String sessionId = "";

    try
    {
        utility.InitializeWebTier(Request);

        MgUserInformation userInfo = new MgUserInformation("Anonymous", "");
        MgSite site = new MgSite();

        site.Open(userInfo);

        sessionId = site.CreateSession();
    }
    catch (MgException mge)
    {
        Response.Write(mge.GetMessage());
        Response.Write(mge.GetDetails());
    }
    finally
    {
       
    }
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>CH05-2  要素操作</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type"/>
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
</head>

<frameset rows="0,*" border="0" framespacing="0">
  <frame />
  <frame src="/mapguide/mapviewernet/ajaxviewer.aspx?SESSION=<%= sessionId %>&WEBLAYOUT=<%= webLayout %>" name="ViewerFrame" />
</frameset>

</html>  

