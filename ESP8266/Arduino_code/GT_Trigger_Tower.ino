#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
#include <Servo.h>

// WiFi credentials
#define ssid "Galaxy S22A88A"         // Replace with your WiFi SSID
#define password "uzjw7402"           // Replace with your WiFi password
IPAddress ip(192, 168, 24, 3);
IPAddress gateway(192, 168, 24, 20);
IPAddress subnet(255, 255, 255, 0);

// Server settings
#define serverIP "192.168.4.121"      // Unity server's IP address
#define serverPort 11000               // Unity server's port

Servo servo;
WiFiUDP udp;

bool towerKilled = false;
const int buttonPin = 15;
bool buttonPressed = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(buttonPin,INPUT);
  servo.attach(D1);
  servo.write(0);
  delay(10);

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

void loop() {
  if(towerKilled == false){
	  buttonPressed = digitalRead(buttonPin);
    if (receiveMessage() == "k"){
      servo.write(180);
      towerKilled = true;
      servo.write(0);
      Serial.println("Tower destroyed!");
    }
    else if(buttonPressed == 1){
      delay(150);
      buttonPressed = digitalRead(buttonPin);
      if(buttonPressed == 0){
        const char* message = "b";
        udp.beginPacket(serverIP, serverPort);
        udp.write(message);
        udp.endPacket();
        Serial.println("Message sent to boost tower!");
        buttonPressed = 0;
      }
    }
  }
  else{
    if(receiveMessage() == "r"){
      towerKilled = false;
      Serial.println("Message recieved to repair tower!");
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


