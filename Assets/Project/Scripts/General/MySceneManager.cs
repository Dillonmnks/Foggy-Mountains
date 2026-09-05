using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private static MySceneManager instance;

    [Header("Name of the loading scene")]
    public string loadingSceneName = "LoadingScreenName";

    private string targetScene;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void LoadScene(string sceneName)
    {
        instance.targetScene = sceneName;
        SceneManager.LoadScene(instance.loadingSceneName);
        instance.StartCoroutine(instance.LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);

        op.allowSceneActivation = false;

        while(!op.isDone)
        {
            Debug.Log($"Loading Scene: {op.progress * 100f}%");
            if(op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
