using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneSelector : MonoBehaviour {

    public GameObject A380;
    public GameObject A320;
    public GameObject Boeing787;

    public int airplaneSelected;

	// Use this for initialization
	void Start () {
		
	}

    public void LoadA380()
    {
        A380.SetActive(true);
        A320.SetActive(false);
        Boeing787.SetActive(false);
    }

    public void LoadA320()
    {
        A380.SetActive(false);
        A320.SetActive(true);
        Boeing787.SetActive(false);
    }

    public void LoadBoeing787()
    {
        A380.SetActive(false);
        A320.SetActive(false);
        Boeing787.SetActive(true);
    }

}
