using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Exercise {


    private List<Vector3> points;
    private List<float> time;
    private string name;
    private float temporaly;
    private string hand;
    private float exact; 

    public Exercise(List<Vector3> points, List<float> time, string name, float temporaly, string hand, float exact){
        this.points = new List<Vector3>(points);
      //  this.points = points;
        this.time = new List<float>(time); 
      //  this.time = time;
        this.name = name;
        this.temporaly = temporaly;
        this.hand = hand;
        this.exact = exact;
    }

}