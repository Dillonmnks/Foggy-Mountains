using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private void OnEnable()
    {
        Player.OnObstacleCollision += Wobble;
    }

    private void OnDisable()
    {
        Player.OnObstacleCollision -= Wobble;
    }

    public void Wobble(Collider other)
    {
        //Apply wobble logic here

        Debug.Log("Camera Wobble called");
    }
}
