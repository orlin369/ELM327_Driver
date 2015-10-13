using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elm327.Core.ObdModes
{
    class OBD2Table
    {
        /// <summary>
        /// Engine RPM
        /// Bytes length: 2
        /// Min: 0
        /// Max: 16,383.75
        /// Formula: value = ((A * 256) + B) / 4
        /// </summary>
        public static byte EngineRPM = 0x0C;
    }
}
