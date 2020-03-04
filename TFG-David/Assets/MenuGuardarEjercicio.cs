using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGuardarEjercicio : MonoBehaviour
{
    // Start is called before the first frame update

    public float headDistance;

    GameObject primeraParteMenu;
    GameObject guardarMenu;
    Vector3 cameraPosition;

    void Start()
    {  
        primeraParteMenu = GameObject.Find("Primera");
        guardarMenu = GameObject.Find("MenuGuardar");
        primeraParteMenu.SetActive(false);
        guardarMenu.SetActive(false);

     //   primeraParteMenu.renderer.enable = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject point = GameObject.Find("Point");
        point.transform.position = Camera.main.transform.position;
        point.transform.rotation = Camera.main.transform.rotation;
    }

    public void renderPrimerMenu()
    {
        primeraParteMenu.SetActive(true);
      //  primeraParteMenu.SetActive(true);
      //  primeraParteMenu.renderer.enable = true;
        menuPosition(primeraParteMenu);
    }

    public void renderGuardarMenu()
    {
        primeraParteMenu.SetActive(false);
        guardarMenu.SetActive(true);
        menuPosition(guardarMenu);


    }

    void menuPosition(GameObject obj)
    {
        cameraPosition = Camera.main.transform.position;
        obj.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z + headDistance);
        obj.transform.rotation = Camera.main.transform.rotation;
    }
}
