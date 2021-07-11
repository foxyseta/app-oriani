using UnityEngine;

public class TabsManager : MonoBehaviour
{
    public TabButton defaultTabButton = null;

    void Start()
    {
        foreach (Transform child in transform)
        {
            TabButton tabButton = child.GetComponent<TabButton>();
            if (tabButton)
                tabButton.Switch(tabButton == defaultTabButton);
        }
    }

    public void Reset()
    {
        Start();
    }
}
