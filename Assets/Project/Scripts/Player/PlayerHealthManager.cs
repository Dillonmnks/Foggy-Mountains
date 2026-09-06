using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance;

    public Image[] hearts = new Image[3];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        Player.OnObstacleCollision += HandleObstacleHit;
        
    }
    private void Start()
    {
        RefreshHearts();
    }

    private void OnDisable()
    {
        Player.OnObstacleCollision -= HandleObstacleHit;
    }

    private void HandleObstacleHit()
    {
        RefreshHearts();
    }

    public void RefreshHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i + 1 < Player.Lives;
        }
    }
}