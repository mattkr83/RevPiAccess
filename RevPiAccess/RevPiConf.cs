using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

using RevPiAccess.Models;

namespace RevPiAccess
{
    public class RevPiConf
    {
        protected String RevPiConfPath = "/etc/revpi/config.rsc";
        protected XmlDocument XMLconfig;
        protected List<RevPiDevice> Devices;

        public bool Open() {
            return Open(RevPiConfPath);
        }

        public bool Open(string path)
        {
            Devices = new List<RevPiDevice>();
            try
            {
                var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var jsonReader = JsonReaderWriterFactory.CreateJsonReader(fs, new XmlDictionaryReaderQuotas());
                XMLconfig = new XmlDocument();
                XMLconfig.LoadXml(XElement.Load(jsonReader).ToString());

                var devnode = XMLconfig.GetElementsByTagName("Devices");
                if(devnode.Count == 1)
                {
                    var devs = devnode.Item(0).ChildNodes;
                    if(devs.Count > 0)
                    {
                        for (int i = 0; i < devs.Count; i++)
                        {
                            Devices.Add(new RevPiDevice(devs.Item(i)));
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public RevPiVar FindVarByName(string name)
        {
            RevPiVar ret = null;

            Devices.ForEach(d =>
            {
                var found = d.AllVars.Where(w => w.Name == name).ToList().Count;
                if (found > 0) ret = d.AllVars.Where(w => w.Name == name).ToList().First();
            });

            return ret;
        }

        public XmlDocument GetCurrentConfig()
        {
            return XMLconfig;
        }
    }
}
