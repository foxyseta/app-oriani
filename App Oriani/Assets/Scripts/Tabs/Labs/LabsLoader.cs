using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LabsLoader : WebLoader
{
    [Header("Tags")]
    public string[] tagsUsed;
    [Header("UI")]
    public Dropdown dropdown;
    public int dropdownFieldIndex;
    public string[] dropdownValues;
    public GameObject bookingsContainer;
    public int columns = 7;
    public int[] columnsUsed = { 0, 1, 2, 3, 4, 5, 6 };
    public InputField dateInput;
    public int dateFieldIndex;
    public string defaultDateFieldValue;

    public override void Refresh()
    {
        formData[dropdownFieldIndex].value = dropdownValues[dropdown.value];
        formData[dateFieldIndex].value = dateInput.text;
        base.Refresh();
    }

    protected override void UseContent()
    {
        int cells = bookingsContainer.transform.childCount;
        for (int i = 0, period = -1; i < cells; ++i)
        {
            int column = i % columns;
            if (column == 0)
                ++period;
            if (columnsUsed.Contains(column))
                bookingsContainer.transform.GetChild(i).GetComponent<Text>().text =
                    content[period][tagsUsed[Array.IndexOf(columnsUsed, column)]];
        }
    }

}
