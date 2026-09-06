using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ScoresManager : MonoBehaviour
{
    public List<ScoreEntryData> Scores;
    public RectTransform Content;
    public GameObject ScoreEntryPrefab;

    private void OnEnable()
    {
        Scores = ScoreboardStorage.Load();
        RefreshUI();
    }

    private void RefreshUI()
    {
        ClearContent();
        SortScores();

        foreach(var s in Scores)
        {
            GameObject entry = Instantiate(ScoreEntryPrefab, Content);
            var ui = entry.GetComponent<ScoreEntry>();

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
