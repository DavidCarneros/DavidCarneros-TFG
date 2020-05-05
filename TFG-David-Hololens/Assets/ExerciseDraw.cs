using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseDraw : MonoBehaviour {

    List<Vector3> points;
    List<GameObject> objects;

    public GameObject torus;
    public GameObject sphere;

    int bt = 3;

    // DEBUG
    public GameObject textParticle;
    public GameObject handPointer;
    int index = 0;

    // Start is called before the first frame update
    void Start () {
        torus.SetActive (false);
        sphere.SetActive (false);
        index = 0;
    }

    // Update is called once per frame
    void Update () {
        comparePoints ();
    }

    public void buildExercise (List<Vector3> points) {
        this.points = new List<Vector3> (points);
        this.objects = new List<GameObject> ();

        for (int i = 0; i < points.Count - 1; i++) {

            if (i != 0 && (i % bt == (bt - 1))) {

                // Torus
                var before = points[i + 1];
                var vector = before - this.points[i];

                var rotation = Quaternion.LookRotation (vector.normalized);

                Vector3 torusPosition = new Vector3 (this.points[i].x, this.points[i].y - 0.09f, this.points[i].z);
                GameObject torusPoint = Instantiate (this.torus, torusPosition, Quaternion.identity);
                torusPoint.tag = "Torus";
                torusPoint.transform.rotation = rotation * Quaternion.Euler (-180, 90, 90);
                torusPoint.SetActive (true);
                this.objects.Add (torusPoint);
            } else {

                GameObject spherePoint = Instantiate (this.sphere, this.points[i], Quaternion.identity);
                spherePoint.tag = "Sphere";
                if (i < bt) {
                    spherePoint.SetActive (true);
                }
                this.objects.Add (spherePoint);

            }

        }
    }

    // TEST FUNCTION 

    void comparePoints () {

        Vector3 handPointerPosition = handPointer.transform.position;
        //Vector3 handPointerPosition = new Vector3 (0, 0, 0);
        if (this.points != null) {
            if (Vector3.Distance (handPointerPosition, this.points[index]) <= 0.05) {
                GameObject text = Instantiate(this.textParticle, this.points[index],Quaternion.identity);
                text.SetActive(true);
                text.GetComponent<TextParticleHandler>().StartAnimation();

                GameObject obj = objects[index];
                this.handPointer.GetComponent<HandPointerAnimation>().StartAnimation();
                if (obj.tag == "Torus") {
                    obj.GetComponent<TorusHandler> ().StartAnimation ();
                    for (int i = index;
                        (i < (index + bt)) && (i < points.Count - 1); i++) {
                        this.objects[i].SetActive (true);
                    }
                } else {
                    this.objects[index].SetActive (false);
                    Destroy(this.objects[index]);
                }
                index += 1;

            }
        }

    }
}