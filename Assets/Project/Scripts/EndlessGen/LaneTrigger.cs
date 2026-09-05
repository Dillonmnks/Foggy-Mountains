using UnityEngine;

public class LaneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        EndlessLaneGenerator.Instance.SpawnLane();
    }
}
