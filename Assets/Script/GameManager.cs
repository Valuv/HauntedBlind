using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SpeechLib;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public Campaign campana;
    public GameObject prRoom;

    public Palabras palabrasDetectadas;
    private Habitacion habitacionActual;

    private Dictionary<int, bool> habitacionesVisitadas = new Dictionary<int, bool>(); // Diccionario para rastrear visitas

    public SpVoice voice = new SpVoice();
    private string apiUrl = "http://localhost/get_level_data.php?level_id=";

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }

        int levelId = 1; // Ejemplo: cargar el nivel 1
        StartCoroutine(CargarNivel(levelId));
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

    IEnumerator CargarNivel(int levelId)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl + levelId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error al obtener los datos del nivel: " + request.error);
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
        // Instanciar solo habitaciones con descript_large
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
                    MoverJugador(-1, 0);
                    break;
                case Palabras.Derecha:
                    MoverJugador(1, 0);
                    break;
                case Palabras.Adelante:
                    MoverJugador(0, 1);
                    break;
                case Palabras.Atras:
                    MoverJugador(0, -1);
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
    Atras,
}
