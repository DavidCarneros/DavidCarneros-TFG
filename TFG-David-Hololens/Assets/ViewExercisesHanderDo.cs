using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewExercisesHanderDo : MonoBehaviour
{
    public Exercise exercise;
    GameObject view;
    public GameObject main;
    // Start is called before the first frame update
    void Start()
    {
        view = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void renderExercise(){
        main.GetComponent<DoExercisesHandler>().viewExercise(this.exercise); 
      //  main.GetComponent<RadialView>().enabled = false;
    }

    public void loadExercise(Exercise exer){
        this.exercise = exer;
        foreach (Transform eachChild in transform) {
            UnityEngine.Debug.Log("[ExerciseHandler]: " + eachChild.name);
            this.caseChild(eachChild);
        }

    }


    void caseChild (Transform child) {

        switch (child.name) {
            case "Handview":
                foreach (Transform eachChild in child.transform) {
                    switch(eachChild.name) {
                        case "IconHandIdleRight":
                            UnityEngine.Debug.Log("[ExerciseHandler]: " +( this.exercise.hand == "Right"));
                            eachChild.gameObject.SetActive(this.exercise.hand == "Right");
                            break;

                        case "IconHandIdleLeft":
                            UnityEngine.Debug.Log("[ExerciseHandler]: " +( this.exercise.hand == "Left"));
                            eachChild.gameObject.SetActive(this.exercise.hand == "Left");
                            break;
                    }
                }
               
                break;

            case "ExerciseButton_View":
                child.gameObject.SetActive(false);
                break;
            
            case "ExerciseButton_Do":
                child.gameObject.SetActive(true);
                break;

            case "Temporalidad":
                child.gameObject.GetComponent<TMP_Text>().text += this.exercise.temporaly;
                break;

            case "name":
                child.gameObject.GetComponent<TMP_Text>().text += this.exercise.name;
                break;

            case "hand":
                child.gameObject.GetComponent<TMP_Text>().text += this.exercise.hand;
                break;

            default:
                break;
        }

    }
}
