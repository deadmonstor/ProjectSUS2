using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static event Action onUpdateOxygen;
    public static void OnUpdateOxygen() { onUpdateOxygen?.Invoke(); }
    public static event Action onOutOfOxygen;
    public static void OnOutOfOxygen(){onOutOfOxygen?.Invoke();}
}
