<%@ Page Language="C#" %>

<%
    //---------------------------------------------------------------------------------------
    //
    //        功能：创建新的要素（地块）
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


     
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Update Feature</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
    <link href="styles/globalstyles.css" rel="stylesheet" type="text/css" />
    <link href="styles/alphastyles.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
        function DigitizePoint() {
            ShowResults("");
            parent.parent.mapFrame.DigitizePoint(OnPointDigitized);
        }
        function OnPointDigitized(point) {
            ShowResults("X: " + point.X + ", Y: " + point.Y);
        }
        function DigitizeLine() {
            ShowResults("");
            parent.parent.mapFrame.DigitizeLine(OnLineDigitized);
        }
        function OnLineDigitized(line) {
            ShowResults(FormatLineResults(line));
        }
        function DigitizePolyline() {
            ShowResults("");
            parent.parent.mapFrame.DigitizePolyline(OnPolylineDigitized);
        }
        function OnPolylineDigitized(line) {
            ShowResults(FormatLineResults(line));
        }
        function DigitizeCircle() {
            ShowResults("");
            parent.parent.mapFrame.DigitizeCircle(OnCircleDigitized);
        }
        function OnCircleDigitized(circle) {
            ShowResults("Center X: " + circle.Center.X + ", Y: " + circle.Center.Y + "\nRadius: " + circle.Radius);
        }
        function DigitizeRectangle() {
            ShowResults("");
            parent.parent.mapFrame.DigitizeRectangle(OnRectangleDigitized);
        }
        function OnRectangleDigitized(rect) {
            str = "X1: " + rect.Point1.X + ", Y1: " + rect.Point1.Y + "\n";
            str += "X2: " + rect.Point2.X + ", Y1: " + rect.Point2.Y + "\n";
            
            ShowResults(str);
        }
function DigitizePolygon() {
    document.getElementById("coordinates").value="";
    document.getElementById("geometrytype").value="polygon";
    parent.parent.mapFrame.DigitizePolygon(OnPolygonDigitized);
}
function OnPolygonDigitized(poly) {
    FormatLineResultsForParser(poly);
}
function FormatLineResultsForParser(line) {
    str = line.Count + "~";
    for(var i = 0; i < line.Count; i++) {
        pt = line.Point(i);
        str += pt.X + "!" + pt.Y + "_";
    }
    document.getElementById("coordinates").value = str;
}


        function ShowResults(res)  {
            document.getElementById("res").value = res;
        }
    </script>
</head>
<body class="AppFrame">   
    <h1 class="AppHeading">第一步:</h1>
    <div class="bold">在地图中定位到要创建新的地块的位置。</div>
    <br />
    <h1 class="AppHeading">第二步:</h1>
    <div class="bold">
        通过交互绘制地块。</div><br />
    <input type="button" value="绘制" onclick="javascript:DigitizePolygon();" /><br /><br />
    
    <h1 class="AppHeading">第三步:</h1>
    <div class="bold">填写属性信息.</div>
    
    <form id="form1" action="CreateParcels.aspx" method="post">
        <strong>面积:</strong><br />
        <input type="text" value="" name="acreage"/><br />
        <strong>地址:</strong><br />
        <input type="text" value="" name="billaddr"/><br />
        <strong>描述1:</strong><br />
        <input type="text" value="" name="desc1"/><br />
        <strong>描述2:</strong><br />
        <input type="text" value="" name="desc2"/><br />
        <strong>描述3:</strong><br />
        <input type="text" value="" name="desc3"/><br />
        <strong>描述4:<br />
        </strong>
        <input type="text" value="" name="desc4" /><br />
        <strong>规格:</strong><br />
        <input type="text" value="" name="lotdim"/><br />
        <strong> 大小:</strong><br />
        <input type="text" value="" name="lotsize" /><br />
        <strong>户主:</strong><br />
        <input type="text" value="" name="owner"/><br />
        <strong>地区:<br />
        </strong>
        <input type="text" value="" name="zoning"/><br /><br />
        <input type="hidden" name="SESSION" value="<%= sessionId %>"/>
        <input type="hidden" id="geometrytype" name="geometrytype"/>
        <input type="hidden" id="coordinates" name="coordinates" />
        <input type="submit" value="创建地块"/>
        
    </form>
</body>
</html>
