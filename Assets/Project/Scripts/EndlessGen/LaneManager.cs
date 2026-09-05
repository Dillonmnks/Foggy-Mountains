using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance;
    public static float[] LaneX = { -2.5f, 0f, 2.5f };

    public float speed = 5f;
    public float maxSpeed = 15f;
    public float acceleration = 0.1f;


    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        speed = Mathf.Min(speed + acceleration * Time.deltaTime, maxSpeed);
    }


}