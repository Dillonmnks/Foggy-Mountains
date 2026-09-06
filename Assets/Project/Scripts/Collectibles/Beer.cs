using UnityEngine;

public class Beer : Collectible
{
    public override void OnCollect()
    {
        base.OnCollect();

        Player.AddMaxLife();

        Debug.Log("Beer collected");
    }
}
