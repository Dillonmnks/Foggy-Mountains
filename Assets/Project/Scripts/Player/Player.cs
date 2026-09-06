using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Stats")]
    public static string Name { get; private set; }

    [Header("Skills")]
    public static float ClearSpeed { get; private set; }
    public static int MaxLives { get; private set; }
    public static int Lives { get; private set; }

    public static event Action OnObstacleCollision;
    public static event Action OnCollect;
    public static event Action OnPlayerDeath;

    public bool Unkillable = false;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Lives = 3;
        MaxLives = 3;
    }

    private void Update()
    {
        Debug.Log("Lives: " + Lives);
    }

    private void OnEnable()
    {
        OnPlayerDeath += SaveScore;

        Name = GameManager.Instance.Name.Trim().Replace("\u200B", "");

        OnObstacleCollision += RemoveLife;

        if(Name == "DZO")
        {
            Name = "FAKE DZO";
        }

        if (Name == "")
            Name = "NO NAME";
    }

    private void OnDisable()
    {
        OnPlayerDeath -= SaveScore;
        OnObstacleCollision -= RemoveLife;
    }

    public static void SetName(string name) => Name = name;

    public static void SetMaxLives(int amount) => MaxLives = amount;

    public static void AddMaxLives(int amount) => MaxLives += amount;

    public static void RemoveMaxLives(int amount) => MaxLives = Mathf.Max(MaxLives - amount, 0);

    public static void SetLives(int amount) => Lives = amount;

    public static void AddLives(int amount) => Lives = Mathf.Min(Lives + amount, MaxLives);

    public static void RemoveLives(int amount) => Lives = Mathf.Max(Lives - amount, 0);

    //Add/Remove 1 of each
    public static void AddLife() => Lives = Mathf.Min(Lives + 1, MaxLives);
    public static void AddMaxLife() => MaxLives++;
    public static void RemoveLife()
    {
        Lives = Mathf.Max(Lives - 1, 0);

        Debug.Log("Player Health: " +  Lives);


        if(Lives <= 0)
            OnPlayerDeath?.Invoke();
    }
    public static void RemoveMaxLife() => MaxLives = Mathf.Max(MaxLives - 1, 1);

    private void OnTriggerEnter(Collider other)
    {
        bool isObstacle = false;
        bool isCollectible = false;

        Collectible collectible = null;

        if (other.tag == "Obstacle")
            isObstacle = true;

        if (other.TryGetComponent<Collectible>(out collectible))
            isCollectible = true;

        if (isObstacle)
        {
            OnObstacleCollision?.Invoke();
        }

        if (isCollectible)
        {
            OnCollect?.Invoke();

            if(collectible != null)
            {
                collectible.OnCollect();
            }
        }
    }

    public void SaveScore()
    {
        Debug.Log("YOU DIED");

        var Scores = ScoreboardStorage.Load();

        Scores.Add(new ScoreEntryData
        {
            Name = Name,
            Score = ScoreManager.Instance.TotalPoints,
            Position = Scores.Count + 1
        });


        Scores.Sort((a, b) => b.Score.CompareTo(a.Score));

        for (int i = 0; i < Scores.Count; i++)
            Scores[i].Position = i + 1;

        ScoreboardStorage.Save(Scores);

        SceneManager.LoadScene("MainMenu");
    }
}
