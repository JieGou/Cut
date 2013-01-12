using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace RevitDevelop
{
    public class ImportDWGFile : IExternalCommand
    {
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document curDoc = commandData.Application.ActiveDocument;
            //创建一个默认的导入选项
            DWGImportOptions dwgOpt = new DWGImportOptions();
            Element elem = new Element();
            //导入指定目录下的一个dwg文件，导入前确保该dwg文件已存在
            curDoc.Import("c:\\a.dwg", dwgOpt, ref elem);

            return IExternalCommand.Result.Succeeded;
        }
    }
}
