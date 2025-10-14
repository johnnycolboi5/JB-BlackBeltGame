using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DisableFogForCamera : MonoBehaviour
{
    private bool previousFogSetting;

    void OnPreRender()
    {
        previousFogSetting = RenderSettings.fog;
        RenderSettings.fog = false;  // Turn off fog for this camera
    }

    void OnPostRender()
    {
        RenderSettings.fog = previousFogSetting;  // Restore fog for the main world
    }
}
