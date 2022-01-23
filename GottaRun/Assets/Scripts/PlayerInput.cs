using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerInput : MonoBehaviour
{
    Controls control;
    PlayerController pc;
    Weapon wp;

    void Awake() {
        control = new Controls();
        control.Enable();
        pc = GetComponent<PlayerController>();
        wp = GetComponentInChildren<Weapon>();
    }


    void moveReg(InputAction.CallbackContext inp) => pc.onMove(inp.ReadValue<float>());
    void shoot(InputAction.CallbackContext inp) => wp.Shoot(inp.ReadValue<float>());


    public void OnEnable() {
        control.Gameplay.Enable();

        control.Gameplay.Movement.performed += moveReg;
        control.Gameplay.Movement.canceled += moveReg;

        control.Gameplay.Jumping.performed += x => pc.Jump();

        if (wp != null) {
            control.Gameplay.Shooting.performed += shoot;
            control.Gameplay.Shooting.canceled += shoot;
        }
           
    }


    public void OnDisable() => control.Gameplay.Disable();

}
