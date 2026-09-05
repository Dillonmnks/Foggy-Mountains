using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set; }

    public event Action<bool> OnLeft;
    public event Action<bool> OnRight;
    public event Action<bool> OnSpace;
    public event Action<bool> OnShift;
    public event Action<bool> OnClearFog;
    public event Action<bool> OnEsc;

    private MyInputActions inputActions;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        inputActions = new MyInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        //LEFT
        inputActions.Standard.Left.performed += ctx =>
        {
            Debug.Log("Left pressed");
            OnLeft?.Invoke(true);
        };

        inputActions.Standard.Left.canceled += ctx =>
        {
            Debug.Log("Left released");
            OnLeft?.Invoke(false);
        };

        //RIGHT
        inputActions.Standard.Right.performed += ctx =>
        {
            Debug.Log("Right pressed");
            OnRight?.Invoke(true);
        };

        inputActions.Standard.Right.canceled += ctx =>
        {
            Debug.Log("Right released");
            OnRight?.Invoke(false);
        };

        //SPACE
        inputActions.Standard.Space.performed += ctx =>
        {
            Debug.Log("Space pressed");
            OnSpace?.Invoke(true);
        };

        inputActions.Standard.Space.canceled += ctx =>
        {
            Debug.Log("Space released");
            OnSpace?.Invoke(false);
        };

        //CLEAR FOG
        inputActions.Standard.ClearFog.performed += ctx =>
        {
            Debug.Log("ClearFog pressed");
            OnClearFog?.Invoke(true);
        };

        inputActions.Standard.ClearFog.canceled += ctx =>
        {
            Debug.Log("ClearFog released");
            OnClearFog?.Invoke(false);
        };

        //SHIFT
        inputActions.Standard.Shift.performed += ctx =>
        {
            Debug.Log("Shift pressed");
            OnShift?.Invoke(true);
        };

        inputActions.Standard.Shift.canceled += ctx =>
        {
            Debug.Log("Shift released");
            OnShift?.Invoke(false);
        };

        //Escape
        inputActions.Standard.Esc.performed += ctx =>
        {
            Debug.Log("Esc pressed");
            OnEsc?.Invoke(true);
        };

        inputActions.Standard.Esc.canceled += ctx =>
        {
            Debug.Log("Esc released");
            OnEsc?.Invoke(false);
        };
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
