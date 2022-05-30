using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ItemSO", fileName = "ItemSO", order = 0)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public List<ItemTransition> itemTransitions;
    public Mesh itemMesh;
    public Material itemMaterial;
}
