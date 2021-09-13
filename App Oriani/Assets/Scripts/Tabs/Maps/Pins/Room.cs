using UnityEngine.UI;

public class Room : Pin
{
    public TabButton calendarTabButton;
    public InputField calendarInputField;
    public string roomName;

    protected override void Interact()
    {
        calendarInputField.text = roomName;
        calendarTabButton.OnClick();
    }
}
