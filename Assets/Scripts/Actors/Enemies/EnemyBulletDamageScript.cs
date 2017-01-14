using UnityEngine;
using System.Collections;

public class EnemyBulletDamageScript : MonoBehaviour
{
    [SerializeField]
    int damage = 1;
	
	void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.tag == "Player" || hit.tag == "Bullets")
        {
            if (hit.gameObject.GetComponent<Health>() != null)
            {
                hit.gameObject.GetComponent<Health>().SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
