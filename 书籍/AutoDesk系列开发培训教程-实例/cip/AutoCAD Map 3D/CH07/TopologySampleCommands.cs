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

// TopologySampleCommands.cs

using System;
using Autodesk.AutoCAD.ApplicationServices;

namespace CH07
{
	/// <summary>
	/// Commands of the topology sample
	/// </summary>
	public sealed class TopologySampleCommands
	{
		/// <summary>
		/// Lists the Commands this sample provided.
		/// </summary>
        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("CmdList")]
        public static void CmdList()
        {
	        Utility.AcadEditor.WriteMessage("\n Topology sample commands : \n");
	        Utility.AcadEditor.WriteMessage("** DisplayTopologyInfo\n");
	        Utility.AcadEditor.WriteMessage("** CreateMapTopology\n");
	        Utility.AcadEditor.WriteMessage("** FindAreaByPoint\n");
	        Utility.AcadEditor.WriteMessage("** FindShortestPath\n");
	        Utility.AcadEditor.WriteMessage("** FindBestPath\n");
	        Utility.AcadEditor.WriteMessage("** PolygonUnion\n");
	        Utility.AcadEditor.WriteMessage("** FindRingsAndEdges\n");
	        Utility.AcadEditor.WriteMessage("** AddGeometryToTopology\n");
	        Utility.AcadEditor.WriteMessage("** ChangeTopologyName\n");
	        Utility.AcadEditor.WriteMessage("** ChangeTopologyDescription\n");
	        Utility.AcadEditor.WriteMessage("** CreateTable\n");
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("DisplayTopologyInfo")]
        public void DisplayTopologyInfo()
        {
            TopologyInfoDisplayer displayer = new TopologyInfoDisplayer();
            displayer.DisplayTopologyInformation();
        }

    	
        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("CreateMapTopology")]
        public void CreateMapTopology()
        {
            MapTopologyCreator creator = new MapTopologyCreator();
            creator.CreateMapTopology();
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("FindAreaByPoint")]
        public void FindAreaByPoint()
        {
            TopoPolygonAreaFinder finder = new TopoPolygonAreaFinder();
            finder.FindAreaByPoint();
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("FindShortestPath")]
        public void FindShortestPath()
        {
            NetworkAnalysis analysis = new NetworkAnalysis();
            analysis.FindShortestPath();
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("FindBestPath")]
        public void FindBestPath()
        {
            NetworkAnalysis analysis = new NetworkAnalysis();
            analysis.FindBestPath();
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("PolygonUnion")]
        public void Union()
        {
            PolygonOverlay overlay = new PolygonOverlay();
            overlay.Union();
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("FindRingsAndEdges")]
        public void FindRingsAndEdges()
        {
            RingsAndEdges ringEdges = new RingsAndEdges();
            ringEdges.FindRingsAndEdges();
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("AddGeometryToTopology")]
        public void AddGeometryToTopology()
        {
            TopologyGeometryModifier modifier = new TopologyGeometryModifier();
            modifier.AddGeometryToExistingTopology ();
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("ChangeTopologyName")]
        public void ChangeTopologyName()
        {
            MapTopologyCreator creator = new MapTopologyCreator();
            creator.ChangeTopologyName();
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("ChangeTopologyDescription")]
        public void ChangeTopologyDescription()
        {
            MapTopologyCreator creator = new MapTopologyCreator();
            creator.ChangeTopologyDescription();
        }

        [Autodesk.AutoCAD.Runtime.CommandMethodAttribute("CreateTable")]
        public void CreateTable()
        {
            TpmidTableCreator creator = new TpmidTableCreator();
            creator.CreateTable();
        }

		public TopologySampleCommands()
		{
		}
	}

	internal class Utility
	{
		private Utility()
		{
		}

		internal static Autodesk.AutoCAD.EditorInput.Editor AcadEditor
		{
			get
			{
				return Application.DocumentManager.MdiActiveDocument.Editor;
			}
		}

        /// <summary>
        /// Sends a command string to the current document of AutoCAD Map 3D to execute.
        /// </summary>
        /// <returns>Returns true if successfully.</returns>
        public static bool SendCommand(string cmd)
        {
            try
            {
                Document doc = null;
                doc = Application.DocumentManager.MdiActiveDocument;
                doc.SendStringToExecute(cmd, true, false, true);
                return true;
            }
            catch (System.Exception e)
            {
                AcadEditor.WriteMessage(e.Message);
                return false;
            }
        }
    }
}
