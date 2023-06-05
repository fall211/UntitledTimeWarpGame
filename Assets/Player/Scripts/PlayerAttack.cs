using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private Animator animator;


    private bool TryAttack()
    {
        var mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // calculate direction of attack from player to mouse position
        var atkDirection = mousePos - new Vector2(transform.position.x, transform.position.y);
        atkDirection.Normalize();

        animator.SetFloat("x_atk", atkDirection.x);
        animator.SetFloat("y_atk", atkDirection.y);
        
        return true;
    }

    private void OnFire(InputValue inputValue)
    {
        if (!IsOwner) return;
        if (TryAttack()) animator.SetTrigger("Attack");
    }
}