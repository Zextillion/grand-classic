using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThroughBullets : MonoBehaviour
{
    [SerializeField]
    GameObject phaseEffect;
    [SerializeField]
    bool willGrow = true;

    // If hits a target that can phase through bullets, display this effect
    public void PhasedThroughBullets()
    {
        GameObject obj = ObjectPooler.current.GetObjectForType(phaseEffect.name, willGrow);
        if (obj == null)
        {
            return;
        }
        else
        {   // Else display a destroy effect
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
        }
    }
}
