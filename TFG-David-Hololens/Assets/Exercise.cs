using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Exercise {


    public List<Vector3> _points;
    public List<float> _time;
    public string _name;
    public float _temporaly;
    public string _hand;
    public float _exact; 

    public Exercise(List<Vector3> points, List<float> time, string name, float temporaly, string hand, float exact){
        this._points = new List<Vector3>(points);
        this._time = new List<float>(time); 
        this._name = name;
        this._temporaly = temporaly;
        this._hand = hand;
        this._exact = exact;
    }

    public Exercise(){
      this._points = new List<Vector3>();
      this._time = new List<float>();
    }

    public List<Vector3> points
    {
    get { return _points; }
    set { _points = value; }
    }

    public List<float> time
    {
    get { return _time; }
    set { _time = value; }
    }

    public string name
    {
    get { return _name; }
    set { _name = value; }
    }

    public float temporaly
    {
    get { return _temporaly; }
    set { _temporaly = value; }
    }

    public string hand
    {
    get { return _hand; }
    set { _hand = value; }
    }

    public float exact
    {
    get { return _exact; }
    set { _exact = value; }
    }

}