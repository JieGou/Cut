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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

// DisplayManager.cs


using System;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.Gis.Map;
using Autodesk.Gis.Map.DisplayManagement;
using Autodesk.Gis.Map.Project;

namespace CH06
{
    //--------------------------------------------------------------------------
    //
    // 功能：工具类，封装类显示管理器的函数
    //
    //  作者： 
    //
    //  日期：200708
    //
    //   历史：
    //--------------------------------------------------------------------------
    public sealed class MyDisplayManager
    {
        private MyDisplayManager()
        {
        }

        private static MyDisplayManager m_Instance = new MyDisplayManager();

        public static MyDisplayManager Instance
        {
            get
            {
                return m_Instance;
            }
        }

         //--------------------------------------------------------------------------
        //
        // 功能：新建地图，新的地图将会出现在显示管理器的下拉列表中
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        public bool AddMap(string mapName)
        {
            bool isAdded = false;

            // 获取当前AutoCAD Map应用程序对象
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;

            project = app.ActiveProject;

            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    ObjectId managerId = DisplayManager.Create(project).MapManagerId(project, true);
                    MapManager manager = (MapManager)trans.GetObject(managerId, OpenMode.ForWrite);
    
                    // 创建新的地图
                    ObjectId id = manager.CreateNewMap();    

                    // 重新命名地图
                    manager.ResetName(id, mapName);
                    string message = string.Format("\n地图 {0}创建成功。", mapName);
                    Utility.ShowMessage(message);

                    trans.Commit();
                    isAdded = true;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                string message = string.Format("\n创建地图失败，错误代码: {0}.", e.ErrorStatus);
                Utility.ShowMessage(message);
            }
  
            return isAdded;
        }

       //--------------------------------------------------------------------------
        //
        // 功能：在显示管理器的下拉列表中移除地图
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        public bool RemoveMap(string mapName)
        {
            bool isRemoved = false;
            
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;

            project = app.ActiveProject;

            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    ObjectId managerId = DisplayManager.Create(project).MapManagerId(project, true);
                    MapManager manager = (MapManager)trans.GetObject(managerId, OpenMode.ForWrite);

                    manager.Remove(mapName);
                    trans.Commit();

                    string message = string.Format("\n地图 {0} 已经被删除。", mapName);
                    Utility.ShowMessage(message);
                    isRemoved = true;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                string message = string.Format("\n地图 {0} 删除失败。错误代码: {1}.", mapName, e.ErrorStatus);
                Utility.ShowMessage(message);
            }

            return isRemoved;
        }

       //--------------------------------------------------------------------------
        //
        // 功能：设置地图将为当前显示管理器中的地图
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        public bool SetMapAsCurrent(string mapName)
        {
            bool isSettedAsCurrent = false;
            
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;

            project = app.ActiveProject;

            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    ObjectId managerId = DisplayManager.Create(project).MapManagerId(project, true);
                    MapManager manager = (MapManager)trans.GetObject(managerId, OpenMode.ForWrite);

                    manager.Current = mapName;
                    trans.Commit();

                    string message = string.Format("\n地图{0}已经设为当前地图。", mapName);
                    Utility.ShowMessage(message);
                    isSettedAsCurrent = true;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                string message = string.Format("\n地图{0}不能设为当前地图，错误代码: {1}。", mapName, e.ErrorStatus);
                Utility.ShowMessage(message);
            }  

            return isSettedAsCurrent;
        }

       //--------------------------------------------------------------------------
        //
        // 功能：设置比例尺值在显示管理器的尺度列表
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //--------------------------------------------------------------------------
        public bool AddScale()
        {
            bool isAdded = false;
            string message = null;
            ObjectId currentMapId = new ObjectId();
            if (!FindCurrentMapId(ref currentMapId))
            {
                return isAdded;
            }

            MapApplication app = HostMapApplicationServices.Application;
            Editor editor = app.GetDocument(app.ActiveProject).Editor;
            PromptDoubleOptions promptOptions = new PromptDoubleOptions("");
            promptOptions.AllowNone = false;
            promptOptions.AllowZero = false;
            promptOptions.AllowNegative = false;
            promptOptions.Message = "\n输入新的比例尺： ";
            double scale = 0.0;
            try
            {
                PromptDoubleResult doublePromptResult = editor.GetDouble(promptOptions);
                if (doublePromptResult.Status == PromptStatus.OK)
                {
                    scale = doublePromptResult.Value;  
                    using (Transaction trans = Utility.TransactionManager.StartTransaction())
                    {
                        Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForWrite);
                        // Add the new scale
                        currentMap.AddScaleThreshold(scale, 0);
                        message = string.Format("\n新的比例尺 {0}被添加。", scale);
                        Utility.ShowMessage(message);
                        isAdded = true;
                        trans.Commit();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                message = string.Format("\n添加比例尺 {0}失败。", scale);
                Utility.ShowMessage(message);
            }

            return isAdded;
        }

        //--------------------------------------------------------------------------
        //
        // 功能：删除在显示管理器的尺度列表中的比例尺
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //-------------------------------------------------------------------------- 
        public bool RemoveScale()
        {
            bool isRemoved = false;
            string message = null;
            ObjectId currentMapId = new ObjectId();
            if (!FindCurrentMapId(ref currentMapId))
            {
                return isRemoved;
            }

            MapApplication app = HostMapApplicationServices.Application;
            Editor editor = app.GetDocument(app.ActiveProject).Editor;
            PromptDoubleOptions promptOptions = new PromptDoubleOptions("");
            promptOptions.AllowNone = false;
            promptOptions.AllowZero = false;
            promptOptions.AllowNegative = false;
            promptOptions.Message = "\n输入要删除的比例尺: ";
            double scale = 0;

            try
            {
                PromptDoubleResult doublePromptResult = editor.GetDouble(promptOptions);
                if (doublePromptResult.Status == PromptStatus.OK)
                {
                    scale = doublePromptResult.Value;  
                    using (Transaction trans = Utility.TransactionManager.StartTransaction())
                    {
                        Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForWrite);
                        // Remove the scale
                        currentMap.RemoveScaleThreshold(scale);
                        trans.Commit();
                        message = string.Format("\n比例尺 {0} 已经被删除。", scale);
                        Utility.ShowMessage(message);
                        isRemoved = true;
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                message = string.Format("\n不能删除比例尺 {0}.", scale);
                Utility.ShowMessage(message);
            }
    
            return isRemoved;
        }


        //--------------------------------------------------------------------------
        //
        // 功能：设置当前比例尺
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //-------------------------------------------------------------------------- 
        public bool SetScaleAsCurrent()
        {
            bool isSettedAsCurrent = false;
            string message = null;
            ObjectId currentMapId = new ObjectId();
            if (!FindCurrentMapId(ref currentMapId))
            {
                return isSettedAsCurrent;
            }

            MapApplication app = HostMapApplicationServices.Application;
            Editor editor = app.GetDocument(app.ActiveProject).Editor;
            PromptDoubleOptions promptOptions = new PromptDoubleOptions("");
            promptOptions.AllowNone = false;
            promptOptions.AllowZero = false;
            promptOptions.AllowNegative = false;
            promptOptions.Message = "\n输入要设为当前的比例尺: ";
            double scale = 0;

            try
            {
                PromptDoubleResult doublePromptResult = editor.GetDouble(promptOptions);
                if (doublePromptResult.Status == PromptStatus.OK)
                {
                    scale = doublePromptResult.Value;  
                    using (Transaction trans = Utility.TransactionManager.StartTransaction())
                    {
                        Map currentMap = (Map )trans.GetObject(currentMapId, OpenMode.ForWrite);

                        currentMap.SetCurrentScale( scale, true );
                        message = string.Format("\n比例尺 {0}已经设为当前。", scale);
                        Utility.ShowMessage(message);
                        isSettedAsCurrent = true;
                        trans.Commit();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                message = string.Format("\n不能设置比例尺 {0}为当前比例尺。", scale);
                Utility.ShowMessage(message);
            }
      
            return isSettedAsCurrent;
        }

        //--------------------------------------------------------------------------
        //
        // 功能：获取当前地图的ID
        //
        //  作者： 
        //
        //  日期：200708
        //
        //   历史：
        //-------------------------------------------------------------------------- 
        private bool FindCurrentMapId(ref ObjectId currentMapId)
        {
            bool isFound = false;
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;

            project = app.ActiveProject;

            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    ObjectId managerId = DisplayManager.Create(project).MapManagerId(project, true);
                    MapManager manager = trans.GetObject(managerId, OpenMode.ForRead) as MapManager;
                    if (null != manager)
                    {
                        currentMapId = manager.CurrentMapId;
                        isFound = true;
                    }
                    trans.Commit();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                Utility.ShowMessage("\n不能获取当前地图的ID。");
            }

            return isFound;
        }
    }
}
