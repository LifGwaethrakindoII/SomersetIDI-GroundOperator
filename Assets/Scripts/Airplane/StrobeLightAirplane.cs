using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supercargo;

public class StrobeLightAirplane : MonoBehaviour {

    private Airplane _airplane;

    public float time = .5f; //time between on and off

    public Airplane airplane
    {
        get
        {
            if (_airplane == null)
            {
                _airplane = GetComponent<Airplane>();
            }
            return _airplane;
        }
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Flicker());
    }


    IEnumerator Flicker()
    {
        while (true)
        {
            foreach (Light light in airplane.lights)
            {
                light.enabled = false;
            }
            yield return new WaitForSeconds(time);

            foreach (Light light in airplane.lights)
            {
                light.enabled = true;
            }
            yield return new WaitForSeconds(time);
        }
    }
}

