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


namespace ArduinoDataToDB
{
    class Program
    {
        /*
         * This is the main part of this appliction. First, the connection between this application and the database
         * gets validated. AFter that, the connection between the application and the Arduino (serial connection) gets
         * validated. If one or both connections are not valid, the user gets notified by a message in the console.
         */
        public static void Main()
        {
            SensorReading sensorReading = new SensorReading();
            DatabaseLogic dBLogic = new DatabaseLogic();
            Program program = new Program();


            // Validate the connection with the MySQL DB
            Console.WriteLine("Connecting to MySQL...");
            if (dBLogic.SuccessfulDBConnection())
            {
                Console.WriteLine("Connection with DB SUCCESFUL");
                Console.WriteLine(" ");
            }
            else
            {
                program.DisplayError("Connection with datbase NOT succesful.Please check the connection to the MySQL database you are using.");
                program.EscapeProgram();
            }

            // Validate the connection with the Arduino
            Console.WriteLine("Connecting to Arduino...");
            if (sensorReading.SuccessfulArduinoConnection())
            {
                Console.WriteLine("Connection with Arduino SUCCESFUL");
                Console.WriteLine(" ");
                sensorReading.ActivateMeasurment();
            }
            else
            {
                program.DisplayError("Connection with Arduino NOT succesful. Please check the serial connection and port you are using.");
                program.EscapeProgram();
            }
        }


        /*
         * In case of a non-valid connection, there will be an error displaying the error that occured. 
         */
        private void DisplayError(string error)
        {
            Console.WriteLine("--- WARNING WARNING WARNING ---");
            Console.WriteLine("--- " + error + " ---");
        }


        /* 
         * In case of an error, the user needs to shutdown the program, fix the error and then restart 
         * the program. To shutdown the console in a easy way, this function helps the user doing this. 
         * After a random button press, the console shuts down.
         */
        private void EscapeProgram()
        {
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}

