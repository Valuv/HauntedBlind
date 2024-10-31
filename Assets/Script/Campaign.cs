using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Puerta
{
    public int id;
    public int pos;
    public string descript; 
    public int switche;
    public string descript_true;
    public string descript_false;
    public int habitacion_id; 

}

[System.Serializable]
public class Habitacion
{
    public int id;
    public int posx;
    public int posy;
    public int campaign_id;
    public string descript_large;
    public string descript_short;

    public Puerta[] puertas;
    public AccionesJugador[] acciones;
}
[System.Serializable]
public class Campaign
{
    public int id;
    public string nombre;
    public string descript;
    public string texto_victoria;

    public Habitacion[] habitaciones;
}

public class Objeto
{
    public int id;
    public int habitacion_id;
    public string nombre;
    public string descript;
    
}

public class AccionesJugador
{
    public int id;
    public string descript;

    public Objeto[] objeto;
}
