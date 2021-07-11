using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WebLoader : Loader
{
    [System.Serializable]
    public struct field { public string key, value; }
    [Header("Web source")]
    public string encoding = "utf-8";
    public bool postRequest = false;
    public field[] formData; // used only in post requests
    public string startTagOpening = "<";
    public string startTagClosing = ">";
    public string endTagOpening = "</";
    public string endTagClosing = ">";
    public string[] ignoredTags = { };
    public string[] ignoredAttributes = { };
    public string[] parsedTags = { "title", "description", "link" };

    protected List<Dictionary<string, string>> content = new List<Dictionary<string, string>>();

    override protected void LoadCache()
    {
        if (File.Exists(path))
        {
            FileStream cache = File.Open(path, FileMode.Open);
            content = (List<Dictionary<string, string>>)(new BinaryFormatter()).Deserialize(cache);
            cache.Close();
            // print("Cache loaded!");
            OnContentUpdate();
        }
    }

    override protected void SaveCache()
    {
        FileStream cache = File.Create(path);
        (new BinaryFormatter()).Serialize(cache, content);
        cache.Close();
        // print("Cache saved!");
    }

    override protected IEnumerator GetContentRoutine()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnNetworkUnreachable();
            yield break;
        }
        UnityWebRequest www;
        if (postRequest)
        {
            WWWForm form = new WWWForm();
            foreach (field f in formData)
                form.AddField(f.key, f.value);
            www = UnityWebRequest.Post(url, form);
        }
        else
            www = UnityWebRequest.Get(url);
        OnLoading();
        yield return www.SendWebRequest();

        if (www.isNetworkError)
            OnNetworkError();
        else if (www.isHttpError)
            OnHttpError();
        else
        {
            Parse(System.Text.Encoding.GetEncoding(encoding).GetString(www.downloadHandler.data));
            OnContentUpdate();
        };
    }

    void Parse(string code)
    {
        if (parsedTags.Length == 0)
            return;
        content.Clear();
        int parsedTagIndex = 0, a = 0;
        // tags to be ignored
        foreach (string ignoredTag in ignoredTags)
            // ignore tag attributes and values
            if ((a = code.IndexOf(startTagOpening + ignoredTag, a)) == -1
                || (a = code.IndexOf(endTagOpening + ignoredTag + endTagClosing, a)) == -1)
                break;
        Dictionary<string, string> record = new Dictionary<string, string>();
        // searches for the tag whose index is tagIndex from index a
        while ((a = code.IndexOf(startTagOpening + parsedTags[parsedTagIndex], a)) != -1)
        {
            // ignore tag attributes and values: jump to index a2
            int a2 = code.IndexOf(startTagClosing, a + (startTagOpening + parsedTags[parsedTagIndex]).Length);
            if (a2 == -1)
                break;
            // some tags should be ignored because of specific attributes
            bool ignore = false;
            foreach (string ignoredAttribute in ignoredAttributes)
            {
                int ignoredTagIndex = code.IndexOf(ignoredAttribute, a + (startTagOpening + parsedTags[parsedTagIndex]).Length);
                if (ignoredTagIndex != -1 && ignoredTagIndex < a2)
                {
                    ignore = true;
                    break;
                }
            }
            // end tag found at index b (hopefully)
            int b = code.IndexOf(endTagOpening + parsedTags[parsedTagIndex] + endTagClosing, a = a2 + startTagClosing.Length);
            if (b == -1) // end tag not found
                break;
            if (ignore)
            {
                a = b + (endTagOpening + parsedTags[parsedTagIndex] + endTagClosing).Length;
                continue;
            }
            // content update
            string key = parsedTags[parsedTagIndex], originalKey = key;
            for (int i = 2; record.ContainsKey(key); ++i)
                key = originalKey + i;
            record[key] = RemoveMarkup(code.Substring(a, b - a));
            a = b + (endTagOpening + parsedTags[parsedTagIndex] + endTagClosing).Length;
            // parsedTagIndex increment
            if (parsedTagIndex == parsedTags.Length - 1) // new record
            {
                content.Add(record);
                record = new Dictionary<string, string>();
                parsedTagIndex = 0;
            }
            else
                ++parsedTagIndex;
        }
        SaveCache();
    }

    string RemoveMarkup(string s)
    {
        const string OPEN_CDATA = @"\<\!\[CDATA\[",
                     CLOSE_CDATA = @"\]\]\>",
                     HTML_TAG = @"\s*<.*?>\s*",
                     MULTIPLE_SPACES = @"\s+";
        s = Regex.Replace(s, OPEN_CDATA, string.Empty);
        s = Regex.Replace(s, CLOSE_CDATA, string.Empty);
        s = Regex.Replace(s, HTML_TAG, " ");
        s = WebUtility.HtmlDecode(Regex.Replace(s, MULTIPLE_SPACES, " "));
        return s.Trim();
    }

    protected virtual void OnContentUpdate()
    {
        PrintOnLoaderLog(content.Count == 0 ? noDataMessage : string.Empty);
        UseContent();
    }
}