using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ViewPatientProgress : MonoBehaviour
{

    public GameObject PatientKeyView;
    public GameObject PatientProgressView;

    public GameObject InputKeyboardHandler;

    public GameObject textKey;
    
    public string PatientKey;

    public GameObject Evaluation_No;
    public GameObject Evaluation_One;
    public GameObject Evaluation_Problems;
    public GameObject Evaluation_NotProblems;
    public GameObject Evaluation_InTime;
    public GameObject Evaluation_NotInTime;
    public GameObject Evaluation_Progress;
    public GameObject Evaluation_NotProgress;
    public GameObject Evaluation_Same;

    public GameObject NextButton;

    public GameObject routineLabel;

    List<PatientEvaluation> patientEvaluations;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        Evaluation_No.SetActive(false);
        Evaluation_One.SetActive(false);
        Evaluation_Problems.SetActive(false);
        Evaluation_NotProblems.SetActive(false);
        Evaluation_InTime.SetActive(false);
        Evaluation_NotInTime.SetActive(false);
        Evaluation_Progress.SetActive(false);
        Evaluation_NotProgress.SetActive(false);
        Evaluation_Same.SetActive(false);
    
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallKeyboard()
    {
        this.textKey.SetActive(true);
        this.InputKeyboardHandler.GetComponent<InputKeyboardHandler>().ActiveKeyboard(gameObject, this.textKey);
    }

    public void SendRequest()
    {
        StartCoroutine(GetEvaluationRequest());
    }
    IEnumerator ShowEvaluation()
    {
        this.PatientKeyView.SetActive(false);
        this.routineLabel.GetComponent<TextMesh>().text = "Rutina con ejercicio " + patientEvaluations[index].routine.exercise.name + "\nRepeticiones: " + patientEvaluations[index].routine.repetitions + "\nDuracion" + patientEvaluations[index].routine.duration;
        this.PatientProgressView.SetActive(true);
        this.NextButton.SetActive(false);
        Debug.Log(this.patientEvaluations[index].evaluations);
        for(int i = 0; i < this.patientEvaluations[index].evaluations.Length; i++)
        {
            switch (this.patientEvaluations[index].evaluations[i])
            {
                case EvaluatePatient.No:
                    this.Evaluation_No.SetActive(true);
                    yield return new WaitForSeconds(3);
                    this.Evaluation_No.SetActive(false);
                    break;
                case EvaluatePatient.One:
                    this.Evaluation_One.SetActive(true);
                    yield return new WaitForSeconds(3);
                    this.Evaluation_One.SetActive(false);
                    break;
                case EvaluatePatient.Problems:
                    this.Evaluation_Problems.SetActive(true);
                    yield return new WaitForSeconds(3);
                    this.Evaluation_Problems.SetActive(false);
                    break;
                case EvaluatePatient.NotProblems:
                    this.Evaluation_NotProblems.SetActive(true);
                    yield return new WaitForSeconds(3);
                    this.Evaluation_NotProblems.SetActive(false);
                    break;
                case EvaluatePatient.InTime:
                    this.Evaluation_InTime.SetActive(true);
                    yield return new WaitForSeconds(3);
                    this.Evaluation_InTime.SetActive(false);
                    break;
                case EvaluatePatient.NotInTime:
                    this.Evaluation_NotInTime.SetActive(true);
                    yield return new WaitForSeconds(3);
                    this.Evaluation_NotInTime.SetActive(false);
                    break;
                case EvaluatePatient.Progress:
                    this.Evaluation_Progress.SetActive(true);
                    yield return new WaitForSeconds(3);
                    this.Evaluation_Progress.SetActive(false);
                    break;
                case EvaluatePatient.NotProgress:
                    this.Evaluation_NotProgress.SetActive(true);
                    yield return new WaitForSeconds(3);
                    this.Evaluation_NotProgress.SetActive(false);
                    break;
                case EvaluatePatient.Same:
                    this.Evaluation_Same.SetActive(true);
                    yield return new WaitForSeconds(3);
                    this.Evaluation_Same.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        if(index < this.patientEvaluations.Count)
        {
            index++;
            this.NextButton.SetActive(true);
            //StartCoroutine(ShowEvaluation());
        }
        else
        {
           // Finish
        }

    }
    
    public void Next()
    {
        StartCoroutine(ShowEvaluation());
    }

    IEnumerator GetEvaluationRequest()
    {
        UnityWebRequest webRequest = new UnityWebRequest("http://phyreup.francecentral.cloudapp.azure.com:3000/patient/evaluate/" + this.textKey.GetComponent<TextMesh>().text, "GET");
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        UnityWebRequestAsyncOperation requestHandler = webRequest.SendWebRequest();
        requestHandler.completed += delegate (AsyncOperation pOperation) {
            string jsonString = webRequest.downloadHandler.text;
            jsonString = JsonHelper.FixJson(jsonString);
            PatientEvaluation[] patientEvaluationsArray = JsonHelper.FromJson<PatientEvaluation>(jsonString);
            this.patientEvaluations = new List<PatientEvaluation>();
            for (int i = 0; i < patientEvaluationsArray.Length; i++)
            {
                patientEvaluations.Add(patientEvaluationsArray[i]);
            }
            if(this.patientEvaluations.Count != 0)
            {
                index = 0;
                StartCoroutine(ShowEvaluation());
            }
            
            //StartRoutines ();

        };

        yield return null;

    }
}
