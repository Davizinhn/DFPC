using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraControl
{
    public bool cameraControl = true;
    public float sensitivity = 2f;
    public float smoothness = 1.5f;
    public int maxMinRotationY = 85;
    public float fovChangeLerp = 0.1f;
    public Camera cam;
}
