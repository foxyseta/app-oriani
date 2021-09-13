using UnityEngine;
using UnityEngine.UI;

public class DeleteMenu : MonoBehaviour
{
    public Text placeholder;
    public string url, nameKey;
    public DishesLoader loader;

    public void OnClick()
    {
        loader.postRequest = true;
        string old_url = loader.url;
        loader.url = url;
        loader.formData = new JSONLoader<Dishes>.Field[] {
            new JSONLoader<Dishes>.Field(nameKey, placeholder.text)
        };
        loader.Refresh();
        loader.url = old_url;
        loader.formData = new JSONLoader<Dishes>.Field[] { };
    }
}
