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

// NetworkAnalysis.cs

using System;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Gis.Map;
using Autodesk.Gis.Map.Topology;

namespace CH07
{
	/// <summary>
	/// NetworkAnalysis
	/// </summary>
	public sealed class NetworkAnalysis
	{
        /// <summary>
        /// Finds the Shortest path between muiltple points
        /// </summary>
        /// <param name="topologyName">[in] Name of existing topology.</param>
        /// <returns> 
        /// Returns true if successful.
        /// </returns>
        private bool FindShortestPath(string topologyName)
        {
	        // Get the Topology object for the specified Topology
	        TopologyModel mapTopology = null;
	        MapApplication mapApp = HostMapApplicationServices.Application;
	        Topologies topos = mapApp.ActiveProject.Topologies;
	        if (topos.Exists(topologyName))
	        {
		        mapTopology = topos[topologyName];
	        }
	        else
	        {
		        Utility.AcadEditor.WriteMessage(string.Format("\nERROR: The Topology {0} doesn't exit!", topologyName));
		        return false;
	        }

	        // Required for the TraceLeastCostPath Function
	        ElementCollection  objectsOnPath = null;
	        TraceParameters objTraceParams = new TraceParameters(     
		        0,		 // MinResistance,
		        100000,	 // MaxResistance,
		        null,	 // NodeResistanceExpression,
		        null,	 // LinkDirectionExpression,
		        null,	 // LinkResistanceExpression,
		        null,	 // LinkReverseResistanceExpression,
		        false,	 // UseReverseDirection,
		        "Result",// Name 
		        "Test"); // Description

	        // Open the topology for read
	        try 
	        {
		        mapTopology.Open(Autodesk.Gis.Map.Topology.OpenMode.ForRead);
	        }
	        catch (MapException e)
	        {
		        Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to open Topology {0} for read with error code: {1}.", topologyName, e.ErrorCode));
		        return false;
	        }
	        
			try
			{
				// Select the starting node
				string promptString = "\nSelect the Start Node:";
				Node startNode = null;
				if (!SelectNode(ref startNode, promptString, mapTopology))
				{
					Utility.AcadEditor.WriteMessage("\nERROR: A valid Start Node was not selected.");
					return false;
				}

				// Select the end node
				Node endNode = null;
				promptString = "\nSelect the End Node:";
				if (!SelectNode(ref endNode, promptString, mapTopology))
				{
					Utility.AcadEditor.WriteMessage("\nERROR: A valid End Node was not selected.");
					return false;
				}

				// Find the shortest path and output it as a new topology called result
				objectsOnPath = mapTopology.TraceLeastCostPath(startNode, endNode, objTraceParams);

				Utility.AcadEditor.WriteMessage("\nThe shortest path function has created a Topology named Result.");

				return true;
			}
			catch (MapException e)
			{
                if (2001 == e.ErrorCode)
                {
                    Utility.AcadEditor.WriteMessage("\nERROR: Topology Result already exists.");
                }
                else
                {
                    Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to find the shortest path with error code: {0}.",  e.ErrorCode));
                }
                return false;
            }
			finally
			{
				mapTopology.Close();
			}
        }

		/// <summary>
		/// Allows the user to find the shortest path between 2 points
		/// </summary>
		public void FindShortestPath()
		{
			string topologyName = "";
			PromptResult promptResult = Utility.AcadEditor.GetString("\nEnter the Topology name:");
			if (promptResult.Status == PromptStatus.OK)
			{
				topologyName = promptResult.StringResult;
				FindShortestPath(topologyName);
			}
		}

        /// <summary>
        /// Finds the Best path between 2 points
        /// </summary>
        /// <param name="topologyName">[in] Name of existing topology.</param>
        /// <returns> 
        /// Returns true if successful.
        /// </returns>
        private bool FindBestPath(string topologyName)
        {
	        // Get the Topology object for the specified Topology
	        TopologyModel mapTopology = null;
	        MapApplication mapApp = HostMapApplicationServices.Application;
	        Topologies topos = mapApp.ActiveProject.Topologies;
	        if (topos.Exists(topologyName))
	        {
		        mapTopology = topos[topologyName];
	        }
	        else
	        {
		        Utility.AcadEditor.WriteMessage(string.Format("\nERROR: The Topology {0} doesn't exit!", topologyName));
		        return false;
	        }

	        // Required for the TraceBestPath Function
	        ElementCollection objectsOnPath = null;
	        TraceParameters objTraceParams = new  TraceParameters(
		        0,		 // MinResistance,
		        100000,	 // MaxResistance,
		        null,	 // NodeResistanceExpression,
		        null,	 // LinkDirectionExpression,
		        null,	 // LinkResistanceExpression,
		        null,	 // LinkReverseResistanceExpression,
		        false,	 // UseReverseDirection,
		        "Result",// Name
		        "Test"); // Description

	        // Open the topology for read
	        try
	        {
		        mapTopology.Open(Autodesk.Gis.Map.Topology.OpenMode.ForRead);
	        }
	        catch (MapException e)
	        {
		        Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to open Topology {0} for read with error code: {1}.", topologyName, e.ErrorCode));
		        return false;
	        }

			try
			{
				// Select the starting node
				string promptString = "\nSelect the Start Node:";
				Node startNode = null;
				if (!SelectNode(ref startNode, promptString, mapTopology))
				{
					Utility.AcadEditor.WriteMessage("\nERROR: A valid Start Node was not selected.");
					return false;
				}

				// Select the end node
				Node endNode = null;
				promptString = "\nSelect the End Node:";
				if (!SelectNode(ref endNode, promptString, mapTopology))
				{
					Utility.AcadEditor.WriteMessage("\nERROR: A valid End Node was not selected.");
					return false;
				}

				// Select the intermediate nodes
				NodeCollection	intermediates = new NodeCollection();
				Node intermediateNode = null;

				promptString = "\nSelect an Intermediate Node:";	
				while (SelectNode(ref intermediateNode, promptString, mapTopology))
				{		
					intermediates.Add(intermediateNode);
					intermediateNode = null;
				}	

				// Find the best path and output it as a new topology called result
				objectsOnPath = mapTopology.TraceBestPath(startNode, endNode, intermediates, objTraceParams);

				Utility.AcadEditor.WriteMessage("\nThe best path function has created a Topology named Result.");
                Utility.SendCommand("_regen\n");
				return true;
			}
			catch (MapException e)
			{
                if (2001 == e.ErrorCode)
                {
                    Utility.AcadEditor.WriteMessage("\nERROR: Topology Result already exists.");
                }
                else
                {
                    Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to find the best path with error code: {0}.",  e.ErrorCode));
                }
                return false;
            }
			finally
			{
				mapTopology.Close();
			}
        }

        /// <summary>
        /// Allows the user to find the Best path between muiltple points
        /// </summary>
        public void FindBestPath()
        {
	        string topologyName = "";
	        PromptResult promptResult = Utility.AcadEditor.GetString("\nEnter the Topology name:");
	        if (promptResult.Status == PromptStatus.OK)
            {
		        topologyName = promptResult.StringResult;
		        FindBestPath(topologyName);
	        }
        }
        
        /// <summary>
        /// Selects a node.
        /// </summary>
        /// <param name="selectedNode">[out] The selected node.</param>
        /// <param name="promptString">[in] The prompt string.</param>
        /// <param name="mapTopology">[in] Map topology object.</param>
        /// <returns> 
        /// Returns true if successful.
        /// </returns>
        private bool SelectNode(ref Node selectedNode, string promptString, TopologyModel mapTopology)
        {
	        // Select the node
	        Point3d selectedPoints = new Point3d(0.0,0.0,0.0);
	        ObjectId objSelectedNode = new ObjectId();

	        MapApplication app = HostMapApplicationServices.Application;
	        Editor editor = app.GetDocument(app.ActiveProject).Editor;
            PromptPointOptions option = new PromptPointOptions(string.Concat(promptString, "\n"));
            option.AllowNone = true;
	        PromptPointResult promptPtRes = editor.GetPoint(option);

	        if (promptPtRes.Status == PromptStatus.OK)
	        { 		
		        selectedPoints = promptPtRes.Value;

		        Point3d ac3dPoint = HelperFunctions.Ucs2Wcs(selectedPoints);

		        double distance = 0.0;

		        // Create the Node from the point selected
		        try
		        {
			        mapTopology.FindNode(ref selectedNode, ref distance, ac3dPoint);
					return true;
		        }
		        catch (MapException e)
		        {
			        Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to find node for read with error code: {0}.", e.ErrorCode));
			        return false;
		        }
	        }

			return false;
        }

        public NetworkAnalysis()
		{
		}
	}
}
