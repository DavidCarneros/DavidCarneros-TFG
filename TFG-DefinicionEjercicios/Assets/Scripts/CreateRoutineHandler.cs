using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CreateRoutineHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ExerciseListContainer;

    public GameObject ExerciseList;
    public GameObject SetRepetitions;
    public GameObject ManagementMenu;

    public GameObject MoreRepetitions;
    public GameObject LessRepetitions;
    public GameObject RepetitionText;

    public GameObject MoreDuration;
    public GameObject LessDuration;
    public GameObject DurationText;


    public Exercise exercise;
    int repetitions;
    int duration;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllExercises()
    {
        this.ExerciseList.SetActive(true);
        this.ExerciseListContainer.SetActive(true);
        this.ExerciseList.GetComponent<ViewExerciseAndSelect>().GetAllExercises();
    }

    public void SetExerciseAndGoNextStep(Exercise exercise)
    {
        this.exercise = exercise;
        this.ExerciseList.SetActive(false);
        this.ExerciseListContainer.SetActive(false);
        //this.ExerciseList.GetComponent<ViewExerciseAndSelect>().Clean();
        this.SetRepetitions.SetActive(true);
        repetitions = 1;
        duration = 15;
        this.DurationText.GetComponent<TextMesh>().text = duration.ToString() + " min";
    }

    public void IncrementRepetitionst()
    {
        repetitions++;
        if (repetitions > 1)
        {
            this.LessRepetitions.SetActive(true);
        }
        this.RepetitionText.GetComponent<TextMesh>().text = repetitions.ToString();
    }

    public void DecrementRepetitions()
    {
        repetitions--;
        if (repetitions == 1)
        {
            Debug.Log("Entra en repetitios == 1");
            this.LessRepetitions.SetActive(false);
        }
        this.RepetitionText.GetComponent<TextMesh>().text = repetitions.ToString();


    }

    public void IncrementDuration()
    {
        duration++;
        if (duration > 1)
        {
            this.LessDuration.SetActive(true);
        }
        this.DurationText.GetComponent<TextMesh>().text = duration.ToString() + " min";
    }

    public void DecrementDuration()
    {
        duration--;
        if (duration == 1)
        {
            this.LessDuration.SetActive(false);
        }
        this.DurationText.GetComponent<TextMesh>().text = duration.ToString() + " min";


    }

    public void BackToSelectExercise()
    {
        this.ExerciseListContainer.SetActive(true);
        this.SetRepetitions.SetActive(false);
        this.ExerciseList.SetActive(true);
    }

    public void BackToManagementMenu()
    {
        this.ExerciseList.GetComponent<ViewExerciseAndSelect>().Clean();
        this.ExerciseList.SetActive(false);
        this.ManagementMenu.SetActive(true);
    }

    public void SaveRoutine()
    {
        Routine routine = new Routine();
        routine.exercise = this.exercise;
        routine.repetitions = this.repetitions;
        routine.duration = this.duration;
        routine.active = true;

        StartCoroutine(PostSaveRoutine(routine));
        this.SetRepetitions.SetActive(false);
        this.ExerciseList.GetComponent<ViewExerciseAndSelect>().Clean();
        this.ManagementMenu.GetComponent<ManagementMenu>().GoToMainMenu();
    }

    IEnumerator PostSaveRoutine(Routine routine)
    {
        UnityWebRequest webRequest = new UnityWebRequest("http://phyreup.francecentral.cloudapp.azure.com:3000/routine", "POST");
        string jsonString = JsonUtility.ToJson(routine);
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
