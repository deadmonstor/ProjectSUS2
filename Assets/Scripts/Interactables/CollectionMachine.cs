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
    
    [SerializeField] private InteractType itemInteractType;
    [SerializeField] private Transform itemSpawnpoint;
    private GameObject spawnedItem;
    [SerializeField] private bool shouldSpawnObject = true;

    [SerializeField] private string collectSound;
    [SerializeField] private GameObject holdInteractDisplay;
    [SerializeField] private GameObject pressInteractDisplay;
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
    public void FaceCheck(PlayerController player, bool enter)
    {
        if (enter)
        {
            if (itemInteractType == InteractType.Hold)
                holdInteractDisplay.SetActive(true);
            else if (itemInteractType == InteractType.Press)
                pressInteractDisplay.SetActive(true);
        }
        else
        {
            holdInteractDisplay.SetActive(false);
            pressInteractDisplay.SetActive(false);
        }
    }
    public bool InteractPressed(PlayerController player)
    {
        if (!canInteract) return false;
        if (player.GetItem() != null) return false;

        transitioning = true;
        canInteract = false;
        timer = 0;
        completionImage.transform.parent.gameObject.SetActive(true);
        RemoveMesh();
        
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
        completionImage.transform.parent.gameObject.SetActive(false);
    }
    
    private void SetMesh(ItemSO item)
    {
        if (shouldSpawnObject)
            spawnedItem = Instantiate(itemToCollect.itemPrefab, itemSpawnpoint);
    }

    private void RemoveMesh()
    {
        if (shouldSpawnObject)
            Destroy(spawnedItem);
    }
}
