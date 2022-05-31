using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    [SerializeField] private Transform _transformLooking;
    private Camera _cameraToLookAt;

    private void Start()
    {
        _cameraToLookAt = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 v = _cameraToLookAt.transform.position - _transformLooking.position;
        v.x = 0.0f; 
        v.z = 0.0f;
        _transformLooking.LookAt( _cameraToLookAt.transform.position - v ); 
        _transformLooking.Rotate(0,180,0);
    }
}
