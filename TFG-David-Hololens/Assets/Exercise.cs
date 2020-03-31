using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Exercise {


    public List<Vector3> points;
    public List<float> time;
    public string name;
    public float temporaly;
    public string hand;
    public float exact; 

    public Exercise(List<Vector3> points, List<float> time, string name, float temporaly, string hand, float exact){
        this.points = new List<Vector3>(points);
        this.time = new List<float>(time); 
        this.name = name;
        this.temporaly = temporaly;
        this.hand = hand;
        this.exact = exact;
    }

    public Exercise(){
      this.points = new List<Vector3>();
      this.time = new List<float>();
    }

}