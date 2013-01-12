using System;
using System.Collections.Generic;
using System.Text;
using Inventor;

namespace Chapter9
{
    class myTranslatorAddIn
    {
        public void PublishDWF(Inventor.Application ThisApplication)
        {
            //获得DWF translator
            TranslatorAddIn DWFAddIn;
            DWFAddIn = (TranslatorAddIn)ThisApplication.ApplicationAddIns.get_ItemById("{0AC6FD95-2F4D-42CE-8BE0-8AEA580399E4}");

            //设置到激活文档（要发布为DWF的文档）的引用
            Document doc;
            doc = ThisApplication.ActiveDocument;

            TranslationContext context;
            context = ThisApplication.TransientObjects.CreateTranslationContext();
            context.Type = IOMechanismEnum.kFileBrowseIOMechanism;

            //创建新的NameValueMap对象
            Inventor.NameValueMap options;
            options = ThisApplication.TransientObjects.CreateNameValueMap();

            //创建新的DataMedium对象
            DataMedium dataMedium;
            dataMedium = ThisApplication.TransientObjects.CreateDataMedium();

            //检查translator是否包含‘SaveCopyAs’选项
            if (DWFAddIn.get_HasSaveCopyAsOptions(dataMedium, context, options))
            {
                options.set_Value("Launch_Viewer", 1);
                //其他选项为：
                //options.set_Value("Publish_All_Component_Props", 1);
                //options.set_Value("Publish_All_Physical_Props", 1);
                //options.set_Value("Password", 0);

                if (doc.DocumentType == DocumentTypeEnum.kDrawingDocumentObject)
                {
                    //工程图选项
                    options.set_Value("Publish_Mode", DWFPublishModeEnum.kCustomDWFPublish);
                    options.set_Value("Publish_All_Sheets", 0);

                    //如果"Publish_All_Sheets"为true(1)，则会忽略指定的页面
                    Inventor.NameValueMap sheets;
                    sheets = ThisApplication.TransientObjects.CreateNameValueMap();

                    //发布第一个页面及其三维模型
                    Inventor.NameValueMap sheet1Options;
                    sheet1Options = ThisApplication.TransientObjects.CreateNameValueMap();

                    sheet1Options.Add("Name", "图纸:1");
                    sheet1Options.Add("3DModel", true);

                    sheets.set_Value("Sheet1", sheet1Options);

                    //发布第三个页面但不发布其三维模型
                    Inventor.NameValueMap sheet3Options;
                    sheet3Options = ThisApplication.TransientObjects.CreateNameValueMap();

                    sheet3Options.Add("Name", "图纸3:3");
                    sheet3Options.Add("3DModel", false);

                    sheets.set_Value("Sheet2", sheet3Options);

                    //设置页面选项
                    options.set_Value("Sheets", sheets);
                }
            }
            //设置DWF文件名
            dataMedium.FileName = "c:\\temp\\test.dwf";

            //发布文档
            DWFAddIn.SaveCopyAs(doc, context, options, dataMedium);
        }
    }
}
