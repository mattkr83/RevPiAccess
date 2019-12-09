using System;
using System.Collections.Generic;
using System.Text;

namespace RevPiAccess
{
    public static class ByteConverter
    {

        public static int ConvertBytesToInt(byte[] data)
        {
            if (data.Length > 4) throw new Exception("Data array to long. Max 4 Bytes / 32 Bits");

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(data);

            return BitConverter.ToInt32(data, 0);
        }

        public static uint ConvertBytesToUInt(byte[] data)
        {
            if (data.Length > 4) throw new Exception("Data array to long. Max 4 Bytes / 32 Bits");

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(data);

            return BitConverter.ToUInt32(data, 0);
        }

        public static short ConvertBytesToShort(byte[] data)
        {
            if (data.Length > 2) throw new Exception("Data array to long. Max 2 Bytes / 16 Bits");

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(data);

            return BitConverter.ToInt16(data, 0);
        }

        public static ushort ConvertBytesToUShort(byte[] data)
        {
            if (data.Length > 2) throw new Exception("Data array to long. Max 2 Bytes / 16 Bits");

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(data);

            return BitConverter.ToUInt16(data, 0);
        }

    }
}
