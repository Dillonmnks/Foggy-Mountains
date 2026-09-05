using UnityEngine;

public class RedBull : Collectible
{
    public override void OnCollect()
    {
        base.OnCollect();

        LaneManager.Instance.acceleration += LaneManager.Instance.acceleration * 0.2f;

        Debug.Log("Red Bull Collected");
    }
}
