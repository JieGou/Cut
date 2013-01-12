using System;
using System.Runtime.InteropServices;
using Inventor;
using Microsoft.Win32;

namespace Chapter7
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("956c1fdd-db68-4c98-aaa7-49e129131dae")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {

        // Inventor application object.
        private Inventor.Application m_inventorApplication;

        public StandardAddInServer()
        {
        }

        #region ApplicationAddInServer Members

        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            // This method is called by Inventor when it loads the addin.
            // The AddInSiteObject provides access to the Inventor Application object.
            // The FirstTime flag indicates if the addin is loaded for the first time.

            // Initialize AddIn members.
            m_inventorApplication = addInSiteObject.Application;

            // TODO: Add ApplicationAddInServer.Activate implementation.
            // e.g. event initialization, command creation etc.
        }

        public void Deactivate()
        {
            // This method is called by Inventor when the AddIn is unloaded.
            // The AddIn will be unloaded either manually by the user or
            // when the Inventor session is terminated

            // TODO: Add ApplicationAddInServer.Deactivate implementation

            // Release objects.
            Marshal.ReleaseComObject(m_inventorApplication);
            m_inventorApplication = null;

            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void ExecuteCommand(int commandID)
        {
            // Note:this method is now obsolete, you should use the 
            // ControlDefinition functionality for implementing commands.
        }

        public object Automation
        {
            // This property is provided to allow the AddIn to expose an API 
            // of its own to other programs. Typically, this  would be done by
            // implementing the AddIn's API interface in a class and returning 
            // that class object through this property.

            get
            {
                // TODO: Add ApplicationAddInServer.Automation getter implementation
                return null;
            }
        }

        #endregion

        #region COM Registration functions

        /// <summary>
        /// Registers this class as an AddIn for Autodesk Inventor.
        /// This function is called when the assembly is registered for COM.
        /// </summary>
        [ComRegisterFunctionAttribute()]
        public static void Register(Type t)
        {
            RegistryKey clssRoot = Registry.ClassesRoot;
            RegistryKey clsid = null;
            RegistryKey subKey = null;

            try
            {
                clsid = clssRoot.CreateSubKey("CLSID\\" + AddInGuid(t));
                clsid.SetValue(null, "Chapter7");
                subKey = clsid.CreateSubKey("Implemented Categories\\{39AD2B5C-7A29-11D6-8E0A-0010B541CAA8}");
                subKey.Close();

                subKey = clsid.CreateSubKey("Settings");
                subKey.SetValue("AddInType", "Standard");
                subKey.SetValue("LoadOnStartUp", "1");

                //subKey.SetValue("SupportedSoftwareVersionLessThan", "");
                subKey.SetValue("SupportedSoftwareVersionGreaterThan", "11..");
                //subKey.SetValue("SupportedSoftwareVersionEqualTo", "");
                //subKey.SetValue("SupportedSoftwareVersionNotEqualTo", "");
                //subKey.SetValue("Hidden", "0");
                //subKey.SetValue("UserUnloadable", "1");
                subKey.SetValue("Version", 0);
                subKey.Close();

                subKey = clsid.CreateSubKey("Description");
                subKey.SetValue(null, "Chapter7");
            }
            catch
            {
                System.Diagnostics.Trace.Assert(false);
            }
            finally
            {
                if (subKey != null) subKey.Close();
                if (clsid != null) clsid.Close();
                if (clssRoot != null) clssRoot.Close();
            }

        }

        /// <summary>
        /// Unregisters this class as an AddIn for Autodesk Inventor.
        /// This function is called when the assembly is unregistered.
        /// </summary>
        [ComUnregisterFunctionAttribute()]
        public static void Unregister(Type t)
        {
            RegistryKey clssRoot = Registry.ClassesRoot;
            RegistryKey clsid = null;

            try
            {
                clssRoot = Microsoft.Win32.Registry.ClassesRoot;
                clsid = clssRoot.OpenSubKey("CLSID\\" + AddInGuid(t), true);
                clsid.SetValue(null, "");
                clsid.DeleteSubKeyTree("Implemented Categories\\{39AD2B5C-7A29-11D6-8E0A-0010B541CAA8}");
                clsid.DeleteSubKeyTree("Settings");
                clsid.DeleteSubKeyTree("Description");
            }
            catch { }
            finally
            {
                if (clsid != null) clsid.Close();
                if (clssRoot != null) clssRoot.Close();
            }
        }

        // This function uses reflection to get the value for the GuidAttribute attached to the class.
        private static String AddInGuid(Type t)
        {
            string guid = "";

            try
            {
                Object[] customAttributes = t.GetCustomAttributes(typeof(GuidAttribute), false);
                GuidAttribute guidAttribute = (GuidAttribute)customAttributes[0];
                guid = "{" + guidAttribute.Value.ToString() + "}";
            }
            catch
            {
            }

            return guid;

        }

        #endregion

    }
}
