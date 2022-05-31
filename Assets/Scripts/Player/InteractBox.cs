using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LayerMask interactLayer;
    public Interactable closestInteractable;
    public GameObject closestGameObject;
    private List<GameObject> objectsInRange;
    private void Start()
    {
        objectsInRange = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactLayer != (interactLayer | (1 << other.gameObject.layer))) return;
        
        objectsInRange.Add(other.gameObject);
        GetClosestInteractable(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactLayer != (interactLayer | (1 << other.gameObject.layer))) return;

        objectsInRange.Remove(other.gameObject);
        GetClosestInteractable(false);
    }

    private void GetClosestInteractable(bool enter)
    {
        float curDist = Single.MaxValue;
        GameObject curObj = null;
        
        if (objectsInRange.Count == 1)
        {
            closestInteractable = objectsInRange[0].GetComponent<Interactable>();
            closestGameObject = objectsInRange[0];
            return;
        }
        
        foreach (var obj in objectsInRange)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) < curDist)
            {
                curObj = obj;
                curDist = Vector3.Distance(transform.position, obj.transform.position);
            }
        }

        if (curObj != null)
        {
            closestInteractable.FaceCheck(playerController, enter);
            closestGameObject = objectsInRange[0];
        }
        else
        {
            closestInteractable = null;
            closestGameObject = null;
        }
    }

    public void InteractPressed()
    {
        if (closestInteractable == null) return;
        closestInteractable.InteractPressed(playerController);
    }
    
    public void InteractReleased()
    {       
        if (closestInteractable == null) return;
        closestInteractable.InteractReleased(playerController);
    }


}
