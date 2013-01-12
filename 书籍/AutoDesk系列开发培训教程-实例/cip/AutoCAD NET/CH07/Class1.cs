using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using Autodesk.AutoCAD.Windows;

namespace CH07
{
    public class Class1
    {
        //全局变量
        bool bCmdActive;
        bool bReposition;
        ObjectIdCollection blkObjIDs = new ObjectIdCollection();
        Point3dCollection blkPositions = new Point3dCollection();

        [CommandMethod("AddEvents")]
        public void AddEvents()
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Database db;
                Document doc;
                //
                doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                db = HostApplicationServices.WorkingDatabase;
                db.ObjectOpenedForModify += new ObjectEventHandler(objOpenedForMod);
                doc.CommandWillStart += new CommandEventHandler(cmdWillStart);
                doc.CommandEnded += new CommandEventHandler(cmdEnded);
                bCmdActive = false;
                bReposition = false;
            }
            catch
            {
                ed.WriteMessage("添加事件错误\n");
            }
        }

        [CommandMethod("RemoveEvents")]
        public void removeDbEvents()
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Database db;
                Document doc;
                doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                db = HostApplicationServices.WorkingDatabase;
                db.ObjectOpenedForModify -= new ObjectEventHandler(objOpenedForMod);
                doc.CommandEnded -= new CommandEventHandler(cmdEnded);
                doc.CommandWillStart -= new CommandEventHandler(cmdWillStart);
                bCmdActive = false;
                bReposition = false;
            }
            catch
            {
                ed.WriteMessage("移除事件错误\n");
            }
        }

        public void objOpenedForMod(object o, ObjectEventArgs e)
        {
            if (bCmdActive == false || bReposition == true)
                return;
            ObjectId objId = e.DBObject.ObjectId;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    Entity ent = (Entity)trans.GetObject(objId, OpenMode.ForRead, false);
                    //判断对象是否为块
                    if (ent is BlockReference)
                    { 
                        BlockReference br = (BlockReference)ent;
                        //获取ObjectID和插入点，并把它们保存到全局变量
                                    blkObjIDs.Add(objId);
                                    blkPositions.Add(br.Position);
                    }
                    trans.Commit();
                }
            }
            catch
            {
                ed.WriteMessage("ObjectOpenedForModify事件的处理函数错误");
            }
        }

        public void cmdWillStart(object o, CommandEventArgs e)
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                if (e.GlobalCommandName == "MOVE")
                {
                    //设置全局变量
                    bCmdActive = true;
                    bReposition = false;
                    //清理所有的保存信息
                    blkObjIDs.Clear();
                    blkPositions.Clear();
                }
            }
            catch
            {
                ed.WriteMessage("CommandWillStart事件的处理函数错误");
            }
        }

        public void cmdEnded(object o, CommandEventArgs e)
        {
            //判断监控命令是否是激活的
            if (bCmdActive == false)
                return;
            bCmdActive = false;

            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                bReposition = true;
                Database db = HostApplicationServices.WorkingDatabase;
                for (int i = 0; i < blkObjIDs.Count; i++)
                {
                    Point3d oldpos;
                    Point3d newpos;
                    using (Transaction trans = db.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                        Entity ent = (Entity)trans.GetObject(blkObjIDs[i], OpenMode.ForWrite);
                        //实体类型判断
                        if (ent is BlockReference )
                        { 
                            BlockReference br = (BlockReference)ent;
                            newpos = br.Position;
                            oldpos = blkPositions[i];

                            //重置为初始的位置
                            if (!oldpos.Equals(newpos))
                                br.Position = oldpos;
                        }
                        trans.Commit();
                    }
                }
            }
            catch
            {
                ed.WriteMessage("CommandEnded事件处理函数错误");
            }
        }
    } 
}
