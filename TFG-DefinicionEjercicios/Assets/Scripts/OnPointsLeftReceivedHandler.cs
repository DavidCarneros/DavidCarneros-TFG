using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPointsLeftReceivedHandler : MonoBehaviour
{
    Vector3 position;
    Color color;
    int confiance;

    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void Update()
    {
        gameObject.transform.localPosition = position;
        gameObject.GetComponent<MeshRenderer>().material.color = color;
    }

    void HandleOnPointsLeftReceived(HandInformation information)
    {
        position = information.position;
        // handPointer.transform.rotation = information.quaternion;
        confiance = information.confianceLevel;
        switch (information.confianceLevel)
        {
            case 0:
                color = Color.red;
                break;
            case 1:
                color = Color.yellow;
                break;
            case 2:
                color = Color.green;
                break;
            case 3:
                color = Color.blue;
                break;
            default:
                break;
        }
    }

    public int GetConfiance() { return confiance;  }

    void OnEnable()
    {
        HandsTrackingHandler.OnPointsLeftReceived += HandleOnPointsLeftReceived;
    }

    void OnDisable()
    {
        HandsTrackingHandler.OnPointsLeftReceived -= HandleOnPointsLeftReceived;
    }
}
