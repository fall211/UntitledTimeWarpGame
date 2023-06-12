using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class PlayerController : NetworkBehaviour
{

    private Vector2 _movementInput;
    private Vector3 _lastPosition;
    private Vector3 _velocity;
    private ContactFilter2D _contactFilter2D = new ContactFilter2D();
    private List<RaycastHit2D> _hitBuffer = new List<RaycastHit2D>();
    [SerializeField] private Transform _raycastOrigin;

    public float moveSpeed = 1f;

    [SerializeField] private Animator animator;
    private Camera _mainCamera;
    [SerializeField] private float _cameraMoveSpeed = 1f;

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
        
        if (_movementInput != Vector2.zero){
            bool success = TryMove(_movementInput);
            if (!success){
                success = TryMove(new Vector2(_movementInput.x, 0));
                if (!success){
                    success = TryMove(new Vector2(0, _movementInput.y));
                }
            }
        }


        _velocity = (transform.position - _lastPosition) / Time.deltaTime;
        UpdateAnimation(_velocity);

        MoveCamera();

    }

    private bool TryMove(Vector2 direction)
    {
        _contactFilter2D.SetLayerMask(LayerMask.GetMask("Obstacles"));
        var hitCount = Physics2D.Raycast(_raycastOrigin.position, direction, _contactFilter2D, _hitBuffer, (moveSpeed * Time.deltaTime) + 0.1f);
        if (hitCount > 0) return false;

        var moveHere = new Vector3(direction.x, direction.y, 0);
        moveHere *= Time.deltaTime * moveSpeed;
        transform.position += moveHere;

        return true;
    }

    private void UpdateAnimation(Vector2 vel)
    {
        if (animator == null) return;

        animator.SetFloat("x", vel.x);
        animator.SetFloat("y", vel.y);

    }

    private void MoveCamera()
    {
        if (_mainCamera == null) _mainCamera = Camera.main;
        if (_mainCamera == null) return;

        var cameraPos = _mainCamera.transform.position;
        var playerPos = transform.position;
        var newPos = Vector3.Lerp(cameraPos, playerPos, Time.deltaTime * _cameraMoveSpeed);
        newPos.z = -10;
        _mainCamera.transform.position = newPos;
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

}

