using UnityEngine;
using UnityEngine.UI;

public class AddPlace : MonoBehaviour
{
    public InputField input;
    public string url, key;
    public TakeoutLoader loader;

    public void OnClick()
    {
        if (input.text != string.Empty)
        {
            loader.postRequest = true;
            string old_url = loader.url;
            loader.url = url;
            loader.formData = new JSONLoader<Takeout>.Field[] { new JSONLoader<Takeout>.Field(key, input.text) };
            input.text = string.Empty;
            loader.Refresh();
            loader.url = old_url;
            loader.formData = new JSONLoader<Takeout>.Field[] { };
        }
    }
}
