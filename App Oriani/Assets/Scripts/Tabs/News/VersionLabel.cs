using System;
using UnityEngine;
using UnityEngine.UI;

public class VersionLabel : MonoBehaviour
{
    void Start()
    {
        Text t = GetComponent<Text>();
        if (t)
            t.text = String.Format(t.text, Application.version);
    }
}
