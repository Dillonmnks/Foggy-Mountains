using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public static class ScoreboardStorage
{
    private const byte KEY = 0x5A;

    public static void Save(List<ScoreEntryData> scores)
    {
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
        if (!File.Exists(GetPath()))
            return new List<ScoreEntryData>();

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

        return list;
    }

    private static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "scores.dat");
    }
}
