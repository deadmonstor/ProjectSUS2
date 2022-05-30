using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour, Interactable
{
    private bool hasItem;
    private ItemSO currentItem;
    private GameObject spawnedItem;
    [SerializeField] private Transform itemSpawnpoint;

    public bool canInteract { get; set; }
    public bool InteractPressed(PlayerController player)
    {
        if (hasItem)
            TryCollectItem(player);
        else
            TrySetItem(player);

        return true;
    }

    public bool InteractReleased(PlayerController player)
    {
        return false;
    }
    
    private bool TryCollectItem(PlayerController player)
    {
        player.SetItem(currentItem);
        currentItem = null;
        hasItem = false;
        RemoveMesh();
        return true;
    }
    private bool TrySetItem(PlayerController player)
    {
        hasItem = true;
        currentItem = player.GetItem();
        SetMesh(currentItem);
        player.SetItem(null);
        return true;
    }
    
    private void SetMesh(ItemSO item)
    {
        spawnedItem = Instantiate(item.itemPrefab, itemSpawnpoint);
    }

    private void RemoveMesh()
    {
        Destroy(spawnedItem);
    }
}
