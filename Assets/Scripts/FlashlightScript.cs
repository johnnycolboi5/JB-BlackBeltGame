using UnityEngine;
using UnityEngine.UI;

public class FlashlightScript : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlightLight;     // Assign spotlight here
    public float maxBattery = 100f;
    public float batteryDrainRate = 5f;      // per second when on
    public float batteryRechargeRate = 2f;   // per second when off

    [Header("Flicker Settings")]
    public float flickerThreshold = 20f;     // below this %, flashlight flickers
    public float flickerChance = 0.1f;       // chance to flicker each frame
    public float flickerIntensityMin = 0.3f; // how dim it gets when flickering
    public float flickerIntensityMax = 1f;   // normal brightness

    [Header("UI")]
    public Slider batteryBar; // Assign UI Slider

    private float currentBattery;
    private bool isOn = false;
    private float baseIntensity;

    void Start()
    {
        currentBattery = maxBattery;
        flashlightLight.enabled = false;
        baseIntensity = flashlightLight.intensity;

        if (batteryBar != null)
            batteryBar.value = 1f; // Start full
    }

    void Update()
    {
        HandleInput();
        HandleBattery();
        HandleFlicker();
        UpdateUI();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentBattery > 0f)
        {
            isOn = !isOn;
            flashlightLight.enabled = isOn;
        }
    }

    void HandleBattery()
    {
        if (isOn)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
            if (currentBattery <= 0f)
            {
                currentBattery = 0f;
                flashlightLight.enabled = false;
                isOn = false;
            }
        }
        else
        {
            currentBattery = Mathf.Min(currentBattery + batteryRechargeRate * Time.deltaTime, maxBattery);
        }
    }

    void HandleFlicker()
    {
        if (isOn && currentBattery / maxBattery <= flickerThreshold / 100f)
        {
            // Random chance to flicker each frame
            if (Random.value < flickerChance)
            {
                flashlightLight.intensity = baseIntensity * Random.Range(flickerIntensityMin, flickerIntensityMax);
            }
            else
            {
                flashlightLight.intensity = baseIntensity;
            }
        }
        else
        {
            flashlightLight.intensity = baseIntensity; // normal when not low
        }
    }

    void UpdateUI()
    {
        if (batteryBar != null)
        {
            batteryBar.value = currentBattery / maxBattery;
        }
    }
}
