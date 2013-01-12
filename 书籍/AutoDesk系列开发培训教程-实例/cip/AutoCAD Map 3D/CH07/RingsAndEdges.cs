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

// RingsAndEdges.cs

using System;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Gis.Map;
using Autodesk.Gis.Map.Topology;

namespace CH07
{
    /// <summary>
    /// RingsAndEdges
    /// </summary>
    public sealed class RingsAndEdges
    {
        /// <summary>
        /// The function used to find the rings and edges
        /// </summary>
        public void FindRingsAndEdges()
        {
            string topologyName;
	
            PromptResult promptRes = Utility.AcadEditor.GetString("\nEnter the Topology name:");
            if (promptRes.Status == PromptStatus.OK)
            {
                topologyName = promptRes.StringResult;
                FindRingsAndEdges(topologyName);
            }
        }

        /// <summary>
        /// Main function used to find the rings and edges
        /// </summary>
        /// <param name="topologyName">[in] Name of existing topology.</param>
        /// <returns>  
        /// Returns true if successful.
        /// </returns>
        private bool FindRingsAndEdges(string topologyName)
        {
            MapApplication mapApp = HostMapApplicationServices.Application;
            Topologies topos = mapApp.ActiveProject.Topologies;
            bool isRingsAndEdgesFound = true;

            // Create an object of TopologyModel;
            TopologyModel mapTopology = null;
	
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
                    Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to open Topology {0} for read with error code: {1}.", topologyName, e.ErrorCode));
                    return false;
                }
            }
            else
            {
                Utility.AcadEditor.WriteMessage(string.Format("\nERROR: The topology {0} doesn't exist.", topologyName));
                return false;
            }	
	
            // Get the nearest polygon to the point selected
            Polygon polygon = null;
            if (GetPolygonPtr(ref polygon, mapTopology))
            {
                // Get the rings for the polygon
                RingCollection rings = null;

                try
                {
                    rings = polygon.GetBoundary();
                }
                catch (MapException e)
                {
                    Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to get the polygon rings with error code: {0}.", e.ErrorCode));
                    mapTopology.Close();
                    return false;
                }
                RingStatistics(rings);

                rings.Clear();
            }
            else
            {
                isRingsAndEdgesFound = false;
            }

            mapTopology.Close();

            return isRingsAndEdgesFound;
        }

        /// <summary>
        /// Selects a point on the centroid of the polygon object and returns the point 
        /// that references to the polygon object. 
        /// </summary>
        /// <param name="polygon"> [out] Polygon topology object.</param>
        /// <param name="mapTopology">[in] Map topology object.</param>
        /// <returns>  
        /// Returns true if successful.
        /// </returns>
        private bool GetPolygonPtr(ref Polygon polygon, TopologyModel mapTopology)
        {
            Point3d selectedPoint = new Point3d(0.0, 0.0, 0.0);
            string promptString = "Select a point inside a polygon of the Topology\n";

            MapApplication app = HostMapApplicationServices.Application;
            Editor editor = app.GetDocument(app.ActiveProject).Editor;
            PromptPointResult  promptPtRes = editor.GetPoint(promptString);
            if (promptPtRes.Status == PromptStatus.OK)
            { 			
                selectedPoint = promptPtRes.Value;
                Point3d ac3dPoint = HelperFunctions.Ucs2Wcs(selectedPoint);

                try
                {
                    polygon = mapTopology.FindPolygon(ac3dPoint);
                }
                catch (MapException e)
                {
                    Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to find the polygon with error code: {0}.", e.ErrorCode));
                    return false;
                }		
            }
            else
            {
                Utility.AcadEditor.WriteMessage("\nERROR: unable to execute getpoint function.");
                return false;
            }
	
            return true;
        }

        /// <summary>
        /// Displays the number of rings, their area and length
        /// </summary>
        /// <param name="rings">[in] Ring collection object: RingCollection.</param>
        /// <returns>  
        /// Returns true if successful.
        /// </returns>
        private bool RingStatistics(RingCollection rings)
        {
            if (rings == null)
            {
                return false;
            }

            // get the number of rings
            int ringCount = rings.Count;
            Utility.AcadEditor.WriteMessage(string.Format("\nThe number of rings :{0}", ringCount.ToString()));
            
			// Do statistics for each ring.
            double ringArea = 0.0;
            double ringLen = 0.0;
            int realRingIndex = 0;
            foreach (Ring ring in rings)
            {
                if (ring.IsExterior)
                {
                    ringArea += ring.Area;
                    ringLen  += ring.Length;
                }
                else
                {
                    ringArea -= ring.Area;
                    ringLen  -= ring.Length;
                }
                // find out if the ring is an Exterior Or Interior ring
                string strExteriorOrInterior = ring.IsExterior ? "exterior" : "interior";

                // print out the rings area and length
                double area = ring.Area;
                double length = ring.Length;
                realRingIndex++;

                Utility.AcadEditor.WriteMessage(string.Format("\n{0} is {1},", realRingIndex.ToString(), strExteriorOrInterior));
                Utility.AcadEditor.WriteMessage(string.Format("Area:{0} Length:{1}", area.ToString(), length.ToString()));

                // highlight all of the ring edges
                HighLightRingEdges(ring);
            }

            return true;
        }

        /// <summary>
        /// Changes the color of the edges for each ring.
        /// </summary>
        /// <param name="ring">[in] Ring object.</param>
        /// <returns>  
        /// Returns true if successful.
        /// </returns>
        private bool HighLightRingEdges(Ring ring)
        {
			// Show how to get 1/2 edges in the Ring.	
            HalfEdgeCollection edgeCollection = ring.GetEdges();

			using (Transaction trans = Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
			{
				// Loops through and highlights all the edges
				int edgeIndex = 0;
				foreach (HalfEdge halfEdge in edgeCollection)
				{
					FullEdge fullEdge = halfEdge.FullEdge;
					ObjectId objId = fullEdge.Entity;

					Entity entity = trans.GetObject(objId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForWrite, false) as Entity;
					if (null != entity)
						entity.ColorIndex = edgeIndex + 1;
					
					edgeIndex = edgeIndex + 1;
				}

				trans.Commit();
			}

            return true;
        }
		
        public RingsAndEdges()
        {
        }
    }
}
