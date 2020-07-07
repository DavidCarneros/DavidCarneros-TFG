using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EvaluatePatient 
{
    No,
    One,
    Problems,
    NotProblems,
    InTime,
    NotInTime,
    Progress,
    NotProgress,
    Same
}

[Serializable]
public class PatientEvaluation
{
    public Routine routine;
    public EvaluatePatient[] evaluations;

    public PatientEvaluation(Routine routine, EvaluatePatient[] evaluations)
    {
        this.routine = routine;
        this.evaluations = evaluations;
    }

    public PatientEvaluation()
    {
        this.routine = null;
        this.evaluations = null;
    }
}