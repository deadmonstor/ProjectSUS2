using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenMachine : MonoBehaviour, Interactable
{
    [SerializeField] private Image completionImage;
    [SerializeField] private float transitionTime;
    private bool transitioning;
    private float timer;
    [SerializeField] private InteractType itemInteractType;

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
    public bool InteractPressed(ItemSO item)
    {
        if (!canInteract) return false;
        transitioning = true;
        canInteract = false;
        timer = 0;
        completionImage.transform.parent.gameObject.SetActive(true);
        return true;
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
        transitioning = false;
        canInteract = true;
        completionImage.transform.parent.gameObject.SetActive(false);

    }

}
