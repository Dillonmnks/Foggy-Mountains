using UnityEngine;

public class Skruf : Collectible
{
    public override void OnCollect()
    {
        base.OnCollect();

        Player.AddLife();

        Debug.Log("Skruf collected");
    }
}
