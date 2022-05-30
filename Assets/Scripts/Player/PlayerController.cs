using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Update = UnityEngine.PlayerLoop.Update;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private InteractBox interactBox;
    private Rigidbody _rigidbody;
    private Vector2 _moveVector;
    private ItemSO item;
    private bool allowMovement = true;
    [SerializeField] private Transform itemSpawnpoint;
    private GameObject spawnedItem;
    public ItemSO GetItem() { return item;}
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        _moveVector = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (!allowMovement) return;
        if (_moveVector == Vector2.zero) return;

        Vector3 dir = new Vector3(_moveVector.x, 0, _moveVector.y);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void FixedUpdate()
    {
        if (!allowMovement) return;

        _rigidbody.AddForce(new Vector3(_moveVector.x, 0.0f, _moveVector.y) * playerSpeed, ForceMode.Impulse);
        //_rigidbody.MovePosition(transform.position * _moveVector * (playerSpeed * Time.deltaTime));
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
            interactBox.InteractPressed();
        
        if (ctx.phase == InputActionPhase.Canceled)
            interactBox.InteractReleased();

    }
    public void ToggleMovement(bool allow)
    {
        allowMovement = allow;
    }

    public void SetItem(ItemSO _item)
    {
        if (_item == null)
        {
            item = null;
            Destroy(spawnedItem);
            return;
        }
        
        item = _item;
        spawnedItem = Instantiate(item.itemPrefab, itemSpawnpoint);
    }
}
