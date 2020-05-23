using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExerciseResult
{
    public int id;
    public int[] failures;
    public int[] failures_back;
    public int total_failures;
    public double[] time;
    public double[] time_back;
    public double total_time;
    public string startAt;
    public string endAt;

    public ExerciseResult(int id, int[] failures, int[] failures_back, int total_failures, double[] time, double[] time_back, double total_time, string startAt, string endAt){
        this.id = id;
        this.failures = failures;
        this.failures_back = failures_back;
        this.total_failures = total_failures;
        this.time = time;
        this.time_back = time_back;
        this.total_time = total_time;
        this.startAt = startAt;
        this.endAt = endAt;

    }

    public ExerciseResult(){
        this.id = 0;
        this.failures = null;
        this.failures_back = null;
        this.total_failures = 0;
        this.time = null;
        this.time_back = null;
        this.total_time = 0;

        this.startAt = null;
        this.endAt = null;
    }

}
