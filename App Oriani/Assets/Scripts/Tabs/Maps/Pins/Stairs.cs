using UnityEngine.UI;

public class Stairs : Pin
{
    public Dropdown floorsDropdown;
    public int destinationFloor;

    protected override void Interact()
    {
        floorsDropdown.value = destinationFloor;
    }
}
