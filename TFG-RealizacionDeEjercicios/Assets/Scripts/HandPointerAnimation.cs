using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPointerAnimation : MonoBehaviour
{

    public float maxSize = 0.3f;
    public float growFactor = 0.1f;
    public float waitTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAnimation(){
        StartCoroutine (Scale ());
    }

    IEnumerator Scale () {
        float timer = 0;
        bool alive = true;
        while (alive) // this could also be a condition indicating "alive or dead"
        {
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            while (maxSize > transform.localScale.x) {
                timer += Time.deltaTime;
                transform.localScale += new Vector3 (0.3f, 0.3f, 0.3f) * Time.deltaTime * growFactor;
                yield return null;
            }
            // reset the timer

            yield return null;

            timer = 0;
            while (0.03f < transform.localScale.x) {
                timer += Time.deltaTime;
                transform.localScale -= new Vector3 (0.3f, 0.3f, 0.3f) * Time.deltaTime * growFactor;
                yield return null;
            }

            timer = 0;
            yield return new WaitForSeconds (waitTime);
            alive = false;
            //gameObject.SetActive(false);
        }

        gameObject.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
    }
}
