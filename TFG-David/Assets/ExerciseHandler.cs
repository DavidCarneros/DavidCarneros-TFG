using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExerciseHandler : MonoBehaviour {

    public Exercise exercise;
    GameObject view;
    // Start is called before the first frame update
    void Start () {
        view = transform.parent.gameObject;

        //    foreach (Transform eachChild in transform) {
        //        
        //        if (eachChild.name == "NameWhatYouNeed") {
        //            Debug.Log ("Child found. Mame: " + eachChild.name);
        //        }
        //    }

    }

    public void loadExercise (Exercise exer) {
        this.exercise = exer;
        foreach (Transform eachChild in transform) {
            UnityEngine.Debug.Log("[ExerciseHandler]: " + eachChild.name);
            this.caseChild(eachChild);
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public void doExercise(){
        GameObject.Find("Visualizador").GetComponent<Visualizacion>().setExercise(this.exercise);
        GameObject.Find("Visualizador").GetComponent<Visualizacion>().dibujarTrayectoria();
    }

    void caseChild (Transform child) {

        switch (child.name) {
            case "RigthHand":
                if (this.exercise.hand == "Right") {
                    child.gameObject.SetActive (true);
                } else {
                    child.gameObject.SetActive (false);

                }
                break;
            case "LeftHand":
                if (this.exercise.hand == "Left") {
                    child.gameObject.SetActive (true);
                } else {
                    child.gameObject.SetActive (false);

                }
                break;

            case "DoButton":

                break;

            case "Temporalidad":
                child.gameObject.GetComponent<TMP_Text>().text += this.exercise.temporaly;
                break;

            case "NombreEj":
                child.gameObject.GetComponent<TMP_Text>().text += this.exercise.name;
                break;

            case "Exactitud":
                child.gameObject.GetComponent<TMP_Text>().text += this.exercise.exact;
                break;

            default:
                break;
        }

    }
}