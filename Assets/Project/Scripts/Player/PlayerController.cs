using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Lane CurrentLane = Lane.Middle;

    private bool isSwitchingLane = false;
    private float laneSwitchDuration = 0.05f;
    private float laneSwitchTimer = 0f;

    private float laneSwitchStartX;
    private Lane targetLane;

    public AudioClip MoveSound;

    public enum Lane { Left, Middle, Right }

    private void FixedUpdate()
    {
        if (isSwitchingLane)
        {
            laneSwitchTimer += Time.fixedDeltaTime;
            float t = laneSwitchTimer / laneSwitchDuration;

            float newX = Mathf.Lerp(
                laneSwitchStartX,
                LaneManager.LaneX[(int)targetLane],
                t
            );

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            if (t >= 1f)
                isSwitchingLane = false;
        }
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

    private void HandleLeft(bool pressed)
    {
        if (!pressed || isSwitchingLane)
            return;

        if (CurrentLane == Lane.Left)
            return;

        if (MoveSound != null)
            SoundFXManager.Instance.PlaySoundFXClip(MoveSound, transform);

        targetLane = (CurrentLane == Lane.Middle) ? Lane.Left : Lane.Middle;
        StartLaneSwitch();
    }

    private void HandleRight(bool pressed)
    {
        if (!pressed || isSwitchingLane)
            return;

        if (CurrentLane == Lane.Right)
            return;

        if(MoveSound != null)
            SoundFXManager.Instance.PlaySoundFXClip(MoveSound, transform);

        targetLane = (CurrentLane == Lane.Middle) ? Lane.Right : Lane.Middle;
        StartLaneSwitch();
    }

    private void StartLaneSwitch()
    {
        isSwitchingLane = true;
        laneSwitchTimer = 0f;

        laneSwitchStartX = transform.position.x;
        CurrentLane = targetLane;
    }
}
