using UnityEngine;

public class SidebarArea : Area
{
    public SidebarContentArea content;

    override protected Rect GetArea()
    {
        return new Rect(0, 0, Screen.safeArea.x + content.relativeWidth * Screen.safeArea.width, Screen.height);
    }
}
