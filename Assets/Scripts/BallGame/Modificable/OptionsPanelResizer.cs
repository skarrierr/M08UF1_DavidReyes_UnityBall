using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class OptionsPanelResizer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
 
    public RectTransform optionsPanel;

    public float minWidth = 200f;
    public float maxWidth = 600f;

    private float initialWidth;
    private Vector2 initialPointerPos;

   
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (optionsPanel == null)
        {
            Debug.LogError("OptionsPanelResizer: No se ha asignado el OptionsPanel.");
            return;
        }
        initialWidth = optionsPanel.rect.width;
        initialPointerPos = eventData.position;
    }

   
    public void OnDrag(PointerEventData eventData)
    {
        if (optionsPanel == null)
            return;

        float deltaX = eventData.position.x - initialPointerPos.x;
        float newWidth = Mathf.Clamp(initialWidth + deltaX, minWidth, maxWidth);
        optionsPanel.sizeDelta = new Vector2(newWidth, optionsPanel.sizeDelta.y);
    }

   
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Panel de opciones redimensionado a: " + optionsPanel.rect.width + " píxeles.");
    }
}
