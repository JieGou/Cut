<%@ Page Language="C#" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%
    String sessionId = Request.Form.Get("SESSION");
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>要素查询操作</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta http-equiv="content-style-type" content="text/css" />
    <link href="styles/globalstyles.css" rel="stylesheet" type="text/css"/>
    <link href="styles/alphastyles.css" rel="stylesheet" type="text/css"/>
</head>
<body class="AppFrame">
    <form action="MakeQuery.aspx" method="post">
        <table border="0">
            <tr>
                <td colspan="2">
                    <strong>请先选择查询的区域(district ）<br />
                    </strong></td>
            </tr>
            <tr>
                <td style="width: 3px" valign="top">
                    <strong>区域 :</strong></td>
                <td style="width: 3px">
                    <select id="Select2" name="DISTRICT" style="width: 153px">
                        <option value="">选择区域 </option>
                        <option value="1">District 1</option>
                        <option value="2">District 2</option>
                        <option value="3">District 3</option>
                        <option value="4">District 4</option>
                        <option value="5">District 5</option>
                        <option value="6">District 6</option>
                        <option value="7">District 7</option>
                        <option value="8">District 8</option>
                        <option value="9">District 9</option>
                        <option value="10">District 10</option>
                        <option value="11">District 11</option>
                    </select>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <strong>输入名称，例如City of Sheboygan 或者 Schmitt.</strong><br/></td>
            </tr>
            <tr>
                <td style="width: 3px" valign="top">
                    <strong>Name</strong></td>
                <td style="width: 3px">
                    <input id="Text2" name="NAME" type="text" /><br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <input type="submit" />
                </td>
            </tr>
        </table>
        <input type="hidden" name="SESSION" value="<%= sessionId %>" />
    </form>
</body>
</html>
