using UnityEngine;
using UnityEngine.UI;

public class PrinterAddressSetter : MonoBehaviour
{
    [SerializeField]
    string printerAddressKey = "Printer Address";

    InputField input;

    private void Awake()
    {
        input = GetComponent<InputField>();
    }

    void Start()
    {
        input.text = PlayerPrefs.HasKey(printerAddressKey) ?
            PlayerPrefs.GetString(printerAddressKey) : string.Empty;
    }

    public void Set()
    {
        PlayerPrefs.SetString(printerAddressKey, input.text);
    }
}