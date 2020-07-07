using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ViewRoutinesAndSelect : MonoBehaviour
{
    public GameObject ViewRoutinesComponent;
    //Vector3[] positionRoutinesView = { new Vector3(-1.33f, -2.36f, -2.55f), new Vector3(-3.88f, -2.36f, -2.55f), new Vector3(-6.38f, -2.36f, -2.55f) };
    //Vector3[] positionRoutinesView = { new Vector3(-0.2924f, -0.0108f, -0.0609f), new Vector3(-0.2924f, -0.0108f, -0.0609f), new Vector3(-0.1674f, -0.0108f, -0.0609f) };
    Vector3[] positionRoutinesView = { new Vector3(-0.06f, -0.006f, 0f), new Vector3(0.003f, -0.006f, 0f), new Vector3(0.065f, -0.006f, 0f) };

    public List<GameObject> VisualRoutineButtonList;
    int offset;
    int first = 3;
    public GameObject previousButton;
    public GameObject nextButton;

    public GameObject RoutinesContainerView;
    public List<Routine> routines;
    int totalRoutine;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clean()
    {
        int total = this.VisualRoutineButtonList.Count - 1;
        if (total >= 0)
        {
            for (int i = total; i >= 0; i--)
            {
                GameObject aux = this.VisualRoutineButtonList[i];
                this.VisualRoutineButtonList.RemoveAt(i);
                Destroy(aux);
            }
        }
        this.VisualRoutineButtonList = null;
    }

    void OpenRoutineViewAndSetRoutine()
    {

        this.totalRoutine = this.routines.Count();
        int final = (totalRoutine > first ? first : totalRoutine);
        this.VisualRoutineButtonList = new List<GameObject>();
        this.offset = 0;

        for (int i = 0; i < final; i++)
        {
            GameObject obj = Instantiate(this.ViewRoutinesComponent);
            obj.transform.SetParent(this.RoutinesContainerView.transform);
            obj.transform.position = gameObject.transform.position;
            obj.transform.localEulerAngles = new Vector3(0, 0, 0);
            obj.transform.localPosition = positionRoutinesView[i];
            obj.GetComponent<SelectRoutineComponentHandler>().SetRoutine(this.routines[i]);
            obj.SetActive(true);
            VisualRoutineButtonList.Add(obj);
        }

        if (totalRoutine > first)
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
        for (int i = VisualRoutineButtonList.Count - 1; i >= 0; i--)
        {
            GameObject aux = this.VisualRoutineButtonList[i];
            this.VisualRoutineButtonList.RemoveAt(i);
            Destroy(aux);
        }

        this.offset += first;
        if (offset < this.totalRoutine)
        {
            for (int i = offset; i < (offset + first) && i < this.totalRoutine; i++)
            {
                GameObject obj = Instantiate(this.ViewRoutinesComponent);
                obj.transform.SetParent(this.RoutinesContainerView.transform);
                obj.transform.position = gameObject.transform.position;
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<SelectRoutineComponentHandler>().SetRoutine(this.routines[i]);
                obj.SetActive(true);
                VisualRoutineButtonList.Add(obj);
            }
        }
        this.previousButton.SetActive(true);
        if (offset < this.totalRoutine - 1)
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
        for (int i = VisualRoutineButtonList.Count - 1; i >= 0; i--)
        {
            GameObject aux = this.VisualRoutineButtonList[i];
            this.VisualRoutineButtonList.RemoveAt(i);
            Destroy(aux);
        }

        this.offset -= first;
        if (offset < this.totalRoutine)
        {
            for (int i = offset; i < (offset + first) && i < this.totalRoutine; i++)
            {
                GameObject obj = Instantiate(this.ViewRoutinesComponent);
                obj.transform.SetParent(this.RoutinesContainerView.transform);
                obj.transform.position = gameObject.transform.position;
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<SelectRoutineComponentHandler>().SetRoutine(this.routines[i]);
                obj.SetActive(true);
                VisualRoutineButtonList.Add(obj);
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

    public void GetAllRoutines()
    {
        StartCoroutine(GetRoutinesRequest());
    }

    IEnumerator GetRoutinesRequest()
    {
        UnityWebRequest webRequest = new UnityWebRequest("http://phyreup.francecentral.cloudapp.azure.com:3000/routine/exercises", "GET");
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        UnityWebRequestAsyncOperation requestHandler = webRequest.SendWebRequest();
        requestHandler.completed += delegate (AsyncOperation pOperation) {
            string jsonString = webRequest.downloadHandler.text;
            jsonString = JsonHelper.FixJson(jsonString);
            Routine[] routineArray = JsonHelper.FromJson<Routine>(jsonString);
            this.routines = new List<Routine>();
            for (int i = 0; i < routineArray.Length; i++)
            {
                routines.Add(routineArray[i]);
            }
            OpenRoutineViewAndSetRoutine();
            //StartRoutines ();

        };

        yield return null;

    }
}
