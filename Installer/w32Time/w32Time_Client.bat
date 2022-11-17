net start w32time
w32tm /config /syncfromflags:manual /manualpeerlist:192.168.1.100 /update
w32tm /dumpreg /subkey:parameters
net stop w32time
net start w32time
w32tm /resync
pause