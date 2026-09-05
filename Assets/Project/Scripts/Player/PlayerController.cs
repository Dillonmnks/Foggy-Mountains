using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Lane CurrentLane = Lane.Middle;

    private bool canGoLeft;
    private bool canGoRight;

    public enum Lane
    {
        Left,
        Middle,
        Right
    }

    private void OnEnable()
    {
        PlayerInput.Instance.OnLeft += HandleLeft;
        PlayerInput.Instance.OnRight += HandleRight;
    }

    private void OnDisable()
    {
        PlayerInput.Instance.OnLeft -= HandleLeft;
        PlayerInput.Instance.OnRight -= HandleRight;
    }

    private void Update()
    {
        switch (CurrentLane)
        {
            case Lane.Left:
                canGoLeft = false;
                canGoRight = true;
                break;
            case Lane.Middle:
                canGoLeft = true;
                canGoRight = true;
                break;
            case Lane.Right:
                canGoLeft = true;
                canGoRight = false;
                break;
        }
    }

    private void UpdateLocation()
    {
        float newX = LaneManager.LaneX[(int)CurrentLane];

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void HandleLeft(bool pressed)
    {
        if (!pressed)
            return;

        if(canGoLeft)
        {
            GoLeft();
        }
    }

    private void GoLeft()
    {
        if(CurrentLane == Lane.Middle)
        {
            CurrentLane = Lane.Left;
            UpdateLocation();
            return;
        }

        CurrentLane = Lane.Middle;
        UpdateLocation();
    }

    private void HandleRight(bool pressed)
    {
        if (!pressed)
            return;

        if (canGoRight)
            GoRight();
    }

    private void GoRight()
    {
        if(CurrentLane == Lane.Middle)
        {
            CurrentLane = Lane.Right;
            UpdateLocation();
            return;
        }

        CurrentLane = Lane.Middle;
        UpdateLocation();
    }
}
