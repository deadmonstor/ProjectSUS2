using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public bool canInteract { get; }
    public void FaceCheck(PlayerController player, bool enter);
    public bool InteractPressed(PlayerController player);
    public bool InteractReleased(PlayerController player);
}
