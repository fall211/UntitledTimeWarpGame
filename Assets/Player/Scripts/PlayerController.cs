using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : NetworkBehaviour
{

    private Vector2 _movementInput;

    public float moveSpeed = 1f;

    [SerializeField] private Animator animator;

    private void Update()
    {
        if (!IsOwner || _movementInput == Vector2.zero) return;
        
        TryMove(_movementInput);
    }

    private void TryMove(Vector2 direction)
    {
        var moveHere = new Vector3(direction.x, direction.y, 0);
        moveHere *= Time.deltaTime * moveSpeed;
        transform.position += moveHere;
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }


}

