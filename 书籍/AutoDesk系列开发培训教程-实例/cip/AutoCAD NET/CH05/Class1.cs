using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace CH05
{
    public class Class1
    {

         //--------------------------------------------------------------
        // 功能:添加扩展数据XDATA
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //----------------------------------------------------------------
        [CommandMethod("AddXData")]
        public void AddXData()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("添加扩充数据XDATA\n");
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

                RegAppTable appTbl = trans.GetObject(db.RegAppTableId, OpenMode.ForWrite) as RegAppTable ;
                if (!appTbl.Has("MyAppName"))
                {
                    RegAppTableRecord appTblRcd = new RegAppTableRecord();
                    appTblRcd.Name = "MyAppName";
                    appTbl.Add(appTblRcd);
                    trans.AddNewlyCreatedDBObject(appTblRcd, true);
                }
                ResultBuffer resBuf = new ResultBuffer();//new TypedValue(1001, "MyAppName"), new TypedValue(1000, "开发部门"));

                resBuf.Add(new TypedValue(1001, "MyAppName"));//注册程序名称
                resBuf.Add(new TypedValue(1000 , " 张三"));//姓名
                resBuf.Add(new TypedValue(1000 , " 工程部"));//部门
                resBuf.Add(new TypedValue(1040, 2000.0));//薪水
                ent.XData =  resBuf;
                trans.Commit();
            }
 
        }


        //--------------------------------------------------------------
        // 功能:获取扩展数据XDATA
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //------------------------------------------------------------
        [CommandMethod("GETXDATA")]
        public void GETXDATA()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("获取扩充数据XDATA\n");

            PromptEntityOptions entOps = new PromptEntityOptions("选择带扩展数据的对象");
            PromptEntityResult entRes = ed.GetEntity(entOps);
            if (entRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("选择对象失败，退出");
                return;
            }
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {

                Entity ent = (Entity)trans.GetObject(entRes.ObjectId, OpenMode.ForRead);
                ResultBuffer resBuf = ent.XData;
                if (resBuf != null)
                {
                    //
                    IEnumerator iter = resBuf.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        TypedValue tmpVal = (TypedValue)iter.Current;
                        ed.WriteMessage(tmpVal.TypeCode.ToString() + ":");
                        ed.WriteMessage(tmpVal.Value.ToString() + "\n");
                    }
                }
            }
        }

        //--------------------------------------------------------------
        // 功能:在命名对象词典中添加数据
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //------------------------------------------------------------
        [CommandMethod("AddInNOD")]
        public void AddInNOD()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("在命名对象词典中添加数据\n");
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //获取命名对象词典（NOD)
                DBDictionary NOD =trans.GetObject(db.NamedObjectsDictionaryId, OpenMode.ForWrite) as DBDictionary ;
                // 声明一个新的词典
                DBDictionary copyrightDict;
                // 判断是否存在COPYRIGHT词典，没有则创建
                try
                {
                    // 获取COPYRIGHT词典
                    copyrightDict = (DBDictionary)trans.GetObject(NOD.GetAt("COPYRIGHT"), OpenMode.ForRead);
                }
                catch
                {
                    //在NOD下创建COPYRIGHT词典
                    copyrightDict = new DBDictionary();
                    NOD.SetAt("COPYRIGHT", copyrightDict);
                    trans.AddNewlyCreatedDBObject(copyrightDict, true);
                }

                // 在copyrightDict中，获取或创建 "author" 词典
                DBDictionary authorDict;
                try
                {
                    authorDict = (DBDictionary)trans.GetObject(copyrightDict.GetAt("Author"), OpenMode.ForWrite);
                }
                catch
                {
                    authorDict = new DBDictionary();
                    //"author" doesn't exist, create one
                    copyrightDict.UpgradeOpen();
                    copyrightDict.SetAt("Author", authorDict);
                    trans.AddNewlyCreatedDBObject(authorDict, true);
                }

                // 通过Xrecord和ResultBuffer添加扩展数据
                Xrecord authorRec;
                try
                {
                    authorRec = (Xrecord)trans.GetObject(authorDict.GetAt("AuthorInfo"), OpenMode.ForWrite);
                }
                catch
                {
                    authorRec = new Xrecord();
                    authorRec.Data = new ResultBuffer(new TypedValue((int)DxfCode.Text, "张三"));
                    authorDict.SetAt("AuthorInfo", authorRec);
                    trans.AddNewlyCreatedDBObject(authorRec, true);
                }
                trans.Commit();
            }
        }

        //--------------------------------------------------------------
        // 功能:获取命名对象词典中的数据
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //------------------------------------------------------------
        [CommandMethod("GetInNOD")]
        public void GetInNod()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("获取命名对象词典中数据\n");

            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // 获取NOD 
                DBDictionary NOD = (DBDictionary)trans.GetObject(db.NamedObjectsDictionaryId, OpenMode.ForRead, false);
                // 获取COPYRIGHT词典
                DBDictionary copyrightDict = (DBDictionary)trans.GetObject(NOD.GetAt("COPYRIGHT"), OpenMode.ForRead);
                // 获取Author词典
                DBDictionary AuthorDict = (DBDictionary)trans.GetObject(copyrightDict.GetAt("Author"), OpenMode.ForRead);
                // 获取AuthorInfo扩展记录Xrecord
                Xrecord authorXRec = (Xrecord)trans.GetObject(AuthorDict.GetAt("AuthorInfo"), OpenMode.ForRead);
                ResultBuffer resBuf = authorXRec.Data;
                TypedValue val = resBuf.AsArray()[0];
                ed.WriteMessage("该图纸由{0}设计\n", val.Value);
            }
        }

        //--------------------------------------------------------------
        // 功能:添加数据到数据库对象的扩展词典中
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //------------------------------------------------------------
        [CommandMethod("AddExtDict")]
        public void AddExtDict()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("创建对象扩展词典\n");

            PromptEntityOptions entOps = new PromptEntityOptions("选择要添加扩展数据的块\n");
            PromptEntityResult entRes = ed.GetEntity(entOps);
            if (entRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("选择对象失败，退出");
                return;
            }
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                DBObject obj = trans.GetObject(entRes.ObjectId, OpenMode.ForWrite) as DBObject;
                BlockReference blkRef;
                if (obj is BlockReference)
                {
                    blkRef = obj as BlockReference;
                }
                else
                {
                    return;
                }

                // 创建对象的扩展词典
                blkRef.CreateExtensionDictionary();
                DBDictionary extensionDict = (DBDictionary)trans.GetObject(blkRef.ExtensionDictionary, OpenMode.ForWrite, false);
               
                // 通过Xrecord准备附加属性数据
                Xrecord xRec = new Xrecord();
                xRec.Data = new ResultBuffer(
                  new TypedValue((int)DxfCode.Text, "张三"),// 姓名
                  new TypedValue((int)DxfCode.Real, 1200.0),//薪水
                  new TypedValue((int)DxfCode.Text, "技术部"));// 部门         
               // 在扩展词典中添加扩展记录
                extensionDict.SetAt("EmployeeInfomation", xRec); 
                trans.AddNewlyCreatedDBObject(xRec, true);

                trans.Commit();
            }

        }


        //--------------------------------------------------------------
        // 功能:获取数据库对象的扩展词典中的数据
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //
        //------------------------------------------------------------
        [CommandMethod("GetExtDict")]
        public void GetExtDict()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("获取对象扩展词典信息\n");

            PromptEntityOptions entOps = new PromptEntityOptions("选择添加了扩展数据的块\n");
            PromptEntityResult entRes = ed.GetEntity(entOps);
            if (entRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("选择对象失败，退出");
                return;
            }
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                DBObject obj = trans.GetObject(entRes.ObjectId, OpenMode.ForWrite) as DBObject;
                BlockReference blkRef;
                if (obj is BlockReference)
                {
                    blkRef = obj as BlockReference;
                }
                else
                {
                    ed.WriteMessage("选择对象不是块，退出\n");
                    return;
                }

                // 创建对象的扩展词典
                DBDictionary extensionDict = (DBDictionary)trans.GetObject(blkRef.ExtensionDictionary, OpenMode.ForWrite, false);
                // 获取AuthorInfo扩展记录Xrecord
                Xrecord EmpXRec = (Xrecord)trans.GetObject(extensionDict.GetAt("EmployeeInfomation"), OpenMode.ForRead);
                ResultBuffer resBuf = EmpXRec.Data;
                TypedValue val = resBuf.AsArray()[0];
                ed.WriteMessage("是员工姓名:{0}\n", val.Value);
                val = resBuf.AsArray()[1];
                ed.WriteMessage("该员工的薪水:{0}\n", val.Value);
                val = resBuf.AsArray()[2];
                ed.WriteMessage("该员工属于:{0}\n", val.Value);
 
                trans.Commit();
            }

        }
        
        ////


    }
}
