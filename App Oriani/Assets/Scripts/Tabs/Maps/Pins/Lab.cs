using UnityEngine.UI;

public class Lab : Pin
{
    public TabButton labsTabButton;
    public Dropdown labsDropdown;
    public int labID;

    protected override void Interact()
    {
        labsDropdown.value = labID;
        labsTabButton.OnClick();
    }
}
