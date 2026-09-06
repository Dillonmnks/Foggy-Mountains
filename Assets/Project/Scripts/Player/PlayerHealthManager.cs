using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance;

    public Image[] hearts = new Image[3];

    private void Update()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(Player.Lives - 1 >= i)
            {
                continue;
            }

            if (hearts[i].isActiveAndEnabled)
                hearts[i].enabled = false;
        }
    }
}