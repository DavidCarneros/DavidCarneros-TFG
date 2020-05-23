using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParticleHandler : MonoBehaviour {

    public float distancePerSecond = 0.5f;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void StartAnimation () {
        //StartCoroutine (Move ());
        StartCoroutine(MoveV2());
    }

    IEnumerator Move () {
        Vector3 position = transform.position;
        transform.Translate (0, distancePerSecond * Time.deltaTime, 0);

        yield return new WaitForSeconds (3);
        Destroy (gameObject);
    }

    IEnumerator MoveV2() {
        var currentPos = transform.position;
        var newPosition = new Vector3(currentPos.x, currentPos.y + 1f, currentPos.z);
        var timeToMove = 3;
        var t = 0f;
        while (t < 1) {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp (currentPos, newPosition, t);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}