using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Suck : MonoBehaviour
{
    public bool enableForce = false;

    private void FixedUpdate()
    {
        if (!enableForce) return;
        
        var rb = this.GetComponent<Rigidbody>();

        rb.velocity = Vector3.forward * 25 + Vector3.up * 5;
    }
}
