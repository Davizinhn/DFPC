using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Crouch
{
    public bool canCrouch=true;
    bool isCrouched=false;
    public float crouchSpeed=2.5f;
    public float crouchY;
}
