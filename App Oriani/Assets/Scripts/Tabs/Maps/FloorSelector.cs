using UnityEngine;
using UnityEngine.UI;

public class FloorSelector : MonoBehaviour
{

    public GameObject[] floors;
    public int defaultFloor = 0;
    private Dropdown dropdown;
    private int currentFloor;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.value = currentFloor = defaultFloor;
        for (int i = 0; i < floors.Length; ++i)
            floors[i].SetActive(i == currentFloor);
    }

    void Update()
    {

    }

    public void changeFloor()
    {
        if (-1 < dropdown.value && dropdown.value < floors.Length)
        {
            if (-1 < currentFloor && currentFloor < floors.Length)
                floors[currentFloor].SetActive(false);
            floors[currentFloor = dropdown.value].SetActive(true);
        }
    }
}
