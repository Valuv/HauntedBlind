using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public Campaign campana;
    public GameObject prRoom;
    public Campaign g2;
    public string js;

    public Palabras palabrasDetectadas;

    public SpVoice voice = new SpVoice();
    public void CrearJSON()
    {
        Debug.Log(JsonUtility.ToJson(campana));
        //g2 = JsonUtility.FromJson<Campaign>(js); 
    }

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }

        StartCoroutine(Jugando());
    }
    IEnumerator Jugando()
    {
        campana = JsonUtility.FromJson<Campaign>(js);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Instantiate(prRoom, new Vector3(i * 1.1f, 0, j * 1.1f), Quaternion.identity);
            }
        }
        yield return new WaitForSeconds(1f);

        Habitacion h0 = GetHabitacion(1, 0);
        Speak(h0.descript_large);
        yield return new WaitForSeconds(5f);
        palabrasDetectadas = Palabras.Nada;
        while (palabrasDetectadas == Palabras.Nada)
        {
            switch (palabrasDetectadas)
            {
                case Palabras.Nada:
                    break;
                case Palabras.Izquierda:
                    break;
                case Palabras.Derecha:
                    break;
                case Palabras.Adelante:
                    break;
                case Palabras.Atras:
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }

    }

    public Habitacion GetHabitacion(int x, int y)
    {
        for (int i = 0; i < campana.habitaciones.Length; i++)
        {
            if (campana.habitaciones[i].posx == x && campana.habitaciones[i].posy == y)
            {
                return campana.habitaciones[i];
            }
        }
        return null;
    }
    void Speak(string text)
    {
        voice.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
    }

    
}

public enum Palabras
{
    Nada,
    Izquierda, 
    Derecha,
    Adelante,
    Atras
}
