using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExerciseSummary {

    public int totalErros;
    public List<int> pointsErros;
    public float totalTime;
    public List<float> pointsTime;

    public ExerciseSummary(int totalErros, List<int> pointsErros, float totalTime, List<float> pointsTime){
        this.totalErros = totalErros;
        this.pointsErros = new List<int>(pointsErros);
        this.totalTime = totalTime;
        this.pointsTime = new List<float>(pointsTime);
    }

}