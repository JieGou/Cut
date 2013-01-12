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

// TopoPolygonAreaFinder.cs

using System;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Gis.Map;
using Autodesk.Gis.Map.Topology;

namespace CH07
{
    /// <summary>
    /// Find out the area of topology.
    /// </summary>
    public sealed class TopoPolygonAreaFinder
    {
        /// <summary>
        /// Displays the Polygon area and perimeter for the selected Polygon
        /// </summary>
        /// <param name="adsSelectedPoint">[in] Selected point inside of a polygon topology.</param>
        /// <param name="topologyName">[in] Name of an existing topology.</param>
        /// <returns> 
        /// Returns true if successful.
        /// </returns>
        private bool FindAreaByPoint(Point3d adsSelectedPoint, string topologyName)
        {
            bool isFoundAreaByPoint = true;
            // Get the Topology object for the specified Topology
            TopologyModel mapTopology = null;

            MapApplication mapApp = HostMapApplicationServices.Application;
            Topologies topos = mapApp.ActiveProject.Topologies;
           	
            // After successfully got the topology object, you have
            // to open the topology for READ/WRITE. 
            mapTopology = topos[topologyName];

            // Open the topology for read
            try
            {
                mapTopology.Open(Autodesk.Gis.Map.Topology.OpenMode.ForRead);
            }
            catch(MapException e)
            {
                Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to open Topology {0} for read with error code: {1}.", topologyName, e.ErrorCode));
                return false;
            }

            Point3d ac3dPoint = HelperFunctions.Ucs2Wcs(adsSelectedPoint);

            Polygon topoPolyObjectpos = null;

            try
            {
                topoPolyObjectpos = mapTopology.FindPolygon(ac3dPoint);
            }
            catch(MapException e)
            {
                Utility.AcadEditor.WriteMessage(string.Format("\nERROR: Unable to find the polygon area with error code: {0}. ", e.ErrorCode));
                mapTopology.Close();
                return false;
            }

            double area = topoPolyObjectpos.Area;
            double preimeter = topoPolyObjectpos.Perimeter;

            Utility.AcadEditor.WriteMessage(string.Format("\nThe Polygon area is:        {0}", area.ToString()));
            Utility.AcadEditor.WriteMessage(string.Format("\nThe Polygon perimeter is:   {0}", preimeter.ToString()));

            if (mapTopology != null)
            {
                mapTopology.Close(); 
            }			

            return isFoundAreaByPoint;
        }

        /// <summary>
        /// Displays the Polygon area and perimeter for the selected Polygon
        /// </summary>
        public bool FindAreaByPoint()
        {
            string topologyName = "";
            bool result = false;
            PromptResult promptResult = Utility.AcadEditor.GetString("\nEnter the Topology name:");
            if (promptResult.Status == PromptStatus.Cancel)
            {
                return false;
            }
            topologyName = promptResult.StringResult;

			MapApplication app = HostMapApplicationServices.Application;
			Topologies topos = app.ActiveProject.Topologies;
			// Does the topology exist to get information from
			if (!topos.Exists(topologyName))
			{
				Utility.AcadEditor.WriteMessage(string.Format("\nThe topology {0} doesn't exist!!!", topologyName));
				return false;
			}

            PromptPointResult pointRes = Utility.AcadEditor.GetPoint("\nPick a point inside a polygon of the Topology\n");
            if ((pointRes != null) && (pointRes.Status == PromptStatus.OK))
            {
                result = FindAreaByPoint(pointRes.Value,topologyName);
            }
            return result;
        }

        public TopoPolygonAreaFinder()
        {
        }
    }
}
