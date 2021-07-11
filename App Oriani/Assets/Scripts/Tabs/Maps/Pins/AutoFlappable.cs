using UnityEngine;

public class AutoFlappable : MonoBehaviour
{

    public AnimationCurve curve;

    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, curve.Evaluate((Time.time % curve.length)), transform.localScale.z);
    }
}
