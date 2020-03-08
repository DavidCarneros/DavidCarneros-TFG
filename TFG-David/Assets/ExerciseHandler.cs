using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseHandler : MonoBehaviour {

    public Exercise exercise;
    GameObject view;
    // Start is called before the first frame update
    void Start () {
        view = transform.parent.gameObject;

        foreach (Transform eachChild in transform) {
            
            if (eachChild.name == "NameWhatYouNeed") {
                Debug.Log ("Child found. Mame: " + eachChild.name);
            }
        }

    }

    // Update is called once per frame
    void Update () {

    }

    void caseChild(Transform child){

        switch (child.name)
        {
            case "RigthHand" : 
             if(this.exercise.hand == "Right"){
                    child.gameObject.SetActive(true);
                }else{
                    child.gameObject.SetActive(false);

                }
                break;
            case "LeftHand" : 
                if(this.exercise.hand == "Left"){
                    child.gameObject.SetActive(true);
                }else{
                    child.gameObject.SetActive(false);

                } 
                break;
            
            case "DoButton" : 

                break;
            
            case "Temporalidad" : 
                    
                break;
            
            case "NombreEj" : 

                break;

            case "Exactitud" : 

                break;

            default:
            break;
        }


    }
}