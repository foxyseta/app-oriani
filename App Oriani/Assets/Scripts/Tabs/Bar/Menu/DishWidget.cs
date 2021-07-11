using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class DishWidget : MonoBehaviour
{

    [Header("UI Elements")]
    public ImageLoader picture = null;
    [SerializeField]
    string pictureUrlFormat = null;
    [SerializeField]
    Text quantity = null, price = null;
    [SerializeField]
    string quantityFormat = "× {0}";
    [SerializeField]
    string culture = "en-US";
    [SerializeField]
    Image addRemove = null;
    [SerializeField]
    Sprite addSprite = null, removeSprite = null;
    [SerializeField]
    OrderedDishWidget orderedDishPrefab = null;

    public DishScreen popUp = null;
    public Transform orderedDishesContainer = null;
    public Text orderedDishesTotal = null;
    public GameObject dishErrorScreen = null;

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
            // quantity
            quantity.text = string.Format(quantityFormat, _dish.quantity_in_stock);
            // price
            price.text = _dish.price.ToString("C", CultureInfo.CreateSpecificCulture(culture));
            // button
            UpdateButtons();
            // ordered dish widget
            if (OrderedDishWidget.Widgets.ContainsKey(_dish.name))
                OrderedDishWidget.Widgets[_dish.name].Widget = this;
        }
    }

    public void UpdateButtons()
    {
        addRemove.sprite = OrderedDishWidget.Widgets.ContainsKey(_dish.name) ?
            removeSprite : addSprite;
    }

    public void OnClick()
    {
        popUp.Fill(this);
        popUp.gameObject.SetActive(true);
    }

    public void OnAddRemoveClick()
    {
        if (OrderedDishWidget.Widgets.ContainsKey(_dish.name))
        {
            // Remove ordered dish
            OrderedDishWidget.Widgets[_dish.name].Destroy();
            addRemove.sprite = addSprite;
        }
        else
        {
            if (_dish.quantity_in_stock > 0)
            {
                // Add ordered dish
                OrderedDishWidget w = Instantiate(orderedDishPrefab, orderedDishesContainer);
                w.Widget = this;
                addRemove.sprite = removeSprite;
            }
            else
                dishErrorScreen.SetActive(true);
        }
    }

}