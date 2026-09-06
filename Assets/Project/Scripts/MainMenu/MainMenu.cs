using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    
    private void Awake()
    {
        if(Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartGame()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
