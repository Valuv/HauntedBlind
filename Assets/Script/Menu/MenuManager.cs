using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SpeechLib;

public class MenuManager : MonoBehaviour
{
    
    public PalabrasInicio palabrasDetectadas;
  
    public SpVoice voice = new SpVoice();
  

    private void Start()
    {
        
        
        StartCoroutine(Jugando());
        
    }

   

    IEnumerator Jugando()
    {
        Speak("¡Bienvenido, aventurero! Te espera un viaje único, lleno de misterios y desafíos. En este mundo, cada elección cuenta, cada paso abre una nueva posibilidad, y cada decisión puede marcar el rumbo de tu historia. No solo escucharás, sino que sentirás cómo este mundo toma forma a tu alrededor. ¿Estás listo para enfrentarte a lo desconocido y descubrir secretos ocultos? Si estás listo para comenzar, di iniciar");
        palabrasDetectadas = PalabrasInicio.Nada;
        yield return new WaitForSeconds(26f);

        while (true)
        {
            print("ya puedes hablar");
            switch (palabrasDetectadas)
            {
                case PalabrasInicio.Nada:
                    break;
                case PalabrasInicio.Iniciar:
                    Speak("Buena suerte. La vas a necesitar");
                    yield return new WaitForSeconds(4f);
                    SceneManager.LoadScene(1);
                    break;
                default:
                    break;
            }


            palabrasDetectadas = PalabrasInicio.Nada;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Speak(string text)
    {
        voice.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
    }
}



public enum PalabrasInicio
{
    Nada,
    Iniciar
}
