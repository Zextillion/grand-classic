using UnityEngine;
using System.Collections;

public class EnableAllScriptsOnTrigger : MonoBehaviour
{
	void OnTriggerEnter2D()
	{
		foreach(MonoBehaviour mono in gameObject.GetComponentsInChildren<MonoBehaviour>())
		{
			mono.enabled = true;
		}
	}
}
