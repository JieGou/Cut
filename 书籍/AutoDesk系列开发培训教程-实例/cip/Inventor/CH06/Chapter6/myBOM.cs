using System;
using System.Collections.Generic;
using System.Text;
using Inventor;
using System.Diagnostics;
using System.Windows.Forms;

namespace Chapter6
{
    class myBOM
    {
        public void QueryCompData(Inventor.Application ThisApplication)
        {
            BOM bom;
            bom = ((AssemblyDocument)ThisApplication.ActiveDocument).ComponentDefinition.BOM;

            BOMView bomView;
            bomView = bom.BOMViews["装配结构"];

            for (int i = 1; i <= bomView.BOMRows.Count; i++ )
            {
                BOMRow row;
                row = bomView.BOMRows[i];

                ComponentDefinition compDef;
                compDef = row.ComponentDefinitions[1];

                PropertySet propSet;
                propSet = ((Document)compDef.Document).PropertySets["设计跟踪特性"];

                MessageBox.Show(string.Format("#: {0} Quantity: {1} Part: {2} Desc: {3}", row.ItemNumber,
                    row.ItemQuantity, propSet["Part Number"].Value, propSet["Description"].Value));

            }
        }
    }
}
