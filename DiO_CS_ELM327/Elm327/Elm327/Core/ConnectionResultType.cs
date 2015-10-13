
namespace Elm327.Core
{
    /// <summary>
    /// Possible results of calling the <see cref="Elm327.Core.Driver.Connect"/>()
    /// method.
    /// </summary>
    public enum ConnectionResultType
    {
        /// <summary>
        /// A connection was successfully made to the ELM as well
        /// as the vehicle's OBD system.
        /// </summary>
        Connected,

        /// <summary>
        /// A connection couldn't be made to the ELM.  Check the wiring
        /// and ensure the COM port is set correctly.
        /// </summary>
        NoConnectionToElm,

        /// <summary>
        /// Communication was successful with the ELM, but the chip
        /// could not establish communication with the vehicle's
        /// OBD system.  This is most likely due to incorrect wiring
        /// or incorrect OBD protocol specification.
        /// </summary>
        NoConnectionToObd
    }
}
