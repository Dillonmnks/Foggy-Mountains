using TMPro;
using UnityEngine;

public class ScoreEntry : MonoBehaviour
{
    public TMP_Text PositionText;
    public TMP_Text ScoreText;
    public TMP_Text NameText;

    public void Init(string position, string score, string name)
    {
        PositionText.text = position;
        ScoreText.text = score;
        NameText.text = name;
    }
}
