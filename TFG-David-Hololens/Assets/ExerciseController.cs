using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExerciseController : MonoBehaviour {

    enum State {
        EXERCISE_START,
        EXERCISE_NOT
    }

    public Exercise exercise;
    public GameObject ExerciseRender;
    public GameObject ExerciseList;
    public GameObject MainView;
    public GameObject handPointer;
    public GameObject InfoMenu;
    public GameObject HelpText;
    public GameObject finishText;
    public GameObject lessErros;
    public GameObject lessTime;
    public GameObject congrats;
    public GameObject restartMenu;
    public GameObject restartText;
    // 
    public GameObject particles;

    List<Vector3> printedPoints;
    List<GameObject> objectPoints;
    List<ExerciseSummary> SummaryList;

    string mainPath = @"Exercises\";

    State state = State.EXERCISE_NOT;
    Vector3 handPosition;
    Vector3 actualPoint;
    int actualPointIndex;
    int totalFail;
    int[] pointFail;
    int acuFail;

    DateTime initTime;
    DateTime endTime;

    // Start is called before the first frame update
    void Start () {
        this.printedPoints = new List<Vector3> ();
        this.objectPoints = new List<GameObject> ();
        this.InfoMenu.SetActive (false);
        this.HelpText.SetActive (false);
        this.finishText.SetActive (false);
        this.lessErros.SetActive (false);
        this.lessTime.SetActive (false);
        this.congrats.SetActive (false);
        this.restartMenu.SetActive (false);
        this.restartText.SetActive (false);
        this.state = State.EXERCISE_NOT;
    }

    // Update is called once per frame
    void Update () {
        if (state == State.EXERCISE_START) {
            handPosition = handPointer.transform.position;
            if ((Vector3.Distance (handPosition, actualPoint)) <= exercise.exact) {
                // Punto completo
                correctPoint ();
            } else {
                // comparar con los otros 
                checkOtherPoints (handPosition);
            }
        }
    }

    void checkOtherPoints (Vector3 handPosition) {
        int index = actualPointIndex + 1;
        for (int i = index; i < printedPoints.Count - 1; i++) {

            if ((Vector3.Distance (handPosition, printedPoints[i])) <= exercise.exact) {
                totalFail++;
                pointFail[i]++;
                acuFail++;
                break;
            }

        }
        if (acuFail == 10) {
            // mostrar Ayuda 
            acuFail = 0;
            this.HelpText.SetActive (true);
            this.HelpText.transform.position = printedPoints[actualPointIndex] + new Vector3 (0f, 0.2f, 0.5f);
        }

    }

    void correctPoint () {
        if (actualPointIndex == 0) {
            initTime = DateTime.Now;
        }
        acuFail = 0;
        this.HelpText.SetActive (false);
        objectPoints[actualPointIndex].GetComponent<MeshRenderer> ().material.color = Color.green;
        // particles
        particles.SetActive(true);
        particles.transform.position = objectPoints[actualPointIndex].transform.position;
        particles.GetComponent<ParticleSystem>().Play();


        actualPointIndex = actualPointIndex + 1;
        if (actualPointIndex <= printedPoints.Count - 1) {
            // Hay mas puntos
            actualPoint = printedPoints[actualPointIndex];
            objectPoints[actualPointIndex].GetComponent<MeshRenderer> ().material.color = Color.yellow;
        } else {
            // Fin del ejercicio
            endTime = DateTime.Now;
            state = State.EXERCISE_NOT;
            exerciseFinish ();
        }
    }

    void exerciseFinish () {
        float totalTime = (endTime - initTime).Seconds;
        ExerciseSummary summary = new ExerciseSummary (totalFail, pointFail, totalTime, null);
        string timeStamp = GetTimestamp (DateTime.Now);
        string jsonString = JsonUtility.ToJson (summary);
        string fileName = "Exercises/" + exercise.name + "/" + timeStamp + ".json";
        File.WriteAllText (fileName, jsonString);
        SummaryList.Add (summary);
        if (SummaryList.Count > 1) {
            StartCoroutine (InforExerciseRoutine ());
        } else {
            // Solo enhorabuena
            StartCoroutine (RestartTextRoutineNew ());

        }
    }

    public void restartExercise () {
        actualPoint = printedPoints[0];
        objectPoints[0].GetComponent<MeshRenderer> ().material.color = Color.yellow;
        for (int i = 1; i < objectPoints.Count - 1; i++) {
            objectPoints[i].GetComponent<MeshRenderer> ().material.color = Color.red;
        }
        actualPointIndex = 0;
        totalFail = 0;
        pointFail = new int[printedPoints.Count];
        acuFail = 0;
        restartMenu.SetActive (false);
        StartCoroutine (RestartTextRoutine ());
        state = State.EXERCISE_START;
    }

    private IEnumerator RestartTextRoutineNew () {
        this.finishText.SetActive (true);
        this.finishText.transform.position = Camera.main.transform.position + new Vector3 (0f, 0f, 1f);
        yield return new WaitForSeconds (4);
        this.finishText.SetActive (false);

        viewRestartMenu ();
    }

    private IEnumerator RestartTextRoutine () {

        this.restartText.SetActive (true);
        this.restartText.transform.position = Camera.main.transform.position + new Vector3 (0f, 0f, 0.7f);
        yield return new WaitForSeconds (4);
        this.restartText.SetActive (false);

    }

    private IEnumerator InforExerciseRoutine () {

        ExerciseSummary actualSummary = SummaryList[SummaryList.Count - 1];
        this.finishText.SetActive (true);
        this.finishText.transform.position = Camera.main.transform.position + new Vector3 (0, 0.0f, 1f);
        yield return new WaitForSeconds (5);
        this.finishText.SetActive (false);

        float totalTimeAvg = 0;
        float totalErrorAvg = 0;
        for (int i = 0; i < SummaryList.Count - 2; i++) {
            totalErrorAvg += SummaryList[i].totalErros;
            totalTimeAvg += SummaryList[i].totalTime;
        }
        totalErrorAvg = totalErrorAvg / (SummaryList.Count - 1);
        totalTimeAvg = totalTimeAvg / (SummaryList.Count - 1);

        if (totalErrorAvg < actualSummary.totalErros) {
            // Se ha equivocado menos
            this.lessErros.SetActive (true);
            this.lessErros.transform.position = Camera.main.transform.position + new Vector3 (0, 0.0f, 1f);
            yield return new WaitForSeconds (3);
            this.lessErros.SetActive (false);
        }

        if (totalTimeAvg < actualSummary.totalTime) {
            this.lessTime.SetActive (true);
            this.lessTime.transform.position = Camera.main.transform.position + new Vector3 (0, 0.0f, 1f);
            yield return new WaitForSeconds (3);
            this.lessTime.SetActive (false);
        }

        viewRestartMenu ();

    }

    void viewRestartMenu () {
        this.restartMenu.SetActive (true);
        this.restartMenu.transform.position = Camera.main.transform.position + new Vector3 (0, 0.0f, 1f);
    }

    public void setExerciseAndStart (Exercise exercise) {
        this.exercise = exercise;
        renderExercise ();
        SummaryList = new List<ExerciseSummary> ();
        // Comprobar si directorio de los summary existen 
        if (Directory.Exists (mainPath + exercise.name)) {
            var summaryFiles = Directory.EnumerateFiles (mainPath + exercise.name, "*.json");
            ExerciseSummary summary;
            foreach (string currentSummary in summaryFiles) {
                var jsonString = File.ReadAllText (currentSummary);
                summary = JsonUtility.FromJson<ExerciseSummary> (jsonString);
                SummaryList.Add (summary);
            }
        } else {
            Directory.CreateDirectory (mainPath + exercise.name);
        }
        InfoMenu.SetActive (true);
    }

    public void AcceptAndStart () {

        actualPoint = printedPoints[0];
        objectPoints[0].GetComponent<MeshRenderer> ().material.color = Color.yellow;
        actualPointIndex = 0;
        totalFail = 0;
        pointFail = new int[printedPoints.Count];
        acuFail = 0;
        InfoMenu.SetActive (false);
        state = State.EXERCISE_START;

    }

    public void renderExercise () {

        for (int i = 0; i < exercise.points.Count - 1; i++) {
            GameObject point = GameObject.CreatePrimitive (PrimitiveType.Sphere);
            Vector3 position = Camera.main.transform.position + exercise.points[i];
            point.transform.localScale = new Vector3 (0.03f, 0.03f, 0.03f);
            point.GetComponent<MeshRenderer> ().material.color = Color.red;
            point.transform.position = Camera.main.transform.position + exercise.points[i];
            point.transform.parent = ExerciseRender.transform;
            printedPoints.Add (position);
            objectPoints.Add (point);

        }

        gameObject.GetComponent<TubeRenderer> ().SetPositions (printedPoints.ToArray ());

    }

    public static String GetTimestamp (DateTime value) {
        return value.ToString ("yyyyMMddHHmmssffff");
    }
}