using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectExerciseComponentHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Exercise exercise;
    public GameObject nameLabel;
    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject DefineRoutineObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetExercise(Exercise exercise)
    {
        this.exercise = exercise;
        if (exercise.hand == "Right")
        {
            this.rightHand.SetActive(true);
            this.leftHand.SetActive(false);
        }
        else
        {
            this.rightHand.SetActive(false);
            this.leftHand.SetActive(true);
        }

        this.nameLabel.GetComponent<TextMesh>().text = exercise.name;
    }

    public void SelectExercise()
    {
       this.DefineRoutineObject.GetComponent<CreateRoutineHandler>().SetExerciseAndGoNextStep(this.exercise);
    }
}
