using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;


public class PlayerController : MonoBehaviour
{
    [Header("Player movement")]
    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    private bool facingRight = true;
    private Rigidbody2D player;

    [Header("Is grounded")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool touchingGround;

    [Header("Wall jump")]
    public float wallJumpTime = 0.2f;
    public float slideSpeed = 0.3f;
    public float wallDistance = 0.5f;
    private bool isWallSliding = false;
    RaycastHit2D wallCheck;
    private float jumpTime;

    // Double jump
    private bool doubleJump;

    //link animator component to player
    private Animator playerAnimation;

    //this is for respawn after falling
    private Vector3 respawnPoint;
    public GameObject fallDetector;

    //maxPlatform is for leaderboard things
    public PlayFabManager playfabManager;
    private int maxPlatform = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        playfabManager = GetComponent<PlayFabManager>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        touchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // if radius of groundCheck overlaps with an obj with a ground layer, then true

        // ------- Switch player direction --------
        direction = Input.GetAxis("Horizontal"); 
        // Debug.Log(direction);
        
        if(direction > 0f)
        {
            facingRight = true;
            transform.localScale = new Vector2(0.4114183f,0.4114183f);              // player is facing right   
            //transform.localScale = new Vector2(0.4114183f, 0.4114183f);
        } else if (direction < 0f)
        {
            facingRight = false;
            transform.localScale = new Vector2(-0.4114183f,0.4114183f);             // player is facing left
            //transform.localScale = new Vector2(-0.4114183f, 0.4114183f);
        } else
        {
            player.velocity = new Vector2(0, player.velocity.y);                    // player faces last direction faced
        }

        // -------- Move ---------
        player.velocity = new Vector2(direction * speed, player.velocity.y);

        //-------- Jump ----------

        if (Input.GetButtonDown("Jump"))
        {
            if(touchingGround || isWallSliding || doubleJump)
            {
                Jump();
                doubleJump = ! doubleJump;
            }
        }

        // --------- Wall stuff ---------
        if (facingRight)
        {
                                        // Vector2 origin         // Vector2 direction    // float distance   //layer mask
            wallCheck = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
            // Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.black);
        } else
        {
            wallCheck = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
            // Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.black);
        }

        if (wallCheck && !touchingGround && direction != 0) // player must have all three, raycast hit a ground layer, not be grounded, and pressing the L/R key
        {
            isWallSliding = true;
            // jumpTime = Time.time + wallJumpTime;  // give time for player to jump if they accidentally let go of L/R key too early
        } else  // add this code as else if for jump time delay -->>>> (jumpTime < Time.time)
        {
            isWallSliding = false;
        }

        if(isWallSliding)
        {
            player.velocity = new Vector2(player.velocity.x, Mathf.Clamp(player.velocity.y, slideSpeed, float.MaxValue));
        }
        // -----End wall stuff -------

        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("OnGround", touchingGround);
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if(collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        else if(collision.tag =="NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position;
            maxPlatform++;
        }
        else if(collision.tag =="PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            respawnPoint = transform.position;
            maxPlatform--;
        }
        else if(collision.tag == "GameOver")
        {
            this.GameOver();
        }
    }
    public void GameOver(){
        playfabManager.SendLeaderboard(maxPlatform);
    }

    public void Jump()
    {
        player.velocity = new Vector2(player.velocity.x, jumpSpeed);
    }
}
