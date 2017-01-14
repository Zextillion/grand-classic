using UnityEngine;
using System.Collections;

public class HarassPlayer : MonoBehaviour
{
    [SerializeField]
    float maxDistance = 1.0f;
   // [SerializeField]
   // float minDistance = 0.0f;
    [SerializeField]
    float moveTime = 1.0f;
    [SerializeField]
    float waitTime = 1.0f;

    // Use this for initialization
    void Start()
    {
        //if (maxDistance < minDistance)
        //{
        //    Debug.LogWarning("WARNING: maxDistance < minDistance");
        //}
        StartCoroutine("RandomMove");
    }

    IEnumerator RandomMove()
    {
        if (PlayerManager.current != null)
        {
            float t = 0.0f;
            float currentMoveTime = moveTime;
            Vector3 initialPosition = transform.position;
            //float randomAreaMax = maxDistance;
            //float randomAreaMin = minDistance;

            //while (randomAreaMax < randomAreaMin)
            //{

            //}

            Vector3 moveVector = Random.insideUnitCircle * maxDistance;
            Vector3 endPosition = PlayerManager.current.transform.position + moveVector;

            if (currentMoveTime < 0.0f)
            {
                currentMoveTime = float.Epsilon;
                Debug.LogWarning("WARNING, WARNING, current move time is set to 0 or less!");
            }

            while (t < currentMoveTime)
            {
                transform.position = Vector3.Lerp(initialPosition, endPosition, t / currentMoveTime);
                t += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
        }
        else
        {
            yield return new WaitForSeconds(waitTime);
        }

        StartCoroutine("RandomMove");
    }
}
