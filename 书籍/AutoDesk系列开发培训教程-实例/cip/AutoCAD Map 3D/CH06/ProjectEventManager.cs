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

// ProjectEventManager.cs

using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.Gis.Map;
using Autodesk.Gis.Map.DisplayManagement;
using Autodesk.Gis.Map.Project;


namespace DisplayManagerSampleCS
{
    internal sealed class ProjectEventManager
    {
        private ProjectEventManager()
        {
        }

        /// <summary>
        /// Adds the map event handlers into the current map.
        /// </summary>
        public static void AddCurrentMapHandlers()
        {
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;
            project = app.ActiveProject;

            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    ObjectId managerId = DisplayManager.Create(project).MapManagerId(project, true);
                    MapManager manager = trans.GetObject(managerId, OpenMode.ForRead) as MapManager;
                    ObjectId currentMapId = manager.CurrentMapId;

                    Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForWrite);
    
                    currentMap.ScaleModified += new ScaleModifiedEventHandler(MapEventManager.ScaleChanged);
                    currentMap.ScaleAdded    += new ScaleAddedEventHandler(MapEventManager.ScaleAdded);
                    currentMap.ScaleErased   += new ScaleErasedEventHandler(MapEventManager.ScaleErased);
                    trans.Commit();
                }
            }
            catch (System.Exception e)
            {
                Utility.ShowMessage(e.Message);
            }
        }

        /// <summary>
        /// Removes the map event handlers from the current map.
        /// </summary>
        public static void RemoveCurrentMapHandlers()
        {
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;
            project = app.ActiveProject;

            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    ObjectId managerId = DisplayManager.Create(project).MapManagerId(project, true);
                    MapManager manager = trans.GetObject(managerId, OpenMode.ForRead) as MapManager;
                    ObjectId currentMapId = manager.CurrentMapId;

                    Map currentMap = trans.GetObject(currentMapId, OpenMode.ForWrite) as Map;

                    currentMap.ScaleModified -= new ScaleModifiedEventHandler(MapEventManager.ScaleChanged);
                    currentMap.ScaleAdded    -= new ScaleAddedEventHandler(MapEventManager.ScaleAdded);
                    currentMap.ScaleErased   -= new ScaleErasedEventHandler(MapEventManager.ScaleErased);
                    trans.Commit();
                }
            }
            catch (System.Exception e)
            {
                Utility.ShowMessage(e.Message);
            }
        }

        public static void MapAppended(System.Object sender, MapAppendedEventArgs e)
        {
            DoMapAppended(e.Map);
        }

        /// <summary>
        /// An event that fires when a Map is appended to the current project.
        /// </summary>
        /// <param name="appendedMap">[in] The Map object Appended to the project.</param>
        public static void DoMapAppended(Map appendedMap)
        {  
            string message = null;
            message = string.Format("\nMap {0} was appended to the current project.", appendedMap.Name);  
            Utility.ShowMessage(message);
        }

        public static void CategoryAppended(System.Object sender, CategoryAppendedEventArgs e)
        {
            DoCategoryAppended(e.Library, e.Category);
        }

        /// <summary>
        /// An event that fires when a Category is appended to the current projects display library.
        /// </summary>
        /// <param name="library">[in] The Style Library object the category was appended too.</param>
        /// <param name="category">[in] The Category object Appended to the Style Library.</param>
        public static void DoCategoryAppended(StyleLibrary library, StyleCategory category)
        {
            string message = null;
            message = string.Format("\nCategory {0} was appended to the Style Library which now has {1} Categories.", category.Name, library.CategoryCount);
            Utility.ShowMessage(message);
        }

        public static void MapSetCurrentBegin(System.Object sender, MapSetCurrentBeginEventArgs e)
        {
            DoMapSetCurrentBegin(e.Map);
        }
    
        /// <summary>
        /// An event that fires when a different Map is about to be set to the current Map.
        /// Note: this function also demonstrates how to unregister the Map events from the current Map.
        /// </summary>
        /// <param name="oldCurrentMap">[in] The Map object that will be replaced as the current Map.</param>
        public static void DoMapSetCurrentBegin(Map oldCurrentMap)
        {
            string message = null;
            message = string.Format("\nMap {0} is the Map about to be replaced as the current Map in the project.\n", oldCurrentMap.Name);
            Utility.ShowMessage(message);

            oldCurrentMap.ScaleModified -= new ScaleModifiedEventHandler(MapEventManager.ScaleChanged);
            oldCurrentMap.ScaleAdded    -= new ScaleAddedEventHandler(MapEventManager.ScaleAdded);
            oldCurrentMap.ScaleErased   -= new ScaleErasedEventHandler(MapEventManager.ScaleErased);
        }

        public static void MapSetCurrentEnd(System.Object sender, MapSetCurrentEndEventArgs e)
        {
            DoMapSetCurrentEnd(e.Map);
        }
    
        /// <summary>
        /// An event that fires when a different Map has been set to the current Map.
        /// Note: this function also demonstrates how to register the Map events to the current Map.
        /// </summary>
        /// <param name="newCurrentMap">[in] The Map object that has been set to the current Map.</param>
        public static void DoMapSetCurrentEnd(Map newCurrentMap)
        {
            string message = null;
            message = string.Format("\nMap {0} is the new current Map for the project.\n", newCurrentMap.Name);
            Utility.ShowMessage(message);

            newCurrentMap.ScaleModified += new ScaleModifiedEventHandler(MapEventManager.ScaleChanged);
            newCurrentMap.ScaleAdded    += new ScaleAddedEventHandler(MapEventManager.ScaleAdded);
            newCurrentMap.ScaleErased   += new ScaleErasedEventHandler(MapEventManager.ScaleErased);
        }
    }
}
