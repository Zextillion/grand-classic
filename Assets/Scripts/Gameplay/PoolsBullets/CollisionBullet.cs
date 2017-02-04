using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Disables bullet on collide, doesn't hit if immune to bullets (ie dashing)
public class CollisionBullet : MonoBehaviour
{
    [SerializeField]
    float damage = 1.0f;
    [SerializeField]
    GameObject destroyEffect;

    private bool willGrow = true;   // If set to true, grow the object pool if there are too many instances

    // On trigger
    void OnTriggerEnter2D(Collider2D hit)
    {
        // Ensures player is not immune
        if (hit.gameObject.layer == LayerMask.NameToLayer("BulletPhaser"))
        {
            if (hit.GetComponent<DashThroughBullets>() != null)
            {
                hit.GetComponent<DashThroughBullets>().PhasedThroughBullets();
            }
        }
        else
        {
            // Applies damage
            if (hit.tag == "Player" || hit.tag == "Bullets" || hit.tag == "Enemies")
            {
                if (hit.gameObject.GetComponent<Health>() != null)
                {
                    hit.gameObject.GetComponent<Health>().SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                }
            }

            // Gets a laser gib particle from the pool
            GameObject obj = ObjectPooler.current.GetObjectForType(destroyEffect.name, willGrow);
            // If there is no destroy effect, don't display a destroy effect
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

            // Recycles the parent bullet
            ObjectPooler.current.CreatePool(transform.parent.gameObject);
        }
    }
}
