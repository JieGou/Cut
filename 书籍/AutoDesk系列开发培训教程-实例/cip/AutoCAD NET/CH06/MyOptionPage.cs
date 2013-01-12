using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

namespace CH06
{
    public partial class MyOptionPage : UserControl
    {
        public MyOptionPage()
        {
            InitializeComponent();
        }
        public void OnOk()
        {
            Class1.sAuthorDefault   = tb_Author.Text;
            Class1.sCompanyDefault  = tb_Company.Text;
        }

        public static void AddTabDialog()
        {

            Autodesk.AutoCAD.ApplicationServices.Application.DisplayingOptionDialog += new TabbedDialogEventHandler(TabHandler);

        }

        public static void RemoveTabDialog()
        {
            Autodesk.AutoCAD.ApplicationServices.Application.DisplayingOptionDialog -= new TabbedDialogEventHandler(TabHandler);
        }
        private static void TabHandler(object sender, Autodesk.AutoCAD.ApplicationServices.TabbedDialogEventArgs e)
        {
            MyOptionPage MyOpPage = new MyOptionPage();
            TabbedDialogExtension tabbedDialog = new TabbedDialogExtension(MyOpPage,new TabbedDialogAction(MyOpPage.OnOk));
            e.AddTab("Œ“µƒ Ù–‘“≥",tabbedDialog);
        }


    }
}
