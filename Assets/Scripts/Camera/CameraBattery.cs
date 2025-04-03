using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class CameraBattery : MonoBehaviour
{
    [Header("Battery UI")]
    [SerializeField]
    private RawImage batteryDisplay; // Assign your UI RawImage that shows the battery

    [SerializeField]
    private Texture battery100;
    [SerializeField]
    private Texture battery75;
    [SerializeField]
    private Texture battery50;
    [SerializeField]
    private Texture battery25;
    [SerializeField]
    private Texture battery0;

    [Header("Battery Settings")]
    [SerializeField]
    private float drainInterval = 5f; // seconds between each drain
    [SerializeField]
    private int drainAmount = 1; // drain percentage per interval

    [SerializeField]
    private int batteryPercentage = 100;

    [Header("Camera Reference")]
    [SerializeField]
    private ToggleCRT toggleCRT; // Reference to the ToggleCRT script

    private float timer = 0f;

    void Start()
    {
        UpdateBatteryDisplay();
    }

    void Update()
    {
        // Drain battery only if the camera is on.
        if (toggleCRT != null && !toggleCRT.IsCameraOn())
            return;

        // Only drain if battery isn't empty
        if (batteryPercentage > 0)
        {
            timer += Time.deltaTime;
            if (timer >= drainInterval)
            {
                timer = 0f;
                batteryPercentage -= drainAmount;
                if (batteryPercentage < 0)
                {
                    batteryPercentage = 0;
                }
                UpdateBatteryDisplay();

                // When battery reaches 0%, disable the camera permanently
                if (batteryPercentage == 0)
                {
                    if (toggleCRT != null)
                    {
                        toggleCRT.DisableCamera();
                    }
                    else
                    {
                        Debug.LogWarning("ToggleCRT reference is missing!");
                    }
                }
            }
        }
    }

    void UpdateBatteryDisplay()
    {
        if (batteryPercentage > 90)
        {
            batteryDisplay.texture = battery100;
        }
        else if (batteryPercentage > 50)
        {
            batteryDisplay.texture = battery75;
        }
        else if (batteryPercentage > 25)
        {
            batteryDisplay.texture = battery50;
        }
        else if (batteryPercentage > 5)
        {
            batteryDisplay.texture = battery25;
        }
        else // battery below 25% (including 0%)
        {
            batteryDisplay.texture = battery0;
        }
    }

    // Call this method when the user picks up a battery to recharge it
    public void RechargeBattery()
    {
        batteryPercentage = 100;
        UpdateBatteryDisplay();

        // Re-enable camera toggling after recharging
        if (toggleCRT != null)
        {
            toggleCRT.EnableCameraToggle();
        }
    }
}
