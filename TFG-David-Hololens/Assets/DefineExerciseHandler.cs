using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using System.IO;

public class DefineExerciseHandler : MonoBehaviour
{

    public GameObject HandPointer;
    public GameObject handsTrackingHandler;
    public GameObject DefineExercise;
    public GameObject point;

    public bool recording;
    public float pointDistance;
    public string hand;

    Vector3 headPosition;
    Vector3 handPosition;
    Vector3 handPositionSave;
    Vector3 oldHandPosition;

    List<Vector3> ExercisePoint;
    List<GameObject> VisualPoints;
    List<Vector3> VisualPointsPosition;

    Exercise exercise;
    float exactExer;

    public TouchScreenKeyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        ExercisePoint = new List<Vector3>();
        VisualPoints = new List<GameObject>();
        VisualPointsPosition = new List<Vector3>();
        DefineExercise = GameObject.Find("DefineExercise");
    }

    // Update is called once per frame
    void Update()
    {
        if(recording) {
            handPosition = HandPointer.transform.position;
         //   handPosition = handsTrackingHandler.GetComponent<HandsTrackingHandler>().handPoint;
            headPosition = Camera.main.transform.position;
            handPositionSave = handPosition - headPosition;
            if(ExercisePoint.Count == 0){
                GameObject obj = Instantiate(point, this.transform);
                obj.transform.position = handPosition;
                VisualPoints.Add(obj);
                VisualPointsPosition.Add(handPosition);
                ExercisePoint.Add(handPositionSave);
            }

            if(Vector3.Distance (handPosition, VisualPointsPosition[VisualPointsPosition.Count-1]) >= pointDistance){
                GameObject obj = Instantiate(point, this.transform);
                obj.transform.position = handPosition;
                VisualPoints.Add(obj);
                VisualPointsPosition.Add(handPosition);
                ExercisePoint.Add(handPositionSave);
            }
        }
    }

    public void drawLine(){
        exercise = new Exercise();
        exercise.points = ExercisePoint;
        exercise.name = "prueba";
        exercise.hand = hand;


        DefineExercise.GetComponent<TubeRenderer>().SetPositions(VisualPointsPosition.ToArray());
    }

    public void setRecording(bool rec){
        this.recording = rec;
    }

    public void setHand(string hand){
        this.hand = hand;
    }

    public void setBack(){
        this.exercise.back = !this.exercise.back;
    }

    public void saveExercise() {
        exercise.exact = this.exactExer;
        string jsonString = JsonUtility.ToJson(this.exercise);
        File.WriteAllText("Exercises/test12.json", jsonString);
    }

    public void OpenSystemKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    }

    public void OnSliderDistanceUpdated(SliderEventData eventData){
        this.pointDistance = eventData.NewValue/10;
    }

    public void OnSliderExactUpdated(SliderEventData eventData){
        this.exactExer = eventData.NewValue/10;
    }
}
