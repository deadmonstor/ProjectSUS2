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
    [SerializeField] private GameObject plate;
    [SerializeField] private Transform itemSpawnpoint;
    private GameObject spawnedItem;

    private void Start()
    {
        GameObject.Instantiate(chairPrefab, transform.position + GetOffset(), chairPrefab.transform.rotation);
    }

    public void PutOnTable(ItemSO item)
    {
        spawnedItem = Instantiate(item.itemPrefab, itemSpawnpoint);
        
        _canInteract = false;
        Events.OnItemPutOnCustomerTable(item);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;
        
        Gizmos.DrawMesh(chairMesh, transform.position + GetOffset(), Quaternion.Euler(new Vector3(-90, 0, 90)));
    }

    private bool _canInteract = true;
    public bool canInteract => _canInteract;

    public bool InteractPressed(PlayerController player)
    {
        ItemSO item = player.GetItem();
     
        if (_canInteract && item != null)
        {
            PutOnTable(item);
            player.SetItem(null);
        }

        return true;
    }

    public bool InteractReleased(PlayerController player)
    {
        return false;
    }
}
