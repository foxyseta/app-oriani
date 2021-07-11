using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class DishWidget : MonoBehaviour
{
    public DishesLoader loader;
    public DishPopUp popUp = null;

    [Header("UI Elements")]
    [SerializeField]
    ImageLoader picture = null;
    [SerializeField]
    string pictureUrlFormat = null;
    [SerializeField]
    Text price = null;
    [SerializeField]
    string priceFormat = "{0} × {1}", culture = "en-US";
    [Header("Data")]
    [SerializeField]
    string deleteUrl = null;
    [SerializeField]
    string nameKey = null;

    Dishes.Dish _dish = null;

    public Dishes.Dish Dish
    {
        get => _dish;
        set
        {
            _dish = value;
            // picture
            picture.url = string.Format(pictureUrlFormat, _dish.name);
            picture.fileName = _dish.name + ".png";
            picture.Restart();
            picture.Refresh();
            // price
            price.text = string.Format(priceFormat, _dish.quantity_in_stock.ToString(), _dish.price.ToString("C", CultureInfo.CreateSpecificCulture(culture)));
        }
    }

    public void Edit()
    {
        popUp.EditDishMode(_dish, picture.image.sprite);
    }

    public void Delete()
    {
        loader.postRequest = true;
        string old_url = loader.url;
        loader.url = deleteUrl;
        loader.formData = new JSONLoader<Dishes>.Field[] {
            new JSONLoader<Dishes>.Field(nameKey, _dish.name)
        };
        loader.Refresh();
        loader.url = old_url;
        loader.formData = new JSONLoader<Dishes>.Field[] { };
    }

}