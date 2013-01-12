<%@ Page Language="C#"  ContentType="text/xml" %>
<%
    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer("Anonymous", "");

    Response.Write(utility.createSheboygon3D());
    
%>
