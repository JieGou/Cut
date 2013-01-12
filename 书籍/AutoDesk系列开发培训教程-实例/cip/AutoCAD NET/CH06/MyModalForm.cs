using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

[assembly: CommandClass(typeof(CH06.MyModalForm))]
namespace CH06
{
    public partial  class MyModalForm : System.Windows.Forms.Form
    {
        public MyModalForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database db = HostApplicationServices.WorkingDatabase;
			Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
			this.Hide();
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    PromptEntityOptions  prEnt = new PromptEntityOptions("选择一个圆：\n");
                    PromptEntityResult  prEntRes = ed.GetEntity(prEnt);
                    if (prEntRes.Status != PromptStatus.OK)
                        throw new System.Exception("错误或用户取消");

                    ObjectId id = prEntRes.ObjectId;
                    DBObject obj = trans.GetObject(id, OpenMode.ForRead) ;
                    Circle cir;
                    if (obj is Circle)
                    {
                        cir = obj as Circle;
                    }
                    else
                    {
                        throw new System.Exception("选择对象错误");
                    }
                    tb_layer.Text = cir.Layer ;
                    tb_center.Text = cir.Center.ToString();
                    tb_radius.Text = cir.Radius .ToString();

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
            finally
            {
                this.Show();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       [CommandMethod("ShowModalForm")]
        public void ShowModalForm()
        {
            MyModalForm modalForm = new MyModalForm();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(modalForm);
        }

        ///////////////////////////
    }
 
}