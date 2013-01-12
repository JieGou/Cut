using System;
using Autodesk.Revit;

namespace RevitDevelop
{
    public class MyMenu : IExternalApplication
    {
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            //创建一级菜单，参数即为菜单名称
            MenuItem topMenu = application.CreateTopMenu("My Menu");
            if (topMenu == null)
                return IExternalApplication.Result.Failed;
            //菜单类型除了BasicMenu，还有PopupMenu, SeparatorMenu
            MenuItem.MenuType basicM = MenuItem.MenuType.BasicMenu;
            //Append参数含义分别为：菜单类型，名称，应用程序路径，类名称
            MenuItem subMenu = topMenu.Append(basicM, "Hello Revit", "C:\\REVIT\\CH02\\HelloRevit\\bin\\Release\\HelloRevit.dll", "RevitDevelop.HelloRevit");
            //设置该菜单的提示信息，当鼠标移到该菜单上时将在状态条上显示
            subMenu.StatusbarTip = "Shows a message box with Hello Revit!";

            return IExternalApplication.Result.Succeeded;
        }
        //即使该函数不做任何事，也需要定义
        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {
            return IExternalApplication.Result.Succeeded;
        }
    }
}
