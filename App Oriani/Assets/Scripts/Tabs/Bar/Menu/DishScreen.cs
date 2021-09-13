using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class DishScreen : MonoBehaviour
{
    [SerializeField]
    Image picture = null;
    [SerializeField]
    Text content = null;
    [SerializeField]
    string culture = "en-US";
    [SerializeField]
    [Multiline]
    string contentFormat = "<b>{0}</b> <i>{1}</i>\n{2}\n<i>Remaining: {3}</i>";

    public void Fill(DishWidget w)
    {
        SetImage(w.picture.image);
        content.text = string.Format(contentFormat,
                                     w.Dish.name,
                                     w.Dish.price.ToString("C", CultureInfo.
                                     CreateSpecificCulture(culture)),
                                     w.Dish.description,
                                     w.Dish.quantity_in_stock);
        ContentSizeFitter c = content.transform.parent.GetComponent<ContentSizeFitter>();
        if (c)
        {
            c.SetLayoutHorizontal();
            c.SetLayoutVertical();
        }
    }

    void SetImage(Image i)
    {
        picture.sprite = i.sprite;
        AspectRatioFitter arf = GetComponent<AspectRatioFitter>();
        if (arf)
            arf.aspectRatio = (float)picture.sprite.rect.width / picture.sprite.rect.height;
    }
}
