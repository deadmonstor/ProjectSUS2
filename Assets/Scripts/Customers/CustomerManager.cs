using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private int totalCustomerAmount = 2;
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ParticleSystem spawnParticle;
    
    [SerializeField] private Table[] tables;
    [SerializeField] private List<Table> availableTables = new List<Table>();

    public bool HasAvailableTables => availableTables.Count > 0;
    
    private List<Customer> activeCustomers = new List<Customer>();


    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        Events.onItemPutOnCustomerTable += ItemPutOnTable;
        Events.onLevelLoaded += LevelLoaded;
        Events.onCustomerSatDown += CustomerSatDown;
        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        Events.onItemPutOnCustomerTable -= ItemPutOnTable;
        Events.onLevelLoaded -= LevelLoaded;
        Events.onCustomerSatDown -= CustomerSatDown;
    }

    private void CustomerSatDown(Customer obj)
    {
        foreach (Customer c in activeCustomers)
        {
            c.RecalculatePath();
        }
    }

    private void LevelLoaded(int index)
    {
        // TODO : ADD CONTEXT OF LEVEL
        SpawnCustomer();

    }

    private void SpawnCustomer()
    {
        if (!HasAvailableTables)
            return;
        
        spawnParticle.Play();
        Table table = GetRandomTable();
        Customer customer = Instantiate(customerPrefab, Vector3.zero, spawnPoint.rotation);
        activeCustomers.Add(customer);
        customer.table = table;
        customer.Warp(spawnPoint);
        StartCoroutine(MoveToTarget(customer));

    }

    private IEnumerator MoveToTarget(Customer customer)
    {
        yield return new WaitForSeconds(1.5f);
        
        customer.MoveToTarget();
    }

    private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        tables = GameObject.FindObjectsOfType<Table>();
        availableTables = tables.ToList();
    }

    private Table GetRandomTable()
    {
        if (!HasAvailableTables)
            return null;

        Table table = availableTables[Random.Range(0, availableTables.Count)];
        availableTables.Remove(table);
        
        return table;
    }

    
    private void ItemPutOnTable(Table table, ItemSO item)
    {
        
    }

    public void OrderReceived()
    {
        
    }

    public void Eat()
    {
        
    }

    public void GetOffFromTable()
    {
        
    }

    public void TableFree()
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
