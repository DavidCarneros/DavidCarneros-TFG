using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineExerciseHandler : MonoBehaviour
{

    public GameObject HandPointer;
    public GameObject DefineExercise;
    public GameObject point;

    public bool recording;
    public float pointDistance;
    public string hand;

    Vector3 headPosition;
    Vector3 handPosition;
    Vector3 oldHandPosition;

    List<Vector3> ExercisePoint;
    List<GameObject> VisualPoints;

    Exercise exercise;

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
            if(ExercisePoint.Count == 0){
                GameObject obj = Instantiate(point, this.transform);
                obj.transform.position = handPosition;
                VisualPoints.Add(obj);
                ExercisePoint.Add(handPosition);
            }

            if(Vector3.Distance (handPosition, ExercisePoint[ExercisePoint.Count-1]) >= pointDistance){
                GameObject obj = Instantiate(point, this.transform);
                obj.transform.position = handPosition;
                VisualPoints.Add(obj);
                ExercisePoint.Add(handPosition);
            }
        }
    }

    public void drawLine(){
        exercise = new Exercise();
        exercise.points = ExercisePoint;
        exercise.name = "prueba";
        exercise.exact = 0.5f;
        exercise.hand = hand;

        DefineExercise.GetComponent<TubeRenderer>().SetPositions(ExercisePoint.ToArray());
    }

    public void setRecording(bool rec){
        this.recording = rec;
    }

    public void setHand(string hand){
        this.hand = hand;
    }

    public void saveExercise() {
      //  string jsonString = JsonUtility.ToJson(this.exer_aux);
      //  File.WriteAllText("test.json", jsonString);
    }
}
