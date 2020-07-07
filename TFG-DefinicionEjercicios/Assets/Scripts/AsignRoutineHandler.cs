using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class AsignRoutineHandler : MonoBehaviour
{
    public GameObject RoutinesList;
    public GameObject SetPatient;
    public GameObject ManagementMenu;

    public GameObject KeyboardHandler;
    public GameObject InputField;
    public GameObject PatientPlaceholder;

    public GameObject SendButton;

    public Routine routine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllRoutines()
    {
        this.RoutinesList.SetActive(true);
        this.RoutinesList.GetComponent<ViewRoutinesAndSelect>().GetAllRoutines();
    }

    public void SetRoutineAndGoNextStep(Routine routine)
    {
        this.routine = routine;
        this.RoutinesList.SetActive(false);
        this.SetPatient.SetActive(true);
        this.InputField.SetActive(false);
        this.PatientPlaceholder.SetActive(true);
        this.SendButton.SetActive(false);
    }

    public void CallKeyboard()
    {
        this.PatientPlaceholder.SetActive(false);
        this.InputField.SetActive(true);
        this.KeyboardHandler.GetComponent<InputKeyboardHandler>().ActiveKeyboard(gameObject, this.InputField);
        this.SendButton.SetActive(true);
    }

    public void BackToSelectRoutine()
    {
        this.SetPatient.SetActive(false);
        this.RoutinesList.SetActive(true);
    }

    public void BackToManagementMenu()
    {
        this.RoutinesList.GetComponent<ViewRoutinesAndSelect>().Clean();
        this.RoutinesList.SetActive(false);
        this.ManagementMenu.SetActive(true);
    }

    public void Save()
    {
        RoutinePatientBody body = new RoutinePatientBody();
        body.active = true;
        body.routine = this.routine;
        body.patient = this.InputField.GetComponent<TextMesh>().text;

        StartCoroutine(SendCreateRoutinePatient(body));
        this.SetPatient.SetActive(false);
        this.RoutinesList.GetComponent<ViewRoutinesAndSelect>().Clean();
        this.ManagementMenu.GetComponent<ManagementMenu>().GoToMainMenu();

    }

    IEnumerator SendCreateRoutinePatient(RoutinePatientBody body)
    {
        UnityWebRequest webRequest = new UnityWebRequest("http://phyreup.francecentral.cloudapp.azure.com:3000/routine-patient", "POST");
        string jsonString = JsonUtility.ToJson(body);
        byte[] encodedPayload = new System.Text.UTF8Encoding().GetBytes(jsonString);
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(encodedPayload);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("cache-control", "no-cache");

        UnityWebRequestAsyncOperation requestHandel = webRequest.SendWebRequest();
        requestHandel.completed += delegate (AsyncOperation pOperation) {

        };


        yield return null;
    }
}


[Serializable]
public class RoutinePatientBody
{
    public string patient;
    public Routine routine;
    public bool active;

    public RoutinePatientBody()
    {

    }
}