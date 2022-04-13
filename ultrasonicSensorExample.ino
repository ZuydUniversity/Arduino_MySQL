// Parameters
float _distanceToSurface;

// Arduino setup
const int trigPin = 8;
const int echoPin = 9;
long duration;


#include <movingAvg.h> // https://github.com/JChristensen/movingAvg
movingAvg distance(3);  // Define the moving average object

// To run once:
void setup() {
  pinMode(trigPin, OUTPUT); 
  pinMode(echoPin, INPUT); 
  Serial.begin(9600);                 //Start Serial
}


// To run repeatedly
void loop() 
{
  GetWaterheight(); 
  // Prints to display the values (CRTL+SHIF+'M')
 // Serial.println("Current height: ");
  delay(5000);
  Serial.println(_distanceToSurface);
}


// To calculate the current distance
void GetWaterheight()
{
  // Clears the trigPin
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  // Sets the trigPin on HIGH state for 10 micro seconds
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);
  // Reads the echoPin, returns the sound wave travel time in microseconds
  duration = pulseIn(echoPin, HIGH);
  // Calculating the distance
  _distanceToSurface = duration * 0.034 / 2;
  return _distanceToSurface;
}
