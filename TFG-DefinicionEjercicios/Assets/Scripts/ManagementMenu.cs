using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainMenuObject;
    public GameObject CreateRoutineObject;
    public GameObject AsignRoutineObject;

    void Start()
    {
        this.CreateRoutineObject.SetActive(false);
        this.AsignRoutineObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoutine()
    {
        gameObject.SetActive(false);
        this.CreateRoutineObject.SetActive(true);
        this.CreateRoutineObject.GetComponent<CreateRoutineHandler>().GetAllExercises();
    }

    public void AsingRoutine()
    {
        gameObject.SetActive(false);
        this.AsignRoutineObject.SetActive(true);
        this.AsignRoutineObject.GetComponent<AsignRoutineHandler>().GetAllRoutines();
    }

    public void GoToMainMenu()
    {
        this.CreateRoutineObject.SetActive(false);
        this.MainMenuObject.GetComponent<MainMenuHandler>().Restart();
        gameObject.SetActive(false);
    }
}
