using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ModularMover : MonoBehaviour
{
    [SerializeField] private bool isGrounded, isJumping, isAction, AttackReady, isDash, downAction, isClimbing, dashAvailiable;
    [SerializeField] private int contigencies;
    [FormerlySerializedAs("numMaxJumps")] [SerializeField] private int numMaxAirJumps;
    [FormerlySerializedAs("numRemainingJumps")] [SerializeField] private int numRemainingAirJumps;
    [SerializeField] private int inputX;
    [SerializeField] private Collider2D body, wallDetectL, wallDetectW, groundDetect;
    private Rigidbody2D rig;
    [SerializeField] private float speed, runSpeed, MaxWalk, MaxRun, jumpPower, attackCooldown, dashCooldown;
    private Vector2 input;
    public int Contigencies
    {
        get => contigencies;
        set => contigencies = value;
    }

    public int NumMaxJumps
    {
        get => numMaxAirJumps;
        set => numMaxAirJumps = value;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float RunSpeed
    {
        get => runSpeed;
        set => runSpeed = value;
    }

    public float MaxWalk1
    {
        get => MaxWalk;
        set => MaxWalk = value;
    }

    public float MaxRun1
    {
        get => MaxRun;
        set => MaxRun = value;
    }

    public float JumpPower
    {
        get => jumpPower;
        set => jumpPower = value;
    }

    public float AttackCooldown
    {
        get => attackCooldown;
        set => attackCooldown = value;
    }

    //All movement except Jump
    public void movementDetect(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        if (input.x > 0)
        {
            inputX = 1;
        }
        else if (input.x < 0)
        {
            inputX = -1;
        }
        else
        {
            inputX = 0;
        }

        if (input.y < -1)
        {
            downAction = true;
        }
        else
        {
            downAction = false;
        }
    }
    public void jumpDetect(InputAction.CallbackContext context)
    {
        float inputj = context.ReadValue<float>();
        if (inputj > 0)
        {
            isJumping = true;
        }
    }

    public void dashDetect(InputAction.CallbackContext context)
    {
        float inputD = context.ReadValue<float>();
        if (inputD > 0 && dashAvailiable)
        {
            isDash = true;
        }
    }
    
    public void runDetect(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        if (input.y > 0)
        {
            isAction = true;
        }
        else
        {
            isAction = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        numMaxAirJumps = 0;
        numRemainingAirJumps = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (groundDetect.IsTouchingLayers(LayerMask.GetMask("Floor")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    IEnumerator dashRecovered()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashAvailiable = true;
    }
    private void FixedUpdate()
    {
        hVelocity();
        jump();
    }

    private void hVelocity()
    {
        if (isDash && dashAvailiable)
        {
            rig.velocity += new Vector2(inputX * (RunSpeed + speed) * 10, rig.velocity.y) * Time.deltaTime;
            dashAvailiable = false;
            StartCoroutine(dashRecovered());
        }
        else if (isAction)
        {
            if (MaxRun > Math.Abs(rig.velocity.x))
            {
                rig.velocity += new Vector2(inputX * runSpeed, rig.velocity.y) * Time.deltaTime;
            }
        }
        else
        {
            if (MaxWalk > Math.Abs(rig.velocity.x))
            {
                rig.velocity += new Vector2(inputX * speed, rig.velocity.y) * Time.deltaTime;
            }
        }
        isDash = false;
    }

    private void jump()
    {
        if (wallDetectL.IsTouchingLayers(LayerMask.GetMask("Floor")) && (inputX == -1) && isJumping && !isGrounded)
        {
            rig.velocity += new Vector2(inputX * -1f * (jumpPower / 2), jumpPower / 2);
        }
        else if (wallDetectW.IsTouchingLayers(LayerMask.GetMask("Floor")) && (inputX == 1) && isJumping && !isGrounded)
        {
            rig.velocity += new Vector2(inputX * -1f * (jumpPower / 2), jumpPower / 2);
        }
        else
        {
            if (isJumping && (numRemainingAirJumps > 0 || isGrounded))
            {
                rig.velocity = new Vector2(rig.velocity.x, jumpPower);
                if (!isGrounded)
                {
                    numRemainingAirJumps--;
                }
            }
            else if (isGrounded && numRemainingAirJumps < numMaxAirJumps)
            {
                numRemainingAirJumps++;
            }
        }
        isJumping = false;
    }
}
