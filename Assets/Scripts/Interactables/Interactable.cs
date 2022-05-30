using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public bool canInteract { get; set; }
    public bool InteractPressed(ItemSO item);
    public bool InteractReleased(ItemSO item);
}
