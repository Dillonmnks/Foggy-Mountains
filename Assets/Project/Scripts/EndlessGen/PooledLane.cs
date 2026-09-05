using UnityEngine;

public class PooledLane : MonoBehaviour
{
    public void Activate(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void ClearObstacles()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
