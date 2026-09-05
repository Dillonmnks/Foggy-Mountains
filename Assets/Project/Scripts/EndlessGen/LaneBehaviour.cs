using UnityEngine;

public class LaneBehaviour : MonoBehaviour
{
    [SerializeField] private float destroyZ = -10f;

    void Update()
    {
        float speed = LaneManager.Instance.speed;
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < destroyZ)
        {
            if (EndlessLaneGenerator.Instance.listNotQueue.Contains(gameObject))
            {
                EndlessLaneGenerator.Instance.listNotQueue.Remove(gameObject);
                Destroy(gameObject);
            }
                
        }
            
    }
}