using UnityEngine;

public class LaneBehaviour : MonoBehaviour
{
    void FixedUpdate()
    {
        float speed = LaneManager.Instance.speed;
        transform.position += Vector3.back * speed * Time.fixedDeltaTime;
    }
}