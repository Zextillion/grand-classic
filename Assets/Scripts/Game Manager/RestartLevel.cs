using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown("Restart"))
        {
            //SceneManager.LoadScene("Test Scene", LoadSceneMode.Additive);
            Time.timeScale = 1f;
            Application.LoadLevel("Boss Test");
        }
	}
}
