using System;
using System.IO;

namespace Elm327.Core.ObdModes
{
    /// <summary>
    /// Contains methods and properties for retrieving generic
    /// OBD mode 01 PIDs.
    /// </summary>
    public class ObdGenericMode01 : AbstractObdMode
    {

        #region Event definitions

        /// <summary>
        /// On error determin.
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnErrorEvent;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="ObdGenericMode01"/>.
        /// </summary>
        /// <param name="elm">A reference to the ELM327 driver.</param>
        internal ObdGenericMode01(Driver elm)
            : base(elm, "01")
        {
        }

        #endregion

        #region Private Instance Properties

        /// <summary>
        /// Gets the current speed of the vehicle in km/h.
        /// </summary>
        private int VehicleSpeedInKilometersPerHour
        {
            get
            {
                string[] reading = this.GetPidResponse("0D");

                return 
                    reading.Length > 0 ? 
                    this.ConvertHexToInt(reading[0]) : 
                    0;
            }
        }

        /// <summary>
        /// Converts kilometers to miles.
        /// </summary>
        /// <param name="km">The value to convert.</param>
        /// <returns>The mileage value.</returns>
        private double ConvertKilometersToMiles(double km)
        {
            // 1 mile = 1.609 km

            return km / 1.609;
        }

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

        /// <summary>
        /// Converts celsius temperature to farenheit.
        /// </summary>
        /// <param name="celsiusTemperature">Temperature in celsius.</param>
        /// <returns>The farenheit temperature.</returns>
        private double ConvertCelsiusToFarenheit(int celsiusTemperature)
        {
            // F = (C x 1.8) + 32

            return celsiusTemperature * 1.8 + 32;
        }

        #endregion

        #region Public Instance Properties

        /// <summary>
        /// Gets the ambient air temperature (in celsius or farenheit,
        /// depending on the current unit selection).
        /// </summary>
        public double AmbientAirTemperature
        {
            get
            {
                // The formula for this value is (A-40)

                string[] reading = this.GetPidResponse("46");

                if (reading.Length > 0)
                {
                    return
                        this.MeasuringUnitType == MeasuringUnitType.English ?
                        this.ConvertCelsiusToFarenheit(this.ConvertHexToInt(reading[0]) - 40) :
                        this.ConvertHexToInt(reading[0]) - 40;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gets the current engine coolant temperature (in celsius or farenheit,
        /// depending on the current unit selection).
        /// </summary>
        public double EngineCoolantTemperature
        {
            get
            {
                // The formula for this value is (A-40)

                string[] reading = this.GetPidResponse("05");

                if (reading.Length > 0)
                {
                    return
                        this.MeasuringUnitType == MeasuringUnitType.English ?
                        this.ConvertCelsiusToFarenheit(this.ConvertHexToInt(reading[0]) - 40) :
                        this.ConvertHexToInt(reading[0]) - 40;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gets the current engine RPM.
        /// </summary>
        public double EngineRpm
        {
            get
            {
                // This value is returned in two bytes.  Divide the reading by
                // 4 to get the correct RPM value.

                string[] reading = this.GetPidResponse("0C");

                return 
                    reading.Length > 1 ? 
                    this.ConvertHexToInt(reading[0] + reading[1]) / 4 : 
                    0;
            }
        }

        /// <summary>
        /// Gets the estimated distance per gallon (either miles per gallon
        /// or kilometers per gallon, depending on the current unit selection).
        /// Note that the vehicle must be equipped with a mass air flow sensor 
        /// in order for this value to be reported accurately. 
        /// </summary>
        public double EstimatedDistancePerGallon
        {
            get
            {
                // Check this link for discussions about this calculation:
                // http://www.mp3car.com/vbulletin/engine-management-obd-ii-engine-diagnostics-etc/75138-calculating-mpg-vss-maf-obd2.html
                // TODO: What I have below could be completely wrong.
                // TODO: This value is instantaneous.  Either we or the caller need 
                // to average the values over a period of time to smooth out the reading.

                // Km/Gallon = (41177.346 * Speed) / (3600 * MAF Rate)

                double mafRate = this.MassAirFlowRate;

                double kmPerGallon =
                    mafRate != 0 ?
                    (41177.346 * this.VehicleSpeedInKilometersPerHour) / (this.MassAirFlowRate * 3600) :
                    0;

                return
                    this.MeasuringUnitType == MeasuringUnitType.English ?
                    this.ConvertKilometersToMiles(kmPerGallon) :
                    kmPerGallon;
            }
        }

        /// <summary>
        /// Gets the current fuel level as a percentage value between 0
        /// and 100.
        /// </summary>
        public double FuelLevel
        {
            get
            {
                // The formula for this value is (A*100)/255

                string[] reading = this.GetPidResponse("2F");

                return
                    reading.Length > 0 ?
                    (this.ConvertHexToInt(reading[0]) * 100) / 255 :
                    0;
            }
        }

        /// <summary>
        /// Gets the fuel type for the vehicle.
        /// </summary>
        public VehicleFuelType FuelType
        {
            get
            {
                string[] reading = this.GetPidResponse("51");

                if (reading.Length < 1)
                    return VehicleFuelType.Unknown;

                try
                {
                    return (VehicleFuelType)this.ConvertHexToInt(reading[0]);
                }
                catch (Exception exception)
                {
                    if (this.OnErrorEvent != null)
                    {
                        this.OnErrorEvent(this, new ErrorEventArgs(exception));
                    }
                    return VehicleFuelType.Unknown;
                }
            }
        }

        /// <summary>
        /// Gets the intake air temperature (in celsius or farenheit,
        /// depending on the current unit selection).
        /// </summary>
        public double IntakeAirTemperature
        {
            get
            {
                // The formula for this value is (A-40)

                string[] reading = this.GetPidResponse("0F");

                if (reading.Length > 0)
                {
                    return
                        this.MeasuringUnitType == MeasuringUnitType.English ?
                        this.ConvertCelsiusToFarenheit(this.ConvertHexToInt(reading[0]) - 40) :
                        this.ConvertHexToInt(reading[0]) - 40;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gets the current MAF rate in grams/sec.
        /// </summary>
        public double MassAirFlowRate
        {
            get
            {
                // This value is returned in two bytes.  Divide the reading by
                // 100 to get the correct MAF value.

                string[] reading = this.GetPidResponse("10");

                return
                    reading.Length > 1 ?
                    this.ConvertHexToInt(reading[0] + reading[1]) / 100 :
                    0;
            }
        }

        /// <summary>
        /// Gets amount of time, in seconds, that the engine has been
        /// running since cranked.
        /// </summary>
        public int RunTimeSinceEngineStart
        {
            get
            {
                // This reading is returned in two bytes

                string[] reading = this.GetPidResponse("1F");

                return
                    reading.Length > 1 ?
                    this.ConvertHexToInt(reading[0] + reading[1]) :
                    0;
            }
        }

        /// <summary>
        /// Gets the throttle position as a percentage value between 0
        /// and 100.
        /// </summary>
        public double ThrottlePosition
        {
            get
            {
                // The formula for this value is (A*100)/255

                string[] reading = this.GetPidResponse("11");

                return
                    reading.Length > 0 ?
                    (this.ConvertHexToInt(reading[0]) * 100) / 255 :
                    0;
            }
        }

        /// <summary>
        /// Gets the current speed of the vehicle (either in mph or km/h,
        /// depending on the current unit selection).
        /// </summary>
        public double VehicleSpeed
        {
            get
            {
                return
                    this.MeasuringUnitType == MeasuringUnitType.English ?
                    this.ConvertKilometersToMiles(this.VehicleSpeedInKilometersPerHour) :
                    this.VehicleSpeedInKilometersPerHour;
            }
        }

        #endregion

    }
}
