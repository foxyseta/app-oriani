using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class DishPopUp : MonoBehaviour
{
    [SerializeField]
    DishesLoader loader = null;
    [Header("UI Elements")]
    [SerializeField]
    Text log = null;
    [SerializeField]
    Image pictureInput = null;
    [SerializeField]
    InputField nameInput = null;
    [SerializeField]
    Dropdown menuInput = null;
    [SerializeField]
    InputField descriptionInput = null, priceInput = null, quantityInput = null;
    [SerializeField]
    float defaultPrice = 1f;
    [SerializeField]
    int defaultQuantity = 0;
    [Header("Data")]
    [SerializeField]
    string addUrl = null;
    [SerializeField]
    string editUrl = null, oldNameKey = null, nameKey = null, menuNameKey = null,
           descriptionKey = null, priceKey = null, quantityKey = null, pictureKey = null;
    [Header("Messages")]
    [SerializeField]
    string missingFieldsMessage = "Please complete the form first.";
    [SerializeField]
    string missingDescriptionMessage = "No description available.";

    readonly string culture = "en-US";
    string oldDishName = null;
    int oldQuantity;
    Sprite defaultPictureInputSprite;

    void ResetPicture()
    {
        if (defaultPictureInputSprite)
            pictureInput.sprite = defaultPictureInputSprite;
    }

    public void AddDishMode()
    {
        gameObject.SetActive(true);
        oldDishName = null;
        nameInput.text = descriptionInput.text = priceInput.text =
            quantityInput.text = string.Empty;
        menuInput.value = 0;
        ResetPicture();
    }

    public void EditDishMode(Dishes.Dish d, Sprite s)
    {
        gameObject.SetActive(true);
        if (!defaultPictureInputSprite)
            defaultPictureInputSprite = pictureInput.sprite;
        pictureInput.sprite = s;
        oldDishName = nameInput.text = d.name;
        for (menuInput.value = 0; menuInput.value < menuInput.options.Count - 1; ++menuInput.value)
            if (menuInput.options[menuInput.value].text == d.menu_name)
                break;
        descriptionInput.text = d.description;
        priceInput.text = d.price.ToString("C", CultureInfo.CreateSpecificCulture(culture));
        quantityInput.text = (oldQuantity = d.quantity_in_stock).ToString();
    }

    public void OnQuantityEditEnd()
    {
        int quantity;
        if (!int.TryParse(quantityInput.text, out quantity) || quantity < 0)
            quantityInput.text = (defaultQuantity).ToString();
    }

    public void OnPriceEditEnd()
    {
        float price;
        if (!float.TryParse(priceInput.text, out price) || price < 0)
            priceInput.text = (defaultPrice).ToString();
    }

    public void Confirm()
    {
        if (nameInput.text != string.Empty && priceInput.text != string.Empty
            && quantityInput.text != string.Empty)
        {
            if (descriptionInput.text == string.Empty)
                descriptionInput.text = missingDescriptionMessage;
            loader.postRequest = true;
            string old_url = loader.url;
            loader.url = oldDishName == null ? addUrl : editUrl;
            loader.formData = oldDishName == null ?
                new JSONLoader<Dishes>.Field[] {
                    new JSONLoader<Dishes>.Field(nameKey, nameInput.text),
                    new JSONLoader<Dishes>.Field(menuNameKey, menuInput.options[menuInput.value].text),
                    new JSONLoader<Dishes>.Field(descriptionKey, descriptionInput.text),
                    new JSONLoader<Dishes>.Field(priceKey, priceInput.text),
                    new JSONLoader<Dishes>.Field(quantityKey, quantityInput.text)
                } :
                new JSONLoader<Dishes>.Field[] {
                    new JSONLoader<Dishes>.Field(oldNameKey, oldDishName),
                    new JSONLoader<Dishes>.Field(nameKey, nameInput.text),
                    new JSONLoader<Dishes>.Field(menuNameKey, menuInput.options[menuInput.value].text),
                    new JSONLoader<Dishes>.Field(descriptionKey, descriptionInput.text),
                    new JSONLoader<Dishes>.Field(priceKey, priceInput.text),
                    new JSONLoader<Dishes>.Field(quantityKey, (int.Parse(quantityInput.text) - oldQuantity).ToString())
                };
            loader.formFiles = new JSONLoader<Dishes>.File[] {
                new JSONLoader<Dishes>.File(pictureKey, pictureInput.sprite.texture.EncodeToPNG(), "upload.png", "image/png")
            };
            loader.Refresh();
            loader.url = old_url;
            loader.formData = new JSONLoader<Dishes>.Field[] { };
            loader.formFiles = new JSONLoader<Dishes>.File[] { };
        }
        else
            log.text = missingFieldsMessage;
    }
}
