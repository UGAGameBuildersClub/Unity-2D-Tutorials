using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlatformerMovementScript : MonoBehaviour
{
    //numbers related to movement such as speed
    public Vector2 speed = new Vector2(3f,6f);
    public float gravityScale = 1.5f;
    public float maxJumpTime = .75f;

    //component for animations
    public Animator anim;

    //UI component of lives text
    public Text livesNum;
    public Text coinsNum;

    //respawn transform
    public GameObject respawn;

    //for UI
    int coinsCollected = 0;

    //used to keep track of movement
    bool isGrounded;
    bool isFalling = true;
    Vector2 movement, moveDirection;
    float jumpTime;

    //number of lives
    int lives = 3;

    //Update is called once per frame
    void Update()
    {
        //moving left
        if (Input.GetAxis("Horizontal") < 0)
        {
            //movement is negative
            moveDirection.x = -1;
            //start the running animation if need be
            anim.SetBool("isRunning", true);
            //have the sprite rotated to face left
            transform.eulerAngles = new Vector3(0,0,0);
        }
        //moving right
        else if (Input.GetAxis("Horizontal") > 0)
        {
            //movement is positive
            moveDirection.x = 1;
            //start the running animation if need be
            anim.SetBool("isRunning",true);
            //have the sprite rotated to face right
            transform.eulerAngles = new Vector3(0,180,0);
        }
        //not moving
        else 
        {
            //movement is zero
            moveDirection.x = 0;
            //do not play the running animation
            anim.SetBool("isRunning",false);
        }

        //move in the x direction with the speed and direction specified
        movement.x = moveDirection.x * speed.x;


        // on the ground (ie able to jump) and recieved input to jump
        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            //play the jump animation
            anim.SetBool("isJumping", true);
            //reset everything for the jump method
            jumpTime = 0;
            isGrounded = false;
            isFalling = false;
        }

        //call the jump method to figure out y direction movement
        Jump();

        //move in the y direction with speed and direction specified
        movement.y = moveDirection.y * speed.y;

        if (lives == 0) 
        {
            transform.position = respawn.transform.position;
            lives = 3;
            livesNum.text = "" + lives;
        }
    }

    void Jump() 
    {
        //have we reached the max height of our jump?
        if (jumpTime == maxJumpTime) 
        {
            //then start falling
            isFalling = true;
        }

        //not on the ground
        if (!isGrounded)
        {
            //not falling
            if (!isFalling)
            {
                //velocity = acceleration * delta time
                moveDirection.y = gravityScale * (maxJumpTime - jumpTime);
                //update delta time
                jumpTime += Time.deltaTime;
            }
            //falling
            else
            {
                //velocity = acceleration * delta time
                moveDirection.y = -gravityScale * (maxJumpTime - jumpTime);
                //update delta time
                jumpTime -= Time.deltaTime;
            }
        }
        //on the ground
        else 
        {
            //don't move in this direction
            moveDirection.y = 0;
        }
    }

    //called zero or more times per frame
    void FixedUpdate() 
    {
        //change the rigidbody component's velocity to the expected movement vector
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    //bumped into something weird
    void OnTriggerEnter2D(Collider2D other) 
    {
        //it was a Coin
        if (other.gameObject.CompareTag("Coin")) 
        {
            coinsCollected += 1;
            Debug.Log("Coins Collect: " + coinsCollected);
            Destroy(other.gameObject);
            coinsNum.text = "" + coinsCollected;
        }

        //it was a portal
        if (other.gameObject.CompareTag("Portal"))
        {
            Debug.Log("Scene Change");
            //determine if the good ending was earned
            if (coinsCollected == 3)
            {
                SceneManager.LoadScene("GoodEnd");
            }
            else
            {
                SceneManager.LoadScene("End");
            }
        }

        if (other.gameObject.CompareTag("Death")) 
        {
            lives -= 1;
            livesNum.text = "" + lives;
        }
    }

    //stopped hitting the ground
    void OnCollisionExit2D(Collision2D other) 
    {
        isGrounded = false;
    }

    //bumped into a wall or the ground
    void OnCollisionEnter2D(Collision2D other) 
    {
        //it was the ground
        if (other.gameObject.CompareTag("Platform")) 
        {
            isGrounded = true;
            jumpTime = 0;
            anim.SetBool("isJumping",false);
        }
    }
}
