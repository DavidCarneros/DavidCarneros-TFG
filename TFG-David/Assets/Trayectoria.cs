using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trayectoria : MonoBehaviour
{
    GameObject palmPointer;
    GameObject point;
    GameObject tubeRenderer;

    public bool recording;
    public float pointDistance;
    public bool DdrawLine;


   
    List<GameObject> points;

    Vector3 oldPosition;
    Vector3 position;

    // 
    List<Vector3> p_points;
    List<float> l_time;
    string name;
    float temporaly;
    float exact;
    string hand;
    //

    // Start is called before the first frame update
    void Start()
    {
        palmPointer = GameObject.Find("HandPointer");
        point = GameObject.Find("Point");
        tubeRenderer = GameObject.Find("MixedRealityPlayspace");

        palmPointer.GetComponent<MeshRenderer>().material.color = Color.red;
        oldPosition = Vector3.zero;

        p_points = new List<Vector3>();
        points = new List<GameObject>();

        DdrawLine = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Aqui se deberia comprrobar si esta o no la mano siendo vista 
        // Cunando no se esta viendo mesh renderer esta a false 

       // if(p_points.Count == 10){
       //     this.recording = false;
       //     this.drawLine();
       // }

        if(recording) {
        //    point.GetComponent<Renderer>().enabled = true;
            position = palmPointer.transform.position;
            palmPointer.GetComponent<MeshRenderer>().material.color = Color.green;

            if (points.Count == 0 ){
                GameObject obj = Instantiate(point, this.transform);
                obj.transform.position = position;
                points.Add(obj);
                p_points.Add(position);
            }

            if(Vector3.Distance (position, p_points[p_points.Count-1]) >= pointDistance){
                GameObject obj = Instantiate(point, this.transform);
                obj.transform.position = position;
                points.Add(obj);
                p_points.Add(position);
            }
        } else {
            palmPointer.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        oldPosition = position;
    }

    public void SetRecording(bool rec) {
        this.recording = rec;
    }

    public void setDrawLine(){
            this.drawLine();
    }

    void drawLine() {
        tubeRenderer.GetComponent<TubeRenderer>().SetPositions(p_points.ToArray());
    }

}
