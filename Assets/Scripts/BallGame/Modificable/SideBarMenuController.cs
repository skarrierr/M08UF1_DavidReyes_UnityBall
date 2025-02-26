using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SidebarMenuController : MonoBehaviour
{
    public RectTransform menuPanel;

    public float slideDuration = 0.3f;

    public GameObject optionsTab;

    public GameObject fillerTab;

    public bool isOpen = false;
    private Coroutine currentCoroutine;
    private Vector2 openPosition;
    private Vector2 closedPosition;

    private void Start()
    {
        openPosition = menuPanel.anchoredPosition;
        closedPosition = new Vector2(-menuPanel.rect.width, menuPanel.anchoredPosition.y);
   
        menuPanel.anchoredPosition = closedPosition;
    }


    public void ToggleMenu()
    {
        if (isOpen)
            SlideOut();
        else
            SlideIn();
    }


    public void SlideIn()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(Slide(menuPanel.anchoredPosition, openPosition, slideDuration));
        isOpen = true;
    }

   
    public void SlideOut()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(Slide(menuPanel.anchoredPosition, closedPosition, slideDuration));
        isOpen = false;
    }

  
    private IEnumerator Slide(Vector2 from, Vector2 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            menuPanel.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
        menuPanel.anchoredPosition = to;
    }

    public void SetMenuWidth(float width)
    {
        Vector2 size = menuPanel.sizeDelta;
        size.x = width;
        menuPanel.sizeDelta = size;
        closedPosition = new Vector2(-width, menuPanel.anchoredPosition.y);
        if (!isOpen)
            menuPanel.anchoredPosition = closedPosition;
    }

    public void ShowOptionsTab()
    {
        if (optionsTab != null)
            optionsTab.SetActive(true);
        if (fillerTab != null)
            fillerTab.SetActive(false);
    }


    public void ShowFillerTab()
    {
        if (optionsTab != null)
            optionsTab.SetActive(false);
        if (fillerTab != null)
            fillerTab.SetActive(true);
    }

  
}
