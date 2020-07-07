using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class RoutineHandler : MonoBehaviour {

    public GameObject TextController;
    public GameObject RoutineHUD;

    /// View Routines list
    public GameObject DoRoutinesList;
    //Vector3[] positionRoutinesView = { new Vector3(-1.27f,-2.36f,-2.86f), new Vector3(-3.88f, -2.36f, -2.86f), new Vector3(-6.48f, -2.36f, -2.86f) };
    Vector3[] positionRoutinesView = { new Vector3(-0.06f, -0.006f, 0f), new Vector3(0.003f, -0.006f, 0f), new Vector3(0.065f, -0.006f, 0f) };

    public GameObject RoutineContainerView;
    public GameObject DoExerciseButton;
    public List<GameObject> VisualExerciseButtonList;
    int offset;
    int first = 3;
    public GameObject previousButton;
    public GameObject nextButton;

    /// Evaluation
    Evaluation[] actualEvaluation;
    public GameObject EvaluationText;

    /// Do routine
    public Routine ActualRoutine;
    
    // Rotuine
    public List<Routine> routines;

    int index;
    int totalRotuines;

    // RoutineResult
    DateTime startDate; 
    RoutineResult routineResult;
    bool problems;

    string patientKey;

    void Start () {
        
    }

    // Update is called once per frame
    void Update () {

    }
    
    public void Clean()
    {
        if(this.VisualExerciseButtonList != null)
        {
            int total = this.VisualExerciseButtonList.Count - 1;
            for (int i = total; i >= 0; i--)
            {
                GameObject obj = this.VisualExerciseButtonList[i];
                this.VisualExerciseButtonList.RemoveAt(i);
                Destroy(obj);
            }
            offset = 0;
            this.routines = null;
            this.VisualExerciseButtonList = null;
        }
    }

    public void OpenVideo()
    {
        Application.OpenURL(this.ActualRoutine.exercise.videoUrl);
    }

    public void GetAllRoutines()
    {
        StartCoroutine(GetRotuinesRequest());
    }

    public void SetPatientKey(string key)
    {
        this.patientKey = key;
    }

    public void Help()
    {

    }

    public void StopRoutine()
    {
        problems = true;
        gameObject.GetComponent<ExerciseHandler>().StopExercise();
        DateTime endDate = DateTime.Now;

        if(endDate.Subtract(startDate).TotalMinutes > this.ActualRoutine.duration){
            routineResult.inTime = false;
        }
        routineResult.endDate = endDate.ToString("o", CultureInfo.InvariantCulture);
        routineResult.complete = true;
        routineResult.problems = problems;
        routineResult.exerciseResult = gameObject.GetComponent<ExerciseHandler>().GetExerciseResultList() ;
        routineResult.routine = this.ActualRoutine;
        RoutineResult result = this.routineResult;
        StartCoroutine(PostSaveRoutineResult(result));

    }


    public void StartRoutine(Routine routine)
    {
        problems = false;
        routineResult = new RoutineResult();
        StartCoroutine(StartRoutineSequency(routine));
        this.DoRoutinesList.SetActive(false);
        this.RoutineContainerView.SetActive(false);
    }

    IEnumerator StartRoutineSequency(Routine routine)
    {
        this.TextController.SetActive(true);
        this.TextController.GetComponent<TextHandler>().SetText("");
        yield return new WaitForSeconds(0.01f);
        this.TextController.GetComponent<TextHandler>().DisableLast();
        this.TextController.GetComponent<TextHandler>().SetText("ExerciseInfo");
        //this.TextController.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 0.4f);
        yield return new WaitForSeconds(4);
        this.TextController.GetComponent<TextHandler>().DisableLast();

        RoutineHUD.SetActive(true);
        RoutineHUD.GetComponent<HUDHandler>().RestartCounters();
        this.TextController.GetComponent<TextHandler>().SetText("HelpInfo");
        //this.TextController.transform.position = Camera.main.transform.position + new Vector3(0f,0f,0.4f);
        yield return new WaitForSeconds(4);
        this.TextController.GetComponent<TextHandler>().DisableLast();

        this.TextController.GetComponent<TextHandler>().SetText("StopInfo");
        //this.TextController.transform.position = Camera.main.transform.position + new Vector3(0f,0f,0.4f);
        yield return new WaitForSeconds(4);
        this.TextController.GetComponent<TextHandler>().DisableLast();
        this.TextController.SetActive(false);

        this.ActualRoutine = routine;
        //index = 0;
        //totalRotuines = routines.Count;
        //routineResult.routine = routines[index].id;
        this.startDate = DateTime.Now;
        routineResult.startDate = startDate.ToString("o", CultureInfo.InvariantCulture);

        gameObject.GetComponent<ExerciseHandler>().SetExerciseAndStart(routine);
        
        yield return null;
        
    }


    public void FinishRoutine(ExerciseResult[] exerciseResultList)
    {

        DateTime endDate = DateTime.Now;
        if (endDate.Subtract(startDate).TotalMinutes > this.ActualRoutine.duration)
        {
            routineResult.inTime = false;
        }

        this.routineResult.endDate = endDate.ToString("o", CultureInfo.InvariantCulture);
        this.routineResult.complete = true;
        this.routineResult.problems = problems;
        this.routineResult.exerciseResult = exerciseResultList;
        this.routineResult.routine = this.ActualRoutine;
        RoutineResult result = this.routineResult;
        StartCoroutine(PostSaveRoutineResult(result));
        //this.DoRoutinesList.SetActive(true);
        //this.RoutineHUD.SetActive(false);
    }

    IEnumerator ShowEvaluation()
    {
        if (this.actualEvaluation != null)
        {
            this.EvaluationText.SetActive(true);
            this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableButtonCollection();
            bool same = false;
            this.EvaluationText.GetComponent<EvaluateTextHandler>().SetText("Same");
            yield return new WaitForSeconds(0.01f);
            this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableLast();
            Debug.Log(this.actualEvaluation);


            bool problems = false;
            for (int i = 0; i < this.actualEvaluation.Length && !problems; i++)
            {
                if (this.actualEvaluation[i] == Evaluation.SameFailures || this.actualEvaluation[i] == Evaluation.SameTime)
                {
                    same = true;
                }
                switch (this.actualEvaluation[i])
                {
                    case Evaluation.AlwaysProblems:
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().SetText("AlwaysProblems");
                        yield return new WaitForSeconds(3);
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableLast();
                        problems = true;
                        break;
                    case Evaluation.NowProblems:
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().SetText("NowProblems");
                        yield return new WaitForSeconds(3);
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableLast();
                        problems = true;
                        break;
                    case Evaluation.NowNotProblems:
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().SetText("NowNotProblems");
                        yield return new WaitForSeconds(3);
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableLast();
                        break;
                    case Evaluation.NowLessFailures:
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().SetText("NowLessFailures");
                        yield return new WaitForSeconds(3);
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableLast();
                        break;
                    case Evaluation.NowMoreFailures:
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().SetText("NowMoreFailures");
                        yield return new WaitForSeconds(3);
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableLast();
                        break;
                    case Evaluation.NowLessTime:
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().SetText("NowLessTime");
                        yield return new WaitForSeconds(3);
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableLast();
                        break;
                    case Evaluation.NowMoreTime:
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().SetText("NowMoreTime");
                        yield return new WaitForSeconds(3);
                        this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableLast();
                        break;
                }
            }
            if (same)
            {
                this.EvaluationText.GetComponent<EvaluateTextHandler>().SetText("Same");
                yield return new WaitForSeconds(3);
                this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableLast();
            }
            this.EvaluationText.GetComponent<EvaluateTextHandler>().EnableButtonCollection();
        }

       
        this.RoutineHUD.SetActive(false);
        
        yield return null;
    }

    public void Repeat()
    {
        this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableButtonCollection();
        this.EvaluationText.SetActive(false);
        this.StartRoutine(this.ActualRoutine);
    }
    public void Exit()
    {
        this.EvaluationText.GetComponent<EvaluateTextHandler>().DisableButtonCollection();
        this.EvaluationText.SetActive(false);
        this.DoRoutinesList.SetActive(true);
        this.RoutineContainerView.SetActive(true);
    }

    public void NextRoutine(ExerciseResult[] exerciseResultList){

        DateTime endDate = DateTime.Now;
        routineResult.endDate = endDate.ToString("o",CultureInfo.InvariantCulture);
        routineResult.complete = true;
        routineResult.problems = problems;
        routineResult.exerciseResult = exerciseResultList;
        RoutineResult result = this.routineResult;
        StartCoroutine(PostSaveRoutineResult(result));

    }


    // Funciones auxiliares
    string GetExerciseTextHub () {
        return (index + 1) + "/" + totalRotuines;
    }

    void OpenRoutineViewAndSetExercises()
    {
        this.DoRoutinesList.SetActive(true);
        this.RoutineContainerView.SetActive(true);

        this.totalRotuines = this.routines.Count();
        int final = (totalRotuines > first ? first : totalRotuines);
        this.VisualExerciseButtonList = new List<GameObject>();
        this.offset = 0;

        for (int i = 0; i < final; i++)
        {
            GameObject obj = Instantiate(this.DoExerciseButton);
            obj.transform.SetParent(this.RoutineContainerView.transform);
            obj.transform.localEulerAngles = new Vector3(0, 0, 0);
            obj.transform.position = this.RoutineContainerView.transform.position;
            obj.transform.localPosition = positionRoutinesView[i];
            Debug.Log(positionRoutinesView[i]);
            Debug.Log(obj.transform.position);
            obj.GetComponent<ExerciseComponentHandler>().SetRoutine(this.routines[i]);
            obj.SetActive(true);
            VisualExerciseButtonList.Add(obj);
        }

        if(totalRotuines > first)
        {
            this.previousButton.SetActive(false);
            this.nextButton.SetActive(true);
        }
        else
        {
            this.nextButton.SetActive(false);
            this.previousButton.SetActive(false);
        }
    }
    public void NextPagination()
    {
       for(int i = VisualExerciseButtonList.Count - 1; i >= 0; i--)
        {
            GameObject aux = this.VisualExerciseButtonList[i];
            this.VisualExerciseButtonList.RemoveAt(i);
            Destroy(aux);
        }

       this.offset += first;
       if(offset < this.totalRotuines)
        {
            for(int i = offset; i < (offset + first) && i < totalRotuines; i++)
            {
                GameObject obj = Instantiate(this.DoExerciseButton);
                obj.transform.SetParent(this.RoutineContainerView.transform);
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.position = this.RoutineContainerView.transform.position;
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<ExerciseComponentHandler>().SetRoutine(this.routines[i]);
                obj.SetActive(true);
                VisualExerciseButtonList.Add(obj);
            }
        }
        this.previousButton.SetActive(true);
        if(offset < this.totalRotuines-1)
        {
            this.nextButton.SetActive(true);
        } 
        else
        {
            this.nextButton.SetActive(false);
        }
    }

    public void PreviousPagination()
    {
        for (int i = VisualExerciseButtonList.Count - 1; i >= 0; i--)
        {
            GameObject aux = this.VisualExerciseButtonList[i];
            this.VisualExerciseButtonList.RemoveAt(i);
            Destroy(aux);
        }

        this.offset -= first;
        if (offset < this.totalRotuines)
        {
            for (int i = offset; i < (offset + first) && i < totalRotuines; i++)
            {
                GameObject obj = Instantiate(this.DoExerciseButton);
                obj.transform.SetParent(this.RoutineContainerView.transform);
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.position = this.RoutineContainerView.transform.position;
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<ExerciseComponentHandler>().SetRoutine(this.routines[i]);
                obj.SetActive(true);
                VisualExerciseButtonList.Add(obj);
            }
        }
        this.nextButton.SetActive(true);
        if (offset == 0)
        {
            this.previousButton.SetActive(false);
        }
        else
        {
            this.previousButton.SetActive(true);
        }
    }

    // Get routines by patient request
    IEnumerator GetRotuinesRequest () {
        UnityWebRequest webRequest = new UnityWebRequest ("http://phyreup.francecentral.cloudapp.azure.com:3000/routine-patient/patient/"+this.patientKey, "GET");
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
            OpenRoutineViewAndSetExercises();
            //StartRoutines ();

        };

        yield return null;

    }

    IEnumerator PostSaveRoutineResult (RoutineResult result) {
        UnityWebRequest webRequest = new UnityWebRequest ("http://phyreup.francecentral.cloudapp.azure.com:3000/routine-result/"+this.patientKey, "POST");
        Debug.Log(this.patientKey);
        string jsonString = JsonUtility.ToJson (result);
        byte[] encodedPayload = new System.Text.UTF8Encoding ().GetBytes (jsonString);
        webRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw (encodedPayload);
        webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer ();
        webRequest.SetRequestHeader ("Content-Type", "application/json");
        webRequest.SetRequestHeader ("cache-control", "no-cache");
        this.actualEvaluation = null;

        UnityWebRequestAsyncOperation requestHandel = webRequest.SendWebRequest ();
        requestHandel.completed += delegate (AsyncOperation pOperation) {
            string jsonStringResponse = webRequest.downloadHandler.text;
            EvaluationResult evaResult = JsonUtility.FromJson<EvaluationResult>(jsonStringResponse);
            Debug.Log(evaResult);
            this.actualEvaluation = evaResult.evaluation;
            StartCoroutine(ShowEvaluation());
        };

        
        yield return null;
    }
}