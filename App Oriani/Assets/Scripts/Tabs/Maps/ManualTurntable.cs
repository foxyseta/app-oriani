using UnityEngine;

public class ManualTurntable : MonoBehaviour
{

    public Vector3 rotation = new Vector3(0f, 0f, 90f);
    public float duration = 0.250f;
    private float sinceStartedSpinning = -1f;

    public void Spin()
    {
        if (sinceStartedSpinning == -1f)
            sinceStartedSpinning = 0;
    }

    public bool IsSpinning()
    {
        return sinceStartedSpinning != -1f;
    }

    void Update()
    {
        if (sinceStartedSpinning != -1)
        {
            float tamperedDeltaTime = (sinceStartedSpinning += Time.deltaTime) > duration ?
                                       duration - sinceStartedSpinning + Time.deltaTime : Time.deltaTime;
            transform.Rotate(rotation * tamperedDeltaTime / duration);
            if (sinceStartedSpinning >= duration)
                sinceStartedSpinning = -1;
        }
    }
}
