using UnityEngine;
using UnityEngine.UI;

public class EditPlace : MonoBehaviour
{
    public InputField inputField;
    public string url, nameKey, newNameKey;
    public TakeoutLoader loader;

    public void OnClick()
    {
        if (inputField)
        {
            Text t = inputField.placeholder.GetComponent<Text>();
            if (inputField.text == string.Empty)
                inputField.text = t.text;
            if (t && t.text != inputField.text)
            {
                loader.postRequest = true;
                string old_url = loader.url;
                loader.url = url;
                loader.formData = new JSONLoader<Takeout>.Field[] {
                    new JSONLoader<Takeout>.Field(nameKey, t.text),
                    new JSONLoader<Takeout>.Field(newNameKey, inputField.text)
                };
                loader.Refresh();
                loader.url = old_url;
                loader.formData = new JSONLoader<Takeout>.Field[] { };
            }
        }
    }
}
