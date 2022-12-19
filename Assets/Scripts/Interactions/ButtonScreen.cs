using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScreen : MonoBehaviour {

    public GameObject displayAir;

    public Material[] screenMaterial;
    Renderer render;

    public void Start()
    {
        //displayAir = GetComponent<GameObject>();
        render = displayAir.GetComponent<Renderer>();
        render.sharedMaterial = screenMaterial[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Enter");
            render.sharedMaterial = screenMaterial[1];
        }
    }



}
