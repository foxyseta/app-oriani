using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarLoader : WebLoader
{
    [Header("Tags used")]
    public string nameTag;
    public string descriptionTag;
    public string notesTag;
    public string dateTag;
    public string timeTag;
    public string placeTag;
    [Header("UI")]
    public InputField query;
    public int queryFieldIndex;
    public Dropdown categoryDropdown;
    public int categoryFieldIndex;
    public string[] categoryDropdownValues;
    public InputField monthInput;
    public int monthFieldIndex;
    public string defaultMonthFieldValue;
    public GameObject eventsContainer;
    public GameObject eventPrefab;
    public int nameChildIndex,
               contentChildIndex,
               dayAndTimeChildIndex,
               placeChildIndex;
    [Multiline]
    public string contentFormat = "<b>{0}</b> {1}"; // {0} = description, {1} = notes
    [Multiline]
    public string dayAndTimeFormat = "{0}, {1}"; // {0} = date, {1} = time
    public string noNotesMessage = "No notes available.";

    string MonthAndYearToDate(string my)
    {
        int x;
        return (my.Length == 7 && int.TryParse(my.Substring(0, 2), out x) && 0 < x && x < 13 &&
            my[2] == '/' && int.TryParse(my.Substring(3), out x) && x > 0) ?
            "01/" + my : defaultMonthFieldValue;
    }

    public override void Refresh()
    {
        formData[queryFieldIndex].value = query.text;
        formData[categoryFieldIndex].value = categoryDropdownValues[categoryDropdown.value];
        formData[monthFieldIndex].value = MonthAndYearToDate(monthInput.text);
        /*foreach (field x in formData)
            print(x.key + ": " + x.value + "\n");*/
        base.Refresh();
    }

    protected override void UseContent()
    {
        foreach (Transform child in eventsContainer.transform)
            Destroy(child.gameObject);
        foreach (Dictionary<string, string> record in content)
        {
            GameObject eventObject = Instantiate(eventPrefab, eventsContainer.transform);
            Transform eventTransform = eventObject.transform;
            // name
            eventTransform.GetChild(nameChildIndex).gameObject.GetComponent<Text>().text = record[nameTag];
            // content
            if (record[notesTag] == String.Empty)
                record[notesTag] = noNotesMessage;
            eventTransform.GetChild(contentChildIndex).gameObject.GetComponent<Text>().text =
                String.Format(contentFormat, record[descriptionTag], record[notesTag]).Trim();
            // day and time
            eventTransform.GetChild(dayAndTimeChildIndex).gameObject.GetComponent<Text>().text =
                String.Format(dayAndTimeFormat, record[dateTag], record[timeTag]).Trim();
            // place
            eventTransform.GetChild(placeChildIndex).gameObject.GetComponent<Text>().text = record[placeTag];
        }
    }
}