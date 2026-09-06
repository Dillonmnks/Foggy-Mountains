using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public ScoreDisplay scoreDisplay;

    public float pointsPerMeter = 1f;
    public float distanceTickInterval = 0.5f;

    public float DistanceTravelled { get; private set; }
    public int TotalPoints { get; private set; }

    private float distanceSinceLastTick;
    private float tickTimer;
    private int nextMilestone = 100;

    public float minSpeed = 20f;
    public float maxSpeed = 100f;
    public float maxSpeedMultiplier = 3f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (LaneManager.Instance == null) return;

        float delta = LaneManager.Instance.speed * Time.deltaTime;
        DistanceTravelled += delta;
        distanceSinceLastTick += delta;

        tickTimer += Time.deltaTime;
        if (tickTimer >= distanceTickInterval)
        {
            tickTimer = 0f;
            AwardDistanceTick();
        }
    }

    private void AwardDistanceTick()
    {
        float speedT = Mathf.InverseLerp(minSpeed, maxSpeed, LaneManager.Instance.speed);
        float multiplier = Mathf.Lerp(1f, maxSpeedMultiplier, speedT);
        float effectiveRate = pointsPerMeter * multiplier;

        int points = Mathf.FloorToInt(distanceSinceLastTick * effectiveRate);
        if (points > 0)
        {
            distanceSinceLastTick -= points / effectiveRate;
            AddPoints(points);
        }
    }

    public void AddPoints(int amount)
    {
        TotalPoints += amount;

        if (scoreDisplay != null)
        {
            scoreDisplay.AddScore(amount);

            while (TotalPoints >= nextMilestone)
            {
                scoreDisplay.PlayMilestoneEffect();
                nextMilestone *= 10;
            }
        }
    }
}