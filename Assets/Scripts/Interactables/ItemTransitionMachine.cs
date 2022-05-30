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
    public bool InteractPressed(ItemSO item)
    {
        if (!canInteract) return false;
        if (hasItem)
            return TryCollectItem(item);
        
        return TrySetItem(item);
    }

    public bool InteractReleased(ItemSO item)
    {
        if (transitioning && itemInteractType == InteractType.Hold)
        {
            StopTransition();
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
    private bool TryCollectItem(ItemSO item)
    {
        return false;
    }
    private bool TrySetItem(ItemSO item)
    {
        foreach (var transition in item.itemTransitions)
        {
            if (transition.machineToUse == machineType)
            {
                SetMesh(item);
                timer = 0;
                transitionTime = transition.transitionTime;
                transitionItem = transition.itemToGet;
                canInteract = false;
                hasItem = true;
                transitioning = true;
                itemInteractType = transition.interactType;
                completionImage.transform.parent.gameObject.SetActive(true);
                completionImage.fillAmount = 0;
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
