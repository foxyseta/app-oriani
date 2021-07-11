using SFB;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImagePicker : MonoBehaviour
{
    [SerializeField]
    string panelTitle = "Pick Image", mainExtensionFilterName = "All supported formats";

    public void Pick()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            panelTitle,
            "",
            new[] {
                new ExtensionFilter(mainExtensionFilterName, "jpg", "jpeg", "jpe", "jfif", "png"),
                new ExtensionFilter("JPEG", "jpg", "jpeg", "jpe", "jfif"),
                new ExtensionFilter("PNG", "png")
            },
            false
        );
        if (paths.Length != 0)
            Apply(paths[0]);
    }

    void Apply(string path)
    {
        if (File.Exists(path))
        {
            Texture2D t = new Texture2D(1, 1);
            if (t.LoadImage(File.ReadAllBytes(path)))
            {
                // load image
                GetComponent<Image>().sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);
                // refresh loaders
                Loader[] loaders = GetComponents<Loader>();
                foreach (Loader l in loaders)
                    l.Refresh();
            }
        }
    }

}