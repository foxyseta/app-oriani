using System.Collections;
using UnityEngine;

public class TabsDelegateLoader : Loader
{
    [Header("Delegate (ignore previous fields)")]
    [SerializeField]
    Transform tabButtonsContainer = null;

    public override void Refresh()
    {
        TabButton.CurrentTabButtons[tabButtonsContainer].RefreshLoaders();
    }

    protected override IEnumerator GetContentRoutine()
    {
        yield break;
    }

    protected override void LoadCache()
    {
    }

    protected override void SaveCache()
    {
    }

    protected override void UseContent()
    {
    }
}