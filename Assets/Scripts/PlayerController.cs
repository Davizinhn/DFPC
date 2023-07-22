using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // properties
    [Header("FPC Properties")]
    public CameraControl pCamera;
    public Walk walk;
    public Run run;
    public Jump jump;
    public Crouch crouch;
    
    // real stuff
    Rigidbody rb;
    float curSpeed;
    Vector2 v;
    Vector2 vC;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        curSpeed = walk.walkSpeed;
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        Look();
    }

    // walk and running
    void Movement()
    {
        Vector2 movementVelocity = new Vector2(Input.GetAxis("Horizontal") * curSpeed, Input.GetAxis("Vertical") * curSpeed);
        rb.velocity = transform.rotation * new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.y);
    }

    // camera stuff
    void Look()
    {
        Vector2 mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawVC = Vector2.Scale(mouse, Vector2.one * pCamera.sensitivity);
        vC = Vector2.Lerp(vC, rawVC, 1 / pCamera.smooth);
        v += vC;
        v.y = Mathf.Clamp(v.y, -90, 90);
        transform.localRotation = Quaternion.AngleAxis(-v.y, Vector3.right);
        this.transform.localRotation = Quaternion.AngleAxis(v.x, Vector3.up);
    }
}
