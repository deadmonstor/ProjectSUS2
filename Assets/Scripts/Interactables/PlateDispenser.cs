using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlateDispenser : MonoBehaviour, Interactable
{
    [SerializeField] private ItemSO itemToCollect;
    private bool transitioning;
    private float timer;

    [SerializeField] private InteractType itemInteractType;
    [SerializeField] private int itemCount;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform itemSpawnpoint;
    private GameObject spawnedItem;
    [SerializeField] private bool shouldSpawnObject = true;

    [SerializeField] private string collectSound;
    [SerializeField] private string returnSound;

    public Transform movePoint;
    
    private void Start()
    {
        canInteract = true;
        text.text = itemCount.ToString();
        if (shouldSpawnObject)
            spawnedItem = Instantiate(itemToCollect.itemPrefab, itemSpawnpoint);
    }

    private void OnEnable()
    {
        Events.onAddDirtyPlate += AddDirtyPlate;
    }

    private void OnDisable()
    {
        Events.onAddDirtyPlate -= AddDirtyPlate;
    }

    public bool canInteract { get; set; }
    public void FaceCheck(PlayerController player, bool enter)
    {
        if (enter)
        {
            
        }
        else
        {
            
        }
    }
    public bool InteractPressed(PlayerController player)
    {
        if (itemCount <= 0) return false;
        if (player.GetItem() != null) return false;

        transitioning = true;
        canInteract = false;
        timer = 0;
        
        if (itemCount <= 0)
            RemoveMesh();
        itemCount--;
        text.text = itemCount.ToString();
        player.SetItem(itemToCollect);
        if (collectSound != "")
            SoundManager.PlaySFX(collectSound, transform.position);
        
        if (itemInteractType == InteractType.Hold)
            player.ToggleMovement(false);
        return true;
    }

    public bool InteractReleased(PlayerController player)
    {
        player.ToggleMovement(true);
        return false;
    }

    private void StopTransition()
    {
        transitioning = false;
        canInteract = true;
    }
    private void RemoveMesh()
    {
        if (shouldSpawnObject)
            Destroy(spawnedItem);
    }

    public void AddDirtyPlate()
    {
        itemCount++;
        text.text = itemCount.ToString();
        if (shouldSpawnObject)
            spawnedItem = Instantiate(itemToCollect.itemPrefab, itemSpawnpoint);

        if (returnSound != "")
            SoundManager.PlaySFX(returnSound, transform.position);
    }
}
