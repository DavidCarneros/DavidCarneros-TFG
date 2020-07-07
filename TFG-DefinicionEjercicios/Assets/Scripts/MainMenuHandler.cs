using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DefineExerciseObject;
    public GameObject ViewExercisesObject;
    public GameObject ViewExercisesAndPatientObject;
    public GameObject ExercisesObjectList;
    public GameObject RoutineManagement;
    public GameObject ViewPatientProgressObject;

    void Start()
    {
        this.DefineExerciseObject.SetActive(false);
        this.ViewExercisesObject.SetActive(false);
        this.RoutineManagement.SetActive(false);
        this.ViewExercisesAndPatientObject.SetActive(false);
        this.ViewPatientProgressObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void DefineExercise(){
        gameObject.SetActive(false);
        this.DefineExerciseObject.GetComponent<DefineExerciseHandler>().InitComponent();
        this.DefineExerciseObject.SetActive(true);
    }

    public void ViewExercises()
    {
        gameObject.SetActive(false);
        this.ViewExercisesObject.SetActive(true);
        this.ViewExercisesObject.GetComponent<ViewExercisesHandler>().GetAllExercises();
    }
    
    public void ViewExercisesAndPatient()
    {
        gameObject.SetActive(false);
        this.ViewExercisesAndPatientObject.SetActive(true);
        this.ViewExercisesObject.SetActive(false);
        this.ViewPatientProgressObject.SetActive(false);
        this.ViewExercisesObject.GetComponent<ViewExercisesHandler>().Clean();

    }

    public void Management()
    {
        gameObject.SetActive(false);
        this.RoutineManagement.SetActive(true);
    }

    public void Restart(){
        if(this.DefineExerciseObject.activeSelf){
            this.DefineExerciseObject.SetActive(false);
            this.DefineExerciseObject.GetComponent<DefineExerciseHandler>().Clean();
        }
        if (this.ViewExercisesObject.activeSelf)
        {
            this.ViewExercisesObject.GetComponent<ViewExercisesHandler>().Clean();
            this.ViewExercisesObject.SetActive(false);
        }
        if (this.ExercisesObjectList.activeSelf)
        {
            this.ViewExercisesObject.SetActive(true);
            this.ViewExercisesObject.GetComponent<ViewExercisesHandler>().Clean();
            this.ViewExercisesObject.SetActive(false);
            this.ExercisesObjectList.SetActive(false);
        }
        if (this.ViewPatientProgressObject.activeSelf)
        {
            this.ViewPatientProgressObject.SetActive(false);
        }
        if (this.ViewExercisesAndPatientObject.activeSelf)
        {
            this.ViewExercisesAndPatientObject.SetActive(false);
        }
        if (this.RoutineManagement.activeSelf)
        {
            this.RoutineManagement.SetActive(false);
        }
        
        gameObject.SetActive(true);
    }
}
