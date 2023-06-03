using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.PlayerScripts
{
    public class PlayerController : NetworkBehaviour
    {

        private Vector2 _movementInput;

        public float moveSpeed = 1f;

        private void Update()
        {
            if (!IsOwner) return;

            
            if (_movementInput == Vector2.zero) return;

            
            TryMove(_movementInput);
            
            
        }

        private void TryMove(Vector2 direction)
        {
            var moveHere = new Vector3(_movementInput.x, _movementInput.y, 0);
            moveHere *= Time.deltaTime * moveSpeed;
            transform.position += moveHere;
        }

        private void OnMove(InputValue inputValue)
        {
            _movementInput = inputValue.Get<Vector2>();
        }
    
    
    }
}
