using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    
    public RectTransform background; 
    public RectTransform handle;     
    public float maxDistance = 100f;   
   
    public bool recenterOnRelease = true;

    private Vector2 inputVector = Vector2.zero;
    private Vector2 originalPosition;

    public Vector2 InputVector => inputVector;

    private void Start()
    {
        if (background == null)
            background = GetComponent<RectTransform>();
        originalPosition = background.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        if (GameSettings.recenterJoystick || recenterOnRelease)
            background.anchoredPosition = originalPosition;
    }

    private void UpdateJoystick(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos);
       
        inputVector = pos / maxDistance;
        inputVector = Vector2.ClampMagnitude(inputVector, 1f);
        
        handle.anchoredPosition = inputVector * maxDistance;

        
        if (!GameSettings.recenterJoystick)
        {
            background.anchoredPosition += pos;
        }
    }
}
