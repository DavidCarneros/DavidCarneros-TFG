using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject ExerciseHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoAnExercise(){
        gameObject.SetActive(false);
        this.ExerciseHandler.SetActive(true);
        this.ExerciseHandler.GetComponent<RoutineHandler>().DEBUG();
    }
}
