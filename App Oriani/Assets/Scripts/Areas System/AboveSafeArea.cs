using UnityEngine;

class AboveSafeArea : Area
{
    override protected Rect GetArea()
    {
        return new Rect(Screen.safeArea.x, 0, Screen.safeArea.width, Screen.safeArea.y);
    }

}