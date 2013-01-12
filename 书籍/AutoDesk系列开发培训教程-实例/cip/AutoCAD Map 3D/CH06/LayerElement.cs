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
// AUTODESK PROVIDES THIS PROGRAM "AS I" AND WITH ALL FAULTS. 
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

// LayerElement.cs

using System;
using System.Collections;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Colors;

using Autodesk.Gis.Map;
using Autodesk.Gis.Map.DisplayManagement;
using Autodesk.Gis.Map.Project;

namespace CH06
{
    public sealed class MyLayerElement
    {
        private MyLayerElement()
        {
        }

        private static MyLayerElement m_Instance = new MyLayerElement();

        public static MyLayerElement Instance
        {
            get
            {
                return m_Instance;
            }
        }

        /// <summary>
        /// Adds an Layer Element to the display manager.
        /// </summary>
        /// <param name="elementName">[in] The name of the Element to be added.</param>
        /// <returns>
        /// Returns true if the element is added.
        /// </returns>
        public bool AddLayerElement(string elementName)
        {
            bool isAdded = false;   
            string message = null;
            try
            {
                // Create the Layer Element 
                LayerElement element = LayerElement.Create();

                // Set the Layer Elements name
                element.Name = elementName;

                // Create the Layer Descriptor
                LayerDataSourceDescriptor descriptor = null; 
                descriptor = LayerDataSourceDescriptor.Create();

                string layerNames = null; 
                PromptResult stringPromptResult = null;

                // Note: this is a comma delimited string consisting of layer names wanted  
                stringPromptResult = Utility.Editor.GetString("\nSpecify the Element's Layer: ");

                if (stringPromptResult.Status == PromptStatus.OK && (layerNames = stringPromptResult.StringResult.Trim()).Length > 0)
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
                }
                else
                {
                    element = null;
                }

                // Now Add the new Element to the current Map
                if (null != element)
                {
                    // Get the Object Id for the current Map
                    ObjectId currentMapId = new ObjectId();
                    if (FindCurrentMapId(ref currentMapId))
                    {
                        using (Transaction trans = Utility.TransactionManager.StartTransaction())
                        {
                            // Open the Map for write
                            Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForWrite);

                            System.Collections.IEnumerator iterator = currentMap.NewIterator(true, true);

                            // Add the new element
                            ObjectId elementId = new ObjectId();
                            try
                            {
                                elementId = currentMap.AddItem(element, iterator);  
                                trans.AddNewlyCreatedDBObject(element, true);

                                if (null != descriptor)
                                {
                                    element = (LayerElement)trans.GetObject(elementId, OpenMode.ForWrite);
                                    element.AcquisitionCriteria = descriptor;
                                }

                                // Print out result
                                message = string.Format("\nElement {0} was sucessfully added.", elementName);
                                Utility.ShowMessage(message);
                                isAdded = true;

                                trans.Commit();
                            }
                            catch (Autodesk.AutoCAD.Runtime.Exception e)
                            {
                                message = string.Format("\nFailed to add Element {0}, Error code: {1}.", elementName, e.ErrorStatus);
                                Utility.ShowMessage(message);
                            }
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
        /// Moves any Element up or down in priority on the display manager.
        /// </summary>
        /// <param name="elementName">[in] The name of the Element to be moved.</param>
        /// <returns>
        /// Returns true if the new Element is moved.
        /// </returns>
        public bool MoveElement(string elementName)
        {
            bool isMoved = false;

            // Get the Object Id for the current Map
            ObjectId currentMapId = new ObjectId();
            if (!FindCurrentMapId(ref currentMapId))
            {
                return isMoved;
            }

            // Find the object Id of the element to be moved
            ObjectId elementIdTobeMoved = new ObjectId();
            if (!FindElementId(ref elementIdTobeMoved, elementName))
            {
                return isMoved;
            }

            string name = null; 
            PromptResult stringPromptResult = null;

            // Note you could also do this by index or strictly by position
            stringPromptResult = Utility.Editor.GetString("\nEnter the Name you want to move in front of.");
            if (stringPromptResult.Status == PromptStatus.OK 
                && (name = stringPromptResult.StringResult.Trim()).Length > 0)
            {
                // Find the object Id of the element to be bumped
                ObjectId elementId = new ObjectId();
                if (FindElementId(ref elementId, name))
                {
                    try
                    {
                        using (Transaction trans = Utility.TransactionManager.StartTransaction())
                        {
                            Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForWrite);

                            // Get the position to move the element to
                            IEnumerator iterator = currentMap.NewIterator(true, true);
							while (iterator.MoveNext())
							{
								if ( (ObjectId)iterator.Current == elementId )
									break;
							}
                            // Set the position
                            IEnumerator pos = iterator;
                            // Move the element
                            currentMap.MoveItem(elementIdTobeMoved, pos);
                            trans.Commit();
                            isMoved = true;
                        }
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception)
                    {
                        Utility.ShowMessage("\nFailed to move element.");
                    }
                }
            }
            else
            {
                Utility.ShowMessage("\nERROR: Invalid Element name");
            }

            return isMoved;
        }

        /// <summary>
        /// Removes any Element from the display manager.
        /// </summary>
        /// <param name="elementName"> Input: The name of the Element to be Moved.</param>
        /// <returns>
        /// Returns true if the Element is removed.
        /// </returns>
        public bool RemoveElement(string elementName)
        {
            bool isRemoved = false;
            string message = null;

            // Get the Object Id for the current Map
            ObjectId currentMapId = new ObjectId();
            if (!FindCurrentMapId(ref currentMapId))
            {
                return isRemoved;
            }

            // Find the object id of the element
            ObjectId elementId = new ObjectId();
            if (FindElementId(ref elementId, elementName))
            {
                // Open the map for write
                try
                {
                    using (Transaction trans = Utility.TransactionManager.StartTransaction())
                    {
                        Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForWrite);
                        try
                        {
                            // Remove the element
                            currentMap.RemoveItem(elementId);
                            trans.Commit();

                            message = string.Format("\nElement {0} was removed.", elementName);
                            Utility.ShowMessage(message);

                            isRemoved = true;              
                        }
                        catch (Autodesk.AutoCAD.Runtime.Exception e)
                        {
                            message = string.Format("\nUnable to remove Element {0}, Error code: {1}.", elementName, e.ErrorStatus);
                            Utility.ShowMessage(message);
                        }    
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception)
                {
                    Utility.ShowMessage("\nUnable to open the current Map for write.");
                }
            }
            return isRemoved;
        }

        /// <summary>
        /// Adds an Entity Style to an existing Element.
        ///  Other types of styles include Annotation, Hatch or Text
        /// </summary>  
        /// <returns>
        /// Returns true if the Style is added.
        /// </returns>
        public bool AddNewStyleToElement(string elementName)
        {
            bool isAdded = false;
            string newStyleName = null; 
            PromptResult stringPromptResult = null;

            // Create an entity type style
            EntityStyle styleEntity = EntityStyle.Create();

            stringPromptResult = Utility.Editor.GetString("\nEnter name of the New Entity Style: ");
            newStyleName = stringPromptResult.StringResult;

            if (stringPromptResult.Status == PromptStatus.OK && newStyleName.Trim().Length > 0)
            {
                newStyleName = newStyleName.Trim();

                // Set the new style's Color, currently hard coded to blue for sample only
                Color color = Color.FromColorIndex(ColorMethod.None, 5);
                styleEntity.Color = color;

                // Set the style name
                styleEntity.Name = newStyleName;
            }
            else
            {
                styleEntity = null;
            }

            // Add the style to the element
            if (null != styleEntity)
            {
                AddStyle(styleEntity, elementName);
            }
            else
            {
                Utility.ShowMessage("\nERROR: Invalid Style name");
            }

            return isAdded;
        }

        /// <summary>
        /// Adds an existing Entity type Style from a Category to an existing Element.
        ///  Other types of styles include Annotation, Hatch or Text
        /// </summary>  
        /// <returns>
        /// Returns true if the Style is added.
        /// </returns>
        public bool AddExistingStyleToElement(string elementName)
        {
            bool isAdded = false;
            string message = null;

            // Get the project associated with the current AutoCAD document
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;
            project = app.ActiveProject;

            string styleName = null;
            bool isStyleExisting = false;
            // Create an entity type style
            EntityStyle styleTobeAdded = EntityStyle.Create();

            // Get the Style Library
            StyleLibrary library = null;
            ObjectId libraryId = new ObjectId();
  
            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    libraryId = DisplayManager.Create(project).StyleLibraryId(project);
                    library = (StyleLibrary)trans.GetObject(libraryId, OpenMode.ForWrite);
          
                    PromptIntegerResult integerPromptResult = null;

                    // Find the category index for the style to be moved
                    int categoryIndex = -1;

                    integerPromptResult = Utility.Editor.GetInteger("\nEnter the category index where the style is located: ");
                    if (integerPromptResult.Status == PromptStatus.OK)
                    {
                        categoryIndex = integerPromptResult.Value;
                        if (categoryIndex < 0 || categoryIndex >= library.CategoryCount)
                        {
                            trans.Commit();
                            message = string.Format("\nCategory index {0} is an invalid index.", categoryIndex);
                            Utility.ShowMessage(message);
                            return isAdded;
                        }
                    }
                    else
                    {
                        trans.Commit();
                        return isAdded;
                    }
                    // Get the category for the style to be moved
                    StyleCategory category = null;
                    ObjectId categoryId = new ObjectId();

                    categoryId = library.GetAt(categoryIndex);
                    category = (StyleCategory)trans.GetObject(categoryId, OpenMode.ForWrite);

                    // Get the style name to be added
                    Style existStyle = null;
                    ObjectId styleId = new ObjectId();

                    PromptResult stringPromptResult = null;
                    stringPromptResult = Utility.Editor.GetString("\nEnter the Style name: ");
                    styleName = stringPromptResult.StringResult;

                    if (stringPromptResult.Status == PromptStatus.OK && styleName.Trim().Length > 0)
                    {
                        styleName = styleName.Trim();

                        // Loop through the category until we find the style
                        for (int index = 0; index < category.StyleCount; ++index)
                        {  
                            styleId = category.GetAt(index);
                            existStyle = (Style)trans.GetObject(styleId, OpenMode.ForRead);

                            string currentStyleName = existStyle.Name;            
                            if (styleName.Equals(currentStyleName)
                                && (existStyle.GetType().Equals(typeof(EntityStyle)) || existStyle.GetType().IsSubclassOf(typeof(EntityStyle))))
                            {                
                                isStyleExisting = true;
                                // create a copy of the entity existStyle
                                styleTobeAdded.CopyFrom(existStyle);              
                                break;
                            }
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                Utility.ShowMessage(e.Message);
            }

            // Add the style to the element
            if (null != styleTobeAdded)
            {
                if (isStyleExisting)
                {
                    AddStyle(styleTobeAdded, elementName);
                }
                else
                {
                    Utility.ShowMessage(string.Format("\nThe style {0} doesn't exist.", styleName));
                }
            }
            else
            {
                Utility.ShowMessage("\nERROR: Invalid Style name");
            }

            return isAdded;
        }

        /// <summary>
        /// Adds a Style to an existing Element.
        /// </summary>  
        /// <returns>
        /// Returns true if the Style is added.
        /// </returns>
        private bool AddStyle(Style style, string elementName)
        {
            string message = null;

            // Find the object Id of the element 
            ObjectId elementId = new ObjectId();
            if (FindElementId(ref elementId, elementName))
            {      
                try
                {
                    using (Transaction trans = Utility.TransactionManager.StartTransaction())
                    {
                        // Open the element for write, so the style can be added
                        Element element = (Element)trans.GetObject(elementId, OpenMode.ForWrite);

                        // pass 0.0 for the current scale
                        StyleReferenceIterator styleRefIterator = element.GetStyleReferenceIterator(0.0, true, true);

                        // Add the style
                        ObjectId id = new ObjectId();
                        id = element.AddStyle(style, styleRefIterator);
                        trans.AddNewlyCreatedDBObject(style, true);
                        Utility.ShowMessage("\nStyle has been sucessfully added.");

                        trans.Commit();
                        return true;
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception e)
                {
                    message = string.Format("\nFailed to add style, Error code: {0}.", e.ErrorStatus);
                    Utility.ShowMessage(message);
                }
                catch (System.Exception e)
                {
                    Utility.ShowMessage(e.Message);
                }
            }

            return false;
        }

        /// <summary>
        /// Finds the object id for the current Map.
        /// </summary>
        /// <param name="currentMapId">[out] The object id of the Element.</param>  
        /// <returns>
        /// Returns true If the Map Object Id is found.
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
                    ObjectId managerId = new ObjectId();
                    MapManager manager = null;

                    managerId = DisplayManager.Create(project).MapManagerId(project, true);
                    manager = (MapManager)trans.GetObject(managerId, OpenMode.ForRead);

                    currentMapId = manager.CurrentMapId;
                    trans.Commit();
                    isFound = true;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                Utility.ShowMessage("\nUnable to get the current Map's Object ID.");
            }
            return isFound;
        }

        /// <summary>
        /// Finds the object id for the element specified.
        /// Note: this only works for top level elements, it does not iterate through any sub groups
        /// </summary>
        /// <param name="elementIdToBeMoved">[out] The object id of the Element.</param>
        /// <param name="elementName">[in] The name of the Element to be Found.</param>
        /// <returns>
        /// Returns true if the Element is found.
        /// </returns>
        private bool FindElementId(ref ObjectId elementId, string elementName)
        {
            bool isFound = false;

            // Get the Object Id for the current Map
            ObjectId currentMapId = new ObjectId();
            if (!FindCurrentMapId(ref currentMapId))
            {
                return isFound;
            }
            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    Map currentMap = (Map)trans.GetObject(currentMapId, OpenMode.ForRead);

                    // Get the Map Iterator    
                    IEnumerator iterator = currentMap.NewIterator(true, true);

                    // Loop through the iter until we find the Element
                    while (iterator.MoveNext())
                    {
                        ObjectId itemId = (ObjectId)iterator.Current;
                        Item item = (Item)trans.GetObject(itemId, OpenMode.ForRead); 

                        if (item.GetType().Equals(typeof(Element))
                            || (item.GetType()).IsSubclassOf(typeof(Element)))
                        {
                            if (item.Name.Equals(elementName))
                            {            
                                // Get the element id
                                elementId = item.ObjectId;
                                isFound = true;
                                break;
                            }
                        }
                    } 

                    // Check to see if we found the elements object id
                    if (!isFound)
                    {
                        string message = string.Format("\nUnable to find the object ID for element {0}.", elementName);
                        Utility.ShowMessage(message);
                    }

                    trans.Commit();
                }      
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                Utility.ShowMessage("\nUnable to open the current Map for read.");
            }
      
            return isFound;
        }
    }
}
