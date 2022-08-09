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


    void MoveReg(InputAction.CallbackContext inp) => ps.OnMove(inp.ReadValue<Vector2>());
    void Shoot(InputAction.CallbackContext inp) => wp.Shoot(inp.ReadValue<float>());


    public void OnEnable() {
        control.Gameplay.Enable();

        control.Gameplay.Movement.performed += MoveReg;
        control.Gameplay.Movement.canceled += MoveReg;

        control.Gameplay.Jumping.performed += x => ps.Jump();

        if (wp != null) {
            control.Gameplay.Shooting.performed += Shoot;
            control.Gameplay.Shooting.canceled += Shoot;
        }
           
    }


    public void OnDisable() => control.Gameplay.Disable();

}
