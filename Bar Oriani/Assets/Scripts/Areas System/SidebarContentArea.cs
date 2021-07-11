using UnityEngine;

public class SidebarContentArea : Area
{
    public float relativeWidth = 0.2f;

    protected override Rect GetArea()
    {
        return new Rect(Screen.safeArea.x, Screen.safeArea.y, relativeWidth * Screen.safeArea.width, Screen.safeArea.height);
    }
}