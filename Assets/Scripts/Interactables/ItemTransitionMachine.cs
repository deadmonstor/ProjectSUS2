using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTransitionMachine : MonoBehaviour, Interactable
{
    [SerializeField] private MachineType machineType;
    [SerializeField] private MeshFilter itemMeshFilter;
    [SerializeField] private MeshRenderer itemMeshRenderer;
    [SerializeField] private Image completionImage;
    private bool hasItem;
    private bool transitioning;
    private ItemSO transitionItem;
    private ItemSO startItem;
    private float transitionTime;
    private float timer;
    private InteractType itemInteractType;

    private void Start()
    {
        canInteract = true;
        hasItem = false;
        completionImage.transform.parent.gameObject.SetActive(false);
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
        }
    }

    public bool canInteract { get; set; }
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
        completionImage.transform.parent.gameObject.SetActive(false);

        if (transitioning && itemInteractType == InteractType.Hold)
        {
            StopTransition();
            player.SetItem(startItem);
            return true;
        }

        if (hasItem)
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
    }
    
    private bool TryCollectItem(PlayerController player)
    {
        player.SetItem(transitionItem);
        hasItem = false;
        RemoveMesh();
        return true;
    }
    private bool TrySetItem(PlayerController player)
    {
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
                
                player.SetItem(null);
                return true;
            }
        }

        return false;
    }

    private void SetMesh(ItemSO item)
    {
        itemMeshFilter.mesh = item.itemMesh;
        itemMeshRenderer.material = item.itemMaterial;
    }

    private void RemoveMesh()
    {
        itemMeshFilter.mesh = null;
        itemMeshRenderer.material = null;
    }
}
