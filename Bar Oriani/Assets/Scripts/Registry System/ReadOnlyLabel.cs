using UnityEngine;
using UnityEngine.UI;

public class ReadOnlyLabel : TabButton
{
    [SerializeField]
    Text nameText = null;
    [SerializeField]
    Image selectedIcon = null;

    public string PlaceName
    {
        get => gameObject.name;
        set => gameObject.name = nameText.text = value;
    }

    public override void Switch(bool mode)
    {
        base.Switch(mode);
        if (selectedIcon)
            selectedIcon.color = new UnityEngine.Color(255, 255, 255, mode ? 255 : 0);
    }
}