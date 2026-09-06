using System.Collections.Generic;
using UnityEngine;

public class MapMover : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float speed;
    [SerializeField] private float mapLength;
    [SerializeField] private int mapsAhead = 4;
    [SerializeField] private int poolSize = 8;
    [SerializeField] private bool reverseDirection;

    private Queue<GameObject> mapPool = new();
    private List<GameObject> activeMaps = new();

    private void Start()
    {
        CreatePool();
        SpawnMaps();
    }

    private void FixedUpdate()
    {
        MoveMaps();
        DeleteMap();
        SpawnMaps();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(mapPrefab);
            go.SetActive(false);
            mapPool.Enqueue(go);
        }
    }

    private void MoveMaps()
    {
        Vector3 dir = GetDirection();

        foreach (var map in activeMaps)
            map.transform.position += dir * LaneManager.Instance.GetSpeed() * Time.fixedDeltaTime;
    }

    private Vector3 GetDirection()
    {
        Vector3 dir = (endPos.position - startPos.position).normalized;
        return reverseDirection ? -dir : dir;
    }

    private void SpawnMaps()
    {
        Vector3 dir = GetDirection();

        while (activeMaps.Count < mapsAhead)
        {
            Vector3 pos = activeMaps.Count == 0
                ? startPos.position
                : activeMaps[^1].transform.position - dir * mapLength;

            GameObject map = mapPool.Count > 0 ? mapPool.Dequeue() : Instantiate(mapPrefab);
            map.transform.position = pos;
            map.SetActive(true);

            activeMaps.Add(map);
        }
    }

    private void DeleteMap()
    {
        if (activeMaps.Count == 0) return;

        GameObject first = activeMaps[0];
        Vector3 dir = GetDirection();

        if (Vector3.Dot(first.transform.position - endPos.position, dir) >= 0f)
        {
            activeMaps.RemoveAt(0);
            first.SetActive(false);
            mapPool.Enqueue(first);
        }
    }



}