import socket

UDP_IP = "127.0.0.1"  # Target IP address
UDP_PORT = 11000      # Target port
MESSAGE = "b"

# Bind to 192.168.26.1 to simulate that the message comes from this IP
BIND_IP = "127.0.0.3"

print("UDP target IP:", UDP_IP)
print("UDP target port:", UDP_PORT)
print("message:", MESSAGE)

# Create a socket
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)  # UDP

# Bind the socket to the specified source IP
sock.bind((BIND_IP, 0))  # Use 0 to let the OS choose the source port

# Send the message
sock.sendto(MESSAGE.encode(), (UDP_IP, UDP_PORT))

print(f"Message sent from {BIND_IP}")
