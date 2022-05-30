using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Suck : MonoBehaviour
{
    [HideInInspector]
    public bool enableForce = false;

    private void FixedUpdate()
    {
        if (!enableForce) return;
        
        var rb = this.GetComponent<Rigidbody>();

        Vector3 target = new Vector3(0, 3, 50005);
        Vector3 dir = target - transform.position;
        dir.Normalize();
        dir *= Random.Range(0.5f, 2.0f);
        rb.AddForce(dir * 30.0f, ForceMode.Impulse);
    }
}
