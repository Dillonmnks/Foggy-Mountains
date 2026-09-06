using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ScoresManager : MonoBehaviour
{
    public List<ScoreEntryData> Scores;
    public RectTransform Content;
    public GameObject ScoreEntryPrefab;

    private void Awake()
    {
        Scores = ScoreboardStorage.Load();
    }

    private void OnEnable()
    {
        Scores = ScoreboardStorage.Load();
        RefreshUI();
    }

    private void RefreshUI()
    {
        ClearContent();
        SortScores();

        if (Scores == null)
        {
            Debug.LogError("Scores is NULL");
            return;
        }

        if (ScoreEntryPrefab == null)
        {
            Debug.LogError("ScoreEntryPrefab is NULL");
            return;
        }

        if (Content == null)
        {
            Debug.LogError("Content is NULL");
            return;
        }

        foreach (var s in Scores)
        {
            GameObject entry = Instantiate(ScoreEntryPrefab, Content);

            if (entry == null)
            {
                Debug.LogError("Instantiate returned NULL");
                continue;
            }

            var ui = entry.GetComponent<ScoreEntry>();

            if (ui == null)
            {
                Debug.LogError("ScoreEntry component is missing on prefab: " + entry.name);
                continue;
            }

            if (s == null)
            {
                Debug.LogError("ScoreEntryData is NULL inside Scores list");
                continue;
            }

            ui.Init(s.Position.ToString(), s.Score.ToString(), s.Name);
        }
    }

    private void SortScores()
    {
        Scores.Sort((a, b) => b.Score.CompareTo(a.Score));

        for(int i = 0; i < Scores.Count; i++)
            Scores[i].Position = i + 1;
    }

    private void ClearContent()
    {
        foreach(Transform e in Content.transform)
            Destroy(e.gameObject);
    }
}

public class ScoreEntryData
{
    public string Name;
    public int Score;
    public int Position;
}
