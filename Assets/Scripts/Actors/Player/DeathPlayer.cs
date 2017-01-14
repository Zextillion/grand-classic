using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Special script for a player dying
public class DeathPlayer : MonoBehaviour
{
    public static DeathPlayer current;

    void Awake()
    {
        current = this;
    }

    public void Death()
    {
        GameOver.current.GetComponent<GameOver>().DisplayMessage();
        gameObject.SetActive(false);
    }
}
