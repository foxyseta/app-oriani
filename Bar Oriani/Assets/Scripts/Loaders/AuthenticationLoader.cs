using UnityEngine;
using UnityEngine.UI;

public class AuthenticationLoader : JSONLoader<Empty>
{
    [Header("UI")]
    public GameObject authenticationScreen;
    public InputField key;
    public Button refresh;
    [Header("Messages")]
    public string typeKeyMessage = "Please type your key here first:";
    public string wrongKeyMessage = "The key was incorrect. Try again:";

    new void Start()
    {
        if (PlayerPrefs.HasKey(keyPlayerPreference))
        {
            key.text = PlayerPrefs.GetString(keyPlayerPreference);
            postRequest = true;
        }
        Refresh();
    }

    override protected void LoadCache()
    {
        //no cache
    }

    override protected void SaveCache()
    {
        //no cache
    }

    public void OnInputFieldEndEdit()
    {
        postRequest = true;
        PlayerPrefs.SetString(keyPlayerPreference, key.text);
        Refresh();
    }

    override protected void OnContentUpdate()
    {
        PrintOnLoaderLog(typeKeyMessage);
        UseContent();
    }

    override protected void UseContent()
    {
        if (content.authentication)
            authenticationScreen.SetActive(false);
        else if (key.text != string.Empty)
        {
            PrintOnLoaderLog(wrongKeyMessage);
            InputMode(true);
        }
        else
            InputMode(true);
    }

    override protected void OnNetworkUnreachable()
    {
        PrintOnLoaderLog(networkUnreachableMessage);
        InputMode(false);
    }

    override protected void OnNetworkError()
    {
        PrintOnLoaderLog(networkErrorMessage);
        InputMode(false);
    }

    override protected void OnHttpError()
    {
        PrintOnLoaderLog(httpErrorMessage);
        InputMode(false);
    }

    private void InputMode(bool b)
    {
        authenticationScreen.SetActive(true);
        key.gameObject.SetActive(b);
        refresh.gameObject.SetActive(!b);
    }
}