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
    [SerializeField] private Transform itemSpawnpoint;
    private bool transitioning;
    private float timer;
    [SerializeField] private InteractType itemInteractType;
    private GameObject spawnedItem;
    [SerializeField] private float removeTime;
    [SerializeField] private bool shouldSpawnObject = true;

    [SerializeField] private string holdSound;
    [SerializeField] private string completeSound;
    private AudioSource holdSource;
    [SerializeField] private GameObject bobDisplay;
    [SerializeField] private Image bobSprite;
    [SerializeField] private GameObject holdInteractDisplay;
    [SerializeField] private GameObject pressInteractDisplay;
    private void Start()
    {
        canInteract = true;
        completionImage.transform.parent.gameObject.SetActive(false);
        bobDisplay.SetActive(false);
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
            StopTransition();
            if (completeSound != "")
                SoundManager.PlaySFX(completeSound, transform.position);
        }
    }

    public bool canInteract { get; set; }
    public void FaceCheck(PlayerController player, bool enter)
    {
        if (enter)
        {
            if (player.GetItem() != requiredItem)
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
        if (player.GetItem() != requiredItem) return false;

        transitioning = true;
        canInteract = false;
        timer = 0;
        completionImage.transform.parent.gameObject.SetActive(true);
        if (shouldSpawnObject)
            spawnedItem = Instantiate(player.GetItem().itemPrefab, itemSpawnpoint);
        player.SetItem(null);

        if (holdSound != "")
            SoundManager.PlaySFX(holdSound, transform.position, out holdSource);
        
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
        if (holdSource != null)
        {
            holdSource.Stop();
            SoundManager.Return(holdSource);
        }
    }

    private IEnumerator RemoveCoroutine()
    {
        yield return new WaitForSeconds(removeTime);
        if (shouldSpawnObject)
            Destroy(spawnedItem);
        Events.OnAddEmptyTank();
    }

}
