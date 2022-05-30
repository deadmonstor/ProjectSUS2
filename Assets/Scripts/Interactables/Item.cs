using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactable
{
    [SerializeField] private ItemSO itemSO;
    public ItemSO GetItem => itemSO;
    public bool canInteract { get; set; }
    public bool InteractPressed(ItemSO item)
    {
        return false;
    }

    public bool InteractReleased(ItemSO item)
    {
        throw new NotImplementedException();
    }
}
