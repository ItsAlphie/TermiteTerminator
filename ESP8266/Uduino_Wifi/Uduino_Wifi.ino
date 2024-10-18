#include <Uduino_Wifi.h>


Uduino_Wifi uduino("uduinoBoard");

void setup()
{
  Serial.begin(9600);

  // Optional function,  to add BEFORE connectWifi(...)
  uduino.setPort(4222);   // default 4222

  uduino.useSendBuffer(true); // default true
  uduino.setConnectionTries(35); // default 35
  uduino.useSerial(true); // default is true

  // mendatory function
  uduino.connectWifi("Quinten's S24", "industria");

}

void loop()
{
  
  uduino.update();

  if (uduino.isConnected()) {
    uduino.println("Board is connected");
  }
  
}
