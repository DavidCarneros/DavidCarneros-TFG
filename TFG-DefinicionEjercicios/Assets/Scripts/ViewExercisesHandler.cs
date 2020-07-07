using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ViewExercisesHandler : MonoBehaviour
{
    public GameObject ViewExerciseComponent;
    Vector3[] positionRoutinesView = { new Vector3(-0.06f, -0.006f, 0f), new Vector3(0.003f, -0.006f, 0f), new Vector3(0.065f, -0.006f, 0f) };
    //Vector3[] positionRoutinesView = { new Vector3(-1.27f, -2.36f, -2.86f), new Vector3(-3.88f, -2.36f, -2.86f), new Vector3(-6.48f, -2.36f, -2.86f) };
    public List<GameObject> VisualExerciseButtonList;
    int offset;
    int first = 3;
    public GameObject previousButton;
    public GameObject nextButton;

    public Exercise actualExercise;
    public List<Exercise> exercises;
    int totalExercises;
    // Start is called before the first frame update

    /// <DRAW>
    public GameObject TorusObject;
    public GameObject PointObject;
    public List<GameObject> VisualPoints;
    public List<Vector3> VisualPointsPosition;
    public GameObject ExercisesObjectList;
    public GameObject ExerciseListContainer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllExercises()
    {
        StartCoroutine(GetRoutinesRequest());
    }

    public void SetExerciseAndDraw(Exercise exercise)
    {
        this.actualExercise = exercise;
        this.ExerciseListContainer.SetActive(false);
        DrawExercise();
        gameObject.SetActive(false);
    }

    public void Clean()
    {
        int total;
        if (VisualPoints != null)
        {
            total = VisualPoints.Count - 1;
            if (total >= 0)
            {
                for (int i = total; i >= 0; i--)
                {
                    GameObject aux = this.VisualPoints[i];
                    this.VisualPoints.RemoveAt(i);
                    Destroy(aux);
                }
            }
        }
        this.VisualPoints = null;
        this.VisualPointsPosition = null;
        this.exercises = null;
        total = this.VisualExerciseButtonList.Count - 1;
        if(total >= 0)
        {
            for(int i = total; i >= 0; i--)
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
            obj.transform.SetParent(this.ExerciseListContainer.transform);
            obj.transform.position = gameObject.transform.position;
            obj.transform.localEulerAngles = new Vector3(0, 0, 0);
            obj.transform.localPosition = positionRoutinesView[i];
            obj.GetComponent<ViewExerciseComponentHandler>().SetExercise(this.exercises[i]);
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
                obj.transform.SetParent(this.ExerciseListContainer.transform);
                obj.transform.position = gameObject.transform.position;
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<ViewExerciseComponentHandler>().SetExercise(this.exercises[i]);
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
                obj.transform.SetParent(this.ExerciseListContainer.transform);
                obj.transform.position = gameObject.transform.position;
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                obj.transform.localPosition = positionRoutinesView[i % 3];
                obj.GetComponent<ViewExerciseComponentHandler>().SetExercise(this.exercises[i]);
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

    public void DrawExercise()
    {
        this.ExercisesObjectList.SetActive(true);
        VisualPoints = new List<GameObject>();
        VisualPointsPosition = new List<Vector3>();

        int bt = this.actualExercise.keyPoint + 1;

        for (int i = 0; i < this.actualExercise.points.Count; i++)
        {

            // Torus
            if (i != 0 && (i % bt == (actualExercise.keyPoint)))
            {
                var before = this.actualExercise.points[i];
                if (i + 1 >= this.actualExercise.points.Count)
                {
                    before = this.actualExercise.points[i];
                }
                else
                {
                    before = this.actualExercise.points[i + 1];
                }
                //var before = this.exercise.points[i + 1];
                var vector = before - this.actualExercise.points[i - 1];

                var rotation = Quaternion.LookRotation(vector.normalized);
                Quaternion newRotation = rotation * Quaternion.Euler(-180, 90, 90);
                Vector3 torusPosition = new Vector3(this.actualExercise.points[i].x, this.actualExercise.points[i].y, this.actualExercise.points[i].z) + Camera.main.transform.position;
                Vector3 centralPoint = this.actualExercise.points[i] + Camera.main.transform.position;
                /*
                if (vector.normalized.x != 0.0 || vector.normalized.z != 0.0)
                {
                    torusPosition = torusPosition + new Vector3(0, -0.05f, 0);
                }
                if (vector.normalized.y != 0.0)
                {
                    if (vector.normalized.y > 0.0)
                    {
                        torusPosition = torusPosition + new Vector3(0, 0, +0.05f);
                    }
                    else
                    {
                        torusPosition = torusPosition + new Vector3(0, 0, -0.05f);
                    }
                }
                */

                GameObject torusPoint = Instantiate(this.TorusObject, torusPosition, Quaternion.identity);
                torusPoint.tag = "Torus";
                torusPoint.transform.rotation = newRotation;
                torusPoint.transform.parent = this.ExercisesObjectList.transform;
                torusPoint.SetActive(true);
                this.VisualPoints.Add(torusPoint);
                this.VisualPointsPosition.Add(centralPoint);
            }
            // Point
            else
            {
                Vector3 pointPosition = this.actualExercise.points[i] + Camera.main.transform.position;
                GameObject spherePoint = Instantiate(this.PointObject, pointPosition, Quaternion.identity);
                spherePoint.transform.parent = this.ExercisesObjectList.transform;
                spherePoint.tag = "Sphere";

                spherePoint.SetActive(true);

                this.VisualPoints.Add(spherePoint);
                this.VisualPointsPosition.Add(spherePoint.transform.position);
            }
        }

        VisualPoints[0].transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        VisualPoints[0].GetComponent<MeshRenderer>().material.color = Color.yellow;

    }

    IEnumerator GetRoutinesRequest()
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
