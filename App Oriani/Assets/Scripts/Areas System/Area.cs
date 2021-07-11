using UnityEngine;

public abstract class Area : MonoBehaviour
{
    RectTransform Panel;
    Rect LastArea = new Rect(0, 0, 0, 0);

    abstract protected Rect GetArea();

    void Awake()
    {
        Panel = GetComponent<RectTransform>();
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        Rect Area = GetArea();

        if (Area != LastArea)
            ApplyArea(Area);
    }

    void ApplyArea(Rect r)
    {
        LastArea = r;

        // Convert  area rectangle from absolute pixels to normalised anchor coordinates
        Vector2 anchorMin = r.position;
        Vector2 anchorMax = r.position + r.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        Panel.anchorMin = anchorMin;
        Panel.anchorMax = anchorMax;

        /* Debug.LogFormat("New  area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
            name, r.x, r.y, r.width, r.height, Screen.width, Screen.height); */
    }
}