using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour {

	public void PlaySimulation(string _nameScene)
    {
        SceneManager.LoadScene(_nameScene);
    }

    public void QuitApp()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}
