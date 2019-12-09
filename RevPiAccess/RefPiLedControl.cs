using System;
using System.Collections.Generic;
using System.Text;
using RevPiAccess.Models;

namespace RevPiAccess
{
    public class RefPiLedControl
    {
        private int _byteAddr;
        private RevPiControl _control;
        private bool _watchDogStatus = false;

        public RefPiLedControl(RevPiControl contr, RevPiConf conf)
        {
            _control = contr;
            var leds = conf.FindVarByName("RevPiLED");
            if(leds == null)
            {
                _byteAddr = 6;
            }
            else
            {
                _byteAddr = leds.ByteAddress;
            }
        }

        public RefPiLedControl(RevPiControl contr, int Offset)
        {
            _control = contr;
            _byteAddr = Offset;
        }

        /// <summary>
        /// Sets the Color of one of the system leds
        /// </summary>
        /// <param name="led">Led to set</param>
        /// <param name="col">Color to set</param>
        public void SetLedColor(SystemLed led, LedColor col)
        {
            var data = new byte[1];
            data = _control.Read(_byteAddr, 1);
            data[0] = (byte)((data[0] & ~(0x03 << (int)led)) | ((byte)col << (int)led));
            _control.Write(_byteAddr, data, 1);
        }

        /// <summary>
        /// Sets the Relais control bit
        /// </summary>
        /// <param name="value">Value for relais bit</param>
        public void setRelaisBit(bool value)
        {
            var data = new byte[1];
            data = _control.Read(_byteAddr, 1);

            if (value)
            {
                data[0] = (byte)((data[0] & ~0x40) | (1 << 6));
            }
            else
            {
                data[0] = (byte)((data[0] & ~0x40) | (0 << 6));
            }

            _control.Write(_byteAddr, data, 1);
        }

        /// <summary>
        /// Sets the WatchdogBit
        /// </summary>
        /// <param name="value">Value for watchdog Bit</param>
        public void setWatchdogBit(bool value)
        {
            var data = new byte[1];
            data = _control.Read(_byteAddr, 1);

            if (value)
            {
                data[0] = (byte)((data[0] & ~0x80) | (1 << 7));
            }
            else
            {
                data[0] = (byte)((data[0] & ~0x80) | (0 << 7));
            }

            _control.Write(_byteAddr, data, 1);
            _watchDogStatus = value;
        }

        /// <summary>
        /// Toggle the WatchdogBit 1 => 0 | 0 => 1
        /// </summary>
        public void toggleWatchdogBit()
        {
            var data = new byte[1];
            data = _control.Read(_byteAddr, 1);

            if (_watchDogStatus)
            {
                data[0] = (byte)((data[0] & ~0x80) | (0 << 7));
                _watchDogStatus = false;
            }
            else
            {
                data[0] = (byte)((data[0] & ~0x80) | (1 << 7));
                _watchDogStatus = true;
            }

            _control.Write(_byteAddr, data, 1);
        }
    }
}
