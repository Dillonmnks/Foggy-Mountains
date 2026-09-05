using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private void OnEnable()
    {
        Player.OnPlayerCollision += Wobble;
    }

    private void OnDisable()
    {
        Player.OnPlayerCollision -= Wobble;
    }

    public void Wobble(Collision collision)
    {
        //Apply wobble logic here

        Debug.Log("Camera Wobble called");
    }
}
