<%@ Page Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections.Specialized" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="OSGeo.MapGuide" %>


<% 
    
   String sessionId = Request.Form.Get("SESSION");
    try
    {
        //
        UtilityClass utility = new UtilityClass();
        utility.InitializeWebTier(Request);
        utility.ConnectToServer(sessionId);
        MgSiteConnection siteConnection = utility.GetSiteConnection();
        if (siteConnection == null) {
            Response.Write("建立链接失败，退出");
            return;
        }

        utility.GenerateMapImage();

        Response.Write(@"<p><b>图象文件保存到 C:\Temp\Map.PNG</b></p>");
        Response.Write(  "<p><b> 请查看图片效果。 </b></p>");


    }
    catch (MgException ex)
    {
        Response.Write(ex.GetMessage ());
        Response.Write(ex.GetDetails());
    }
 
 %>
 
 
 
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CreateMapInPlainImage</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
