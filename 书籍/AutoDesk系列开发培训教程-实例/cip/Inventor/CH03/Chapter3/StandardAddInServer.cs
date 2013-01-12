using System;
using System.Runtime.InteropServices;
using Inventor;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;

namespace Chapter3
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("a1157258-8d89-443d-a607-6e32ae68bec0")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {

        // Inventor application object.
        private Inventor.Application m_inventorApplication;

        public StandardAddInServer()
        {
        }

        public void GetPropSets()
        {
            //设置到激活文档的引用
            Document doc;
            doc = m_inventorApplication.ActiveDocument;

            //获得PropertySets集合对象
            PropertySets propSets;
            propSets = doc.PropertySets;

            //依次遍历所有的PropertySets
            foreach(PropertySet propSet in propSets)
            {
                //获得PropertySet的显示名和内部名
                MessageBox.Show(String.Format("Display name: {0}\nInternal name: {1}", propSet.DisplayName, propSet.InternalName));
            }

            //获得到“设计跟踪特性”特性集的引用
            PropertySet myPropSet;
            myPropSet = propSets["设计跟踪特性"];

            //获得到“设计跟踪特性”特性集的引用
            PropertySet myPropSet1;
            myPropSet1 = propSets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"];

        }

        public void GetProps()
        {
            //设置到激活文档的引用
            Document doc;
            doc = m_inventorApplication.ActiveDocument;

            //获得PropertySets集合对象
            PropertySets propSets;
            propSets = doc.PropertySets;

            foreach(PropertySet propSet in propSets)
            {
                //遍历当前特性集中的所有特性
                foreach(Property prop in propSet)
                {
                    //获得特性名
                    String name;
                    name = prop.Name;

                    //获得特性值
                    object value;
                    value = prop.Value;

                    //获得特性的Id
                    long propID;
                    propID = prop.PropId;
                }
            }

            object value1;
            value1 = propSets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)PropertiesForDesignTrackingPropertiesEnum.kCostDesignTrackingProperties).Value;
        }

        public void CreateCustomPropSet()
        {
            //设置到激活文档的引用
            Document doc;
            doc = m_inventorApplication.ActiveDocument;

            //获得PropertySets集合对象
            PropertySets propSets;
            propSets = doc.PropertySets;

            //声明新的要添加的PropertySet对象
            PropertySet newPropSet;

            //添加新PropertySet对象
            newPropSet = propSets.Add("New PropertySet", null);

            //在新PropertySet对象中添加新特性
            newPropSet.Add("A Value", "New Property", 2);

            //获得到“Date Checked”特性的引用
            Property myProp;
            myProp = propSets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)PropertiesForDesignTrackingPropertiesEnum.kDateCheckedDesignTrackingProperties);

            //定义需要的日期变量
            DateTime newDate;
            newDate = DateTime.Parse("2007-6-23");

            //将日期指定给特性
            myProp.Value = newDate;
        }

        public void DeleteProps()
        {
            //设置到激活文档的引用
            Document doc;
            doc = m_inventorApplication.ActiveDocument;

            //获得PropertySets集合对象
            PropertySets propSets;
            propSets = doc.PropertySets;

            //声明PropertySet对象
            PropertySet propSet;

            //使用名称获得PropertySet对象
            propSet = propSets["New PropertySet"];

            //遍历集合中的特性并删除
            foreach(Property prop in propSet)
            {
                prop.Delete();
            }

            //删除PropertySet对象
            propSet.Delete();
        }

        public void CreateAttribute(ComponentOccurrence myPart)
        {
            AttributeSets attribSets;
            attribSets = myPart.AttributeSets;

            AttributeSet attribSet;

            if((attribSet = attribSets.Add("MyAttribSet", false)) == null)
            {
                attribSet = attribSets["MyAttribSet"];
            }

            Inventor.Attribute attrib;
            if(attribSet.Count == 0)
            {
                attrib = attribSet.Add("MyAttrib", ValueTypeEnum.kStringType, "Some Text");
            }

        }

        public void RetrievAttributes(ComponentOccurrence myPart)
        {
            AttributeSets attribSets;
            attribSets = myPart.AttributeSets;

            Inventor.Attribute attrib;
            attrib = attribSets["MyAttribSet"]["MyAttrib"];

            MessageBox.Show(attrib.Value.ToString());
        }

        public void DeleteAttributes(ComponentOccurrence myPart)
        {
            AttributeSets attribSets;
            attribSets = myPart.AttributeSets;

            if(attribSets.get_NameIsUsed("MyAttribSet"))
            {
                Inventor.Attribute attrib;
                attrib = attribSets["MyAttribSet"]["MyAttrib"];
                attrib.Delete();
                attribSets["MyAttribSet"].Delete();
            }
        }

        public void FindAttributes()
        {
            PartDocument doc;
            doc = (PartDocument)m_inventorApplication.ActiveDocument;

            ObjectCollection faceColl;
            faceColl = m_inventorApplication.TransientObjects.CreateObjectCollection(null);

            foreach(Face face in doc.ComponentDefinition.SurfaceBodies[1].Faces)
            {
                faceColl.Add(faceColl);
            }

            AttributeSetsEnumerator setEnum;
            setEnum = doc.AttributeManager.OpenAttributeSets(faceColl, "Test");

            foreach(AttributeSet set in setEnum)
            {
                MessageBox.Show(set[0].Value.ToString());
            }
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
                clsid.SetValue(null, "Chapter3");
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
                subKey.SetValue(null, "Chapter3");
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
