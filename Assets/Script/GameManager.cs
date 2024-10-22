using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Campaign campaña;

    public Campaign g2;
    public string js; 
    public void CrearJSON()
    {
        Debug.Log(JsonUtility.ToJson(campaña));
        g2 = JsonUtility.FromJson<Campaign>(js); 
    }
}
