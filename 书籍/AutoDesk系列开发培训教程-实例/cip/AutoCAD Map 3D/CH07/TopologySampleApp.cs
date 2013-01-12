//
// (C) Copyright 2004-2007 by Autodesk, Inc.
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

// TopologySampleApp.cs

using System;
using Autodesk.AutoCAD.ApplicationServices;

namespace CH07
{
    /// <summary>
    /// Summary description for TopologySampleApp.
    /// </summary>
    public class TopologySampleApp : Autodesk.AutoCAD.Runtime.IExtensionApplication
    {
        public void Initialize()
        {
            Utility.AcadEditor.WriteMessage("\nTopologyCS Sample Application Initialized.");
        }

        public void Terminate()
        {
            Utility.AcadEditor.WriteMessage("\nTopologyCS Sample Application Terminated.");
        }
    }
}
