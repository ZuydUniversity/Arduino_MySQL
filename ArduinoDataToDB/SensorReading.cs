/* 
 ---IMPORTANT---
It is important to read the README attached to this project on GitHub.
When using this code, you need to set the right serialport, in my case it was
COM7, but this will be different on every laptop/computer. Also, you will need to make
a connection to your own MySQL db. This means that the connectionstring needs to be
changed. A manual about connecting your own database can be found in the README file. 

GitHub README link: https://github.com/SteRompen/Connect-Arduino-to-MySQL-DB#readme
*/


using System;
using System.Globalization;
using System.IO.Ports;
using System.Threading;


namespace ArduinoDataToDB
{
    class SensorReading
    {
        readonly DatabaseLogic dBLogic = new DatabaseLogic();
        static SerialPort _serialPort;


        /* 
         * This bool validates the connection between this program and the Arduino. If there is no valid connection,
         * the user gets an error about this. 
         */
        public bool SuccessfulArduinoConnection()
        {
            try
            {
                _serialPort = new SerialPort
                {
                    //Set your personal COM
                    PortName = "COM7",
                    // Maybe you need to change this, check the BaudRate settings in your Arduino code!
                    BaudRate = 9600
                };
                _serialPort.Open();
                _serialPort.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /*
         * This void read the data from the Arduino and converts it (it is a string by default) to 
         * a float. In the most cases, you will need a float as datatype. In some cases, you need 
         * an integer. You can change this by yourself. When the reading is succesful, the reading
         * will be save into the database.
         */
        public void ActivateMeasurment()
        {
            _serialPort.Open();
            while (true)
            {
                string record = _serialPort.ReadExisting();
                try
                {
                    float data = float.Parse(record, CultureInfo.InvariantCulture.NumberFormat);
                    dBLogic.SaveData(data);
                    // DO NOT CHANGE THIS SLEEP
                    Thread.Sleep(5000);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse '{record}'");
                    // This is the time between the readings. You can change this if you want to.
                    // IMPORTANT: if you want to change this, you also need to change the `delay` in 
                    // the Arduino Code, if you don't, the program will not work correctly.
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
