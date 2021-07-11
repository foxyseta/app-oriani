using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderSubmitter : MonoBehaviour
{
    [SerializeField]
    string url = null,
           notesPostKey = "notes",
           orderedItemsPostKey = "orderedItems",
           quantityPostKey = "quantity",
           dishNamePostKey = "dishName";
    [Header("UI")]
    [SerializeField]
    Transform orderedItemsContainer = null;
    [SerializeField]
    InputField notes = null;
    [SerializeField]
    GameObject submitScreen = null, noProductsScreen = null;
    [SerializeField]
    OrdersLoader loader = null;

    public void OnOpenScreen()
    {
        (orderedItemsContainer.childCount > 0 ?
            submitScreen : noProductsScreen).SetActive(true);
    }

    public void OnSubmit()
    {
        loader.postRequest = true;
        string old_url = loader.url;
        loader.url = url;
        loader.formData = GetFormData();
        loader.Refresh();
        // After web request
        loader.url = old_url;
        loader.formData = new OrdersLoader.Field[] { };
        foreach (Transform child in orderedItemsContainer)
        {
            OrderedDishWidget w = child.GetComponent<OrderedDishWidget>();
            if (w)
                w.Destroy();
        }
        notes.text = string.Empty;
    }

    OrdersLoader.Field[] GetFormData()
    {
        List<OrdersLoader.Field> res = new List<OrdersLoader.Field>();
        int i = 0;
        foreach (Transform child in orderedItemsContainer)
        {
            OrderedDishWidget w = child.GetComponent<OrderedDishWidget>();
            if (w)
            {
                // Quantity
                res.Add(new OrdersLoader.Field(
                    orderedItemsPostKey + '[' + i + "][" + quantityPostKey + ']',
                    w.Quantity.ToString()));
                // Dish name
                res.Add(new OrdersLoader.Field(
                    orderedItemsPostKey + '[' + i + "][" + dishNamePostKey + ']',
                    w.Name));
                ++i;
            }
        }
        if (notes.text != string.Empty)
            res.Add(new OrdersLoader.Field(notesPostKey, notes.text));
        return res.ToArray();
    }
}