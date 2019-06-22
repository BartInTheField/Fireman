using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action OnJump;
    public event Action<float> OnHorizontalAxis;
    public event Action OnHorizontalAxisZero;

    private void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (!moveInput.Equals(0))
        {
            OnHorizontalAxis?.Invoke(moveInput);
        }
        else
        {
            OnHorizontalAxisZero?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump?.Invoke();
        }
    }
}