#include "SparkFunLSM6DS3.h"
#include "Wire.h"
#include "SPI.h"
#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

#define ssid "Galaxy S22A88A"         // Replace with your WiFi SSID
#define password "uzjw7402"           // Replace with your WiFi password
IPAddress ip(192, 168, 24, 7);
IPAddress gateway(192, 168, 24, 20);
IPAddress subnet(255, 255, 255, 0);

// Server settings
#define serverIP "192.168.24.60"      // Unity server's IP address
#define serverPort 11000               // Unity server's port
WiFiUDP udp;

LSM6DS3 myIMU; //Default constructor is I2C, addr 0x6B
const float movementThreshold = 1.0; // in g (acceleration due to gravity)

bool fixing;

void setup() {
// put your setup code here, to run once:
  Serial.begin(9600);
  delay(1000); //relax...
  Serial.println("Processor came out of reset.\n");

  //Call .begin() to configure the IMU
  myIMU.begin();

  Serial.print("Connecting to WiFi...");
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  WiFi.config(ip, gateway, subnet);
  Serial.println("\nConnected to WiFi");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
  udp.begin(serverPort);
}


void loop()
{
//Get all parameters
  // Check for movement
  if(receiveMessage() == "tr") //tr = time to repair
  {
    fixing = true;
  }
  if(fixing == true){
    float accelX = myIMU.readFloatAccelX();
    float accelY = myIMU.readFloatAccelY();
    float accelZ = myIMU.readFloatAccelZ();

    // Print accelerometer values
    Serial.print("\nAccelerometer:\n");
    Serial.print(" X = ");
    Serial.println(abs(accelX), 4);
    Serial.print(" Y = ");
    Serial.println( abs(accelY), 4);
    Serial.print(" Z = ");
    Serial.println(abs(accelZ), 4);

    if (abs(accelX) > movementThreshold || abs(accelY) > movementThreshold || abs(accelZ) > movementThreshold) {
      const char* message = "hr"; //hammer repair
      udp.beginPacket(serverIP, serverPort);
      udp.write(message);
      udp.endPacket();
      Serial.println("Movement detected!");
      Serial.println("Message sent to repair tower!");
    } 
    else {
      Serial.println("No movement detected.");
    }
  }
  
  if (receiveMessage() == "sr"){ //stop repairing
    fixing = false;
  }
  delay(200);
}

String receiveMessage() {
  int packetSize = udp.parsePacket();  // Check for an incoming UDP packet
  if (packetSize) {
    char incomingPacket[255];  // Buffer to hold incoming message
    int len = udp.read(incomingPacket, 255);
    if (len > 0) {
      incomingPacket[len] = '\0';  // Null-terminate the string
    }
    Serial.println(String(incomingPacket));
    return String(incomingPacket);  // Return as a String
  }
  return "";  // Return an empty String if no message was received
}