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
    [SerializeField] private float timeBetweenCustomerSpawns = 5.0f;
    
    [SerializeField] private Table[] tables;
    [SerializeField] private List<Table> availableTables = new List<Table>();

    private int _customersRemaining;
    private int _totalSpawnedCustomers;
    private float _customerSpawnTime;
    
    public bool HasAvailableTables => availableTables.Count > 0;
    public bool HasMoreCustomers => _customersRemaining > 0;
    public bool HasCustomerLeftToSpawn => _totalSpawnedCustomers < totalCustomerAmount;

    private Dictionary<Table, Customer> activeCustomers = new Dictionary<Table, Customer>();


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

    private void Update()
    {
        _customerSpawnTime += Time.deltaTime;

        if (_customerSpawnTime >= timeBetweenCustomerSpawns)
        {
            _customerSpawnTime = 0;
            SpawnCustomer();
        }
        
    }

    private void CustomerSatDown(Customer obj)
    {
        foreach (Customer c in activeCustomers.Values)
        {
            c.RecalculatePath();
        }
    }

    private void LevelLoaded(LevelSO index)
    {
        // TODO : ADD CONTEXT OF LEVEL
        _customersRemaining = index.MaxCustomersSpawned;
        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        if (!HasAvailableTables)
            return;

        if (!HasMoreCustomers)
            return;

        if (!HasCustomerLeftToSpawn)
            return;
        
        spawnParticle.Play();
        Table table = GetRandomTable();
        Customer customer = Instantiate(customerPrefab, Vector3.zero, spawnPoint.rotation);
        activeCustomers.Add(table, customer);
        customer.order = new Order();
        customer.table = table;
        customer.Warp(spawnPoint);
        StartCoroutine(MoveToTarget(customer));
        _totalSpawnedCustomers++;

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
        if (!activeCustomers.ContainsKey(table)) 
            return;
        
        Order order = activeCustomers[table].order;
        if ((item.itemName == "Glass" || item.itemName == "HydratedFood") && order.needsFood && !order.hasFood)
        {
            order.hasFood = true;
        }
            
        if (item.itemName == "FilledGlass" && order.needsDrink && !order.hasDrink)
        {
            order.hasDrink = true;
        }

        if (order.hasFood && order.hasDrink)
        {
            // Order given, time to eat lol
            OrderReceived(table, activeCustomers[table]);
        }
    }

    public void OrderReceived(Table table, Customer customer)
    {
        customer.indicatorTransform.gameObject.SetActive(false);
        StartCoroutine(Eat(table, customer));
    }

    public IEnumerator Eat(Table table, Customer customer)
    {
        yield return new WaitForSeconds(2f);
        table.ClearItems();
        activeCustomers.Remove(table);
        availableTables.Add(table);
        customer.Vanish();
        yield return new WaitForSeconds(2f);
        _customersRemaining--;
        Events.OnCustomerOrderCompleted(_customersRemaining);
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnNewCustomer());
    }

    private IEnumerator SpawnNewCustomer()
    {
        yield return new WaitForSeconds(2f);
        SpawnCustomer();
    }
}

public class Order
{
    public bool needsFood;
    public bool needsDrink;

    public bool hasFood;
    public bool hasDrink;

    private int _orderOption = 0;

    public Order()
    {
        _orderOption = Random.Range(0, 3);

        switch (_orderOption)
        {
            case 0:
                needsFood = true;
                needsDrink = false;
                hasDrink = true;
                break;
            case 1:
                needsFood = false;
                needsDrink = true;
                hasFood = true;
                break;
            case 2:
                needsFood = true;
                needsDrink = true;
                break;
        }
    }
}
