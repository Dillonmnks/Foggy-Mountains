using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Lane CurrentLane = Lane.Middle;

    private bool isSwitchingLane = false;
    private float laneSwitchDuration = 0.05f;
    private float laneSwitchTimer = 0f;

    [Header("Sound")]
    public AudioClip switchingLaneSound;

    private Lane targetLane;

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
        if (isSwitchingLane)
        {
            laneSwitchTimer += Time.deltaTime;

            float t = laneSwitchTimer / laneSwitchDuration;

            float newX = Mathf.Lerp(
                LaneManager.LaneX[(int)CurrentLane],
                LaneManager.LaneX[(int)targetLane],
                t
            );

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            if (t >= 1f)
            {
                CurrentLane = targetLane;
                isSwitchingLane = false;
            }
        }
    }

    private void HandleLeft(bool pressed)
    {
        if (!pressed || Time.timeScale == 0f)
            return;

        if (isSwitchingLane)
            return;

        if (CurrentLane == Lane.Left)
            return;

        targetLane = (CurrentLane == Lane.Middle) ? Lane.Left : Lane.Middle;
        StartLaneSwitch();
    }

    private void HandleRight(bool pressed)
    {
        if (!pressed || Time.timeScale == 0f)
            return;

        if (isSwitchingLane)
            return;

        if (CurrentLane == Lane.Right)
            return;

        targetLane = (CurrentLane == Lane.Middle) ? Lane.Right : Lane.Middle;
        StartLaneSwitch();
    }

    private void StartLaneSwitch()
    {
        SoundFXManager.Instance.PlaySoundFXClip(switchingLaneSound, transform);
        isSwitchingLane = true;
        laneSwitchTimer = 0f;
    }
}
