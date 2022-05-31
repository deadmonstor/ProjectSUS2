using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTransitionMachine : MonoBehaviour, Interactable
{
    [SerializeField] private MachineType machineType;
    [SerializeField] private Image completionImage;
    private bool hasItem;
    private bool transitioning;
    private ItemSO transitionItem;
    private ItemSO startItem;
    private float transitionTime;
    private float timer;
    private InteractType itemInteractType;
    [SerializeField] private Transform itemSpawnpoint;
    private GameObject spawnedItem;
    [SerializeField] private ItemSO requiredItem;
    [SerializeField] private bool shouldSpawnObject = true;
    [SerializeField] private bool requiresItemToCollect = false;
    [SerializeField] private ItemSO itemRequiredToCollect;
    [SerializeField] private string completeSound;
    [SerializeField] private string holdSound;
    [SerializeField] private string collectSound;
    [SerializeField] private string placeSound = "pluck_002";
    private AudioSource holdSource;

    [SerializeField] private GameObject bobDisplay;
    [SerializeField] private GameObject holdInteractDisplay;
    [SerializeField] private GameObject pressInteractDisplay;
    [SerializeField] private Image bobSprite;
    private void Start()
    {
        canInteract = true;
        hasItem = false;
        completionImage.transform.parent.gameObject.SetActive(false);
        bobDisplay.SetActive(false);
    }

    private void Update()
    {
        if (hasItem && transitioning)
            TransitionItem();
    }

    public void TransitionItem()
    {
        timer += Time.deltaTime;
        completionImage.fillAmount = timer / transitionTime;

        if (timer >= transitionTime)
        {
            transitioning = false;
            canInteract = true;
            SetMesh(transitionItem);
            if (completeSound != "")
                SoundManager.PlaySFX(completeSound, transform.position);
        }
    }

    public bool canInteract { get; set; }
    public void FaceCheck(PlayerController player, bool enter)
    {
        Debug.Log(itemInteractType);
        if (player.GetItem() != null)
            itemInteractType = player.GetItem().itemTransitions[0].interactType;
        if (enter)
        {
            if (!transitioning && hasItem && (player.GetItem() != itemRequiredToCollect || player.GetItem() == null))
            {
                bobDisplay.SetActive(true);
                bobSprite.sprite = itemRequiredToCollect.displaySprite;
            }
            else if (!hasItem && (player.GetItem() == null || player.GetItem() != requiredItem))
            {
                bobDisplay.SetActive(true);
                bobSprite.sprite = requiredItem.displaySprite;
            }
            else
            {
                bobDisplay.SetActive(false);
                if (itemInteractType == InteractType.Hold)
                {
                    holdInteractDisplay.SetActive(true);
                    pressInteractDisplay.SetActive(false);
                }
                else if (itemInteractType == InteractType.Press)
                {
                    holdInteractDisplay.SetActive(false);
                    pressInteractDisplay.SetActive(true);
                }
            }
        }
        else
        {
            bobDisplay.SetActive(false);
            holdInteractDisplay.SetActive(false);
            pressInteractDisplay.SetActive(false);
        }
    }

    public bool InteractPressed(PlayerController player)
    {
        if (!canInteract) return false;
        if (hasItem)
            return TryCollectItem(player);
        
        return TrySetItem(player);
    }

    public bool InteractReleased(PlayerController player)
    {        
        player.ToggleMovement(true);

        if (transitioning && itemInteractType == InteractType.Hold)
        {
            completionImage.transform.parent.gameObject.SetActive(false);
            StopTransition();
            player.SetItem(startItem);
            return true;
        }

        if (hasItem && !transitioning)
        {
            TryCollectItem(player);
            return true;
        }

        return false;
    }

    private void StopTransition()
    {
        RemoveMesh();
        transitioning = false;
        hasItem = false;
        transitionItem = null;
        canInteract = true;
        completionImage.transform.parent.gameObject.SetActive(false);
        if (holdSource != null)
        {
            holdSource.Stop();
            SoundManager.Return(holdSource);
        }
    }
    
    private bool TryCollectItem(PlayerController player)
    {
        if (requiresItemToCollect)
        {
            if (player.GetItem() != requiresItemToCollect) return false;
            player.SetItem(null);
        }
        if (collectSound != "")
            SoundManager.PlaySFX(collectSound, transform.position);
        
        completionImage.transform.parent.gameObject.SetActive(false);
        player.SetItem(transitionItem);
        hasItem = false;
        RemoveMesh();
        StopTransition();
        FaceCheck(player, true);
        return true;
    }
    private bool TrySetItem(PlayerController player)
    {
        if (player.GetItem() == null) return false;

        foreach (var transition in player.GetItem().itemTransitions)
        {
            if (transition.machineToUse == machineType)
            {
                startItem = player.GetItem();
                SetMesh(startItem);
                timer = 0;
                transitionTime = transition.transitionTime;
                transitionItem = transition.itemToGet;
                canInteract = false;
                hasItem = true;
                transitioning = true;
                itemInteractType = transition.interactType;
                completionImage.transform.parent.gameObject.SetActive(true);
                completionImage.fillAmount = 0;
                if (itemInteractType == InteractType.Hold)
                    player.ToggleMovement(false);
                if (placeSound != "")
                    SoundManager.PlaySFX(placeSound, transform.position);
                if (holdSound != "" && itemInteractType == InteractType.Hold)
                    SoundManager.PlaySFX(holdSound, transform.position, out holdSource);
                
                player.SetItem(null);
                return true;
            }
        }

        return false;
    }

    private void SetMesh(ItemSO item)
    {
        if (!shouldSpawnObject) return;

        RemoveMesh();
        spawnedItem = Instantiate(item.itemPrefab, itemSpawnpoint);
    }

    private void RemoveMesh()
    {
        if (!shouldSpawnObject) return;

        if (spawnedItem == null) return;

        Destroy(spawnedItem);
    }
}
