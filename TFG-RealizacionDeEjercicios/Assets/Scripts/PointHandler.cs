using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Blink(){
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine(){
        float timer = 0;
        float maxSize = 0.04f;
        float growFactor = 0.3f;

        while(true){
            while(maxSize > transform.localScale.x){
                timer += Time.deltaTime;
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime * growFactor;
                yield return null;
            }
            timer = 0;
            yield return null;
            while(0.02f < transform.localScale.x){
                timer += Time.deltaTime;
                transform.localScale -= new Vector3(0.1f,0.1f,0.1f) * Time.deltaTime * growFactor;
                yield return null;
            }

            timer = 0;

        }

    }
}
