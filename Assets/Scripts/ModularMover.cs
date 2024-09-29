using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
public class ModularMover : MonoBehaviour
{
    [SerializeField] private bool isGrounded, isJumping, isAction, AttackReady, isDash, downAction, isClimbing, dashAvailiable, isSlipping;
    [SerializeField] private int contigencies;
    [FormerlySerializedAs("numMaxJumps")] [SerializeField] private int numMaxAirJumps;
    [FormerlySerializedAs("numRemainingJumps")] [SerializeField] private int numRemainingAirJumps;
    [SerializeField] private int inputX, smartness;
    private Animator _animator;
    public int Smartness
    {
        get => smartness;
        set => smartness = value;
    }

    [SerializeField] private Collider2D body, wallDetectL, wallDetectW, groundDetect;
    private Rigidbody2D rig;
    [SerializeField] private float speed, runSpeed, MaxWalk, MaxRun, jumpPower, attackCooldown, dashCooldown;
    private Vector2 input;
    private SpriteRenderer _spriteRenderer;
    
    //Scuffed Zone
    [SerializeField] private Transform LastGround;
    
    public bool DashAvailiable
    {
        get => dashAvailiable;
        set => dashAvailiable = value;
    }

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
            inputX = 1 *smartness;
        }
        else if (input.x < 0)
        {
            inputX = -1 *smartness;
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
        if (context.performed)
        {
            isAction = true;
        }

        if (context.canceled)
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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        dashAvailiable = false;
        isSlipping = false;
        smartness = 1;
        _animator = GetComponent<Animator>();
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
        jump();
        hVelocity();
        if (body.IsTouchingLayers(LayerMask.GetMask("Murder")))
        {
            if (contigencies == 0)
            {
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                contigencies--;
                rig.transform.position = LastGround.transform.position;
            }
        }
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

        if (inputX != 0 && isGrounded)
        {
            if (isAction || isDash)
            {
                _animator.SetBool("isRunning", true);
                _animator.SetBool("isWalking", false);
            }
            else
            {
                _animator.SetBool("isRunning", false);
                _animator.SetBool("isWalking", true);
            }
        }
        else if (!isGrounded)
        {
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isWalking", false);
            if (rig.velocity.y > 0)
            {
                _animator.SetBool("isJump",true);
            }
            else
            {
                _animator.SetBool("isFall", true);
                _animator.SetBool("isJump", false);
            }
        }
        if (isGrounded)
        {
            LastGround.transform.position = rig.transform.position;
            _animator.SetBool("isFall", false);
            _animator.SetBool("isJump",false);
        }
        if (inputX > 0)
        {
            _spriteRenderer.flipX = false;
            
        }
        else if (inputX < 0)
        {
            _spriteRenderer.flipX = true;
            
        }
        else
        {
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isRunning", false);
        }
        
        isDash = false;
    }

    private void jump()
    {
        if (wallDetectL.IsTouchingLayers(LayerMask.GetMask("Floor")) && (inputX == -1) && isJumping && !isGrounded)
        {
            rig.velocity += new Vector2(inputX * -1f * (jumpPower * 1.25f), jumpPower *0.6f);
        }
        else if (wallDetectW.IsTouchingLayers(LayerMask.GetMask("Floor")) && (inputX == 1) && isJumping && !isGrounded)
        {
            rig.velocity += new Vector2(inputX * -1f * jumpPower * 1.25f, jumpPower * 0.75f);
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

    public void slip()
    {
        if (!isSlipping)
        {
            rig.drag = 0f;
            rig.gravityScale = 6;
            isSlipping = true;
        }
    }
}
