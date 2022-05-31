using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] private Transform bobbingTransform;
    public float speed;
    public float height = 0.5f;
    
    private float _startY;

    private void Awake()
    {
        _startY = bobbingTransform.transform.position.y;
    }

    private void Update()
    {
        Bob();
    }

    private void Bob()
    {
        var pos = bobbingTransform.transform.position;
        pos.y = _startY + (Mathf.Sin(Time.time * speed) * height);
        bobbingTransform.transform.position = pos;
    }
}
