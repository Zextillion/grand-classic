using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that's called when player dies
public class GameOver : MonoBehaviour
{
    public static GameOver current;

    void Awake()
    {
        current = this;
    }

    // Displays a game over message
    public void DisplayMessage()
    {
        Debug.Log("you died LMFAO. Press 'q' to restart the level");
    }
}
