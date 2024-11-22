#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
#include <Servo.h>

// WiFi credentials
#define ssid "Galaxy S22A88A"         // Replace with your WiFi SSID
#define password "uzjw7402"           // Replace with your WiFi password

// Server settings
#define serverIP "192.168.4.121"      // Unity server's IP address
#define serverPort 11000               // Unity server's port

Servo servo;
WiFiUDP udp;

bool towerKilled = false;
bool firstStartup = false;

int IR_LED = 13;
int previousSensorValue; 
void setup() {
  Serial.begin(9600);
  servo.attach(2);
  servo.write(0);
  delay(10);

  // Connect to WiFi network
  Serial.print("Connecting to WiFi...");
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("\nConnected to WiFi");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
  udp.begin(serverPort);

  pinMode(IR_LED, OUTPUT);
  digitalWrite(IR_LED, HIGH);
}

void loop() {
  // put your main code here, to run repeatedly
  if(towerKilled == false){
    int sensorValue = analogRead(A0);   // read the input on analog pin 0
    sensorValue = sensorValue * (1 / 1024.0);   // Convert the analog reading (which goes from 0 - 1023) to a voltage (0 - 5V)
    //Serial.println(sensorValue);
    if (receiveMessage() == "k"){
      servo.write(180);
      towerKilled = true;
      servo.write(0);
      Serial.println("Tower destroyed!");
    }
    else if (sensorValue == 1 && previousSensorValue == 0 && firstStartup == true){
      Serial.println(sensorValue);
      Serial.println(previousSensorValue);
      const char* message = "b";
      udp.beginPacket(serverIP, serverPort);
      udp.write(message);
      udp.endPacket();
      Serial.println("Message sent to boost tower!");
    }
    previousSensorValue = sensorValue;
    firstStartup = true;
  }
  else{
    if(receiveMessage() == "r"){
      towerKilled = false;
      Serial.println("Message recieved to repair barrier!");
    }
    delay(300);
  }
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
