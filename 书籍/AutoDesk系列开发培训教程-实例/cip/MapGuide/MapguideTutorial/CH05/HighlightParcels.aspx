<%@ Page Language="C#" %>
<%@ Import Namespace="OSGeo.MapGuide" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%
    
     //---------------------------------------------------------------------------------------
    //
    //        功能：查询要素,高亮显示查询结果
    //
    //         作者：Qin H.X.
    //
    //         日期： 2007.5.23
    //          
    //         修改历史：无 
    //          
    //--------------------------------------------------------------------------------------- 
   
    String sessionID = Request.Form.Get("SESSION");
    // 获取用户的查询条件 
    String queryString = Request.Form.Get("QuertyString");

    // 链接站点
    UtilityClass utility = new UtilityClass();
    utility.InitializeWebTier(Request);
    utility.ConnectToServer(sessionID);
    // 
    utility.CreateSelectionXML(queryString);
    String selectionXML = utility.SelectionXML;
    String selectionResult = utility.SelectionResult;
%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
    <link href="styles/globalstyles.css" rel="stylesheet" type="text/css"/>
    <link href="styles/alphastyles.css" rel="stylesheet" type="text/css"/>
    <title>高亮显示选中的地块</title>

    <script type="text/javascript">
        
        //将查询结果（XML格式）传递给Viewer
        function onPageLoad() {
            var selectionXML = '<%= selectionXML %>';
            parent.parent.SetSelectionXML(selectionXML);         
        }
    </script>

</head>


<body onload="onPageLoad()" class="AppFrame">
    <div class="bold">
        请缩放至地块级别（ parcels),查看选中的对象。
    </div>
    <br />
    <%= selectionResult %>
    
</body>
</html>
<%
%>

