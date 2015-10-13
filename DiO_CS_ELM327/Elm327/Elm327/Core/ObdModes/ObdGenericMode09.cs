using System;
using System.IO;

namespace Elm327.Core.ObdModes
{
    /// <summary>
    /// Contains methods and properties for retrieving generic
    /// OBD mode 09 PIDs.
    /// </summary>
    public class ObdGenericMode09 : AbstractObdMode
    {

        #region Event Definitions

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnErrorEvent;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="ObdGenericMode09"/>.
        /// </summary>
        /// <param name="elm">A reference to the ELM327 driver.</param>
        internal ObdGenericMode09(Driver elm)
            : base(elm, "09")
        {
        }

        #endregion

        #region Public Instance Properties

        /// <summary>
        /// Gets the VIN of the vehicle.
        /// </summary>
        public string VehicleIdentificationNumber
        {
            get
            {
                string[] reading = this.GetPidResponse("02");

                if (reading == null || reading.Length == 0)
                    return string.Empty;

                try
                {
                    char[] vinCharacters = new char[reading.Length];

                    for (int i = 0; i < reading.Length; i++)
                    {
                        vinCharacters[i] = (char)this.ConvertHexToInt(reading[i]);
                    }

                    return new string(
                        vinCharacters,
                        0,
                        vinCharacters.Length);
                }
                catch (Exception exception)
                {
                    if (this.OnErrorEvent != null)
                    {
                        this.OnErrorEvent(this, new ErrorEventArgs(exception));
                    }
                    return string.Empty;
                }
            }
        }

        #endregion

        #region Private Instance Methods

        /// <summary>
        /// Accepts a hexadecimal string (such as "01AB") and returns its integer
        /// value.
        /// </summary>
        /// <param name="hexNumber">The string representation of the hexadecimal
        /// value.</param>
        /// <returns>The integer value represented by the hex string.</returns>
        private int ConvertHexToInt(string hexNumber)
        {
            try
            {
                return Convert.ToInt32(hexNumber, 16);
            }
            catch (Exception exception)
            {
                if (this.OnErrorEvent != null)
                {
                    this.OnErrorEvent(this, new ErrorEventArgs(exception));
                }
                return 0;
            }
        }

        #endregion

    }
}
