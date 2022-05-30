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

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Warp(Transform target)
    {
        _agent.Warp(target.position);
    }
    
    public void MoveToTarget()
    {
        _agent.SetDestination(table.chair.transform.position);
    }

    public void RotateToTarget(Transform targetTransform)
    {
        Vector3 lookPos = targetTransform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1); 
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f); 
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
            _agent.Warp(table.chair.transform.position);
            table.chair.GetComponent<NavMeshObstacle>().enabled = true;
            transform.GetComponent<NavMeshObstacle>().enabled = false;
            _agent.enabled = false;
            RotateToTarget(table.transform);

            Events.OnCustomerSatDown(this);
        }
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
}
