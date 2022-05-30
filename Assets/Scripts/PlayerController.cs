using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    
    private Rigidbody _rigidbody;
    private Vector2 _moveVector;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        _moveVector = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(new Vector3(_moveVector.x, 0.0f, _moveVector.y) * playerSpeed, ForceMode.Impulse);
        //_rigidbody.MovePosition(transform.position * _moveVector * (playerSpeed * Time.deltaTime));
    }
}
