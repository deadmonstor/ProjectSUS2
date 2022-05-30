using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    public GameObject machine;
    public ItemSO item;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (machine.TryGetComponent<Interactable>(out var interactable))
            {
                interactable.InteractPressed(item);
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (machine.TryGetComponent<Interactable>(out var interactable))
            {
                interactable.InteractReleased(item);
            }
        }
    }
}
