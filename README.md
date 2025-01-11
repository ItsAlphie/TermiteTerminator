# Termite Terminator
For a more in-depth dive into the working and development of this project, we refer to the paper also available in this repository. Below is brief summary on how to set up the game yourself.

## Credits
* Mention the two OpenCV repo's
* Wij als awesome sauce team
* Coaches?
* OpenCV Marker Detection: https://github.com/KhairulIzwan/ArUco-markers-with-OpenCV-and-Python
* OpenCV Gesture Detection: https://github.com/kinivi/hand-gesture-recognition-mediapipe

## Setup Instruction
The following instructions are open to adjustments, if the operator deems themselves worthy and knowleadgable enough to customize the setup. But for the others, these next few sections explain how to duplicate our specific setup rather than reconfigure everything.

### Software requirements
Unity Version:
- 2022.3.49f1

python libraries: (Python 3.9 works)
- opencv-contrib-python==4.6.0.66
- imutils
- tensorflow
- mediapipe

### Wireless Connection
#### Background Information
UDP is used to communicate between the main processing computer running the Unity and camera vision, and a set of ESP8266 microcontrollers processing and controlling everything concerning the individual towers.

During the design procedure, hardcoded IP's are used to easily indentify towers and communicate with them accordingly. To set up the wireless network we rely on a mobile hotspot from a modern phone, with a preset 255.255.255.0 subnetmask.

#### Configuration
Following table shows all IP-addresses used:
Device/Service | IP | Use
--- | --- | ---
Towers | 192.168.24.TowerID | Send "b" for boosts, receive "k" and "r" to control servo
Unity (For Towers)| 192.168.24.60 | Receive "b" for boosts and identify tower, send "k" and "r" to selected towers
Unity (For Camera)| 127.0.0.1 | Unity listens to this IP, to locate and place the towers and spells within the playing field

The easiest way to set this network up on new devices is duplicate the original network. Specifically, the network name should be "Galaxy S22A88A" and its password should be "uzjw7402". It is also essential to turn off roaming and internet sevices on the mobile phone, to avoid any external DHCP server to set IP's rendering them unable to be overwritten. Next, the IP of the main computer should be set to match the one in the table above.

### Camera Vision & Towers
This part of the setup refers to the physical configuration. The fundamentals come down to a TV being laid flat on a table (projector would work, but the shadows mess with the gesture recognition) with a camera of any sort hanging above it. Ideally, the camera hangs straight above the center of the screen. 

Within Unity, more specifically in scene "Wave1" in the "LevelManager" the setup can be calibrated. Under the TowerSpawner script skewFactorX, skewFactorY, skewFactorXX, skewFactorYY and heightLimit can be adjusted to conform to the height of the camera, causing a paralax error.

As for the towers, they work on 9V batteries. So, these batteries have to be connected, after which the pins from the connector have to be put in the correct pinheader. The program on the microcontrollers should still be programmed and continue sending and receiving data as normal. As for the recognition of the towers, original aRuCo markers are used with towers ranging from 1 to 7 and the corners of the TV being 21, 22 and 23.
