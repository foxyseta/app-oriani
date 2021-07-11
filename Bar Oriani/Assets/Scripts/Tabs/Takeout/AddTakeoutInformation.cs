using UnityEngine;
using UnityEngine.UI;

public class AddTakeoutInformation : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    TakeoutLoader loader = null;
    [SerializeField]
    string url = null, user_name_key = null, password_key = null, orders_time_limit_key = null,
           delivery_time_key = null, place_name_key = null;
    [Header("UI")]
    [SerializeField]
    InputField user_name = null;
    [SerializeField]
    InputField password = null, orders_time_limit = null, delivery_time = null;
    [SerializeField]
    Dropdown place_name = null;
    [SerializeField]
    string emptyFieldsMessage = "Please fill every field first.";

    public void Add()
    {
        if (user_name.text != string.Empty && password.text != string.Empty &&
            orders_time_limit.text != string.Empty && delivery_time.text != string.Empty)
        {
            loader.postRequest = true;
            string old_url = loader.url;
            loader.url = url;
            loader.formData = new JSONLoader<Takeout>.Field[] {
                new JSONLoader<Takeout>.Field(user_name_key, user_name.text),
                new JSONLoader<Takeout>.Field(password_key, password.text),
                new JSONLoader<Takeout>.Field(orders_time_limit_key, orders_time_limit.text),
                new JSONLoader<Takeout>.Field(delivery_time_key, delivery_time.text),
                new JSONLoader<Takeout>.Field(place_name_key, place_name.options[place_name.value].text)
            };
            loader.Refresh();
            loader.url = old_url;
            loader.formData = new JSONLoader<Takeout>.Field[] { };
            Close();
        }
        else
            loader.log.text = emptyFieldsMessage;
    }

    public void Close()
    {
        user_name.text = password.text = orders_time_limit.text =
            delivery_time.text = string.Empty;
        place_name.value = 0;
        gameObject.SetActive(false);
    }
}
