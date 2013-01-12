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

// DisplayLibrary.cs

using System;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Colors;

using Autodesk.Gis.Map;
using Autodesk.Gis.Map.DisplayManagement;
using Autodesk.Gis.Map.Project;

namespace CH06
{
    public sealed class MyDisplayLibrary
    {
        private MyDisplayLibrary()
        {
        }

        private static MyDisplayLibrary m_Instance = new MyDisplayLibrary();

        public static MyDisplayLibrary Instance
        {
            get
            {
                return m_Instance;
            }
        }

        /// <summary>
        /// Adds a New Category to the Display Library.
        /// </summary>
        /// <param name="categoryName">[in] The name of the new Category.</param>
        /// <returns>
        /// Returns true if the New Category is added successfully.
        /// </returns>
        public bool AddNewCategory(string categoryName)
        {
            bool isAdded = false;

            // Get the project associated with the current AutoCAD document
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;

            project = app.ActiveProject;

            // Get the Style Library
            StyleLibrary library = null;
            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    ObjectId libraryId = DisplayManager.Create(project).StyleLibraryId(project);
                    library = (StyleLibrary)trans.GetObject(libraryId, OpenMode.ForWrite);

                    // Create a new category
                    StyleCategory category = StyleCategory.Create();
                    category.Name = categoryName;

                    // Add it to the style Library
                    ObjectId id = new ObjectId();    
                    id = library.Append(category);
                    trans.AddNewlyCreatedDBObject(category, true);

                    trans.Commit();

                    string message = string.Format("\nNew category {0} has been sucessfully added.", categoryName);
                    Utility.ShowMessage(message);
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                string message = string.Format("\nFailed to add new category {0}. Error code: {1}.", categoryName, e.ErrorStatus);
                Utility.ShowMessage(message);
            }

            return isAdded;
        }

        /// <summary>
        /// Removes a Category from the Display Library.
        /// </summary>
        /// <param name="categoryName"> [in] The name of the Category to be removed.</param>
        /// <returns>
        /// Returns true if the Category is removed successfully.
        /// </returns>
        public bool RemoveCategory()
        {
            bool isRemoved = false;
            string message = null;
            // Get the project associated with the current AutoCAD document
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;
            project = app.ActiveProject;
            StyleLibrary library = null;
            int index = -1;
            try
            {  
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    ObjectId libraryId = DisplayManager.Create(project).StyleLibraryId(project);
                    library = (StyleLibrary)trans.GetObject(libraryId, OpenMode.ForWrite);
                    PromptIntegerResult integerPromptResult = null;

                    // Get the old location (index) for the category
                    integerPromptResult = Utility.Editor.GetInteger("\nEnter the category index you want to move: ");
                    if (integerPromptResult.Status == PromptStatus.OK)
                    {
                        index = integerPromptResult.Value;
                        if (index < 0 || index >= library.CategoryCount)
                        {
                            message = string.Format("\nCategory index {0} is invalid.", index);
                            Utility.ShowMessage(message);
                            return isRemoved;
                        }
                    }
                    else
                    {
                        return isRemoved;
                    }

                    // Remove the category
                    library.RemoveAt(index);
                    trans.Commit();
                    message = string.Format("\nCategory at index {0} has been sucessfully removed.", index);

                    Utility.ShowMessage(message);
                    isRemoved = true;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                message = string.Format("\nCategory at index {0} has not been removed.", index);
                Utility.ShowMessage(message);
            }

            return isRemoved;
        }

        /// <summary>
        /// Adds a new Style to a Category in the display Library.
        /// </summary>
        /// <returns>
        /// Returns true if the new Style is added.
        /// </returns>
        public bool AddStyleToCategory()
        {
            bool isAdded = false;
            string message = null;
            // Get the project associated with the current AutoCAD document
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;
            project = app.ActiveProject;

            StyleLibrary library = null;
            try
            {
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    // Get the current style Library
                    ObjectId libraryId = DisplayManager.Create(project).StyleLibraryId(project);
                    library = (StyleLibrary)trans.GetObject(libraryId, OpenMode.ForWrite);
                    PromptResult stringPromptResult = null;
                    PromptIntegerResult integerPromptResult = null;

                    int index = -1;
                    integerPromptResult = Utility.Editor.GetInteger("\nEnter the category index you want to add a style to: ");
                    if (integerPromptResult.Status == PromptStatus.OK)
                    {
                        index = integerPromptResult.Value;
                        if (index < 0 || index >= library.CategoryCount)
                        {
                            message = string.Format("\nCategory index {0} is invalid.", index);
                            Utility.ShowMessage(message);
                            return isAdded;
                        }
                    }
                    else
                    {
                        return isAdded;
                    }

                    stringPromptResult = Utility.Editor.GetString("\nEnter name of the New Style: ");
                    if (stringPromptResult.Status == PromptStatus.OK && stringPromptResult.StringResult.Trim().Length > 0)
                    {
                        // Create an entity type style
                        EntityStyle styleEntity = EntityStyle.Create();
                        // Set the new style's Color, currently hard coded to blue for sample only
                        Color color = Color.FromColorIndex(ColorMethod.None, 5);
                        styleEntity.Color = color;

                        // Set the style name
                        string styleEntityName = stringPromptResult.StringResult.Trim();
                        styleEntity.Name = styleEntityName;

                        // Open the style category for write
                        StyleCategory category = null;
                        try
                        {
                            ObjectId categoryId = library.GetAt(index);
                            category = (StyleCategory)trans.GetObject(categoryId, OpenMode.ForWrite);

                            ObjectId id;
                            try
                            {
                                id = category.Append(styleEntity);
                                trans.AddNewlyCreatedDBObject(styleEntity, true);
                                trans.Commit();

                                message = string.Format("\nThe new style {0} has been sucessfully added in the category.", styleEntityName);
                                Utility.ShowMessage(message);
                                isAdded = true;
                            }
                            catch (Autodesk.AutoCAD.Runtime.Exception e)
                            {
                                message = string.Format("\nFailed to add the new {0} style in the category. Error code: {1}.", styleEntityName, e.ErrorStatus);
                                Utility.ShowMessage(message);
                            }
                        }
                        catch (Autodesk.AutoCAD.Runtime.Exception)
                        {
                            message = string.Format("\nFailed to open the category at index '{0}.'", index);
                            Utility.ShowMessage(message);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Utility.ShowMessage(e.Message);
            }

            return isAdded;
        }

        /// <summary>
        /// Removes a style from a Category in the display Library.
        /// </summary>
        /// <returns>
        /// Returns true if the Style was removed.
        /// </returns>
        public bool RemoveStyleFromCategory()
        {
            bool isRemoved = false;
            string message = null;
            // Get the project associated with the current AutoCAD document
            ProjectModel project = null;
            MapApplication app = HostMapApplicationServices.Application;

            project = app.ActiveProject;

            StyleLibrary library = null;
            try
            {  
                using (Transaction trans = Utility.TransactionManager.StartTransaction())
                {
                    // Get the current style Library
                    ObjectId libraryId = DisplayManager.Create(project).StyleLibraryId(project);
                    library = (StyleLibrary)trans.GetObject(libraryId, OpenMode.ForWrite);

                    PromptIntegerResult integerPromptResult = null;

                    int categoryIndex = -1;

                    integerPromptResult = Utility.Editor.GetInteger("\nEnter the category index where the style is located: ");
                    if (integerPromptResult.Status == PromptStatus.OK)
                    {
                        categoryIndex = integerPromptResult.Value;
                        if (categoryIndex < 0 || categoryIndex >= library.CategoryCount)
                        {
                            message = string.Format("\nCategory index {0} is invalid.", categoryIndex);
                            Utility.ShowMessage(message);
                            return isRemoved;
                        }
                    }
                    else
                    {
                        return isRemoved;
                    }

                    // Get the category for the style to be moved
                    StyleCategory category = null;    
                    try
                    {
                        ObjectId categoryId = library.GetAt(categoryIndex);
                        category = (StyleCategory)trans.GetObject(categoryId, OpenMode.ForWrite);

                        int styleIndex = -1;
                        integerPromptResult = Utility.Editor.GetInteger("\nEnter the index of the style you want to remove:");

                        if (integerPromptResult.Status == PromptStatus.OK)
                        {
                            styleIndex = integerPromptResult.Value;
                            if (styleIndex < 0 || styleIndex >= category.StyleCount)
                            {
                                message = string.Format("\nStyle index {0} is invalid.", styleIndex);
                                Utility.ShowMessage(message);
                                return isRemoved;
                            }
                        }
                        else
                        {
                            return isRemoved;
                        }

                        // Remove the style    
                        try
                        {
                            category.RemoveAt(styleIndex);
                            trans.Commit();
                            message = string.Format("\nStyle at index {0} has been sucessfully removed.", styleIndex);
                            Utility.ShowMessage(message);
                            isRemoved = true;
                        }
                        catch (Autodesk.AutoCAD.Runtime.Exception)
                        {      
                            message = string.Format("\nStyle at index {0} has not been removed.", styleIndex);
                            Utility.ShowMessage(message);
                        }
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception)
                    {
                        message = string.Format("\nFailed to open the category at index '{0}.'", categoryIndex);
                        Utility.ShowMessage(message);
                    }
                }
            }
            catch (System.Exception e)
            {
                Utility.ShowMessage(e.Message);
            }

            return isRemoved;
        }
    }
}
