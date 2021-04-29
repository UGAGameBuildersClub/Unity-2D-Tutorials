using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformerPlayerMovement : MonoBehaviour
{
    // Move player in 2D space
    public Vector2 speed = new Vector2(3f,6f);
    public float gravityScale = 1.5f;
    public float maxJumpTime = .2f;

    int coinsCollected = 0;
    bool isGrounded;
    bool isFalling = true;
    Vector2 movement,moveDirection;
    float jumpTime;

    // Update is called once per frame
    void Update()
    {
        //Horizontal Movement
        if (Input.GetAxis("Horizontal") < 0)
        {
            moveDirection.x = -1;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            moveDirection.x = 1;
        }
        else 
        {
            moveDirection.x = 0;
        }
        movement.x = moveDirection.x * speed.x;

        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            jumpTime = 0;
            isGrounded = false;
            isFalling = false;
        }
        Jump();

        movement.y = moveDirection.y * speed.y;
    }

    void Jump() 
    {
        if (jumpTime == maxJumpTime) //reached max jump height
        {
            isFalling = true;
        }

        if (!isGrounded)
        {
            if (!isFalling)
            {
                moveDirection.y = gravityScale * (maxJumpTime - jumpTime);
                jumpTime += Time.deltaTime;
            }
            else //isGrounded = false and isFalling = true
            {
                moveDirection.y = -gravityScale * (maxJumpTime - jumpTime);
                jumpTime -= Time.deltaTime;
            }
        }
        else //isGrounded = true
        {
            moveDirection.y = 0;
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        // Coin
        if (other.gameObject.CompareTag("Coin")) 
        {
            coinsCollected += 1;
            Debug.Log("Coins Collected: " + coinsCollected);
        }

        if (other.gameObject.CompareTag("Finish")) 
        {
            Debug.Log("Changing Scene");
            SceneManager.LoadScene("GoodEnd");
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        // On Ground
        if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            jumpTime = 0;
        }
    }
}
