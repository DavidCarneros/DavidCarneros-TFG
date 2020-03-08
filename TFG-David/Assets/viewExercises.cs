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
    // Start is called before the first frame update
    void Start()
    {
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
                UnityEngine.Debug.Log(currentExercise);
                var jsonString = File.ReadAllText(currentExercise);
                exer = JsonUtility.FromJson<Exercise>(jsonString);
                UnityEngine.Debug.Log(exer.hand);
                GameObject obj = GameObject.Find("VistaEjercicio");
                obj.GetComponent<ExerciseHandler>().loadExercise(exer);
            }
        }
        catch(Exception e)
        {
             UnityEngine.Debug.Log(e);
        }
        
    }
}
