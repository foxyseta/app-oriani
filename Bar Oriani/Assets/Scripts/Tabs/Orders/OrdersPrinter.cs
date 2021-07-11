using UnityEngine;
using UnityEngine.UI;

public class OrdersPrinter : MonoBehaviour
{
    [SerializeField]
    OrdersLoader loader = null;
    [SerializeField]
    Text log = null;
    [SerializeField]
    string goodPrint = "\"{0}\": printed.",
           badPrint = "\"{0}\": bad print",
           noPrinter = "Please set printer address first.";
    string printerAddressKey = "Printer Address";

    public void OnClick()
    {
        OnClick(null);
    }

    public void OnClick(OrderWidget w)
    {
        string printerAddress = PlayerPrefs.GetString(printerAddressKey);
        try
        {
            if (w == null)
                loader.EscPosPrint(printerAddress, true);
            else
                loader.EscPosPrint(printerAddress, w.Order);
            log.text = string.Format(goodPrint, printerAddress);
        }
        catch (System.ArgumentNullException)
        {
            log.text = noPrinter;
        }
        catch (System.ArgumentException e)
        {
            OnFailedPrint(printerAddress);
            print(e);
        }
        catch (System.IO.FileNotFoundException e)
        {
            OnFailedPrint(printerAddress);
            print(e);
        }
    }

    void OnFailedPrint(string printerAddress)
    {
        log.text = printerAddress == string.Empty ?
        noPrinter : string.Format(badPrint, printerAddress);
    }
}