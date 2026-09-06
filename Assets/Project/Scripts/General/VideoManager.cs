using UnityEngine;

public class VideoManager : MonoBehaviour
{
    private static VideoManager instance;

    public static bool IsFullscreen => Screen.fullScreen;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }

    public static void ToggleCheckbox()
    {
        return;
    }
}
