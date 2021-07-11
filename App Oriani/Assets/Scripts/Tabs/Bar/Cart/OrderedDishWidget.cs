using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OrderedDishWidget : MonoBehaviour
{
    public static Dictionary<string, OrderedDishWidget> Widgets
    { get; private set; } = new Dictionary<string, OrderedDishWidget>();
    public DishWidget Widget
    {
        get => _widget;
        set
        {
            if (_widget)
                Widgets.Remove(_widget.Dish.name);
            _widget = value;
            Widgets.Add(_widget.Dish.name, this);
            SetImage(_widget.picture.image);
            title.text = _widget.Dish.name;
            UpdatePrices();
        }
    }
    public float TotalPrice { get; private set; }
    public int Quantity
    {
        get => int.Parse(quantityInput.text);
    }
    public string Name
    {
        get => title.text;
    }

    [Header("UI")]
    [SerializeField]
    Image picture = null;
    [SerializeField]
    Text title = null;
    [SerializeField]
    InputField quantityInput = null;
    [SerializeField]
    int defaultQuantity = 1;
    [SerializeField]
    Text pricing = null;
    [SerializeField]
    string cultureName = "en-US";
    [SerializeField]
    string quantityFormat = "+##;-##",
           pricingFormat = " × {0} = {1}",
           totalPriceFormat = "Total: {0}";


    DishWidget _widget = default;

    void SetImage(Image i)
    {
        picture.sprite = i.sprite;
        AspectRatioFitter arf = picture.GetComponent<AspectRatioFitter>();
        if (arf)
            arf.aspectRatio = (float)picture.sprite.rect.width / picture.sprite.rect.height;
    }

    public void UpdatePrices()
    {
        if (!int.TryParse(quantityInput.text, out int quantity) || quantity == 0)
            quantity = defaultQuantity;
        quantityInput.text = quantity.ToString(quantityFormat);
        pricing.text = string.Format(pricingFormat,
                                     _widget.Dish.price.ToString("C",
                                        CultureInfo
                                        .CreateSpecificCulture(cultureName)),
                                     (TotalPrice =
                                     quantity * _widget.Dish.price)
                                     .ToString("C",
                                        CultureInfo
                                        .CreateSpecificCulture(cultureName)));
        UpdateTotalPrice();
    }

    public void UpdateTotalPrice()
    {
        float totalPrice = 0f;
        foreach (Transform child in _widget.orderedDishesContainer.transform)
        {
            OrderedDishWidget odw = child.GetComponent<OrderedDishWidget>();
            if (odw)
                totalPrice += odw.TotalPrice;
        }
        _widget.orderedDishesTotal.text =
            string.Format(totalPriceFormat, totalPrice.ToString("C",
                CultureInfo.CreateSpecificCulture(cultureName)));
    }

    public void Destroy()
    {
        // Menu
        Widgets.Remove(_widget.Dish.name);
        _widget.UpdateButtons();
        // Cart
        TotalPrice = 0f;
        UpdateTotalPrice();
        Destroy(gameObject);
    }
}