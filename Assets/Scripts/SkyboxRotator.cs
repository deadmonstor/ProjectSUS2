using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SkyboxRotator : MonoBehaviour
{
    public float rotation;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    [SerializeField] private Material skyboxMat;

    private void Start()
    {
        RenderSettings.skybox = skyboxMat;
    }

    private void Update()
    {
        rotation += Time.deltaTime;
        if (rotation >= 360) rotation = 0;

        skyboxMat.SetFloat(Rotation, rotation);
    }
}
