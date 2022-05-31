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
    public static event Action onAddEmptyTank;
    public static void OnAddEmptyTank(){onAddEmptyTank?.Invoke();}

    public static event Action<Table, ItemSO> onItemPutOnCustomerTable;
    public static void OnItemPutOnCustomerTable(Table table, ItemSO itemSo) { onItemPutOnCustomerTable?.Invoke(table, itemSo); }

    // int = level index. Change to scriptable object when it has been added.
    public static event Action<int> onLevelLoaded;
    public static void OnLevelLoaded(int index) { onLevelLoaded?.Invoke(index); }

    public static event Action<Customer> onCustomerSatDown;
    public static void OnCustomerSatDown(Customer customer) { onCustomerSatDown?.Invoke(customer); }

    public static event Action<int> onCustomerOrderCompleted;
    public static void OnCustomerOrderCompleted(int customersRemaining) { onCustomerOrderCompleted?.Invoke(customersRemaining); }
}
