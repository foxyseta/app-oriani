using UnityEngine;
using UnityEngine.UI;

public class MenuWidget : MonoBehaviour
{
    [SerializeField]
    Transform dishesContainer = null;
    [SerializeField]
    DishWidget dishWidgetPrefab = null;

    public DishScreen popUp = null;
    public Transform orderedDishesContainer = null;
    public Text orderedDishesTotal = null;

    public DishWidget AddDishWidget(Dishes.Dish d, GameObject errorScreen)
    {
        DishWidget w = Instantiate(dishWidgetPrefab, dishesContainer);
        w.gameObject.name = d.name;
        w.popUp = popUp;
        w.orderedDishesContainer = orderedDishesContainer;
        w.orderedDishesTotal = orderedDishesTotal;
        w.dishErrorScreen = errorScreen;

        // should be called last
        w.Dish = d;
        return w;
    }
}