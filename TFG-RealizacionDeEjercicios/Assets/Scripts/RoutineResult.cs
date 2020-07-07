﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoutineResult
{
    public int id;
    public int routine;
    public string startDate;
    public string endDate;
    public bool complete;
    public bool problems;
    public ExerciseResult[] exerciseResult;

    public RoutineResult(int id, string startDate, string endDate, bool complete, bool problems, ExerciseResult[] exerciseResult){
        this.id = id;
        this.startDate = startDate;
        this.endDate = endDate;
        this.complete = complete;
        this.problems = problems;
        this.exerciseResult = exerciseResult;
    }

    public RoutineResult(){
        this.id = 0;
        this.startDate = null;
        this.endDate = null;
        this.complete = false;
        this.problems = false;
        this.exerciseResult = null;
    }
}