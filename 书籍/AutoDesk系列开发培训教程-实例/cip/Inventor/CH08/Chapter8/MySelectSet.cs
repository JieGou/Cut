using System;
using System.Collections.Generic;
using System.Text;
using Inventor;
using System.Windows.Forms;

namespace Chapter8
{
    class MySelectSet
    {
        public void ShowSurfaceArea(Inventor.Application ThisApplication)
        {
            //设置到激活文档中选择集的引用
            SelectSet selectSet;
            selectSet = ThisApplication.ActiveDocument.SelectSet;

            //确认只选中了一个图元
            if(selectSet.Count == 1)
            {
                //确认选中了一个表面
                if (selectSet[1].GetType() is Face == false)
                {
                    //设置到所选表面的引用
                    Face face;
                    face = (Face)selectSet[1];

                    //显示所选表面的面积
                    MessageBox.Show(string.Format("Surface area:  {0}  cm^2", face.Evaluator.Area));
                }
                else
                {
                    MessageBox.Show("You must select a single face.");
                }
            }
            else
            {
                MessageBox.Show("You must select a single face.");
            }
        }
    }
}
