using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace CH03
{
    public class Class1
    {

        //--------------------------------------------------------------
        // 功能:创建一个新层
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //----------------------------------------------------------------

        [CommandMethod("CreateLayer")]
        public void CreateLayer()
        {
            ObjectId layerId;
            Database db = HostApplicationServices.WorkingDatabase;
            //开始一个事务
            Transaction trans = db.TransactionManager.StartTransaction();
            try
            {
                //首先取得层表
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);
                //检查MyLayer层是否存在
                if (lt.Has("MyLayer"))
                {
                    layerId = lt["MyLayer"];
                }
                else
                {
                    //如果MyLayer层不存在，就创建它
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Name = "MyLayer"; //设置层的名字
                    layerId = lt.Add(ltr);
                    trans.AddNewlyCreatedDBObject(ltr, true);
                }
                //提交事务
                trans.Commit();
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                //放弃事务
                trans.Abort();
            }
            finally
            {
                // 显式地释放
                trans.Dispose();
            }
        }

        //--------------------------------------------------------------
        // 功能:创建一个圆
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //----------------------------------------------------------------
        [CommandMethod("CreateCircle")]
        public void  CreateCircle()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            // 使用 "using" ，结束是自动调用事务的 "Dispose" 
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //获取块表和模型空间
                BlockTable bt = (BlockTable)(trans.GetObject(db.BlockTableId, OpenMode.ForRead));
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                //创建一个圆并添加到块表记录（模型空间）
                Point3d center = new Point3d(10, 10, 0);
                Circle circle = new Circle(center, Vector3d.ZAxis, 10.0);
                circle.ColorIndex = 1;

                btr.AppendEntity(circle);
                trans.AddNewlyCreatedDBObject(circle, true);
                trans.Commit();
            }

        }

        //--------------------------------------------------------------
        // 功能:创建一个块定义（块表记录）
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //   
        //----------------------------------------------------------------
        public ObjectId CreateBlkDef()
        {
            //定义函数的返回值ObjectId
            ObjectId blkObjId = new ObjectId(); 
            Database db = HostApplicationServices.WorkingDatabase; 

            // 使用 "using"关键字指定事务的边界
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //获取块表
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForWrite);
                //通过块名myBlkName判断块表中是否包含块表记录
                if ((bt.Has("myBlkName")))
                {
                    blkObjId = bt["myBlkName"];//如果已经存在，通过块名获取块对应的ObjectId
                }
                else
                {
                    //创建一个圆
                    Point3d center = new Point3d(10, 10, 0); 
                    Circle circle = new Circle(center, Vector3d.ZAxis, 2);
                    circle.ColorIndex = 1;      
                    //创建文本Text:
                    MText text = new MText();
                    text.Contents = " ";
                    text.Location = center;
                    text.ColorIndex = 2;

                    //创建新的块表记录 myBlkName
                    BlockTableRecord newBtr = new BlockTableRecord();
                    newBtr.Name = "myBlkName";
                    newBtr.Origin = center;
                    //保存块表记录到块表
                    blkObjId = bt.Add(newBtr); // 返回块对应的ObjectId
                    trans.AddNewlyCreatedDBObject(newBtr, true); //Let the transaction know about any object/entity you add to the database!
                   
                    //保存新创建的实体到块表记录
                    newBtr.AppendEntity(circle); 
                    newBtr.AppendEntity(text);
                    // 通知事务新创建了对象
                    trans.AddNewlyCreatedDBObject(circle, true);
                    trans.AddNewlyCreatedDBObject(text, true);
                }
                trans.Commit(); //提交事务
            }
            return blkObjId;
        }


        //--------------------------------------------------------------
        // 功能:创建一个新层
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //----------------------------------------------------------------
        [CommandMethod("CreateBlk")]
        public void CreateBlkRef()
        {
            
             
            //获取块的插入点
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptPointOptions ptOps = new PromptPointOptions("选择块的插入点");
            PromptPointResult ptRes;
            ptRes = ed.GetPoint(ptOps);
            Point3d ptInsert;
            if (ptRes.Status == PromptStatus.OK)
            {
                ptInsert = ptRes.Value ;
            }
            else
            {
                ptInsert = new Point3d(0, 0, 0);
            }

            Database db = HostApplicationServices.WorkingDatabase;
            // 使用 "using"关键字指定事务的边界
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //获取块表和模型空间
                BlockTable bt = (BlockTable)(trans.GetObject(db.BlockTableId, OpenMode.ForWrite));
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                
                //创建块引用
                BlockReference blkRef = new BlockReference(ptInsert,CreateBlkDef());// 指定插入点和所引用的块表记录
                blkRef.Rotation = 1.57;//指定旋转角，按弧度

                //保存新创建的块引用到模型空间    
                btr.AppendEntity(blkRef); 
                trans.AddNewlyCreatedDBObject(blkRef, true);    // 通知事务新创建了对象

                trans.Commit(); //提交事务
            }
  
        }

        //--------------------------------------------------------------
        // 功能:创建一个新层
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //----------------------------------------------------------------
        [CommandMethod("OpenEnt")]
        public void OpenEnt()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
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
                Entity ent = trans.GetObject(objId, OpenMode.ForWrite);
                ent.ColorIndex = 1;
                trans.Commit();
            }

        }
        ///////////////////////////////////////////////////
    }
}
