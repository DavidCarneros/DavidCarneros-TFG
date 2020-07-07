using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ViewExerciseAndSelect : MonoBehaviour
{
    public GameObject ViewExerciseComponent;
    //Vector3[] positionRoutinesView = { new Vector3(-1.27f, -2.36f, -2.86f), new Vector3(-3.88f, -2.36f, -2.86f), new Vector3(-6.48f, -2.36f, -2.86f) };
    Vector3[] positionRoutinesView = { new Vector3(-0.06f, -0.006f, -0.01f), new Vector3(0.003f, -0.006f, -0.01f), new Vector3(0.065f, -0.006f, -0.01f) };

    public List<GameObject> VisualExerciseButtonList;
    int offset;
    int first = 3;
    public GameObject previousButton;
    public GameObject nextButton;

    public GameObject ExerciseContainerView;

    public List<Exercise> exercises;
    int totalExercises;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clean()
    {
        int total = this.VisualExerciseButtonList.Count - 1;
        if (total >= 0)
        {
            for (int i = total; i >= 0; i--)
            {
                GameObject aux = this.VisualExerciseButtonList[i];
                this.VisualExerciseButtonList.RemoveAt(i);
                Destroy(aux);
            }
        }
        this.VisualExerciseButtonList = null;
    }

    void OpenRoutineViewAndSetExercises()
    {

        //gameObject.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 0.6f);

        this.totalExercises = this.exercises.Count();
        int final = (totalExercises > first ? first : totalExercises);
        this.VisualExerciseButtonList = new List<GameObject>();
        this.offset = 0;

        for (int i = 0; i < final; i++)
        {
            GameObject obj = Instantiate(this.ViewExerciseComponent);
            obj.transform.SetParent(this.ExerciseContainerView.transform);
            obj.transform.position = gameObject.transform.position;
            obj.transform.localEulerAngles = new Vector3(0, 0, 0);
            obj.transform.localPosition = positionRoutinesView[i];
            obj.GetComponent<SelectExerciseComponentHandler>().SetExercise(this.exercises[i]);
            obj.SetActive(true);
            VisualExerciseButtonList.Add(obj);
        }

        if (totalExercises > first)
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
        for (int i = VisualExerciseButtonList.Count - 1; i >= 0; i--)
        {
            GameObject aux = this.VisualExerciseButtonList[i];
            this.VisualExerciseButtonList.RemoveAt(i);
            Destroy(aux);
        }

        this.offset += first;
        if (offset < this.totalExercises)
        {
            for (int i = offset; i < (offset + first) && i < totalExercises; i++)
            {
                GameObject obj = Instantiate(this.ViewExerciseComponent);
                obj.transform.SetParent(this.ExerciseContainerView.transform);
                obj.transform.position = gameObject.transform.position;
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<SelectExerciseComponentHandler>().SetExercise(this.exercises[i]);
                obj.SetActive(true);
                VisualExerciseButtonList.Add(obj);
            }
        }
        this.previousButton.SetActive(true);
        if (offset < this.totalExercises - 1)
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
        if (offset < this.totalExercises)
        {
            for (int i = offset; i < (offset + first) && i < totalExercises; i++)
            {
                GameObject obj = Instantiate(this.ViewExerciseComponent);
                obj.transform.SetParent(this.ExerciseContainerView.transform);
                obj.transform.position = gameObject.transform.position;
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<SelectExerciseComponentHandler>().SetExercise(this.exercises[i]);
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

    public void GetAllExercises()
    {
        StartCoroutine(GetExercisesRequest());
    }

    IEnumerator GetExercisesRequest()
    {
        UnityWebRequest webRequest = new UnityWebRequest("http://phyreup.francecentral.cloudapp.azure.com:3000/exercise", "GET");
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        UnityWebRequestAsyncOperation requestHandler = webRequest.SendWebRequest();
        requestHandler.completed += delegate (AsyncOperation pOperation) {
            string jsonString = webRequest.downloadHandler.text;
            jsonString = JsonHelper.FixJson(jsonString);
            Exercise[] exercisesArray = JsonHelper.FromJson<Exercise>(jsonString);
            this.exercises = new List<Exercise>();
            for (int i = 0; i < exercisesArray.Length; i++)
            {
                exercises.Add(exercisesArray[i]);
            }
            OpenRoutineViewAndSetExercises();
            //StartRoutines ();

        };

        yield return null;

    }
}
