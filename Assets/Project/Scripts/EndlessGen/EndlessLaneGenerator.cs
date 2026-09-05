using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EndlessLaneGenerator : MonoBehaviour
{
    public float gameSpeed;

    public GameObject lanePrefab;
    public GameObject[] obstaclePrefabs;
    public float laneLength = 35f;
    public int lanesAhead = 5;

    public Queue<PooledLane> lanePool = new();
    public List<PooledLane> activeLanes = new();

    public int poolSize = 10;

    public static EndlessLaneGenerator Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        CreatePool();
        SpawnLane();
    }

    private void Update()
    {
        Time.timeScale = gameSpeed;

        UpdateLanes();
    }

    private void CreatePool()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(lanePrefab);
            go.SetActive(false);

            lanePool.Enqueue(go.AddComponent<PooledLane>());
        }
    }

    private void UpdateLanes()
    {
        if(DeleteFirstLane())
        {
            PooledLane lane = activeLanes[0];
            activeLanes.RemoveAt(0);

            lane.Deactivate();
            lanePool.Enqueue(lane);
        }

        SpawnLane();
    }

    private bool DeleteFirstLane()
    {
        return activeLanes.Count > 0 && activeLanes[0].transform.position.z < Player.Instance.transform.position.z - laneLength + 10f;
    }

    public void SpawnLane()
    {
        while(activeLanes.Count < lanesAhead)
        {
            float zPos = activeLanes.Count == 0 ? Player.Instance.transform.position.z : activeLanes[^1].transform.position.z + laneLength;

            Vector3 pos = new Vector3(0, 0, zPos);

            PooledLane lane = lanePool.Dequeue();
            lane.ClearObstacles();
            lane.Activate(pos);

            activeLanes.Add(lane);

            if (obstaclePrefabs.Length > 0)
                PlaceObstacle(lane.gameObject, obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
        }
    }
    void PlaceObstacle(GameObject lane, GameObject obstaclePrefab)
    {
        int lanePos = Random.Range(0, LaneManager.LaneX.Length);
        Vector3 pos = lane.transform.position;
        pos.x = LaneManager.LaneX[lanePos];

        GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);

        obstacle.transform.SetParent(lane.transform, true);
    }
}