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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

// CreateMapTopology.cs

using System;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Gis.Map;
using Autodesk.Gis.Map.Topology;

namespace CH07
{
	/// <summary>
	/// MapTopologyCreator
	/// </summary>
	public class MapTopologyCreator
	{
        /// <summary>
        /// Creates a new Topology from users input of the name, description, type
        /// and selected entities in the drawing file
        /// </summary>
        public void CreateMapTopology()
        {
	        string name = "";
	        string description = "";
	        string typeName = "";

	        //Get the name, description, type string.        	
	        PromptResult promptResult = Utility.AcadEditor.GetString("\nEnter the Topology name:");
	        if (promptResult.Status == PromptStatus.Cancel)
		        return;
	        name = promptResult.StringResult;
        	
	        promptResult = Utility.AcadEditor.GetString("\nEnter the new Topology Description:");
	        if (promptResult.Status == PromptStatus.Cancel)
		        return;
	        description = promptResult.StringResult;

			PromptKeywordOptions options = new PromptKeywordOptions("\nEnter the new Topology Type: ");
			options.Keywords.Add("Node");
			options.Keywords.Add("netWork");
			options.Keywords.Add("Polygon");
			options.Keywords.Default = "Polygon";
	        promptResult = Utility.AcadEditor.GetKeywords(options);
	        if (promptResult.Status == PromptStatus.Cancel)
		        return;
	        typeName = promptResult.StringResult;

	        TopologyTypes topoType;
	        //Parse the type
			if (string.Compare(typeName, "Polygon", true) == 0)
			{
				topoType = TopologyTypes.Polygon;
			}
			else if (string.Compare(typeName, "Node", true) == 0)
			{
				topoType = TopologyTypes.Point;
			}
			else if (string.Compare(typeName, "Network", true) == 0)
			{
				topoType = TopologyTypes.Linear;
			}
			else 
			{	
				Utility.AcadEditor.WriteMessage("\nInvalid Topology type.");
				return;
			}

	        // Get the selection set according to the type
	        ObjectIdCollection polygonCentroidCollection = new ObjectIdCollection();
	        ObjectIdCollection linkCollection = new ObjectIdCollection();
	        ObjectIdCollection nodeCollection = new ObjectIdCollection();

	        try
	        {
		        switch (topoType)
		        {
		        case TopologyTypes.Polygon:
                    Utility.AcadEditor.WriteMessage("\nSelect centroids to add to the new topology: ");
                    HelperFunctions.SelectIds(polygonCentroidCollection);
                    Utility.AcadEditor.WriteMessage("\nSelect links to add to the new topology: ");
                    HelperFunctions.SelectIds(linkCollection);
                    Utility.AcadEditor.WriteMessage("\nSelect nodes to add to the new topology: ");
                    HelperFunctions.SelectIds(nodeCollection);
                    break;
                case TopologyTypes.Linear:
                    Utility.AcadEditor.WriteMessage("\nSelect links to add to the new topology: ");
                    HelperFunctions.SelectIds(linkCollection);
                    Utility.AcadEditor.WriteMessage("\nSelect nodes to add to the new topology: ");
                    HelperFunctions.SelectIds(nodeCollection);
                    break;
                case TopologyTypes.Point:
			        Utility.AcadEditor.WriteMessage("\nSelect nodes to add to the new topology: ");
                    HelperFunctions.SelectIds(nodeCollection);
                    break;
		        }
	        }
	        catch (Autodesk.AutoCAD.Runtime.Exception expt)
	        {
		        Utility.AcadEditor.WriteMessage(string.Format("\nAutodesl.AutoCAD.Runtime.Exception throwed out containing the error status: {0}", expt.ErrorStatus));
		        return;
	        }
	        CreateMapTopology(name, description, topoType, polygonCentroidCollection, linkCollection, nodeCollection);
        }

        /// <summary>
        /// Creates a new Topology from users input of the name, description, type
        /// and selected entities in the drawing file
        /// </summary>
        /// <param name="name">[in] Topology name.</param>
        /// <param name="description">[in] Topology description.</param>
        /// <param name="topologyType">[in] Topology type.</param>
        /// <param name="polygonsCentroIdCollection">[in] Collection of Polygons Centroids' Object ID.</param>
        /// <param name="linkCollection">[in] Collection of Links' Object ID.</param>
        /// <param name="nodeCollection">[in] Collection of Nodes' Object ID.</param>
        private void CreateMapTopology(string name, 
            string description, 
            TopologyTypes topologyType,
            ObjectIdCollection polygonsCentroIdCollection, 
            ObjectIdCollection linkCollection,
            ObjectIdCollection nodeCollection)
        {
	        TopologyModel topo = null; 
	        MapApplication mapApp = HostMapApplicationServices.Application;
	        Topologies topos = mapApp.ActiveProject.Topologies;
	        try
	        {		
		        topos.Create(name, linkCollection, nodeCollection, polygonsCentroIdCollection, topologyType);
		        topo = topos[name];
		        topo.Open(Autodesk.Gis.Map.Topology.OpenMode.ForWrite);
		        topo.Description = description;
				topo.Close();
	        }
	        catch (MapException expt)
	        {
		        Utility.AcadEditor.WriteMessage(string.Format("\nException throwed containing the error code: {0}", expt.ErrorCode));
	        }
        }

        /// <summary>
        /// Allows a user to change the name of an existing Topology
        /// </summary>
        public void ChangeTopologyName()
        {
            // Get the current name of the topology to be changed
	        string currentName = "";
	        PromptResult promptResult = Utility.AcadEditor.GetString("\nEnter the current Topology name:");
	        if (promptResult.Status == PromptStatus.OK)
	        {
		        currentName = promptResult.StringResult;
        	
		        MapApplication mapApp = HostMapApplicationServices.Application;
		        Topologies topos = mapApp.ActiveProject.Topologies;
		        // Does the topology exist to get information from
		        if (topos.Exists(currentName))
		        {   
			        // Now get the new name for the topology
			        string newName = "";
			        promptResult = Utility.AcadEditor.GetString("\nEnter the new Topology name:");
			        if (promptResult.Status == PromptStatus.OK)
			        {
				        newName = promptResult.StringResult;
        			
				        // Change the topology name
				        try 
				        {
					        topos.Rename(currentName, newName);
				        }
				        catch(MapException expt)
				        {		
					        Utility.AcadEditor.WriteMessage(string.Format("\nFailed to change Topology name containing the error code: {0}", expt.ErrorCode));
				        }
			        }
			        else
			        {
				        Utility.AcadEditor.WriteMessage("\nERROR: Invalid Topology name");
				        return;
			        }
		        }
		        else
		        {
			        Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Topology {0} doesn't exist.", currentName));
			        return;
		        }
	        }
	        else
	        {
		        Utility.AcadEditor.WriteMessage("\nERROR: Invalid Topology name");
		        return;
	        }
        }

        /// <summary>
        /// Allows a user to change the description of an existing Topology
        /// </summary>
        public void ChangeTopologyDescription()
        {
            // Get the name of the topology 
	        string currentName = "";
	        PromptResult promptResult = Utility.AcadEditor.GetString("\nEnter the current Topology name:");
	        if (promptResult.Status != PromptStatus.OK)
	        {	
		        Utility.AcadEditor.WriteMessage("\nERROR: Invalid Topology name");
		        return;
	        }
	        currentName = promptResult.StringResult;

	        // Get the Topology object for the specified Topology
	        TopologyModel mapTopology = null;
        	
            MapApplication mapApp = HostMapApplicationServices.Application;
            Topologies topos = mapApp.ActiveProject.Topologies;
            // Does the topology exist to get information from
            if (topos.Exists(currentName))
            {
                string newDesc = "";
                promptResult = Utility.AcadEditor.GetString("\nEnter the Topology description:");
                if (promptResult.Status == PromptStatus.OK)
                {
                    try
                    {
                        newDesc = promptResult.StringResult;
                    
                        // Open the topology for Write
                        mapTopology = topos[currentName];
                        try
                        {
                            mapTopology.Open(Autodesk.Gis.Map.Topology.OpenMode.ForWrite);
                        }
                        catch (MapException e)
                        {
                            Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to open Topology {0} for write with error code: {1}.", currentName, e.ErrorCode));
                            return;
                        }

                        try
                        {
                            mapTopology.Description = newDesc;
                        }
                        catch (MapException e)
                        {
                            Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to set Topology {0}'s description with error code: {1}.", currentName, e.ErrorCode));
                            return;
                        }
                    }
                    finally
                    {
                        if (null != mapTopology)
                        {
                            mapTopology.Close();
                        }
                    }                
                }
                else
                {
                    Utility.AcadEditor.WriteMessage("\nERROR: Invalid Topology description");
                    return;
                }
            }
            else
            {
                Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Topology {0} doesn't exist.", currentName));
                return;
            }
        }

        public MapTopologyCreator()
		{
		}
	}
}
