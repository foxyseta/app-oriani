using ESC_POS_USB_NET.Printer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Takeout
{
    [System.Serializable]
    public class Place
    {
        public string name;
    }

    [System.Serializable]
    public class Takeout_information
    {
        public string name, password, orders_time_limit, delivery_time, place_name;
    }

    public Place[] places;
    public Takeout_information[] users;
}

public class TakeoutLoader : JSONLoader<Takeout>
{
    [Header("Places")]
    public TabsManager placesContainer;
    public Label labelPrefab;

    [Header("Takeout information")]
    public GameObject pageContainer;
    public Page pagePrefab;
    public TakeoutPost takeoutPostPrefab;
    public Dropdown popupPlaceSelector;

    [Header("Messages")]
    [SerializeField]
    string noPlaceSelected = "No place selected.";
    [SerializeField]
    string emptyPlace = "There are zero users for this place.",
           titleFormat = "Passwords for {0}";

    protected override void UseContent()
    {
        string oldPlaceName = TabButton.CurrentTabButtons.ContainsKey(placesContainer.transform) ?
            TabButton.CurrentTabButtons[placesContainer.transform].gameObject.name : null;
        // Destroy places
        foreach (Transform child in placesContainer.transform)
            Destroy(child.gameObject);
        Dictionary<string, Transform> takeoutInformationContainers = new Dictionary<string, Transform>();
        // Destroy takeout information
        foreach (Transform child in pageContainer.transform)
            Destroy(child.gameObject);
        // Destroy place selector options
        popupPlaceSelector.ClearOptions();
        // Add places
        if (content.data.places == null)
            return;
        foreach (Takeout.Place place in content.data.places)
        {
            // Label
            Label placeLabel = Instantiate(labelPrefab, placesContainer.transform);
            placeLabel.ChangeName(place.name);
            EditPlace e = placeLabel.GetComponent<EditPlace>();
            DeletePlace d = placeLabel.GetComponent<DeletePlace>();
            if (e)
                e.loader = this;
            if (d)
                d.loader = this;
            // Page
            Page takeoutInformationPage = Instantiate(pagePrefab, pageContainer.transform);
            CanvasGroup pageCanvasGroup = takeoutInformationPage.GetComponent<CanvasGroup>();
            if (pageCanvasGroup)
                placeLabel.linkedTab = pageCanvasGroup;
            if (place.name == oldPlaceName)
            {
                placesContainer.defaultTabButton = placeLabel;
                placesContainer.Reset();
            }
            takeoutInformationContainers[place.name] = takeoutInformationPage.container;
            // Pop up dropdown
            popupPlaceSelector.AddOptions(new List<string> { place.name });
        }
        // Add takeout information
        if (content.data.users == null || content.data.users.Length == 0)
            OnNoData();
        foreach (Takeout.Takeout_information takeoutInformation in content.data.users)
            if (takeoutInformationContainers.ContainsKey(takeoutInformation.place_name))
            {
                TakeoutPost takeoutPost =
                    Instantiate(takeoutPostPrefab, takeoutInformationContainers[takeoutInformation.place_name]);
                takeoutPost.Takeout_Information = takeoutInformation;
                takeoutPost.loader = this;
            }
        // Tabs manager
        placesContainer.Reset();
    }

    public void EscPosPrint(string printerAddress)
    {
        // Get current place
        string place = TabButton.CurrentTabButtons.ContainsKey(placesContainer.transform) ?
            TabButton.CurrentTabButtons[placesContainer.transform].gameObject.name : null;
        if (place == null)
        {
            PrintOnLoaderLog(noPlaceSelected);
            return;
        }
        Printer printer = new Printer(printerAddress);
        // printer.Image(LogoBitmap());
        // Header
        printer.AlignCenter();
        printer.ExpandedMode(ESC_POS_USB_NET.Enums.PrinterModeState.On);
        printer.Append(string.Format(titleFormat, place));
        printer.ExpandedMode(ESC_POS_USB_NET.Enums.PrinterModeState.Off);
        printer.AlignLeft();
        printer.Separator();
        // Body
        bool printedSomething = false;
        foreach (Takeout.Takeout_information user in content.data.users)
            if (user.place_name == place)
            {
                int deltaLength;
                string username = user.name;
                deltaLength = username.Length + user.password.Length - printer.ColsNomal;
                if (deltaLength > 0)
                    username = username.Substring(0, printer.ColsNomal - user.password.Length);
                else if (deltaLength < 0)
                    username = username.PadRight(printer.ColsNomal - user.password.Length);
                printer.Append(username + user.password);
                printedSomething = true;
            }
        if (!printedSomething)
        {
            log.text = emptyPlace;
            return;
        }
        printer.FullPaperCut();
        printer.NewLines(2);
        printer.PrintDocument();
    }
}