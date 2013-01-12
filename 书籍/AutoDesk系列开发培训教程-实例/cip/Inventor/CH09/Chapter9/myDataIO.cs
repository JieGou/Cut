using System;
using System.Collections.Generic;
using System.Text;
using Inventor;
using System.Windows.Forms;
using System.Collections;

namespace Chapter9
{
    class myDataIO
    {
        Inventor.Application ThisApplication;

        public myDataIO(Inventor.Application inventorApplication)
        {
            ThisApplication = inventorApplication;
        }

        public void query_dataIO()
        {
            PartDocument doc;
            doc = (PartDocument)ThisApplication.ActiveDocument;

            PartComponentDefinition compDef;
            compDef = doc.ComponentDefinition;

            Array formats = null;
            Array storageTypes = null;
            DataIO dataIO;
            dataIO = compDef.DataIO;

            dataIO.GetInputFormats(ref formats, ref storageTypes);
            MessageBox.Show(string.Format("CompDef Input: {0}", ioPrint(formats, storageTypes)));
            dataIO.GetOutputFormats(ref formats, ref storageTypes);
            MessageBox.Show(string.Format("CompDef Output: {0}", ioPrint(formats, storageTypes)));
        }

        public string ioPrint(Array Formats, Array StorageTypes)
        {
            string msgString = "";

            for (int iIndex = 0; iIndex < Formats.Length; iIndex++)
            {
                msgString = msgString + Formats.GetValue(iIndex);

                StorageTypeEnum lType;
                lType = (StorageTypeEnum)StorageTypes.GetValue(iIndex);
                
                switch(lType)
                {
                    case StorageTypeEnum.kFileOrStreamStorage:
                        msgString = msgString + "(File or Stream) ";
                        break;
                    case StorageTypeEnum.kFileStorage:
                        msgString = msgString + "(File) ";
                        break;
                    case StorageTypeEnum.kStorageStorage:
                        msgString = msgString + "(Storage) ";
                        break;
                    case StorageTypeEnum.kUnknownStorage:
                        msgString = msgString + "(Unknown) ";
                        break;
                    default:
                        msgString = msgString + "(Stream) ";
                        break;
                }
            }
            return msgString;
        }
    }
}
