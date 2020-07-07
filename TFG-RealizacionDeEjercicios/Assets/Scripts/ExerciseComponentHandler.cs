using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseComponentHandler : MonoBehaviour
{

    public Routine routine;
    public GameObject nameLabel;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject repetitionsLabel;

    public GameObject ExerciseHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoutine(Routine routine)
    {
        this.routine = routine;
        if(routine.exercise.hand == "Right")
        {
            this.rightHand.SetActive(true);
            this.leftHand.SetActive(false);
        }
        else
        {
            this.rightHand.SetActive(false);
            this.leftHand.SetActive(true);
        }

        this.nameLabel.GetComponent<TextMesh>().text = routine.exercise.name;
        this.repetitionsLabel.GetComponent<TextMesh>().text = routine.repetitions.ToString();
    }

    public void DoExercise()
    {
        this.ExerciseHandler.GetComponent<RoutineHandler>().StartRoutine(this.routine);
    }
}
