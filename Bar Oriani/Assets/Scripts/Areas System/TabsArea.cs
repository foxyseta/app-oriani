using UnityEngine;

public class TabsArea : Area
{
    public SidebarContentArea sidebarContent;

    override protected Rect GetArea()
    {
        return new Rect(Screen.safeArea.x + sidebarContent.relativeWidth * Screen.safeArea.width, Screen.safeArea.y,
                        (1 - sidebarContent.relativeWidth) * Screen.safeArea.width, Screen.safeArea.height);
    }
}
