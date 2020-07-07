using System;
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
    public Exercise exercise;

    public Routine(int id, int repetitions, int duration, bool active, int accuracy, Exercise exercise){
        this.id = id;
        this.repetitions = repetitions;
        this.active = active;
        this.exercise = exercise;
        this.duration = duration;
    }

    public Routine(){
        repetitions = 0;
        active = true;
        exercise = null;
        duration = 0;
    }
}
