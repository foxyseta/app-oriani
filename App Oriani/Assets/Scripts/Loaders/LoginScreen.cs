using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    public string changePasswordUrl, newPasswordPostKey;
    [Header("Player Preferences")]
    public string usernamePlayerPref = "JSONLoader_username";
    public string passwordPlayerPref = "JSONLoader_password";
    [Header("UI")]
    [SerializeField]
    InputField username = null;
    [SerializeField]
    InputField password = null;
    [SerializeField]
    InputField changePassword = null;

    public delegate void Del();

    Del afterLogin = null;

    public void OpenLoginScreen(Del refresh)
    {
        afterLogin = refresh;
        gameObject.SetActive(true);
        username.text = PlayerPrefs.HasKey(usernamePlayerPref) ?
            PlayerPrefs.GetString(usernamePlayerPref) : string.Empty;
        password.text = PlayerPrefs.HasKey(passwordPlayerPref) ?
            PlayerPrefs.GetString(passwordPlayerPref) : string.Empty;
    }

    public void CloseLoginScreen()
    {
        PlayerPrefs.SetString(usernamePlayerPref, username.text);
        PlayerPrefs.SetString(passwordPlayerPref, password.text);
        afterLogin();
        gameObject.SetActive(false);
    }


    public void ChangePassword(BarDataLoader loader)
    {
        if (changePassword.text == PlayerPrefs.GetString(passwordPlayerPref))
            changePassword.text = string.Empty;
        if (changePassword.text != string.Empty)
        {
            loader.postRequest = true;
            string old_url = loader.url;
            loader.url = changePasswordUrl;
            loader.formData =
                new JSONLoader<BarData>.Field[] {
                    new JSONLoader<BarData>.Field(newPasswordPostKey,
                                                 changePassword.text)
                };
            loader.Refresh();
            loader.url = old_url;
            loader.formData = new JSONLoader<BarData>.Field[] { };
            // PlayerPrefs.SetString(passwordPlayerPref, changePassword.text);
            changePassword.text = string.Empty;
        }
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey(usernamePlayerPref);
        PlayerPrefs.DeleteKey(passwordPlayerPref);
    }
}