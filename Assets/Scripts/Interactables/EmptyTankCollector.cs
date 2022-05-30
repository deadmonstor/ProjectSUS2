using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmptyTankCollector : MonoBehaviour, Interactable
{
    [SerializeField] private ItemSO itemToCollect;
    private bool transitioning;
    private float timer;

    [SerializeField] private InteractType itemInteractType;
    [SerializeField] private int itemCount;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform itemSpawnpoint;
    private GameObject spawnedItem;
    private void Start()
    {
        canInteract = true;
        text.text = itemCount.ToString();
        spawnedItem = Instantiate(itemToCollect.itemPrefab, itemSpawnpoint);
    }

    private void OnEnable()
    {
        Events.onAddEmptyTank += AddEmptyTank;
    }

    private void OnDisable()
    {
        Events.onAddEmptyTank -= AddEmptyTank;
    }

    public bool canInteract { get; set; }
    public bool InteractPressed(PlayerController player)
    {
        if (itemCount <= 0) return false;
        if (player.GetItem() == itemToCollect) return false;

        transitioning = true;
        canInteract = false;
        timer = 0;
        
        if (itemCount <= 0)
            RemoveMesh();
        itemCount--;
        text.text = itemCount.ToString();
        player.SetItem(itemToCollect);
        
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
       Destroy(spawnedItem);
    }

    private void AddEmptyTank()
    {
        itemCount++;
        text.text = itemCount.ToString();
        spawnedItem = Instantiate(itemToCollect.itemPrefab, itemSpawnpoint);
    }
}