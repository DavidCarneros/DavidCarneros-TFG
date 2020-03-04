using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputName : MonoBehaviour
{

    public TouchScreenKeyboard keyboard;
    public bool keyboardVisible;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        keyboardVisible = false;
        text = GameObject.Find("textName");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnKeyboard()
    {
        if(!keyboardVisible){
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
            keyboardVisible = true;
            text.GetComponent<TextMeshPro>().text = keyboard.area;
        }
        else {
            
        }
    }
}
