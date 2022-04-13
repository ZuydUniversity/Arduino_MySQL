# Connect Arduino to MySQL DB by cable

This is a console app written in C# to connect your own Arduino to a MySQL database (local). With this console app, you can get the readings from the Arduino to the DB by using the serial port, so there is no Wi-Fi, Ethernet or Bluetooth module/shield needed. You only need an Arduino, a cable to connect the Arduino to your device (laptop or computer), MySQL Workbench, Visual Studio 2019 or above and the Arduino IDE.
## Important 
By default, this application works only with a value from Arduino that is a `float`, if you want to save a `int` for example, you need to change the try/catch box in `Arduino` class. Also, it is very important to make only one print statement in the Arduino code. This can only be a `float` by default. You can change it to `int` if you want to. Right before this print statement, you need to place `delay: 5000` to get it working right. 

# What do you need to do?
### MySQL
It is important the install MySQL on your device. This can be done here: https://dev.mysql.com/downloads/installer/
Installing MySQL is pretty simple, but if you need any help to get started, please visit https://qawithexperts.com/article/c-sharp/mysql-connection-in-c-console-application-example/321 or contact me (for contact details, see below). You need to create a NEW database with a table containing the next items: 
- id (PK, autoincrement and not null)
- record (this is the reading from the Arduino)
- timestamp (this is the moment when the reading entered the DB)
- Maybe you need additional data, you can enter this here. 
If you want to do this quickly, here some SQL to do it:
```
CREATE TABLE `test` (
  `id` int NOT NULL AUTO_INCREMENT,
  `measurement` float NOT NULL,
  `timestamp` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
```
Please note that you also need to change the INSERT statement in the C# code if you want to do this! Some IMPORTANT information: in the guide above, they tell you to use `root` as user and your DB password as password. This is not very secure. My advice is to create a user with access and admin rights (you need these to make changes to the DB) to execute this code. Now you can share this code with other is a save way, without sharing your strong password for entering your MySQL. 

### Arduino
The only thing you need to add to your existing Arduino code is a print statement. For example, if you have the variable `_distance-to-surface` when using an ultrasonic sensor, you write the following statement in your void: `Serial.println(_distance-to-surface)`. IT IS VERY IMPORTANT, ALSO WHEN YOU DON'T USE THE ULTRASONIC SENSOR, TO PUT A DELAY OF 5000 ms BEFORE THE PRINT STATEMENT!!! The print statement will be captured in the `record` in your MySQL DB (see above). In some cases, you need to change the `BaudRate`. This depends on your Arduino code, so make sure the `BaudRate` in the Arduino code and the C# match. If they don't, there is a big chance of getting an error or invalid measurements/readings.

### Setup & link
Before you can use this program, you need to define the MySQL connection string in the code. This can be done very easily. Also, you need to make sure (see class "Arduino"), that you chose the right COM port. This will be different on every device you use, so make sure the right COM port is written in the code. If you don't know what serial port you are using right now, you can learn how do to do this here: https://support.arduino.cc/hc/en-us/articles/4406856349970-Select-board-and-port-in-Arduino-IDE. To get the data into the right database and table, you need to change a small part of the `INSERT` statement. This is the part after `INSERT INTO`. You need to type `yourDatabaseName.TableName`. It is important to change this, otherwise the code won't work!

## MS SQL Server
If you prefer using MS SQL Server, you can use the code below. Remove all of the MySQL code in `DatabaseLogic`, then paste this code. Please make sure changing the connection string, this will not be the same as in the example below. When using MS SQL, make sure you add a table with the same content described in chapter MySQL.


        /*
        * This bool validates the connection between this program and the database. If there is 
        * no valid connection, the user gets an error about this. 
        */
        public bool ConnectionToDBSuccessful()
        {
            //This is your personal MySQL connection string, you need to change this. 
            string _connStr = "Server = .; Database = myDatabase; Trusted_Connection = True;";
            SqlConnection conn = new SqlConnection(_connStr);

            // Try to make a connection with the DB from the string above
            try
            {
                conn.Open();
                conn.Close();
                return true;
            }
            // Error catch when connection is unvalid
            catch (Exception err)
            {
                // Show the error that occured 
                Console.WriteLine(err.ToString());
                conn.Close();
                return false;
            }
        }


        /* After each successful reading done by the Arduino, the data gets saved in the database. 
         * This function saves the reading and attaches a timestamp to You can add more properties to it
         * if you want to, but make sure to add them also to the table in the DB. Otherwise, the program 
         * will crash because of a non-existing part in the table. 
         */
        public void SaveData(float data)
        {
            Console.WriteLine("Data from Arduino: " + data);

            string _connStr = "Server = .; Database = myDatabase; Trusted_Connection = True;"; SqlConnection conn = new SqlConnection(_connStr);
            DateTime getDateTime = DateTime.Now;
            // You also need to change the INSERT statement. After 'INSERT INTO', you need to write yourDatabase.yourTable.
            string sqlInsert = "INSERT INTO myDatabase.dbo.test(measurement, timestamp) VALUES (@currentMeasurement, @currentDateTime)";


            conn.Open();
            // Parameterized query used to prevent SQL injection attacks 
            using (SqlCommand cmd = new SqlCommand(sqlInsert, conn))
            {
                _ = cmd.Parameters.AddWithValue("@currentMeasurement", data);
                _ = cmd.Parameters.AddWithValue("@currentDateTime", getDateTime);
                _ = cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

  
# Contact
If you have any questions or problems, please reach out to me by sending an email to 1915525rompen@zuyd.nl or info@stefanrompen.nl

