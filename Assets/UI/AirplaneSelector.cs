using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AirplaneSelector : MonoBehaviour {

    public GameObject A380;
    public GameObject A320;
    public GameObject Boeing787;

    public int airplaneSelected;

	// Use this for initialization
	void Start() 
    {
        A380.GetComponent<AirplaneEngineDijkstra>().enabled = false;
        A320.GetComponent<AirplaneEngineDijkstra>().enabled = false;
        Boeing787.GetComponent<AirplaneEngineDijkstra>().enabled = false;
		LoadA380();
	}

    public void LoadA380()
    {
        A380.SetActive(true);
        A320.SetActive(false);
        Boeing787.SetActive(false);
        PlayerPrefs.SetString("Airplane", "A380");
    }

    public void LoadA320()
    {
        A380.SetActive(false);
        A320.SetActive(true);
        Boeing787.SetActive(false);
        PlayerPrefs.SetString("Airplane", "A320");
    }

    public void LoadBoeing787()
    {
        A380.SetActive(false);
        A320.SetActive(false);
        Boeing787.SetActive(true);
        PlayerPrefs.SetString("Airplane", "Boeing787");
    }

    public void ToEvaluationScene()
    {
        SceneManager.LoadScene("Aproximation Area");
    }

}
