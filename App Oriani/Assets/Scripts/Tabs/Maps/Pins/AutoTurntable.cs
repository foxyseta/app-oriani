using UnityEngine;

public class AutoTurntable : MonoBehaviour
{

    public Vector3 rotationPerSecond = new Vector3(0f, 0f, 90f);

    void Update()
    {
        transform.Rotate(rotationPerSecond * Time.deltaTime);
    }
}
