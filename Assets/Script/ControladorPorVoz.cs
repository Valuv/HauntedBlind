using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Stt : MonoBehaviour
{

    KeywordRecognizer keywordRecognizer;

    Dictionary<string, Action> wordToAction;

    //Guardar la respuesta del pelado
    private string lastAnswer;

    void Start()
    {
        wordToAction = new Dictionary<string, Action>();
        //Los Adds son para añadir acciones según la frase
        wordToAction.Add("izquierda", Left);
        wordToAction.Add("derecha", Rigth);
        wordToAction.Add("rojo", Red);
        wordToAction.Add("abrir puerta izquierda", Up);
        wordToAction.Add("coger llave", Key);

        keywordRecognizer = new KeywordRecognizer(wordToAction.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += WordRecognized;
        keywordRecognizer.Start();
    }

    private void WordRecognized(PhraseRecognizedEventArgs word)
    {
        //Imprime la palabra y realiza la acción
        lastAnswer = word.text;
        Debug.Log(lastAnswer);
        wordToAction[word.text].Invoke();
    }

    private void Up()
    {
        transform.Translate(0, 1, 0);
    }

    private void Red()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    private void Key()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.green);
    }

    private void Left()
    {
        transform.Translate(1, 0, 0);
    }

    private void Rigth()
    {
        transform.Translate(-1, 0, 0);
    }

    void Update()
    {
        if(lastAnswer != null)
        {
            Debug.Log(lastAnswer);
        }
        
    }

}
