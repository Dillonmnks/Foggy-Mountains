using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource soundFXObjectLooped;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = 0.5f;
    private Dictionary<GameObject, AudioSource> activeSounds = new Dictionary<GameObject, AudioSource>();
    private Dictionary<GameObject, float> activeSoundBaseVolumes = new Dictionary<GameObject, float>();
    private float musicBaseVolume = 1f;
    private float ambientBaseVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (VolumeManager.Instance != null)
            VolumeManager.Instance.OnVolumeChanged += UpdateAllVolumes;
    }

    private void OnDisable()
    {
        if (VolumeManager.Instance != null)
            VolumeManager.Instance.OnVolumeChanged -= UpdateAllVolumes;
    }

    private float GetSFXVolume() => VolumeManager.Instance != null ? VolumeManager.Instance.GetCurrentSFXVolume : 1f;
    private float GetMusicVolume() => VolumeManager.Instance != null ? VolumeManager.Instance.GetCurrentMusicVolume : 1f;
    private float GetAmbienceVolume() => VolumeManager.Instance != null ? VolumeManager.Instance.GetCurrentAmbienceVolume : 1f;

    // Called whenever a slider in VolumeManager changes, so currently playing sounds react immediately
    private void UpdateAllVolumes()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.volume = musicBaseVolume * GetMusicVolume();

        if (ambientSource != null && ambientSource.isPlaying)
            ambientSource.volume = ambientBaseVolume * GetAmbienceVolume();

        foreach (var kvp in activeSoundBaseVolumes)
        {
            if (activeSounds.TryGetValue(kvp.Key, out AudioSource source) && source != null)
                source.volume = kvp.Value * GetSFXVolume();
        }
    }

    // Play music
    public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true)
    {
        if (musicSource == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicBaseVolume = volume;
        StopCoroutine(nameof(FadeMusic));
        StartCoroutine(FadeMusic(clip, volume * GetMusicVolume(), loop));
    }

    // Stop music with fade out
    public void StopMusic()
    {
        if (musicSource == null || !musicSource.isPlaying) return;
        StopCoroutine(nameof(FadeMusic));
        StartCoroutine(FadeMusic(null, 0f, false));
    }

    // Fade coroutine for music
    // kevin smells like fishies
    private IEnumerator FadeMusic(AudioClip newClip, float targetVolume, bool loop)
    {
        float startVolume = musicSource.volume;
        float timer = 0f;

        // Fade out
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        musicSource.Stop();

        if (newClip != null)
        {
            musicSource.clip = newClip;
            musicSource.loop = loop;
            musicSource.Play();
            timer = 0f;

            // Fade in
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
                yield return null;
            }
        }
    }
    // Play one shot sounds 
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume = 1f, float pitchVariation = 0.1f)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume * GetSFXVolume();
        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length / audioSource.pitch);
    }

    public void PlayTrackedSound(AudioClip audioClip, GameObject trackedObject, float volume = 1f)
    {
        if (activeSounds.ContainsKey(trackedObject)) return;
        AudioSource audioSource = Instantiate(soundFXObject, trackedObject.transform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume * GetSFXVolume();
        audioSource.loop = true;
        audioSource.Play();
        activeSounds[trackedObject] = audioSource;
        activeSoundBaseVolumes[trackedObject] = volume;
    }

    public void StopTrackedSound(GameObject trackedObject)
    {
        if (activeSounds.TryGetValue(trackedObject, out AudioSource source))
        {
            Destroy(source.gameObject);
            activeSounds.Remove(trackedObject);
            activeSoundBaseVolumes.Remove(trackedObject);
        }
    }

    public void SwitchAmbient(AudioClip newClip, float targetVolume = 1f)
    {
        if (ambientSource.clip == newClip) return;
        ambientBaseVolume = targetVolume;
        StopCoroutine(nameof(FadeAmbient));
        StartCoroutine(FadeAmbient(newClip, targetVolume * GetAmbienceVolume()));
    }

    public void StopAmbient()
    {
        StopCoroutine(nameof(FadeAmbient));
        StartCoroutine(FadeAmbient(null, 0f));
    }

    private IEnumerator FadeAmbient(AudioClip newClip, float targetVolume)
    {
        float startVolume = ambientSource.volume;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            ambientSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }
        ambientSource.Stop();
        if (newClip != null)
        {
            ambientSource.clip = newClip;
            ambientSource.loop = true;
            ambientSource.Play();
            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                ambientSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
                yield return null;
            }
        }
    }

    [System.Obsolete]
    public void StopAllSounds()
    {
        StopAllCoroutines();
        if (ambientSource != null) { ambientSource.Stop(); ambientSource.clip = null; }
        if (musicSource != null) { musicSource.Stop(); musicSource.clip = null; }
        foreach (var kvp in activeSounds)
            if (kvp.Value != null) Destroy(kvp.Value.gameObject);
        activeSounds.Clear();
        activeSoundBaseVolumes.Clear();
        AudioSource[] allSources = FindObjectsOfType<AudioSource>();
        foreach (var source in allSources)
            if (source != ambientSource && source != musicSource)
            { source.Stop(); Destroy(source.gameObject); }
    }

    private void Update()
    {
        List<GameObject> toRemove = new List<GameObject>();
        foreach (var kvp in activeSounds)
        {
            if (kvp.Key == null || !kvp.Key.activeInHierarchy)
            {
                Destroy(kvp.Value.gameObject);
                toRemove.Add(kvp.Key);
            }
        }
        foreach (var key in toRemove)
        {
            activeSounds.Remove(key);
            activeSoundBaseVolumes.Remove(key);
        }
    }
}