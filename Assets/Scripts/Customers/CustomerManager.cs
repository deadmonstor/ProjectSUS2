using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private int totalCustomerAmount = 2;
    [SerializeField] private GameObject tables;

    private List<GameObject> activeCustomers;

    private void Start()
    {
        
    }
}

[System.Serializable]
public class Order
{
    public bool needsFood;
    public bool needsDrink;

    public Order()
    {
        needsFood = Random.Range(0, 2) == 0;
        needsDrink = Random.Range(0, 2) == 0;
    }
}
