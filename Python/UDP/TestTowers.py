import socket

UDP_IP = "127.0.0.1"
UDP_PORT = 11000
MESSAGE = """[[1.00, 0.2, 0.30, 0.00, 90.00],
              [2.00, 0.00, 0.00, 0.00, 0.00],
              [3.00, 0.4, 0.4, 0.00, 200.00],
              [4.00, 0.00, 0.00, 0.00, 0.00],
              [5.00, 0.80, 0.80, 0.00, 120.00],
              [6.00, 0.00, 0.00, 0.00, 0.00],
              [7.00, 0.00, 0.00, 0.00, 0.00],
              [8.00, 0.00, 0.00, 0.00, 0.00],
              [9.00, 0.00, 0.00, 0.00, 0.00],
              [10.00, 0.00, 0.00, 0.00, 0.00]]"""

print ("UDP target IP:", UDP_IP)
print ("UDP target port:", UDP_PORT)
print ("message:", MESSAGE)

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP
sock.sendto(MESSAGE.encode(), (UDP_IP, UDP_PORT))