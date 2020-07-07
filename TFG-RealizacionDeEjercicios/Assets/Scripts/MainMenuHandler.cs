using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject ExerciseHandler;
    public GameObject ViewExerciseHandler;
    public GameObject ViewExercisesListView;
    public GameObject DoExerciseListView;
    public GameObject ViewExercisesContainer;

    public GameObject KeyPatient;
    public GameObject ButtonCollection;

    public GameObject textKey;
    public GameObject placeholderKey;
    public GameObject EnterButton;

    public GameObject InputKeyboardHandler;

    public string patientKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 0.7f);
    }

    public void CallKeyboard()
    {
        this.placeholderKey.SetActive(false);
        this.textKey.SetActive(true);
        this.InputKeyboardHandler.GetComponent<InputKeyboardHandler>().ActiveKeyboard(gameObject, this.textKey);
        this.EnterButton.SetActive(true);
    }

    public void DoAnExercise(){
        gameObject.SetActive(false);
        this.ExerciseHandler.SetActive(true);
        //    this.ExerciseHandler.GetComponent<RoutineHandler>().DEBUG();
        this.ExerciseHandler.GetComponent<RoutineHandler>().SetPatientKey(this.patientKey);
        this.ExerciseHandler.GetComponent<RoutineHandler>().GetAllRoutines();  
    }

    public void ViewExercises()
    {
        gameObject.SetActive(false);
        this.ViewExerciseHandler.SetActive(true);
        this.ViewExerciseHandler.GetComponent<ViewRoutineHandler>().SetPatientKey(this.patientKey);
        //    this.ExerciseHandler.GetComponent<RoutineHandler>().DEBUG();
        this.ViewExerciseHandler.GetComponent<ViewRoutineHandler>().GetAllRoutines();
    }

    public void SetKey()
    {
        this.patientKey = this.textKey.GetComponent<TextMesh>().text;
        this.KeyPatient.SetActive(false);
        this.ButtonCollection.SetActive(true);
    }

    public void Restart()
    {
        if (this.ExerciseHandler.activeSelf)
        {
            this.ExerciseHandler.GetComponent<RoutineHandler>().Clean();
            this.ExerciseHandler.SetActive(false);
        }
        if (this.ViewExerciseHandler.activeSelf)
        {
            this.ViewExerciseHandler.GetComponent<ViewRoutineHandler>().Clean();
            this.ViewExerciseHandler.SetActive(false);
        }
        if (this.ViewExercisesListView.activeSelf)
        {
            this.ViewExercisesListView.SetActive(false);
        }
        if (this.DoExerciseListView.activeSelf)
        {
            this.DoExerciseListView.SetActive(false);
        }

        gameObject.SetActive(true);
    }

    public void BackToViewExercises()
    {
        if (this.ViewExerciseHandler.activeSelf)
        {
            this.ViewExerciseHandler.GetComponent<ViewExerciseHandler>().Clean();
            this.ViewExerciseHandler.SetActive(false);
            this.ViewExercisesListView.SetActive(true);
            this.ViewExercisesContainer.SetActive(true);
        }
    }

}
