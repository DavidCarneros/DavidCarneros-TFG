using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DefineExerciseHandler : MonoBehaviour {
    // Views
    public GameObject ViewExercise;
    public GameObject InputKeyboardHandler;

    // MENUS
    public GameObject DefineExerciseMenu;
    public GameObject HandSelectorMenu;
    public GameObject PointDistanceMenu;
    public GameObject BackSelectorMenu;
    public GameObject SaveExerciseMenu;

    public GameObject MoreKeyPoint;
    public GameObject LessKeyPoint;
    public GameObject KeyPointText;
    public GameObject MoreDistance;
    public GameObject LessDistance;
    public GameObject DistanceText;


    public GameObject textName;
    public GameObject textPlaceholder;

    // Hand Pointer
    public GameObject HandsTrackingHandler;
    public GameObject HandPointer;
    public GameObject Point;

    // Exercise objects
    public GameObject TorusObject;
    public GameObject PointObject;

    Exercise exercise;

    // auxiliars
    float pointDistance;
    int keyPoint;
    bool back;
    Vector3 HandPosition;
    Vector3 HandPositionSave;
    List<Vector3> ExercisePoints;
    List<GameObject> VisualPoints;
    List<Vector3> VisualPointsPosition;

    ////////////
    List<Vector3> testingData;
    ////////////


    bool recording;

    void Start () {
        InitComponent();
    }

    public void InitComponent(){
        exercise = new Exercise ();
        ExercisePoints = new List<Vector3>();
        VisualPointsPosition = new List<Vector3>();
        
        recording = false;
        back = false;
        this.TorusObject.SetActive (false);
        this.PointObject.SetActive (false);

        this.DefineExerciseMenu.SetActive(true);
        this.HandSelectorMenu.SetActive (true);
        this.PointDistanceMenu.SetActive (false);
        this.BackSelectorMenu.SetActive (false);
        this.SaveExerciseMenu.SetActive (false);
    }

    public void BackToSetHand()
    {
        this.PointDistanceMenu.SetActive(false);
        this.HandSelectorMenu.SetActive(true);
    }

    public void BackToPointSelector()
    {
        if(this.ExercisePoints != null)
        {
            int total = VisualPoints.Count - 1;
            for(int i = total; i >= 0; i--)
            {
                GameObject obj = VisualPoints[i];
                this.VisualPoints.RemoveAt(i);
                Destroy(obj);
            }
            this.VisualPoints = null;
            this.VisualPointsPosition = null;
            this.ExercisePoints = null;
        }
        this.PointDistanceMenu.SetActive(true);
        this.BackSelectorMenu.SetActive(false);
    }

    public void BackToBackSelector()
    {
        this.BackSelectorMenu.SetActive(true);
        this.SaveExerciseMenu.SetActive(false);
    }

    public void Clean(){
        if(VisualPoints != null)
        {
            int total = VisualPoints.Count - 1;
            if (total >= 0)
            {
                for (int i = total; i >= 0; i--)
                {
                    GameObject aux = VisualPoints[i];
                    VisualPoints.RemoveAt(i);
                    Destroy(aux);
                }
            }
            ExercisePoints = null;
            VisualPointsPosition = null;
        }
       
    }

    // Update is called once per frame
    void Update () {
        if (recording) {
            HandPosition = HandPointer.transform.position;
            HandPositionSave = HandPosition - Camera.main.transform.position;
            if (this.GetConfiance() >= 2)
            {
                this.testingData.Add(HandPositionSave);
                if (ExercisePoints.Count == 0)
                {
                    GameObject obj = Instantiate(Point, this.transform);
                    obj.transform.position = HandPosition;
                    obj.transform.parent = ViewExercise.transform;
                    obj.SetActive(true);
                    VisualPoints.Add(obj);
                    VisualPointsPosition.Add(HandPosition);
                    ExercisePoints.Add(HandPositionSave);
                }
                if (Vector3.Distance(HandPosition, VisualPointsPosition[VisualPointsPosition.Count - 1]) >= pointDistance)
                {
                    GameObject obj = Instantiate(Point, this.transform);
                    obj.transform.position = HandPosition;
                    obj.transform.parent = ViewExercise.transform;
                    obj.SetActive(true);
                    VisualPoints.Add(obj);
                    VisualPointsPosition.Add(HandPosition);
                    ExercisePoints.Add(HandPositionSave);
                }
            }
        }
    }

    public int GetConfiance()
    {
        if(this.exercise.hand == "Left")
        {
            return this.HandPointer.GetComponent<OnPointsLeftReceivedHandler>().GetConfiance();
        }
        else
        {
            return this.HandPointer.GetComponent<OnPointsRightReceivedHandler>().GetConfiance();
        }
    }

    // Hand Menu 
    public void SetHand (string Hand) {
        this.exercise.hand = Hand;
        this.HandsTrackingHandler.GetComponent<HandsTrackingHandler>().SetHand(Hand);
        this.HandPointer = this.HandsTrackingHandler.GetComponent<HandsTrackingHandler>().GetActiveHand();
        this.HandSelectorMenu.SetActive (false);
        this.PointDistanceMenu.SetActive (true);
        keyPoint = 1;
        pointDistance = 0.05f;
        this.LessKeyPoint.SetActive(false);
        this.KeyPointText.GetComponent<TextMesh>().text = "1";
        this.DistanceText.GetComponent<TextMesh>().text = "5cm";
    }

    

    public void IncrementKeyPoint()
    {
        keyPoint++;
        if(keyPoint > 1)
        {
            this.LessKeyPoint.SetActive(true);
        }
        this.KeyPointText.GetComponent<TextMesh>().text = keyPoint.ToString();
    }

    public void DecrementKeyPoint()
    {
        keyPoint--;
        if(keyPoint == 1)
        {
            this.LessKeyPoint.SetActive(false);
        }
        this.KeyPointText.GetComponent<TextMesh>().text = keyPoint.ToString();

    }

    public void IncrementDistance()
    {
        pointDistance += 0.01f;
        if (keyPoint > 1)
        {
            this.LessDistance.SetActive(true);
        }
        this.DistanceText.GetComponent<TextMesh>().text = ((int)(pointDistance * 100)).ToString() + "cm";
    }

    public void DecrementDistance()
    {
        pointDistance -= 0.01f;
        if (pointDistance == 0.01f)
        {
            this.LessDistance.SetActive(false);
        }
        this.DistanceText.GetComponent<TextMesh>().text = ((int)(pointDistance * 100)).ToString() + "cm";

    }

    public void DistanceConfigFinish()
    {
        this.PointDistanceMenu.SetActive(false);
        this.DefineExerciseMenu.SetActive(false);
    }
    // Point distance menu 




    // Define exercise

    public void StartRecording () {
        ExercisePoints = new List<Vector3> ();;
        VisualPoints = new List<GameObject> ();
        VisualPointsPosition = new List<Vector3> ();
        recording = true;
        ///////
        this.testingData = new List<Vector3>();
    }
    public void StopRecording () {
        recording = false;
        this.DefineExerciseMenu.SetActive (true);
        this.BackSelectorMenu.SetActive(true);
        for(int i = 0; i < VisualPoints.Count; i++)
        {
            this.VisualPoints[i].SetActive(false);
        }
    }

    // Save menu

    public void SetBack(bool back)
    {
        this.back = back;
        this.BackSelectorMenu.SetActive(false);
        this.textName.SetActive(false);
        this.textPlaceholder.SetActive(true);
        this.SaveExerciseMenu.SetActive(true);
    }

    public void CallKeyboard()
    {
        this.textPlaceholder.SetActive(false);
        this.textName.SetActive(true);
        this.InputKeyboardHandler.GetComponent<InputKeyboardHandler>().ActiveKeyboard(gameObject, this.textName);
    }
    

    public void Save () {
        exercise.points = this.ExercisePoints;
        exercise.back = this.back;
        exercise.keyPoint = keyPoint;
        string name = this.textName.GetComponent<TextMesh>().text;
        exercise.name = name != "" ? name : "Unnamed";
        string jsonString = JsonUtility.ToJson (this.exercise);
        //File.WriteAllText ("TEST.json", jsonString);
        this.SaveExerciseMenu.SetActive (false);
        this.DefineExerciseMenu.SetActive(false);
        StartCoroutine (PostSaveExercise ());
        /// Testing
        StartCoroutine(PostTestingData());
        ViewExerciseSaved ();
    }

    public void ViewExerciseSaved () {
        int total = VisualPoints.Count - 1;
        for (int i = total; i >= 0; i--) {
            GameObject aux = VisualPoints[i];
            VisualPoints.RemoveAt (i);
            Destroy (aux);
        }

        VisualPoints = new List<GameObject> ();

        int bt = exercise.keyPoint + 1;

        for (int i = 0; i < this.exercise.points.Count - 1; i++) {

            // Torus
            if (i != 0 && (i % bt == (exercise.keyPoint))) {
                var before = this.exercise.points[i + 1];
                var vector = before - this.exercise.points[i-1];

                var rotation = Quaternion.LookRotation (vector.normalized);
                Quaternion newRotation = rotation * Quaternion.Euler (-180, 90, 90);
                Vector3 torusPosition = new Vector3 (this.exercise.points[i].x, this.exercise.points[i].y, this.exercise.points[i].z) + Camera.main.transform.position;
                /*
                if(vector.normalized.x != 0.0 || vector.normalized.z != 0.0){
                  torusPosition = torusPosition + new Vector3(0,-0.05f,0);
                }
                if(vector.normalized.y != 0.0){
                    if(vector.normalized.y > 0.0){
                        torusPosition = torusPosition + new Vector3(0,0,+0.05f);    
                    }
                    else {
                        torusPosition = torusPosition + new Vector3(0,0,-0.05f);
                    }
                }
                */

                GameObject torusPoint = Instantiate (this.TorusObject, torusPosition, Quaternion.identity);
                torusPoint.tag = "Torus";
                torusPoint.transform.rotation = newRotation;
                Debug.Log (i + " -- " + vector.normalized);
                torusPoint.transform.parent = ViewExercise.transform;
                torusPoint.SetActive (true);
                this.VisualPoints.Add (torusPoint);

            }
            // Point
            else {
                Vector3 pointPosition = this.exercise.points[i] + Camera.main.transform.position;
                GameObject spherePoint = Instantiate (this.PointObject, pointPosition, Quaternion.identity);
                spherePoint.transform.parent = ViewExercise.transform;
                spherePoint.tag = "Sphere";
                spherePoint.SetActive (true);
                this.VisualPoints.Add (spherePoint);
            }
        }

    }

    // Requests: 
    IEnumerator PostSaveExercise () {
        UnityWebRequest webRequest = new UnityWebRequest ("http://phyreup.francecentral.cloudapp.azure.com:3000/exercise", "POST");
        string jsonString = JsonUtility.ToJson (this.exercise);
        byte[] encodedPayload = new System.Text.UTF8Encoding ().GetBytes (jsonString);
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

    IEnumerator PostTestingData()
    {
        UnityWebRequest webRequest = new UnityWebRequest("http://phyreup.francecentral.cloudapp.azure.com:3000/recording", "POST");
        TestingDataSend testingDataSend = new TestingDataSend(this.testingData);
        string jsonString = JsonUtility.ToJson(testingDataSend);
        byte[] encodedPayload = new System.Text.UTF8Encoding().GetBytes(jsonString);
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(encodedPayload);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("cache-control", "no-cache");

        UnityWebRequestAsyncOperation requestHandel = webRequest.SendWebRequest();
        requestHandel.completed += delegate (AsyncOperation pOperation) {
            Debug.Log(webRequest.responseCode);
            Debug.Log(webRequest.downloadHandler.text);
        };

        yield return null;
    }

}

[Serializable]
public class TestingDataSend
{
    public Vector3[] items;

    public TestingDataSend(List<Vector3> items)
    {
        this.items = items.ToArray();
    }
}