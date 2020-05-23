using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ExerciseHandler : MonoBehaviour {
    public GameObject HelpText;
    public GameObject RoutineHUD;
    public GameObject NextRepetitionsText;
    // Ejercicio
    public Exercise exercise;
    public Routine routine;

    // HandPointer
    public GameObject HandPointer;
    //public GameObject Point;

    // Exercise Object 
    public GameObject TorusObject;
    public GameObject PointObject;
    public GameObject TextParticlePlusOne;
    public GameObject ViewExercise; // Objeto empty que sera el padre de los renderizados

    // Objects list
    List<Vector3> VisualPointsPosition;
    List<GameObject> VisualPoints;

    bool exerciseStarted;
    bool isBack;
    int index;
    int repet;
    bool checkingFail;
    int acuFail;

    // 
    List<ExerciseResult> ExerciseResultList;
    ExerciseResult actualResult;
    DateTime startAt;
    DateTime endAt;
    DateTime previousDateTime;

    // Start is called before the first frame update
    void Start () {
        index = 0;
        exerciseStarted = false;
        isBack = false;
        ExerciseResultList = new List<ExerciseResult> ();
        checkingFail = false;

    }

    // Update is called once per frame
    void Update () {
        if (exerciseStarted) {
            ComparePoints ();
        }
    }

    public void SetExerciseAndStart (Routine routine) {
        this.routine = routine;
        this.exercise = routine.exercise;
        this.isBack = false;
        index = 0;
        repet = 0;
        DrawExercise ();
        StartEndExercise (true);
        RoutineHUD.GetComponent<HUDHandler> ().SetTextExercise (GetExerciseTextHub ());
        InitActualResult ();
        this.ExerciseResultList = new List<ExerciseResult> ();
    }

    void InitActualResult () {
        this.actualResult = new ExerciseResult ();
        int totalPoints = this.exercise.points.Count;
        this.actualResult.failures = new int[totalPoints];
        this.actualResult.time = new double[totalPoints];
        if (this.exercise.back) {
            this.actualResult.failures_back = new int[totalPoints];
            this.actualResult.time_back = new double[totalPoints];
        }
        this.startAt = DateTime.Now;
        this.actualResult.startAt = this.startAt.ToString ("o", CultureInfo.InvariantCulture);
    }

    public void FinishExercise () {
        this.endAt = DateTime.Now;
        this.actualResult.endAt = this.endAt.ToString ("o", CultureInfo.InvariantCulture);
        this.actualResult.total_time = (this.endAt - this.startAt).TotalSeconds;
        int total_failures = 0;
        for (int i = 0; i < this.actualResult.failures.Length; i++) {
            total_failures += this.actualResult.failures[i];
        }
        if (this.exercise.back) {
            for (int i = 0; i < this.actualResult.failures_back.Length; i++) {
                total_failures += this.actualResult.failures_back[i];
            }
        }
        actualResult.total_failures = total_failures;
        ExerciseResultList.Add (actualResult);

        if (repet < routine.repetitions - 1) {

            InitActualResult ();
            StartCoroutine (NextRepetitionSequency ());
        } else {
            gameObject.GetComponent<RoutineHandler> ().NextRoutine (ExerciseResultList.ToArray ());
        }
    }

    IEnumerator NextRepetitionSequency () {
        repet++;
        index = 0;
        isBack = false;
        this.NextRepetitionsText.SetActive (true);
        this.NextRepetitionsText.transform.position = Camera.main.transform.position + new Vector3 (0f, 0f, 0.4f);
        yield return new WaitForSeconds (3);
        this.NextRepetitionsText.SetActive (false);
        DrawExercise ();
        StartEndExercise (true);
        RoutineHUD.GetComponent<HUDHandler> ().SetTextExercise (GetExerciseTextHub ());
        yield return null;
    }

    ////////////////////////////////////////////
    ////////////////////////////////////////////
    ////////////////////////////////////////////
    public void DEBUG () {
        StartCoroutine (GetExerciseRequest ());
    }

    IEnumerator GetExerciseRequest () {
        UnityWebRequest webRequest = new UnityWebRequest ("http://192.168.1.163:3000/exercise/14", "GET");
        webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer ();

        UnityWebRequestAsyncOperation requestHandler = webRequest.SendWebRequest ();
        requestHandler.completed += delegate (AsyncOperation pOperation) {
            string jsonString = webRequest.downloadHandler.text;
            Exercise exer = JsonUtility.FromJson<Exercise> (jsonString);
            this.exercise = exer;
            DrawExercise ();
            StartEndExercise (true);
            index = 0;
        };

        yield return null;
    }
    ////////////////////////////////////////////
    ////////////////////////////////////////////
    ////////////////////////////////////////////

    public void StartEndExercise (bool startEnd) {
        exerciseStarted = startEnd;
    }

    // Draw exercise
    public void DrawExercise () {
        index = 0;
        acuFail = 0;
        VisualPoints = new List<GameObject> ();
        VisualPointsPosition = new List<Vector3> ();

        int bt = exercise.keyPoint + 1;

        for (int i = 0; i < this.exercise.points.Count; i++) {

            // Torus
            if (i != 0 && (i % bt == (exercise.keyPoint))) {
                var before = this.exercise.points[i];
                if (i + 1 >= this.exercise.points.Count) {
                    before = this.exercise.points[i];
                } else {
                    before = this.exercise.points[i + 1];
                }
                //var before = this.exercise.points[i + 1];
                var vector = before - this.exercise.points[i - 1];

                var rotation = Quaternion.LookRotation (vector.normalized);
                Quaternion newRotation = rotation * Quaternion.Euler (-180, 90, 90);
                Vector3 torusPosition = new Vector3 (this.exercise.points[i].x, this.exercise.points[i].y, this.exercise.points[i].z) + Camera.main.transform.position;
                Vector3 centralPoint = this.exercise.points[i] + Camera.main.transform.position;

                if (vector.normalized.x != 0.0 || vector.normalized.z != 0.0) {
                    torusPosition = torusPosition + new Vector3 (0, -0.05f, 0);
                }
                if (vector.normalized.y != 0.0) {
                    if (vector.normalized.y > 0.0) {
                        torusPosition = torusPosition + new Vector3 (0, 0, +0.05f);
                    } else {
                        torusPosition = torusPosition + new Vector3 (0, 0, -0.05f);
                    }
                }

                GameObject torusPoint = Instantiate (this.TorusObject, torusPosition, Quaternion.identity);
                torusPoint.tag = "Torus";
                torusPoint.transform.rotation = newRotation;
                torusPoint.transform.parent = ViewExercise.transform;
                torusPoint.SetActive (true);
                this.VisualPoints.Add (torusPoint);
                this.VisualPointsPosition.Add (centralPoint);
            }
            // Point
            else {
                Vector3 pointPosition = this.exercise.points[i] + Camera.main.transform.position;
                GameObject spherePoint = Instantiate (this.PointObject, pointPosition, Quaternion.identity);
                spherePoint.transform.parent = ViewExercise.transform;
                spherePoint.tag = "Sphere";
                if (i < exercise.keyPoint) {
                    spherePoint.SetActive (true);
                }
                this.VisualPoints.Add (spherePoint);
                this.VisualPointsPosition.Add (spherePoint.transform.position);
            }
        }

        VisualPoints[0].transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
        VisualPoints[0].GetComponent<MeshRenderer> ().material.color = Color.yellow;

    }

    // Compare function 
    public void ComparePoints () {
        Vector3 handPointerPosition = HandPointer.transform.position;

        if (Vector3.Distance (handPointerPosition, this.VisualPointsPosition[index]) <= 0.03) {
            acuFail = 0;
            DateTime dateAux = DateTime.Now;
            if (!isBack) {
                // Estoy ida
                if (index == 0) {
                    this.actualResult.time[index] = (dateAux - this.startAt).TotalSeconds;
                } else {
                    this.actualResult.time[index] = (dateAux - previousDateTime).TotalSeconds;
                }

            } else {
                // Estoy vuelta
                if (index == 0) {
                    this.actualResult.time_back[index] = (dateAux - this.startAt).TotalSeconds;
                } else {
                    this.actualResult.time_back[index] = (dateAux - previousDateTime).TotalSeconds;
                }

            }

            GameObject text = Instantiate (this.TextParticlePlusOne, this.VisualPointsPosition[index], Quaternion.identity);
            text.SetActive (true);
            text.GetComponent<TextParticleHandler> ().StartAnimation ();

            GameObject obj = VisualPoints[index];
            this.HandPointer.GetComponent<HandPointerAnimation> ().StartAnimation ();
            if (obj.tag == "Torus") {
                obj.GetComponent<TorusHandler> ().StartAnimation ();
                for (int i = index + 1;
                    ((i < (index + exercise.keyPoint + 1)) && (i < VisualPointsPosition.Count)); i++) {
                    this.VisualPoints[i].SetActive (true);
                }

                if (index + 1 < this.VisualPoints.Count) {
                    if (this.VisualPoints[index + 1].tag == "Sphere") {
                        this.VisualPoints[index + 1].transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
                        this.VisualPoints[index + 1].GetComponent<MeshRenderer> ().material.color = Color.yellow;
                    }
                }

            } else {
                gameObject.GetComponent<AudioSource> ().Play ();
                this.VisualPoints[index].SetActive (false);
                Destroy (this.VisualPoints[index]);

                if (index + 1 < this.VisualPoints.Count) {
                    if (this.VisualPoints[index + 1].tag == "Sphere") {
                        this.VisualPoints[index + 1].transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
                        this.VisualPoints[index + 1].GetComponent<MeshRenderer> ().material.color = Color.yellow;
                    } else {
                        this.VisualPoints[index + 1].GetComponent<MeshRenderer> ().material.color = Color.yellow;
                    }
                }

            }

            index += 1;
            this.previousDateTime = dateAux;
        } else {
            bool noOne = true;
            for (int i = 0; i < VisualPointsPosition.Count; i++) {
                if (i != index) {
                    if (Vector3.Distance (handPointerPosition, this.VisualPointsPosition[i]) <= 0.03) {
                        noOne = false;
                        if (!checkingFail) {
                            checkingFail = true;
                            acuFail++;
                            if (isBack) {
                                this.actualResult.failures_back[index]++;
                                break;
                            } else {
                                this.actualResult.failures[index]++;
                                break;
                            }
                            
                        }

                    }
                }
            }
            if (noOne) {
                checkingFail = false;
            }
        }
        Debug.Log(acuFail);
        if(acuFail >= 3){
            if (this.VisualPoints[index].tag == "torus"){
                //this.VisualPoints[index].GetComponent<TorusHandler>().Blink();
            }
            else {
                Debug.Log("ACTIVADO PARPADEO");
                this.VisualPoints[index].GetComponent<PointHandler>().Blink();
            }
            acuFail = 0;
        }

        if (index == VisualPointsPosition.Count) {
            exerciseStarted = false;
            if (exercise.back) {
                if (!isBack) {
                    backExercise ();
                    exerciseStarted = true;
                    isBack = true;
                } else {
                    FinishExercise ();
                }
            } else {
                FinishExercise ();
            }
        }
    }

    public void backExercise () {
        index = 0;
        List<Vector3> exercisePointReverse = Enumerable.Reverse (VisualPointsPosition).ToList ();

        VisualPoints = new List<GameObject> ();
        VisualPointsPosition = new List<Vector3> ();

        int bt = exercise.keyPoint + 1;

        for (int i = 0; i < exercisePointReverse.Count; i++) {

            // Torus
            if (i != 0 && (i % bt == (exercise.keyPoint))) {
                var before = this.exercise.points[i];
                if (i + 1 >= this.exercise.points.Count) {
                    before = this.exercise.points[i];
                } else {
                    before = this.exercise.points[i + 1];
                }
                var vector = before - exercisePointReverse[i - 1];

                var rotation = Quaternion.LookRotation (vector.normalized);
                Quaternion newRotation = rotation * Quaternion.Euler (-180, 90, 90);
                Vector3 torusPosition = new Vector3 (exercisePointReverse[i].x, exercisePointReverse[i].y, exercisePointReverse[i].z) + Camera.main.transform.position;
                Vector3 centralPoint = exercisePointReverse[i] + Camera.main.transform.position;

                if (vector.normalized.x != 0.0 || vector.normalized.z != 0.0) {
                    torusPosition = torusPosition + new Vector3 (0, -0.05f, 0);
                }
                if (vector.normalized.y != 0.0) {
                    if (vector.normalized.y > 0.0) {
                        torusPosition = torusPosition + new Vector3 (0, 0, +0.05f);
                    } else {
                        torusPosition = torusPosition + new Vector3 (0, 0, -0.05f);
                    }
                }

                GameObject torusPoint = Instantiate (this.TorusObject, torusPosition, Quaternion.identity);
                torusPoint.tag = "Torus";
                torusPoint.transform.rotation = newRotation;
                torusPoint.transform.parent = ViewExercise.transform;
                torusPoint.SetActive (true);
                this.VisualPoints.Add (torusPoint);
                this.VisualPointsPosition.Add (centralPoint);
            }
            // Point
            else {
                Vector3 pointPosition = exercisePointReverse[i] + Camera.main.transform.position;
                GameObject spherePoint = Instantiate (this.PointObject, pointPosition, Quaternion.identity);
                spherePoint.transform.parent = ViewExercise.transform;
                spherePoint.tag = "Sphere";
                if (i < exercise.keyPoint) {
                    spherePoint.SetActive (true);
                }
                this.VisualPoints.Add (spherePoint);
                this.VisualPointsPosition.Add (spherePoint.transform.position);
            }
        }

    }

    string GetExerciseTextHub () {
        return (repet + 1) + "/" + routine.repetitions;
    }
}