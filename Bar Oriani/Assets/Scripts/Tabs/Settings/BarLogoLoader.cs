using UnityEngine;
using UnityEngine.UI;

public class BarLogoLoader : JSONLoader<Empty>
{
    [Header("UI")]
    public ImageLoader downloader;

    override public void Refresh()
    {
        postRequest = true;
        formFiles = new File[] { new File("barLogo", GetComponent<Image>().sprite.texture.EncodeToPNG(), "bar_logo.png", "image/png") };
        base.Refresh();
    }

    override protected void UseContent()
    {
        downloader.Refresh();
    }

    override protected void LoadCache()
    {
        //no cache
    }

    override protected void SaveCache()
    {
        //no cache
    }
}
