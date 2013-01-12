<%@ Page Language="C#"  ContentType="text/xml" %>
<%
    String query = Request.QueryString.Get("RTYPE");
    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer("Anonymous", "");

    Response.Write(utility.createQueryPolygon("RTYPE='" + query + "'"));
%>

