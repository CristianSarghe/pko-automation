# pko-automation - A Pirate King Online game bot

A C# Windows Forms application meant to automate Pirate King Online MMORPG actions. 
It can trigger monster killing and automatic drop pick up.

The application uses the network communication between the game client and its server. 
It captures attack and movement TCP packets, forges a custom payload and resends them through the same open connection.

The tools that I have used are:
- The PacketEditor (http://www.packeteditor.com/) open source DLL that is able to attach to processes and replay packets through same socket
- SharpPcap package (https://github.com/chmorgan/sharppcap)
