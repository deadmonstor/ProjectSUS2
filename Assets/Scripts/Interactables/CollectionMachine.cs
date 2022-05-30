using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionMachine : MonoBehaviour, Interactable
{
   [SerializeField] private Image completionImage;
    [SerializeField] private float transitionTime;
    [SerializeField] private ItemSO itemToCollect;
    private bool transitioning;
    private float timer;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private InteractType itemInteractType;

    private void Start()
    {
        canInteract = true;
        completionImage.transform.parent.gameObject.SetActive(false);
        SetMesh(itemToCollect);
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
            SetMesh(itemToCollect);
        }
    }

    public bool canInteract { get; set; }
    public bool InteractPressed(ItemSO item)
    {
        if (!canInteract) return false;
        transitioning = true;
        canInteract = false;
        timer = 0;
        completionImage.transform.parent.gameObject.SetActive(true);
        RemoveMesh();
        return true;
    }

    public bool InteractReleased(ItemSO item)
    {
        return false;
    }

    private void StopTransition()
    {
        transitioning = false;
        canInteract = true;
        completionImage.transform.parent.gameObject.SetActive(false);
    }
    
    private void SetMesh(ItemSO item)
    {
        meshFilter.mesh = item.itemMesh;
        meshRenderer.material = item.itemMaterial;
    }

    private void RemoveMesh()
    {
        meshFilter.mesh = null;
        meshRenderer.material = null;
    }
}
