using ESC_POS_USB_NET.Printer;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Orders
{
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

        public int ID;
        public string date, customer_notes, barman_notes, state, place, user, time;
        public OrderedItem[] orderedItems;
    }

    public Takeout.Place[] places;
    public Order[] orders;
}

public class OrdersLoader : JSONLoader<Orders>
{
    [Header("Printer")]
    [SerializeField]
    OrdersPrinter ordersPrinter = null;
    [SerializeField]
    UnityEngine.UI.Image printedLogo = null;
    [SerializeField]
    string printerEncoding = "Windows-1252";
    [Header("Post Request")]
    [SerializeField]
    InputField dateInput = null;
    [SerializeField]
    string dateKey = "date";

    [Header("Places")]
    public TabsManager placesContainer;
    public ReadOnlyLabel labelPrefab;

    [Header("Orders")]
    public GameObject pageContainer;
    public Page pagePrefab;
    public OrderWidget orderWidgetPrefab;
    [SerializeField]
    OrderEdit orderEdit = null;
    [SerializeField]
    string printableState = "Accepted",
           confirmPlaceUrl = null,
           placePostKey = "confirmedPlace";
    [Header("Messages")]
    [SerializeField]
    string noPlaceSelected = "No place selected.";
    [SerializeField]
    string emptyPlace = "There are zero orders for this place.",
           cultureName = "en-US", totalName = "Total";

    HashSet<int> printedOrdersIDs = new HashSet<int>();
    Dictionary<string, Transform> ordersContainers;

    void AddToPrinter(ref Printer printer, Orders.Order order)
    {
        printedOrdersIDs.Add(order.ID);
        // Header
        printer.AlignCenter();
        printer.ExpandedMode(ESC_POS_USB_NET.Enums.PrinterModeState.On);
        printer.Append(order.place + " - " + order.user);
        printer.Separator();
        printer.ExpandedMode(ESC_POS_USB_NET.Enums.PrinterModeState.On);
        printer.Append(order.date + " - " + order.time);
        printer.Separator();
        float total = 0f;
        // Body
        int deltaLength;
        foreach (Orders.Order.OrderedItem item in order.orderedItems)
        {
            printer.AlignLeft();
            string left = item.quantity + " x " + item.dish;
            string right = Encoding.GetEncoding(printerEncoding).GetString(
                    Encoding.GetEncoding(encoding).GetBytes(
                        item.totalPrice.ToString("F", CultureInfo.CreateSpecificCulture(cultureName))
                    ));
            deltaLength = left.Length + right.Length - printer.ColsNomal;
            if (deltaLength > 0)
                left = left.Substring(0, printer.ColsNomal - right.Length);
            else if (deltaLength < 0)
                left = left.PadRight(printer.ColsNomal - right.Length);
            printer.Append(left + right);
            total += item.totalPrice;
        }
        // Footer
        printer.Separator();
        printer.ExpandedMode(ESC_POS_USB_NET.Enums.PrinterModeState.On);
        string totalPrice = total.ToString("F", CultureInfo.CreateSpecificCulture(cultureName));
        deltaLength = totalName.Length + totalPrice.Length - printer.ColsExpanded;
        if (deltaLength > 0)
            totalName = totalName.Substring(0, printer.ColsExpanded - totalPrice.Length);
        else if (deltaLength < 0)
            totalName = totalName.PadRight(printer.ColsExpanded - totalPrice.Length);
        printer.Append(totalName + totalPrice);
        printer.Separator();
        printer.AlignLeft();
        printer.Append(order.customer_notes);
        printer.AlignRight();
        printer.Append(order.barman_notes);
        printer.PartialPaperCut();
    }

    string GetCurrentPlace()
    {
        return TabButton.CurrentTabButtons.ContainsKey(placesContainer.transform) ?
            TabButton.CurrentTabButtons[placesContainer.transform].gameObject.name : null;
    }

    public void EscPosPrint(string printerAddress, bool onlyNotPrintedYet = false)
    {
        string place = GetCurrentPlace();
        if (place == null)
        {
            PrintOnLoaderLog(noPlaceSelected);
            return;
        }
        // Set every order for the current place as already printed
        foreach (Transform child in ordersContainers[place])
        {
            OrderWidget w = child.GetComponent<OrderWidget>();
            if (w)
                w.SetPrinterSprite(true);
        }
        Printer printer = new Printer(printerAddress);
        // printer.Image(LogoBitmap());
        bool printedSomething = false;
        foreach (Orders.Order order in content.data.orders)
            if (order.place == place && order.state == printableState && !(onlyNotPrintedYet && printedOrdersIDs.Contains(order.ID)))
            {
                AddToPrinter(ref printer, order);
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

    public void EscPosPrint(string printerAddress, Orders.Order order)
    {
        Printer printer = new Printer(printerAddress);
        AddToPrinter(ref printer, order);
        printer.PrintDocument();
    }

    public void ConfirmPageOrders()
    {
        string place = GetCurrentPlace();
        if (place == null)
        {
            PrintOnLoaderLog(noPlaceSelected);
            return;
        }
        formData = new Field[] { new Field(placePostKey, place) };
        string old_url = url;
        url = confirmPlaceUrl;
        Refresh();
        url = old_url;
    }

    Bitmap LogoBitmap()
    {
        Bitmap res = new Bitmap(new System.IO.MemoryStream(printedLogo.sprite.texture.GetRawTextureData()));
        return res;
    }

    override public void Refresh()
    {
        if (dateInput.text != string.Empty)
        {
            postRequest = true;
            List<Field> newFormData = new List<Field>(formData);
            newFormData.Add(new Field(dateKey, dateInput.text));
            formData = newFormData.ToArray();
            base.Refresh();
            formData = new Field[] { };
        }
        else
            base.Refresh();
    }

    void PrintedOrdersIDsUpdate()
    {
        foreach (Orders.Order o in content.data.orders)
            if (o.state != printableState)
                printedOrdersIDs.Remove(o.ID);
    }

    protected override void UseContent()
    {
        PrintedOrdersIDsUpdate();
        string oldPlaceName = TabButton.CurrentTabButtons.ContainsKey(placesContainer.transform) ?
            TabButton.CurrentTabButtons[placesContainer.transform].gameObject.name : null;
        // Destroy places
        foreach (Transform child in placesContainer.transform)
            Destroy(child.gameObject);
        ordersContainers = new Dictionary<string, Transform>();
        // Destroy takeout information
        foreach (Transform child in pageContainer.transform)
            Destroy(child.gameObject);
        // Add places
        if (content.data.places == null)
            return;
        foreach (Takeout.Place place in content.data.places)
        {
            // Label
            ReadOnlyLabel placeLabel = Instantiate(labelPrefab, placesContainer.transform);
            placeLabel.PlaceName = place.name;
            // Page
            Page ordersPage = Instantiate(pagePrefab, pageContainer.transform);
            CanvasGroup pageCanvasGroup = ordersPage.GetComponent<CanvasGroup>();
            if (pageCanvasGroup)
                placeLabel.linkedTab = pageCanvasGroup;
            if (place.name == oldPlaceName)
            {
                placesContainer.defaultTabButton = placeLabel;
                placesContainer.Reset();
            }
            ordersContainers[place.name] = ordersPage.container;
        }
        // Add orders
        if (content.data.orders.Length == 0)
            OnNoData();
        foreach (Orders.Order o in content.data.orders)
            if (ordersContainers.ContainsKey(o.place))
            {
                OrderWidget orderWidget =
                    Instantiate(orderWidgetPrefab, ordersContainers[o.place]);
                orderWidget.Order = o;
                orderWidget.orderEdit = orderEdit;
                orderWidget.ordersPrinter = ordersPrinter;
                orderWidget.SetPrinterSprite(printedOrdersIDs.Contains(o.ID) || o.state != printableState);
            }
        // Tabs manager
        placesContainer.Reset();
    }
}