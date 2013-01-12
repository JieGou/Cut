using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Gis.Map.Filters;

namespace CH05
{

    public  class CmdFilter
    {
        //--------------------------------------------------------------------------
        //
        // 功能：运行基本过滤
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("BasicFilter")]
        public void BasicFilter()
        {
            BasicFilter bfilter = new BasicFilter();
            // 设定层过滤条件
            StringCollection layerCollection = new StringCollection();
             //layerCollection.Add("*");  // 过滤所有的层
            layerCollection.Add("1");    //  过滤层 1
            bfilter.SetLayers(layerCollection);

            //  设定块过滤条件
            StringCollection blkCollection = new StringCollection();
            //如果过滤所有的块，设置块名集合包含 "*"
            // 如果忽略块过滤，设置块名集合包含 ""  
            blkCollection.Add("");
            bfilter.SetBlocks(blkCollection);
            //设定要素过滤条件
            StringCollection blkClassification = new StringCollection();
            //blkClassification.Add("pipe"); // 过滤pipe要素
            blkClassification.Add("") ;// 忽略要素过滤
            bfilter.SetFeatureClasses(blkClassification);

            ObjectIdCollection selectedIDCollection = new ObjectIdCollection();
            selectedIDCollection = GetAllEntity();

            ObjectIdCollection outIDCollection = new ObjectIdCollection() ;
            bfilter.FilterObjects(ref outIDCollection, selectedIDCollection);

            // 处理过滤得到的对象
            HighlightEntity(outIDCollection);

        }
        //--------------------------------------------------------------------------
        //
        // 功能：运行自定义过滤
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("CustFilter")]
        public void CustFilter()
        {
            try
            {
                // 创建自定义过滤对象，并设定过滤条件
                MyPolylineFilter cfilter = new MyPolylineFilter();
                cfilter.MinLen = 500.0;
                cfilter.MaxLen = 2000.0;
                // 获取要过滤的对象
                ObjectIdCollection selectedIDCollection = new ObjectIdCollection();
                selectedIDCollection = GetAllEntity();
                // 执行过滤
                ObjectIdCollection outIDCollection = new ObjectIdCollection();
                cfilter.FilterObjects(outIDCollection, selectedIDCollection);

                // 处理过滤得到的对象
                HighlightEntity(outIDCollection);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                Utility.AcadEditor.WriteMessage(e.Message);
            }
        }


       
        //--------------------------------------------------------------------------
        //
        // 功能：获取AutoCAD数据库中所有实体
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        private ObjectIdCollection GetAllEntity()
        {
            ObjectIdCollection returnValue;
            Autodesk.AutoCAD.DatabaseServices.Database curDb;
            curDb = Autodesk.AutoCAD.DatabaseServices.HostApplicationServices.WorkingDatabase;

            Transaction trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction();
            ObjectIdCollection IDCol = new ObjectIdCollection();
            IDCol.Clear();
            BlockTable blockTable;
            BlockTableRecord blkTblRecord;
            Entity entity;
            try
            {
                blockTable = trans.GetObject(curDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                blkTblRecord = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord ;

                ObjectId objId;
                foreach (ObjectId tempLoopVar_objId in blkTblRecord)
                {
                    objId = tempLoopVar_objId;
                    entity = trans.GetObject(objId, OpenMode.ForRead) as Entity ;
                    IDCol.Add(entity.ObjectId);
                }
                trans.Commit();
            }
            catch (System.Exception e )
            {
                Utility.AcadEditor.WriteMessage(e.Message);
            }
            finally
            {
                trans.Dispose();
            }
            returnValue = IDCol;

            return returnValue;
        }

        //--------------------------------------------------------------------------
        //
        // 功能：高亮显示选中的实体
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        private void HighlightEntity(Autodesk.AutoCAD.DatabaseServices.ObjectIdCollection entityIdCollection)
        {
            Transaction trans = null;
            try
            {
                trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction();
                ObjectId id;
                foreach (ObjectId tempLoopVar_id in entityIdCollection)
                {
                    id = tempLoopVar_id;
                    Entity entity;
                    try
                    {
                        entity = trans.GetObject(id, OpenMode.ForRead)as Entity;
                        entity.UpgradeOpen();
                        Type blockType = entity.GetType();

                        if (entity is BlockReference)
                        {
                            BlockReference blkRef = entity as BlockReference ;
                            BlockTableRecord blkTblRecord = trans.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord ;
                            ObjectId objId;
                            foreach (ObjectId tempLoopVar_objId in blkTblRecord)
                            {
                                objId = tempLoopVar_objId;
                                Entity blkEntity = trans.GetObject(objId, OpenMode.ForWrite)as Entity ;
                                blkEntity.ColorIndex = 1;
                            }
                        }
                        entity.ColorIndex = 1;
                    }
                    catch (Autodesk.Gis.Map.MapException e )
                    {
                        Utility.AcadEditor.WriteMessage(e.Message);
                    }
                }
                trans.Commit();
            }
            catch (System.Exception e )
            {
                Utility.AcadEditor.WriteMessage(e.Message);
            }
            finally
            {
                trans.Dispose();
            }
        }



    }

    //--------------------------------------------------------------------------
    //
    // 功能：功能函数类，用于获取Editor
    //
    //  作者： 
    //
    //  日期：200708
    //
    //   历史：
    //--------------------------------------------------------------------------
    public class Utility
    {
        private Utility()
        {
        }

        public static Autodesk.AutoCAD.EditorInput.Editor AcadEditor
        {
            get
            {
                return Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            }
        }
    }
}
