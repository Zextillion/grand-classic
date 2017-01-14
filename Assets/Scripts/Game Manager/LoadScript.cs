using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScript : MonoBehaviour {

	// Use this for initialization
	public void LoadByIndex (int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
        Application.LoadLevel("Test Scene");
    }
}
