//
// (C) Copyright 2004-2007 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Text;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

using Autodesk.Gis.Map;
using Autodesk.Gis.Map.Utilities;
using Autodesk.Gis.Map.Project;
using Autodesk.Gis.Map.Query;
using Autodesk.Gis.Map.Constants;



namespace CH02
{
    public class CmdQuery
    {
        //--------------------------------------------------------------------------
        //
        // 功能：执行查询
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        [CommandMethod("RunQuery")]
        public void RunQuery()
        {
            // 获取地图应用程序对象和工程对象
            MapApplication mapApi = HostMapApplicationServices.Application;
            ProjectModel proj = mapApi.ActiveProject;
            DrawingSet drawingSet = proj.DrawingSet;

           //放缩到图形集边界
            proj.DrawingSet.ZoomExtents();

          //使用LocationCondition来定义一个查询条件
            LocationCondition qryCondition  =   new LocationCondition ();
            qryCondition.LocationType  = LocationType.LocationInside ;
            qryCondition.JoinOperator = JoinOperator .OperatorAnd ;
           // 定义圆边界
            CircleBoundary boundary = null;

            PromptPointResult pointRes = Utility.AcadEditor.GetPoint("\n选择原点: ");
            if (pointRes.Status == PromptStatus.OK)
            {
                Point3d point = pointRes.Value;
                PromptDistanceOptions distOptions = new PromptDistanceOptions("\n选择半径: ");
                distOptions.BasePoint = point;
                distOptions.UseBasePoint = true;
                distOptions.DefaultValue = 0.0;
                PromptDoubleResult doubleRes = Utility.AcadEditor.GetDistance(distOptions);
                if (doubleRes.Status == PromptStatus.OK)
                {
                    double rad = doubleRes.Value;
                    boundary = new CircleBoundary(new Point3d(point.X, point.Y, 0.0), rad);
                }
            }
            //
            qryCondition.Boundary = boundary;
            //创建一个条件分支
             QueryBranch qryBranch  =  QueryBranch.Create ();
            qryBranch.JoinOperator = JoinOperator.OperatorAnd;
            qryBranch.AppendOperand(qryCondition);
             ////使用PropertyCondition来定义一个属性查询条件(LWPOLYLINE)
            PropertyCondition propCondition= new PropertyCondition();
             //JoinOperator.OperatorAnd, PropertyType.EntityType, ConditionOperator.ConditionEqual, "LWPOLYLINE";
            propCondition.JoinOperator = JoinOperator.OperatorAnd ;
            propCondition.PropertyType  = PropertyType.EntityType ;
            propCondition.ConditionOperator = ConditionOperator.ConditionEqual ;
            propCondition.Value = "LWPOLYLINE";

            //	通过QueryBranch的构造函数来定义一个或多个查询分支。
            QueryBranch  subBranch  = new QueryBranch(JoinOperator.OperatorAnd);
             //	创建一个带有查询条件和分支的查询树。
            subBranch.AppendOperand(propCondition);
            qryBranch.AppendOperand(subBranch);
            //  创建一个查询。
            QueryModel qryModel  = proj.CreateQuery (true);
            //	完成查询的定义。
            qryModel.Define (qryBranch);
            //设置绘制模式
            qryModel.Mode = QueryType.QueryDraw;
            //定义查询
            qryModel.Define(qryBranch);
            //执行查询。
            qryModel.Run();

            Utility.AcadEditor.WriteMessage("\n 完成查询 。");
   }

   //--------------------------------------------------------------------------
   //
   // 功能：输出查询库内容
   //
   //  作者： 
   //
   //  日期：200708
   //
   //   历史：
   //--------------------------------------------------------------------------
        [CommandMethod("PrintQuery")]
        public void PrintQueryLibrary()
        {
            MapApplication mapApi = HostMapApplicationServices.Application;
            ProjectModel proj = mapApi.ActiveProject;
            QueryLibrary qryLib = proj.QueryCategories;

            if (qryLib.CategoryCount <= 0)
                return;

            Utility.AcadEditor.WriteMessage("\n***** 查询库 *****");
            for (int i = 0; i < qryLib.CategoryCount; i++)
            {
                QueryCategory qryCat = qryLib[i];
                Utility.AcadEditor.WriteMessage(string.Format("\n*类别: {0}", qryCat.Name));

                for (int j = 0; j < qryCat.QueryCount; j++)
                {
                    QueryAttribute qryAttr = qryCat[j];
                    Utility.AcadEditor.WriteMessage(string.Format("\n   查询: {0} - {1}",
                        qryAttr.Name, qryAttr.Description));
                }
            }
        }
    }
}
