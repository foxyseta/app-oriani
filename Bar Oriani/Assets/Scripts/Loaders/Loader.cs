using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Loader : MonoBehaviour
{
    [Header("Web source")]
    public string url = "https://blogs.unity3d.com/feed/";
    [Header("Messages")]
    public Text log = null;
    public string loadingMessage = "Loading...";
    public string networkUnreachableMessage = "No network connection!";
    public string networkErrorMessage = "Network error!";
    public string httpErrorMessage = "Http error!";
    public string noDataMessage = "No data found.";
    [Header("Cache")]
    public string fileName;

    protected string path;

    abstract protected void UseContent();
    abstract protected void LoadCache();
    abstract protected void SaveCache();
    abstract protected IEnumerator GetContentRoutine();

    protected void Start()
    {
        path = Application.persistentDataPath + "/" + fileName;
        LoadCache();
        // Refresh();
    }

    public void Restart()
    {
        Start();
    }

    public virtual void Refresh()
    {
        // print("Refresh!");
        StartCoroutine(GetContentRoutine());
    }

    protected virtual void PrintOnLoaderLog(string msg)
    {
        if (log)
            log.text = msg;
    }

    protected virtual void OnLoading()
    {
        PrintOnLoaderLog(loadingMessage);
    }

    protected virtual void OnNetworkUnreachable()
    {
        PrintOnLoaderLog(networkUnreachableMessage);
    }

    protected virtual void OnNetworkError()
    {
        PrintOnLoaderLog(networkErrorMessage);
    }

    protected virtual void OnHttpError()
    {
        PrintOnLoaderLog(httpErrorMessage);
    }

    protected virtual void OnNoData()
    {
        PrintOnLoaderLog(noDataMessage);
    }

}