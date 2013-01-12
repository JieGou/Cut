using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Inventor;

namespace Chapter8
{
    public partial class frmSelection : Form
    {
        private Inventor.Application ThisApplication;
        private InteractionEvents interaction;
        private SelectEvents select;

        public frmSelection(Inventor.Application Application)
        {
            InitializeComponent();

            ThisApplication = Application;
        }

        private void frmSelection_Load(object sender, EventArgs e)
        {
            //创建新的InteractionEvents对象
            interaction = ThisApplication.CommandManager.CreateInteractionEvents();

            //设置提示
            interaction.StatusBarText = "Select an edge.";

            //连接到相关的选择事件
            select = interaction.SelectEvents;

            //定义所有的零件边都可以选择
            select.AddSelectionFilter(SelectionFilterEnum.kPartEdgeFilter);

            //启用单一选择
            select.SingleSelectEnabled = true;

            //开始选择过程
            interaction.Start();

            //事件响应
            select.OnSelect += new SelectEventsSink_OnSelectEventHandler(select_OnSelect);
            select.OnPreSelect += new SelectEventsSink_OnPreSelectEventHandler(select_OnPreSelect);
        }

        void select_OnSelect(ObjectsEnumerator JustSelectedEntities, SelectionDeviceEnum SelectionDevice, Inventor.Point ModelPosition, Point2d ViewPosition, Inventor.View View)
        {
            //计算所选边的长度
            int i;
            double length = 0.0;
            for (i = 0; i < JustSelectedEntities.Count; i++)
            {
                //因为已经过滤器设置为选择边，所以将返回的图元指定为Edge对象是安全的
                Edge edge;
                edge = (Edge)JustSelectedEntities[i];

                //确定当前边的长度
                double min;
                double max;
                edge.Evaluator.GetParamExtents(out min, out max);

                double singleLength;
                edge.Evaluator.GetLengthAtParam(min, max, out singleLength);

                //计算集合中所有边的长度
                length = length + singleLength;
            }

            //显示边的长度和数量
            txtLength.Text = string.Format("{0} cm", length);
            txtEdgeCount.Text = string.Format("{0}", JustSelectedEntities.Count);
        }

        void select_OnPreSelect(ref object PreSelectEntity, out bool DoHighlight, ref ObjectCollection MorePreSelectEntities, SelectionDeviceEnum SelectionDevice, Inventor.Point ModelPosition, Point2d ViewPosition, Inventor.View View)
        {
            DoHighlight = true;

            //设置到鼠标所选的对象的引用，由于前面定义的过滤器，可以知道该对象一定是一条边
            Edge edge;
            edge = (Edge)PreSelectEntity;

            //确定是否有与该边相切并连接的边
            EdgeCollection edges;
            edges = edge.TangentiallyConnectedEdges;
            if (edges.Count > 1)
            {
                //建立包含其他边的对象集合
                MorePreSelectEntities = ThisApplication.TransientObjects.CreateObjectCollection(null);

                for (int i = 1; i <= edges.Count; i++)
                {
                    MorePreSelectEntities.Add(edges[i]);
                }
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //停止选择并释放全局引用
            interaction.Stop();
            select = null;
            interaction = null;

            //卸载窗体
            this.Hide();
        }

        public void InteractionEventsSink_OnTerminateEventHandler()
        {
            //释放全局引用
            select = null;
            interaction = null;

            //卸载窗体
            this.Hide();
        }
    }
}