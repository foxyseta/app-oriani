using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public string tutorialElementID;

    void Start()
    {
        gameObject.SetActive(!PlayerPrefs.HasKey(tutorialElementID));
    }

    public void Disable()
    {
        PlayerPrefs.SetInt(tutorialElementID, 0);
        Start();
    }

    public void Enable()
    {
        PlayerPrefs.DeleteKey(tutorialElementID);
        Start();
    }
}
