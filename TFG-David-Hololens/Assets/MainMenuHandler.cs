using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour {

    public GameObject MainMenu;

    public GameObject DefineExercise;
    public GameObject ViewExercises;
    public GameObject DoExercise;

    public GameObject HandsTracking;

    // Start is called before the first frame update
    void Start () {
        this.DefineExercise.SetActive (false);
        this.ViewExercises.SetActive (false);
        this.DoExercise.SetActive (false);
    }

    // Update is called once per frame
    void Update () {

    }

    public void Navigate (string mod) {

        this.DefineExercise.SetActive (false);
        this.ViewExercises.SetActive (false);
        this.DoExercise.SetActive (false);

        switch (mod) {
            case "DEFINEEXERCISE":
                this.DefineExercise.SetActive (true);
                break;
            case "VIEWEXERCISES":
                this.ViewExercises.SetActive (true);
                break;
            case "DOEXERCISE":
                this.DoExercise.SetActive (true);
                break;
            default:
                break;
        }
        //this.HandsTracking.GetComponent<HandsTrackingHandler>().setHandPointerActive(false);
        this.MainMenu.SetActive(false);

    }
}