using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public enum Volume
    {
        Master,
        SFX,
        Music,
        Ambience
    }

    public TMP_Text AmountPreview;
    public Slider Slider;

    public Volume ModifyVolume;

    private void OnEnable()
    {
        UpdatePreviewText();
    }

    public void UpdatePreviewText()
    {
        string text = Slider.value.ToString() + "%";
        AmountPreview.text = text;
    }

    public void UpdateVolume()
    {
        switch (ModifyVolume)
        {
            case Volume.Master:
                //audiomanager.setMasterVolume(slider.value)
                break;
            case Volume.SFX:
                break;
            case Volume.Music:
                break;
            case Volume.Ambience:
                break;
        }
    }
}
