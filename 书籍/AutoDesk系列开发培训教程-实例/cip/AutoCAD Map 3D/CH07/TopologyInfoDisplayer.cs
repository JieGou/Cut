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

// TopologyInfoDisplayer.cs

using System;
using Autodesk.Gis.Map.Topology;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Gis.Map;

namespace CH07
{
	/// <summary>
	/// Show the information of a topology.
	/// </summary>
	public sealed class TopologyInfoDisplayer
	{
        /// <summary>
        /// Displays the Name, description and Type of topology for the name passed
        /// </summary>
        /// <param name="topologyName">[in] Name of an existing topology.</param>
        private void DisplayTopologyInformation(string topologyName)
        {
	        // Create an object of Topology;
	        TopologyModel mapTopology = null;
	        MapApplication mapApp = HostMapApplicationServices.Application;
	        Topologies topos = mapApp.ActiveProject.Topologies;
	        // Does the topology exist to get information from
	        if (topos.Exists(topologyName))
	        {
                mapTopology = topos[topologyName];
		        // Open the topology for read
		        try
		        {
			        mapTopology.Open(Autodesk.Gis.Map.Topology.OpenMode.ForRead);
		        }
		        catch (MapException e)
		        {
			        Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to open Topology {0} for read with error code: {1}.", topologyName, (e.ErrorCode).ToString()));
			        return;
		        }
	        }
	        else
	        {
		        Utility.AcadEditor.WriteMessage(string.Format("\nThe topology {0} doesn't exist!!!", topologyName));
		        return;
	        }

	        // Print the Name, Description, and Type of Topology
	        Utility.AcadEditor.WriteMessage(string.Format("\nThe Topology name is:        {0}", mapTopology.Name));
	        Utility.AcadEditor.WriteMessage(string.Format("\nThe Topology Description is: {0}", mapTopology.Description));

	        string topologyTypeName = "";	
	        if (mapTopology.IsPolygonType)
	        {
		        topologyTypeName = "Polygon";
	        }
	        else if (mapTopology.IsLinearType)
	        {
		        topologyTypeName = "Network";
	        }
	        else if (mapTopology.IsPointType)
	        {
		        topologyTypeName = "Node";
	        }
	        else 
	        {
		        topologyTypeName = "Unknown Type";
	        }
	        Utility.AcadEditor.WriteMessage(string.Format("\nThe Topology Type is:        {0}", topologyTypeName));

	        mapTopology.Close(); 
        }

        /// <summary>
        /// Displays the Name, description and Type of topology for the name input
        /// </summary>
        public void DisplayTopologyInformation()
        {
	        string topologyName = "";
	        PromptResult promptResult = Utility.AcadEditor.GetString("\nEnter the Topology name:");
	        if ( promptResult.Status == PromptStatus.OK )
            {
		        topologyName = promptResult.StringResult;
		        DisplayTopologyInformation(topologyName);
	        }
        }

        public TopologyInfoDisplayer()
		{
		}
	}
}
