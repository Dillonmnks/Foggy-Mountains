using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EndlessLaneGenerator : MonoBehaviour
{
    public GameObject lanePrefab;
    public GameObject[] obstaclePrefabs;
    public GameObject[] collectiblePrefabs;
    public float laneLength = 35f;
    public int lanesAhead = 5;

    public float collectibleSpawnChance = 0.05f;

    public Queue<PooledLane> lanePool = new();
    public List<PooledLane> activeLanes = new();

    public int poolSize = 10;

    public int safeLaneCount = 3;
    private int lanesSpawnedTotal = 0;


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
        while (activeLanes.Count < lanesAhead)
        {
            float zPos = activeLanes.Count == 0 ? Player.Instance.transform.position.z : activeLanes[^1].transform.position.z + laneLength;

            Vector3 pos = new Vector3(0, 0, zPos);

            PooledLane lane = lanePool.Dequeue();
            lane.ClearObstacles();
            lane.Activate(pos);

            activeLanes.Add(lane);
            lanesSpawnedTotal++;

            if (obstaclePrefabs.Length > 0 && lanesSpawnedTotal > safeLaneCount)
                PlaceObstacle(lane.gameObject, obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
        }
    }

    void PlaceObstacle(GameObject lane, GameObject obstaclePrefab)
    {
        int amount = 1;

        float v = UnityEngine.Random.Range(0f, 1f);

        bool isCollectible = v <= collectibleSpawnChance;

        if (v > 0.7f)
            amount = 2;

        int lanePosA = Random.Range(0, LaneManager.LaneX.Length);

        int lanePosB = lanePosA;
        if (amount == 2)
            while (lanePosB == lanePosA)
                lanePosB = Random.Range(0, LaneManager.LaneX.Length);
        Vector3 posA = lane.transform.position;
        posA.x = LaneManager.LaneX[lanePosA];

        if(!isCollectible)
        {
            GameObject obstacleA = Instantiate(obstaclePrefab, posA, Quaternion.identity);
            obstacleA.transform.SetParent(lane.transform, true);
        }

        else
        {
            Collectible collectible = GetWeightedCollectible();
            GameObject collectibleGO = Instantiate(collectible.gameObject, posA, Quaternion.identity);
            collectibleGO.transform.SetParent(lane.transform, true);
        }

        if (amount == 2)
        {
            Vector3 posB = lane.transform.position;
            posB.x = LaneManager.LaneX[lanePosB];

            GameObject obstacleB = Instantiate(obstaclePrefab, posB, Quaternion.identity);
            obstacleB.transform.SetParent(lane.transform, true);
        }
    }

    private Collectible GetWeightedCollectible()
    {
        float totalWeight = 0f;

        foreach (var c in collectiblePrefabs)
            totalWeight += c.GetComponent<Collectible>().SpawnWeight;

        if (totalWeight <= 0f)
            return collectiblePrefabs[0].GetComponent<Collectible>();

        float r = Random.Range(0f, totalWeight);

        foreach (var c in collectiblePrefabs)
        {
            r -= c.GetComponent<Collectible>().SpawnWeight;
            if (r <= 0f)
                return c.GetComponent<Collectible>();
        }

        return collectiblePrefabs[collectiblePrefabs.Length - 1].GetComponent<Collectible>();
    }
}