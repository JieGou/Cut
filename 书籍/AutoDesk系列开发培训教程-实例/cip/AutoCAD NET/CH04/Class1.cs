using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
namespace CH04
{
    public class Class1
    {


        //--------------------------------------------------------------
        // 功能:通过ObjectId打开对象
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //----------------------------------------------------------------
        [CommandMethod("OpenEnt")]
        public void OpenEnt()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("通过ObjectId打开对象\n");
            PromptEntityOptions entOps = new PromptEntityOptions("选择要打开的对象\n");
            PromptEntityResult entRes;
            entRes = ed.GetEntity(entOps);
            if (entRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("选择对象失败，退出");
                return;
            }
            ObjectId objId = entRes.ObjectId;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity ent = trans.GetObject(objId, OpenMode.ForWrite) as Entity ;
                ent.ColorIndex = 1;
                trans.Commit();
            }
 
        }

        //--------------------------------------------------------------
        // 功能:类型识别和转换
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //----------------------------------------------------------------
        [CommandMethod("GetType")]
        public void GetType()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("数据库对象的类型识别和转换\n");

            PromptEntityOptions entOps = new PromptEntityOptions("选择要打开的对象");
            PromptEntityResult entRes;
            entRes = ed.GetEntity(entOps);
            if (entRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("选择对象失败，退出");
                return;
            }
            ObjectId objId = entRes.ObjectId;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity ent = trans.GetObject(objId, OpenMode.ForWrite) as Entity;
                ed.WriteMessage("ent.GetRXClass().Name :" + ent.GetRXClass().Name + "\n");

                if (ent is Line)
                {
                    Line aLine = ent as Line;
                    aLine.ColorIndex = 1;
                }
                else if (ent.GetType() == typeof(Circle))
                {
                    Circle cir = (Circle)ent;
                    cir.ColorIndex = 2;
                }

                trans.Commit();
            }
        }
        //--------------------------------------------------------------
        // 功能:实体对象的属性
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //----------------------------------------------------------------
        [CommandMethod("EntPro")]
        public void EntPro()
        {
            
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("实体对象的属性\n");

            PromptEntityOptions entOps = new PromptEntityOptions("选择要打开的对象\n");
            PromptEntityResult entRes;
            entRes = ed.GetEntity(entOps);
            if (entRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("选择对象失败，退出\n");
                return;
            }
            ObjectId objId = entRes.ObjectId;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity ent = trans.GetObject(objId, OpenMode.ForWrite) as Entity;
                ed.WriteMessage("获取或设置实体的线型\n");
                ed.WriteMessage("实体的原先的线型为 :" + ent.Linetype + "\n");
                // 获取线型表记录
                LinetypeTable lineTypeTbl = trans.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                // 确保DOT线型名已经加载到当前数据库
                LinetypeTableRecord lineTypeTblRec = trans.GetObject(lineTypeTbl["DOT"], OpenMode.ForRead) as LinetypeTableRecord;
                // 设置实体的线型
                ent.LinetypeId = lineTypeTblRec.ObjectId;

                // 设置实体的线型比例
                ed.WriteMessage("设置实体的线型比例为2.0\n");
                ent.LinetypeScale = 2.0;

                //设置实体的可见性
                ent.Visible = true;

                 //设置实体所在的层
                ed.WriteMessage("实体的原先所在的层为 :" + ent.Layer + "\n");
                ent.Layer = "layer0";
                trans.Commit();
            }
        }



        ///////////////////////////
    }
}
