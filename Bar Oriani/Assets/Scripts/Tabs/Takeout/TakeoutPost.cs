using UnityEngine;
using UnityEngine.UI;

public class TakeoutPost : MonoBehaviour
{
    public InputField user_name, password, orders_time_limit, delivery_time;
    public string urlEdit, urlDelete, user_name_key, new_user_name_key,
        password_key, orders_time_limit_key, delivery_time_key, place_name_key;
    public TakeoutLoader loader;

    Takeout.Takeout_information _takeout_information = null;

    public Takeout.Takeout_information Takeout_Information
    {
        get => _takeout_information;
        set
        {
            _takeout_information = value;
            user_name.text = value.name;
            password.text = value.password;
            orders_time_limit.text = value.orders_time_limit;
            delivery_time.text = value.delivery_time;
        }
    }

    public void Edit()
    {
        // Missing fields
        if (user_name.text == string.Empty || password.text == string.Empty ||
            orders_time_limit.text == string.Empty || delivery_time.text == string.Empty)
        {
            Takeout_Information = Takeout_Information;
            return;
        }
        // No actual edits
        if (user_name.text == Takeout_Information.name &&
            password.text == Takeout_Information.password &&
            orders_time_limit.text == Takeout_Information.orders_time_limit &&
            delivery_time.text == Takeout_Information.delivery_time)
            return;
        loader.postRequest = true;
        string old_url = loader.url;
        loader.url = urlEdit;
        loader.formData = new JSONLoader<Takeout>.Field[] {
            new JSONLoader<Takeout>.Field(user_name_key, Takeout_Information.name),
            new JSONLoader<Takeout>.Field(new_user_name_key, user_name.text),
            new JSONLoader<Takeout>.Field(password_key, password.text),
            new JSONLoader<Takeout>.Field(orders_time_limit_key, orders_time_limit.text),
            new JSONLoader<Takeout>.Field(delivery_time_key, delivery_time.text),
            new JSONLoader<Takeout>.Field(place_name_key, Takeout_Information.place_name)
        };
        loader.Refresh();
        loader.url = old_url;
        loader.formData = new JSONLoader<Takeout>.Field[] { };
    }

    public void Delete()
    {
        loader.postRequest = true;
        string old_url = loader.url;
        loader.url = urlDelete;
        loader.formData = new JSONLoader<Takeout>.Field[] {
            new JSONLoader<Takeout>.Field(user_name_key, Takeout_Information.name)
        };
        loader.Refresh();
        loader.url = old_url;
        loader.formData = new JSONLoader<Takeout>.Field[] { };
    }
}