using UnityEngine;

public class URLButton : MonoBehaviour
{
    public string url = "https://unity.com",
                  androidUrl = null,
                  iOSurl = null;

    public void OnClick()
    {
        Application.OpenURL(ChooseUrl(
#if UNITY_ANDROID
            androidUrl
#elif UNITY_IOS
            iOSurl
#else
            url
#endif
        ));
    }

    private string ChooseUrl(string platformUrl)
    {
        return platformUrl != string.Empty ? platformUrl : url;
    }
}
