using UnityEngine;

class BelowSafeArea : Area
{
    override protected Rect GetArea()
    {
        return new Rect(Screen.safeArea.x, Screen.safeArea.yMax, Screen.safeArea.width, Screen.height - Screen.safeArea.yMax);
    }

}