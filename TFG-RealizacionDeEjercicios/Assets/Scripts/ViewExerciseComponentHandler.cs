using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewExerciseComponentHandler : MonoBehaviour
{
    public Routine routine;
    public GameObject nameLabel;
    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject ViewExerciseHandler;

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
    }

    public void ViewExercise()
    {
        this.ViewExerciseHandler.GetComponent<ViewRoutineHandler>().ViewRoutine(this.routine);
    }
}
