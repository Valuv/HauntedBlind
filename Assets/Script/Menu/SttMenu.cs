using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SttMenu : MonoBehaviour
{

    public MenuManager menuManager;

    KeywordRecognizer keywordRecognizer;

    Dictionary<string, Action> wordToAction;

    //Guardar la respuesta del pelado
    private string lastAnswer;

    void Start()
    {
        wordToAction = new Dictionary<string, Action>();

        //Los Adds son para añadir acciones según la frase
        wordToAction.Add("iniciar", Iniciar);

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

    private void Iniciar()
    {
        menuManager.palabrasDetectadas = PalabrasInicio.Iniciar;
    }


}
