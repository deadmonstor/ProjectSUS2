using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class OxygenMachine : MonoBehaviour, Interactable
{
    [SerializeField] private Image completionImage;
    [SerializeField] private float transitionTime;
    [SerializeField] private ItemSO requiredItem;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    private bool transitioning;
    private float timer;
    [SerializeField] private InteractType itemInteractType;

    [SerializeField] private float removeTime;
    private void Start()
    {
        canInteract = true;
        completionImage.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (transitioning)
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
            completionImage.transform.parent.gameObject.SetActive(false);
            OxygenManager.FillOxygen();
        }
    }

    public bool canInteract { get; set; }
    public bool InteractPressed(PlayerController player)
    {
        if (!canInteract) return false;
        if (player.GetItem() != requiredItem) return false;

        transitioning = true;
        canInteract = false;
        timer = 0;
        completionImage.transform.parent.gameObject.SetActive(true);
        meshRenderer.material = requiredItem.itemMaterial;
        meshFilter.mesh = requiredItem.itemMesh;
        player.SetItem(null);

        
        if (itemInteractType == InteractType.Hold)
            player.ToggleMovement(false);
        
        return true;
    }

    public bool InteractReleased(PlayerController player)
    {
        player.ToggleMovement(true);

        if (transitioning && itemInteractType == InteractType.Hold)
        {
            StopTransition();
            player.SetItem(requiredItem);
            return true;
        }
        else
        {
            StartCoroutine(RemoveCoroutine());
        }

        return false;
    }

    private void StopTransition()
    {
        transitioning = false;
        canInteract = true;
        completionImage.transform.parent.gameObject.SetActive(false);

    }

    private IEnumerator RemoveCoroutine()
    {
        yield return new WaitForSeconds(removeTime);
        meshFilter.mesh = null;
        meshRenderer.material = null;
        Events.OnAddEmptyTank();
    }

}
