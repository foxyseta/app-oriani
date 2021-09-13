using UnityEngine;

public class LoaderUrlSwapper : MonoBehaviour
{
    [SerializeField]
    Loader loader = null;
    [SerializeField]
    string newUrl = null;

    public void OnClick(bool IsUrlTemp = true)
    {
        string old_url = loader.url;
        loader.url = newUrl;
        loader.Refresh();
        if (IsUrlTemp)
            loader.url = old_url;
    }
}