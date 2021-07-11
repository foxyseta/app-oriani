using System;
using UnityEngine;
using UnityEngine.UI;

public class DishesResetter : MonoBehaviour
{
    [SerializeField]
    string menuNamesPlayerPrefsKey = "DishesResetter_MenuNames";
    [SerializeField]
    InputField input = null;
    [SerializeField]
    char separator = ',';
    [SerializeField]
    DishesLoader loader = null;
    [SerializeField]
    string noMenuNamesMessageFormat = "Please enter menu names separated by '{0}'.",
           url = null,
           menuNamesPostKey = "menu_names";


    void Start()
    {
        input.text = PlayerPrefs.HasKey(menuNamesPlayerPrefsKey) ?
            PlayerPrefs.GetString(menuNamesPlayerPrefsKey) : string.Empty;
    }

    public void Set()
    {
        PlayerPrefs.SetString(menuNamesPlayerPrefsKey, input.text);
    }

    public void Confirm()
    {
        string[] menuNames = input.text.Split(separator);
        if (menuNames != null && menuNames.Length > 0 && Array.IndexOf(menuNames, string.Empty) == -1)
        {
            loader.postRequest = true;
            string old_url = loader.url;
            loader.url = url;
            loader.formData = ToFormData(menuNames);
            loader.Refresh();
            loader.url = old_url;
            loader.formData = new JSONLoader<Dishes>.Field[] { };
        }
        else
            loader.log.text = string.Format(noMenuNamesMessageFormat, separator);
    }

    DishesLoader.Field[] ToFormData(string[] menuNames)
    {
        DishesLoader.Field[] res = new DishesLoader.Field[menuNames.Length];
        for (int i = 0; i < menuNames.Length; ++i)
            res[i] = new DishesLoader.Field(menuNamesPostKey + '[' + i + ']', menuNames[i].Trim());
        return res;
    }
}