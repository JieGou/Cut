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

// MapEventManager.cs

using Autodesk.Gis.Map.DisplayManagement;

namespace CH06
{
    internal sealed class MapEventManager
    {
        private MapEventManager()
        {
        }

        public static void ScaleChanged(System.Object sender, ScaleModifiedEventArgs e)
        {
            DoScaleChanged(e.OldScale, e.NewScale);
        }

        /// <summary>
        /// An event that fires when a Scale is changed.
        /// </summary>
        /// <param name="oldscale">[in] The old Scale </param>
        /// <param name="newScale">[in] The new scale.</param>
        public static void DoScaleChanged(double oldScale, double newScale)
        {
            string message = null;
            message = string.Format("\nThe Scale was changed from {0} to {1}", oldScale, newScale);
            Utility.ShowMessage(message);
        }

        public static void ScaleAdded(System.Object sender, ScaleAddedEventArgs e)
        {
            DoScaleAdded(e.Scale, e.Copy);
        }

        /// <summary>
        /// An event that fires when a Scale is added to the current project.
        /// </summary>
        /// <param name="scale">[in] The new Scale added.</param>
        /// <param name="isCopy">[in] The Map object Appended to the project.</param>
        public static void DoScaleAdded(double scale, bool isCopy)
        {
            string message = null;
            message = string.Format("\nScale {0} was added", scale);
            Utility.ShowMessage(message);
        }

        public static void ScaleErased(System.Object sender, ScaleErasedEventArgs e)
        {
            DoScaleErased(e.Scale);
        }

        /// <summary>
        /// An event that fires when a Scale is erased.
        /// </summary>
        /// <param name="scale">[in] ±ÈÀý³ß erased.</param>
        public static void DoScaleErased(double scale)
        {
            string message = null;
            message = string.Format("\nScale {0} was removed", scale);
            Utility.ShowMessage(message);
        }
    }
}
