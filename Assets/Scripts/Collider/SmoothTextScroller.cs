using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmoothTextScroller : MonoBehaviour
{
    [Header("Setup")]
    public RectTransform contentArea; // The Content of ScrollRect
    public TMP_Text linePrefab;

    [Header("Settings")]
    public int maxLines = 5;
    public float fadeDuration = 0.25f;
    public float scrollSpeed = 10f; // speed to scroll to bottom

    private readonly List<TMP_Text> activeLines = new List<TMP_Text>();
    private ScrollRect scrollRect;

    void Awake()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
        if (scrollRect == null)
            Debug.LogWarning("SmoothTextScroller: ScrollRect not found in parent.");
    }

    public void SetFirstlineText(string text)
    {
        if (activeLines!=null && activeLines.Count>0)
        {
            TMP_Text oldest = activeLines[0];
            oldest.text = text;
        }
        else
        {
            TMP_Text newLine = Instantiate(linePrefab, contentArea);
            newLine.gameObject.SetActive(true);
            newLine.text = text;
            newLine.alpha = 0.0f;
            activeLines.Add(newLine);

            // Fade in
            StartCoroutine(FadeText(newLine, 0f, 1f, fadeDuration));
        }
    }

    //public float fadedestroydy

    public void AddLine(string text)
    {
        AddLine(text, -1);
    }

    public void AddLine(string text,float lifetime)
    {
        // Instantiate new line
        TMP_Text newLine = Instantiate(linePrefab, contentArea);
        if (lifetime>0)
        {
            //Destroy(newLine.gameObject, lifetime);

            StartCoroutine(DestroyDelay(newLine, lifetime));
        }
        newLine.gameObject.SetActive(true);
        newLine.text = text;
        newLine.alpha = 0.0f;
        activeLines.Add(newLine);

        // Fade in
        StartCoroutine(FadeText(newLine, 0f, 1f, fadeDuration));

        // Remove oldest if more than maxLines
        while (activeLines.Count > maxLines)
        {
            TMP_Text oldest = activeLines[0];
            activeLines.RemoveAt(0);
           // StartCoroutine(FadeAndDestroy(oldest, fadeDuration));
            Destroy(oldest);
        }
       // newLine.gameObject.SetActive(true);

        // Force layout update so Content Size Fitter recalculates size
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentArea);

        // Smooth scroll to bottom
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        //unscaledDeltaTime
        float target = 0f; // ScrollRect.verticalNormalizedPosition = 0 (bottom)
        while (Mathf.Abs(scrollRect.verticalNormalizedPosition - target) > 0.001f)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(
                scrollRect.verticalNormalizedPosition, target, Time.unscaledDeltaTime * scrollSpeed);
            yield return null;
        }
        scrollRect.verticalNormalizedPosition = target; // snap exactly
    }


    private IEnumerator FadeText(TMP_Text text, float from, float to, float duration)
    {
        if (text == null) yield break;

        float elapsed = 0f;
        text.alpha = from;

        while (elapsed < duration)
        {
            if (text == null) yield break; // prevent errors if destroyed early
            elapsed += Time.unscaledDeltaTime;
            text.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }

        if (text != null) text.alpha = to;
    }

    private IEnumerator FadeAndDestroy(TMP_Text text, float duration)
    {
        if (text == null) yield break;

        yield return FadeText(text, text.alpha, 0f, duration);

        if (text != null)
        {
            Destroy(text.gameObject);
        }
    }

    public IEnumerator DestroyDelay(TMP_Text text, float delay, float fadeDuration = 1.0f)
    {
        if (text == null) yield break;

        yield return new WaitForSeconds(delay);
        yield return FadeAndDestroy(text, fadeDuration);
    }



}
