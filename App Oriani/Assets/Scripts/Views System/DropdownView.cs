using System.Collections.Generic;
using UnityEngine;

public class DropdownView : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private static Dictionary<Transform, DropdownView> currentViews = new Dictionary<Transform, DropdownView>();

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Select()
    {
        if (currentViews.ContainsKey(transform.parent) && currentViews[transform.parent])
            currentViews[transform.parent].Switch(false);
        Switch(true);
    }

    public virtual void Switch(bool mode)
    {
        SwitchCanvasGroup(mode);
        if (mode)
        {
            currentViews[transform.parent] = this;
            Loader[] loaders = canvasGroup.GetComponents<Loader>();
            foreach (Loader l in loaders)
                l.Refresh();
        }
    }

    void SwitchCanvasGroup(bool mode)
    {
        if (canvasGroup)
            canvasGroup.alpha = (canvasGroup.interactable = canvasGroup.blocksRaycasts = mode) ? 1 : 0;
    }
}