using UnityEngine.UI;

public class Label : TabButton
{
    public InputField inputField;
    public Image selectedIcon = null;

    public void ChangeName(string s)
    {
        if (s.Length > 0)
            gameObject.name = inputField.text =
                inputField.placeholder.GetComponent<Text>().text = s;
    }

    public override void Switch(bool mode)
    {
        base.Switch(mode);
        if (selectedIcon)
            selectedIcon.color = new UnityEngine.Color(255, 255, 255, mode ? 255 : 0);
    }
}