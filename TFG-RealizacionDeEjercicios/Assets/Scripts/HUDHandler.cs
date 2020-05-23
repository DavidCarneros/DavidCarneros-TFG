using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDHandler : MonoBehaviour
{
    public GameObject textRoutine;
    public GameObject textExercise;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextRoutine(string text){
        textRoutine.GetComponent<TMP_Text>().text = text;
    }

    public void SetTextExercise(string text){
        textExercise.GetComponent<TMP_Text>().text = text;
    }

}
