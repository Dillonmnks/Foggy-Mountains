using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Stats")]
    public static string Name { get; private set; }
    public static int Points { get; private set; }

    [Header("Skills")]
    public static float ClearSpeed { get; private set; }
    public static int MaxLives { get; private set; }
    public static int Lives { get; private set; }
    public static float Acceleration {  get; private set; }

    public static event Action<Collider> OnObstacleCollision;
    public static event Action<Collider> OnCollect;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public static void SetName(string name) => Name = name;

    public static void SetPoints(int points) => Points = points;

    public static void AddPoints(int points) => Points += points;

    public static void RemovePoints(int points) => Points = Mathf.Max(Points - points, 0);

    public static void SetClearSpeed(float speed) => ClearSpeed = speed;

    public static void SetMaxLives(int amount) => MaxLives = amount;

    public static void AddMaxLives(int amount) => MaxLives += amount;

    public static void RemoveMaxLives(int amount) => MaxLives = Mathf.Max(MaxLives - amount, 0);

    public static void SetLives(int amount) => Lives = amount;

    public static void AddLives(int amount) => Lives = Mathf.Min(Lives + amount, MaxLives);

    public static void RemoveLives(int amount) => Lives = Mathf.Max(Lives - amount, 0);

    //Add/Remove 1 of each
    public static void AddLife() => Lives = Mathf.Min(Lives + 1, MaxLives);
    public static void AddMaxLife() => MaxLives++;
    public static void RemoveLife() => Lives = Mathf.Max(Lives - 1, 0);
    public static void RemoveMaxLife() => MaxLives = Mathf.Max(MaxLives - 1, 0);

    public static void SetAcceleration(float amount) => Acceleration = amount;

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
            RemoveLife();

            OnObstacleCollision?.Invoke(other);
        }

        if (isCollectible)
        {
            OnCollect?.Invoke(other);

            if(collectible != null)
            {
                collectible.OnCollect();
            }
        }
    }
}
