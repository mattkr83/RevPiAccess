using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace RevPiAccess.Models
{
    public class RevPiVar
    {
        public Guid DeviceUID { get; }
        public VarType VarType { get; }
        public int Index { get; }
        public string Name { get; }
        public string Default { get; }
        public int BitLength { get; } //1 = BIT, 8 = Byte, 16 = Word, 32 = DWord
        public BitLength LengthType { get; }
        public int ByteAddress { get; }
        public bool Exported { get; }
        public int Counter { get; }
        public string Comment { get; }
        public int BitPos { get; }

        public RevPiVar(Guid duid, XmlNode node, VarType t)
        {
            DeviceUID = duid;
            VarType = t;
            Index = int.Parse(node.Attributes.GetNamedItem("item").InnerText);
            Name = node.ChildNodes.Item(0).InnerText;
            Default = node.ChildNodes.Item(1).InnerText;
            BitLength = int.Parse(node.ChildNodes.Item(2).InnerText);
            LengthType = (BitLength)BitLength;
            ByteAddress = int.Parse(node.ChildNodes.Item(3).InnerText);
            Exported = bool.Parse(node.ChildNodes.Item(4).InnerText);
            Counter = int.Parse(node.ChildNodes.Item(5).InnerText);
            Comment = node.ChildNodes.Item(6).InnerText;
            try
            {
                BitPos = int.Parse(node.ChildNodes.Item(7).InnerText);
            }
            catch
            {
                BitPos = 0;
            }
        }
    }
}
