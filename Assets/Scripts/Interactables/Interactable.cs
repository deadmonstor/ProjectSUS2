using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public bool canInteract { get; }
    public bool InteractPressed(PlayerController player);
    public bool InteractReleased(PlayerController player);
}
