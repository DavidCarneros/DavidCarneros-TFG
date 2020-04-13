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
    public ExerciseSummary summary;
    public GameObject ExerciseRender;
    public GameObject ExerciseList;
    public GameObject MainView;
    public GameObject handPointer;
    public GameObject InfoMenu;
    public GameObject HelpText;

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

    // Start is called before the first frame update
    void Start () {
        this.printedPoints = new List<Vector3> ();
        this.objectPoints = new List<GameObject> ();
        this.InfoMenu.SetActive(false);
        this.HelpText.SetActive(false);
        this.state = State.EXERCISE_NOT;
    }

    // Update is called once per frame
    void Update () {
        if (state == State.EXERCISE_START) {
            handPosition = handPointer.transform.position;
            if ((Vector3.Distance (handPosition, actualPoint)) <= exercise.exact) {
                // Punto completo
                correctPoint();
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
        if (acuFail == 5) {
            // mostrar Ayuda 
            acuFail = 0;
            this.HelpText.SetActive(true);
            this.HelpText.transform.position = printedPoints[actualPointIndex] + new Vector3(0.2f,0.2f,1f);
        }

    }

    void correctPoint () {
        acuFail = 0;
        this.HelpText.SetActive(false);
        objectPoints[actualPointIndex].GetComponent<MeshRenderer> ().material.color = Color.green;
        actualPointIndex = actualPointIndex + 1;
        if (actualPointIndex <= printedPoints.Count - 1) {
            // Hay mas puntos
            actualPoint = printedPoints[actualPointIndex];
            objectPoints[actualPointIndex].GetComponent<MeshRenderer> ().material.color = Color.yellow;
        } else {
            // Fin del ejercicio
            state = State.EXERCISE_NOT;
        }
    }

    public void setExerciseAndStart (Exercise exercise) {
        this.exercise = exercise;
        renderExercise ();
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
        InfoMenu.SetActive(true);
    }

    public void AcceptAndStart () {

        actualPoint = printedPoints[0];
        objectPoints[0].GetComponent<MeshRenderer> ().material.color = Color.yellow;
        actualPointIndex = 0;
        totalFail = 0;
        pointFail = new int[printedPoints.Count];
        acuFail = 0;
        InfoMenu.SetActive(false);
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
}