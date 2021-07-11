using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OrderWidget : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    Text date = null, articles = null;
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
    Color[] stateColors = { new Color(33, 87, 97), new Color(81, 248, 32), new Color(248, 152, 132) };
    [SerializeField]
    Sprite[] stateSprites = null;
    [SerializeField]
    GameObject lockButton = null;

    public GameObject lockScreen = null;
    public InputField lockNotes = null;

    void SetLockable(bool value)
    {
        if (lockButton)
            lockButton.SetActive(value);
    }

    public Order Order
    {
        get => _order;
        set
        {
            _order = value;
            gameObject.name = _order.ID.ToString();
            date.text = _order.date;
            articles.text = GetArticlesString();
            prices.text = GetPricesString();
            if (_order.customer_notes != null && _order.customer_notes != string.Empty)
                customer_notes.text = _order.customer_notes;
            else
                customer_notes.gameObject.SetActive(false);
            if (_order.barman_notes != null && _order.barman_notes != string.Empty)
                barman_notes.text = _order.barman_notes;
            else
                barman_notes.gameObject.SetActive(false);
            StateUpdate();
            SetLockable(value.lockable != 0);
        }
    }

    Order _order = null;

    string GetArticlesString()
    {
        string res = string.Empty;
        foreach (Order.OrderedItem o in _order.orderedItems)
            res += string.Format(articleStringFormat, o.quantity, o.dish);
        return res + totalString;
    }

    string GetPricesString()
    {
        string res = string.Empty;
        float totalPrice = 0f;
        foreach (Order.OrderedItem o in _order.orderedItems)
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

    public void OnLockScreenClick()
    {
        lockScreen.SetActive(true);
        lockNotes.text = _order.customer_notes;
    }

}