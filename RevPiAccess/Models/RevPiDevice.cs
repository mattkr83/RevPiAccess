using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace RevPiAccess.Models
{
    public class RevPiDevice
    {
        private List<RevPiVar> _inp;
        private List<RevPiVar> _out;
        private List<RevPiVar> _mem;
        private List<RevPiVar> _ext;
        private List<RevPiVar> _all;

        public Guid GUID { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public int ProductType { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Bmk { get; set; }
        public int InpVariant { get; set; }
        public int OutVariant { get; set; }
        public string Comment { get; set; }
        public int Offset { get; set; }

        public List<RevPiVar> Inp { get { return _inp; } }
        public List<RevPiVar> Out { get { return _out; } }
        public List<RevPiVar> Mem { get { return _mem; } }
        public List<RevPiVar> Extended { get { return _ext; } }
        public List<RevPiVar> AllVars { get { return _all; } }

        public RevPiDevice(XmlNode node)
        {
            _inp = new List<RevPiVar>();
            _out = new List<RevPiVar>();
            _mem = new List<RevPiVar>();
            _ext = new List<RevPiVar>();
            _all = new List<RevPiVar>();

            var tempXDoc = new XmlDocument();
            tempXDoc.LoadXml(node.OuterXml);

            GUID = Guid.Parse(tempXDoc.GetElementsByTagName("GUID").Item(0).InnerText);
            Id = tempXDoc.GetElementsByTagName("id").Item(0).InnerText;
            Type = tempXDoc.GetElementsByTagName("type").Item(0).InnerText;
            ProductType = int.Parse(tempXDoc.GetElementsByTagName("productType").Item(0).InnerText);
            Position = int.Parse(tempXDoc.GetElementsByTagName("position").Item(0).InnerText);
            Name = tempXDoc.GetElementsByTagName("name").Item(0).InnerText;
            Bmk = tempXDoc.GetElementsByTagName("bmk").Item(0).InnerText;
            InpVariant = int.Parse(tempXDoc.GetElementsByTagName("inpVariant").Item(0).InnerText);
            OutVariant = int.Parse(tempXDoc.GetElementsByTagName("outVariant").Item(0).InnerText);
            Comment = tempXDoc.GetElementsByTagName("comment").Item(0).InnerText;
            Offset = int.Parse(tempXDoc.GetElementsByTagName("offset").Item(0).InnerText);

            var inpNodes = tempXDoc.GetElementsByTagName("inp").Item(0).ChildNodes;
            var outNodes = tempXDoc.GetElementsByTagName("out").Item(0).ChildNodes;
            var memNodes = tempXDoc.GetElementsByTagName("mem").Item(0).ChildNodes;
            var extNodes = tempXDoc.GetElementsByTagName("extend").Item(0).ChildNodes;

            AddVarToRegister(ref _inp, ref inpNodes, VarType.Inp);
            AddVarToRegister(ref _out, ref outNodes, VarType.Out);
            AddVarToRegister(ref _mem, ref memNodes, VarType.Mem);
            AddVarToRegister(ref _ext, ref extNodes, VarType.Extend);
        }

        private void AddVarToRegister(ref List<RevPiVar> list, ref XmlNodeList nodeList, VarType type)
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                var ne = new RevPiVar(GUID, nodeList.Item(i), type);
                list.Add(ne);
                _all.Add(ne);
            }
        }
    }
}
