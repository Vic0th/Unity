using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class MoveEvent : UnityEvent<Vector2> { }
[Serializable]
public class isShooting : UnityEvent<float> { }

public class PlayerInput : MonoBehaviour
{
    Controls control;
    public MoveEvent moveEvent;
    public isShooting shootEvent;
    public PlayerController pc;

    void Awake() {
        control = new Controls();
    }



    //Vector2 moveVector;
    void moveReg(InputAction.CallbackContext inp) {
        moveEvent.Invoke(inp.ReadValue<Vector2>());
        //Debug.Log(inp.ReadValue<Vector2>());
    }



    void shoot(InputAction.CallbackContext inp) {
        shootEvent.Invoke(inp.ReadValue<float>());
        //Debug.Log(inp.ReadValue<float>());
    }



    private void OnEnable() {
        control.Enable();
        control.Gameplay.Movement.performed += moveReg;
        control.Gameplay.Movement.canceled += moveReg;
        control.Gameplay.Shooting.performed += shoot;
        control.Gameplay.Shooting.canceled += shoot;
    }


    private void OnDisable() {
        //control.Disable();
        control.Gameplay.Disable();
    }



    private void Update() {
        pc.aimTo(control.Gameplay.Aiming.ReadValue<Vector2>());
    }
}
