using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Exercise
{
    
    public int id;
    public int keyPoint;
    public string name;
    public string videoUrl;
    public List<Vector3> points;
    public string hand;
    public bool back;

    public Exercise(int id, int keyPoint, string name, string videoUrl, List<Vector3> points, string hand, bool back){
        this.id = id;
        this.name = name;
        this.keyPoint = keyPoint;
        this.videoUrl = videoUrl;
        this.points = new List<Vector3>(points);
        this.hand = hand;
        this.back = back;
    }

    public Exercise(){
        this.points = new List<Vector3>();
        this.keyPoint = 2;
        this.name = "";
        this.videoUrl = "";
        this.hand = "";
        this.back = false;
    }

  
}
