using UnityEngine;
using UnityEngine.UI;

public class AddMenu : MonoBehaviour
{
    public InputField input;
    public string url, key;
    public DishesLoader loader;

    public void OnClick()
    {
        if (input.text != string.Empty)
        {
            loader.postRequest = true;
            string old_url = loader.url;
            loader.url = url;
            loader.formData = new JSONLoader<Dishes>.Field[] { new JSONLoader<Dishes>.Field(key, input.text) };
            input.text = string.Empty;
            loader.Refresh();
            loader.url = old_url;
            loader.formData = new JSONLoader<Dishes>.Field[] { };
        }
    }
}
