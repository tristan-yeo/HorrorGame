using UnityEngine;
using UnityEngine.UI;

public class CameraBattery : MonoBehaviour
{
    [Header("Battery UI")]
    [SerializeField] private RawImage batteryDisplay;
    [SerializeField] private Texture battery100; 
    [SerializeField] private Texture battery75;   
    [SerializeField] private Texture battery50;   
    [SerializeField] private Texture battery25;   
    [SerializeField] private Texture battery0;    

    [Header("Drain Settings")]
    [SerializeField] private float drainInterval = 5f;
    [SerializeField] private int drainAmount = 1;

    [Header("Starting Level")]
    [SerializeField] private int batteryPercentage = 100;

    [Header("Camera Control")]
    [SerializeField] private ToggleCRT toggleCRT;

    private float timer;

    void Awake()
    {
        if (toggleCRT == null)
        {
            toggleCRT = FindObjectOfType<ToggleCRT>();
            if (toggleCRT == null)
                Debug.LogError("CameraBattery: No ToggleCRT found in scene!");
        }
    }

    void Start()
    {
        UpdateBatteryDisplay();
    }

    void Update()
    {
        // 1) Drain only while camera is ON and battery remains
        if (toggleCRT != null && toggleCRT.IsCameraOn() && batteryPercentage > 0)
        {
            timer += Time.deltaTime;
            if (timer >= drainInterval)
            {
                timer -= drainInterval;
                batteryPercentage = Mathf.Max(0, batteryPercentage - drainAmount);

                // 2) The moment we hit zero, kill both the filter & the UI
                if (batteryPercentage == 0)
                    toggleCRT.DisableCamera();
            }
        }

        // 3) Always refresh the icon to match current percentage
        UpdateBatteryDisplay();
    }

    private void UpdateBatteryDisplay()
    {
        if (batteryPercentage > 75) batteryDisplay.texture = battery100;
        else if (batteryPercentage > 50) batteryDisplay.texture = battery75;
        else if (batteryPercentage > 25) batteryDisplay.texture = battery50;
        else if (batteryPercentage > 0) batteryDisplay.texture = battery25;
        else batteryDisplay.texture = battery0;
    }

    public void RechargeBattery()
    {
        batteryPercentage = 100;
        timer = 0f;
        UpdateBatteryDisplay();

        if (toggleCRT != null)
            toggleCRT.EnableCameraToggle();
    }
}
