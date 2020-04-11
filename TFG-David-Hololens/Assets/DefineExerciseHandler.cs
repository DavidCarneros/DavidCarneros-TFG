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

    Exercise exercise;
    float exactExer;

    public TouchScreenKeyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        ExercisePoint = new List<Vector3>();
        VisualPoints = new List<GameObject>();
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
                obj.transform.position = handPositionSave;
                VisualPoints.Add(obj);
                ExercisePoint.Add(handPositionSave);
            }

            if(Vector3.Distance (handPosition, ExercisePoint[ExercisePoint.Count-1]) >= pointDistance){
                GameObject obj = Instantiate(point, this.transform);
                obj.transform.position = handPositionSave;
                VisualPoints.Add(obj);
                ExercisePoint.Add(handPositionSave);
            }
        }
    }

    public void drawLine(){
        exercise = new Exercise();
        exercise.points = ExercisePoint;
        exercise.name = "prueba";
        exercise.hand = hand;

    //    Vector3[] arrayPoints = new Vector3[ExercisePoint.Count];
    //    for(int i = 0 ; i < ExercisePoint.Count; i++){
    //        arrayPoints[i] = Camera.main.transform.position + ExercisePoint[i]; 
    //    }

        DefineExercise.GetComponent<TubeRenderer>().SetPositions(ExercisePoint.ToArray());
    }

    public void setRecording(bool rec){
        this.recording = rec;
    }

    public void setHand(string hand){
        this.hand = hand;
    }

    public void saveExercise() {
        exercise.exact = this.exactExer;
        string jsonString = JsonUtility.ToJson(this.exercise);
        File.WriteAllText("Exercises/test1.json", jsonString);
    }

    public void OpenSystemKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    }

    public void OnSliderDistanceUpdated(SliderEventData eventData){
        this.pointDistance = eventData.NewValue/10;
    }

    public void OnSliderExactUpdated(SliderEventData eventData){
        Debug.Log(this.exactExer);
        this.exactExer = eventData.NewValue/10;
    }
}
