using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void OnEnable()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();

        var resolution = Screen.currentResolution;

        if (resolution.width == 1280 && resolution.height == 720)
        {
            dropdown.value = 0;
        }

        else if (resolution.width == 1920 && resolution.height == 1080)
        {
            dropdown.value = 1;
        }

        else if (resolution.width == 3840 && resolution.height == 2160)
        {
            dropdown.value = 2;
        }

        else
        {
            dropdown.value = 1;
            OnSetResolution();
        }
    }

    public void OnSetResolution()
    {
        if(dropdown == null)
        {
            Debug.LogError("Resolution dropdown was null");
            return;
        }

        switch(dropdown.value)
        {
            case 0:
                //1280x720
                Screen.SetResolution(1280, 720, VideoManager.IsFullscreen);
                break;
            case 1:
                //1920x1080
                Screen.SetResolution(1920, 1080, VideoManager.IsFullscreen);
                break;
            case 2:
                //3840x2160
                Screen.SetResolution(3840, 2160, VideoManager.IsFullscreen);
                break;
        }
    }
}
