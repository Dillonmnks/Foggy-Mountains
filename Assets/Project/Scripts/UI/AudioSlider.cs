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
        switch (ModifyVolume)
        {
            case Volume.Master:
                Slider.value = VolumeManager.Instance.GetCurrentMasterVolume * 100f;
                break;
            case Volume.SFX:
                Slider.value = VolumeManager.Instance.RawSFXVolume * 100f;
                break;
            case Volume.Music:
                Slider.value = VolumeManager.Instance.RawMusicVolume * 100f;
                break;
            case Volume.Ambience:
                Slider.value = VolumeManager.Instance.RawAmbienceVolume * 100f;
                break;
        }

        UpdatePreviewText();
    }

    public void UpdatePreviewText()
    {
        string text = Slider.value.ToString() + "%";
        AmountPreview.text = text;

        UpdateVolume();
    }

    public void UpdateVolume()
    {
        switch (ModifyVolume)
        {
            case Volume.Master:
                VolumeManager.Instance.SetMasterVolume(Slider.value * 0.01f);
                break;
            case Volume.SFX:
                VolumeManager.Instance.SetSFXVolume(Slider.value * 0.01f);
                break;
            case Volume.Music:
                VolumeManager.Instance.SetMusicVolume(Slider.value * 0.01f);
                break;
            case Volume.Ambience:
                VolumeManager.Instance.SetAmbienceVolume(Slider.value * 0.01f);
                break;
        }
    }
}
