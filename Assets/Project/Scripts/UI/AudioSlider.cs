using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public TMP_Text AmountPreview;
    public Slider Slider;

    private void OnEnable()
    {
        UpdatePreviewText();
    }

    public void UpdatePreviewText()
    {
        string text = Slider.value.ToString() + "%";
        AmountPreview.text = text;
    }
}
