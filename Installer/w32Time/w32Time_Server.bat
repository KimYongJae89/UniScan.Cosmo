netsh advfirewall firewall add rule name="NTP" dir=in action=allow protocol=UDP localPort=123'

net start w32time
sc query w32time
netstat -ano|findstr 123
pause