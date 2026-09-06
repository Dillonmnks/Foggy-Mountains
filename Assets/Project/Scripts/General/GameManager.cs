using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string _name;

    public static string Name => Name;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public static void SetName(string name)
    {
        Instance._name = name;
    }
}
