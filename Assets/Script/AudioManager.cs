using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;



public class AudioManager : MonoBehaviour
{
    //public Slider volumeSlider;
    public Audio[] audios;
    public static AudioManager instancia;

    float volumeValue;
    float lastVolumeValue;
    float volSaved;

    private void Awake()
    {

        volSaved = PlayerPrefs.GetFloat("volumen");

        foreach (Audio a in audios)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.audioFile;
            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }
    }

    private void Start()
    {
        Play("Musica");
    }


    public void Play(string name)
    {
        Audio a = Array.Find(audios, audios => audios.name == name);

        if (a == null)
        {
            print("El nombre del archivo de audio" + name + "no existe");
            return;
        }
        a.source.Play();
    }

   
}

