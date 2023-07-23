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
    public SoundEffects soundEffects;
    
    // real stuff
    Rigidbody rb;
    float curSpeed;
    Vector2 v;
    Vector2 vC;
    bool alreadydid = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        curSpeed = walk.walkSpeed;
    }

    void FixedUpdate()
    {
        Movement();
        rb.AddForce(Physics.gravity * jump.gravityScale, ForceMode.Force);
        pCamera.cam.fieldOfView = Mathf.Lerp(pCamera.cam.fieldOfView, run.isRunning ? run.runFOV : walk.walkFOV, pCamera.fovChangeLerp);
    }

    void Update()
    {
        if(!crouch.isCrouched){
            crouch.canCrouch = !run.isRunning && jump.isGrounded && crouch.crouchEnabled;
            curSpeed = run.isRunning ? run.runSpeed : walk.walkSpeed;
        } else {
            crouch.canCrouch = !run.isRunning && crouch.crouchEnabled;
            curSpeed = crouch.crouchSpeed;
        }
        crouch.isCrouched = Input.GetKey(crouch.crouchKey) && crouch.canCrouch;
        Jump();
        Crouch();
        Look();
        CheckGround();
    }

    // walk and running
    void Movement()
    {
        if(walk.isWalking && !soundEffects.walkAndrun.isPlaying && jump.isGrounded)
        {
            soundEffects.walkAndrun.Play();
        }
        else if(!walk.isWalking && soundEffects.walkAndrun.isPlaying)
        {
            soundEffects.walkAndrun.Stop();
        }
        else if(soundEffects.walkAndrun.isPlaying && !jump.isGrounded)
        {
            soundEffects.walkAndrun.Stop();
        }
        soundEffects.walkAndrun.pitch = run.isRunning ? 1.2f : 1f;
        Vector2 movementVelocity = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f) * curSpeed;
        walk.isWalking = movementVelocity != new Vector2(0, 0);
        rb.velocity = transform.rotation * new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.y); 
        run.isRunning = !crouch.isCrouched && walk.isWalking && run.canRun && Input.GetKey(run.runKey) ? true : false;
    }

    // crouching
    void Crouch()
    {
        if(crouch.isCrouched && transform.localScale.y!=0.75f)
        {
            soundEffects.crouchS.PlayOneShot(soundEffects.crouch[Random.Range(0, soundEffects.crouch.Length-1)]);
            transform.localScale = new Vector3(transform.localScale.x, 0.75f, transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y-0.25f, transform.localPosition.z);
        }
        else if(!crouch.isCrouched && transform.localScale.y!=1f)
        {
            soundEffects.LcrouchS.PlayOneShot(soundEffects.Lcrouch[Random.Range(0, soundEffects.Lcrouch.Length-1)]);
            transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
        }
    }

    // jump
    void Jump()
    {
        if(!jump.canJump || !jump.isGrounded || crouch.isCrouched)
            return;

        if(Input.GetKeyDown(jump.jumpKey))
        {
            rb.AddForce(Vector3.up*jump.jumpForce*100, ForceMode.Force);
            soundEffects.jumpS.PlayOneShot(soundEffects.jump[Random.Range(0, soundEffects.jump.Length-1)]);
            alreadydid=false;
        }
    }

    // ground check
    void CheckGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(jump.groundChecker.position, Vector3.down);
        jump.isGrounded = Physics.Raycast(ray, out hit, 0.1f, jump.groundLayer) ? true : false;
        if(!alreadydid && jump.isGrounded && !soundEffects.jumpS.isPlaying)
        {
            alreadydid=true;
            soundEffects.landS.PlayOneShot(soundEffects.land[Random.Range(0, soundEffects.land.Length-1)]);
        }
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
