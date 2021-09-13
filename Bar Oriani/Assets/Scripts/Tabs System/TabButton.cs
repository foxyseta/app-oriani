using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public CanvasGroup linkedTab;
    public static Dictionary<Transform, TabButton> CurrentTabButtons
    {
        get;
        private set;
    } = new Dictionary<Transform, TabButton>();

    public void OnClick()
    {
        if (CurrentTabButtons.ContainsKey(transform.parent) && CurrentTabButtons[transform.parent])
            CurrentTabButtons[transform.parent].Switch(false);
        Switch(true);
    }

    public virtual void Switch(bool mode)
    {
        GetComponent<Button>().interactable = !mode;
        SwitchLinkedTab(mode);
        if (mode)
        {
            CurrentTabButtons[transform.parent] = this;
            Loader[] loaders = linkedTab.GetComponents<Loader>();
            foreach (Loader l in loaders)
                l.Refresh();
        }
    }

    void SwitchLinkedTab(bool mode)
    {
        linkedTab.alpha = (linkedTab.interactable = linkedTab.blocksRaycasts = mode) ? 1 : 0;
    }
}
