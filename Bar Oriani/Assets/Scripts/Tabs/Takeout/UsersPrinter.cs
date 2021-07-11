using UnityEngine;
using UnityEngine.UI;

public class UsersPrinter : MonoBehaviour
{
    [SerializeField]
    TakeoutLoader loader = null;
    [SerializeField]
    Text log = null;
    [SerializeField]
    string goodPrint = "\"{0}\": printed.",
           badPrint = "\"{0}\": bad print",
           noPrinter = "Please set printer address first.";
    string printerAddressKey = "Printer Address";

    public void OnClick()
    {
        string printerAddress = PlayerPrefs.GetString(printerAddressKey);
        try
        {
            loader.EscPosPrint(printerAddress);
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