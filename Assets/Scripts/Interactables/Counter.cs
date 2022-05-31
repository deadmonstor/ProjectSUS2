using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour, Interactable
{
    private bool hasItem;
    private ItemSO currentItem;
    private GameObject spawnedItem;
    [SerializeField] private Transform itemSpawnpoint;
    [SerializeField] private bool shouldSpawnObject = true;
    public bool canInteract { get; set; }
    [SerializeField] private string placeSound;
    [SerializeField] private string collectSound;
    [SerializeField] private GameObject holdInteractDisplay;
    [SerializeField] private GameObject pressInteractDisplay;
    public void FaceCheck(PlayerController player, bool enter)
    {
        if (enter)
        {
            pressInteractDisplay.SetActive(true);
        }
        else
        {
            pressInteractDisplay.SetActive(false);
        }
    }
    public bool InteractPressed(PlayerController player)
    {
        if (hasItem)
            return TryCollectItem(player);
        else
            return TrySetItem(player);

        return true;
    }

    public bool InteractReleased(PlayerController player)
    {
        return false;
    }
    
    private bool TryCollectItem(PlayerController player)
    {
        if (player.GetItem() != null) return false;

        player.SetItem(currentItem);
        currentItem = null;
        hasItem = false;
        RemoveMesh();
        if (collectSound != "")
            SoundManager.PlaySFX(collectSound, transform.position);
        return true;
    }
    private bool TrySetItem(PlayerController player)
    {
        if (player.GetItem() == null) return false;

        hasItem = true;
        currentItem = player.GetItem();
        SetMesh(currentItem);
        player.SetItem(null);
        if (placeSound != "")
            SoundManager.PlaySFX(placeSound, transform.position);
        return true;
    }
    
    private void SetMesh(ItemSO item)
    {        
        if (shouldSpawnObject)
            spawnedItem = Instantiate(item.itemPrefab, itemSpawnpoint);
    }

    private void RemoveMesh()
    {
        if (shouldSpawnObject)
            Destroy(spawnedItem);
    }
}
