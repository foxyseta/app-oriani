using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoader : Loader
{
    [Header("UI")]
    public Image image;

    AspectRatioFitter arf;

    new void Start()
    {
        base.Start();
        arf = GetComponent<AspectRatioFitter>();
    }

    Sprite ToSprite(Texture2D t)
    {
        return Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);
    }

    protected override IEnumerator GetContentRoutine()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnNetworkUnreachable();
            yield break;
        }
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        OnLoading();
        yield return www.SendWebRequest();
        if (www.isNetworkError)
            OnNetworkError();
        else if (www.isHttpError)
            OnHttpError();
        else
        {
            Texture2D t = ((DownloadHandlerTexture)www.downloadHandler).texture;
            image.sprite = ToSprite(t);
            SaveCache();
            UseContent();
        };
    }

    protected override void LoadCache()
    {
        if (File.Exists(path))
        {
            Texture2D t = new Texture2D(1, 1);
            if (t.LoadImage(File.ReadAllBytes(path)))
            {
                image.sprite = ToSprite(t);
                UpdateAspectRatio();
                // print("Cache loaded!");
            }
        }
    }

    protected override void SaveCache()
    {
        if (path != null)
            File.WriteAllBytes(path, image.sprite.texture.EncodeToPNG());
        // print("Cache saved!");
    }

    protected override void UseContent()
    {
        UpdateAspectRatio();
        PrintOnLoaderLog(string.Empty);
    }

    void UpdateAspectRatio()
    {
        if (arf)
            arf.aspectRatio = (float)image.sprite.rect.width / image.sprite.rect.height;
    }
}