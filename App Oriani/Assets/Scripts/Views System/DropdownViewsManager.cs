using UnityEngine;
using UnityEngine.UI;

public class DropdownViewsManager : MonoBehaviour
{
    [SerializeField]
    DropdownView defaultView = null;
    [SerializeField]
    Dropdown dropdown = null;

    void Start()
    {
        foreach (Transform child in transform)
        {
            DropdownView view = child.GetComponent<DropdownView>();
            if (view)
                view.Switch(view == defaultView);
        }
    }

    public void Reset()
    {
        Start();
    }

    public void OnDropdownValueChanged()
    {
        if (dropdown.value > -1 && dropdown.value < transform.childCount)
        {
            DropdownView view = transform.GetChild(dropdown.value).GetComponent<DropdownView>();
            if (view)
                view.Select();
        }
    }
}