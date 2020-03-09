using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizacion : MonoBehaviour
{
    // Start is called before the first frame update


    public Exercise exer;
    public List<GameObject> objectPointList;
    public GameObject objectPoint;

    GameObject ejerciciosList;
    void Start()
    {
        ejerciciosList = GameObject.Find("Ejercicios");
        objectPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setExercise(Exercise aux)
    {
        this.exer = aux;
    }

    public void dibujarTrayectoria()
    {
        ejerciciosList.SetActive(false);
        objectPointList = new List<GameObject>();
        for(int i = 0; i < this.exer.points.Count ; i++)
        {
            Vector3 position = this.exer.points[i];
            GameObject p = Instantiate(this.objectPoint, position, transform.rotation);
            p.transform.parent = gameObject.transform;
            p.SetActive(true);
            objectPointList.Add(p);
        }
        gameObject.GetComponent<TubeRenderer>().SetPositions(this.exer.points.ToArray());

    }
}
