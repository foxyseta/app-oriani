using UnityEngine;

public class ShareButton : MonoBehaviour
{
    [Multiline]
    [SerializeField]
    string subjectFormat = "{0}",
           textFormat = "{0}\n{1}\n{2}";
    [SerializeField]
    string title = "Share with";
    
    NativeShare nativeShare = new NativeShare();

    public void SetArticle(string header = null, string description = null, string link = null)
    {
        nativeShare.SetSubject(string.Format(subjectFormat, header))
                   .SetText(string.Format(textFormat, header, description, link))
                   .SetTitle(title);
    }

    public void OnClick()
    {
        nativeShare.Share();
    }

}
