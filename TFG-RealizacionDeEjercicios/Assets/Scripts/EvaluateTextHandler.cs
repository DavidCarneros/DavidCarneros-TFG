using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluateTextHandler : MonoBehaviour
{
    public GameObject AlwaysProblems;
    public GameObject NowProblems;
    public GameObject NowNotProblems;
    public GameObject NowLessFailures;
    public GameObject NowMoreFailures;
    public GameObject NowLessTime;
    public GameObject NowMoreTime;
    public GameObject Same;
    public GameObject InTime;
    public GameObject NotInTime;

    public GameObject buttonCollection;

    GameObject lastAcitve;

    // Start is called before the first frame update
    void Start()
    {
        AlwaysProblems.SetActive(false);
        NowProblems.SetActive(false);
        NowNotProblems.SetActive(false);
        NowLessFailures.SetActive(false);
        NowMoreFailures.SetActive(false);
        NowLessTime.SetActive(false);
        NowMoreTime.SetActive(false);
        Same.SetActive(false);
        InTime.SetActive(false);
        NotInTime.SetActive(false);
        buttonCollection.SetActive(false);
        lastAcitve = null;
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        switch (text)
        {
            case "AlwaysProblems":
                this.AlwaysProblems.SetActive(true);
                lastAcitve = this.AlwaysProblems;
                break;
            case "NowProblems":
                this.NowProblems.SetActive(true);
                lastAcitve = this.NowProblems;
                break;

            case "NowNotProblems":
                this.NowNotProblems.SetActive(true);
                lastAcitve = this.NowNotProblems;
                break;
            case "NowLessFailures":
                this.NowLessFailures.SetActive(true);
                lastAcitve = this.NowLessFailures;
                break;

            case "NowMoreFailures":
                this.NowMoreFailures.SetActive(true);
                lastAcitve = this.NowMoreFailures;
                break;
            case "Same":
                this.Same.SetActive(true);
                lastAcitve = this.Same;
                break;
            case "NowLessTime":
                this.NowLessTime.SetActive(true);
                lastAcitve = this.NowLessTime;
                break;
            case "NowMoreTime":
                this.NowMoreTime.SetActive(true);
                lastAcitve = this.NowMoreTime;
                break;
            case "InTime":
                this.InTime.SetActive(true);
                lastAcitve = this.InTime;
                break;
            case "NotInTime":
                this.NotInTime.SetActive(true);
                lastAcitve = this.NotInTime;
                break;
            default:
                Debug.Log("ERROR: " + text);
                break;
        }
    }

    public void DisableLast()
    {
        if (this.lastAcitve != null)
        {
            Debug.Log("Desactivada ");
            this.lastAcitve.SetActive(false);
            this.lastAcitve = null;
        }

    }

    public void DisableButtonCollection()
    {
        this.buttonCollection.SetActive(false);
    }

    public void EnableButtonCollection()
    {
        this.buttonCollection.SetActive(true);
    }
}
