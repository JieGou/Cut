<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CH03:站点和资源管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ul>
          <li>
            <h1>站点和资源管理</h1>
          </li>
          <li>
            <h2><strong>站点管理</strong></h2>
          <ul>
            <li><span style="font-family: Arial"><a href="UserAdmin.htm" target="_blank">用户认证</a></span></li>
              <li><span style="font-family: Arial">
                <a href="CreateSession.htm" target="_blank">创建会话</a></span><br />
              </li>
          </ul>
          </li>
          <li>
            <h2>资源管理</h2>
          </li>
        </ul>
        <ul>
          <ul>
            <li><a href="GetResourceHeader.aspx" target="_blank">获取资源标题（以地图资源为例）</a></li>
            <li><a href="GetResourceContent.aspx" target="_blank">获取资源内容（以地图资源为例）</a></li>
            <li><a href="RespositoryHeader.aspx" target="_blank">获取仓储标题</a></li>
            <li><a href="RespositoryContent.aspx"target="_blank">获取仓储内容</a></li>
          </ul>
        </ul>
    </div>
    </form>
</body>
</html>
