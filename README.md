# OSCTestProject

Using Jorge Garcia's [UnityOSC](https://github.com/jorgegarcia/UnityOSC) receiver to control particles
This example is using a TouchOSC controller running on an iPhone. 

Instructions to get the OSC up and running
1. Get the TouchOSC app from the app store
2. Setup this computer to be a WIFI hub
3. Get your computer's IP address from Preferences>Network
4. Hook your iPhone up to the WIFI hub you just made
5. Open up TouchOSC and set up the IP to match this computer
6. This script is using the TouchOSC control setup as Mix 16
7. Enjoy!

To set this up in Unity, do this:
1. This script needs to be on a GameObject in your scene (only one instance of this script should exist in your level)
2. Create new scripts that subscribe to the events here, see the ParticleController.cs for an example of how to do this

