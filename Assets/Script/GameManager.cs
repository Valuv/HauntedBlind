using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SpeechLib;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public Campaign campana;
    private Habitacion habitacion;
    public GameObject prRoom;

    public Palabras palabrasDetectadas;
    private Habitacion habitacionActual;
    public string js;
    public bool[] switches;

    private Dictionary<int, bool> habitacionesVisitadas = new Dictionary<int, bool>(); // Diccionario para rastrear visitas

    public SpVoice voice = new SpVoice();
    public string apiUrl = "http://localhost/get_level_data.php?campaign_id=";

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        switches = new bool[20];
        switches[0] = true;
        IniciarSwitches();

        int campaignId = 1; // Ejemplo: cargar la campaña 1
        StartCoroutine(CargarCampana(campaignId));
    }

    IEnumerator CargarCampana(int campaignId)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl + campaignId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error al obtener los datos de la campaña: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            campana = JsonUtility.FromJson<Campaign>(jsonResponse);
            StartCoroutine(Jugando());
        }
    }

    IEnumerator Jugando()
    {
        // Instanciar habitaciones con descript_large
        foreach (var habitacion in campana.habitaciones)
        {
            if (!string.IsNullOrEmpty(habitacion.descript_large))
            {
                Instantiate(prRoom, new Vector3(habitacion.posx * 1.1f, 0, habitacion.posy * 1.1f), Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(1f);

        // Inicializar la habitación actual
        habitacionActual = GetHabitacion(1, 0);
        Speak(habitacionActual.descript_large);

        // Marcar la habitación inicial como visitada
        habitacionesVisitadas[habitacionActual.id] = true;

        yield return new WaitForSeconds(13f);

        palabrasDetectadas = Palabras.Nada;

        while (true)
        {
            print("Ahora si, pregunta");
            switch (palabrasDetectadas)
            {
                case Palabras.Nada:
                    break;
                case Palabras.Izquierda:
                    bool mover = false;
                    for (int i = 0; i < habitacionActual.puertas.Length; i++)
                    {
                        if (habitacionActual.puertas[i].pos == 2)
                        {
                            if (switches[habitacionActual.puertas[i].switche])
                            {
                                Speak(habitacionActual.puertas[i].descript_true);
                                yield return new WaitForSeconds(4f);
                                MoverJugador(-1, 0);
                                mover = true;
                                break;
                            }
                            else
                            {
                                Speak(habitacionActual.puertas[i].descript_false);
                            }
                        }
                    }
                    if (!mover)
                    {
                        Speak("Ahí no hay puerta, intenta moverte hacia otra dirección");
                    }
                    break;
                case Palabras.Derecha:
                    mover = false;
                    for (int i = 0; i < habitacionActual.puertas.Length; i++)
                    {
                        if (habitacionActual.puertas[i].pos == 1)
                        {
                            if (switches[habitacionActual.puertas[i].switche])
                            {
                                Speak(habitacionActual.puertas[i].descript_true);
                                yield return new WaitForSeconds(4f);
                                MoverJugador(1, 0);
                                mover = true;
                                break;
                            }
                            else
                            {
                                Speak(habitacionActual.puertas[i].descript_false);
                            }
                        }
                    }
                    if (!mover)
                    {
                        Speak("Ahí no hay puerta, intenta moverte hacia otra dirección");
                    }
                    break;
                case Palabras.Adelante:
                    mover = false;
                    for (int i = 0; i < habitacionActual.puertas.Length; i++)
                    {
                        if (habitacionActual.puertas[i].pos == 3)
                        {
                            if (switches[habitacionActual.puertas[i].switche])
                            {
                                Speak(habitacionActual.puertas[i].descript_true);
                                yield return new WaitForSeconds(4f);
                                MoverJugador(0, 1);
                                mover = true;
                                break;
                            }
                            else
                            {
                                Speak(habitacionActual.puertas[i].descript_false);
                            }
                        }
                    }
                    if (!mover)
                    {
                        Speak("Ahí no hay puerta, intenta moverte hacia otra dirección");
                    }
                    break;
                case Palabras.Atras:
                    mover = false;
                    for (int i = 0; i < habitacionActual.puertas.Length; i++)
                    {
                        if (habitacionActual.puertas[i].pos == 0)
                        {
                            if (switches[habitacionActual.puertas[i].switche])
                            {
                                Speak(habitacionActual.puertas[i].descript_true);
                                yield return new WaitForSeconds(4f);
                                MoverJugador(0, -1);
                                mover = true;
                                break;
                            }
                            else
                            {
                                Speak(habitacionActual.puertas[i].descript_false);
                            }
                        }
                    }
                    if (!mover)
                    {
                        Speak("Ahí no hay puerta, intenta moverte hacia otra dirección");
                    }
                    break;
                case Palabras.Examinar:
                    Objetos();
                    break;
                default:
                    break;
            }


            palabrasDetectadas = Palabras.Nada;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void MoverJugador(int x, int y)
    {
        int nuevoX = habitacionActual.posx + x;
        int nuevoY = habitacionActual.posy + y;

        Habitacion nuevaHabitacion = GetHabitacion(nuevoX, nuevoY);

        if (nuevaHabitacion != null && !string.IsNullOrEmpty(nuevaHabitacion.descript_large))
        {
            habitacionActual = nuevaHabitacion;

            if (habitacionesVisitadas.ContainsKey(habitacionActual.id))
            {
                // Ya fue visitada
                Speak(habitacionActual.descript_short);
            }
            else
            {
                // Primera vez
                Speak(habitacionActual.descript_large);
                habitacionesVisitadas[habitacionActual.id] = true;
            }
        }
        else
        {
            Speak("No puedes moverte en esa dirección."); // Mensaje si no hay habitación válida en esa dirección
        }
    }

    public Habitacion GetHabitacion(int x, int y)
    {
        foreach (var habitacion in campana.habitaciones)
        {
            if (habitacion.posx == x && habitacion.posy == y)
            {
                return habitacion;
            }
        }
        return null;
    }
    public void CrearJSON()
    {
        if (campana != null)
        {
            string json = JsonUtility.ToJson(campana, true);
            Debug.Log(json);
        }
        else
        {
            Debug.LogWarning("Campaña no cargada. No se puede crear JSON.");
        }
    }


    void Speak(string text)
    {
        voice.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
    }

    public void Objetos()
    {
        for (int i = 0; i < campana.habitaciones.Length; i++)
        {

            if (habitacionActual.objetos.Length > 0)
            {
                
                for (int j = 0; j < habitacionActual.objetos.Length; j++)
                {
                    Speak(habitacionActual.objetos[j].descript);
                    switches[habitacionActual.objetos[j].puerta_id] = true;
                    //Es solo un objeto pero se especifica el id. 
                }
            }

        }
    }

    public void IniciarSwitches()
    {

        foreach (var habitacion in campana.habitaciones)
        {
            foreach (Puerta puerta in habitacion.puertas)
            {
                if (puerta.switche == 1)
                {
                    switches[puerta.id] = true;
                }
                else
                {
                    switches[puerta.id] = false;
                }
            }
        }
    }
}


public enum Palabras
{
    Nada,
    Izquierda,
    Derecha,
    Adelante,
    Atras,
    Examinar
}
