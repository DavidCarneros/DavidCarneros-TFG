using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputKeyboardHandler : MonoBehaviour
{


    public GameObject KeyboardText;
    public GameObject KeyboardCaller;
    public GameObject Keyboard;
    public GameObject InputField;

    public string text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveKeyboard(GameObject KeyboardCaller, GameObject InputField)
    {
        this.InputField = InputField;
        this.KeyboardCaller = KeyboardCaller;
        this.KeyboardCaller.SetActive(false);
        this.Keyboard.SetActive(true);
    }

    public void SetText()
    {
        this.text = this.KeyboardText.GetComponent<TextMeshProUGUI>().text;
        this.Keyboard.SetActive(false);
        this.InputField.GetComponent<TextMesh>().text = this.text;
        this.KeyboardCaller.SetActive(true);
        this.InputField = null;
        this.KeyboardCaller = null;
    }
}
