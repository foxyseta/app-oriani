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
    [Header("UI")]
    [SerializeField]
    Dropdown menuSelector = null;
    [SerializeField]
    DropdownViewsManager menuWidgetsContainer = null;
    [SerializeField]
    MenuWidget menuWidgetPrefab = null;
    [SerializeField]
    DishScreen popUp = null;
    [SerializeField]
    Transform orderedDishesContainer = null;
    [SerializeField]
    Text orderedDishesTotal = null;
    [SerializeField]
    GameObject dishErrorScreen = null;

    protected override void UseContent()
    {
        if (content.data.menus.Length == 0 || content.data.dishes.Length == 0)
            PrintOnLoaderLog(noDataMessage);
        UpdateDishes(UpdateMenus());
    }

    Dictionary<string, MenuWidget> UpdateMenus()
    {
        // Destroy old menus
        foreach (Transform child in menuWidgetsContainer.transform)
            Destroy(child.gameObject);
        // Save old selected menu
        string currentMenu = menuSelector.options.Count > 0 ?
            menuSelector.options[menuSelector.value].text :
            string.Empty;
        // Update menus
        menuSelector.ClearOptions();
        List<string> menuNames = new List<string>();
        Dictionary<string, MenuWidget> menuWidgets = new Dictionary<string, MenuWidget>();
        foreach (Dishes.Menu menu in content.data.menus)
        {
            menuNames.Add(menu.name);
            MenuWidget m = (menuWidgets[menu.name] =
                Instantiate(menuWidgetPrefab, menuWidgetsContainer.transform));
            m.gameObject.name = menu.name;
            m.popUp = popUp;
            m.orderedDishesContainer = orderedDishesContainer;
            m.orderedDishesTotal = orderedDishesTotal;
            DropdownView d = m.GetComponent<DropdownView>();
            if (d)
                d.Switch(false);
        }
        // Update dropdown
        menuSelector.AddOptions(menuNames);
        if (currentMenu != string.Empty)
        {
            int i;
            for (i = menuSelector.options.Count - 1; menuSelector.options[i].text != currentMenu && i > 0; --i) ;
            menuSelector.value = i;
            DropdownView d = menuWidgets[menuSelector.options[menuSelector.value].text].GetComponent<DropdownView>();
            if (d)
                d.Switch(true);
        }
        return menuWidgets;
    }

    void UpdateDishes(Dictionary<string, MenuWidget> menuWidgets)
    {
        foreach (Dishes.Dish d in content.data.dishes)
            if (menuWidgets.ContainsKey(d.menu_name))
                menuWidgets[d.menu_name].AddDishWidget(d, dishErrorScreen);
    }

}