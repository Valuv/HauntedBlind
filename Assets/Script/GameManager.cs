using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Campaign campa�a;

    public Campaign g2;
    public string js; 
    public void CrearJSON()
    {
        Debug.Log(JsonUtility.ToJson(campa�a));
        g2 = JsonUtility.FromJson<Campaign>(js); 
    }
}
