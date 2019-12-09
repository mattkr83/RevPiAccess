using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using RevPiAccess.Models;

namespace RevPiAccess
{
    /// <summary>
    /// Interface with the processimage through piControl driver
    /// </summary>
    public class RevPiControl
    {
        private string PiControlDev = "/dev/piControl0";
        private FileStream _fs;
        private bool _fsStatus = false;

        public RevPiControl()
        {
        
        }

        /// <summary>
        /// Open the PiControl device
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                if (!_fsStatus)
                {
                    _fs = new FileStream(PiControlDev, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    _fsStatus = true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                _fsStatus = false;
            }

            return _fsStatus;
        }

        /// <summary>
        /// Close thi PiControl device
        /// </summary>
        public void Disconnect()
        {
            _fs.Close();
        }

        /// <summary>
        /// True if PiControl device is open
        /// </summary>
        public bool Connected { get { return _fsStatus; } }

        /// <summary>
        /// Writes Bytes to the processimage
        /// </summary>
        /// <param name="data">Array of bytes to write</param>
        /// <param name="var">Processimage variable to write</param>
        /// <returns>Bytes written</returns>
        public int Write(byte[] data, RevPiVar var)
        {
            return Write(var.ByteAddress, data, var.BitLength / 8);
        }

        /// <summary>
        /// Writes Bytes to the processimage
        /// </summary>
        /// <param name="offset">Position to start writing</param>
        /// <param name="data">Array of bytes to write</param>
        /// <param name="length">Number of bytes to write</param>
        /// <returns></returns>
        public int Write(int offset, byte[] data, int length)
        {
            if (data.Length < length) throw new Exception("Data byte array to short.");

            try
            {
                _fs.Seek(offset, SeekOrigin.Begin);
                for(int i = 0; i < length; i++)
                {
                    _fs.WriteByte(data[i]);
                }
                _fs.Flush();
            }
            catch
            {
                return -1;
            }

            return length;
        }

        /// <summary>
        /// Reads bytes fromm the processimage
        /// </summary>
        /// <param name="var">Processimage variable to read</param>
        /// <returns>Array of bytes read from processimage</returns>
        public byte[] Read(RevPiVar var)
        {
            return Read(var.ByteAddress, var.BitLength / 8);
        }

        /// <summary>
        /// Reads bytes fromm the processimage
        /// </summary>
        /// <param name="offset">Position to start reading</param>
        /// <param name="length">Number of bytes to read</param>
        /// <returns></returns>
        public byte[] Read(int offset, int length)
        {
            var ret = new byte[length];
            try
            {
                _fs.Seek(offset, SeekOrigin.Begin);
                for (int i = 0; i < length; i++)
                {
                    ret[i] = (byte)_fs.ReadByte();
                }
            }
            catch
            {
                return null;
            }

            return ret;
        }
    }
}
