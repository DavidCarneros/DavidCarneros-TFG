using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;


public class viewExercises : MonoBehaviour
{

    string exercisesDirectory = @"Ejercicios";
    GameObject vistaEjercicio;
    // Start is called before the first frame update
    void Start()
    {
        vistaEjercicio = GameObject.Find("VistaEjercicio");
        vistaEjercicio.SetActive(false);
        this.loadExercises();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void loadExercises(){

        try {
            var exercises = Directory.EnumerateFiles(exercisesDirectory, "*.json");
            Exercise exer;
            foreach (string currentExercise in exercises)
            {
                var jsonString = File.ReadAllText(currentExercise);
                exer = JsonUtility.FromJson<Exercise>(jsonString);
                GameObject ej = Instantiate(this.vistaEjercicio,transform.position, transform.rotation);
                ej.GetComponent<ExerciseHandler>().loadExercise(exer);
                ej.SetActive(true);
                ej.transform.parent = gameObject.transform;
               // GameObject obj = GameObject.Find("VistaEjercicio");
               // obj.GetComponent<ExerciseHandler>().loadExercise(exer);
            }
        }
        catch(Exception e)
        {
             UnityEngine.Debug.Log(e);
        }
        
    }
}
