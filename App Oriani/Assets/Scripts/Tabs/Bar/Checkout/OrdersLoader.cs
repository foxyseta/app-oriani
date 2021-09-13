using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Order
{
    [System.Serializable]
    public class OrderedItem
    {
        public int order, quantity;
        public float totalPrice;
        public string dish;
    }

    public int ID, lockable;
    public string date, customer_notes, barman_notes, state;
    public OrderedItem[] orderedItems;
}

class OrdersLoader : JSONLoader<Order[]>
{
    [Header("UI")]
    [SerializeField]
    Transform orderWidgetsContainer = null;
    [SerializeField]
    OrderWidget orderWidgetPrefab = null;
    [SerializeField]
    GameObject lockScreen = null;
    [SerializeField]
    InputField lockNotes = null;
    [SerializeField]
    string lockUrl = null,
           lockNotesPostKey = "notes";

    public void LockOrder()
    {
        postRequest = true;
        string old_url = url;
        url = lockUrl;
        Field[] old_formData = formData;
        formData = string.IsNullOrWhiteSpace(lockNotes.text) ?
            new Field[] { } : new Field[] { new Field(lockNotesPostKey, lockNotes.text) };
        Refresh();
        url = old_url;
        formData = old_formData;
        lockNotes.text = string.Empty;
    }

    protected override void UseContent()
    {
        if (content.data.Length == 0)
            OnNoData();
        EmptyContainer();
        RefillContainer();
    }

    void EmptyContainer()
    {
        foreach (Transform child in orderWidgetsContainer)
            Destroy(child.gameObject);
    }

    void RefillContainer()
    {
        for (int i = 0; i < content.data.Length; ++i)
        {
            OrderWidget w = Instantiate(orderWidgetPrefab, orderWidgetsContainer);
            w.Order = content.data[i];
            w.lockScreen = lockScreen;
            w.lockNotes = lockNotes;
        }
    }
}