using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elm327.Core.ObdModes
{
    /// <summary>
    /// Possible fuel types.
    /// </summary>
    public enum VehicleFuelType : byte
    {
        Unknown = 0x00,
        Gasoline = 0x01,
        Methanol = 0x02,
        Ethanol = 0x03,
        Diesel = 0x04,
        LPG = 0x05,
        CNG = 0x06,
        Propane = 0x07,
        Electric = 0x08,
        BifuelRunningGasoline = 0x09,
        BifuelRunningMethanol = 0x0A,
        BifuelRunningEthanol = 0x0B,
        BifuelRunningLPG = 0x0C,
        BifuelRunningCNG = 0x0D,
        BifuelRunningProp = 0x0E,
        BifuelRunningElectricity = 0x0F,
        BifuelMixedGasElectric = 0x10,
        HybridGasoline = 0x11,
        HybridEthanol = 0x12,
        HybridDiesel = 0x13,
        HybridElectric = 0x14,
        HybridMixedFuel = 0x15,
        HybridRegenerative = 0x16
    }
}
