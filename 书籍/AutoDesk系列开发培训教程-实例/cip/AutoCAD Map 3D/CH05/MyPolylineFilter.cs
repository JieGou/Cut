using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.Gis.Map.Filters;

namespace CH05
{

    public  class MyPolylineFilter : ObjectFilter
    {

        // 定义多义线的最小长度和最大长度，用作过滤的条件
        private double m_MinLen;
        private double m_MaxLen;

        public double MinLen
        {
            get
            {
                return m_MinLen;
            }
            set
            {

                m_MinLen = value;
            }
        }

        public double MaxLen
        {
            get
            {
                return m_MaxLen;
            }
            set
            {
                m_MaxLen = value;
            }
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
        public virtual void FilterObjects(ObjectIdCollection outputIds,ObjectIdCollection inputIds)
        {

            // 遍历所有传入的objectID 
            Transaction trans = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction();
            try
            {
                ObjectId objId;
                foreach (ObjectId tempLoopVar_objId in inputIds)
                {
                    objId = tempLoopVar_objId;
                    Entity ent;
                    //打开对象
                    ent = trans.GetObject(objId, OpenMode.ForRead )as Entity ;
                    // 类型判断
                    if (ent is Polyline)
                    {
                        Polyline pl = ent as Polyline;
                        // 计算多义线的长度，然后判断其长度是否在指定的最大和最小长度之间
                        double Len = GetPolylineLength(pl);
                        // 如果满足条件则将对象id保存，最后输出
                        if (Len >= m_MinLen && Len <= m_MaxLen)
                        {
                            outputIds.Add(objId);
                        }
                    }
                }
                trans.Commit();
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                Utility.AcadEditor.WriteMessage(e.Message);
            }


        }


        //--------------------------------------------------------------------------
        //
        // 功能：计算多义线的长度
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        private double GetPolylineLength(Polyline polyline)
        {
            return polyline.GetDistanceAtParameter(polyline.EndParam);
        }

    }
}
