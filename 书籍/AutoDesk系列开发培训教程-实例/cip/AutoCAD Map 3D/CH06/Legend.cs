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

// Legend.cs

using System;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.Gis.Map;
using Autodesk.Gis.Map.DisplayManagement;
using Autodesk.Gis.Map.Project;

namespace CH06
{
    public sealed class MyLegend
    {
        private MyLegend()
        {
        }

        private static MyLegend m_Instance = new MyLegend();

        public static MyLegend Instance
        {
            get
            {
                return m_Instance;
            }
        }

        /// <summary>
        /// Creates the Legend which Displays the active (checked) Themes, Elements, and 
        /// Groups as in the Display Manager tree.
        /// Note that Legend does not keep information on which layout or map it is
        /// associated with. The order of items in the Legend will match the 
        /// order of items in the Display Manager tree.
        /// </summary>  
        /// <returns>
        /// Returns true if the Legend is created.
        /// </returns>
        public bool CreateLegend()
        {
            bool isCreated = false;
      
            // Get the Object Id for the current Map
            ObjectId currentMapId = new ObjectId();
            if (FindCurrentMapId(ref currentMapId))
            {
                // Open the Map for Write
                try
                {
                    using (Transaction trans = Utility.TransactionManager.StartTransaction())
                    {
                        Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForWrite);

                        // Find the ID of the currrent Layout
                        ObjectId currentLayoutId = new ObjectId();  
                        FindCurrentLayoutId(ref currentLayoutId);

                        // Try to get existing legend if it exists
                        Legend legend = null;
                        ObjectId legendId;
                        legendId = currentMap.GetLegendId(currentLayoutId);
                        if (legendId.IsValid)
                        {
                            legend = (Legend)trans.GetObject(legendId, OpenMode.ForWrite);
                            legend.UpdateTableContents(currentMap, false);
                        }
                        else
                        {
                            try
                            {
                                PromptPointResult pointPromptResult = Utility.Editor.GetPoint("\nSpecify insertion point: ");
                                if (pointPromptResult.Status == PromptStatus.OK)
                                {
                                    // CreateLegend() creates a new legend for the specified layout.
                                    // It stores the object ids of the layout and the legend used for
                                    // that layout.
                                    // If the legend already exists, it deletes it and creates a new one.
                                    // CreateLegend() does NOT fill the legend with data.  
                                    legendId = currentMap.CreateLegend(currentLayoutId);
                                    legend = (Legend)trans.GetObject(legendId, OpenMode.ForWrite);

                                    legend.Position = pointPromptResult.Value;
                                    // UpdateTableContents clears out the contents of the table and
                                    // updates it with current data.
                                    legend.UpdateTableContents(currentMap, true);      

                                    isCreated = true;
                                }
                            }  
                            catch (Autodesk.AutoCAD.Runtime.Exception)
                            {
                                Utility.ShowMessage("\nUnable to create the Legend object.");
                            }
                        }
                        trans.Commit();
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception)
                {
                    Utility.ShowMessage("\nUnable to open Map for Write.");
                }
            }
            return isCreated;
        }

        /// <summary>
        /// Finds the object id for the current Map.
        /// </summary>
        /// <param name="currentMapId">[out] The object id of the Element.</param>  
        /// <returns>
        /// Returns true if the Map ObjectId is found.
        /// </returns>
        private bool FindCurrentMapId(ref ObjectId currentMapId)
        {
            bool isFound = false;

            // Get the project associated with the current AutoCAD document
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;

            project = app.ActiveProject;
  
            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    ObjectId managerId = DisplayManager.Create(project).MapManagerId(project, true);
                    MapManager manager = (MapManager)trans.GetObject(managerId, OpenMode.ForRead);
                    currentMapId = manager.CurrentMapId;
                    isFound = true;
                    trans.Commit();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                Utility.ShowMessage("\nUnable to get the current Map's Object ID.");
            }
            return isFound;
        }

        /// <summary>
        /// Finds the object id for the current Layout.
        /// </summary>
        /// <param name="currentLayoutId">[out] The object id of the current Layout.</param>  
        private void FindCurrentLayoutId(ref ObjectId currentLayoutId)
        {
            LayoutManager layerManager = LayoutManager.Current;

            // Get the Id
            currentLayoutId = layerManager.GetLayoutId(layerManager.CurrentLayout);
        }
    }
}
