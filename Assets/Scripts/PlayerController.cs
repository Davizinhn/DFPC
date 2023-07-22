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
        pCamera.cam.fieldOfView = Mathf.Lerp(pCamera.cam.fieldOfView, run.isRunning ? run.runFOV : walk.walkFOV, pCamera.fovChangeLerp);
    }

    void Update()
    {
        curSpeed = run.isRunning ? run.runSpeed : walk.walkSpeed;
        Jump();
        Look();
        CheckGround();
    }

    // walk and running
    void Movement()
    {
        Vector2 movementVelocity = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f) * curSpeed;
        walk.isWalking = movementVelocity != new Vector2(0, 0);
        rb.velocity = transform.rotation * new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.y); 
        run.isRunning = walk.isWalking && run.canRun && Input.GetKey(run.runKey) ? true : false;
    }

    // jump
    void Jump()
    {
        if(!jump.canJump || !jump.isGrounded)
            return;

        if(Input.GetKeyDown(jump.jumpKey))
        {
            rb.AddForce(Vector3.up*jump.jumpForce*100, ForceMode.Force);
        }
    }

    // ground check
    void CheckGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(jump.groundChecker.position, Vector3.down);
        jump.isGrounded = Physics.Raycast(ray, out hit, 0.1f, jump.groundLayer) ? true : false;
    }

    // camera stuff
    void Look()
    {
        Vector2 mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawVC = Vector2.Scale(mouse, Vector2.one * pCamera.sensitivity);
        vC = Vector2.Lerp(vC, rawVC, 1 / pCamera.smoothness);
        v += vC;
        v.y = Mathf.Clamp(v.y, -pCamera.maxMinRotationY, pCamera.maxMinRotationY);
        pCamera.cam.transform.localRotation = Quaternion.AngleAxis(-v.y, Vector3.right);
        this.transform.localRotation = Quaternion.AngleAxis(v.x, Vector3.up);
    }
}
