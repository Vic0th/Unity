using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerInput : MonoBehaviour
{
    Controls control;
    PlayerScript ps;
    Weapon wp;

    void Awake() {
        control = new Controls();
        control.Enable();
        ps = GetComponent<PlayerScript>();
        wp = GetComponentInChildren<Weapon>();
    }


    void moveReg(InputAction.CallbackContext inp) => ps.onMove(inp.ReadValue<Vector2>());
    void shoot(InputAction.CallbackContext inp) => wp.Shoot(inp.ReadValue<float>());


    public void OnEnable() {
        control.Gameplay.Enable();

        control.Gameplay.Movement.performed += moveReg;
        control.Gameplay.Movement.canceled += moveReg;

        control.Gameplay.Jumping.performed += x => ps.Jump();

        if (wp != null) {
            control.Gameplay.Shooting.performed += shoot;
            control.Gameplay.Shooting.canceled += shoot;
        }
           
    }


    public void OnDisable() => control.Gameplay.Disable();

}
