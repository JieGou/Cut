using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Inventor;


namespace Chapter2
{
    public partial class BrowserForm : Form
    {
        public BrowserForm(Inventor.Application InventorApplication)
        {
            InitializeComponent();
            m_inventorApplication = InventorApplication;
        }

        //声明日历控件的变量
        private MSACAL.Calendar calCtrl;

        //声明Inventor应用程序的变量
        private Inventor.Application m_inventorApplication;

        //声明事件和浏览器窗格的变量
        private DocumentEvents docEvents;
        private BrowserPane browserPane;

        private void BrowserForm_Load(object sender, EventArgs e)
        {
            //获得激活文档，假设有一个文档被打开
            Inventor.Document doc;
            doc = m_inventorApplication.ActiveDocument;

            //连接到文档事件，用于响应文档关闭
            docEvents = doc.DocumentEvents;

            //使用日历控件创建新的浏览器窗格
            browserPane = doc.BrowserPanes.Add("Calendar", "MSCAL.Calendar");

            //设置到日历控件的引用
            calCtrl = (MSACAL.Calendar)browserPane.Control;

            //设置日历控件显示当前日期
            calCtrl.Today();

            //使新窗格成为激活窗格
            browserPane.Activate();

            calCtrl.Click += new MSACAL.DCalendarEvents_ClickEventHandler(calCtrl_Click);
        }

        void calCtrl_Click()
        {
            //在Inventor状态栏显示新数据
            m_inventorApplication.StatusBarText = string.Format("Selected new date: {0}-{1}-{2}", calCtrl.Year, calCtrl.Month,calCtrl.Day);

            //在选择2000年1月1日时卸载窗体
            if (calCtrl.Value.ToString() == "2000-1-1 0:00:00")
            {
                Unload();
            }
        }

        private void BrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //卸载窗体
            Unload();
        }

        private void Unload()
        {
            calCtrl=null;
            docEvents = null;
            if (browserPane != null)
            {
                browserPane.Delete();
                browserPane = null;
            }
        }
     }
}