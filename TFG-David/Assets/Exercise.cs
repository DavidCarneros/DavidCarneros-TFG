using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Exercise {


    private List<Vector3> _points;
    private List<float> _time;
    private string _name;
    private float _temporaly;
    private string _hand;
    private float _exact; 

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