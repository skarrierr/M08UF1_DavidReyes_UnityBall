using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIIndicator : MonoBehaviour
{
    [Header("Referencia del objeto a seguir")]
    [Tooltip("Objeto del mundo a seguir (pelota u objetivo).")]
    public Transform targetTransform;
    [Tooltip("Cámara usada en la escena.")]
    public Camera cam;
    [Tooltip("Canvas en el que se ubica este elemento UI.")]
    public Canvas canvas;
    [Tooltip("Marca 'true' para la pelota, 'false' para el objetivo.")]
    public bool isBall = true;

    [Header("Valores de Alfa para la Pelota")]
    [Tooltip("Alpha cuando la pelota está visible (sin obstrucción).")]
    public float ballVisibleAlpha = 0f;
    [Tooltip("Alpha cuando la pelota está oculta (detrás de un objeto).")]
    public float ballOccludedAlpha = 0.5f;
    [Tooltip("Alpha cuando la pelota está fuera del área de cámara.")]
    public float ballOffscreenAlpha = 1f;

    [Header("Valores de Alfa para el Objetivo")]
    [Tooltip("Alpha cuando el objetivo está visible.")]
    public float targetVisibleAlpha = 0.5f;
    [Tooltip("Alpha cuando el objetivo está oculto (detrás de un objeto).")]
    public float targetOccludedAlpha = 0.75f;
    [Tooltip("Alpha cuando el objetivo está fuera de la pantalla.")]
    public float targetOffscreenAlpha = 1f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (cam == null)
            cam = Camera.main;
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        Canvas.willRenderCanvases += UpdateIndicator;
    }

    private void OnDisable()
    {
        Canvas.willRenderCanvases -= UpdateIndicator;
    }

    private void UpdateIndicator()
    {
        if (targetTransform == null || cam == null)
            return;

        // Convertir la posición del objeto a coordenadas de viewport
        Vector3 viewportPos = cam.WorldToViewportPoint(targetTransform.position);
        bool isBehind = viewportPos.z < 0;
        bool onScreen = (!isBehind &&
                         viewportPos.x >= 0 && viewportPos.x <= 1 &&
                         viewportPos.y >= 0 && viewportPos.y <= 1);

        // Detectar si el objeto está oculto usando un raycast
        bool occluded = false;
        Vector3 direction = (targetTransform.position - cam.transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, direction, out hit))
        {
            if (hit.transform != targetTransform)
                occluded = true;
        }

        // Asignar el valor alfa según el estado y el tipo de objeto
        float indicatorAlpha = 1f;
        if (isBall)
        {
            if (onScreen)
                indicatorAlpha = occluded ? ballOccludedAlpha : ballVisibleAlpha;
            else
                indicatorAlpha = ballOffscreenAlpha;
        }
        else // indicador para el objetivo
        {
            if (onScreen)
                indicatorAlpha = occluded ? targetOccludedAlpha : targetVisibleAlpha;
            else
                indicatorAlpha = targetOffscreenAlpha;
        }
        canvasGroup.alpha = indicatorAlpha;

        // Calcular la posición en pantalla (en píxeles)
        Vector3 screenPos = cam.WorldToScreenPoint(targetTransform.position);

        // Para el objetivo, si no está en pantalla o se encuentra "muy lejos", lo posicionamos en el borde
        if (!onScreen)
        {
            if (isBall)
            {
                // Para la pelota, la mostramos en el centro de la pantalla cuando esté fuera
                screenPos = new Vector3(Screen.width / 2, Screen.height / 2, screenPos.z);
            }
            else
            {
                // Para el objetivo, se clampa la posición a los bordes de la pantalla con un margen
                float margin = 50f; // margen en píxeles
                float clampedX = Mathf.Clamp(screenPos.x, margin, Screen.width - margin);
                float clampedY = Mathf.Clamp(screenPos.y, margin, Screen.height - margin);
                screenPos = new Vector3(clampedX, clampedY, screenPos.z);
            }
        }

        // Convertir la posición de pantalla a coordenadas locales del Canvas
        Vector2 canvasPos;
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam, out canvasPos);
        rectTransform.anchoredPosition = canvasPos;
    }
}
