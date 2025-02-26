using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
   
    public Slider uiScaleSlider;
    public InputField uiScaleInput;
   
    public Button uiScalePlusButton;
    public Button uiScaleMinusButton;
   
    public CanvasScaler canvasScaler;

   
    public Slider safeAreaHSlider;
    public InputField safeAreaHInput;
    
    public Button safeAreaHPlusButton;
    public Button safeAreaHMinusButton;

    
    public Slider safeAreaVSlider;
    public InputField safeAreaVInput;
    
    public Button safeAreaVPlusButton;
    public Button safeAreaVMinusButton;

    public RectTransform safeAreaPanel;

   
    public Slider cameraSensitivitySlider;
    public InputField cameraSensitivityInput;
    
    public Button cameraSensitivityPlusButton;
    public Button cameraSensitivityMinusButton;
    
    public Toggle invertCameraToggle;

   
    public Toggle recenterJoystickToggle;

    

    private void Start()
    {
        
        uiScaleSlider.value = GameSettings.uiScale;
        uiScaleInput.text = GameSettings.uiScale.ToString("F2");

        safeAreaHSlider.value = GameSettings.safeAreaHorizontal;
        safeAreaHInput.text = GameSettings.safeAreaHorizontal.ToString("F2");

        safeAreaVSlider.value = GameSettings.safeAreaVertical;
        safeAreaVInput.text = GameSettings.safeAreaVertical.ToString("F2");

        cameraSensitivitySlider.value = GameSettings.cameraSensitivity;
        cameraSensitivityInput.text = GameSettings.cameraSensitivity.ToString("F2");

        invertCameraToggle.isOn = GameSettings.invertCamera;
        recenterJoystickToggle.isOn = GameSettings.recenterJoystick;

        
        uiScaleSlider.onValueChanged.AddListener(OnUIScaleChanged);
        safeAreaHSlider.onValueChanged.AddListener(OnSafeAreaHChanged);
        safeAreaVSlider.onValueChanged.AddListener(OnSafeAreaVChanged);
        cameraSensitivitySlider.onValueChanged.AddListener(OnCameraSensitivityChanged);

       
        uiScaleInput.onEndEdit.AddListener(OnUIScaleInputEndEdit);
        safeAreaHInput.onEndEdit.AddListener(OnSafeAreaHInputEndEdit);
        safeAreaVInput.onEndEdit.AddListener(OnSafeAreaVInputEndEdit);
        cameraSensitivityInput.onEndEdit.AddListener(OnCameraSensitivityInputEndEdit);

        
        uiScalePlusButton.onClick.AddListener(() => AdjustSlider(uiScaleSlider, uiScaleInput, 0.1f));
        uiScaleMinusButton.onClick.AddListener(() => AdjustSlider(uiScaleSlider, uiScaleInput, -0.1f));

        safeAreaHPlusButton.onClick.AddListener(() => AdjustSlider(safeAreaHSlider, safeAreaHInput, 0.01f));
        safeAreaHMinusButton.onClick.AddListener(() => AdjustSlider(safeAreaHSlider, safeAreaHInput, -0.01f));

        safeAreaVPlusButton.onClick.AddListener(() => AdjustSlider(safeAreaVSlider, safeAreaVInput, 0.01f));
        safeAreaVMinusButton.onClick.AddListener(() => AdjustSlider(safeAreaVSlider, safeAreaVInput, -0.01f));

        cameraSensitivityPlusButton.onClick.AddListener(() => AdjustSlider(cameraSensitivitySlider, cameraSensitivityInput, 0.1f));
        cameraSensitivityMinusButton.onClick.AddListener(() => AdjustSlider(cameraSensitivitySlider, cameraSensitivityInput, -0.1f));

        
        invertCameraToggle.onValueChanged.AddListener(OnInvertCameraChanged);
        recenterJoystickToggle.onValueChanged.AddListener(OnRecenterJoystickChanged);

       
        UpdateSafeArea();
    }

    private void OnUIScaleChanged(float value)
    {
        GameSettings.uiScale = value;
        uiScaleInput.text = value.ToString("F2");
        if (canvasScaler != null)
        {
            canvasScaler.scaleFactor = value;
        }
    }
    private void OnUIScaleInputEndEdit(string value)
    {
        if (float.TryParse(value, out float val))
            uiScaleSlider.value = val;
    }

    private void OnSafeAreaHChanged(float value)
    {
        GameSettings.safeAreaHorizontal = value;
        safeAreaHInput.text = value.ToString("F2");
        UpdateSafeArea();
    }
    private void OnSafeAreaHInputEndEdit(string value)
    {
        if (float.TryParse(value, out float val))
            safeAreaHSlider.value = val;
    }

    private void OnSafeAreaVChanged(float value)
    {
        GameSettings.safeAreaVertical = value;
        safeAreaVInput.text = value.ToString("F2");
        UpdateSafeArea();
    }
    private void OnSafeAreaVInputEndEdit(string value)
    {
        if (float.TryParse(value, out float val))
            safeAreaVSlider.value = val;
    }

    private void OnCameraSensitivityChanged(float value)
    {
        GameSettings.cameraSensitivity = value;
        cameraSensitivityInput.text = value.ToString("F2");
    }
    private void OnCameraSensitivityInputEndEdit(string value)
    {
        if (float.TryParse(value, out float val))
            cameraSensitivitySlider.value = val;
    }

    
    private void OnInvertCameraChanged(bool value)
    {
        GameSettings.invertCamera = value;
    }
    private void OnRecenterJoystickChanged(bool value)
    {
        GameSettings.recenterJoystick = value;
    }

    private void AdjustSlider(Slider slider, InputField inputField, float delta)
    {
        slider.value += delta;
        inputField.text = slider.value.ToString("F2");
    }

    private void UpdateSafeArea()
    {
        if (safeAreaPanel == null)
            return;

        Rect safeAreaRect = Screen.safeArea;

        Vector2 anchorMin = new Vector2(safeAreaRect.x / Screen.width, safeAreaRect.y / Screen.height);
        Vector2 anchorMax = new Vector2((safeAreaRect.x + safeAreaRect.width) / Screen.width,
                                        (safeAreaRect.y + safeAreaRect.height) / Screen.height);

        anchorMin.x = Mathf.Clamp01(anchorMin.x + GameSettings.safeAreaHorizontal);
        anchorMin.y = Mathf.Clamp01(anchorMin.y + GameSettings.safeAreaVertical);
        anchorMax.x = Mathf.Clamp01(anchorMax.x - GameSettings.safeAreaHorizontal);
        anchorMax.y = Mathf.Clamp01(anchorMax.y - GameSettings.safeAreaVertical);

        safeAreaPanel.anchorMin = anchorMin;
        safeAreaPanel.anchorMax = anchorMax;
    }
}
