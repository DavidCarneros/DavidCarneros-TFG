using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRoutineComponentHandler : MonoBehaviour
{

    public Routine routine;
    public GameObject nameLabel;
    public GameObject repetitionsLabel;
    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject AsingRoutineObject;
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
        if (this.routine.exercise.hand == "Right")
        {
            this.rightHand.SetActive(true);
            this.leftHand.SetActive(false);
        }
        else
        {
            this.rightHand.SetActive(false);
            this.leftHand.SetActive(true);
        }

        this.nameLabel.GetComponent<TextMesh>().text = this.routine.exercise.name;
        this.repetitionsLabel.GetComponent<TextMesh>().text = this.routine.repetitions.ToString();
    }

    public void SelectRoutine()
    {
       this.AsingRoutineObject.GetComponent<AsignRoutineHandler>().SetRoutineAndGoNextStep(this.routine);
    }
}
