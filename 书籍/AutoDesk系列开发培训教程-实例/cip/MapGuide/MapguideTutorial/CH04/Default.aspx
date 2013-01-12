<%@ Page Language="C#" %>
<%@ Import Namespace = "OSGeo.MapGuide"%>
<%
  
    UtilityClass utility = new UtilityClass();
    String webLayout = "Library://Samples/Sheboygan/Layouts/Sheboygan_CH04.WebLayout";

    String sessionId = "";
    try
    {

        utility.InitializeWebTier(Request);
        MgUserInformation userInfo = new MgUserInformation("Anonymous", "");
        MgSite site= new MgSite();
        
        site.Open(userInfo);
        sessionId = site.CreateSession();
    }
    catch(MgException ex)
    {
        Response.Write(ex.GetMessage());
        Response.Write(ex.GetDetails());
    }
    finally
    {
        //MapGuideApi.TerminateSockets();
    }
    
 %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>第 4 章  层和地图</title>
</head>
<frameset rows="0,*" border="0" framespacing="0">
  <frame />
  <frame src="/mapguide/mapviewernet/ajaxviewer.aspx?SESSION=<%= sessionId %>&WEBLAYOUT=<%= webLayout %>" name="ViewerFrame" />
</frameset>
</html>
