using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hit)
    {
        hit.GetComponent<PowerUpManager>().SendMessage("PowerUp");
        gameObject.GetComponent<Destroy>().SendMessage("DestroyGameObject");
    }
}
