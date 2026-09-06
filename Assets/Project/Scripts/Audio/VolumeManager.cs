using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    [Range(0f, 1f)][SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float musicVolume = 1f;

    private const string MASTER_KEY = "MasterVolume";
    private const string SFX_KEY = "SFXVolume";
    private const string MUSIC_KEY = "MusicVolume";

    public event System.Action OnVolumeChanged;

    public float GetCurrentMasterVolume => masterVolume;
    public float GetCurrentSFXVolume => masterVolume * sfxVolume;
    public float GetCurrentMusicVolume => masterVolume * musicVolume;

    public float RawSFXVolume => sfxVolume;
    public float RawMusicVolume => musicVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(MASTER_KEY, masterVolume);
        OnVolumeChanged?.Invoke();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(SFX_KEY, sfxVolume);
        OnVolumeChanged?.Invoke();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(MUSIC_KEY, musicVolume);
        OnVolumeChanged?.Invoke();
    }
}