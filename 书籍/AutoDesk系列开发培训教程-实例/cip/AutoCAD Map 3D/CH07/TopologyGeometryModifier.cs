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

// TopologyGeometryModifier.cs

using System;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Gis.Map;
using Autodesk.Gis.Map.Topology;

namespace CH07
{
    /// <summary>
    /// Summary description for TopologyGeometryModifier.
    /// </summary>
    public class TopologyGeometryModifier
    {
        /// <summary>
        /// Allows the user to add entities to the topology based on the topology type
        /// </summary>
        /// <param name="topologyName">[in] Name of existing topology.</param>
        /// <returns> 
        /// Returns true if successful.
        /// </returns>
        private bool AddGeometryToExistingTopology(string topologyName)
        {
            bool isAddedGeometryToExistingTopology = true;

            // Gets the Topology object for the specified Topology
            TopologyModel mapTopology;

            MapApplication mapApp = HostMapApplicationServices.Application;
            Topologies topos = mapApp.ActiveProject.Topologies;

            // Does the topology exist to get information from
            if (topos.Exists(topologyName))
            {
                // After successfully got the topology object, you have
                // to open the topology for READ. 
                mapTopology = topos[topologyName];
        		
                try
                {
                    mapTopology.Open(Autodesk.Gis.Map.Topology.OpenMode.ForRead);
                }
                catch(MapException e)
                {
                    Utility.AcadEditor.WriteMessage(String.Format("\nERROR: Unable to open Topology {0} for read with error code: {1}.", topologyName, e.ErrorCode));
                    return false;
                }

                // Set the Topology Type
                TopologyTypes topologyTyppe = TopologyTypes.Fixed;
                if (mapTopology.IsPolygonType)
                {					
                    topologyTyppe = TopologyTypes.Polygon;
                }
                else if (mapTopology.IsLinearType)
                {
                    topologyTyppe = TopologyTypes.Linear;
                }
                else if (mapTopology.IsPointType)
                {			
                    topologyTyppe = TopologyTypes.Point;	  
                }

                // close the topoloy object for read
                mapTopology.Close();

                // add the geometry to the existing topology
                if ( SelectTopologyEntities(mapTopology, topologyTyppe) == false)
                {
                    isAddedGeometryToExistingTopology = false;
                }
            }
            else
            {
                isAddedGeometryToExistingTopology = false;
                Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Topology {0} doesn't exist.", topologyName));
            }

            return isAddedGeometryToExistingTopology;
        }

        /// <summary>
        /// Allows the user to add entities to the topology based on the topology type
        /// </summary>
        public void AddGeometryToExistingTopology()
        {
            string topologyName = "";
            PromptResult promptResult = Utility.AcadEditor.GetString("\nEnter the Topology name:");
            if (promptResult.Status == PromptStatus.Cancel)
            {
                return;
            }
            topologyName = promptResult.StringResult;
            AddGeometryToExistingTopology(topologyName);
        }

        /// <summary>
        /// Adds selected entities to the new topology object
        /// </summary>
        /// <param name="mapTopology">[in] Map topology object.</param>
        /// <param name="topologyType">[in] The type of topology.</param>
        /// <returns>  
        /// Returns true if successful.
        /// </returns>
        private bool SelectTopologyEntities(TopologyModel mapTopology, TopologyTypes  topologyType)
        {
            ObjectIdCollection edgeObjIds = new ObjectIdCollection();
            ObjectIdCollection nodeObjIds = new ObjectIdCollection();

            // Get the required selection sets
            switch (topologyType) 
            {
                case TopologyTypes.Polygon:
                case TopologyTypes.Linear:
                    Utility.AcadEditor.WriteMessage("\nSelect links to add to the new topology: ");
                    if (HelperFunctions.SelectIds(edgeObjIds) == PromptStatus.Cancel)
                    {
                        return false;
                    }
                    // Convert the selection sets to arrays of object ids
                    break;
                case TopologyTypes.Point:
                    Utility.AcadEditor.WriteMessage("\nSelect nodes to add to the new topology: ");
                    if (HelperFunctions.SelectIds(nodeObjIds) == PromptStatus.Cancel)
                    {
                        return false;
                    }
                    break;
            } 
        	
            // Open the topology for Write
            try 
            {
                mapTopology.Open(Autodesk.Gis.Map.Topology.OpenMode.ForWrite);
            }
            catch (MapException e)
            {
                Utility.AcadEditor.WriteMessage(String.Format("\nERROR: Unable to open Topology {0} for write with error code: {1}.",mapTopology.Name , e.ErrorCode));
                return false;
            }

            if (topologyType == TopologyTypes.Polygon)
            {
                // This is polygon topology, add edges to the topology.
                if (edgeObjIds.Count > 0)
                {
                    try
                    {
                        mapTopology.AddPolygons(edgeObjIds);
                    }
                    catch (MapException e)
                    {
                        mapTopology.Close();
                        Utility.AcadEditor.WriteMessage(String.Format("\nERROR: Failed to add links to topology with error code: {0}.", e.ErrorCode));
                        return false;
                    }
                }
            }
            else if (topologyType == TopologyTypes.Linear)
            {
                // This is network topology, add edges to the topology.
                if (edgeObjIds.Count > 0)
                {
                    foreach (ObjectId objId in edgeObjIds)
                    {
                        FullEdge returnedEdge = null;

                        try
                        {
                            returnedEdge = mapTopology.AddCurveObject(objId);
                        }
                        catch (MapException e)
                        {
                            mapTopology.Close();
                            Utility.AcadEditor.WriteMessage(String.Format("\nERROR: Failed to add links to topology with error code: {0}.", e.ErrorCode));
                            return false;
                        }
                    }
                }
            }

            // The node objects could be added to Polygon/Netware/Node topology
            if ((topologyType == TopologyTypes.Polygon || topologyType == TopologyTypes.Linear) && edgeObjIds.Count > 0 ||
                topologyType == TopologyTypes.Point)
            {
                if (nodeObjIds.Count > 0)
                {
                    foreach (ObjectId objId in nodeObjIds)
                    {
                        Node returnedNode = null;
                        try
                        {
                            returnedNode = mapTopology.AddPointObject(objId);
                        }
                        catch (MapException e)
                        {
                            mapTopology.Close();
                            Utility.AcadEditor.WriteMessage(String.Format("\nERROR: Failed to add nodes to topology with error code: {0}.", e.ErrorCode));
                            return false;
                        }
                    }
                }
            }
            mapTopology.Close();

            return true;
        }

        public TopologyGeometryModifier()
        {
        }
    }
}
