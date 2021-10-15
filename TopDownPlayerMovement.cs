using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(5f, 2f);          //change this to make player faster. public = visible on component screen
    Vector2 targetPosition, relativePosition, movement;  //keep track of where player is/should be/is going in form (x,y)
    int coinsCollected = 0, keyNum = 0, hp = 3;          //numbers of coins and keys found, and hit points. have default values
    bool canInput = true, torch = false;                 //can the player move now? do they have the torch?

    // Update is called once per frame
    void Update()
    {
        if (canInput) //if you can move
        {
            if (Input.GetButtonDown("Fire1")) //if you clicked
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //player should be is where the mouse is on the camera screen
            }
            if (Input.GetAxis("Horizontal") < 0) //left input button
            {
                targetPosition.x = gameObject.transform.position.x - (60 * speed.x * Time.deltaTime); //player should be left
            }
            if (Input.GetAxis("Horizontal") > 0) //right input button
            {
                targetPosition.x = gameObject.transform.position.x + (60 * speed.x * Time.deltaTime); //player should be right
            }
            if (Input.GetAxis("Vertical") < 0) //down input button
            {
                targetPosition.y = gameObject.transform.position.y - (60 * speed.y * Time.deltaTime); //player should be down
            }
            if (Input.GetAxis("Vertical") > 0) //up input button
            {
                targetPosition.y = gameObject.transform.position.y + (60 * speed.y * Time.deltaTime); //player should be up
            }
        }

        //set our relative position as where we should be minus where we are
        relativePosition = new Vector2 
            (targetPosition.x - gameObject.transform.position.x,
             targetPosition.y - gameObject.transform.position.y);
    }
    
    //called once, many, or none times a frame
    void FixedUpdate()
    {
        if (hp <= 0) //if we are dead
        {
            canInput = false; //stop the input
            transform.position = GameObject.FindWithTag("Respawn").transform.position; //go to the nearest respawn point
            hp = 3; //RESET THE HEALTH
            canInput = true; //RESET INPUT!!!!
        }
        else
        {
            if (speed.x * Time.deltaTime >= Mathf.Abs(relativePosition.x)) //if we're about to overshoot
            {
                movement.x = relativePosition.x; //just go where we need to go
            }
            else //otherwise
            {
                movement.x = speed.x * Mathf.Sign(relativePosition.x); //move one distance unit away
            }

            //same thing but in y direction
            if (speed.y * Time.deltaTime >= Mathf.Abs(relativePosition.y))
            {
                movement.y = relativePosition.y;
            }
            else
            {
                movement.y = speed.y * Mathf.Sign(relativePosition.y);
            }

            //set our movement to the calculated movement
            GetComponent<Rigidbody2D>().velocity = movement;
        }
    }

    //oh no! we hit a TRIGGER collider
    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("hit something"); //say we hit something
        if (other.gameObject.CompareTag("Coin")) //if that was tagged coin
        {
            coinsCollected += 1; //increase the coin number
            Debug.Log("coinsCollected" + coinsCollected); //say we got a coin
            Destroy(other.gameObject); //get rid of the coin
        }

        //this is the camera stuff. follows this format
        //if i hit this edge, stop player input. move the camera in
        //the right direction. turn player input back on.
        if (other.gameObject.CompareTag("Left Edge")) 
        {
            canInput = false;

            Camera.main.transform.Translate(-19.5f,0f,0f);

            canInput = true;
        }
        if (other.gameObject.CompareTag("Right Edge"))
        {
            canInput = false;

            Camera.main.transform.Translate(19.5f, 0f, 0f);

            canInput = true;
        }
        if (other.gameObject.CompareTag("Down Edge"))
        {
            canInput = false;

            Camera.main.transform.Translate(0f, -8.5f, 0f);

            canInput = true;
        }
        if (other.gameObject.CompareTag("Up Edge"))
        {
            canInput = false;

            Camera.main.transform.Translate(0f, 8.5f, 0f);

            canInput = true;
        }

        if (other.gameObject.CompareTag("Torch")) //if it was a torch
        {
            torch = true; //we got the torch
            Destroy(other.gameObject); //no duplicate torches, delete the one we got
            Debug.Log("got torch"); //say we got a torch
        }

        if (other.gameObject.CompareTag("Key")) //if it was a key
        {
            keyNum++; //increase the key num
            Destroy(other.gameObject); //destroy the key. no duplicates
            Debug.Log("key num = " + keyNum); //say we got a key
        }
    }

    //oh no! we hit a STANDARD collider thing
    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Door") && keyNum > 0) //if we hit a door and we have a key
        {
            Destroy(other.gameObject); //destroy the door
            keyNum--; //remove a key
            Debug.Log("opened door"); //say what happened
        }
        else if (other.gameObject.CompareTag("Ow")) //if we hit an enemy/spikepit
        {
            //find the script that the enemy and call the getter method
            //to tell us how much to reduce health by
            hp -= other.gameObject.GetComponent<EnemyScript>().GetHurt();
            Debug.Log("hp = " + hp); //print out missing health
        }
    }

    //getter method for the torch bool
    public bool GetTorch()
    {
        return torch;
    }
}
