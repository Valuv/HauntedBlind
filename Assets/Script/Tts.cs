using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;

public class Tts : MonoBehaviour
{
    public SpVoice voice = new SpVoice();
    // Start is called before the first frame update
    void Start()
    {
        voice.Speak("Buenas tardes", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void Speak
}
