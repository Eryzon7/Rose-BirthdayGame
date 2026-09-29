using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public TMP_Text characterName;
    public TMP_Text dialogueText;
    public Image characterIcon;

    public void SetLine(string name, Sprite icon, string text)
    {
        characterName.text = name;
        characterIcon.sprite = icon;
        dialogueText.text = text;

        if (name == "Rose")
        {
            characterIcon.rectTransform.anchoredPosition = new Vector2(-700, characterIcon.rectTransform.anchoredPosition.y);
            characterName.rectTransform.anchoredPosition = new Vector2(-470, characterName.rectTransform.anchoredPosition.y);
        }
        else
        {
            characterIcon.rectTransform.anchoredPosition = new Vector2(700, characterIcon.rectTransform.anchoredPosition.y);
            characterName.rectTransform.anchoredPosition = new Vector2(470, characterName.rectTransform.anchoredPosition.y);
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}