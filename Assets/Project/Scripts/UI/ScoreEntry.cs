using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEntry : MonoBehaviour
{
    public TMP_Text PositionText;
    public TMP_Text ScoreText;
    public TMP_Text NameText;

    public Image image;

    public Sprite No1;
    public Sprite No2;
    public Sprite No3;

    public void Init(string position, string score, string name)
    {
        PositionText.text = position;
        ScoreText.text = score;
        NameText.text = name;

        if (position == "1")
        {
            image.sprite = No1;
        }

        else if (position == "2")
        {
            image.sprite = No2;
        }

        else if (position == "3")
        {
            image.sprite = No3;
        }

        else
            image.enabled = false;
    }
}
