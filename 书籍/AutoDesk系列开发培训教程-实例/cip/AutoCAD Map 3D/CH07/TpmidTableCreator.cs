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

// TpmidTableCreator.cs

using System;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Gis.Map;
using Autodesk.Gis.Map.Topology;
using Autodesk.Gis.Map.Project;
using Autodesk.Gis.Map.ObjectData;

namespace CH07
{
	/// <summary>
	/// Create the Tpmid Table.
	/// </summary>
	public sealed class TpmidTableCreator
	{
        /// <summary>
        /// Create a table
        /// <summary>
        public void CreateTable()
        {
	        PromptResult promptResult = Utility.AcadEditor.GetString("\nEnter the topology name:");
	        if (promptResult.Status == PromptStatus.Cancel)
            {
		        return;
	        }
        	
	        CreateTable(promptResult.StringResult);
        }

        /// <summary>
        ///    This routine recreates a necessary object data table used 
        ///    when editing topologies. The OD table name has the form:
        ///             TPMID_toponame
        ///    where 'toponame' is the name of a topology. Every topology 
        ///    has this table and the table holds the last element ID used 
        ///    by the topology. The table can be lost by write blocking a
        ///    topology's objects to file.
        ///
        ///    Without this table, it is not possible to edit the geometry 
        ///    of the topology because for new elements the topology engine
        ///    doesn't know  which ID to assign. Use this routine to restore
        ///    the TPMID_toponame OD table to the drawing. 
        /// </summary>
        /// <param name="topologyName">[in] Name of the topology.</param>
        /// <returns>
        /// Returns true if successful.
        /// </returns>
        private bool CreateTable(string topologyName)
        {
	        MapApplication mapApp = HostMapApplicationServices.Application;
	        Topologies topos = mapApp.ActiveProject.Topologies;
        	
	        // Does the topology exist to get information from
	        if (topos.Exists(topologyName))
	        {	        
 		        // Create a object of AcMapTopology;
		        TopologyModel mapTopology = topos[topologyName];

				try 
				{
                    // open the topology for read
                    mapTopology.Open(Autodesk.Gis.Map.Topology.OpenMode.ForRead);
                    int intLastTopoId = FindLastTopoId(mapTopology);
					if (0 != intLastTopoId)			
					{
						// Create the Object Data Table
						string tableName = "";
						tableName = string.Format("TPMID_{0}", topologyName);
						CreateODTable(tableName, intLastTopoId);
					}

					return true;
				}
				catch (MapException error)
				{
					// Deal with the exception here as your will
                    Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Operation failed with error code: {0}.", error.ErrorCode));
                    return false;
				}
				finally
				{
					mapTopology.Close();
				}
	        }
	        else
	        {
		        Utility.AcadEditor.WriteMessage(string.Format("\nERROR: The Topology {0} doesn't exist.", topologyName));
		        return false;
	        }
        }
        
        /// <summary>
        /// Creates the required table TPMID_TopologyName
        /// </summary>
        /// <param name="tableName">[in] Name of the table to be created.</param>
        /// <param name="lastTopoId">[in] The last id for an existing Topology.</param>
        /// <returns>
        /// Returns true if successful.
        /// </returns>
        private bool CreateODTable(string tableName, int lastTopoId)
        {
            MapApplication  mapApp = HostMapApplicationServices.Application;
            ProjectModel proj = mapApp.ActiveProject;
            Tables odTables = proj.ODTables;
				
            if (odTables.IsTableDefined(tableName))
            {
                Utility.AcadEditor.WriteMessage("\nERROR:Table already exists.");
                return false;
            }

            try
            {
                // Create the table
                FieldDefinitions tabDefs = mapApp.ActiveProject.MapUtility.NewODFieldDefinitions();
                FieldDefinition tabDef = FieldDefinition.Create("LAST_ID", "Holds the last element ID used by the topology", lastTopoId);

                // Add the column to the table
                tabDefs.AddColumn(tabDef, -1);

                odTables.Add(tableName, tabDefs, "", true);

                return true;
            }
            catch (MapException error)
            {
                Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Operation failed with error code: {0}.", error.ErrorCode));
                return false;
            }
        }

        /// <summary>
        /// Finds the last id for an existing Topology.
        /// </summary>
        /// <param name="mapTopology">[in] Topology, The topology object.</param>
        /// <returns>
        /// Returns the Last Id for the topology object passed
        /// </returns>
        private int FindLastTopoId(TopologyModel mapTopology)
        {
            long intLastTopoId = 0;

            // Get all the Edges in the topology
            FullEdgeCollection edgeCollection = mapTopology.GetFullEdges();

            // Loop through all the nodes 	
            foreach (FullEdge fullEdge in edgeCollection)
            {
                if (intLastTopoId < fullEdge.ID)
                {
                    intLastTopoId = fullEdge.ID;
                }
            }

            // Get all the Polygons in the topology
            PolygonCollection polygonCollection = mapTopology.GetPolygons();

            // Loop through all the nodes 	
            foreach (Polygon polygon in polygonCollection)
            {
                if (intLastTopoId < polygon.ID)
                {
                    intLastTopoId = polygon.ID;
                }
            }

            // Get all the Nodes in the topology
            NodeCollection nodeCollection = mapTopology.GetNodes();

            // Loop through all the nodes 	
            foreach (Node node in nodeCollection)
            {
                if (intLastTopoId < node.ID)
                {
                    intLastTopoId = node.ID;
                }
            }

            return (int)intLastTopoId;
        }

        public TpmidTableCreator()
		{
		}
	}
}
