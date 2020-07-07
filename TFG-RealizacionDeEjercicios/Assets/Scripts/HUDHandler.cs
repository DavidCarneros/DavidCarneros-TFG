using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDHandler : MonoBehaviour
{
    public GameObject TorusCounterText;
    public GameObject PointCounterText;
    public GameObject TorusEffect;

    int torusCounter;
    int pointCounter;

    // Start is called before the first frame update
    void Start()
    {
        torusCounter = 0;
        pointCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RestartCounters()
    {
        torusCounter = 0;
        pointCounter = 0;
        this.TorusCounterText.GetComponent<TextMesh>().text = torusCounter.ToString();
        this.PointCounterText.GetComponent<TextMesh>().text = torusCounter.ToString();
    }

    public void IncrementTorusCounter()
    {
        torusCounter++;
        StartCoroutine(IncrementTorusRoutine());
        //this.TorusCounterText.GetComponent<TextMesh>().text = torusCounter.ToString();
    }

    IEnumerator IncrementTorusRoutine()
    {
        this.TorusEffect.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        this.TorusCounterText.GetComponent<TextMesh>().text = torusCounter.ToString();
        this.TorusEffect.SetActive(false);

        yield return null;
    }

    public void IncrementPointCounter()
    {
        pointCounter++;
        this.PointCounterText.GetComponent<TextMesh>().text = pointCounter.ToString();
    }


    /*
    public void SetTextRoutine(string text){
        textRoutine.GetComponent<TMP_Text>().text = text;
    }

    public void SetTextExercise(string text){
        textExercise.GetComponent<TMP_Text>().text = text;
    }
    */



}
