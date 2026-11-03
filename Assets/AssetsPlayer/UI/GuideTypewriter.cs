using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideTypewriter : MonoBehaviour
{


    public TextMeshProUGUI storyText;
    [TextArea(3, 10)] public string fullStory;
    public float typingSpeed = 0.05f;

    public GameObject guidePanel;      // Panel hiện cốt truyện
    public GameObject showGuidePanel;  // Panel hiện hướng dẫn

    private Coroutine typingCoroutine;
    private bool finishedTyping = false;

    void Start()
    {
        guidePanel.SetActive(false);         // Ẩn ban đầu
        showGuidePanel.SetActive(false);     // Ẩn ban đầu
    }

    public void OpenGuidePanel()
    {
        storyText.text = "";
        guidePanel.SetActive(true);
        showGuidePanel.SetActive(false);
        finishedTyping = false;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char c in fullStory)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        finishedTyping = true;
    }

    public void OnNextClicked()
    {
        if (!finishedTyping)
        {
            StopCoroutine(typingCoroutine);
            storyText.text = fullStory;
            finishedTyping = true;
        }
        else
        {
            guidePanel.SetActive(false);
            showGuidePanel.SetActive(true);
        }
    }
    void OnEnable()
{
    if (storyText != null)
        storyText.text = "";

    finishedTyping = false;

    if (typingCoroutine != null)
        StopCoroutine(typingCoroutine);

    if (!string.IsNullOrEmpty(fullStory))
        typingCoroutine = StartCoroutine(TypeText());
}
}
