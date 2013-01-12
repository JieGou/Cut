using System;
using Autodesk.Revit;

namespace RevitDevelop
{
    public class MyToolbar : IExternalApplication
    {
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            Toolbar newToolbar = application.CreateToolbar();
            if (newToolbar == null)
                return IExternalApplication.Result.Failed;
            newToolbar.Name = "My Toolbar";
            newToolbar.Image = "C:\\REVIT\\CH02\\MyToolbar\\Toolbar_Image.bmp";//工具条图像文件
            //参数依次为：应用程序文件路径，类名称
            ToolbarItem item = newToolbar.AddItem("C:\\REVIT\\CH02\\HelloRevit\\bin\\Release\\HelloRevit.dll", "RevitDevelop.HelloRevit");
            //该按钮的状态条提示和ToolTip提示
            item.StatusbarTip = item.ToolTip = "Shows a message box with Hello Revit!";

            return IExternalApplication.Result.Succeeded;
        }

        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {
            return IExternalApplication.Result.Succeeded;
        }
    }
}
