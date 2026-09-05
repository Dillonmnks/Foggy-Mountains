using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerInput.Instance.OnLeft += HandleLeft;
        PlayerInput.Instance.OnRight += HandleRight;
    }

    private void OnDisable()
    {
        PlayerInput.Instance.OnLeft -= HandleLeft;
        PlayerInput.Instance.OnRight -= HandleRight;
    }

    private void HandleLeft(bool pressed)
    {
        if (pressed)
        {
            Debug.Log("The Left button was pressed n shiet");
        }

        else
        {
            Debug.Log("Huh... left button wasnt pressed i guess...");
        }
    }

    private void HandleRight(bool pressed)
    {
        if (pressed)
        {
            Debug.Log("The Right button was pressed n shiet");
        }

        else
        {
            Debug.Log("Huh... right button wasnt pressed i guess...");
        }
    }
}
