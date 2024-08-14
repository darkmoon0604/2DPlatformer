using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    private static UserInput _Instance;

    public static UserInput Instance
    {
        get
        {
            return _Instance;
        }
    }

    private Controls m_Controls;
    public Controls Controls { get => m_Controls; set => m_Controls = value; }

    private Vector2 m_MoveInput;
    public Vector2 MoveInput { get => m_MoveInput; set => m_MoveInput = value; }

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(_Instance);
        }

        m_Controls = new Controls();

        m_Controls.Movement.Move.performed += Move;
    }

    private void Move(InputAction.CallbackContext context)
    {
        m_MoveInput = context.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        m_Controls.Enable();
    }

    private void OnDisable()
    {
        m_Controls.Disable();
    }
}
