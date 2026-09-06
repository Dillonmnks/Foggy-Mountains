using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    private Button button;
    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(PlaySound);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        Debug.Log("Button pressed");
        SoundFXManager.Instance.PlaySoundFXClip(audioClip, Camera.main.transform);
    }
}
