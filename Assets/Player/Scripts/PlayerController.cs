using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : NetworkBehaviour
{

    private Vector2 _movementInput;
    private Vector3 _lastPosition;
    private Vector3 _velocity;

    public float moveSpeed = 1f;

    [SerializeField] private Animator animator;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;
        _lastPosition = transform.position;
    }


    private void Update()
    {
        if (!IsOwner) return;
        _lastPosition = transform.position;
        
        TryMove(_movementInput);

        _velocity = (transform.position - _lastPosition) / Time.deltaTime;
        UpdateAnimation(_velocity);


    }

    private void TryMove(Vector2 direction)
    {
        var moveHere = new Vector3(direction.x, direction.y, 0);
        moveHere *= Time.deltaTime * moveSpeed;
        transform.position += moveHere;
    }


    private void UpdateAnimation(Vector2 vel)
    {
        if (animator == null) return;

        animator.SetFloat("x", vel.x);
        animator.SetFloat("y", vel.y);

    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

}

