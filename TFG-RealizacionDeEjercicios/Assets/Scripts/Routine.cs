﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Routine
{
    public int id;
    public int repetitions;
    public int duration;
    public bool active;
    public int accuracy;
    public Exercise exercise;

    public Routine(int id, int repetitions,int duration, bool active, int accuracy, Exercise exercise){
        this.id = id;
        this.repetitions = repetitions;
        this.duration = duration;
        this.accuracy = accuracy;
        this.active = active;
        this.exercise = exercise;
    }

    public Routine(){
        repetitions = 0;
        active = true;
        accuracy = 1;
        exercise = null;
        duration = 0;
    }
}
