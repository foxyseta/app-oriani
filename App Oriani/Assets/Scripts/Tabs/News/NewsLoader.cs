using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsLoader : WebLoader
{
    [Header("Tags used")]
    public string titleTag = "title";
    public string descriptionTag = "description";
    public string linkTag = "link";
    [Header("UI")]
    public GameObject newsContainer, articlePrefab;
    public int titleChildIndex = 0,
               descriptionChildIndex = 1;
    public string noDescriptionMessage = "No description available.",
                  descriptionSubstringFrom = "<br>",
                  descriptionSubstringTo = " <span class=\"testo_molto_piccolo text-muted\">";


    protected override void UseContent()
    {
        foreach (Transform child in newsContainer.transform)
            Destroy(child.gameObject);
        foreach (Dictionary<string, string> article in content)
        {
            GameObject articleObject = Instantiate(articlePrefab, newsContainer.transform);
            Transform articleTransform = articleObject.transform;
            // Title
            articleTransform.GetChild(titleChildIndex).gameObject.GetComponent<Text>().text = article[titleTag];
            // Description
            int from = article[descriptionTag].IndexOf(descriptionSubstringFrom),
                to = article[descriptionTag].IndexOf(descriptionSubstringTo);
            string description = articleTransform.GetChild(descriptionChildIndex).gameObject.GetComponent<Text>().text =
                from == -1 || to == -1 || from + descriptionSubstringFrom.Length == to ?
                noDescriptionMessage :
                article[descriptionTag].Substring(from + descriptionSubstringFrom.Length, to - from - descriptionSubstringFrom.Length);
            // Link
            articleObject.GetComponent<URLButton>().url = article[linkTag];
            // Share Button
            ShareButton shareButton = articleObject.GetComponent<ShareButton>();
            if (shareButton)
                shareButton.SetArticle(article[titleTag], description, article[linkTag]);
        }
    }
}
