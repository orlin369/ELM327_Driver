
namespace Elm327.Core
{
    /// <summary>
    /// Represents all OBD protocols supported by the ELM chip.
    /// </summary>
    public enum ObdProtocolType
    {
        /// <summary>
        /// The ELM chip will automatically determine the best protocol to use.
        /// </summary>
        Automatic = '0',

        /// <summary>
        /// SAE J1850 PWM (41.6 kbaud).
        /// </summary>
        SaeJ1850Pwm = '1',

        /// <summary>
        /// SAE J1850 VPW (10.4 kbaud).
        /// </summary>
        SaeJ1850Vpw = '2',

        /// <summary>
        /// ISO 9141-2 (5 baud init, 10.4 kbaud).
        /// </summary>
        Iso9141_2 = '3',

        /// <summary>
        /// ISO 14230-4 KWP (5 baud init, 10.4 kbaud).
        /// </summary>
        Iso14230_4_Kwp = '4',

        /// <summary>
        /// ISO 14230-4 KWP (fast init, 10.4 kbaud).
        /// </summary>
        Iso14230_4_KwpFastInit = '5',

        /// <summary>
        /// ISO 15765-4 CAN (11 bit ID, 500 kbaud).
        /// </summary>
        Iso15765_4_Can11BitFast = '6',

        /// <summary>
        /// ISO 15765-4 CAN (29 bit ID, 500 kbaud).
        /// </summary>
        Iso15765_4_Can29BitFast = '7',

        /// <summary>
        /// ISO 15765-4 CAN (11 bit ID, 250 kbaud).
        /// </summary>
        Iso15765_4_Can11Bit = '8',

        /// <summary>
        /// ISO 15765-4 CAN (29 bit ID, 250 kbaud).
        /// </summary>
        Iso15765_4_Can29Bit = '9',

        /// <summary>
        /// SAE J1939 CAN (29 bit ID, 250 kbaud).
        /// </summary>
        SaeJ1939Can = 'A'
    }
}
