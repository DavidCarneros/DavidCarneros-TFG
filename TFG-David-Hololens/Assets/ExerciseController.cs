using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseController : MonoBehaviour
{

    public Exercise exercise;
    public ExerciseSummary summary;
    public GameObject ExerciseRender;
    public GameObject ExerciseList;
    public GameObject MainView;

    List<Vector3> printedPoints;
    List<GameObject> objectPoints;
    List<ExerciseSummary> oldSummary;

    float exac;

    // Start is called before the first frame update
    void Start()
    {
        this.printedPoints = new List<Vector3>();
        this.objectPoints = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setExerciseAndStart(Exercise exercise){
        this.exercise = exercise;
        renderExercise();
    }

    public void renderExercise(){

        for(int i = 0; i < exercise.points.Count - 1; i++){
            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Vector3 position = Camera.main.transform.position + exercise.points[i];
            point.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            point.GetComponent<MeshRenderer>().material.color = Color.red;
            point.transform.position = Camera.main.transform.position + exercise.points[i];
            point.transform.parent = ExerciseRender.transform;
            printedPoints.Add(position);
            objectPoints.Add(point);

        }

        gameObject.GetComponent<TubeRenderer>().SetPositions(printedPoints.ToArray()); 

    }
}
 