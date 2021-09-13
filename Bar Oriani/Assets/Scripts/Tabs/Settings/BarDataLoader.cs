using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BarData
{
    [System.Serializable]
    public class TimeInterval
    {
        public string from, until;
    }

    [System.Serializable]
    public class Lunch
    {
        public TimeInterval orders;
        public TimeInterval delivery;
    }

    public string barName;
    public Lunch lunch;
}

public class BarDataLoader : JSONLoader<BarData>
{
    [Header("Bar Data")]
    [SerializeField]
    InputField barNameText = default;
    [SerializeField]
    string barNamePostKey = default;
    [SerializeField]
    InputField lunchOrdersFromText = default;
    [SerializeField]
    string lunchOrdersFromPostKey = default;
    [SerializeField]
    InputField lunchOrdersUntilText = default;
    [SerializeField]
    string lunchOrdersUntilPostKey = default;
    [SerializeField]
    InputField lunchDeliveryFromText = default;
    [SerializeField]
    string lunchDeliveryFromPostKey = default;
    [SerializeField]
    InputField lunchDeliveryUntilText = default;
    [SerializeField]
    string lunchDeliveryFromUntilKey = default;

    override protected void UseContent()
    {
        barNameText.text = content.data.barName;
        // orders
        lunchOrdersFromText.text = content.data.lunch.orders.from;
        lunchOrdersUntilText.text = content.data.lunch.orders.until;
        // deliveries
        lunchDeliveryFromText.text = content.data.lunch.delivery.from;
        lunchDeliveryUntilText.text = content.data.lunch.delivery.until;
    }

    public void OnInputFieldEndEdit()
    {
        postRequest = true;
        formData = new Field[] {
            new Field(barNamePostKey, barNameText.text),
            new Field(lunchOrdersFromPostKey, lunchOrdersFromText.text),
            new Field(lunchOrdersUntilPostKey, lunchOrdersUntilText.text),
            new Field(lunchDeliveryFromPostKey, lunchDeliveryFromText.text),
            new Field(lunchDeliveryFromUntilKey, lunchDeliveryUntilText.text)
        };
        Refresh();
    }
}