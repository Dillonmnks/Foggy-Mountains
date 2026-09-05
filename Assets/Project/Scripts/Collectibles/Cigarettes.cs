using UnityEngine;

public class Cigarettes : Collectible
{
    public override void OnCollect()
    {
        base.OnCollect();

        LaneManager.Instance.speed = LaneManager.Instance.speed * 0.5f;

        Debug.Log("Cigarettes collected");
    }
}
