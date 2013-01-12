using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections; // For ArrayList

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using Autodesk.AutoCAD.Windows;

[assembly: ExtensionApplication(typeof(CH06.Class1))]
//[assembly: CommandClass(typeof(CH06.Class1))]

namespace CH06
{
    public class Class1:Autodesk.AutoCAD.Runtime.IExtensionApplication 
    {
        public static string sAuthorDefault = "张三";
        public static string sCompanyDefault = "公司名称";

        public void Initialize()
        {
            AddContextMenu();
            MyOptionPage.AddTabDialog();
        }
        public void Terminate()
        {
            RemoveContextMenu();
            MyOptionPage.RemoveTabDialog();
        }

        ContextMenuExtension m_ContextMenu;


        private void MyMenuItem_OnClick(object Sender, EventArgs e)
        {
            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
               //创建一个红色的圆
                Database db = HostApplicationServices.WorkingDatabase;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable ;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    Circle cir = new Circle(new Point3d(10, 10, 0), Vector3d.ZAxis, 100);
                    cir.ColorIndex  = 1;
                    btr.AppendEntity (cir);
                    trans.AddNewlyCreatedDBObject (cir,true);
                    trans.Commit ();
                }
            }
        }

        private void AddContextMenu()
        {
            //ContextMenuExtension m_ContextMenu;//声明为类的成员变量
            m_ContextMenu = new ContextMenuExtension();
            m_ContextMenu.Title = "我的自定义菜单";
            Autodesk.AutoCAD.Windows.MenuItem mi;
            mi = new Autodesk.AutoCAD.Windows.MenuItem("创建圆");
            mi.Click += MyMenuItem_OnClick;
            m_ContextMenu.MenuItems.Add(mi);

            Autodesk.AutoCAD.ApplicationServices.Application.AddDefaultContextMenuExtension(m_ContextMenu);
            //添加对象级别的右键菜单(说明：和上面的语句不同）
            //Autodesk.AutoCAD.ApplicationServices.Application.AddObjectContextMenuExtension(Circle.GetClass(typeof(Circle)), m_ContextMenu);
        }

        private void RemoveContextMenu()
        {
            if (m_ContextMenu != null)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.RemoveDefaultContextMenuExtension(m_ContextMenu);
                m_ContextMenu = null;
            }
        }


        ////
    }
}
