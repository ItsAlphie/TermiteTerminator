#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
#include <Servo.h>

#define ssid "Galaxy S22A88A"         // Replace with your WiFi SSID
#define password "uzjw7402"           // Replace with your WiFi password
IPAddress ip(192, 168, 24, 9);
IPAddress gateway(192, 168, 24, 20);
IPAddress subnet(255, 255, 255, 0);

// Server settings
#define serverIP "192.168.4.121"      // Unity server's IP address
#define serverPort 11000               // Unity server's port

Servo servo;
WiFiUDP udp;

bool barrierKilled;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  servo.attach(D1);
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
  WiFi.config(ip, gateway, subnet);
  Serial.println("\nConnected to WiFi");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
  udp.begin(serverPort);
}

void loop() {
  // put your main code here, to run repeatedly:
  if(!barrierKilled){
    if (receiveMessage() == "k"){
      barrierKilled = true;
      servo.write(180);
      Serial.println("Barrier destroyed!");
    }
  }
  else{
    if(receiveMessage()== "r"){
      barrierKilled = false;
      servo.write(0);
    }
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
