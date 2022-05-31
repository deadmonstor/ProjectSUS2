using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SkyboxRotator : MonoBehaviour
{
    public float rotation;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");

    private void Update()
    {
        rotation += Time.deltaTime;
        if (rotation >= 360) rotation = 0;

        RenderSettings.skybox.SetFloat(Rotation, rotation);

    }
}
