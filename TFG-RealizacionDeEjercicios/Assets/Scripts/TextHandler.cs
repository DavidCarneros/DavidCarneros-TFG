using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class TextHandler : MonoBehaviour
{
    public GameObject ExerciseInfoText;
    public GameObject HelpInfoText;
    public GameObject StopInfoText;
    public GameObject ExerciseRepetitionsText;

    public GameObject ExerciseRepetitionsTextLeft;

    GameObject lastAcitve;

    // Start is called before the first frame update
    void Start()
    {
        ExerciseInfoText.SetActive(false);
        HelpInfoText.SetActive(false);
        StopInfoText.SetActive(false);
        ExerciseRepetitionsText.SetActive(false);
        lastAcitve = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        switch (text)
        {
            case "ExerciseInfo":
                Debug.Log("ACTIVADO TEXT DE INFO");
                this.ExerciseInfoText.SetActive(true);
                lastAcitve = this.ExerciseInfoText;
                break;
            case "HelpInfo":
                this.HelpInfoText.SetActive(true);
                lastAcitve = this.HelpInfoText;
                break;

            case "StopInfo":
                this.StopInfoText.SetActive(true);
                lastAcitve = this.StopInfoText;
                break;
            case "RepetitionText":
                this.ExerciseRepetitionsText.SetActive(true);
                lastAcitve = this.ExerciseRepetitionsText;
                break;
            default:
                Debug.Log("ERROR: " + text);
                break;
        }
    }

    public void DisableLast()
    {
        if(this.lastAcitve != null)
        {
            Debug.Log("Desactivada ");
            this.lastAcitve.SetActive(false);
            this.lastAcitve = null;
        }
        
    }

    public void SetExerciseRepetitions(int exerciseLeft)
    {
        this.ExerciseRepetitionsTextLeft.GetComponent<TextMesh>().text = "Repeticiones restantes: " + exerciseLeft.ToString();
    }

}
