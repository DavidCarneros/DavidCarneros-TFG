using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineExerciseHandler : MonoBehaviour
{

    public GameObject HandPointer;
    public GameObject tubeRenderer;
    public GameObject point;

    public bool recording;
    public float pointDistance;

    Vector3 headPosition;
    Vector3 handPosition;
    Vector3 oldHandPosition;

    List<Vector3> ExercisePoint;
    List<GameObject> VisualPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(recording) {
            handPosition = HandPointer.transform.position;
            if(ExercisePoint.Count == 0){
                GameObject obj = Instantiate(point, this.transform);
                obj.transform.position = position;
                VisualPoints.Add(obj);
                ExercisePoint.Add(position);
            }

            if(Vector3.Distance (position, ExercisePoint[ExercisePoint.Count-1]) >= pointDistance){
                GameObject obj = Instantiate(point, this.transform);
                obj.transform.position = position;
                VisualPoints.Add(obj);
                ExercisePoint.Add(position);
            }
            

        }
    }
}
