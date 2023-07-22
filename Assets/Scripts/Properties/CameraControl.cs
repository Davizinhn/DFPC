using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraControl
{
    public bool cameraControl = true;
    public float sensitivity = 2f;
    public float smooth = 1.5f;
    public Camera cam;
}
