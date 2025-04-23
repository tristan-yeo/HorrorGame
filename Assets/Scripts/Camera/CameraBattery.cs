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
    
    [Header("Audio")]
    [SerializeField] private AudioSource lowBatteryWarningSound;
    private bool lowBatteryWarningPlayed = false;

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
        
        // 4) Check for low battery and play warning sound if needed
        CheckLowBattery();
    }

    private void UpdateBatteryDisplay()
    {
        if (batteryPercentage > 75) batteryDisplay.texture = battery100;
        else if (batteryPercentage > 50) batteryDisplay.texture = battery75;
        else if (batteryPercentage > 25) batteryDisplay.texture = battery50;
        else if (batteryPercentage > 0) batteryDisplay.texture = battery25;
        else batteryDisplay.texture = battery0;
    }
    
    private void CheckLowBattery()
    {
        // Play warning sound when battery drops to 25% or lower
        if (batteryPercentage <= 25 && !lowBatteryWarningPlayed && lowBatteryWarningSound != null)
        {
            lowBatteryWarningSound.Play();
            lowBatteryWarningPlayed = true;
            Debug.Log("Low battery warning played at " + batteryPercentage + "%");
        }
    }

    public void RechargeBattery()
    {
        batteryPercentage = 100;
        timer = 0f;
        lowBatteryWarningPlayed = false; // Reset warning flag when battery is recharged
        UpdateBatteryDisplay();

        if (toggleCRT != null)
            toggleCRT.EnableCameraToggle();
    }
}
