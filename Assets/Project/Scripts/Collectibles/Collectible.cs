using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public float SpawnWeight;

    public virtual void OnCollect()
    {
        Destroy(gameObject);
    }
}