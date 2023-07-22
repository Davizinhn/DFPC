using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Jump
{
    public bool canJump=true;
    bool isGrounded=false;
    public float jumpForce=2f;
}
