using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExerciseSummary {

    public int totalErros;
    public int[] pointsErros;
    public float totalTime;
    public float[] pointsTime;

    public ExerciseSummary(int totalErros, int[] pointsErros, float totalTime, float[] pointsTime){
        this.totalErros = totalErros;
    //    this.pointsErros = new List<int>(pointsErros);
        this.pointsErros = pointsErros;
        this.totalTime = totalTime;
        this.pointsTime = pointsTime;
    //    this.pointsTime = new List<float>(pointsTime);
    }

}