using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public bool canInteract { get; set; }
    public bool InteractPressed(PlayerController player);
    public bool InteractReleased(PlayerController player);
}
