using UnityEngine;
using UnityEngine.UI;

public class FlashlightScript : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light hotspotLight;     
    public Light throwLight;         
    public float maxBattery = 100f;
    public float batteryDrainRate = 5f;    
    public float batteryRechargeRate = 2f;  

    [Header("Flicker Settings")]
    public float flickerThreshold = 20f;   
    public float flickerChance = 0.1f;     
    public float flickerIntensityMin = 0.3f;
    public float flickerIntensityMax = 1f;  

    [Header("UI")]
    public Slider batteryBar;

    private float currentBattery;
    private bool isOn = false;

    private float baseHotspotIntensity;
    private float baseThrowIntensity;

    void Start()
    {
        currentBattery = maxBattery;

     
        if (hotspotLight != null)
        {
            baseHotspotIntensity = hotspotLight.intensity;
            hotspotLight.enabled = false;
        }

        if (throwLight != null)
        {
            baseThrowIntensity = throwLight.intensity;
            throwLight.enabled = false;
        }

        if (batteryBar != null)
            batteryBar.value = 1f; 
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
            SetFlashlightState(isOn);
        }
    }

    void SetFlashlightState(bool state)
    {
        if (hotspotLight != null) hotspotLight.enabled = state;
        if (throwLight != null) throwLight.enabled = state;
    }

    void HandleBattery()
    {
        if (isOn)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
            if (currentBattery <= 0f)
            {
                currentBattery = 0f;
                isOn = false;
                SetFlashlightState(false);
            }
        }
        else
        {
            currentBattery = Mathf.Min(currentBattery + batteryRechargeRate * Time.deltaTime, maxBattery);
        }
    }

    void HandleFlicker()
    {
        if (!isOn)
        {
            ResetIntensities();
            return;
        }

        if (currentBattery / maxBattery <= flickerThreshold / 100f)
        {
           
            if (Random.value < flickerChance)
            {
                float hotspotFlicker = baseHotspotIntensity * Random.Range(flickerIntensityMin, flickerIntensityMax);
                float throwFlicker = baseThrowIntensity * Random.Range(flickerIntensityMin, flickerIntensityMax);

                if (hotspotLight != null) hotspotLight.intensity = hotspotFlicker;
                if (throwLight != null) throwLight.intensity = throwFlicker;
                return;
            }
        }

        ResetIntensities();
    }

    void ResetIntensities()
    {
        if (hotspotLight != null) hotspotLight.intensity = baseHotspotIntensity;
        if (throwLight != null) throwLight.intensity = baseThrowIntensity;
    }

    void UpdateUI()
    {
        if (batteryBar != null)
            batteryBar.value = currentBattery / maxBattery;
    }
}
