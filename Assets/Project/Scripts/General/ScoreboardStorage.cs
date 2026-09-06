using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public static class ScoreboardStorage
{
    private const byte KEY = 0x5A;

    private static readonly string[] SpecialNames = { "DZO", "Legionaire" };

    public static void Save(List<ScoreEntryData> scores)
    {
        EnsureSpecialEntries(scores);

        SortScores(scores);

        AssignPositions(scores);

        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms, Encoding.UTF8);

        bw.Write(scores.Count);

        foreach (var s in scores)
        {
            bw.Write(s.Name);
            bw.Write(s.Score);
            bw.Write(s.Position);
        }

        byte[] data = ms.ToArray();
        for (int i = 0; i < data.Length; i++)
            data[i] ^= KEY;

        File.WriteAllBytes(GetPath(), data);
    }

    public static List<ScoreEntryData> Load()
    {
        try
        {
            if (!File.Exists(GetPath()))
                return CreateDefaultScoreboard();

            byte[] data = File.ReadAllBytes(GetPath());

            for (int i = 0; i < data.Length; i++)
                data[i] ^= KEY;

            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms, Encoding.UTF8);

            int count = br.ReadInt32();
            var list = new List<ScoreEntryData>(count);

            for (int i = 0; i < count; i++)
            {
                list.Add(new ScoreEntryData
                {
                    Name = br.ReadString(),
                    Score = br.ReadInt32(),
                    Position = br.ReadInt32()
                });
            }

            EnsureSpecialEntries(list);

            SortScores(list);

            AssignPositions(list);

            return list;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Scoreboard load failed: " + ex);
            return CreateDefaultScoreboard();
        }
    }

    private static List<ScoreEntryData> CreateDefaultScoreboard()
    {
        var list = new List<ScoreEntryData>();
        EnsureSpecialEntries(list);
        SortScores(list);
        AssignPositions(list);
        return list;
    }

    private static void EnsureSpecialEntries(List<ScoreEntryData> scores)
    {
        // Find highest NON-special score
        int highestNormalScore = 0;

        foreach (var s in scores)
        {
            if (!IsSpecial(s.Name))
                highestNormalScore = Mathf.Max(highestNormalScore, s.Score);
        }

        // Generate special scores
        int dzoScore = highestNormalScore + UnityEngine.Random.Range(0, 1001);
        int legScore = highestNormalScore + UnityEngine.Random.Range(0, 1001);

        // Ensure DZO exists
        var dzo = scores.Find(s => s.Name == "DZO");
        if (dzo == null)
        {
            scores.Add(new ScoreEntryData
            {
                Name = "DZO",
                Score = dzoScore
            });
        }
        else
        {
            dzo.Score = dzoScore;
        }

        // Ensure Legionaire exists
        var leg = scores.Find(s => s.Name == "Legionaire");
        if (leg == null)
        {
            scores.Add(new ScoreEntryData
            {
                Name = "Legionaire",
                Score = legScore
            });
        }
        else
        {
            leg.Score = legScore;
        }
    }

    private static bool IsSpecial(string name)
    {
        return name == "DZO" || name == "Legionaire";
    }

    private static void SortScores(List<ScoreEntryData> scores)
    {
        scores.Sort((a, b) =>
        {
            bool aSpecial = IsSpecial(a.Name);
            bool bSpecial = IsSpecial(b.Name);

            if (aSpecial && !bSpecial) return -1;
            if (!aSpecial && bSpecial) return 1;

            return b.Score.CompareTo(a.Score);
        });
    }

    private static void AssignPositions(List<ScoreEntryData> scores)
    {
        for (int i = 0; i < scores.Count; i++)
            scores[i].Position = i + 1;
    }

    private static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "scores.dat");
    }
}
