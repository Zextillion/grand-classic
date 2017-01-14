using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake current;
    // Transform of the camera to shake. Grabs the gameObject's transform if null.
    public Transform camTransform;

    Vector3 originalPos;

    private float shakeDurationUpdate;
    private float shakeAmountUpdate;
    private float decreaseFactorUpdate;

    void Awake()
    {
        current = this;
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        Shake(shakeDurationUpdate, shakeAmountUpdate, decreaseFactorUpdate);
    }

    public void Shake(float shakeDuration, float shakeAmount, float decreaseFactor)
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;

            shakeDurationUpdate = shakeDuration;
            shakeAmountUpdate = shakeAmount;
            decreaseFactorUpdate = decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }  
    }
}