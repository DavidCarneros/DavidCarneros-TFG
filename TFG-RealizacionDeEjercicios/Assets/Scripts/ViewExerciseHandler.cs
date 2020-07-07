using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewExerciseHandler : MonoBehaviour
{

    public Exercise exercise;
    public Routine routine;

    public GameObject TorusObject;
    public GameObject PointObject;
    public GameObject ViewExercise;

    List<Vector3> VisualPointsPosition;
    List<GameObject> VisualPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetExerciseAndDraw(Routine routine)
    {
        this.routine = routine;
        this.exercise = routine.exercise;
        DrawExercise();
    }

    public void Clean()
    {
        if(this.VisualPoints != null)
        {
            int total = this.VisualPoints.Count - 1;
            for (int i = total; i >= 0; i--)
            {
                GameObject obj = this.VisualPoints[i];
                this.VisualPoints.RemoveAt(i);
                Destroy(obj);
            }

            this.routine = null;
            this.exercise = null;
            this.VisualPointsPosition = null;
            this.VisualPoints = null;
        }
    }

    public void DrawExercise()
    {
        VisualPoints = new List<GameObject>();
        VisualPointsPosition = new List<Vector3>();

        int bt = exercise.keyPoint + 1;

        for (int i = 0; i < this.exercise.points.Count; i++)
        {

            // Torus
            if (i != 0 && (i % bt == (exercise.keyPoint)))
            {
                var before = this.exercise.points[i];
                if (i + 1 >= this.exercise.points.Count)
                {
                    before = this.exercise.points[i];
                }
                else
                {
                    before = this.exercise.points[i + 1];
                }
                //var before = this.exercise.points[i + 1];
                var vector = before - this.exercise.points[i - 1];

                var rotation = Quaternion.LookRotation(vector.normalized);
                Quaternion newRotation = rotation * Quaternion.Euler(-180, 90, 90);
                Vector3 torusPosition = new Vector3(this.exercise.points[i].x, this.exercise.points[i].y, this.exercise.points[i].z) + Camera.main.transform.position;
                Vector3 centralPoint = this.exercise.points[i] + Camera.main.transform.position;

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
                torusPoint.transform.parent = ViewExercise.transform;
                torusPoint.SetActive(true);
                this.VisualPoints.Add(torusPoint);
                this.VisualPointsPosition.Add(centralPoint);
            }
            // Point
            else
            {
                Vector3 pointPosition = this.exercise.points[i] + Camera.main.transform.position;
                GameObject spherePoint = Instantiate(this.PointObject, pointPosition, Quaternion.identity);
                spherePoint.transform.parent = ViewExercise.transform;
                spherePoint.tag = "Sphere";
            
                spherePoint.SetActive(true);
               
                this.VisualPoints.Add(spherePoint);
                this.VisualPointsPosition.Add(spherePoint.transform.position);
            }
        }

        VisualPoints[0].transform.localScale = new Vector3(0.013f, 0.013f, 0.013f);
        VisualPoints[0].GetComponent<MeshRenderer>().material.color = Color.yellow;

    }
}
