using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public TMP_Text textField;

    public void OnStartGame()
    {
        if(textField.text.Length != 4)
        {
            Debug.Log($"Name length: {textField.text.Length - 1}");
            return;
        }

        GameManager.Instance.SetName(textField.text);
        SceneManager.LoadScene("Game");
    }
}
