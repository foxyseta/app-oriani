using UnityEngine;
using UnityEngine.UI;

public class OrderEdit : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    InputField notes = null;
    [Header("Data")]
    [SerializeField]
    OrdersLoader loader = null;
    [SerializeField]
    string url = null,
           IdPostKey = "ID",
           notesPostKey = "notes",
           statePostKey = "state";

    int ID;

    public void SetUpEditScreen(Orders.Order o)
    {
        gameObject.SetActive(true);
        ID = o.ID;
        notes.text = o.barman_notes;
    }

    public void EditOrder(string newState)
    {
        loader.postRequest = true;
        string old_url = loader.url;
        loader.url = url;
        loader.formData = notes.text == string.Empty ?
            new OrdersLoader.Field[] {
                new OrdersLoader.Field(IdPostKey, ID.ToString()),
                new OrdersLoader.Field(statePostKey, newState)
            } :
            new OrdersLoader.Field[] {
                new OrdersLoader.Field(IdPostKey, ID.ToString()),
                new OrdersLoader.Field(notesPostKey, notes.text),
                new OrdersLoader.Field(statePostKey, newState)
        };
        loader.Refresh();
        loader.url = old_url;
        Close();
    }

    public void Close()
    {
        notes.text = string.Empty;
        gameObject.SetActive(false);
    }
}