using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void OnEnable()
    {
        Time.timeScale = 0f;

        PlayerInput.Instance.OnEsc += HandleEsc;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void HandleEsc(bool pressed)
    {
        if (!pressed) return;

        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
