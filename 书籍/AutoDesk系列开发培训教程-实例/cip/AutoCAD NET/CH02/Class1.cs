using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.EditorInput ;
using Autodesk.AutoCAD.Runtime ;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices ;

namespace CH02
{
    public class Class1
    {

        //--------------------------------------------------------------
        // 功能:获取用户输入
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //   
        //----------------------------------------------------------------
        [CommandMethod("GetData")]
        public void GetData()
        {
            //获取Editor对象
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //获取整型数据
            PromptIntegerOptions intOp = new PromptIntegerOptions("请输入多边形的边数：");
            PromptIntegerResult intRes;
            intRes = ed.GetInteger(intOp);
            //判断用户输入
            if (intRes.Status == PromptStatus.OK)
            {
                int nSides = intRes.Value;
                ed.WriteMessage("多边形的边数为：" + nSides);
            } if (intRes.Status == PromptStatus.Cancel)
            {
                ed.WriteMessage("用户按了取消ESC键/n" );
            }

        }

        //--------------------------------------------------------------
        // 功能:要求用户输入点
        // 作者： 
        // 日期：2007-7-20
        // 说明：
        //   
        //----------------------------------------------------------------
			[CommandMethod("PickPoint")]
        static public void PickPoint() 
			{
                //获取Editor对象
               Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
			    PromptPointOptions promptPtOp = new PromptPointOptions("选择一个点：");
                //指定的基点，如果指定了该点，则在选择的时候绘制一条橡皮线。
                promptPtOp.BasePoint = new Autodesk.AutoCAD.Geometry.Point3d(0, 0, 0);
			    PromptPointResult resPt; 
			    resPt = ed.GetPoint(promptPtOp); 
			    if (resPt.Status == PromptStatus.OK) 
			    {

                    ed.WriteMessage("选择的点为：" + resPt.Value.ToString());
			    } 
            }


            //--------------------------------------------------------------
            // 功能:获取选择集
            // 作者： 
            // 日期：2007-7-20
            // 说明：
            //   
            //----------------------------------------------------------------
			[CommandMethod("SelectEnt")]
        static public void SelectEnt() 
			{
                //获取Editor对象
               Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
               PromptSelectionOptions selectionOp = new PromptSelectionOptions();
               PromptSelectionResult ssRes = ed.GetSelection(selectionOp);
               if (ssRes.Status == PromptStatus.OK)
               {
                   SelectionSet SS = ssRes.Value;
                   int nCount = SS.Count;
                   ed.WriteMessage("选择了{0}个实体"  , nCount);
               }		    
            }

            //--------------------------------------------------------------
            // 功能:获取选择集（带过滤）
            // 作者： 
            // 日期：2007-7-20
            // 说明：
            //   
            //----------------------------------------------------------------
        		[CommandMethod("SelectEnt2")]
        static public void SelectEnt2() 
			{
                //获取Editor对象
               Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                // 定义选择集选项
               PromptSelectionOptions selectionOp = new PromptSelectionOptions();
                 //创建选择集过滤器，只选择块对象
                TypedValue[] filList = new TypedValue[1];
                filList[0] = new TypedValue((int)DxfCode.Start, "INSERT");
               SelectionFilter filter = new SelectionFilter(filList);

               PromptSelectionResult ssRes = ed.GetSelection(selectionOp, filter);
               if (ssRes.Status == PromptStatus.OK)
               {
                   SelectionSet SS = ssRes.Value;
                   int nCount = SS.Count;
                   ed.WriteMessage("选择了{0}个块"  , nCount);
               }		    

            }

     

        ///////////////////////
		}
	
		}
 

