using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;

public class DefineExerciseHandler : MonoBehaviour
{
    // Views
    public GameObject HandSelectorMenu;
    public GameObject PointDistanceMenu;
    public GameObject SaveMenu;

    public GameObject ViewExercise;

    // Hand Pointer
    public GameObject HandPointer;
    public GameObject Point;

    Exercise exercise; 

    // auxiliars
    float pointDistance;
    int keyPoint;
    Vector3 HandPosition;
    Vector3 HandPositionSave;
    List<Vector3> ExercisePoints;
    List<GameObject> VisualPoints;
    List<Vector3> VisualPointsPosition;

    bool recording;

    void Start()
    {
        exercise = new Exercise();
        recording = false;
        this.HandSelectorMenu.SetActive(true);
        this.PointDistanceMenu.SetActive(false);
        this.SaveMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(recording){
            HandPosition = HandPointer.transform.position;
            HandPositionSave = HandPosition - Camera.main.transform.position;
            
            if(ExercisePoints.Count == 0){
                GameObject obj = Instantiate(Point, this.transform);
                obj.transform.position = HandPosition;
                obj.transform.parent = ViewExercise.transform;
                obj.SetActive(true);
                VisualPoints.Add(obj);
                VisualPointsPosition.Add(HandPosition);
                ExercisePoints.Add(HandPositionSave);
            }
            if(Vector3.Distance (HandPosition, VisualPointsPosition[VisualPointsPosition.Count-1]) >= pointDistance){
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

    // Hand Menu 
    public void SetHand(string Hand){
        this.exercise.hand = Hand;
        this.HandSelectorMenu.SetActive(false);
        this.PointDistanceMenu.SetActive(true);
    }

    // Point distance menu 

    public void OnDistanceSliderUpdated(SliderEventData eventData){
        this.pointDistance = eventData.NewValue / 10;
    }

    public void OnKeyPointSliderUpdated(SliderEventData eventData){
        this.keyPoint = (int) (eventData.NewValue * 10 + 1);
    }

    public void DistanceConfigFinish(){
        this.PointDistanceMenu.SetActive(false);
    }
    
    // Define exercise

    public void StartRecording(){
        ExercisePoints = new List<Vector3>();;
        VisualPoints = new List<GameObject>();
        VisualPointsPosition = new List<Vector3>();
        recording = true;
        Debug.Log("STARTING");
    }
    public void StopRecording(){
        recording = false;
        this.SaveMenu.SetActive(true);
    }

    // Save menu

    public void Save(){
        exercise.points = this.ExercisePoints;
        exercise.back = false;
        exercise.name = "prueba";
        string jsonString = JsonUtility.ToJson(this.exercise);
        File.WriteAllText("TEST.json", jsonString);
        this.SaveMenu.SetActive(false);
        StartCoroutine(PostSaveExercise());
    }



    // Requests: 
    IEnumerator PostSaveExercise(){
        UnityWebRequest webRequest = new UnityWebRequest("http://192.168.1.163:3000/exercise", "POST");
        string jsonString = JsonUtility.ToJson(this.exercise);
        byte[] encodedPayload = new System.Text.UTF8Encoding().GetBytes(jsonString);
        webRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(encodedPayload);
        webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("cache-control", "no-cache");
        
        UnityWebRequestAsyncOperation requestHandel = webRequest.SendWebRequest();
        requestHandel.completed += delegate(AsyncOperation pOperation) {
            Debug.Log(webRequest.responseCode);
            Debug.Log(webRequest.downloadHandler.text);
        };

        yield return null;
    }

}
