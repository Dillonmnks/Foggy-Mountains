using System.Collections.Generic;
using UnityEngine;

public class MapMover : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float speed;
    [SerializeField] private float mapLength;

    private List<GameObject> activeMaps = new();

    private void Start()
    {
        activeMaps.Add(Instantiate(mapPrefab, startPos.position, Quaternion.identity));
    }


    private void FixedUpdate()
    {
        MoveMap(); // noice
        CheckMap(); // noice
        DeleteMap(); // noice
    }

    private void MoveMap()
    {
        Vector3 dir = (startPos.position - endPos.position).normalized;

        foreach (var map in activeMaps) 
            map.transform.position += dir * speed * Time.fixedDeltaTime;
    }

    private void CheckMap()
    {
        GameObject last = activeMaps[activeMaps.Count - 1];
        if (Vector3.Distance(startPos.position, last.transform.position) >= mapLength)
            activeMaps.Add(Instantiate(mapPrefab, startPos.position, Quaternion.identity));
    }

    private void DeleteMap()
    {
        GameObject first = activeMaps[0];

        if(Vector3.Distance(first.transform.position, endPos.position) < 0.5f)
        {
            activeMaps.RemoveAt(0);
            Destroy(first);
        }
    }

}


