using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ViewRoutineHandler : MonoBehaviour
{

    public GameObject ViewRoutineList;
    public GameObject ViewExerciseButton;
    //Vector3[] positionRoutinesView = { new Vector3(-1.27f, -2.36f, -2.86f), new Vector3(-3.88f, -2.36f, -2.86f), new Vector3(-6.48f, -2.36f, -2.86f) };
    Vector3[] positionRoutinesView = { new Vector3(-0.06f, -0.006f, 0f), new Vector3(0.003f, -0.006f, 0f), new Vector3(0.065f, -0.006f, 0f) };

    public GameObject RoutineListContainer;
    public List<GameObject> VisualExerciseButtonList;
    int offset;
    int first = 3;
    public GameObject previousButton;
    public GameObject nextButton;

    public Routine actualRoutine;
    public List<Routine> routines;
    int totalRotuines;

    public string patientKey;

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
        StartCoroutine(GetRoutinesRequest());
    }

    public void Clean()
    {
        if(this.VisualExerciseButtonList != null)
        {
            int total = this.VisualExerciseButtonList.Count - 1;
            for(int i = total; i >= 0; i--)
            {
                GameObject obj = this.VisualExerciseButtonList[i];
                this.VisualExerciseButtonList.RemoveAt(i);
                Destroy(obj);
            }
            offset = 0;
            this.routines = null;
            this.VisualExerciseButtonList = null;
            this.RoutineListContainer.SetActive(false);
        }
    }

    public void SetPatientKey(string key)
    {
        this.patientKey = key;
    }

    public void ViewRoutine(Routine routine)
    {
        this.ViewRoutineList.SetActive(false);
        this.RoutineListContainer.SetActive(false);
        gameObject.GetComponent<ViewExerciseHandler>().SetExerciseAndDraw(routine);
    }

    void OpenRoutineViewAndSetExercises()
    {
        this.ViewRoutineList.SetActive(true);
        this.RoutineListContainer.SetActive(true);

        this.totalRotuines = this.routines.Count();
        int final = (totalRotuines > first ? first : totalRotuines);
        this.VisualExerciseButtonList = new List<GameObject>();
        this.offset = 0;

        for (int i = 0; i < final; i++)
        {
            GameObject obj = Instantiate(this.ViewExerciseButton);
            obj.transform.SetParent(this.RoutineListContainer.transform);
            obj.transform.localEulerAngles = new Vector3(0, 0, 0);
            obj.transform.position = gameObject.transform.position;
            obj.transform.localPosition = positionRoutinesView[i];
            obj.GetComponent<ViewExerciseComponentHandler>().SetRoutine(this.routines[i]);
            obj.SetActive(true);
            VisualExerciseButtonList.Add(obj);
        }

        if (totalRotuines > first)
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
        if (offset < this.totalRotuines)
        {
            for (int i = offset; i < (offset + first) && i < totalRotuines; i++)
            {
                GameObject obj = Instantiate(this.ViewExerciseButton);
                obj.transform.SetParent(this.RoutineListContainer.transform);
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.position = gameObject.transform.position;
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<ViewExerciseComponentHandler>().SetRoutine(this.routines[i]);
                obj.SetActive(true);
                VisualExerciseButtonList.Add(obj);
            }
        }
        this.previousButton.SetActive(true);
        if (offset < this.totalRotuines - 1)
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
                GameObject obj = Instantiate(this.ViewExerciseButton);
                obj.transform.SetParent(this.RoutineListContainer.transform);
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.position = gameObject.transform.position;
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<ViewExerciseComponentHandler>().SetRoutine(this.routines[i]);
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

    IEnumerator GetRoutinesRequest()
    {
        UnityWebRequest webRequest = new UnityWebRequest("http://phyreup.francecentral.cloudapp.azure.com:3000/routine-patient/patient/" + patientKey, "GET");
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        UnityWebRequestAsyncOperation requestHandler = webRequest.SendWebRequest();
        requestHandler.completed += delegate (AsyncOperation pOperation) {
            string jsonString = webRequest.downloadHandler.text;
            jsonString = JsonHelper.FixJson(jsonString);
            Routine[] routineArray = JsonHelper.FromJson<Routine>(jsonString);
            this.routines = new List<Routine>();
            if(routineArray.Length > 0)
            {
                for (int i = 0; i < routineArray.Length; i++)
                {
                    routines.Add(routineArray[i]);
                }
            }
            
            OpenRoutineViewAndSetExercises();
            //StartRoutines ();

        };

        yield return null;

    }
}
