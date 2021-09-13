using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OrderWidget : MonoBehaviour
{
    public OrderEdit orderEdit = null;
    public OrdersPrinter ordersPrinter = null;

    [Header("UI")]
    [SerializeField]
    Sprite notPrintedYetSprite = null;
    [SerializeField]
    Sprite printedAlreadySprite = null;
    [SerializeField]
    Text timestamp = null;
    [SerializeField]
    string timestampFormat = "<b>{0}</b> ~ {1}";
    [SerializeField]
    Text user = null;
    [SerializeField]
    Text articles = null;
    [SerializeField]
    string articleStringFormat = "{0} × {1}\n",
           totalString = "<b>Total</b>";
    [SerializeField]
    Text prices = null;
    [SerializeField]
    string priceStringFormat = "{0}\n",
           totalPriceStringFormat = "<b>{0}</b>",
           cultureName = "en-US";
    [SerializeField]
    Text customer_notes = null, barman_notes = null, state = null;
    [SerializeField]
    Image stateBackground = null;
    [SerializeField]
    string[] stateValues = { "Waiting", "Accepted", "Rejected" },
             stateMessages = { "Waiting", "Accepted", "Rejected" };
    [SerializeField]
    Color[] stateColors = { new Color(33, 87, 97),
                            new Color(81, 248, 32),
                            new Color(248, 152, 132) };
    [SerializeField]
    Sprite[] stateSprites = null;
    [SerializeField]
    Image image = null;

    public Orders.Order Order
    {
        get => _order;
        set
        {
            _order = value;
            gameObject.name = _order.ID.ToString();
            timestamp.text = string.Format(timestampFormat,
                _order.time, _order.date);
            user.text = _order.user;
            articles.text = GetArticlesString();
            prices.text = GetPricesString();
            customer_notes.text = _order.customer_notes;
            barman_notes.text = _order.barman_notes;
            StateUpdate();
        }
    }

    Orders.Order _order = null;

    string GetArticlesString()
    {
        string res = string.Empty;
        foreach (Orders.Order.OrderedItem o in _order.orderedItems)
            res += string.Format(articleStringFormat, o.quantity, o.dish);
        return res + totalString;
    }

    string GetPricesString()
    {
        string res = string.Empty;
        float totalPrice = 0f;
        foreach (Orders.Order.OrderedItem o in _order.orderedItems)
        {
            res += string.Format(priceStringFormat, o.totalPrice.ToString("C",
                CultureInfo.CreateSpecificCulture(cultureName)));
            totalPrice += o.totalPrice;
        }
        return res + string.Format(totalPriceStringFormat, totalPrice.ToString("C",
            CultureInfo.CreateSpecificCulture(cultureName)));
    }

    void StateUpdate()
    {
        int i = Array.IndexOf(stateValues, _order.state);
        if (i == -1)
            i = 0;
        state.text = stateMessages[i];
        state.color = stateColors[i];
        stateBackground.sprite = stateSprites[i];
    }

    public void OnClick()
    {
        orderEdit.SetUpEditScreen(_order);
    }

    public void OnPrintClick()
    {
        ordersPrinter.OnClick(this);
        image.sprite = printedAlreadySprite;
    }

    public void SetPrinterSprite(bool printedAlready)
    {
        image.sprite = printedAlready ? printedAlreadySprite : notPrintedYetSprite;
    }
}
