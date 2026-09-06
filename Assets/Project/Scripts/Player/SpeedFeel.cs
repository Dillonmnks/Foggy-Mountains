using UnityEngine;

public class SpeedFeel : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float minSpeed = 20f, maxSpeed = 100f;
    [SerializeField] private float minFov = 60f, maxFov = 78f;
    [SerializeField] private float shakeAmplitude = 0.05f;

    private Vector3 basePos;

    private void Awake()
    {
        basePos = cam.transform.localPosition;
    }

    private void LateUpdate()
    {
        float t = Mathf.InverseLerp(minSpeed, maxSpeed, LaneManager.Instance.GetSpeed());

        cam.fieldOfView = Mathf.Lerp(minFov, maxFov, t);

        Vector3 shakeOffset = Random.insideUnitSphere * shakeAmplitude * t;
        shakeOffset.z = 0f;
        cam.transform.localPosition = basePos + shakeOffset;
    }
}