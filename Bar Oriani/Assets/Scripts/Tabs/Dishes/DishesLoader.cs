using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dishes
{
    [System.Serializable]
    public class Menu
    {
        public string name;
    }

    [System.Serializable]
    public class Dish
    {
        public string name, menu_name, description;
        public float price;
        public int quantity_in_stock;
    }

    public Menu[] menus;
    public Dish[] dishes;
}

public class DishesLoader : JSONLoader<Dishes>
{
    [Header("Menus")]
    public TabsManager menusContainer;
    public Label labelPrefab;

    [Header("Dishes")]
    public GameObject pageContainer;
    public Page pagePrefab;
    public DishPopUp popUp;
    public Dropdown popupMenuSelector;
    public DishWidget dishWidgetPrefab;

    protected override void UseContent()
    {
        string oldMenuName = TabButton.CurrentTabButtons.ContainsKey(menusContainer.transform) ?
            TabButton.CurrentTabButtons[menusContainer.transform].gameObject.name : null;
        // Destroy menus
        foreach (Transform child in menusContainer.transform)
            Destroy(child.gameObject);
        popupMenuSelector.ClearOptions();
        // Destroy dishes
        foreach (Transform child in pageContainer.transform)
            Destroy(child.gameObject);
        // Add menus
        if (content.data.menus == null)
            return;
        Dictionary<string, Transform> dishesContainers = new Dictionary<string, Transform>();
        foreach (Dishes.Menu menu in content.data.menus)
        {
            // Label
            Label menuLabel = Instantiate(labelPrefab, menusContainer.transform);
            menuLabel.ChangeName(menu.name);
            EditMenu e = menuLabel.GetComponent<EditMenu>();
            DeleteMenu d = menuLabel.GetComponent<DeleteMenu>();
            if (e)
                e.loader = this;
            if (d)
                d.loader = this;
            // Page
            Page dishesPage = Instantiate(pagePrefab, pageContainer.transform);
            CanvasGroup pageCanvasGroup = dishesPage.GetComponent<CanvasGroup>();
            if (pageCanvasGroup)
                menuLabel.linkedTab = pageCanvasGroup;
            if (menu.name == oldMenuName)
            {
                menusContainer.defaultTabButton = menuLabel;
                menusContainer.Reset();
            }
            dishesContainers[menu.name] = dishesPage.container;
            // Pop up dropdown
            popupMenuSelector.AddOptions(new List<string> { menu.name });
        }
        // Add dishes
        if (content.data.dishes == null || content.data.dishes.Length == 0)
            OnNoData();
        foreach (Dishes.Dish dish in content.data.dishes)
            if (dishesContainers.ContainsKey(dish.menu_name))
            {
                DishWidget dishWidget =
                    Instantiate(dishWidgetPrefab, dishesContainers[dish.menu_name]);
                dishWidget.loader = this;
                dishWidget.popUp = popUp;
                dishWidget.Dish = dish;
            }
        // Tabs manager
        menusContainer.Reset();
    }
}