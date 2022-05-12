# Python 3 server example
from http.server import BaseHTTPRequestHandler, HTTPServer
import time
import json
import socketserver

hostName = "localhost"
serverPort = 8080

positions = {}

class MyServer(BaseHTTPRequestHandler):

    def do_GET(self):
        self.send_response(200)
        self.send_header("Content-type", "application/json")
        self.end_headers()

        global positions
        self.wfile.write(json.dumps(positions).encode("utf-8"))
        
    def do_POST(self):
        self.send_response(200, "OK")
        self.end_headers()
        
        length = int(self.headers['Content-length'])
        x = json.loads(self.rfile.read(length)) 
        
        global positions
        positions = x


with HTTPServer((hostName, serverPort), MyServer) as webServer:
    print("Server running")       
    try:
        webServer.serve_forever()
    except KeyboardInterrupt:
        pass
    webServer.server_close()
    print("Server stopped.")