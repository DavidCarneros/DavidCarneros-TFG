using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class RoutineHandler : MonoBehaviour {

    public GameObject RoutineHUD;
    public GameObject StartRoutinesText;
    public GameObject NextExerciseText;
    public GameObject FinishRoutineText;
    public GameObject HelpText;
    public GameObject ProblemText;

    // Rotuine
    public List<Routine> routines;

    int index;
    int totalRotuines;

    // RoutineResult
    RoutineResult routineResult;
    bool problems;

    void Start () {
        
    }

    // Update is called once per frame
    void Update () {

    }

    public void DEBUG(){
        StartCoroutine (GetRotuinesRequest ());
    }


    // Start rotuines
    void StartRoutines () {
        /*
        index = 0;
        totalRotuines = routines.Count;
        RoutineHUD.SetActive(true);
        gameObject.GetComponent<ExerciseHandler>().SetExerciseAndStart(routines[index]);
        RoutineHUD.GetComponent<HUDHandler>().SetTextRoutine(GetExerciseTextHub ());
        */
        problems = false;
        routineResult = new RoutineResult();
        StartCoroutine(StartRoutinesSequency());
    }

    IEnumerator StartRoutinesSequency(){
        this.StartRoutinesText.SetActive(true);
        this.StartRoutinesText.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 0.4f);
        yield return new WaitForSeconds(3);
        this.StartRoutinesText.SetActive(false);

        RoutineHUD.SetActive(true);
        this.HelpText.SetActive(true);
        this.HelpText.transform.position = Camera.main.transform.position + new Vector3(0f,0f,0.4f);
        yield return new WaitForSeconds(3);
        this.HelpText.SetActive(false);
        this.ProblemText.SetActive(true);
        this.ProblemText.transform.position = Camera.main.transform.position + new Vector3(0f,0f,0.4f);
        yield return new WaitForSeconds(3);
        this.ProblemText.SetActive(false);

        index = 0;
        totalRotuines = routines.Count;
        routineResult.routine = routines[index].id;
        DateTime startDate = DateTime.Now;
        routineResult.startDate = startDate.ToString("o",CultureInfo.InvariantCulture);

        gameObject.GetComponent<ExerciseHandler>().SetExerciseAndStart(routines[index]);
        RoutineHUD.GetComponent<HUDHandler>().SetTextRoutine(GetExerciseTextHub ());

        yield return null;

    }

    public void NextRoutine(ExerciseResult[] exerciseResultList){

        DateTime endDate = DateTime.Now;
        routineResult.endDate = endDate.ToString("o",CultureInfo.InvariantCulture);
        routineResult.complete = true;
        routineResult.problems = problems;
        routineResult.exerciseResult = exerciseResultList;
        RoutineResult result = this.routineResult;
        StartCoroutine(PostSaveRoutineResult(result));
        StartCoroutine(NextRoutineSecuency());
        /*
        index++;
        if(index < totalRotuines) {
            gameObject.GetComponent<ExerciseHandler>().SetExerciseAndStart(routines[index]);
            RoutineHUD.GetComponent<HUDHandler>().SetTextRoutine(GetExerciseTextHub ());
        }
        */
    }

    IEnumerator NextRoutineSecuency(){
        index++;
        if(index < totalRotuines) {
            routineResult.routine = routines[index].id;
            this.NextExerciseText.SetActive(true);
            this.NextExerciseText.transform.position = Camera.main.transform.position + new Vector3(0f,0f,0.4f);
            yield return new WaitForSeconds(3);
            this.NextExerciseText.SetActive(false);

            DateTime startDate = DateTime.Now;
            routineResult.startDate = startDate.ToString("o",CultureInfo.InvariantCulture);
            gameObject.GetComponent<ExerciseHandler>().SetExerciseAndStart(routines[index]);
            RoutineHUD.GetComponent<HUDHandler>().SetTextRoutine(GetExerciseTextHub ());
        }
        else {
            //DateTime endDate = DateTime.Now;
            //routineResult.endDate = endDate.ToString("o",CultureInfo.InvariantCulture);
            //routineResult.complete = true;
            //routineResult.problems = problems;
            this.FinishRoutineText.SetActive(true);
            this.FinishRoutineText.transform.position = Camera.main.transform.position + new Vector3(0f,0f,0.4f);
            yield return new WaitForSeconds(3);

            this.FinishRoutineText.SetActive(false);
        }
        yield return null;
    }

    // Funciones auxiliares
    string GetExerciseTextHub () {
        return (index + 1) + "/" + totalRotuines;
    }

    // Get routines by patient request
    IEnumerator GetRotuinesRequest () {
        UnityWebRequest webRequest = new UnityWebRequest ("http://192.168.1.163:3000/routine/patient/e-1234", "GET");
        webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer ();

        UnityWebRequestAsyncOperation requestHandler = webRequest.SendWebRequest ();
        requestHandler.completed += delegate (AsyncOperation pOperation) {
            string jsonString = webRequest.downloadHandler.text;
            jsonString = JsonHelper.FixJson (jsonString);
            Routine[] routineArray = JsonHelper.FromJson<Routine> (jsonString);
            this.routines = new List<Routine> ();
            for (int i = 0; i < routineArray.Length; i++) {
                routines.Add (routineArray[i]);
            }
            StartRoutines ();
        };

        yield return null;

    }

    IEnumerator PostSaveRoutineResult (RoutineResult result) {
        UnityWebRequest webRequest = new UnityWebRequest ("http://192.168.1.163:3000/routine-result", "POST");
        string jsonString = JsonUtility.ToJson (result);
        byte[] encodedPayload = new System.Text.UTF8Encoding ().GetBytes (jsonString);
        Debug.Log(jsonString);
        webRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw (encodedPayload);
        webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer ();
        webRequest.SetRequestHeader ("Content-Type", "application/json");
        webRequest.SetRequestHeader ("cache-control", "no-cache");

        UnityWebRequestAsyncOperation requestHandel = webRequest.SendWebRequest ();
        requestHandel.completed += delegate (AsyncOperation pOperation) {
            Debug.Log (webRequest.responseCode);
            Debug.Log (webRequest.downloadHandler.text);
        };

        yield return null;
    }
}