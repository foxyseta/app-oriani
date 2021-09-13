using UnityEngine;
using UnityEngine.UI;

public class DeletePlace : MonoBehaviour
{
    public Text placeholder;
    public string url, nameKey;
    public TakeoutLoader loader;

    public void OnClick()
    {
        loader.postRequest = true;
        string old_url = loader.url;
        loader.url = url;
        loader.formData = new JSONLoader<Takeout>.Field[] {
            new JSONLoader<Takeout>.Field(nameKey, placeholder.text)
        };
        loader.Refresh();
        loader.url = old_url;
        loader.formData = new JSONLoader<Takeout>.Field[] { };
    }
}
