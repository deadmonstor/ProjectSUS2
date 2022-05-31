using System;
using UnityEngine;

public class Table : MonoBehaviour, Interactable
{
    private enum CharDirection
    {
        Right,
        Left,
        Front,
        Back
    }

    private Vector3 GetOffset()
    {
        Vector3 offset = Vector3.zero;
        switch (chairDirection)
        {
            case CharDirection.Front:
                offset = Vector3.forward;
                break;
            case CharDirection.Back:
                offset = Vector3.back;
                break;
            case CharDirection.Left:
                offset = Vector3.left;
                break;
            case CharDirection.Right:
                offset = Vector3.right;
                break;
        }

        return offset;
    }

    [SerializeField] private CharDirection chairDirection;
    [SerializeField] private GameObject chairPrefab;
    [SerializeField] private Mesh chairMesh;
    [SerializeField] private Transform itemSpawnpoint;
    [SerializeField] private Transform drinkSpawnpoint;
    [SerializeField] private string placeSound = "click_002";
    private GameObject spawnedItem;
    private GameObject spawnedDrink;

    private GameObject ghostFood;
    private GameObject ghostDrink;
    
    public GameObject drinkGhostPrefab;
    public GameObject foodGhostPrefab;

    public GameObject chair;

    private void Start()
    {
        chair = GameObject.Instantiate(chairPrefab, transform.position + GetOffset(), chairPrefab.transform.rotation);
    }

    public void SpawnGhost(Order order)
    {
        if (order.needsDrink)
        {
            ghostDrink = Instantiate(drinkGhostPrefab, drinkSpawnpoint);
        }

        if (order.needsFood)
        {
            ghostFood = Instantiate(foodGhostPrefab, itemSpawnpoint);
        }
    }
    public void PutOnTable(PlayerController playerController, ItemSO item)
    {
        if (item.itemName == "FilledGlass" && spawnedDrink == null)
        {
            if (ghostDrink != null)
            {
                Destroy(ghostDrink.gameObject);
            }
            spawnedDrink = Instantiate(item.itemPrefab, drinkSpawnpoint);
            playerController.SetItem(null);
            Events.OnItemPutOnCustomerTable(this, item);
        }
        else if (spawnedItem == null && item.itemName == "HydratedFood")
        {
            if (ghostFood != null)
            {
                Destroy(ghostFood.gameObject);
            }
            spawnedItem = Instantiate(item.itemPrefab, itemSpawnpoint);
            playerController.SetItem(null);
            Events.OnItemPutOnCustomerTable(this, item);
        }
        else
        {
            return;
        }

        if (placeSound != "")
            SoundManager.PlaySFX(placeSound, transform.position);
        
        if (spawnedDrink != null && spawnedItem != null)
        {
            _canInteract = false;
        }
    }

    public void ClearItems()
    {
        if (spawnedItem != null)
        {
            Destroy(spawnedItem.gameObject);
        }

        if (spawnedDrink != null)
        {
            Destroy(spawnedDrink.gameObject);
        }

        _canInteract = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;
        
        Gizmos.DrawMesh(chairMesh, transform.position + GetOffset(), Quaternion.Euler(new Vector3(-90, 0, 90)));
    }
#endif

    private bool _canInteract = true;
    public bool canInteract => _canInteract;

    public void FaceCheck(PlayerController player, bool enter)
    {
        if (enter)
        {
        }
        else
        {
            
        }
    }

    public bool InteractPressed(PlayerController player)
    {
        ItemSO item = player.GetItem();
     
        if (_canInteract && item != null)
        {
            PutOnTable(player, item);
        }

        return true;
    }

    public bool InteractReleased(PlayerController player)
    {
        return false;
    }
}
