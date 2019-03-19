//Bryan Leister
//March 2019
//
//Grab the values from our OSC reciever and do something with them!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem m_particles;
    public Texture2D m_textureGradient;

    void Start()
    {
        //This is how you subscribe to events. Whenever the OSC receive gets a message, it will send
        //the results of that message to any script that subscribes to that event (like this script!)
        BLSimpleReceiver.instance.OnSlider1 += ReadSlider1;
        BLSimpleReceiver.instance.OnSlider2 += ReadSlider2;
        BLSimpleReceiver.instance.OnSlider3 += ReadSlider3;
        BLSimpleReceiver.instance.OnSlider4 += ReadSlider4;
        BLSimpleReceiver.instance.OnXY += ReadXY;
        BLSimpleReceiver.instance.OnToggle += ReadToggle;
    }

    void OnDisable()
    {
        //You should always unsubscribe on disable to clean up the event system
        BLSimpleReceiver.instance.OnSlider1 -= ReadSlider1;
        BLSimpleReceiver.instance.OnSlider2 -= ReadSlider2;
        BLSimpleReceiver.instance.OnSlider3 -= ReadSlider3;
        BLSimpleReceiver.instance.OnSlider4 -= ReadSlider4;
        BLSimpleReceiver.instance.OnXY -= ReadXY;
        BLSimpleReceiver.instance.OnToggle -= ReadToggle;
    }

    void ReadSlider1(float f)
    {
        //Now that we have the slider float value, do something with it

        //Pick a pixel along the x axis of the texture, based on f. 0 = far left side of texture, 1 = far right
        int x = (int)Mathf.Lerp(0, m_textureGradient.width, f);
        //Get the color of the pixel, we are coming 2 down on the Y axis
        Color c = m_textureGradient.GetPixel(x, 2);

        //Color the particle using that pixel
        var main = m_particles.main;
        main.startColor = c;
    }

    //This function changes the brightness of the background
    void ReadSlider2(float f)
    {
        float h, s, v;

        //Get the camera tagged MainCamera's background color
        Color c = Camera.main.backgroundColor;

        //Get the Hue, Saturation and Value (brightness ) of the color
        Color.RGBToHSV(c, out h, out s, out v);

        //Based on the slider, change the value
        c = Color.HSVToRGB(h, s, f);

        //Apply the color to the camera's background
        Camera.main.backgroundColor = c;
    }

    void ReadSlider3(float f)
    {
        float h, s, v;
        Color c = Camera.main.backgroundColor;
        Color.RGBToHSV(c, out h, out s, out v);
        c = Color.HSVToRGB(h, f, v);
        Camera.main.backgroundColor = c;
    }

    void ReadXY(Vector2 formantVector)
    {
        //find a position between -3 and 3, if formant.x = 0, position is -3, if formant.x = 1, position is 3
        float posx = Mathf.Lerp(-3, 3, formantVector.x);
        float posy = Mathf.Lerp(-3, 3, formantVector.y);

        m_particles.transform.position = new Vector3(posx, posy, m_particles.transform.position.z);

    }

    void ReadSlider4(float f)
    {
        float h, s, v;
        Color c = Camera.main.backgroundColor;
        Color.RGBToHSV(c, out h, out s, out v);
        c = Color.HSVToRGB(f, s, v);
        Camera.main.backgroundColor = c;
    }



    void ReadToggle(bool isOn)
    {
        var main = m_particles.main;
        if (isOn)
            main.simulationSpace = ParticleSystemSimulationSpace.World;
        else
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
    }
}
