using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ViewExercises : MonoBehaviour {

    string exerciseDirectory = @"Exercises";
    public GameObject ViewExercisesMAIN;
    public GameObject ExerciseView;
    public GameObject plane;
    public GameObject buttons;
    public GameObject point;

    float viewOffset;
    List<Exercise> exercises;
    List<GameObject> exercisesView;
    int totalExercises;
    int first; // de cuantos en cuanto
    int offset; // donde empeieza

    // Start is called before the first frame update
    void Start () {
        this.exercises = new List<Exercise> ();
        this.exercisesView = new List<GameObject> ();
        first = 3;
        offset = 0;
        this.loadExercises ();
        renderPagination ();
        viewOffset = 0.0f;
    }

    // Update is called once per frame
    void Update () {

    }

    void loadExercises () {
        try {
            var exercisesLoad = Directory.EnumerateFiles (exerciseDirectory, "*.json");
            Exercise exer;
            foreach (string currentExercise in exercisesLoad) {
                var jsonString = File.ReadAllText (currentExercise);
                exer = JsonUtility.FromJson<Exercise> (jsonString);
                exercises.Add (exer);
            }
        } catch (Exception e) {
            UnityEngine.Debug.Log (e);
        }
        totalExercises = exercises.Count;
    }

    public void nextPage () {
        offset += first;
        if (offset >= totalExercises) {
            offset -= first;
        }
        int total = exercisesView.Count - 1;
        for (int i = total; i >= 0; i--) {
            GameObject aux = exercisesView[i];
            exercisesView.RemoveAt (i);
            Destroy (aux);
        }
        renderPagination ();
    }

    public void backPage () {
        offset -= first;
        if (offset <= 0) {
            offset = 0;
        }
        int total = exercisesView.Count - 1;
        for (int i = total; i >= 0; i--) {
            GameObject aux = exercisesView[i];
            exercisesView.RemoveAt (i);
            Destroy (aux);
        }
        renderPagination ();
    }

    public void viewExercise (Exercise exer) {
        plane.SetActive (false);
        buttons.SetActive (false);
        int total = exercisesView.Count - 1;

        for (int i = total; i >= 0; i--) {
            GameObject aux = exercisesView[i];
            exercisesView.RemoveAt (i);
            Destroy (aux);
        }

        for (int i = 0; i < exer.points.Count - 1; i++) {
            GameObject obj = Instantiate (point, this.transform);
            obj.transform.position = Camera.main.transform.position + exer.points[i];
        }
        ViewExercisesMAIN.GetComponent<TubeRenderer> ().SetPositions (exer.points.ToArray ());

    }

    void renderPagination () {

        // primero eliminar los otros
        int index = offset;
        int max = offset + first;

        if ((offset + first) >= totalExercises) {
            max = totalExercises;
        }

        for (int i = index; i < max; i++) {
            var exer = exercises[i];
            Vector3 pos = transform.position + new Vector3 (0.0f, viewOffset, 0.0f);
            GameObject ej = Instantiate (this.ExerciseView, pos, transform.rotation);
            ej.GetComponent<ExerciseViewHandler> ().loadExercise (exer);
            ej.SetActive (true);
            ej.transform.parent = gameObject.transform;
            viewOffset += 0.1f;
            exercisesView.Add (ej);

        }

        viewOffset = 0.0f;
        // offset = offset + first;
    }
}