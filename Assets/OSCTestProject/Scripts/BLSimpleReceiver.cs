//Bryan Leister
//March 2019
//
//Using Jorge Garcia's UnityOSC receiver to control particles
//https://github.com/jorgegarcia/UnityOSC
//
//This example is using a TouchOSC controller running on an iPhone. 
//
//Instructions to get the OSC up and running
//1. Get the TouchOSC app from the app store
//2. Setup this computer to be a WIFI hub
//3. Get your computer's IP address from Preferences>Network
//4. Hook your iPhone up to the WIFI hub you just made
//5. Open up TouchOSC and set up the IP to match this computer
//6. This script is using the TouchOSC control setup as Mix 16
//7. Enjoy!
//
//To set this up in Unity, do this:
//1. This script needs to be on a GameObject in your scene (only one instance of this script should exist in your level)
//2. Create new scripts that subscribe to the events here, see the ParticleController.cs for an example of how to do this
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;
using System;

public class BLSimpleReceiver : MonoBehaviour
{

    private OSCReciever reciever;
    public static BLSimpleReceiver instance;

    //These events can be subscribed to by other scripts. Whenever a message comes in to a subscriber, it will get
    //the value immediately
    public event Action<float> OnSlider1;
    public event Action<float> OnSlider2;
    public event Action<float> OnSlider3;
    public event Action<float> OnSlider4;
    public event Action<Vector2> OnXY;
    public event Action<bool> OnToggle;

    //This was set up using the iOS TouchOSC with the controller Mix16
    public int port = 8000;
    public string m_slider1 = "/1/fader1";
    public string m_slider2 = "/1/fader2";
    public string m_slider3 = "/1/fader3";
    public string m_slider4 = "/1/fader4";
    public string m_xy = "/1/xy";
    public string m_toggle = "/1/toggle1";

    void Awake()
    {
        //Make sure this is in fact the only instance (Singleton pattern)
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void OnEnable()
    {
        reciever = new OSCReciever();
        reciever.Open(port);
    }

    void OnDisable()
    {
        if (reciever != null)
        {
            Debug.Log("Closing the reciever");
            reciever.Close();
        }
    }

    void Update()
    {
        if (reciever.hasWaitingMessages())
        {
            OSCMessage msg = reciever.getNextMessage();
            Debug.Log(string.Format("message received: {0} {1}", msg.Address, DataToString(msg.Data)));

            //Check if the OSC message is from m_slider, and if anyone is listening for Slider1, if so, send them the float values
            if (msg.Address == m_slider1 && OnSlider1 != null)
                OnSlider1((float)msg.Data[0]);

            if (msg.Address == m_slider2 && OnSlider2 != null)
                OnSlider2((float)msg.Data[0]);

            if (msg.Address == m_slider3 && OnSlider3 != null)
                OnSlider3((float)msg.Data[0]);

            if (msg.Address == m_slider4 && OnSlider4 != null)
                OnSlider4((float)msg.Data[0]);

            if (msg.Address == m_xy && OnXY != null)
            {
                if (msg.Data.Count < 2)
                {
                    Debug.LogError("This message is not sending xy coordinate!");
                    return;
                }
                Vector2 m_data = new Vector2((float)msg.Data[0], (float)msg.Data[1]);
                OnXY(m_data);
            }


            if (msg.Address == m_toggle && OnToggle != null)
                OnToggle(Convert.ToBoolean(msg.Data[0]));
        }
    }

    private string DataToString(List<object> data)
    {
        string buffer = "";

        for (int i = 0; i < data.Count; i++)
        {
            buffer += data[i].ToString() + " ";
        }

        buffer += "\n";

        return buffer;
    }
}
