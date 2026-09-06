using UnityEngine;

public class Skruf : Collectible
{
    public override void OnCollect()
    {
        base.OnCollect();

        ScoreManager.Instance.AddPoints((int)(ScoreManager.Instance.TotalPoints * 0.2f));

        Debug.Log("Skruf collected");
    }
}
