using System.Collections.Generic;
using UnityEngine;

public class EndlessLaneGenerator : MonoBehaviour
{
    public GameObject lanePrefab;
    public GameObject[] obstaclePrefabs;
    public float laneLength = 10f;
    public int lanesAhead = 5;

    private float nextSpawnZ = 0f;
    private float distanceTraveled = 0f;

    public List<GameObject> listNotQueue = new();


    public static EndlessLaneGenerator Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        distanceTraveled += LaneManager.Instance.speed * Time.deltaTime;

        int lanesNeeded = 5;

        for (int i = 0; i < lanesNeeded; i++)
            SpawnLane();
    }


    public void SpawnLane()
    {
        float zPosition = 0f;
        if (listNotQueue.Count > 0)
        zPosition = listNotQueue[listNotQueue.Count - 1].transform.position.z + laneLength;

        GameObject lane = Instantiate(lanePrefab, new Vector3(0, 0, zPosition), Quaternion.identity);

        listNotQueue.Add(lane);

        if (obstaclePrefabs.Length > 0)
            PlaceObstacle(lane, obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
    }

    void PlaceObstacle(GameObject lane, GameObject obstaclePrefab) // if we have lag == object pooling
    {
        int lanePos = Random.Range(0, LaneManager.LaneX.Length); // 0,1,2
        Vector3 pos = lane.transform.position;
        pos.x = LaneManager.LaneX[lanePos];

        Instantiate(obstaclePrefab, pos, Quaternion.identity, lane.transform);
    }


}