using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DefineExerciseHandler : MonoBehaviour {
    // Views
    public GameObject HandSelectorMenu;
    public GameObject PointDistanceMenu;
    public GameObject SaveMenu;

    public GameObject ViewExercise;

    // Hand Pointer
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

    bool recording;

    void Start () {
        InitComponent();
    }

    public void InitComponent(){
        exercise = new Exercise ();
        ExercisePoints = new List<Vector3>();
        VisualPointsPosition = new List<Vector3>();
        /*
        int total = VisualPoints.Count-1;
        if(total >= 0 ) {
            for (int i = total; i>= 0 ; i++){
                GameObject aux = VisualPoints[i];
                VisualPoints.RemoveAt (i);
                Destroy (aux);
            }
        }
        */
        recording = false;
        back = false;
        this.TorusObject.SetActive (false);
        this.PointObject.SetActive (false);
        this.HandSelectorMenu.SetActive (true);
        this.PointDistanceMenu.SetActive (false);
        this.SaveMenu.SetActive (false);
    }

    public void CleanObjects(){
        int total = VisualPoints.Count-1;
        if(total >= 0 ) {
            for (int i = total; i>= 0 ; i--){
                GameObject aux = VisualPoints[i];
                VisualPoints.RemoveAt (i);
                Destroy (aux);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (recording) {
            HandPosition = HandPointer.transform.position;
            HandPositionSave = HandPosition - Camera.main.transform.position;

            if (ExercisePoints.Count == 0) {
                GameObject obj = Instantiate (Point, this.transform);
                obj.transform.position = HandPosition;
                obj.transform.parent = ViewExercise.transform;
                obj.SetActive (true);
                VisualPoints.Add (obj);
                VisualPointsPosition.Add (HandPosition);
                ExercisePoints.Add (HandPositionSave);
            }
            if (Vector3.Distance (HandPosition, VisualPointsPosition[VisualPointsPosition.Count - 1]) >= pointDistance) {
                GameObject obj = Instantiate (Point, this.transform);
                obj.transform.position = HandPosition;
                obj.transform.parent = ViewExercise.transform;
                obj.SetActive (true);
                VisualPoints.Add (obj);
                VisualPointsPosition.Add (HandPosition);
                ExercisePoints.Add (HandPositionSave);
            }
        }
    }

    // Hand Menu 
    public void SetHand (string Hand) {
        this.exercise.hand = Hand;
        this.HandSelectorMenu.SetActive (false);
        this.PointDistanceMenu.SetActive (true);
    }

    // Point distance menu 

    public void OnDistanceSliderUpdated (SliderEventData eventData) {
        this.pointDistance = eventData.NewValue / 10;
    }

    public void OnKeyPointSliderUpdated (SliderEventData eventData) {
        this.keyPoint = (int) (eventData.NewValue * 10 + 1);
    }

    public void DistanceConfigFinish () {
        this.PointDistanceMenu.SetActive (false);
    }

    // Define exercise

    public void StartRecording () {
        ExercisePoints = new List<Vector3> ();;
        VisualPoints = new List<GameObject> ();
        VisualPointsPosition = new List<Vector3> ();
        recording = true;
        Debug.Log ("STARTING");
    }
    public void StopRecording () {
        recording = false;
        this.SaveMenu.SetActive (true);
    }

    // Save menu

    public void ToggleBack(){
        this.back = !this.back;
    }

    public void Save () {
        exercise.points = this.ExercisePoints;
        exercise.back = this.back;
        exercise.keyPoint = keyPoint;
        exercise.name = "prueba";
        string jsonString = JsonUtility.ToJson (this.exercise);
        File.WriteAllText ("TEST.json", jsonString);
        this.SaveMenu.SetActive (false);
        StartCoroutine (PostSaveExercise ());
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
        UnityWebRequest webRequest = new UnityWebRequest ("http://192.168.1.163:3000/exercise", "POST");
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

}