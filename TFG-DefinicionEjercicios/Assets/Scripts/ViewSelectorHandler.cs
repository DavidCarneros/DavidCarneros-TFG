using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSelectorHandler : MonoBehaviour
{

    public GameObject ViewProgressObject;
    public GameObject ViewExercisesObject;
    public GameObject ViewSelector;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewProgress()
    {
        gameObject.SetActive(false);
        //this.ViewSelector.SetActive(false);
        this.ViewExercisesObject.SetActive(false);
        this.ViewProgressObject.SetActive(true);
    }

    public void ViewExercise()
    {
        gameObject.SetActive(false);
        //this.ViewSelector.SetActive(false);
        this.ViewProgressObject.SetActive(false);
        this.ViewExercisesObject.SetActive(true);
        this.ViewExercisesObject.GetComponent<ViewExercisesHandler>().GetAllExercises();
    }

    public void Back()
    {
        this.ViewProgressObject.SetActive(false);
        this.ViewExercisesObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
