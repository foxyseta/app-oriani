using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Pin : MonoBehaviour
{
    protected abstract void Interact();

    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject(
#if UNITY_ANDROID || UNITY_IOS
            Input.GetTouch(0).fingerId
#endif
        ))
            Interact();
    }
}