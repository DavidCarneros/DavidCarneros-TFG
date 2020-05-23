using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DefineExerciseObject;

    void Start()
    {
        this.DefineExerciseObject.SetActive(false);
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

    public void Restart(){
        if(this.DefineExerciseObject.activeSelf){
            this.DefineExerciseObject.SetActive(false);
            this.DefineExerciseObject.GetComponent<DefineExerciseHandler>().CleanObjects();
        }
        
        gameObject.SetActive(true);
    }
}
