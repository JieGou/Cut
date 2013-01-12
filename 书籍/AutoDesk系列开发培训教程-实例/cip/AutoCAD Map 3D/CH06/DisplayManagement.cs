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

// DisplayManagement.cs

using System;
using System.Collections;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.Gis.Map;
using Autodesk.Gis.Map.Project;
using Autodesk.Gis.Map.DisplayManagement;

namespace CH06
{
	public sealed class DisplayManagerSampleCommand
	{
		private static ArrayList m_eventedProjects = null;
		private static ArrayList EventedProjects
		{
			get
			{
				if (m_eventedProjects == null)
					m_eventedProjects = new ArrayList();
				return m_eventedProjects;
			}
		}

		private DisplayManagerSampleCommand()
		{
		}
             
		/// <summary>
		/// List the Commands this sample provided.
		/// </summary>
		[Autodesk.AutoCAD.Runtime.CommandMethodAttribute("CmdList")]
		public static void CmdList()
		{
			Utility.ShowMessage("\n DisplayManager Sample Commands : \n");
			Utility.ShowMessage("** Cmd : AddMap \n");
			Utility.ShowMessage("** Cmd : RemoveMap \n");
			Utility.ShowMessage("** Cmd : SetMapAsCurrent \n");
			Utility.ShowMessage("** Cmd : AddScale \n");
			Utility.ShowMessage("** Cmd : RemoveScale \n");
			Utility.ShowMessage("** Cmd : SetScaleAsCurrent \n");
            Utility.ShowMessage("** Cmd : AddCategory \n");
            Utility.ShowMessage("** Cmd : AddStyleToCategory \n");
            Utility.ShowMessage("** Cmd : RemoveStyleFromCategory \n");
            Utility.ShowMessage("** Cmd : AddLayerElement \n");
            Utility.ShowMessage("** Cmd : MoveElement \n");
           Utility.ShowMessage("** Cmd : RemoveElement \n");
            Utility.ShowMessage("** Cmd : AddNewStyleToElement \n");
            Utility.ShowMessage("** Cmd : AddExistingStyleToElement \n");
            Utility.ShowMessage("** Cmd : RegisterProjectEvents \n");
            Utility.ShowMessage("** Cmd : UnregisterProjectEvents \n");
            Utility.ShowMessage("** Cmd : AddGroup \n");
            Utility.ShowMessage("** Cmd : RemoveGroup \n");
            Utility.ShowMessage("** Cmd : AddElementToGroup \n");
            Utility.ShowMessage("** Cmd : CreateLegend \n");
		}

		[Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddMap")]
		public static void CommandAddMap()
		{
			try
			{
				string mapName = null;
				PromptResult stringPromptResult = null;
				stringPromptResult = Utility.Editor.GetString("\nEnter the new Map's name:");

				if (stringPromptResult.Status == PromptStatus.OK && (mapName = stringPromptResult.StringResult.Trim()).Length > 0)
				{
					MyDisplayManager.Instance.AddMap(mapName);      
				}
				else
				{
					Utility.ShowMessage("\nERROR: Invalid Map name");
				}
			}
			catch (System.Exception err)
			{
				Utility.Editor.WriteMessage(err.Message);
			}
		}

		[Autodesk.AutoCAD.Runtime.CommandMethodAttribute("RemoveMap")]
		public static void CommandRemoveMap()
		{
			try
			{
				string mapName = null;
				PromptResult stringPromptResult = null;
				stringPromptResult = Utility.Editor.GetString("\nEnter the Map's name to be removed:");

				if (stringPromptResult.Status == PromptStatus.OK 
					&& (mapName = stringPromptResult.StringResult.Trim()).Length > 0)
				{
					MyDisplayManager.Instance.RemoveMap(mapName);      
				}
				else
				{
					Utility.ShowMessage("\nERROR: Invalid Map name");
				}  
			}
			catch (System.Exception err)
			{
				Utility.Editor.WriteMessage(err.Message);
			}
		}

		[Autodesk.AutoCAD.Runtime.CommandMethodAttribute("SetMapAsCurrent")]
		public static void CommandSetMapAsCurrent()
		{
			try
			{
				string mapName = null;
				PromptResult stringPromptResult = null;
				stringPromptResult = Utility.Editor.GetString("\nEnter the Map's name you want to be current:");

				if (stringPromptResult.Status == PromptStatus.OK 
					&& (mapName = stringPromptResult.StringResult).Length > 0)
				{
					MyDisplayManager.Instance.SetMapAsCurrent(mapName);      
				}
				else
				{
					Utility.ShowMessage("\nERROR: Invalid Map name");
				}
			}
			catch (System.Exception err)
			{
				Utility.Editor.WriteMessage(err.Message);
			}
		}

		[Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddScale")]
		public static void CommandAddScale()
		{  
			try
			{        
				MyDisplayManager.Instance.AddScale();
			}
			catch (System.Exception err)
			{
				Utility.Editor.WriteMessage(err.Message);
			}
		}

		[Autodesk.AutoCAD.Runtime.CommandMethodAttribute("RemoveScale")]
		public static void CommandRemoveScale()
		{  
			try
			{        
				MyDisplayManager.Instance.RemoveScale();
			}
			catch (System.Exception err)
			{
				Utility.Editor.WriteMessage(err.Message);
			}
		}

		[Autodesk.AutoCAD.Runtime.CommandMethodAttribute("SetScaleAsCurrent")]
		public static void CommandSetScaleAsCurrent()
		{      
			try
			{
				MyDisplayManager.Instance.SetScaleAsCurrent();
			}
			catch (System.Exception err)
			{
				Utility.Editor.WriteMessage(err.Message);
			}
		}

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddCategory")]
        public static void CommandAddNewCategory()
        {
            try
            {
                string categoryName = null;
                PromptResult stringPromptResult = null;
                stringPromptResult = Utility.Editor.GetString("\nEnter the new Category's name:");
                categoryName = stringPromptResult.StringResult;

                if (stringPromptResult.Status == PromptStatus.OK && categoryName.Trim().Length > 0)
                {
                    MyDisplayLibrary.Instance.AddNewCategory(categoryName.Trim());
                }
                else
                {
                    Utility.ShowMessage("\nERROR: Invalid Category name");
                }
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("RemoveCategory")]
        public static void CommandRemoveCategory()
        {
            try
            {
                MyDisplayLibrary.Instance.RemoveCategory();
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddStyleToCategory")]
        public static void CommandAddStyleToCategory()
        {
            try
            {
                MyDisplayLibrary.Instance.AddStyleToCategory();
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("RemoveStyleFromCategory")]
        public static void CommandRemoveStyleFromCategory()
        {
            try
            {
                MyDisplayLibrary.Instance.RemoveStyleFromCategory();
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddLayerElement")]
        public static void CommandAddLayerElement()
        {
            try
            {
                string elementName = null;
                PromptResult stringPromptResult = null;
                stringPromptResult = Utility.Editor.GetString("\nSpecify the new Layer element name: ");
                elementName = stringPromptResult.StringResult;

                if (stringPromptResult.Status == PromptStatus.OK
                    && elementName.Trim().Length > 0)
                {
                    MyLayerElement.Instance.AddLayerElement(elementName.Trim());
                }
                else
                {
                    Utility.ShowMessage("\nERROR: Invalid Element name");
                }
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("MoveElement")]
        public static void CommandMoveElement()
        {
            try
            {
                string elementName = null;
                PromptResult stringPromptResult = null;
                stringPromptResult = Utility.Editor.GetString("\nEnter the Element's name you want to move:");
                elementName = stringPromptResult.StringResult;

                if (stringPromptResult.Status == PromptStatus.OK && elementName.Trim().Length > 0)
                {
                    MyLayerElement.Instance.MoveElement(elementName.Trim());
                }
                else
                {
                    Utility.ShowMessage("\nERROR: Invalid Element name");
                }
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("RemoveElement")]
        public static void CommandRemoveElement()
        {
            try
            {
                string elementName = null;
                PromptResult stringPromptResult = null;
                stringPromptResult = Utility.Editor.GetString("\nEnter the Element's name you want to Remove:");
                elementName = stringPromptResult.StringResult;

                if (stringPromptResult.Status == PromptStatus.OK && elementName.Trim().Length > 0)
                {
                    MyLayerElement.Instance.RemoveElement(elementName.Trim());
                }
                else
                {
                    Utility.ShowMessage("\nERROR: Invalid Element name");
                }
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddNewStyleToElement")]
        public static void CommandAddNewStyleToElement()
        {
            try
            {
                string elementName = null;
                PromptResult stringPromptResult = null;
                stringPromptResult = Utility.Editor.GetString("\nEnter the Element's name you want to add the style to:");
                elementName = stringPromptResult.StringResult;

                if (stringPromptResult.Status == PromptStatus.OK && elementName.Trim().Length > 0)
                {
                    MyLayerElement.Instance.AddNewStyleToElement(elementName.Trim());
                }
                else
                {
                    Utility.ShowMessage("\nERROR: Invalid Element name");
                }
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddExistingStyleToElement")]
        public static void CommandAddExistingStyleToElement()
        {
            try
            {
                string elementName = null;
                PromptResult stringPromptResult = null;
                stringPromptResult = Utility.Editor.GetString("\nEnter the Element's name you want to add the style to:");
                elementName = stringPromptResult.StringResult;

                if (stringPromptResult.Status == PromptStatus.OK && elementName.Trim().Length > 0)
                {
                    MyLayerElement.Instance.AddExistingStyleToElement(elementName.Trim());
                }
                else
                {
                    Utility.ShowMessage("\nERROR: Invalid Element name");
                }
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }



        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddGroup")]
        public static void CommandAddNewGroup()
        {
            try
            {
                string groupName = null;
                PromptResult stringPromptResult = null;
                stringPromptResult = Utility.Editor.GetString("\nSpecify the new Group's name: ");
                groupName = stringPromptResult.StringResult;

                if (stringPromptResult.Status == PromptStatus.OK && groupName.Trim().Length > 0)
                {
                    MyGroup.Instance.AddNewGroup(groupName.Trim());
                }
                else
                {
                    Utility.ShowMessage("\nERROR: Invalid Group name");
                }
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("RemoveGroup")]
        public static void CommandRemoveGroup()
        {
            try
            {
                string groupName = null;
                PromptResult stringPromptResult = null;
                stringPromptResult = Utility.Editor.GetString("\nSpecify the Group to be removed: ");
                groupName = stringPromptResult.StringResult;

                if (stringPromptResult.Status == PromptStatus.OK && groupName.Trim().Length > 0)
                {
                    MyGroup.Instance.RemoveGroup(groupName.Trim());
                }
                else
                {
                    Utility.ShowMessage("\nERROR: Invalid Group name");
                }
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddElementToGroup")]
        public static void CommandAddElementToGroup()
        {
            try
            {
                string groupName = null;
                PromptResult stringPromptResult = null;
                stringPromptResult = Utility.Editor.GetString("\nSpecify the Group name that you want to add the element to: ");
                groupName = stringPromptResult.StringResult;

                if (stringPromptResult.Status == PromptStatus.OK && groupName.Trim().Length > 0)
                {
                    MyGroup.Instance.AddElementToGroup(groupName.Trim());
                }
                else
                {
                    Utility.ShowMessage("\nERROR: Invalid Group name");
                }
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("CreateLegend")]
        public static void CommandCreateLegend()
        {
            try
            {
                MyLegend.Instance.CreateLegend();
            }
            catch (System.Exception err)
            {
                Utility.Editor.WriteMessage(err.Message);
            }
        }
   }

	public sealed class DisplayManagerSampleApplication : IExtensionApplication
	{
		public void Initialize()
		{
			Utility.ShowMessage("\n DisplayManager Sample Application initialized. \n");
		}

		public void Terminate()
		{
		}
	}
}
