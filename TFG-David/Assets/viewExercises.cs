using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.Json;


public class viewExercises : MonoBehaviour
{

    string exercisesDirectory = @"\Ejercicios";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void loadExercises(){
        try {
            var exercises = Directory.EnumerateFiles(exercisesDirectory, "*.json");
            var jsonString;
            Exercise exer;
            foreach (string currentExercise in exercises)
            {
                jsonString = File.ReadAllText(currentExercise);
                exer = JsonSerializer.Deserialize<Exercise>(jsonString);
            }
        }
        catch(Exception e)
        {
             
        }
        
    }
}
