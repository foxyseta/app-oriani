using UnityEngine;

public class URLButton : MonoBehaviour
{
    public string url = "https://unity.com";

    public void OnClick()
    {
        Application.OpenURL(url);
    }

}
