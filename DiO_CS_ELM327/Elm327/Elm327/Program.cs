using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elm327.Core;
using Loging;
using System.IO;

namespace Elm327
{
    class Program
    {

        //TODO: OBD Mode 1 to see what is the protocol table.
        //TODO: Export to class a OBD, OBD2 table op codes.
        //TODO: Create pure OBD comunication object.

        // Serial port name.
        static string SerialPortName = "COM57";

        // Loging path name.
        static string LogPath = @"C:\Users\DiO\Desktop\Logs\";

        // OBD connector.
        static Driver connector = new Driver(SerialPortName, ObdProtocolType.Automatic, MeasuringUnitType.Metric);

        // Data loger.
        static Log loger = new Loging.Log(LogPath);

        /// <summary>
        /// Main runtime.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            loger.SetColectionSize(1);

            loger.CreateRecord("ELM327", "Starting ...", Loging.LogMessageTypes.Info);
            //connector.ObdConnectionLost += NotifyConnectionLost;
            connector.ObdErrorEvent += ErrorEvent;
            connector.ObdMode01.OnErrorEvent += ErrorEvent;
            connector.ObdMode09.OnErrorEvent += ErrorEvent;

            ConnectionResultType result = connector.Connect();

            // Default ELM327 connector.
			//ELM327Connector.Open();
            //ELM327Connector.Write("ATZ");
            //Thread.Sleep(2000);
            //ELM327Connector.Write("ATE0");
            //Thread.Sleep(1000);
            //ELM327Connector.Write("ATL0");
            //Thread.Sleep(500);
            //ELM327Connector.Write("ATH1");
            //Thread.Sleep(500);
            //ELM327Connector.Write("ATSP 5");
            //Thread.Sleep(500);
            //ELM327Connector.Write("01 00");
            //Thread.Sleep(500);
			
            loger.CreateRecord("ELM327", String.Format("Connection result: {0}", result.ToString()), Loging.LogMessageTypes.Warning);
            loger.CreateRecord("ELM327", "Started ...", Loging.LogMessageTypes.Info);

            while (true)
            {
                double engRpm = connector.ObdMode01.EngineRpm;
                Console.WriteLine("RPM: {0:F3}", engRpm);
                System.Threading.Thread.Sleep(1000);
            }

            connector.Disconnect();
            loger.CreateRecord("ELM327", "Stoped ...", Loging.LogMessageTypes.Info);

        }

        /// <summary>
        /// Error event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ErrorEvent(object sender, ErrorEventArgs e)
        {
            loger.CreateRecord("ELM327", e.GetException().Message, Loging.LogMessageTypes.Error);
            Console.WriteLine("OBD: {0}", e.GetException().Message);
        }
    }
}
