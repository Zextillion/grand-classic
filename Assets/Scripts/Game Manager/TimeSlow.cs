using UnityEngine;
using System.Collections;

public class TimeSlow : MonoBehaviour
{
    public void TimeSlowdown(float pauseDelay)
    {
        StartCoroutine("Pause", pauseDelay);
    }

    // Time freeze
    private IEnumerator Pause(float pauseDelay)
    {
        Time.timeScale = 0.1f;
        float waitTime = Time.realtimeSinceStartup + pauseDelay;
        yield return new WaitWhile(() => Time.realtimeSinceStartup < waitTime);
        Time.timeScale = 1.0f;
    }
}
