using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputName : MonoBehaviour
{

    public TouchScreenKeyboard keyboard;
    public bool keyboardVisible;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        keyboardVisible = false;
    //    m_TextComponent = GameObject.Find("textName").GetComponent<TMP_text>();
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
          //  m_TextComponent.text = keyboard.area;
        }
        else {
            
        }
    }
}
