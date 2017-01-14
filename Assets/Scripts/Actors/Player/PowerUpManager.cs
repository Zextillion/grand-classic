using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    public FireOnePlayer[] weapons;
    private int powerLevel = 0;
    [SerializeField]
    GameObject maximumPower;

    void PowerUp()
    {
        ++powerLevel;
        if (powerLevel < weapons.Length)
        {
            if (weapons[powerLevel] != null)
            {
                weapons[powerLevel].enabled = true;
            }
        }
        if (powerLevel == 15)
        {
            Instantiate(maximumPower, transform.position, transform.rotation);
        }
    }
}
