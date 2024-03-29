using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Table table;
    public Order order;
    public ParticleSystem deSpawnParticle;

    public float movingSineScale;
    public float sittingSineScale;
    public float speed;
    private float startY;
    public GameObject gfxObject;
    public Transform indicatorTransform;

    public Transform emptyStuffTransform;
    public GameObject emptyPlatePrefab;
    public GameObject emptyDrinkPrefab;

    public GameObject emptyPlate;
    public GameObject emptyGlass;
    
    private PlateDispenser _plateDispenser;

    public bool hasSatDown;
    public bool atPlateDispenser;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        startY = gfxObject.transform.position.y;
    }

    public void Warp(Transform target)
    {
        _agent.Warp(target.position);
    }
    
    public void MoveToTarget()
    {
        _agent.SetDestination(table.chair.transform.position);
    }

    public void MoveToPlateDispenser()
    {
        transform.GetComponent<NavMeshObstacle>().enabled = true;
        _agent.updateRotation = true;
        _agent.isStopped = false;
        _agent.enabled = true;

        _agent.SetDestination(_plateDispenser.movePoint.position);
    }

    public void GiveEmptyPlateAndGlass()
    {
        if (order.needsDrink)
        {
            emptyGlass = Instantiate(emptyDrinkPrefab.gameObject, emptyStuffTransform);
        }

        if (order.needsFood)
        {
            emptyPlate = Instantiate(emptyPlatePrefab.gameObject, emptyStuffTransform);
        }
    }
    
    public void Vanish()
    {
        StartCoroutine(CoroutineVanish());
    }

    private IEnumerator CoroutineVanish()
    {
        yield return new WaitForSeconds(1.5f);
        deSpawnParticle.Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    public void RotateToTarget(Transform targetTransform)
    {
        Vector3 lookPos = targetTransform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1); 
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f); 
    }

    private void Update()
    {
        Bob(_agent.enabled ? movingSineScale : sittingSineScale);
    }

    private void Bob(float scale)
    {
        var pos = gfxObject.transform.position;
        pos.y = startY + (Mathf.Sin(Time.time * speed) * scale);
        gfxObject.transform.position = pos;
    }

    private void FixedUpdate()
    {
        if (!_agent.enabled)
            return;
        
        float dist = _agent.remainingDistance;
        if (!float.IsPositiveInfinity(dist) && _agent.pathStatus == NavMeshPathStatus.PathComplete &&
            _agent.remainingDistance <= 0.05f && _agent.hasPath)
        {
            _agent.ResetPath();
            _agent.updateRotation = false;
            _agent.isStopped = true;
            _agent.enabled = false;

            if (!hasSatDown)
            {
                _agent.Warp(table.chair.transform.position);
                table.chair.GetComponent<NavMeshObstacle>().enabled = true;
                transform.GetComponent<NavMeshObstacle>().enabled = false;
                indicatorTransform.gameObject.SetActive(true);
                table.ClearItems();
                RotateToTarget(table.transform);
                table.SpawnGhost(order);
                hasSatDown = true;
            }else if (!atPlateDispenser)
            {
                _agent.Warp(_plateDispenser.movePoint.position);
                atPlateDispenser = true;
                RotateToTarget(_plateDispenser.transform);
                Events.OnCustomerArrivedAtPlateDispenser(this);
                StartCoroutine(RemoveEmptyStuff());
            }

            Events.OnCustomerSatDown(this);
        }
    }

    private IEnumerator RemoveEmptyStuff()
    {
        yield return new WaitForSeconds(1f);
        _plateDispenser.AddDirtyPlate();
        if (emptyGlass != null)
            Destroy(emptyGlass.gameObject);
        if (emptyPlate != null)
            Destroy(emptyPlate.gameObject);
    }

    private void OnDrawGizmos()
    {
        if (table == null) 
            return;
        
        if (!_agent.enabled || !_agent.hasPath) 
            return;
        
        Gizmos.DrawLine(transform.position, _agent.path.corners[0]);
        
        for (int i = 1; i < _agent.path.corners.Length; i++)
        {
            var p = _agent.path.corners[i - 1];
            var p2 = _agent.path.corners[i];
            Gizmos.DrawLine(p, p2);
        }
    }

    public void RecalculatePath()
    {
        if (!_agent.enabled)
            return;
        
        _agent.SetDestination(table.chair.transform.position);
    }

    public void WalkToPlateDisplay(PlateDispenser plateDispenser)
    {
        _plateDispenser = plateDispenser;
        GiveEmptyPlateAndGlass();
        MoveToPlateDispenser();
    }
}
