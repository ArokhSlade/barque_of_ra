using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NarrativeDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text narrativeText;

    [SerializeField]
    private GameObject narrativeBackground;

    private string[] textElements;

    private bool isDisplayingText = false;
    private int currentTextIndex = 0;

    private float textAlpha = 0;

    [SerializeField]
    private float fadeInSpeed;
    private bool isFadingIn = false;

    [SerializeField]
    private float fadeOutSpeed;
    private bool isFadingOut = false;

    [SerializeField]
    private float textStayTime = 1f;

    void Awake()
    {
        if (narrativeText == null) 
        {
            narrativeText = GetComponentInChildren<TMP_Text>();
            
        }

        if (narrativeText != null) 
        {
            NarrativeTrigger.OnNarrativeStarted += DisplayText;
            NarrativeTrigger.OnNarrativeEnded += StopDisplayingText;
        }

        else
        {
            Destroy(gameObject);
        }
        
    }

    private void DisplayText(string[] narrativeElements)
    {
        if (narrativeElements != null && narrativeElements.Length > 0)
        {
            narrativeBackground.SetActive(true);
            isDisplayingText = true;
            isFadingIn = true;
            textElements = narrativeElements;
            narrativeText.text = textElements[currentTextIndex];
        }
    }

    private void StopDisplayingText()
    {
        narrativeText.text = string.Empty;
        currentTextIndex = 0;
        isDisplayingText = false;
        isFadingIn = false;
        isFadingOut = false;
        textAlpha = 0.0f;
        narrativeText.color = new Color(narrativeText.color.r, narrativeText.color.g, narrativeText.color.b, textAlpha);
        narrativeBackground.SetActive (false);
    }

    private void Update()
    {
        if (isDisplayingText) 
        {
            if (isFadingIn)
            {
                FadeIn();
            }

            if (isFadingOut)
            {
                FadeOut();
            }
        }
    }

    private void FadeIn()
    {
        textAlpha += fadeInSpeed * Time.deltaTime;
        narrativeText.color = new Color(narrativeText.color.r, narrativeText.color.g, narrativeText.color.b, textAlpha);
        if (textAlpha >= 1.0f)
        {
            isFadingIn = false;
            StartCoroutine(WaitTextStayTime());
        }
    }

    private void FadeOut()
    {
        textAlpha -= fadeOutSpeed * Time.deltaTime;
        narrativeText.color = new Color(narrativeText.color.r, narrativeText.color.g, narrativeText.color.b, textAlpha);
        if (textAlpha <= 0.0f)
        {
            isFadingOut = false;
            isFadingIn = true;
            currentTextIndex++;
            if (currentTextIndex < textElements.Length)
            {
                narrativeText.text = textElements[currentTextIndex];
            }

            else
            {
                StopDisplayingText();
            }
        }
    }   

    private void OnDestroy()
    {
        NarrativeTrigger.OnNarrativeStarted -= DisplayText;
        NarrativeTrigger.OnNarrativeEnded -= StopDisplayingText;
    }

    private IEnumerator WaitTextStayTime()
    {
        yield return new WaitForSecondsRealtime(textStayTime);
        isFadingOut = true;
    }
}
