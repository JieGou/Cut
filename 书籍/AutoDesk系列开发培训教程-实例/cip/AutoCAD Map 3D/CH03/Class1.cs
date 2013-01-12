//
// (C) Copyright 2004-2007 by Autodesk, Inc.
//
//
//
// By using this code, you are agreeing to the terms
// and conditions of the License Agreement that appeared
// and was accepted upon download or installation
// (or in connection with the download or installation)
// of the Autodesk software in which this code is included.
// All permissions on use of this code are as set forth
// in such License Agreement provided that the above copyright
// notice appears in all authorized copies and that both that
// copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Text;
using System.Collections;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

using Autodesk.Gis.Map;
using Autodesk.Gis.Map.ObjectData;
using Autodesk.Gis.Map.Constants;
using Autodesk.Gis.Map.Utilities;

namespace CH03
{
    public class Class1
    {
        public void Initialize()
        {
            AcadEditor.WriteMessage("\n对象数据示例初始化. ");
        }

        public void Terminate()
        {
        }

        #region "Utility Function"
        //--------------------------------------------------------------------------
        //
        // 功能：删除对象数据表
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        public bool RemoveTable(Tables tables, string tableName)
        {
            try
            {
                tables.RemoveTable(tableName);
                return true;
            }
            catch
            {
                return false;
            }
        }


        //--------------------------------------------------------------------------
        //
        // 功能：创建对象数据表
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        public bool CreateTable(Tables tables, string tableName)
        {
            ErrorCode errODCode = ErrorCode.OK;
            Autodesk.Gis.Map.ObjectData.Table table = null;

            try
            {
                table = tables[tableName];
            }
            catch (MapException e)
            {
                errODCode = (ErrorCode)(e.ErrorCode);
            }

            if (ErrorCode.ErrorObjectNotFound == errODCode)
            {
                try
                {
                    MapApplication app = HostMapApplicationServices.Application;

                    //   定义一个表名称为"MyODTable" 包含下面四个字段定义:
                    // 
                    //    字段名称        数值类型
                    //    FIRST_FIELD                字符串
                    //   SECOND_FIELD           整型
                    //   THIRD_FIELD                 浮点型
                    //    LAST_FIELD                  点
                    FieldDefinitions tabDefs = app.ActiveProject.MapUtility.NewODFieldDefinitions();

                    FieldDefinition def1 = FieldDefinition.Create("FIRST_FIELD", "String Type", "A");
                    tabDefs.AddColumn(def1, 0);

                    FieldDefinition def2 = FieldDefinition.Create("SECOND_FIELD", "Int Type", 0);
                    tabDefs.AddColumn(def2, 1);

                    FieldDefinition def3 = FieldDefinition.Create("THIRD_FIELD", "Real Type", 0.0);
                    tabDefs.AddColumn(def3, 2);

                    FieldDefinition def4 = FieldDefinition.Create("LAST_FIELD", "Point Type", new Point3d(0, 0, 0));
                    tabDefs.AddColumn(def4, 3);

                    tables.Add(tableName, tabDefs, "Desc", true);

                    return true;
                }
                catch (MapException e)
                {
                    AcadEditor.WriteMessage(e.Message);
                    return false;
                }
            }

            return false;
        }

        private long m_index = 1;

        //--------------------------------------------------------------------------
        //
        // 功能：添加对象数据
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        public bool AddODRecord(Tables tables, string tableName, ObjectId id)
        {
            try
            {
                Autodesk.Gis.Map.ObjectData.Table table = tables[tableName];

                // Create and initialize an record 
                Record tblRcd = Record.Create();
                table.InitRecord(tblRcd);

                string msg = ""; // Output string

                MapValue val = tblRcd[0]; // String type
                string temp = null;
                temp = string.Format("String_{0}", m_index);
                val.Assign(temp);
                msg += temp + "; ";

                val = tblRcd[1]; // integer
                val.Assign(m_index);
                msg += m_index.ToString() + "; ";

                val = tblRcd[2]; // real
                val.Assign(3.14159 * m_index);
                msg += (3.14159 * m_index).ToString() + "; ";

                val = tblRcd[3]; // point
                Point3d pt = new Point3d(10 * m_index, 20 * m_index, 30 * m_index);
                val.Assign(pt);
                msg += pt.ToString();

                m_index++;

                table.AddRecord(tblRcd, id);

                AcadEditor.WriteMessage("\n 记录为： : [" + msg + "] ");

                return true;
            }
            catch (MapException)
            {
                return false;
            }
        }

        //--------------------------------------------------------------------------
        //
        // 功能：输出对象附着的对象数据
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        public bool ShowObjectDataInfo(Tables tables, ObjectId id)
        {
            ErrorCode errCode = ErrorCode.OK;
            try
            {
                bool success = true;

                // Get and Initialize Records
                using (Records records
                           = tables.GetObjectRecords(0, id, Autodesk.Gis.Map.Constants.OpenMode.OpenForRead, false))
                {
                    if (records.Count == 0)
                    {
                        AcadEditor.WriteMessage("\n 实体没有附着对象数据。");
                        return true;
                    }

                    int index = 0;

                    // Iterate through all records
                    foreach (Record record in records)
                    {
                        String msg = null;
                        msg = String.Format("\n记录 {0} : ", index);
                        index++;
                        AcadEditor.WriteMessage(msg);

                        // 获取表
                        Autodesk.Gis.Map.ObjectData.Table table = tables[record.TableName];

                        int valInt = 0;
                        double valDouble = 0.0;
                        string str = null;

                        // 获取记录信息
                        for (int i = 0; i < record.Count; i++)
                        {
                            FieldDefinitions tableDef = table.FieldDefinitions;
                            FieldDefinition column = null;
                            column = tableDef[i];
                            string colName = column.Name;
                            MapValue val = record[i];

                            switch (val.Type)
                            {
                                case Autodesk.Gis.Map.Constants.DataType.Integer:
                                    valInt = val.Int32Value;
                                    msg = string.Format("{0}; ", valInt);
                                    AcadEditor.WriteMessage(msg);
                                    break;

                                case Autodesk.Gis.Map.Constants.DataType.Real:
                                    valDouble = val.DoubleValue;
                                    msg = string.Format("{0}; ", valDouble);
                                    AcadEditor.WriteMessage(msg);
                                    break;

                                case Autodesk.Gis.Map.Constants.DataType.Character:
                                    str = val.StrValue;
                                    msg = string.Format("{0}; ", str);
                                    AcadEditor.WriteMessage(msg);
                                    break;

                                case Autodesk.Gis.Map.Constants.DataType.Point:
                                    {
                                        Point3d pt = val.Point;
                                        double x = pt.X;
                                        double y = pt.Y;
                                        double z = pt.Z;
                                        msg = string.Format("点({0},{1},{2}); ", x, y, z);
                                        AcadEditor.WriteMessage(msg);
                                    }
                                    break;

                                default:
                                    AcadEditor.WriteMessage("\n错误的数据类型\n");
                                    success = false;
                                    break;
                            }
                        }
                    }
                }

                return success;
            }
            catch (MapException e)
            {
                AcadEditor.WriteMessage(e.Message);
                return false;
            }
        }

        private static Editor AcadEditor
        {
            get
            {
                return Application.DocumentManager.MdiActiveDocument.Editor;
            }
        }
        //--------------------------------------------------------------------------
        //
        // 功能：判断表是否存在
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        private bool IsTableExisted(Tables tables, string tableName)
        {
            try
            {
                Autodesk.Gis.Map.ObjectData.Table table = tables[tableName];
                return table != null;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        // A preset ObjectData table name
        private string m_tableName = "MyODTable";

        #region "Sample Command"
        //--------------------------------------------------------------------------
        //
        // 功能：命令列表
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("CmdList")]
        public void CmdList()
        {
            AcadEditor.WriteMessage("\n 对象数据定义的命令: \n");
            AcadEditor.WriteMessage("** Cmd : CreateTable\n");
            AcadEditor.WriteMessage("** Cmd : AddRecord\n");
            AcadEditor.WriteMessage("** Cmd : ShowRecords\n");
            AcadEditor.WriteMessage("** Cmd : DeleteRecord\n");
            AcadEditor.WriteMessage("** Cmd : RemoveTable\n");
        }

        //--------------------------------------------------------------------------
        //
        // 功能：创建表
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("CreateTable")]
        public void CommandCreateTable()
        {
            try
            {
                Tables tables = HostMapApplicationServices.Application.ActiveProject.ODTables;

                if (this.IsTableExisted(tables, m_tableName))
                {
                    AcadEditor.WriteMessage("\n 对象数据表已经存在。");
                    return;
                }

                if (CreateTable(tables, m_tableName))
                {
                    string msg = "\n 表 'MyODTable' 创建成功.";
                    AcadEditor.WriteMessage(msg);
                }
                else
                {
                    AcadEditor.WriteMessage("\n 对象数据表创建失败。 ");
                }
            }
            catch (System.Exception err)
            {
                AcadEditor.WriteMessage(err.Message);
            }
        }

        //--------------------------------------------------------------------------
        //
        // 功能：附着对象数据
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddRecord")]
        public void CommandAddRecord()
        {
            try
            {
                Tables tables = HostMapApplicationServices.Application.ActiveProject.ODTables;

                if (!this.IsTableExisted(tables, m_tableName))
                {
                    AcadEditor.WriteMessage("\n 没有创建对象数据表。");
                    return;
                }

                PromptSelectionOptions options = new PromptSelectionOptions();

                // 选择实体
                options.SingleOnly = true;
                options.SinglePickInSpace = true;
                options.MessageForAdding = "选择附着了对象数据的实体:";
                PromptSelectionResult result = AcadEditor.GetSelection(options);

                if (result.Status != PromptStatus.OK)
                {
                    AcadEditor.WriteMessage("\n 用户取消! ");
                    return;
                }

                ObjectId[] ids = result.Value.GetObjectIds();
                if (ids.Length != 1)
                {
                    AcadEditor.WriteMessage("\n 无效的选择! ");
                    return;
                }

                if (AddODRecord(tables, m_tableName, ids[0]))
                {
                    AcadEditor.WriteMessage("\n 对象数据记录添加成功. ");
                }
                else
                {
                    AcadEditor.WriteMessage("\n  对象数据记录添加失败. ");
                }
            }
            catch (System.Exception err)
            {
                AcadEditor.WriteMessage(err.Message);
            }
        }
        //--------------------------------------------------------------------------
        //
        // 功能：访问对象数据
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("ShowRecords")]
        public void CommandShowRecords()
        {
            try
            {
                Tables tables = HostMapApplicationServices.Application.ActiveProject.ODTables;

                if (!this.IsTableExisted(tables, m_tableName))
                {
                    AcadEditor.WriteMessage("\n 没有创建对象数据表。");
                    return;
                }

                PromptSelectionOptions options = new PromptSelectionOptions();

                // 选择实体
                options.SingleOnly = true;
                options.SinglePickInSpace = true;
                options.MessageForAdding = "选择附着了对象数据的实体:";
                PromptSelectionResult result = AcadEditor.GetSelection(options);

                if (result.Status != PromptStatus.OK)
                {
                    AcadEditor.WriteMessage("\n 用户取消! ");
                    return;
                }

                ObjectId[] ids = result.Value.GetObjectIds();
                if (ids.Length != 1)
                {
                    AcadEditor.WriteMessage("\n 无效的选择! ");
                    return;
                }

                if (!ShowObjectDataInfo(tables, ids[0]))
                {
                    AcadEditor.WriteMessage("\n 访问对象数据失败. ");
                }
            }
            catch (System.Exception err)
            {
                AcadEditor.WriteMessage(err.Message);
            }
        }
        //--------------------------------------------------------------------------
        //
        // 功能：删除记录
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("DeleteRecord")]
        public void CommandDeleteRecord()
        {
            try
            {
                Tables tables = HostMapApplicationServices.Application.ActiveProject.ODTables;

                if (!this.IsTableExisted(tables, m_tableName))
                {
                    AcadEditor.WriteMessage("\n 没有创建对象数据表。");
                    return;
                }

                PromptSelectionOptions options = new PromptSelectionOptions();

                // 选择实体
                options.SingleOnly = true;
                options.SinglePickInSpace = true;
                options.MessageForAdding = "选择附着了对象数据的实体:";
                PromptSelectionResult result = AcadEditor.GetSelection(options);

                if (result.Status != PromptStatus.OK)
                {
                    AcadEditor.WriteMessage("\n 用户取消! ");
                    return;
                }

                ObjectId[] ids = result.Value.GetObjectIds();
                if (ids.Length != 1)
                {
                    AcadEditor.WriteMessage("\n 无效的选择! ");
                    return;
                }


                ShowObjectDataInfo(tables, ids[0]);

                PromptIntegerResult resultInt =
                    AcadEditor.GetInteger("\n 输入要删除的记录的索引 ");

                if (resultInt.Status != PromptStatus.OK)
                {
                    AcadEditor.WriteMessage("\n  用户取消! ");
                    return;
                }

                using (Records records = tables.GetObjectRecords(0, ids[0], Autodesk.Gis.Map.Constants.OpenMode.OpenForWrite, false))
                {
                    int deleteIndex = resultInt.Value;
                    if (deleteIndex < 0 || deleteIndex >= records.Count)
                    {
                        AcadEditor.WriteMessage("\n 无效的索引! ");
                        return;
                    }

                    IEnumerator ie = records.GetEnumerator();
                    ie.Reset();

                    int index = 0;
                    while (index <= deleteIndex)
                    {
                        ie.MoveNext();
                        index++;
                    }

                    try
                    {
                        // 移除记录
                        records.RemoveRecord();
                    }
                    catch (MapException e)
                    {
                        AcadEditor.WriteMessage(e.Message);
                    }
                }

                ShowObjectDataInfo(tables, ids[0]);
            }
            catch (System.Exception err)
            {
                AcadEditor.WriteMessage(err.Message);
            }
        }

        //--------------------------------------------------------------------------
        //
        // 功能：删除表
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("RemoveTable")]
        public void CommandRemoveTable()
        {
            try
            {
                Tables tables = HostMapApplicationServices.Application.ActiveProject.ODTables;

                if (!this.IsTableExisted(tables, m_tableName))
                {
                    AcadEditor.WriteMessage("\n 表不存在.");
                    return;
                }

                if (RemoveTable(tables, m_tableName))
                {
                    AcadEditor.WriteMessage("\n 删除对象数据表成功. ");
                }
                else
                {
                    AcadEditor.WriteMessage("\n 删除对象数据表出错. ");
                }
            }
            catch (System.Exception err)
            {
                AcadEditor.WriteMessage(err.Message);
            }
        }
        #endregion
    }
}
