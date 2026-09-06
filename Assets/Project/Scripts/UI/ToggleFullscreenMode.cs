using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ToggleFullscreenMode : MonoBehaviour
{
    public Image Checkmark;

    private void OnEnable()
    {
        Checkmark.enabled = Screen.fullScreen;
    }

    private void Update()
    {
        Checkmark.enabled = Screen.fullScreen;
    }

    public void OnToggleFullscreenMode()
    {
        Screen.fullScreen = !Screen.fullScreen;

        Checkmark.enabled = Screen.fullScreen;
    }
}
