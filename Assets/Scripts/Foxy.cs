using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class Foxy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private float jumpforce = 12f;

    private int amountOfJumpsLeft;
    public int amountOfJumps = 1;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canJump;

    public float movementSpeed = 10.0f;

    public float groundCheckRadius = 0.35f;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public Transform groundCheck;
    public Transform wallCheck;


    [SerializeField] private LayerMask Ground;

    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource jem;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource fallsound;
    [SerializeField] private AudioSource oops;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryt;
    [SerializeField] private int health = 5;
    [SerializeField] private Text healtam;
    [SerializeField] public int score = 0;
    public int hscore = 0;
    [SerializeField] private Text scam;


    private enum State {idle, running, jumping, falling, hurt}
    private State state = State.idle;

    private float hurtForce = 10f;

    private void Awake()
    {
        hscore= PlayerPrefs.GetInt("TotalScore", 0);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
        healtam.text = health.ToString();
        if (score <= PlayerPrefs.GetInt("TotalScore", 0))
        {
            scam.text = PlayerPrefs.GetInt("TotalScore", 0).ToString();
            score = PlayerPrefs.GetInt("TotalScore", 0);
        }
    //scam.text = score.ToString();
    }
    void Update()
    {

            CheckIfCanJump();
        if (Input.GetButtonDown("Jump") /*&& coll.IsTouchingLayers(Ground)*/)
        {        

            Jump2();
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            Flip();
        }

        CheckIfWallSliding();
        StateSwitch();        
        anim.SetInteger("state", (int)state);
    }

    void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, Ground);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, Ground);
    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && (state == State.idle || state == State.running)) || isWallSliding)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

    }

    private void ApplyMovement()
    {
        if (isGrounded && state != State.hurt)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 10f, rb.velocity.y);
        }
        else if (!isGrounded && !isWallSliding && Input.GetAxisRaw("Horizontal") != 0)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * Input.GetAxisRaw("Horizontal"), 0);
            rb.AddForce(forceToAdd);

            if (Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
            }
        }
        else if (!isGrounded && !isWallSliding && Input.GetAxisRaw("Horizontal") == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }

        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            cherries++;
            cherryt.text = cherries.ToString();
            score += 100;
            PlayerPrefs.SetInt("TotalScore", score);
            scam.text = score.ToString();
        }
        if (collision.tag == "Powerup")
        {
            jem.Play();
            jumpforce = 18f;
            Destroy(collision.gameObject);
            score += 200;
            PlayerPrefs.SetInt("TotalScore", score);
            scam.text = score.ToString();
        }
        if (collision.tag == "JumpStopper")
        {
            jumpforce = 12f;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D enem)
    {
        if (enem.gameObject.tag == "Enemy")
        {
            Enemy enemy = enem.gameObject.GetComponent<Enemy>();
            
            if (state == State.falling)
            {
                enemy.Jumpon();
                Jump();
                score += 500;
                PlayerPrefs.SetInt("TotalScore", score);
                scam.text = score.ToString();
            }
            else
            {
                oops.Play();
                state = State.hurt;
                health -= 1;
                healtam.text = health.ToString();
                if (health <= 0)
                {
                    PlayerPrefs.DeleteKey("TotalScore");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                if (enem.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
       

    }
    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
        state = State.jumping;
    }

    private void Jump2()
    {
        if (canJump && !isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            amountOfJumpsLeft--;
            state = State.jumping;
        }
        else if (isWallSliding && Input.GetAxisRaw("Horizontal") == 0 && canJump) //Wall hop
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            state = State.jumping;
        }
        else if ((isWallSliding || isTouchingWall) && Input.GetAxisRaw("Horizontal") != 0 && canJump)
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * Input.GetAxisRaw("Horizontal"), wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            state = State.jumping;
        }
    }
    private void StateSwitch()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f) state = State.falling;
        }
        else if (state == State.falling)
        {
            if (isGrounded)
            {
                fallsound.Play();
                state = State.idle;
                
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f) state = State.idle;
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else if (state == State.idle && !isGrounded && !isTouchingWall)
        {
            state = State.falling;
        }

        else state = State.idle;

        while (state == State.running && (!isGrounded && !isTouchingWall) && state != State.jumping)
        {
            state = State.falling;
        }
    }
    private void FootStep()
    {
        footstep.Play();
    }
    private void falls()
    {
        fallsound.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

}
