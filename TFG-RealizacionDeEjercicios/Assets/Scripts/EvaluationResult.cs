using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Evaluation
{
    AlwaysProblems,
    NowProblems,
    NowNotProblems,
    NowLessFailures,
    NowMoreFailures,
    SameFailures,
    NowLessTime,
    NowMoreTime,
    SameTime,
    InTime,
    NotInTime
}

[Serializable]
public class EvaluationResult
{
    public Evaluation[] evaluation;

    public EvaluationResult(Evaluation[] evaluations)
    {
        this.evaluation = evaluation;
    }

    public EvaluationResult()
    {
        this.evaluation = null;
    }
}
