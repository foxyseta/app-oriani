using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BarData
{
    [System.Serializable]
    public class Generics
    {
        [System.Serializable]
        public class TimeInterval
        {
            public string from, until;
        }

        [System.Serializable]
        public class Lunch
        {
            public TimeInterval orders, delivery;
        }

        public string barName;
        public Lunch lunch;
    }

    public Generics bar;
    public string name, place_name, orders_time_limit, delivery_time;
    
}

public class BarDataLoader : JSONLoader<BarData>
{
    [Header("UI")]
    [SerializeField]
    Text barDataHeader = null;
    [SerializeField]
    Text barDataTimetable = null;
    [Multiline(5)]
    [SerializeField]
    string barDataHeaderFormat = "{0} {1} {2}",
           barDataTimetableFormat = "{0} {1} {2} {3} {4} {5}";

    override protected void UseContent()
    {
        barDataHeader.text = string.Format(barDataHeaderFormat,
                                           content.data.bar.barName,
                                           content.data.name,
                                           content.data.place_name);
        barDataTimetable.text = string.Format(barDataTimetableFormat,
                                              content.data.orders_time_limit,
                                              content.data.delivery_time,
                                              content.data.bar.lunch.orders.from,
                                              content.data.bar.lunch.orders.until,
                                              content.data.bar.lunch.delivery.from,
                                              content.data.bar.lunch.delivery.until);
    }
}