using UnityEngine;

class SafeArea : Area
{
    override protected Rect GetArea()
    {
        return Screen.safeArea;
    }

}