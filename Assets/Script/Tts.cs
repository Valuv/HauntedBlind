using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;

public class Tts : MonoBehaviour
{
    public string sayit = "Que bonitos ojos tienes, quiero chuparte el pepe";
    public SpVoice voice = new SpVoice();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Speak(sayit);
        }
    }

    void Speak(string text)
    {
        voice.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
    }
}
