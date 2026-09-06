using UnityEngine;
using UnityEngine.UI;

public class ToggleVSynv : MonoBehaviour
{
    private bool vsyncToggle;
    public Image Checkmark;

    private void OnEnable()
    {
        Checkmark.enabled = vsyncToggle;
    }

    public void OnToggleVSync()
    {
        vsyncToggle = !vsyncToggle;

        QualitySettings.vSyncCount = vsyncToggle ? 1 : 0;

        if(!vsyncToggle)
        {
            Application.targetFrameRate = 144;
        }

        else
        {
            Application.targetFrameRate = -1;
        }

        Checkmark.enabled = vsyncToggle;
    }
}
