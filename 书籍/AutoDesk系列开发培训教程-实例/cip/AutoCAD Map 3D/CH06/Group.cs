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

// Group.cs

using System;
using System.Collections;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.Gis.Map.Project;
using Autodesk.Gis.Map;
using Autodesk.Gis.Map.DisplayManagement;

namespace CH06
{
    public sealed class MyGroup
    {
        private MyGroup()
        {
        }

        private static MyGroup m_Instance = new MyGroup();

        public static MyGroup Instance
        {
            get
            {
                return m_Instance;
            }
        }

        /// <summary>
        /// Adds a Group to the display manager.
        /// </summary>
        /// <param name="groupName">[in] The name of the Group to be added.</param>
        /// <returns>
        /// Returns true if the Group is added.
        /// </returns>
        public bool AddNewGroup(string groupName)
        {
            bool isAdded = false;   
            string message = null;
            
			// Create the Group 
            Autodesk.Gis.Map.DisplayManagement.Group newGroup = 
				Autodesk.Gis.Map.DisplayManagement.Group.Create();

            // Set the new Group's name
            newGroup.Name = groupName;

            // Get the Object Id for the current Map
            ObjectId currentMapId = new ObjectId();
            if (FindCurrentMapId(ref currentMapId))
            {
                try
                {
                    using (Transaction trans = Utility.TransactionManager.StartTransaction())
                    {
                        // Open the Map for write
                        Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForWrite);

                        IEnumerator iterator = currentMap.NewIterator(true, true);

                        // Add the new Group to the current Map
                        ObjectId elementId;
                        elementId = currentMap.AddItem(newGroup, iterator);  
                        trans.AddNewlyCreatedDBObject(newGroup, true);

                        // Print out result
                        message = string.Format("\nGroup {0} was sucessfully added.", groupName);
                        Utility.ShowMessage(message);
                        isAdded = true;
                        trans.Commit();
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception e)
                {
                    message = string.Format("\nFailed to add Group {0}, Error code: {1}.", groupName, e.ErrorStatus);
                    Utility.ShowMessage(message);
                }
            }
            return isAdded;
        }

        /// <summary>
        /// Removes a Group from the display manager.
        /// </summary>
        /// <param name="groupName">[in] The name of the Group to be Removed.</param>
        /// <returns>
        /// Returns true if the Group is removed.
        /// </returns>
        public bool RemoveGroup(string groupName)
        {
            bool isRemoved = false;   
            string message = null;

            // Get the Object Id for the current Map
            ObjectId currentMapId = new ObjectId();
            if (FindCurrentMapId(ref currentMapId))
            {  
                // Find the object Id of the Group 
                ObjectId groupId = new ObjectId();
                if (FindGroupId(ref groupId, groupName))
                {
                    try
                    {
                        using (Transaction trans = Utility.TransactionManager.StartTransaction())
                        {
                            // Open the Map for write
                            Map currentMap = 
                                (Map)trans.GetObject(currentMapId, OpenMode.ForWrite);
                            currentMap.RemoveItem(groupId);
                            trans.Commit();
                            // Print out result
                            message = string.Format("\nGroup {0} was sucessfully removed.", groupName);
                            Utility.ShowMessage(message);
                            isRemoved = true;
                        }
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception e)
                    {
                        message = string.Format("\nFailed to remove Group {0}, Error code: {1}.", groupName, e.ErrorStatus);
                        Utility.ShowMessage(message);
                    }
                }    
            }
            return isRemoved;
        }

        /// <summary>
        /// Adds a Layer element to a specific Group.
        /// </summary>
        /// <param name="groupName">[in] The name of the Group, the Element is to be added too.</param>  
        /// <returns>
        /// Returns true if Element is added.
        /// </returns>
        public bool AddElementToGroup(string groupName)
        {
            bool isAdded = false;
            string message = null;
            string elementName = null; 
            PromptResult stringPromptResult = null;

            stringPromptResult = Utility.Editor.GetString("\nSpecify the new Layer element name: ");  
            if (stringPromptResult.Status != PromptStatus.OK
                || (elementName = stringPromptResult.StringResult.Trim()).Length == 0)
            {
                message = string.Format("\nERROR: {0} is an Invalid Element name", elementName);
                Utility.ShowMessage(message);
                return isAdded;
            }

            try
            {
                // Create the Layer Element 
                LayerElement element = null;
                element = LayerElement.Create();

                // Set the Layer Elements name
                element.Name = elementName;

                // Create the Layer Descriptor
                LayerDataSourceDescriptor descriptor = null; 
                descriptor = LayerDataSourceDescriptor.Create();

                string layerNames = null; 

                stringPromptResult = Utility.Editor.GetString("\nSpecify the Element's Layers: ");
                if (stringPromptResult.Status == PromptStatus.OK
                    && (layerNames = stringPromptResult.StringResult.Trim()).Length > 0)
                {
                    try 
                    {
                        descriptor.AcquisitionStatement = layerNames;
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception e)
                    {
                        message = string.Format("\nThe Element Layer names {0} are invalid, Error code: {1}.", layerNames, e.ErrorStatus);
                        Utility.ShowMessage(message);
                        element = null;
                    }
                    catch (System.Exception e)
                    {
                        Utility.ShowMessage(e.Message);
                        element = null;
                    }
                }
                else
                {
                    // Clean up element memory
                    element = null;
                }

                if (null != element)
                {
                    // Find the object Id of the Group 
                    ObjectId groupId = new ObjectId();
                    if (FindGroupId(ref groupId, groupName))
                    {      
                        // Open the Group for write
                        Autodesk.Gis.Map.DisplayManagement.Group group = null;
                        try
                        {
                            using (Transaction trans = Utility.TransactionManager.StartTransaction())
                            {
                                group = (Autodesk.Gis.Map.DisplayManagement.Group )trans.GetObject(groupId, OpenMode.ForWrite);

                                System.Collections.IEnumerator iterator = group.NewIterator(true, true);

                                // Add element to the group
                                ObjectId groupElementId;

                                groupElementId = group.AddItem(element, iterator);
                                trans.AddNewlyCreatedDBObject(element, true);

                                if (null != descriptor)
                                {  
                                    element.AcquisitionCriteria = descriptor;
                                }

                                // Print out result
                                message = string.Format("\nElement {0} was sucessfully added to Group {1}.", elementName, groupName);
                                Utility.ShowMessage(message);
                                trans.Commit();
                                isAdded = true;
                            }
                        }
                        catch (Autodesk.AutoCAD.Runtime.Exception e)
                        {
                            message = string.Format("\nFailed to add Element {0} to Group {1}, Error code: {2}.", elementName, groupName, e.ErrorStatus);
                            Utility.ShowMessage(message);
                        }
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                Utility.ShowMessage(e.Message);
            }
            return isAdded;
        }


        /// <summary>
        /// Finds the object id for the current Map.
        /// </summary>
        /// <param name="currentMapId"> [out] The object id of the Element.</param>  
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
                    currentMapId = ((MapManager )trans.GetObject(managerId, OpenMode.ForRead)).CurrentMapId;
                    isFound = true;
                    trans.Commit();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception )
            {
                Utility.ShowMessage("\nUnable to get the current Map's Object ID.");
            }
            return isFound;
        }


        /// <summary>
        /// Finds the object id for the Group specified.
        /// Note: this only works for top level groups, it does not iterate through any sub groups
        /// </summary>
        /// <param name="groupId">[out] The object id of the Element.</param>
        /// <param name="groupName">[in] The name of the Element to be Found.</param>
        /// <returns>
        /// Returns true if the Group is found.
        /// </returns>
        private bool FindGroupId(ref ObjectId groupId, string groupName)
        {
            // Get the Object Id for the current Map
            ObjectId currentMapId = new ObjectId();
            if (!FindCurrentMapId(ref currentMapId))
            {
                return false;
            }

            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForRead);

                    // get the Map Iterator    
                    IEnumerator iterator = currentMap.NewIterator(true, true);          
    
                    // Loop through the iter until we find the Element
                    while (iterator.MoveNext())
                    {
                        ObjectId itemId = (ObjectId)iterator.Current;
                        Item item = 
                            (Item)trans.GetObject(itemId, OpenMode.ForRead); 
        
                        if (item.GetType().Equals(typeof(Autodesk.Gis.Map.DisplayManagement.Group)) 
                            || item.GetType().IsSubclassOf(typeof(Autodesk.Gis.Map.DisplayManagement.Group)))
                        {
                            if (item.Name.Equals(groupName))
                            {            
                                // Get the element id            
                                groupId = item.ObjectId;     
                                return true;
                            }
                        }
                    }
    
                    trans.Commit();
     
                    string message = string.Format("\nUnable to find the object ID for Group {0}.", groupName);
                    Utility.ShowMessage(message);
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                Utility.ShowMessage("\nUnable to open the current Map for read.");
            }

            return false;
        }
    }
}
