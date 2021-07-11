using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    [SerializeField]
    CanvasGroup linkedTab = null;
    public static Dictionary<Transform, TabButton> CurrentTabButtons { get; private set; } = new Dictionary<Transform, TabButton>();

    public void OnClick(bool refreshLoaders = true)
    {
        if (CurrentTabButtons.ContainsKey(transform.parent) && CurrentTabButtons[transform.parent])
            CurrentTabButtons[transform.parent].Switch(false);
        Switch(true, refreshLoaders);
    }

    public virtual void Switch(bool mode, bool refreshLoaders = true)
    {
        GetComponent<Button>().interactable = !mode;
        SwitchLinkedTab(mode);
        if (mode)
        {
            CurrentTabButtons[transform.parent] = this;
            if (refreshLoaders)
                RefreshLoaders();
        }
    }

    public int RefreshLoaders()
    {
        Loader[] loaders = linkedTab.GetComponents<Loader>();
        foreach (Loader l in loaders)
            l.Refresh();
        return loaders.Length;
    }

    void SwitchLinkedTab(bool mode)
    {
        if (linkedTab)
            linkedTab.alpha = (linkedTab.interactable = linkedTab.blocksRaycasts = mode) ? 1 : 0;
    }
}