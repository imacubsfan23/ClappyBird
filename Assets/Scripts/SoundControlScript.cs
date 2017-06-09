using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControlScript : MonoBehaviour {

    public float sensitivity = 1000;
    public float loudness = 0;
    public Slider thresholdTopSlider;
    public Slider thresholdBottomSlider;
    public float thresholdTop;
    public float thresholdBottom;
    public bool clapping = false;

    AudioSource _audio;
    public float VelocityPerJump = 3;
    public AudioClip FlyAudioClip;

    // Use this for initialization
    void Start () {
        if (GetComponent<AudioSource>() != null)
        {
            _audio = GetComponent<AudioSource>();
            _audio = GetComponent<AudioSource>();
            _audio.clip = Microphone.Start(Microphone.devices[0].ToString(), false, 1000, 44100);
            while (!(Microphone.GetPosition(Microphone.devices[0]) > 0))
            {
                _audio.Play();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(thresholdBottomSlider.value + ", " + thresholdTopSlider.value);
        thresholdTop = thresholdTopSlider.value;
        thresholdBottom = thresholdBottomSlider.value;
        loudness = GetAverageVolume() * sensitivity;
        if (loudness > thresholdTop && !clapping)
        {
            clapping = true;
            BoostOnYAxis();
        }
        if (loudness < thresholdBottom)
        {
            clapping = false;
        }
    }

    float GetAverageVolume() {
        float[] data = new float[1];
        float a = 0;
        if (GetComponent<AudioSource>() != null)
        {
            _audio.GetOutputData(data, 0);
        }
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a;
    }

    void BoostOnYAxis()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, VelocityPerJump);
        if(GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().PlayOneShot(FlyAudioClip);
        }
    }
}
